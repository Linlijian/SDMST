using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess
{
    public class StandardGridServerModel : StandardModel
    {
        public int recordsTotal { get; set; }
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public int DT_RowId { get; set; }
        public List<GridOrderModel> order { get; set; }
        public List<GridColumnModel> columns { get; set; }
        
    }
}
