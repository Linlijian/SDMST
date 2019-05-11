using System.Collections.Generic;
using System.Web.Mvc;
using DataAccess;
using DataAccess.SEC;
using WEBAPP.Helper;
using UtilityLib;

namespace WEBAPP.Areas.SEC.Controllers
{
    public class SECS01P005Controller : SECBaseController
    {
        #region Property
        private SECS01P005Model localModel = new SECS01P005Model();
        private SECS01P005Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new SECS01P005Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as SECS01P005Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private SECS01P005Model TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new SECS01P005Model();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as SECS01P005Model;
            }
            set { TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = value; }
        }
        #endregion

        #region View
        public ActionResult Index()
        {
            SetDefaulButton(StandardButtonMode.Index);

            RemoveStandardButton(StandardButtonName.Add);
            RemoveStandardButton(StandardButtonName.DeleteSearch);

            if (TempSearch.IsDefaultSearch && !Request.GetRequest("page").IsNullOrEmpty())
            {
                localModel = TempSearch;
            }
            localModel.COM_CODE_MODEL = GetDDLCenter(DDLCenterKey.DD_VSMS_COMPANY_001);
            localModel.SYS_CODE_MODEL = GetDDLCenter(DDLCenterKey.DD_VSMS_SYSTEM_001);
            return View(StandardActionName.Index, localModel);
        }

        [HttpGet]
        public ActionResult Edit(string COM_CODE, string SYS_CODE)
        {
            SetDefaulButton(StandardButtonMode.Modify);
            localModel = new SECS01P005Model();
            TempModel.COM_CODE = localModel.COM_CODE = COM_CODE;
            TempModel.SYS_CODE = localModel.SYS_CODE = SYS_CODE;
            localModel.COM_CODE_MODEL = GetDDLCenter(DDLCenterKey.DD_VSMS_COMPANY_001);
            localModel.SYS_CODE_MODEL = GetDDLCenter(DDLCenterKey.DD_VSMS_SYSTEM_001, new VSMParameter(COM_CODE));
            return View(StandardActionName.Edit, localModel);
        }
        #endregion

        #region Action
        public ActionResult Search(SECS01P005Model model)
        {
            var da = new SECS01P005DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS01P005ExecuteType.GetAll;
            if (Request.GetRequest("page").IsNullOrEmpty())
            {
                model.IsDefaultSearch = true;
                TempSearch = model;
            }
            da.DTO.Model = TempSearch;
            da.Select(da.DTO);
            return JsonAllowGet(da.DTO.Models);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveModify(List<SECS01P005Model> SysPrg)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                var result = SaveData(StandardActionName.SaveModify, SysPrg);
                jsonResult = Success(result, StandardActionName.SaveModify, Url.Action(StandardActionName.Index, new { page = 1 }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveModify);
            }
            return jsonResult;
        }
        #endregion

        #region Event
        public ActionResult GetSystem(string COM_CODE)
        {
            return JsonAllowGet(GetDDLCenter(DDLCenterKey.DD_VSMS_SYSTEM_001, new VSMParameter(COM_CODE)));
        }
        public ActionResult GetProgram()
        {
            var da = new SECS01P005DA();
            da.DTO.Execute.ExecuteType = SECS01P005ExecuteType.GetProgram;
            da.DTO.Model = TempModel.CloneObject();
            da.SelectNoEF(da.DTO);
            return JsonAllowGet(da.DTO.Models);
        }
        public ActionResult GetSysPrg()
        {
            var da = new SECS01P005DA();
            da.DTO.Execute.ExecuteType = SECS01P005ExecuteType.GetSysPrg;
            da.DTO.Model = TempModel.CloneObject();
            da.Select(da.DTO);
            return JsonAllowGet(da.DTO.Models);
        }
        #endregion

        #region Method
        private DTOResult SaveData(string mode, object model)
        {
            var da = new SECS01P005DA();
            //ในกรณีที่มีการ SaveLog ให้ Include SetStandardLog ด้วย
            SetStandardLog(
               da.DTO,
               model,
               GetSaveLogConfig("dbo", "VSMS_SYS_PGC", "COM_CODE", "SYS_CODE"));

            if (mode == StandardActionName.SaveModify)
            {
                SetStandardField(model);
                da.DTO.Models = (List<SECS01P005Model>)model;
                da.DTO.Model.COM_CODE = TempModel.COM_CODE;
                da.DTO.Model.SYS_CODE = TempModel.SYS_CODE;
                da.Update(da.DTO);
            }
            return da.DTO.Result;
        }
        #endregion
    }
}