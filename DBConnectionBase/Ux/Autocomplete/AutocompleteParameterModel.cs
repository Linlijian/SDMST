namespace DataAccess.Ux
{
    public class AutocompleteParameterModel
    {
        public string KeySource { get; set; }
        public string SearchTerm { get; set; }
        public string Sort { get; set; }
        public string ParameterValue { get; set; }
        public string ClientID { get; set; }
        public string FitterData { get; set; }

        public System.Collections.Generic.List<AutocompleteNotInParameterModel> DataNotIn { get; set; }
        public string CRET_BY { get; set; }
        public System.Nullable<System.DateTime> CRET_DATE { get; set; }
    }

    public class AutocompleteNotInParameterModel
    {
        public string DATA_VALUE { get; set; }
        public string COLUMN_NAME { get; set; }
    }
}
