using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace runplus
{
    interface IWatcherSink
    {
        void changed();
    }
    class FSWatcher : FileSystemWatcher
    {
        IWatcherSink Sink = null;
        public FSWatcher(IWatcherSink sink)
        {
            Sink = sink;
        }
        public void WatchChange(string dir, string ext)
        {
            // 监视这个目录的变化
            Path = dir;
            IncludeSubdirectories = true;
            Filter = ext;
            NotifyFilter = //NotifyFilters.LastAccess | NotifyFilters.LastWrite|
                NotifyFilters.FileName | NotifyFilters.DirectoryName;
            Changed += new FileSystemEventHandler(OnChanged);
            Created += new FileSystemEventHandler(OnChanged);
            Deleted += new FileSystemEventHandler(OnChanged);
            Renamed += new RenamedEventHandler(OnRenamed);
            EnableRaisingEvents = true;
        }
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Sink.changed();
        }
        private void OnRenamed(object source, RenamedEventArgs e)
        {
            Sink.changed();
        }

    }
}
