namespace Higgs.Web.Controls.JqGrid
{
    /// <summary>
    /// The supported operations in where-extension
    /// </summary>
    public enum WhereOperation
    {
        [StringValue("eq")]
        Equal,
        [StringValue("ne")]
        NotEqual,
        [StringValue("cn")]
        Contains,
        [StringValue("nc")]
        NotContains,
        [StringValue("bw")]
        BeginsWith,
        [StringValue("bn")]
        NotBeginsWith,
        [StringValue("ew")]
        EndsWith,
        [StringValue("en")]
        NotEndsWith,
        [StringValue("lt")]
        LessThan,
        [StringValue("le")]
        LessThanOrEqual,
        [StringValue("gt")]
        GreaterThan,
        [StringValue("ge")]
        GreaterThanOrEqual
    }
}