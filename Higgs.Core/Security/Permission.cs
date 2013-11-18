using System;

namespace Higgs.Core.Security
{
    public class Permission
    {
        public Guid? RoleId { get; set; }
        public string GroupName { get; set; }
        public string ActionName { get; set; }
        public byte? ActionType { get; set; }
        public bool IsGrant { get; set; }
    }
}
