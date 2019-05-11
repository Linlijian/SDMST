using DataAccess;
using DataAccess.SEC;
using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using UtilityLib;
using WEBAPP.Helper;

namespace WEBAPP.Areas.SEC.Controllers
{
    public class SECS01P003Controller : SECBaseController
    {
        #region Property

        private SECS01P003Model localModel = new SECS01P003Model();
        private SECS01P003Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new SECS01P003Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as SECS01P003Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private SECS01P003Model TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new SECS01P003Model();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as SECS01P003Model;
            }
            set { TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = value; }
        } 

        #endregion

        #region Action

        public ActionResult Index()
        {
            SetDefaulButton(StandardButtonMode.Index);
            if (TempSearch.IsDefaultSearch && !Request.GetRequest("page").IsNullOrEmpty())
            {
                localModel = TempSearch.CloneObject();
            }
            SetDefaultData();
            return View(StandardActionName.Index, localModel);
        }
        public ActionResult Search(SECS01P003Model model)
        {
            var da = new SECS01P003DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS01P003ExecuteType.GetAll;
            if (Request.GetRequest("page").IsNullOrEmpty())
            {
                model.IsDefaultSearch = true;
                TempSearch = model;
            }
            da.DTO.Model = TempSearch;
            da.Select(da.DTO);
            return JsonAllowGet(da.DTO.Models, da.DTO.Result);
        }
        [HttpPost]
        public ActionResult DeleteSearch(List<SECS01P003Model> data)
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
        [RuleSetForClientSideMessages("Add")]
        public ActionResult Add()
        {
            SetDefaulButton(StandardButtonMode.Create);

            SetDefaultData();
            return View(StandardActionName.Add, localModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCreate(SECS01P003Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                var result = SaveData(StandardActionName.SaveCreate, model);
                jsonResult = Success(result, StandardActionName.SaveCreate, Url.Action(StandardActionName.Index, new { page = 1 }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveCreate);
            }
            return jsonResult;
        }
        [RuleSetForClientSideMessages("Edit")]
        public ActionResult Edit(string COM_CODE, string PRG_CODE)
        {
            SetDefaulButton(StandardButtonMode.Modify);

            var da = new SECS01P003DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS01P003ExecuteType.GetByID;
            TempModel.COM_CODE = da.DTO.Model.COM_CODE = COM_CODE;
            TempModel.PRG_CODE = da.DTO.Model.PRG_CODE = PRG_CODE;
            da.Select(da.DTO);
            if (da.DTO.Model != null)
            {
                localModel = da.DTO.Model;
            }
            SetDefaultData();
            return View(StandardActionName.Edit, localModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveModify(SECS01P003Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                model.COM_CODE = TempModel.COM_CODE;
                model.PRG_CODE = TempModel.PRG_CODE;
                var result = SaveData(StandardActionName.SaveModify, model);
                jsonResult = Success(result, StandardActionName.SaveModify, Url.Action(StandardActionName.Index, new { page = 1 }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveModify);
            }
            return jsonResult;
        }

        #endregion

        #region Method

        private void SetDefaultData(string mode = "")
        {
            localModel.PRG_TYPE_MODEL = BindType();
            localModel.PRG_STATUS_MODEL = BindStatus();
            localModel.PRG_TYPE_MODEL = BindPrgType();
        }
        private List<DDLCenterModel> BindType()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_OPTIONDETAIL_002, new VSMParameter(FIXOptionID.FixOpt_71));
        }
        private List<DDLCenterModel> BindStatus()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_OPTIONDETAIL_002, new VSMParameter(FIXOptionID.FixOpt_71));
        }
        private List<DDLCenterModel> BindPrgType()
        {
            var data = new List<DDLCenterModel>()
            {
                new DDLCenterModel() { Value = "A" , Text = "Common Master" },
                new DDLCenterModel() { Value = "M" , Text = "Maintenance" },
                new DDLCenterModel() { Value = "T" , Text = "Transaction" },
                new DDLCenterModel() { Value = "S" , Text = "System" },
                new DDLCenterModel() { Value = "R" , Text = "Report" }
            };
            return data;
        }
        private DTOResult SaveData(string mode, object model)
        {
            var da = new SECS01P003DA();
            //ในกรณีที่มีการ SaveLog ให้ Include SetStandardLog ด้วย
            //SetStandardLog(
            //   da.DTO,
            //   model,
            //   GetSaveLogConfig("dbo", "VSMS_CONFIG_GENERAL", "ID"));


            if (mode == StandardActionName.SaveCreate)
            {
                SetStandardField(model);
                da.DTO.Model = (SECS01P003Model)model;
                da.Insert(da.DTO);
            }
            else if (mode == StandardActionName.SaveModify)
            {
                SetStandardField(model);
                da.DTO.Model = (SECS01P003Model)model;
                da.Update(da.DTO);
            }
            else if (mode == StandardActionName.Delete)
            {
                da.DTO.Model = new SECS01P003Model();
                SetStandardField(da.DTO.Model);
                da.DTO.Models = (List<SECS01P003Model>)model;
                da.Delete(da.DTO);
            }
            return da.DTO.Result;
        }

        #endregion
    }
}