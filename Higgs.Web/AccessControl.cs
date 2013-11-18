using System.Collections.Generic;
using System.Linq;

namespace Higgs.Web
{
    public static class AccessControl
    {
        public static List<ActionTypePattern> CreateActionPatterns = new List<ActionTypePattern>
        {
            new ActionTypePattern(NamePattern.Between, "Create List"), 
            new ActionTypePattern(NamePattern.Between, "Create Detail"),
            new ActionTypePattern(NamePattern.Between, "Add List"), 
            new ActionTypePattern(NamePattern.Between, "Add Detail"), 
            new ActionTypePattern(NamePattern.Between, "Insert List"), 
            new ActionTypePattern(NamePattern.Between, "Insert Detail"), 
            new ActionTypePattern("Create"), 
            new ActionTypePattern("Add"), 
            new ActionTypePattern("Insert")
        };
        public static List<ActionTypePattern> DeleteActionPatterns = new List<ActionTypePattern>
        {
            new ActionTypePattern(NamePattern.Between, "Delete List"), 
            new ActionTypePattern(NamePattern.Between, "Delete Detail"), 
            new ActionTypePattern(NamePattern.Between, "Remove List"), 
            new ActionTypePattern(NamePattern.Between, "Remove Detail"), 
            new ActionTypePattern("Delete"), new ActionTypePattern("Remove")
        };
        public static List<ActionTypePattern> OtherActionPatterns = new List<ActionTypePattern>();
        public static List<ActionTypePattern> ReadActionPatterns = new List<ActionTypePattern>
        {
            new ActionTypePattern((actionName, keyword) => actionName.StartsWith("Get") && actionName.Contains("LookupDataBy"), (actionName, keyword) => actionName.Substring(3, actionName.IndexOf("LookupDataBy") - 3)), 
            new ActionTypePattern((actionName, keyword) => actionName.StartsWith("Get") && actionName.Contains("By"), (actionName, keyword) => actionName.Substring(3, actionName.IndexOf("By") - 3)), 
            new ActionTypePattern(NamePattern.Between, "Get SummaryData"), 
            new ActionTypePattern(NamePattern.Between, "Get DetailData"), 
            new ActionTypePattern(NamePattern.Between, "Get ListData"), 
            new ActionTypePattern(NamePattern.Between, "Get Detail"), 
            new ActionTypePattern(NamePattern.Between, "Get Data"), 
            new ActionTypePattern(NamePattern.Between, "Get List"), 
            new ActionTypePattern(NamePattern.Between, "Get Suggestion"), 
            new ActionTypePattern("Open"), 
            new ActionTypePattern("View"),
            new ActionTypePattern("Get"), 
            new ActionTypePattern("Download"), 
            new ActionTypePattern(NamePattern.EndWith, "Dialog")
        };
        public static List<ActionTypePattern> UpdateActionPatterns = new List<ActionTypePattern> { new ActionTypePattern(NamePattern.Between, "Edit List"), new ActionTypePattern(NamePattern.Between, "Edit Detail"), new ActionTypePattern(NamePattern.Between, "Update List"), new ActionTypePattern(NamePattern.Between, "Update Detail"), new ActionTypePattern(NamePattern.Between, "Set List"), new ActionTypePattern(NamePattern.Between, "Set Detail"), new ActionTypePattern(NamePattern.Between, "Upload List"), new ActionTypePattern(NamePattern.Between, "Upload Detail"), new ActionTypePattern("Edit"), new ActionTypePattern("Update"), new ActionTypePattern("Set"), new ActionTypePattern("Upload") };
        
        public static AccessType GetAccessType(string actionName, out ActionTypePattern pattern)
        {
            if (IsMatch(OtherActionPatterns, actionName, out pattern))
            {
                return AccessType.Custom;
            }
            if (IsMatch(DeleteActionPatterns, actionName, out pattern))
            {
                return AccessType.Delete;
            }
            if (IsMatch(CreateActionPatterns, actionName, out pattern))
            {
                return AccessType.Create;
            }
            if (IsMatch(UpdateActionPatterns, actionName, out pattern))
            {
                return AccessType.Update;
            }
            if (!IsMatch(ReadActionPatterns, actionName, out pattern))
            {
                pattern = new ActionTypePattern(ActionTypePattern.MatchAllFn, ActionTypePattern.GetSameActionGroupName);
            }

            return AccessType.Read;
        }

        public static ActionInfo GetActionInfo(string actionName)
        {
            ActionTypePattern pattern;
            var accessType = GetAccessType(actionName, out pattern);

            return new ActionInfo
            {
                ActionName = actionName, 
                ActionGroupName = pattern.GetActionGroupNameFn(actionName, pattern.Keyword), 
                Type = accessType
            };
        }

        public static bool IsMatch(List<ActionTypePattern> patterns, string actionName, out ActionTypePattern matchedPattern)
        {
            var result = from p in patterns
                             where p.IsMatchFn(actionName, p.Keyword)
                             select p;

            foreach (var pattern in result)
            {
                matchedPattern = pattern;
                return true;
            }
            matchedPattern = null;

            return false;
        }
    }
}
