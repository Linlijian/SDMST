using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess
{
    public class GridOrderModel
    {
        public int column { get; set; }
        public string dir { get; set; }
    }
}
