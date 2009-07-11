using System;

using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace runplus
{
    class LinkSet
    {
        
        private Dictionary<string, Link> links = new Dictionary<string, Link>();

        private Dictionary<string, Link> Links
        {
            get { return links; }
            set { links = value; }
        }
        string confName = "runplus.txt";
        ~LinkSet()
        {
            SaveKeys();
        }
        void SaveKeys()
        {
            string conf = AppDomain.CurrentDomain.BaseDirectory + confName;            
            string text ="";
            foreach (KeyValuePair<string, Link> keyValue in Links)
            {
                if (keyValue.Value is KeyLink)
                {
                    text += (keyValue.Value as KeyLink).KeyName + ";" + keyValue.Value.FullFileName + "\r\n";
                }
            }            
            // File.WriteAllText(conf,text)
            // 明确指明为utf8，这样sc可以识别，不会默认为codepage，表现出乱码。
            File.WriteAllText(conf,text,Encoding.UTF8);
            
        }
        void LoadKeys()
        {
            string conf = AppDomain.CurrentDomain.BaseDirectory + confName;
            if (File.Exists(conf))
            {
                StreamReader reader = File.OpenText(conf);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string []  keys = line.Split(';');
                    if (keys.Length ==2)
                        AddKeyWord(keys[0],keys[1]);
                }


            }
        }
        internal void Fill(FileInfo[] rgFiles)
        {
            foreach (FileInfo fi in rgFiles)
            {
                string fullfilename = fi.DirectoryName + "\\" + fi.Name;
                string filename = fi.Name.ToLower();
                string key = filename;
                Link runlink = new Link(filename, fullfilename);
                if (!Links.ContainsKey(key))
                    Links.Add(key, runlink);
                else
                    links[key] = runlink;
            }
        }
        List<LinkAdapter> adapters = new List<LinkAdapter>();
        internal void Init()
        {
            Links.Clear();            
            adapters.Add(new StartMenuLinkAdapter(this));    
            adapters.Add(new SystemLinkAdapter(this));
            adapters.Add(new CurrentDirLinkAdapter(this));
            adapters.Add(new ProgramLinkAdapter(this));
            LoadKeys();
        }
      
        internal void AddKeyWord(string keyword,  Link link)
        {
            KeyLink keylink = new KeyLink(keyword,link.FileName,link.FullFileName); 
            string filename = link.FileName;
            Links[filename] = keylink;
        }
        internal void AddKeyWord(string keyword, string filename)
        {
            string name = new FileInfo(filename).Name;            
            Links[name] = new KeyLink(keyword, name, filename);            
        }


        internal Dictionary<string, Link> MatchLinks(string query)
        {
            return MatchLinks(query,Links);
        }
        private Dictionary<string, Link> MatchLinks(string query, Dictionary<string, Link> ht)
        {
            // 
            Dictionary<string, Link> results = new Dictionary<string, Link>();
            string querykey = query.ToLower();
            if (querykey == "")
            {
                foreach (KeyValuePair<string, Link> k in ht)
                {
                    AddOrUpdate(results, k);
                }
            }
            else
            {
                // 优先列出相等的
                foreach (KeyValuePair<string, Link> k in ht)
                {
                    if (IsEquals(querykey, k))
                    {
                        AddOrUpdate(results, k);
                    }

                }
                // 其实列出打头的
                foreach (KeyValuePair<string, Link> k in ht)
                {
                    string innerkey = k.Key.ToString();
                    if (!IsEquals(querykey, k) && IsStartWiths(querykey, k))
                    {
                        AddOrUpdate(results, k);
                    }
                }
                // 然后列出包含的
                foreach (KeyValuePair<string, Link> k in ht)
                {
                    string innerkey = k.Key.ToString();
                    if (!innerkey.Equals(querykey) && !innerkey.StartsWith(querykey) && IsContains(querykey, k))
                    {
                        AddOrUpdate(results, k);
                    }
                }
            }
            return results;

        }
        private void AddOrUpdate(Dictionary<string, Link> results, KeyValuePair<string, Link> keyValue)
        {
            if (results.ContainsKey(keyValue.Key))
                results[keyValue.Key] = keyValue.Value;
            else
                results.Add(keyValue.Key, keyValue.Value);            
        }
        private static bool IsEquals(string querykey, KeyValuePair<string, Link> k)
        {
            string innerkey = k.Key.ToString();
            return (k.Value.GetType() == typeof(Link) && innerkey.Equals(querykey))
                                    || (k.Value.GetType() == typeof(KeyLink) && ((KeyLink)(k.Value)).KeyName.Equals(querykey));
        }
        private static bool IsStartWiths(string querykey, KeyValuePair<string, Link> k)
        {
            string innerkey = k.Key.ToString();
            return (k.Value.GetType() == typeof(Link) && innerkey.StartsWith(querykey))
                                    || (k.Value.GetType() == typeof(KeyLink) && ((KeyLink)(k.Value)).KeyName.StartsWith(querykey));
        }
        private static bool IsContains(string querykey, KeyValuePair<string, Link> k)
        {
            string innerkey = k.Key.ToString();
            return (k.Value.GetType() == typeof(Link) && innerkey.Contains(querykey))
                                    || (k.Value.GetType() == typeof(KeyLink) && ((KeyLink)(k.Value)).KeyName.Contains(querykey));
        }
    }
    class StartMenuLinkAdapter : LinkAdapter
    {
        public StartMenuLinkAdapter(LinkSet links)
            : base(ShellHelper.GetStartMenu(), "*.lnk", links, true)
        {
        }
    }
    class ProgramLinkAdapter : LinkAdapter
    {
        public ProgramLinkAdapter(LinkSet links)
            : base(Environment.GetFolderPath(Environment.SpecialFolder.Programs), "*.lnk", links, true)
        {
        }
    }
    class SystemLinkAdapter : LinkAdapter
    {
        public SystemLinkAdapter (LinkSet links)
            : base(Environment.SystemDirectory, "*.exe", links, false)            
        {
        }
    }
    class CurrentDirLinkAdapter : LinkAdapter
    {
        public CurrentDirLinkAdapter(LinkSet links)
            : base(Environment.CurrentDirectory, "*.exe", links,false)
        {
        }
    }
    class LinkAdapter
    {
        FileSystemWatcher watcher = new FileSystemWatcher();
        public string Dir;
        public string Ext;
        public bool Rescure = false;
        public LinkAdapter(string dir, string ext,LinkSet links,bool rescure)
        {
            Dir = dir;
            Ext = ext;
            RunLinks = links;
            Rescure = rescure;
            MakeLinks();
            WatchChange(dir, ext);
        }

        private void WatchChange(string dir, string ext)
        {
            // 监视这个目录的变化
            watcher.Path = dir;
            watcher.IncludeSubdirectories = true;
            watcher.Filter = ext;
            watcher.NotifyFilter = //NotifyFilters.LastAccess | NotifyFilters.LastWrite|
                NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            watcher.EnableRaisingEvents = true;
        }

        private  void MakeLinks()
        {
            // 搜索这个目录，构建搜索池            
            SearchOption so = SearchOption.TopDirectoryOnly;
            if (Rescure)
                so = SearchOption.AllDirectories;
            DirectoryInfo di = new DirectoryInfo(Dir);
            FileInfo[] rgFiles = di.GetFiles(Ext, so);
            RunLinks.Fill(rgFiles);
        }
        LinkSet RunLinks = null;
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            MakeLinks();
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            MakeLinks();
        }

    }
    class Link
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
        public Link(string filename,string fullfilename)
        {
            fileName = filename;
            fullFileName = fullfilename;
        }
    }
    class KeyLink : Link
    {
        private string keyName = "";

        public string KeyName
        {
            get { return keyName; }
            set { keyName = value; }
        }
        public KeyLink(string keyname,string filename, string fullfilename):base(filename,fullfilename)
        {
            keyName = keyname;
        }
    }
}
