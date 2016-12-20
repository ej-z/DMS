using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DMS
{
    static class Logger
    {
        static string file;

        static Logger()
        {
            var systemPath = System.Environment.
                             GetFolderPath(
                                 Environment.SpecialFolder.CommonApplicationData
                             );
            file = Path.Combine(systemPath, "Error.log");
        }

        public static string LogLocation { get { return file; } }

        public static void Log(Exception ex)
        {
            File.AppendAllText(file, string.Format("Type: {0} \r\n", ex.GetType().ToString()));
            File.AppendAllText(file, string.Format("Message: {0} \r\n",ex.Message));
            File.AppendAllText(file, string.Format("InnerException: {0} \r\n", ex.InnerException));
            File.AppendAllText(file, string.Format("Source: {0} \r\n", ex.Source));
            File.AppendAllText(file, string.Format("TargetSite: {0} \r\n", ex.TargetSite));
            File.AppendAllText(file, string.Format("StackTrace: {0} \r\n", ex.StackTrace));            
        }
    }
}
