namespace TeamPackage
{
    public class RemoteConfig
    {
        public ConfigVersionData UpdateVersion;
    }

    public class ConfigVersionData
    {
        public bool IsUpdate;
        public bool IsForceUpdate;
        public string UpdateTitle;
        public string UpdateContext;
        public string UpdateUrl;
    }

}