namespace Higgs.Web
{
    public class ActionTypePattern
    {
        public static GetActionGroupNameHandler GetBetweenActionGroupName;
        public static GetActionGroupNameHandler GetContainActionGroupName = (actionName, keyword) => actionName.Replace(keyword, "");
        public static GetActionGroupNameHandler GetEndWithActionGroupName = (actionName, keyword) => actionName.Substring(0, actionName.Length - keyword.Length);
        public static GetActionGroupNameHandler GetSameActionGroupName = (actionName, keyword) => actionName;
        public static GetActionGroupNameHandler GetStartWithActionGroupName = (actionName, keyword) => actionName.Substring(keyword.Length);

        public static IsMatchHandler IsBetweenFn = (actionName, keyword) => actionName.StartsWith(keyword.Split(new [] { ' ' })[0]) && actionName.EndsWith(keyword.Split(new [] { ' ' })[1]);
        public static IsMatchHandler IsContainFn = (actionName, keyword) => actionName.StartsWith(keyword);
        public static IsMatchHandler IsEndWithFn = (actionName, keyword) => actionName.StartsWith(keyword);
        public static IsMatchHandler IsStartWithFn = (actionName, keyword) => actionName.StartsWith(keyword);
        public static IsMatchHandler MatchAllFn = (actionName, keyword) => true;

        static ActionTypePattern()
        {
            GetBetweenActionGroupName = delegate(string actionName, string keyword)
            {
                var strArray = keyword.Split(new [] {' '});
                var str = strArray[0];
                var str2 = strArray[1];

                return actionName.Substring(str.Length, (actionName.Length - str.Length) - str2.Length);
            };
        }

        public ActionTypePattern(string keyword) : this(NamePattern.StartWith, keyword)
        {
        }

        public ActionTypePattern(NamePattern pattern, string keyword)
        {
            Pattern = pattern;
            Keyword = keyword;
            switch (pattern)
            {
                case NamePattern.StartWith:
                    IsMatchFn = IsStartWithFn;
                    GetActionGroupNameFn = GetStartWithActionGroupName;
                    return;

                case NamePattern.EndWith:
                    IsMatchFn = IsEndWithFn;
                    GetActionGroupNameFn = GetEndWithActionGroupName;
                    return;

                case NamePattern.Between:
                    IsMatchFn = IsBetweenFn;
                    GetActionGroupNameFn = GetBetweenActionGroupName;
                    return;
            }

            IsMatchFn = IsContainFn;
            GetActionGroupNameFn = GetContainActionGroupName;
        }

        public ActionTypePattern(IsMatchHandler getAccessTypeFn, GetActionGroupNameHandler getActionGroupNameFn, string keyword = null)
        {
            IsMatchFn = getAccessTypeFn;
            GetActionGroupNameFn = getActionGroupNameFn;
            Keyword = keyword;
        }

        public GetActionGroupNameHandler GetActionGroupNameFn { get; set; }
        public IsMatchHandler IsMatchFn { get; set; }
        public string Keyword { get; set; }
        public NamePattern? Pattern { get; set; }

        public delegate string GetActionGroupNameHandler(string actionName, string keyword);
        public delegate bool IsMatchHandler(string actionName, string keyword);
    }
}