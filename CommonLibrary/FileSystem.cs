using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Library
{
    public static class FileSystem
    {
        #region FormatByteSize
        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern long StrFormatByteSize(long fileSize, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer, int bufferSize);

        public static string FormatByteSize(long FileSize)
        {
            StringBuilder sb = new StringBuilder(20);
            StrFormatByteSize(FileSize, sb, sb.Capacity);
            return sb.ToString();
        }

        public static string FormatByteSize(string File)
        {
            FileInfo fi = new FileInfo(File);
            return FormatByteSize(fi.Length);
        }
        #endregion

        #region RemoveInvalidCharsFromFileName
        public static string RemoveInvalidCharsFromFileName(string file)
        {
            //string IllegalChars = "\\/:*?\"<>|";
            //char[] chars = IllegalChars.ToCharArray();

            char[] chars = Path.GetInvalidFileNameChars();

            string value = file;

            for (int i = 0; i < chars.Length; i++)
            {
                value = value.Replace(chars[i].ToString(), "");
            }

            return value;
        }
        #endregion

        #region FindFiles
        // brutal langsam
        public static List<FileInfo> FindFiles(string startDir, string filename)
        {
            List<FileInfo> files = new List<FileInfo>();
            FindFilesHelper(startDir, filename, files);
            return files;
        }

        public static List<FileInfo> FindFiles(Environment.SpecialFolder specialFolder, string filename)
        {
            return FindFiles(Environment.GetFolderPath(specialFolder), filename);
        }

        private static void FindFilesHelper(string startDir, string filename, List<FileInfo> files)
        {
            DirectoryInfo mainDir = new DirectoryInfo(startDir);
            FileSystemInfo[] items = mainDir.GetFileSystemInfos();

            //Debug.Print(startDir);

            foreach (FileSystemInfo item in items)
            {
                if (item is DirectoryInfo)
                {
                    FindFilesHelper(((DirectoryInfo)item).FullName, filename, files);
                }

                if (item is FileInfo)
                {
                    FileInfo fileInfo = item as FileInfo;

                    if (string.Compare(fileInfo.Name, filename, true) == 0)
                    {
                        files.Add(fileInfo);
                    }
                }
            }
        }
        #endregion

        #region GetAllFilesByExtensionRecursive
        public static List<FileInfo> GetAllFilesByExtensionRecursive(string dir, string extension)
        {
            List<FileInfo> files = new List<FileInfo>();
            GetAllFilesByExtensionRecursiveHelper(dir, extension, files);
            return files;
        }

        private static void GetAllFilesByExtensionRecursiveHelper(string dir, string extension, List<FileInfo> files)
        {
            DirectoryInfo mainDir = new DirectoryInfo(dir);
            FileSystemInfo[] items = mainDir.GetFileSystemInfos();

            foreach (FileSystemInfo item in items)
            {
                if (item is DirectoryInfo)
                {
                    GetAllFilesByExtensionRecursiveHelper(((DirectoryInfo)item).FullName, extension, files);
                }

                if (item is FileInfo)
                {
                    FileInfo fileInfo = item as FileInfo;
                    if (fileInfo.Extension.ToUpper().CompareTo(extension.ToUpper()) == 0)
                    {
                        files.Add(fileInfo);
                    }
                }
            }
        }
        #endregion

        #region CompactPath
        public static string CompactPath(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return string.Empty;
            }

            string compactedPath = file;

            if (file.Length > 40)
            {
                compactedPath = string.Empty;
                string[] tokens = file.Split(new char[] { '\\' });

                if (tokens.Length > 1)
                {
                    for (int i = tokens.Length - 2; i > 0; i--)
                    {
                        if (compactedPath.Length > 0)
                        {
                            compactedPath = "\\" + compactedPath;
                        }

                        compactedPath = tokens[i] + compactedPath;

                        if (compactedPath.Length > 30)
                        {
                            break;
                        }
                    }

                    if ((tokens[0] + "\\" + compactedPath + "\\" + tokens[tokens.Length - 1]) == file)
                    {
                        compactedPath = (tokens[0] + "\\" + compactedPath + "\\" + tokens[tokens.Length - 1]);
                    }
                    else
                    {
                        compactedPath = tokens[0] + "\\...\\" + compactedPath + "\\" + tokens[tokens.Length - 1];
                    }
                }
            }

            return compactedPath;
        }
        #endregion

        #region MergeFiles
        public static void MergeFiles(string[] filesToMerge, string targetFile)
        {
            if (filesToMerge == null || filesToMerge.Length == 0 || string.IsNullOrEmpty(targetFile))
            {
                return;
            }

            using (FileStream output = new FileStream(targetFile, FileMode.Create))
            {
                foreach (string fileToMerge in filesToMerge)
                {
                    int bytesRead = 0;
                    byte[] buffer = new byte[1024];

                    using (FileStream input = new FileStream(fileToMerge, FileMode.Open))
                    {
                        while ((bytesRead = input.Read(buffer, 0, 1024)) > 0)
                        {
                            output.Write(buffer, 0, bytesRead);
                        }

                        input.Close();
                    }
                }

                output.Close();
            }
        }
        #endregion

        #region RemoveExtension
        public static string RemoveExtension(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            if (path.EndsWith("."))
            {
                return path.Substring(0, path.Length - 1);
            }

            int pos = path.LastIndexOf('.');
            return path.Substring(0, pos);
        }
        #endregion

        #region GetUniqueFilename
        /// <summary>
        /// Gets an unique filename in a directory. The uniqueness is created by adding
        /// increasing numbers to the filename, as long as it's neccessary.
        /// </summary>
        /// <param name="filenameToMakeUnique">The filename to make unique.</param>
        /// <returns>The unique filenam</returns>
        public static string GetUniqueFilename(string filenameToMakeUnique)
        {
            string dir = Path.GetDirectoryName(filenameToMakeUnique);
            string ext = Path.GetExtension(filenameToMakeUnique);
            string filename = Path.GetFileNameWithoutExtension(filenameToMakeUnique);
            string uniqueFilename = string.Format("{0}{1}", filename, ext);
            string filenameToCheck = Path.Combine(dir, uniqueFilename);
            int i = 0;
            while (File.Exists(filenameToCheck))
            {
                uniqueFilename = string.Format("{0}_{1}{2}", filename, i.ToString(), ext);
                filenameToCheck = Path.Combine(dir, uniqueFilename);
                i++;
            }
            return filenameToCheck;
        }
        #endregion

        #region AppendLastChangeToFilename
        /// <summary>
        /// Appends the last change date and time to the filename
        /// </summary>
        /// <param name="oldFilename">filename of the original file</param>
        /// <returns></returns>
        public static string AppendLastChangeToFilename(string oldFilename, string format)
        {
            if (File.Exists(oldFilename))
            {
                return AppendDateTimeToFilename(oldFilename, File.GetLastWriteTime(oldFilename), format);
            }
            else
            {
                return oldFilename;
            }
        }
        #endregion

        /// <summary>
        /// Appends the creation date and time to the filename
        /// </summary>
        /// <param name="oldFilename">filename of the original file</param>
        /// <returns></returns>
        public static string AppendCreationToFilename(string oldFilename, string format)
        {
            if (File.Exists(oldFilename))
            {
                return AppendDateTimeToFilename(oldFilename, File.GetCreationTime(oldFilename), format);
            }
            else
            {
                return oldFilename;
            }
        }

        private static string AppendDateTimeToFilename(string oldFilename, DateTime dateTimeToAppend, string format)
        {
                string filename = Path.GetFileNameWithoutExtension(oldFilename);
                string extension = Path.GetExtension(oldFilename);
                string directory = Path.GetDirectoryName(oldFilename);
                string lastChange = dateTimeToAppend.ToString(format);
                return Path.Combine(directory, string.Format("{0}{1}{2}", filename, lastChange, extension));

        }
    }
}
