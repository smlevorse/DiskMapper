using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DiskMapper
{
    class Program
    {
        // Constants
        private const int NUM_F_THREADS = 5;
        private const int NUM_D_THREADS = 5;

        // Two global queues for processing directories and files
        public static Queue<DirectoryNode> directoryQueue;
        public static Queue<FileNode> fileQueue;

        // Locks to prevent two threads from accessing one queue at the same time
        private static object readFileLock = new object();
        private static object readDirLock = new object();

        static void Main(string[] args)
        {
            directoryQueue = new Queue<DirectoryNode>();
            fileQueue = new Queue<FileNode>();

            // Create threads
            List<Thread> fileThreads = new List<Thread>();
            List<Thread> directoryThreads = new List<Thread>();

            for (int i = 0; i < NUM_F_THREADS; i++)
            {
                fileThreads.Add(new Thread(ProcessFileNode));
            }

            for(int i = 0; i < NUM_D_THREADS; i++)
            {
                directoryThreads.Add(new Thread(ProcessDirectoryNode));
            }

            // Create root node
            DirectoryNode root = new DirectoryNode(@"C:\Users\Sean\Documents\2D_Anim_Asset_Prod", null);
            directoryQueue.Enqueue(root);

            // Start threads
            foreach(Thread t in fileThreads.Concat(directoryThreads))
            {
                t.Start();
            }

            foreach(Thread t in fileThreads.Concat(directoryThreads))
            {
                t.Join();
            }

            Console.WriteLine("DONE");
            Console.ReadLine();
            
        }

        private static void ProcessFileNode()
        {
            SpinWait.SpinUntil(() =>
            {
                try
                {
                    FileNode node = null;

                    // Get the next file
                    lock (readFileLock)
                    {
                        if (fileQueue.Count >= 1)
                        {
                            node = fileQueue.Dequeue();
                        }
                    }

                    // Start processing the file's size and adding it to all of the parent directories
                    if (node != null)
                    {
                        node.UpdateSize(0);
                    }
                    return false;
                }
                catch(Exception e)
                {
                    Console.WriteLine("\tFile Node threw exception: " + e.Message);
                    return true;
                }
            }, -1);
        }

        private static void ProcessDirectoryNode()
        {
            SpinWait.SpinUntil(() =>
           {
               try
               {
                   DirectoryNode node = null;

                   // Get the next directory
                   lock (readDirLock)
                   {
                       if (directoryQueue.Count >= 1)
                       {
                           node = directoryQueue.Dequeue();
                       }
                   }

                   // Start processin the directory by creating it's child nodes
                   if (node != null)
                   {
                       node.CreateChildNodes();
                   }
                   return false;
               }
               catch(Exception e)
               {
                   Console.WriteLine("\tDirectory Node threw exception: " + e.Message);
                   return true;
               }
           }, -1);
        }
    }
}
