using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.Threading;
using System.Net.Sockets;
namespace WindowsFormsApp1
{
    public static class cs
    {

        public static Process Run(String fn, String arg)
        {
            Process p = null;
            ProcessStartInfo si = null;
            try
            {
                si = new ProcessStartInfo(fn);
                si.Arguments = arg;
                p = Process.Start(si);
                return p;
            }
            catch
            {
                return null;
            }
        }

        public static object LoadFile(object sender, EventArgs e, String filename)
        {
            try
            {
                return ByteArrayToObject(File.ReadAllBytes(filename));
            }
            catch
            {
                return null;
            }
        }

        public static void SaveFile(object sender, EventArgs e, String filename, object o)
        {
            try
            {
                File.WriteAllBytes(filename, ObjectToByteArray(o));
            }
            catch
            { }
        }

        // Convert an object to a byte array
        public static byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        // Convert a byte array to an Object
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return obj;
        }

        public static byte[] NetObjectToByteArray(Socket s, Object obj)
        {
            // here error !!!
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
        public static Object NetByteArrayToObject(Socket s, byte[] arrBytes)
        {
            NetworkStream memStream = new NetworkStream(s);
            if (memStream.DataAvailable)
            {
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                Object obj = (Object)binForm.Deserialize(memStream);
                return obj;
            }
            return null;
        }
    }


    public class RecursiveFileSearch
    {
        static System.Collections.Specialized.StringCollection log = new System.Collections.Specialized.StringCollection();

        static void r_Main()
        {
            // Start with drives if you have to search the entire computer.
            string[] drives = System.Environment.GetLogicalDrives();

            foreach (string dr in drives)
            {
                System.IO.DriveInfo di = new System.IO.DriveInfo(dr);

                // Here we skip the drive if it is not ready to be read. This
                // is not necessarily the appropriate action in all scenarios.
                if (!di.IsReady)
                {
                    Console.WriteLine("The drive {0} could not be read", di.Name);
                    continue;
                }
                System.IO.DirectoryInfo rootDir = di.RootDirectory;
                WalkDirectoryTree(rootDir);
            }

            // Write out all the files that could not be processed.
            Console.WriteLine("Files with restricted access:");
            foreach (string s in log)
            {
                Console.WriteLine(s);
            }
            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }

        public static void WalkDirectoryTree(System.IO.DirectoryInfo root)
        {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.*");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException e)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                log.Add(e.Message);
            }

            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    // In this example, we only access the existing FileInfo object. If we
                    // want to open, delete or modify the file, then
                    // a try-catch block is required here to handle the case
                    // where the file has been deleted since the call to TraverseTree().
                    Console.WriteLine(fi.FullName);
                    // 
                    PlayFile(fi);
                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkDirectoryTree(dirInfo);
                }
            }
        }

        public static void PlayFile(FileInfo fi)
        {
            if ((fi.Extension == ".png") || (fi.Extension == ".jpg") || (fi.Extension == ".jfif") || (fi.Extension == ".gif") || (fi.Extension == ".jpeg") || (fi.Extension == ".bmp"))
            {
                /*
                Form1.mf.BackgroundImage = Image.FromFile(fi.FullName);
                Thread.Sleep(1500);
                */
            }
            else if ((fi.Extension == ".mp4"))
            {

            }
        }
    }
}