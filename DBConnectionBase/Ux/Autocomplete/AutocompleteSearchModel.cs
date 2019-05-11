namespace DataAccess.Ux
{
    public class AutocompleteSearchModel
    {
        public string data { get; set; }
        public string name { get; set; }
        public string searchable { get; set; }
        public string orderable { get; set; }
        public AutocompleteSearchValueModel search { get; set; }
    }

    public class AutocompleteSearchValueModel
    {
        public string value { get; set; }
        public string regex { get; set; }
    }
}
