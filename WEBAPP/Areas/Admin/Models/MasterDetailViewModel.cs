using System;
using System.Collections.Generic;

namespace WEBAPP.Areas.Admin.Models
{

    public class MasterModel
    {
        public int Hid { get; set; }
        public string Name { get; set; }
        IEnumerable<DetailModel> Detail { get; set; }
    }

    public class DetailModel
    {
        public int DId { get; set; }
        public int Hid { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

    }


}