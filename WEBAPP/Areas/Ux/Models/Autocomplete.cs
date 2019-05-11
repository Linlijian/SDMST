using System.Collections.Generic;

namespace WEBAPP.Areas.Ux.Models
{
    public class Autocomplete
    {
        //$page = $_GET['page']; // get the requested page
        //09.
        //$limit = $_GET['rows']; // get how many rows we want to have into the grid
        //10.
        //$sidx = $_GET['sidx']; // get index row - i.e. user click to sort
        //11.
        //$sord = $_GET['sord']; // get the direction
        //12.
        //$searchTerm = $_GET['searchTerm'];

        public string page { get; set; }
        public string rows { get; set; }
        public string sidx { get; set; }
        public string sord { get; set; }
        public string searchTerm { get; set; }

        public class Students
        {
            public int StudentID;
            public string StudentName;
            public string Address;
            public string City;
        }

        public class Result
        {
            public int pageIndex { get; set; }
            public int totalcount { get; set; }
            public int pageSize { get; set; }
            public List<ColumModel> colModel { get; set; }
            public List<Dictionary<object,string>> rows { get; set; }

        }

        public class ColumModel
        {
            
            public string ColumnName { get; set; }
            public string HeaderName { get; set; }
            public string width { get; set; }
        }

        public static string GetStrauture(string key = null)
        {
            return "";
        }
    }



}