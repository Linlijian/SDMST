using System;
using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.Admin.Dashboard
{
    public class DashboardDA : BaseDA
    {
        public DashboardDTO DTO { get; set; }

        public DashboardDA()
        {
            DTO = new DashboardDTO();
        }


        #region ====Select==========
        protected override BaseDTO DoSelect(BaseDTO DTO)
        {
            var dto = (DashboardDTO)DTO;


            switch (dto.Execute.ExecuteType)
            {
                case DashboardExecuteType.GetQueryAllGrdUser:
                    return GetAllGrdUser(dto);
                case DashboardExecuteType.GetQueryAllGrdManage:
                    return GetAllGrdManage(dto);
                case DashboardExecuteType.GetTreeView01adm:
                    return GetTreeView01adm(dto);
            }


            return dto;
        }

        private DashboardDTO GetAllGrdUser(DashboardDTO dto)
        {
            //var parameters = CreateParameter();

            //parameters.AddRefCursorParameter("RecordSet");
            //parameters.AddRefCursorParameter("RecordSet2");

            //var result = _DBManger.ExecuteDataSet("PKG_SEC_DASHBOARD.SP_QUERY_ALL", parameters);
            //if (result.Success(dto))
            //{
            //    dto.Models = result.OutputDataSet.Tables[0].ToList<DashboardModel>();
            //}
            return dto;
        }
        private DashboardDTO GetAllGrdManage(DashboardDTO dto)
        {
            //var parameters = CreateParameter();

            //parameters.AddRefCursorParameter("RecordSet");
            //parameters.AddRefCursorParameter("RecordSet2");

            //var result = _DBManger.ExecuteDataSet("PKG_SEC_DASHBOARD.SP_QUERY_ALL", parameters);
            //if (result.Success(dto))
            //{
            //    dto.Models = result.OutputDataSet.Tables[0].ToList<DashboardModel>();
            //}
            return dto;
        }
        private DashboardDTO GetTreeView01adm(DashboardDTO dto)
        {
            //var parameters = CreateParameter();

            //parameters.AddRefCursorParameter("RECORDSET");

            //parameters.AddParameter("pMS_CORP_ID", dto.Model.MS_CORP_ID);

            //var result = _DBManger.ExecuteDataSet("PKG_IVW0_IVAW00100.SP_TREEVIEW_01_ADM", parameters);
            //if (result.Success(dto))
            //{
            //    dto.Model.TreeView01admModel = result.OutputDataSet.Tables[0].ToList<TreeModel>().ToTreeView();
            //}
            return dto;
        }
        #endregion
    }
}