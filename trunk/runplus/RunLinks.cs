using System;

using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace runplus
{
    class RunLinks
    {
        private Hashtable ht = new Hashtable();
        // 看了文章后，自己看看2.0 内泛型的支持
        private Dictionary<string, RunLink> ht1 = new Dictionary<string, RunLink>();
        private List<string> ht2 = new List<string>();
        
        public Hashtable HT
        {
            get { return ht; }
        }
        private void GetStartMenu1()
        {
            string wild = "*.lnk";
            SearchOption so = SearchOption.AllDirectories;
            string currdir = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
            GetFiles(wild, so, currdir);            
        }

        private void GetFiles(string wild, SearchOption so, string currdir)
        {
            DirectoryInfo di = new DirectoryInfo(currdir);
            FileInfo[] rgFiles = di.GetFiles(wild, so);
            foreach (FileInfo fi in rgFiles)
            {
                string fullfilename = fi.DirectoryName + "\\" + fi.Name;
                string filename = fi.Name.ToLower();
                string key = filename;
                RunLink runlink = new RunLink(filename, fullfilename);
                if (!ht.Contains(key))
                    ht.Add(key, runlink);
            }
            
        }
        private void GetStartMenu2()
        {
            string currdir = ShellHelper.GetStartMenu();
            string wild = "*.lnk";
            SearchOption so = SearchOption.AllDirectories;
            GetFiles(wild, so, currdir);
        }
        private void GetSystem32()
        {
            string currdir = Environment.SystemDirectory;
            string wild = "*.exe";
            SearchOption so = SearchOption.TopDirectoryOnly;
            GetFiles(wild, so, currdir);
        }
        private void GetCurrent()
        {
            string currdir = Environment.CurrentDirectory;
            string wild = "*.exe";
            SearchOption so = SearchOption.TopDirectoryOnly;
            GetFiles(wild, so, currdir);
        }
        internal void Populate()
        {
            HT.Clear();            
            GetStartMenu1();
            GetStartMenu2();
            GetSystem32();
            GetCurrent();
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Environment.CurrentDirectory;
            watcher.IncludeSubdirectories = true;
            watcher.Filter = "*.exe";
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = //NotifyFilters.LastAccess | NotifyFilters.LastWrite|
                NotifyFilters.FileName | NotifyFilters.DirectoryName;
            
            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Begin watching.
            watcher.EnableRaisingEvents = true;

        }
        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            //MessageBox.Show("change" + e.FullPath+e.Name+e.ChangeType.ToString());
            Populate();
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            Populate();
        }


        internal void AddKeyWord(string keyword,  RunLink link)
        {
            KeyRunLink keylink = new KeyRunLink(keyword,link.FileName,link.FullFileName); 
            string filename = link.FileName;
            ht[filename] = keylink;
        }
    }
    class RunLink
    {
        private string fileName = "";

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        private string fullFileName = "";

        public string FullFileName
        {
            get { return fullFileName; }
            set { fullFileName = value; }
        }
        public RunLink(string filename,string fullfilename)
        {
            fileName = filename;
            fullFileName = fullfilename;
        }
    }
    class KeyRunLink : RunLink
    {
        private string keyName = "";

        public string KeyName
        {
            get { return keyName; }
            set { keyName = value; }
        }
        public KeyRunLink(string keyname,string filename, string fullfilename):base(filename,fullfilename)
        {
            keyName = keyname;
        }
    }
}
