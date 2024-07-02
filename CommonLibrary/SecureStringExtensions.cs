using System.Security;
using System.Runtime.InteropServices;

namespace Extensions
{
    public static class SecureStringExtensions
    {
        public static string AsSystemString(this SecureString SecureString)
        {
            return Marshal.PtrToStringUni(Marshal.SecureStringToBSTR(SecureString));
        }
    }
}