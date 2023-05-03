namespace MTWServerApiClient
{
    public static class Configuration
    {
        //public static string BaseHost { get; private set; } = "http://192.168.6.129/";
        //public static string BaseHost { get; private set; } = "http://192.168.6.129/";
        //public static string BaseHost { get; set; }   = "https://localhost:7139/";
        public static string BaseHost { get; set; }   = "http://localhost:5139/";
        //public static string BaseHost { get; set; } = "http://localhost:5000/";
        //public static string BaseHost { get;  set; } = "http://10.1.1.31/";
        //public static string BaseHost { get;  set; } = "http://10.1.1.32/";
        //public static string BaseHost { get;  set; } = "http://172.16.2.218/";
        public static string EquipmentHost { get; private set; } = BaseHost + "Equipments";
        public static string GroupHost { get; private set; } = BaseHost + "Groups";
        public static string UserHost { get; private set; } = BaseHost + "Users";
        public static string ProfileHost { get; private set; } = BaseHost + "api/Profiles";
        public static string LprHost { get; private set; } = BaseHost + "api/lprs";
        public static string LprRecordHost { get; private set; } = BaseHost + "api/lprRecords";
        public static string RecordHost { get; internal set; } = BaseHost + "api/Records";
        public static string SessionHost { get; internal set; } = BaseHost + "api/Sessions";
        public static string ServerHost { get; internal set; } = BaseHost + "api/Servers";
        public static string DvcHost { get; internal set; } = BaseHost + "api/DVCs";
    }
}
