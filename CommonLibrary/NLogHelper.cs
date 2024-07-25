using NLog;
using System.IO;

namespace Library
{
    public static class NLogHelper
    {
        public static void EnsureNLogDirectoryExists()
        {
            var logFileTarget = LogManager.Configuration?.FindTargetByName<NLog.Targets.FileTarget>("logfile");
            string logFileName = logFileTarget?.FileName.Render(LogEventInfo.CreateNullEvent());
            Directory.CreateDirectory(Path.GetDirectoryName(logFileName));
        }
    }
}