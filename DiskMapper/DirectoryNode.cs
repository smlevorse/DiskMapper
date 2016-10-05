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
        private DirectoryObject _parent;
        private DirectoryObject[] _children;

        #endregion

        #region Properties

        public override DirectoryObject Parent { get { return _parent; } }

        public override long Size { get { return _size; } }

        public override string Path { get { return _path; } }
        
        public DirectoryObject[] Children { get { return _children; } }

        #endregion

        #region Constructors

        public DirectoryNode(DirectoryObject parent, string objectName)
        {
            _path = parent.Path + objectName;
            _size = 0;
            _parent = parent;

            
        }

        #endregion

        protected override void UpdateSize(long size)
        {
            throw new NotImplementedException();
        }
    }
}
