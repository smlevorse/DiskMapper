using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DiskMapper
{
    class DirectoryNode : DirectoryObject
    {
        #region Fields

        private long _size;
        private string _path;
        private DirectoryNode _parent;
        private List<DirectoryObject> _children;
        private DirectoryInfo _dirInfo;
        private object _mutexLock;

        #endregion

        #region Properties

        public override DirectoryNode Parent { get { return _parent; } }

        public override long Size { get { return _size; } }

        public override string Path { get { return _path; } }

        public DirectoryInfo DirectoryInfo { get { return _dirInfo; } }
        
        public List<DirectoryObject> Children { get { return _children; } }

        #endregion

        #region Constructors

        public DirectoryNode(DirectoryNode parent, string objectName)
        {
            _path = parent.Path + objectName;
            _size = 0;
            _parent = parent;
            _children = new List<DirectoryObject>();
            _dirInfo = new DirectoryInfo(_path);
            _mutexLock = new object();
        }

        public DirectoryNode(DirectoryInfo directoryInfo, DirectoryNode parent)
        {
            _dirInfo = directoryInfo;
            _path = _dirInfo.FullName;
            _size = 0;
            _parent = parent;
            _children = new List<DirectoryObject>();
            _mutexLock = new object();
        }

        public DirectoryNode(string path, DirectoryNode parent)
        {
            _path = path;
            _size = 0;
            _parent = parent;
            _children = new List<DirectoryObject>();
            _dirInfo = new DirectoryInfo(_path);
            _mutexLock = new object();
        }

        #endregion

        public override void UpdateSize(long size)
        {
         
            this.AddSize(size);
            if(Parent == null)
            {
                Console.WriteLine($"Size of {_path} is {_size}");
            }
            this.Parent?.UpdateSize(size);
        }
        
        private void AddSize(long size)
        {
            lock (_mutexLock)
            {
                _size += size;
            }
        }

        public void CreateChildNodes()
        {
            // Get the files in this directory
            FileInfo[] files = _dirInfo.GetFiles();
            
            foreach(FileInfo file in files)
            {
                FileNode newNode = new FileNode(file, this);
                Children.Add(newNode);
                Program.fileQueue.Enqueue(newNode);
            }

            // Get the subdirectories in this directory
            DirectoryInfo[] subDirs = _dirInfo.GetDirectories();

            foreach(DirectoryInfo dir in subDirs)
            {
                DirectoryNode newNode = new DirectoryNode(dir, this);
                Children.Add(newNode);
                Program.directoryQueue.Enqueue(newNode);
            }
        }
    }
}
