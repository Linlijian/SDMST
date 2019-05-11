using DataAccess;
using DataAccess.SEC;
using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using UtilityLib;
using WEBAPP.Helper;

namespace WEBAPP.Areas.SEC.Controllers
{
    public class SECS02P002Controller : SECBaseController
    {
        #region Property
        private SECS02P002Model localModel = new SECS02P002Model();
        private SECS02P002Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new SECS02P002Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as SECS02P002Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private SECS02P002Model TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new SECS02P002Model();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as SECS02P002Model;
            }
            set { TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = value; }
        }

        #endregion

        #region View
        public ActionResult Index()
        {
            SetDefaulButton(StandardButtonMode.Index);

            //AddButton(StandButtonType.ButtonAjax, "report", "Report", cssClass: "std-btn-print", iconCssClass: FaIcons.FaPrint, url: Url.Action("ViewReport"));

            if (TempSearch.IsDefaultSearch && !Request.GetRequest("page").IsNullOrEmpty())
            {
                localModel = TempSearch.CloneObject();
            }
            SetDefaultData(StandardActionName.Index);

            return View(StandardActionName.Index, localModel);
        }

        [RuleSetForClientSideMessages("Add")]
        public ActionResult Add()
        {
            SetDefaulButton(StandardButtonMode.Create);
            SetDefaultData(StandardActionName.Add);
            return View(StandardActionName.Add, localModel);
        }

        [RuleSetForClientSideMessages("Edit")]
        public ActionResult Edit(string USER_ID)
        {
            SetDefaulButton(StandardButtonMode.Modify);

            var da = new SECS02P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS02P002ExecuteType.GetByID;
            TempModel.USER_ID = da.DTO.Model.USER_ID = USER_ID;
            da.Select(da.DTO);
            if (da.DTO.Model != null)
            {
                localModel = da.DTO.Model;
                localModel.APP_CODE = da.DTO.Model.COM_CODE;
                TempModel.COM_CODE = da.DTO.Model.COM_CODE;
            }

            SetDefaultData(StandardActionName.Edit);

            return View(StandardActionName.Edit, localModel);
        }

        [HttpGet]
        public ActionResult Info(string USER_ID)
        {
            SetDefaulButton(StandardButtonMode.View);
            var da = new SECS02P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = DTOExecuteType.GetByID;
            TempModel.USER_ID = da.DTO.Model.USER_ID = USER_ID;
            da.Select(da.DTO);
            if (da.DTO.Model != null)
            {
                localModel = da.DTO.Model;
            }
            return View(StandardActionName.Info, localModel);
        }
        #endregion

        #region Action 
        public ActionResult Search(SECS02P002Model model)
        {
            var da = new SECS02P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS02P002ExecuteType.GetQuerySearchAll;
            if (Request.GetRequest("page").IsNullOrEmpty())
            {
                model.IsDefaultSearch = true;
                TempSearch = model;
            }
            da.DTO.Model = TempSearch;

            da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;

            da.Select(da.DTO);
            return JsonAllowGet(da.DTO.Models, da.DTO.Result);
        }

        [HttpPost]
        public ActionResult DeleteSearch(List<SECS02P002Model> data)
        {
            var jsonResult = new JsonResult();
            if (data != null && data.Count > 0)
            {
                var result = SaveData(StandardActionName.Delete, data);
                jsonResult = Success(result, StandardActionName.Delete);
            }
            else
            {
                jsonResult = ValidateError(StandardActionName.Delete, new ValidationError("", Translation.CenterLang.Center.DataNotFound));
            }
            return jsonResult;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCreate(SECS02P002Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                model.COM_CODE = SessionHelper.SYS_COM_CODE;

                var result = SaveData(StandardActionName.SaveCreate, model);
                jsonResult = Success(result, StandardActionName.SaveCreate, Url.Action(StandardActionName.Index, new { page = 1 }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveCreate);
            }
            return jsonResult;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveModify(SECS02P002Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {                
                var result = SaveData(StandardActionName.SaveModify, model);
                jsonResult = Success(result, StandardActionName.SaveModify, Url.Action(StandardActionName.Index, new { page = 1 }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveModify);
            }
            return jsonResult;
        }

        public ActionResult CustomsExport(SECS02P002Model model)
        {
            var da = new SECS02P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS02P002ExecuteType.GetQuerySearchAll;

            da.Select(da.DTO);

            if (da.DTO.Models != null)
            {
                ExportHelper.ExportExcel(Response, da.DTO.Models);
            }

            return Content("ExportExcel clicked");
        }

        public ActionResult ViewReport(SECS02P002Model model)
        {
            string ReportName = "J03FWL007R01";
            return Content("http://" + AppConfigHelper.ReportServerName + "/ReportServer?/" + AppConfigHelper.ReportFolderName + "/" + ReportName + "&rs:Command=Render&rs:Format=HTML4.0&rc:Parameters=false" + "&company_name=AutoAlliance%20(Thailand)%20Co.,Ltd.&user_id=vsmadmin&DateFromHead=21+%e0%b8%98%e0%b8%b1%e0%b8%99%e0%b8%a7%e0%b8%b2%e0%b8%84%e0%b8%a1+2558&DateToHead=25+%e0%b8%98%e0%b8%b1%e0%b8%99%e0%b8%a7%e0%b8%b2%e0%b8%84%e0%b8%a1+2558&error_code=0&WARE_HOUSE_FT=AAT&PE_HID=25&PE_NAME=2015-DEC%20PERIOD%204%20TO%20AAT&PE_START_DATE=2015-12-21&PE_END_DATE=2015-12-25");
        }

        public ActionResult BindDetailAdd()
        {
            var da = new SECS02P002DA();
            SetStandardErrorLog(da.DTO);

            return JsonAllowGet(da.DTO.Model.Details);
        }
        public ActionResult BindDetail()
        {
            var da = new SECS02P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS02P002ExecuteType.GetDetailByID;
            da.DTO.Model.USER_ID = TempModel.USER_ID;
            

            da.SelectNoEF(da.DTO);
            return JsonAllowGet(da.DTO.Model.Details);
        }
        public ActionResult CheckAdmin(SECS02P002Model model)
        {
            var jsonResult = new JsonResult();

            var da = new SECS02P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS02P002ExecuteType.CheckAdmin;
            da.DTO.Model.USG_ID = model.USG_ID;
            da.SelectNoEF(da.DTO);

            jsonResult = Success(da.DTO.Result, StandardActionName.Add);

            return JsonAllowGet(da.DTO.Model);
            //return jsonResult;
        }
        [HttpPost]
        public ActionResult DeleteDetails(List<SECS02P002Model> data)
        {
            var jsonResult = new JsonResult();
            if (data != null && data.Count > 0)
            {
                var result = SaveData("DeleteDetails", data);
                if (result.IsResult)
                {
                    jsonResult = Success(result, StandardActionName.Delete);
                }
                else
                {
                    jsonResult = ValidateError(StandardActionName.Delete, new ValidationError("", result.ResultMsg));
                }

                return JsonAllowGet(result);
            }
            else
            {
                jsonResult = ValidateError(StandardActionName.Delete, new ValidationError("", Translation.CenterLang.Center.DataNotFound));
            }
            return jsonResult;
        }
        #endregion

        #region ====Private Mehtod====
        private DTOResult SaveData(string mode, object model)
        {
            var da = new SECS02P002DA();

            //ในกรณีที่มีการ SaveLog ให้ Include SetStandardLog ด้วย
            SetStandardLog(
              da.DTO,
              model,
              GetSaveLogConfig("dbo", "VSMS_USER", "USER_ID"));


            if (mode == StandardActionName.SaveCreate)
            {
                SetStandardField(model);
                da.DTO.Model = (SECS02P002Model)model;
               // SetStandardField(da.DTO.Model.ComUserModel);
                da.InsertNoEF(da.DTO);
            }
            else if (mode == StandardActionName.SaveModify)
            {
                SetStandardField(model);               
                da.DTO.Model = (SECS02P002Model)model;
                da.DTO.Model.COM_CODE = TempModel.COM_CODE;
                //SetStandardField(da.DTO.Model.ComUserModel);
                da.UpdateNoEF(da.DTO);
            }
            else if (mode == "DeleteDetails")
            {
                da.DTO.Model = new SECS02P002Model();
                da.DTO.Models = (List<SECS02P002Model>)model;
                da.DTO.Execute.ExecuteType = SECS02P002ExecuteType.DeleteDetail;
                da.Delete(da.DTO);
            }
            else if (mode == StandardActionName.Delete)
            {
                da.DTO.Model = new SECS02P002Model();
                da.DTO.Models = (List<SECS02P002Model>)model;
                da.DTO.Execute.ExecuteType = SECS02P002ExecuteType.Delete;
                da.DeleteNoEF(da.DTO);
            }
            return da.DTO.Result;
        }

        private void SetDefaultData(string mode = "")
        {
            if (mode == StandardActionName.Index)
            {
                localModel.IS_DISABLED_MODEL = BindIS_DISABLED_MODEL();
                //localModel.DEPT_ID_MODEL = BindDEPT_ID_MODEL();
                localModel.USG_ID_MODEL = BindUSG_ID_MODEL();

            }
            else if (mode == StandardActionName.Add || mode == StandardActionName.Edit)
            {
                localModel.IS_DISABLED_MODEL = BindIS_DISABLED_MODEL();
                localModel.APP_CODE_MODEL = BindAPP_CODE();
                localModel.USG_ID_MODEL = BindUSG_ID_MODEL_ADD();
                localModel.TITLE_ID_MODEL = BindTITLE_ID_MODEL();
                localModel.USER_STATUS_MODEL = BindUSER_STATUS_MODEL();
                localModel.USER_ID_MODEL = BindUserId();

                Set(localModel);
            }
        }

        private void Set(SECS02P002Model model)
        {
            foreach (var item in model.USER_STATUS_MODEL)
            {
                item.Value = item.Value.Trim();
            }
        }

        private List<DDLCenterModel> BindIS_DISABLED_MODEL()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_OPTIONDETAIL_002, new VSMParameter(FIXOptionID.FixOpt_71));
        }

        private List<DDLCenterModel> BindDEPT_ID_MODEL()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_DEPARTMENT_001, new VSMParameter(SessionHelper.SYS_COM_CODE));
        }

        private List<DDLCenterModel> BindUSG_ID_MODEL()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_USRGROUP_001);
        }
        private List<DDLCenterModel> BindUSG_ID_MODEL_ADD()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_USRGROUP_002);
            //var da = new SECS02P002DA();
            //SetStandardErrorLog(da.DTO);
            //da.DTO.Execute.ExecuteType = SECS02P002ExecuteType.GetQueryCheckUserAdmin;
            //da.DTO.Model.USER_ID = SessionHelper.SYS_USER_ID;
            //da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;
            //da.SelectNoEF(da.DTO);

            //if (da.DTO.Models[0].USG_LEVEL.AsString() == "S")
            //{
            //    return GetDDLCenter(DDLCenterKey.DD_VSMS_USRGROUP_001);
            //}
            //else
            //{//
            //    return GetDDLCenter(DDLCenterKey.DD_VSMS_USRGROUP_002);
            //}
        }
        private List<DDLCenterModel> BindUserId()
        {
            return GetDDLCenter(DDLCenterKey.DD_SECS01P001_001);
        }
        private List<DDLCenterModel> BindTITLE_ID_MODEL()
        {
            //return GetDDLCenter(DDLCenterKey.DD_VSMS_TITLE_001, new VSMParameter(SessionHelper.SYS_COM_CODE));
            return GetDDLCenter(DDLCenterKey.DD_VSMS_TITLE_001);
        }

        private List<DDLCenterModel> BindAPP_CODE()
        {
            //return GetDDLCenter(DDLCenterKey.DD_VSMS_TITLE_001, new VSMParameter(SessionHelper.SYS_COM_CODE));
            return GetDDLCenter(DDLCenterKey.DD_VSMS_COMPANY_001);
        }

        private List<DDLCenterModel> BindUSER_STATUS_MODEL()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_OPTIONDETAIL_002, new VSMParameter(FIXOptionID.FixOpt_89));
        }
        #endregion
    }
}