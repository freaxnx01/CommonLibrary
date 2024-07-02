using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using Library.InteropWin32;
using System.Diagnostics;

namespace Library
{
    public class IniFileAccess
    {
        public string IniFilePath { get; set; }

        public IniFileAccess() {}

        public IniFileAccess(string iniFilePath)
        {
            iniFilePath = this.IniFilePath;
        }

        public string GetSectionContent(string sectionName)
        {
            const int MAX_BUFFER_SIZE = 32767;
            byte[] buffer = new byte[MAX_BUFFER_SIZE];
            Kernel32.GetPrivateProfileSectionA(sectionName, buffer, MAX_BUFFER_SIZE, this.IniFilePath);
            string content = Encoding.ASCII.GetString(buffer);

            StringBuilder sb = new StringBuilder();

            foreach (string item in content.Split('\0'))
            {
                if (!string.IsNullOrEmpty(item))
                {
                    sb.AppendLine(item);
                }
            }

            return sb.ToString();
        }

        public Dictionary<string, string> GetSectionData(string sectionName)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string content = GetSectionContent(sectionName);
            string[] splitted = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in splitted)
            {
                if (!dict.ContainsKey(ExtractKey(item)))
                {
                    dict.Add(ExtractKey(item), ExtractValue(item));
                }
            }

            return dict;
        }

        public string ExtractKey(string data)
        {
            string[] splitted = data.Split('=');
            if (splitted.Length == 2)
            {
                return splitted[0];
            }

            return string.Empty;
        }

        public string ExtractValue(string data)
        {
            string[] splitted = data.Split('=');
            if (splitted.Length == 2)
            {
                return splitted[1];
            }

            return string.Empty;
        }

        public List<string> GetSectionNames()
        {
            List<string> names = new List<string>();

            const int INITIAL_BUFFER_SIZE = 1024;

            byte[] buffer = new byte[0];

            uint maxsize = INITIAL_BUFFER_SIZE;
            while (true)
            {
                buffer = new byte[maxsize];
                uint size = Kernel32.GetPrivateProfileSectionNamesA(buffer, maxsize, this.IniFilePath);
                if ((size != 0) && (size == (maxsize - 2)))
                {
                    maxsize *= 2;
                }
                else
                {
                    break;
                }
            }

            string sections = Encoding.ASCII.GetString(buffer);

            foreach (string section in sections.Split('\0'))
            {
                if (!string.IsNullOrEmpty(section))
                {
                    names.Add(section);
                }
            }

            return names;
        }
    }
}
