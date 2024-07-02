using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Library
{
    public static class ExceptionHandling
    {
        // safe delete
        // LanguageUtils.IgnoreErrors(() => File.Delete(workFile));

        /// <summary>
        /// Runs an operation and ignores any Exceptions that occur.
        /// Returns true or falls depending on whether catch was
        /// triggered
        /// </summary>
        /// <param name="operation">lambda that performs an operation that might throw</param>
        /// <returns></returns>
        public static bool IgnoreErrors(Action operation)
        {
            if (operation == null)
                return false;
            try
            {
                operation.Invoke();
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Runs an function that returns a value and ignores any Exceptions that occur.
        /// Returns true or falls depending on whether catch was
        /// triggered
        /// </summary>
        /// <param name="operation">parameterless lamda that returns a value of T</param>
        /// <param name="defaultValue">Default value returned if operation fails</param>
        public static T IgnoreErrors<T>(Func<T> operation, T defaultValue = default(T))
        {
            if (operation == null)
                return defaultValue;

            T result;
            try
            {
                result = operation.Invoke();
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        #region GetCustomStackTrace
        public static string GetCustomStackTrace(Exception e)
        {
            StringBuilder sb = new StringBuilder();

            StackTrace st = new StackTrace(e, true);

            foreach (StackFrame frame in st.GetFrames())
            {
                if (!string.IsNullOrEmpty(frame.GetFileName()))
                {
                    sb.AppendLine(string.Format("   at {0}({1}) in {2}:line {3}", frame.GetMethod().DeclaringType.FullName + '.' + frame.GetMethod().Name, GetParameterString(frame.GetMethod()), Path.GetFileName(frame.GetFileName()), frame.GetFileLineNumber().ToString()));
                }
                else
                {
                    sb.AppendLine(string.Format("   at {0}({1})", frame.GetMethod().DeclaringType.FullName + '.' + frame.GetMethod().Name, GetParameterString(frame.GetMethod())));
                }
            }

            return sb.ToString();
        }

        private static string GetParameterString(MethodBase method)
        {
            if (method == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            ParameterInfo[] paramInfos = method.GetParameters();

            for (int i = 0; i < paramInfos.Length; i++)
            {
                ParameterInfo paramInfo = paramInfos[i];

                string data = paramInfo.ToString();
                if (paramInfo.ParameterType.FullName != null)
                {
                    data = data.Replace(paramInfo.ParameterType.FullName, paramInfo.ParameterType.Name);
                }

                sb.Append(data);

                if (i < paramInfos.Length - 1)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }
        #endregion
    }
}