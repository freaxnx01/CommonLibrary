using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Library
{
    public class Shell
    {
        #region ShowHtmlFile
        public static void ShowHtmlFile(string file)
        {
            StartDocument(file);
        }
        #endregion

        #region StartDocument
        public static void StartDocument(string file)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = file;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }
        #endregion

        #region ShowFolder
        public static void ShowFolder(string folder)
        {
            if (Directory.Exists(folder))
            {
                Process proc = new Process();
                proc.StartInfo.FileName = "explorer.exe";
                proc.StartInfo.Arguments = folder;
                proc.Start();
            }
        }
        #endregion
    }
}
