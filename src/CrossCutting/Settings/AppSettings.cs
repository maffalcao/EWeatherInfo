namespace CrossCutting.Settings
{
    public class AppSettings
    {
        public string UrlBase { get; set; }
        public string FolderBase { get; set; }
        public int MaxDaysFromNowToAFileBeCompressed { get; set; }
        public int DevicesMinutesToExpireInRedis { get; set; }
        public int MeasurementsMinutesToExpireInRedis { get; set; }

    }
}