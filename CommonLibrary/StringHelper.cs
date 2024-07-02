using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public static class StringHelper
    {
        #region Reverse
        public static string Reverse(string data)
        {
            char[] chars = data.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        
            /*
            Array arr = data.ToCharArray();
            Array.Reverse(arr);
            char[] c = (char[])arr;
            byte[] b = Encoding.Default.GetBytes(c);
            return Encoding.Default.GetString(b);
            */
        }
        #endregion

        #region HexDecode
        public static string HexDecode(string data)
        {
            int byteLen = data.Length / 2;
            byte[] bytes = new byte[byteLen];

            string hex;
            int j = 0;

            for (int i = 0; i < bytes.Length; i++)
            {
                hex = new String(new Char[] { data[j], data[j + 1] });
                bytes[i] = HexToByte(hex);
                j = j + 2;
            }

            return Encoding.Default.GetString(bytes);
        }

        private static byte HexToByte(string hex)
        {
            byte newByte = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return newByte;
        }
        #endregion

        #region IsNumeric
        public static bool IsNumeric(string Data)
        {
            try
            {
                double.Parse(Data);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        #endregion

        #region IsDate
        public static bool IsDate(string Data)
        {
            try
            {
                DateTime.Parse(Data);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        #endregion

        #region ProperCase
        public static string ProperCase(string stringInput)
        {
            StringBuilder sb = new StringBuilder();
            string[] words = stringInput.Split(' ');
            foreach (string word in words)
            {
                if (word.Length > 2)
                {
                    sb.Append(Char.ToUpper(word[0]));
                    sb.Append(word.Substring(1).ToLower());
                }
                else
                {
                    sb.Append(word);
                }

                sb.Append(" ");
            }

            return sb.ToString();
        }
        #endregion

        #region CountChar
        public static int CountChar(string searchString, char find)
        {
            return CountChar(searchString.ToCharArray(), find);
        }

        public static int CountChar(char[] searchString, char find)
        {
            int x = 0;
            for (int i = 0; i < searchString.Length; i++)
            {
                if (searchString[i].CompareTo(find) == 0)
                {
                    x++;
                }
            }
            return x;
        }
        #endregion

        #region SubstringDelimitedByStartAndEndChar
        public static string SubstringDelimitedByStartAndEndChar(string data, char startChar, char endChar)
        {
            if (data == null)
                return "";

            int posFirst = data.IndexOf(startChar);
            int posSecond = data.IndexOf(endChar, posFirst);

            string retValue = "";

            if (posSecond > posFirst)
            {
                retValue = data.Substring(posFirst + 1, posSecond - posFirst - 1);
            }

            return retValue;
        }
        #endregion

        #region InsertMid
        public static void InsertMid(ref string data, int startIndexOneBased, string insertText)
        {
            if (string.IsNullOrEmpty(insertText))
            {
                return;
            }

            string left = string.Empty;

            if (startIndexOneBased > 1)
            {
                left = data.Substring(0, startIndexOneBased - 1);
            }

            string right = data.Substring(startIndexOneBased - 1 + insertText.Length);
            data = left + insertText + right;
        }
        #endregion

        #region FillStr
        public static string FillStr(string data, int length)
        {
            return string.Format("{0,-" + length + "}", data);
        }
        #endregion

        #region SplitByLength
        public static List<string> SplitByLength(string data, int blockLength)
        {
            return SplitByLength(data, 1, blockLength);
        }

        public static List<string> SplitByLength(string data, int startOneBased, int blockLength)
        {
            List<string> retval = new List<string>();

            int counter = 0;

            while (true)
            {
                int index = startOneBased - 1 + (counter * blockLength);

                if (index + 1 + blockLength > data.Length)
                {
                    retval.Add(data.Substring(index));
                    break;
                }

                string splitted = data.Substring(index, blockLength);
                retval.Add(splitted);
                counter++;
            }

            return retval;
        }

        /*
        public static string[] SplitByPos(string data, int pos)
        {
            string[] result = null;

            if (!String.IsNullOrEmpty(data))
            {
                if (data.Length <= pos)
                {
                    result = new string[0];
                    result[0] = data;
                }
                else
                {
                    result = new string[(data.Length / pos) + 1];

                    for (int i = 0; i < result.GetUpperBound(0) + 1; i++)
                        result[i] = SubstringExt(data, pos * i + 1, pos);
                }
            }

            return result;
        }
        */
        #endregion

        #region SubstringExt
        public static string SubstringExt(string data, int startIndexOneBased, int length)
        {
            if (data.Length < startIndexOneBased - 1)
            {
                return string.Empty;
            }

            if (startIndexOneBased + length > data.Length)
            {
                return data.Substring(startIndexOneBased - 1).Trim();
            }

            return data.Substring(startIndexOneBased - 1, length).Trim();
        }
        #endregion
    }
}
