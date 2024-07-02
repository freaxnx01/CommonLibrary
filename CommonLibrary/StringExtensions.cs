using System.Security;

namespace Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static SecureString AsSecureString(this string String)
        {
            SecureString result = null;

            try
            {
                result = new SecureString();

                foreach (char c in String)
                {
                    result.AppendChar(c);
                }

                result.MakeReadOnly();

                return result;
            }
            catch
            {
                if (result != null)
                {
                    result.Dispose();
                    throw;
                }
            }

            return null;
        }
    }
}
