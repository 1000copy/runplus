﻿using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
namespace runplus
{
     static class Hotkey
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool RegisterHotKey(IntPtr hWnd, int id, HotkeyModifiers fsModifiers, Keys vk);

        [DllImport("user32.dll")]
        static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        /// <summary> 
        /// 注册快捷键 
        /// </summary> 
        /// <param name="hWnd">持有快捷键窗口的句柄</param> 
        /// <param name="fsModifiers">组合键</param> 
        /// <param name="vk">快捷键的虚拟键码</param> 
        /// <param name="callBack">回调函数</param> 
        public static void Register(IntPtr hWnd, HotkeyModifiers fsModifiers, Keys vk, HotKeyCallBackHanlder callBack)
        {
            int id = keyid++;
            if (!RegisterHotKey(hWnd, id, fsModifiers, vk))
                throw new Exception("regist hotkey fail.");
            keymap[id] = callBack;
        }

        /// <summary> 
        /// 注销快捷键 
        /// </summary> 
        /// <param name="hWnd">持有快捷键窗口的句柄</param> 
        /// <param name="callBack">回调函数</param> 
        public static void UnRegist(IntPtr hWnd, HotKeyCallBackHanlder callBack)
        {
            foreach (KeyValuePair<int, HotKeyCallBackHanlder> var in keymap)
            {
                if (var.Value == callBack)
                    UnregisterHotKey(hWnd, var.Key);
            }
        }

        /// <summary> 
        /// 快捷键消息处理 
        /// </summary> 
        public static void ProcessHotKey(System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                int id = m.WParam.ToInt32();
                HotKeyCallBackHanlder callback;
                if (keymap.TryGetValue(id, out callback))
                {
                    callback();
                }
            }
        }

        const int WM_HOTKEY = 0x312;
        static int keyid = 10;
        static Dictionary<int, HotKeyCallBackHanlder> keymap = new Dictionary<int, HotKeyCallBackHanlder>();

        public delegate void HotKeyCallBackHanlder();
    }

    enum HotkeyModifiers
    {
        MOD_ALT = 0x1,
        MOD_CONTROL = 0x2,
        MOD_SHIFT = 0x4,
        MOD_WIN = 0x8
    }

    class ShellHelper
    {
        [DllImport("USER32.DLL")]
        public static extern void SwitchToThisWindow(IntPtr hwnd, Boolean fAltTab);   
        [DllImport("shfolder.dll", CharSet = CharSet.Auto)]
        internal static extern int SHGetFolderPath(IntPtr hwndOwner,
        int nFolder, IntPtr hToken, int dwFlags, StringBuilder
        lpszPath);
        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0;
        private const uint SHGFI_SMALLICON = 0x1;

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        [DllImport("Shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, uint uFlags);

        internal static string GetStartMenu()
        {
            StringBuilder sbPath = new StringBuilder(260);
            SHGetFolderPath(IntPtr.Zero, 0x17, IntPtr.Zero, 0, sbPath);
            return sbPath.ToString();
        }

        internal static IntPtr GetIcon(string Path2Link)
        {
            SHFILEINFO shinfo = new SHFILEINFO();
            SHGetFileInfo(Path2Link, 0, ref shinfo, Marshal.SizeOf(shinfo), SHGFI_ICON);
            return shinfo.hIcon;
        }
    }
}
