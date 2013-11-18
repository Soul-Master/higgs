namespace Higgs.Web
{
    public enum AccessType : byte
    {
        Create = 2,
        Custom = 0xff,
        Delete = 3,
        Read = 0,
        Update = 1
    }
}
