using DataAccess;
using DataAccess.SEC;
using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UtilityLib;
using WEBAPP.Helper;

namespace WEBAPP.Areas.SEC.Controllers
{
    public class SECS02P001Controller : SECBaseController
    {
        #region Property
        private SECS02P001Model localModel = new SECS02P001Model();
        private SECS02P001Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new SECS02P001Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as SECS02P001Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private SECS02P001Model TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new SECS02P001Model();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as SECS02P001Model;
            }
            set { TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = value; }
        }
        #endregion

        #region View
        public ActionResult Index()
        {
            SetDefaulButton(StandardButtonMode.Index);

            if (TempSearch.IsDefaultSearch && !Request.GetRequest("page").IsNullOrEmpty())
            {
                localModel = TempSearch;
            }
            localModel.USG_STATUS_MODEL = GetUsgStatus();
            return View(StandardActionName.Index, localModel);
        }

        [HttpGet]
        [RuleSetForClientSideMessages("Add")]
        public ActionResult Add()
        {
            SetDefaulButton(StandardButtonMode.Create);
            localModel.COM_CODE = SessionHelper.SYS_COM_CODE;
            localModel.USG_STATUS_MODEL = GetUsgStatus();
            localModel.USG_LEVEL_MODEL = GetUsgLevel();
            return View(localModel);
        }

        [RuleSetForClientSideMessages("Edit")]
        public ActionResult Edit(string COM_CODE, decimal? USG_ID)
        {
            SetDefaulButton(StandardButtonMode.Modify);
            var da = new SECS02P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS02P001ExecuteType.GetByID;

            TempModel = null;
            TempModel.COM_CODE = da.DTO.Model.COM_CODE = COM_CODE;
            TempModel.USG_ID = da.DTO.Model.USG_ID = USG_ID;

            da.Select(da.DTO);

            if (da.DTO.Model != null)
            {
                localModel = da.DTO.Model;
            }

            localModel.USG_STATUS_MODEL = GetUsgStatus();
            localModel.USG_LEVEL_MODEL = GetUsgLevel();
            Set(localModel, "USG_LEVEL_MODEL");
            Set(localModel, "USG_STATUS_MODEL");

            return View(StandardActionName.Edit, localModel);
        }

        public ActionResult ConfPrg(string COM_CODE, decimal? USG_ID)
        {
            SetDefaulButton(StandardButtonMode.Other);
            AddStandardButton(StandardButtonName.SaveModify, url: Url.Action("SaveConfPrg"));
            var da = new SECS02P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS02P001ExecuteType.GetByID;

            TempModel = null;
            TempModel.COM_CODE = da.DTO.Model.COM_CODE = COM_CODE;
            TempModel.USG_ID = da.DTO.Model.USG_ID = USG_ID;

            da.Select(da.DTO);

            if (da.DTO.Model != null)
            {
                localModel = da.DTO.Model;
                TempModel.USG_LEVEL = da.DTO.Model.USG_LEVEL;
            }

            localModel.SYS_CODE_MODEL = GetDDLCenter(DDLCenterKey.DD_VSMS_SYSTEM_002);
            return View("ConfPrg", localModel);
        }
        public ActionResult ConfSysSeq(string COM_CODE, string USG_LEVEL, decimal? USG_ID)
        {
            SetDefaulButton(StandardButtonMode.Other);
            AddStandardButton(StandardButtonName.SaveModify, url: Url.Action("SaveConfSysSeq"));
            var da = new SECS02P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS02P001ExecuteType.GetByID;

            TempModel = null;
            TempModel.COM_CODE = da.DTO.Model.COM_CODE = COM_CODE;
            TempModel.USG_ID = da.DTO.Model.USG_ID = USG_ID;
            TempModel.USG_LEVEL = da.DTO.Model.USG_LEVEL = USG_LEVEL;

            da.Select(da.DTO);

            if (da.DTO.Model != null)
            {
                localModel = da.DTO.Model;
            }
            localModel.SYS_GROUP_NAME_MODEL = GetDDLCenter(DDLCenterKey.DD_VSMS_USRGROUP_003, new VSMParameter(COM_CODE.Trim()), new VSMParameter(USG_LEVEL));
            return View("ConfSysSeq", localModel);
        }
        public ActionResult ConfPrgSeq(string COM_CODE, string USG_LEVEL, decimal? USG_ID)
        {
            SetDefaulButton(StandardButtonMode.Other);
            AddStandardButton(StandardButtonName.SaveModify, url: Url.Action("SaveConfPrgSeq"));
            var da = new SECS02P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS02P001ExecuteType.GetByID;

            TempModel = null;
            TempModel.COM_CODE = da.DTO.Model.COM_CODE = COM_CODE;
            TempModel.USG_ID = da.DTO.Model.USG_ID = USG_ID;
            TempModel.USG_LEVEL = da.DTO.Model.USG_LEVEL = USG_LEVEL;

            da.Select(da.DTO);

            if (da.DTO.Model != null)
            {
                localModel = da.DTO.Model;
            }
            //localModel.SYS_CODE_MODEL = GetDDLCenter(DDLCenterKey.DD_VSMS_SYSTEM_002);
            localModel.SYS_GROUP_NAME_MODEL = GetDDLCenter(DDLCenterKey.DD_VSMS_USRGROUP_003, new VSMParameter(COM_CODE.Trim()), new VSMParameter(USG_LEVEL));
            return View("ConfPrgSeq", localModel);
        }
        #endregion

        #region Action
        public ActionResult Search(SECS02P001Model model)
        {
            var da = new SECS02P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS02P001ExecuteType.GetAll;

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
        public ActionResult SaveCreate(SECS02P001Model model, string button = "")
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                model.COM_CODE = SessionHelper.SYS_COM_CODE;
                var result = SaveData(StandardActionName.SaveCreate, model);
                jsonResult = Success(result, StandardActionName.SaveModify, Url.Action(StandardActionName.Index, new { page = 1 }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveModify);
            }
            return jsonResult;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveModify(SECS02P001Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                model.COM_CODE = TempModel.COM_CODE;
                model.USG_ID = TempModel.USG_ID;
                var result = SaveData(StandardActionName.SaveModify, model);
                jsonResult = Success(result, StandardActionName.SaveModify, Url.Action(StandardActionName.Index, new { page = 1 }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveModify);
            }
            return jsonResult;
        }

        [HttpPost]
        public ActionResult DeleteSearch(List<SECS02P001Model> data)
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
        public ActionResult SYS_CODE_OnChange(string SYS_CODE)
        {
            var da = new SECS02P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS02P001ExecuteType.GetUsgPriv;
            da.DTO.Model = TempModel.CloneObject();
            da.DTO.Model.SYS_CODE = SYS_CODE;
            da.DTO.Model.USG_LEVEL = TempModel.USG_LEVEL;
            da.SelectNoEF(da.DTO);
            return JsonAllowGet(da.DTO.Model.PRIV_MODEL);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveConfPrg(SECS02P001Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                model.COM_CODE = TempModel.COM_CODE;
                model.USG_LEVEL = TempModel.USG_LEVEL;
                model.USG_ID = TempModel.USG_ID;
                var result = SaveData("SaveConfPrg", model);
                if (result.IsResult && SessionHelper.SYS_USG_ID == TempModel.USG_ID)
                {
                    Session[SessionSystemName.SYS_MENU] = null;
                    var Menu = SessionHelper.SYS_MenuModel;
                }
                jsonResult = Success(result, StandardActionName.SaveModify, Url.Action(StandardActionName.Index, new { page = 1 }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveModify);
            }
            return jsonResult;
        }

        public ActionResult GetSysSeq(string SYS_GROUP_NAME)
        {
            var da = new SECS02P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS02P001ExecuteType.GetSysSeq;
            da.DTO.Model = TempModel.CloneObject();
            da.DTO.Model.SYS_GROUP_NAME = SYS_GROUP_NAME;
            da.SelectNoEF(da.DTO);

            return JsonAllowGet(da.DTO.Model.PRIV_MODEL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveConfSysSeq(SECS02P001Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                model.COM_CODE = TempModel.COM_CODE;
                model.USG_ID = TempModel.USG_ID;
                var result = SaveData("SaveConfSysSeq", model);
                if (result.IsResult && SessionHelper.SYS_USG_ID == TempModel.USG_ID)
                {
                    Session[SessionSystemName.SYS_MENU] = null;
                    var Menu = SessionHelper.SYS_MenuModel;
                }
                jsonResult = Success(result, StandardActionName.SaveModify, Url.Action("ConfSysSeq", new { page = 1, TempModel.COM_CODE, TempModel.USG_ID }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveModify);
            }
            return jsonResult;
        }

        public ActionResult GetPrgSeq(string SYS_GROUP_NAME, string SYS_CODE)
        {
            var da = new SECS02P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS02P001ExecuteType.GetPrgSeq;
            da.DTO.Model = TempModel.CloneObject();
            da.DTO.Model.SYS_GROUP_NAME = SYS_GROUP_NAME;
            da.DTO.Model.SYS_CODE = SYS_CODE;
            da.SelectNoEF(da.DTO);

            return JsonAllowGet(da.DTO.Model.PRIV_MODEL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveConfPrgSeq(SECS02P001Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                model.COM_CODE = TempModel.COM_CODE;
                model.USG_ID = TempModel.USG_ID;
                var result = SaveData("SaveConfPrgSeq", model);
                if (result.IsResult && SessionHelper.SYS_USG_ID == TempModel.USG_ID)
                {
                    Session[SessionSystemName.SYS_MENU] = null;
                    var Menu = SessionHelper.SYS_MenuModel;
                }
                jsonResult = Success(result, StandardActionName.SaveModify, Url.Action("ConfPrgSeq", new { page = 1, TempModel.COM_CODE, TempModel.USG_ID }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveModify);
            }
            return jsonResult;
        }

        public ActionResult BindSystemByName(string SYS_GROUP_NAME)
        {
            TempModel.COM_CODE = TempModel.COM_CODE.Trim();
            var model = GetDDLCenter(DDLCenterKey.DD_VSMS_USRGROUP_004, new VSMParameter(TempModel.USG_LEVEL), new VSMParameter(SYS_GROUP_NAME));
            return JsonAllowGet(model);
        }
        #endregion

        #region Method
        private DTOResult SaveData(string mode, object model)
        {
            var da = new SECS02P001DA();

            SetStandardLog(
               da.DTO,
               model,
               GetSaveLogConfig("dbo", "OIC_T_SEC_USRGROUP", "COM_CODE", "USG_CODE"));

            if (mode == StandardActionName.SaveCreate)
            {
                SetStandardField(model);
                da.DTO.Execute.ExecuteType = SECS02P001ExecuteType.InsertData;
                da.DTO.Model = (SECS02P001Model)model;
                da.InsertNoEF(da.DTO);
            }
            else if (mode == StandardActionName.SaveModify)
            {
                SetStandardField(model);
                da.DTO.Execute.ExecuteType = SECS02P001ExecuteType.UpdateData;
                da.DTO.Model = (SECS02P001Model)model;
                da.UpdateNoEF(da.DTO);
            }
            else if (mode == StandardActionName.Delete)
            {
                da.DTO.Model = new SECS02P001Model();
                SetStandardField(da.DTO.Model);
                da.DTO.Execute.ExecuteType = SECS02P001ExecuteType.DeleteData;
                da.DTO.Models = (List<SECS02P001Model>)model;
                da.Delete(da.DTO);
            }
            else if (mode == "SaveConfPrg")
            {
                SetStandardField(model);
                da.DTO.Execute.ExecuteType = SECS02P001ExecuteType.UpdateUsgPriv;
                da.DTO.Model = (SECS02P001Model)model;
                da.Update(da.DTO);
            }
            else if (mode == "SaveConfSysSeq")
            {
                SetStandardField(model);
                da.DTO.Execute.ExecuteType = SECS02P001ExecuteType.UpdateSysSeq;
                da.DTO.Model = (SECS02P001Model)model;
                da.Update(da.DTO);
            }
            else if (mode == "SaveConfPrgSeq")
            {
                SetStandardField(model);
                da.DTO.Execute.ExecuteType = SECS02P001ExecuteType.UpdatePrgSeq;
                da.DTO.Model = (SECS02P001Model)model;
                da.Update(da.DTO);
            }

            return da.DTO.Result;
        }
        private List<DDLCenterModel> GetUsgLevel()
        {
            var result = new List<DDLCenterModel>();
            if (SessionHelper.SYS_USG_LEVEL == "S")
            {
                result = GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_OPTIONDETAIL_002, new VSMParameter(FIXOptionID.FixOpt_100));
            }
            else
            {
                result = GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_OPTIONDETAIL_002, " and DDL_VALUE in ('U')", "", new VSMParameter(FIXOptionID.FixOpt_100));
            }
            return result;
        }
        private List<DDLCenterModel> GetUsgStatus()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_OPTIONDETAIL_002, new VSMParameter(FIXOptionID.FixOpt_99));
        }
        private void Set(SECS02P001Model model, string mode)
        {
            if(mode == "USG_STATUS_MODEL")
            {
                foreach (var item in model.USG_STATUS_MODEL)
                {
                    item.Value = item.Value.Trim();
                }
            }
            else if(mode == "USG_LEVEL_MODEL")
            {
                foreach (var item in model.USG_LEVEL_MODEL)
                {
                    item.Value = item.Value.Trim();
                }
            }
        }
        #endregion
    }
}