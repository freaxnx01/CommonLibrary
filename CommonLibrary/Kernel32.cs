using System;
using System.Collections.Generic;
using System.Text;

namespace Library.InteropWin32
{
    public static class Kernel32
    {
        #region initialization file (INI)
        /// Return Type: DWORD->unsigned int
        ///lpAppName: LPCSTR->CHAR*
        ///lpKeyName: LPCSTR->CHAR*
        ///lpDefault: LPCSTR->CHAR*
        ///lpReturnedString: LPSTR->CHAR*
        ///nSize: DWORD->unsigned int
        ///lpFileName: LPCSTR->CHAR*
        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "GetPrivateProfileStringA")]
        public static extern uint GetPrivateProfileStringA([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpAppName, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpKeyName, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpDefault, [System.Runtime.InteropServices.OutAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] System.Text.StringBuilder lpReturnedString, uint nSize, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpFileName);

        /// Return Type: BOOL->int
        ///lpAppName: LPCSTR->CHAR*
        ///lpKeyName: LPCSTR->CHAR*
        ///lpString: LPCSTR->CHAR*
        ///lpFileName: LPCSTR->CHAR*
        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "WritePrivateProfileStringA")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool WritePrivateProfileStringA([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpAppName, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpKeyName, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpString, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpFileName);

        /// Return Type: BOOL->int
        ///lpAppName: LPCSTR->CHAR*
        ///lpString: LPCSTR->CHAR*
        ///lpFileName: LPCSTR->CHAR*
        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "WritePrivateProfileSectionA")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool WritePrivateProfileSectionA([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpAppName, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpString, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpFileName);

        /// Return Type: DWORD->unsigned int
        ///lpAppName: LPCSTR->CHAR*
        ///lpReturnedString: LPSTR->CHAR*
        ///nSize: DWORD->unsigned int
        ///lpFileName: LPCSTR->CHAR*
        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "GetPrivateProfileSectionA")]
        public static extern uint GetPrivateProfileSectionA([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpAppName, [System.Runtime.InteropServices.OutAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPArray)] byte[] lpReturnedByteArray, uint nSize, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpFileName);
        //public static extern uint GetPrivateProfileSectionA([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpAppName, [System.Runtime.InteropServices.OutAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] System.Text.StringBuilder lpReturnedString, uint nSize, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpFileName);

        /// Return Type: DWORD->unsigned int
        ///lpszReturnBuffer: LPSTR->CHAR*
        ///nSize: DWORD->unsigned int
        ///lpFileName: LPCSTR->CHAR*
        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "GetPrivateProfileSectionNamesA")]
        public static extern uint GetPrivateProfileSectionNamesA([System.Runtime.InteropServices.OutAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPArray)] byte[] lpReturnedByteArray, uint nSize, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpFileName);
        //public static extern uint GetPrivateProfileSectionNamesA([System.Runtime.InteropServices.OutAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] System.Text.StringBuilder lpszReturnBuffer, uint nSize, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpFileName);

        /// Return Type: DWORD->unsigned int
        ///lpAppName: LPCSTR->CHAR*
        ///lpKeyName: LPCSTR->CHAR*
        ///lpDefault: LPCSTR->CHAR*
        ///lpReturnedString: LPSTR->CHAR*
        ///nSize: DWORD->unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "GetProfileStringA")]
        public static extern uint GetProfileStringA([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpAppName, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpKeyName, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpDefault, [System.Runtime.InteropServices.OutAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] System.Text.StringBuilder lpReturnedString, uint nSize);

        /// Return Type: BOOL->int
        ///lpAppName: LPCSTR->CHAR*
        ///lpKeyName: LPCSTR->CHAR*
        ///lpString: LPCSTR->CHAR*
        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "WriteProfileStringA")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool WriteProfileStringA([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpAppName, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpKeyName, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpString);
        #endregion
    }
}
