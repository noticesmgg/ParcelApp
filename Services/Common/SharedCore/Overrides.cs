using SharedCore.Utilities;

namespace SharedCore
{
    public static class Overrides
    {
        private static int uniqueId;
        public static String? AppName { get; set; }
        public static String? LogFileName { get; set; }
        public static String? LogFilePath { get; set; }
        public static bool LogToConsole { get; set; } = false;
        public static String? SQLConnectionString { get; set; }
        public static int JobId { get; set; }
        public static String? JobName { get; set; }
        public static DateTime AsOfdate { get; set; } = DateTime.Now.Date;
        public static EnvironmentType Environment { get; set; } = EnvironmentType.Local;

        public static int GetUniqueID()
        {
            if(uniqueId == 0)
            {
                uniqueId = int.Parse($"{DateTime.Now.ToString("yyMMddhh")}") + 60;
            }
            return uniqueId++;
        }

        public enum EnvironmentType
        {
            Development,
            UAT,
            Production,
            Local
        }
    }
}
