
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.Admin.Dashboard
{
    [Serializable]
    public class DashboardDTO : BaseDTO
    {
        public DashboardDTO()
        {
            Model = new DashboardModel();
            Models = new List<DashboardModel>();
        }

        public DashboardModel Model { get; set; }
        public List<DashboardModel> Models { get; set; }

        [DefaultValue(0)]
        public int TotalRows { get; set; }
    }

    public class DashboardExecuteType : DTOExecuteType
    {
        public const string GetQueryAllGrdUser = "GetQueryAllGrdUser";
        public const string GetQueryAllGrdManage = "GetQueryAllGrdManage";

        public const string GetTreeView01adm = "GetTreeView01adm";
    }
}