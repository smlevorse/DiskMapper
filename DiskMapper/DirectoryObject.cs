using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskMapper
{
    abstract class DirectoryObject
    {
        /// <summary>
        /// The size in bytes of the object
        /// </summary>
        public abstract long Size { get; }

        /// <summary>
        /// The path to the object
        /// </summary>
        public abstract string Path { get; }

        /// <summary>
        /// The directory this directory object is located in
        /// NULL implies this is the root of the file tree
        /// </summary>
        public abstract DirectoryNode Parent { get; }
        
        /// <summary>
        /// Sets the size of this directory object 
        /// </summary>
        /// <param name="size">The number of bytes this directory object takes up</param>
        public abstract void UpdateSize(long size);
        
    }
}
