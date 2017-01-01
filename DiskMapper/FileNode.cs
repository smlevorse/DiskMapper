using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DiskMapper
{
    class FileNode : DirectoryObject
    {
        // Variables
        private DirectoryNode _parent;
        private string _path;
        private FileInfo _fileInfo;
        private long _size;

        // Properties
        public override DirectoryNode Parent
        {
            get
            {
                return _parent;
            }
        }

        public override string Path
        {
            get
            {
                return _path;
            }
        }

        public FileInfo FileInfo { get { return _fileInfo; } }

        public override long Size
        {
            get
            {
                return _size;
            }
        }

        public FileNode(FileInfo fileInfo, DirectoryNode parent)
        {
            _parent = parent;
            this._fileInfo = fileInfo;
            this._path = fileInfo.FullName;
            this._size = 0;

        }

        public FileNode(string path, DirectoryNode parent)
        {
            this._fileInfo = new FileInfo(path);
            this._path = path;
            this._parent = parent;
            this._size = 0;
        }

        // Parameter is ignored here
        public override void UpdateSize(long size)
        {
            _size = _fileInfo.Length;
            _parent.UpdateSize(_size);
        }
    }
}
