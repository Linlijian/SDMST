using DataAccess;
using System.Web.Mvc;
using WEBAPP.Helper;
using DataAccess.Admin.Dashboard;

namespace WEBAPP.Areas.Admin.Controllers
{
    public class DashboardController : AdminBaseController
    {
        #region properties
        private DashboardModel localModel = new DashboardModel();

        private DashboardModel TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new DashboardModel();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as DashboardModel;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private DashboardModel TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new DashboardModel();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as DashboardModel;
            }
            set { TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = value; }
        }
        #endregion

        #region View
        public ActionResult Index()
        {
            SetDefaultData(StandardActionName.Index);

            return View();
        }
        #endregion

        #region Action
        public ActionResult BindGridUser()
        {
            var da = new DashboardDA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = DashboardExecuteType.GetQueryAllGrdUser;

            da.Select(da.DTO);
            return JsonAllowGet(da.DTO.Models, da.DTO.Result);
        }

        public ActionResult BindGridManageCertificate()
        {
            var da = new DashboardDA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = DashboardExecuteType.GetQueryAllGrdManage;

            da.Select(da.DTO);
            return JsonAllowGet(da.DTO.Models, da.DTO.Result);
        }

        public ActionResult SearchTreeView01adm()
        {
            var da = new DashboardDA();
            //SetStandardErrorLog(da.DTO);
            //da.DTO.Execute.ExecuteType = DashboardExecuteType.GetTreeView01adm;

            //da.DTO.Model.MS_CORP_ID = SessionHelper.SYS_MS_CORP_ID;

            //da.Select(da.DTO);

            return JsonAllowGet(da.DTO.Model.TreeView01admModel, da.DTO.Result);
        }
        #endregion

        #region Method
        private void SetDefaultData(string mode = "")
        {
            //localModel.APP_GROUP_CODE_MODEL = BindAPP_GROUP_CODE(mode);
            //localModel.STATUS_MODEL = BindAPP_STATUS(mode);
            //localModel.ACTIVE_MODEL = BindACTIVE(mode);
            @ViewBag.PRG_ACTION = "";
            @ViewBag.PRG_CONTROLLER = "";
        }
        #endregion
    }
}