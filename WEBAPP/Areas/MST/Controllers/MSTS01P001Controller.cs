using DataAccess;
using DataAccess.MST;
using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using UtilityLib;
using WEBAPP.Helper;

namespace WEBAPP.Areas.MST.Controllers
{
    public class MSTS01P001Controller : MSTBaseController
    {
        #region Property
        private MSTS01P001Model localModel = new MSTS01P001Model();
        private MSTS01P001Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new MSTS01P001Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as MSTS01P001Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private MSTS01P001Model TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new MSTS01P001Model();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as MSTS01P001Model;
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
        public ActionResult Search(MSTS01P001Model model)
        {
            var da = new MSTS01P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MSTS01P001ExecuteType.GetAll;
            if (Request.GetRequest("page").IsNullOrEmpty())
            {
                model.IsDefaultSearch = true;
                TempSearch = model;
            }
            da.DTO.Model = TempSearch;
            SetStandardField(da.DTO.Model);
            da.SelectNoEF(da.DTO);
            return JsonAllowGet(da.DTO.Models, da.DTO.Result);
        }
        [HttpPost]
        public ActionResult DeleteSearch(List<MSTS01P001Model> data)
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
            SetDefaultData();   //set ค่า DDL
            localModel.COM_CODE = SessionHelper.SYS_COM_CODE;
            localModel.APP_CODE = SessionHelper.SYS_COM_CODE;

            return View(StandardActionName.Add, localModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCreate(MSTS01P001Model model)
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
        public ActionResult Edit(MSTS01P001Model model)
        {
            var da = new MSTS01P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MSTS01P001ExecuteType.GetByID;
            da.DTO.Model.ISSUE_TYPE = model.ISSUE_TYPE;
            da.DTO.Model.TYPE_RATE = model.TYPE_RATE;
            da.DTO.Model.COM_CODE = model.COM_CODE;

            da.SelectNoEF(da.DTO);
            localModel = da.DTO.Model;

            SetDefaulButton(StandardButtonMode.Modify);
            SetDefaultData();   //set ค่า DDL
            localModel.ISSUE_TYPE_MODEL = BindIssueType();

            return View(StandardActionName.Edit, localModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveModify(MSTS01P001Model model)
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
        #endregion

        #region Mehtod  
        //----------------------- DDL-----------------------
        private List<DDLCenterModel> BindAppCode()
        {
            return GetDDLCenter(KEY_ID: DDLCenterKey.DD_MISS01P002_001);
        }
        private void SetDefaultData(string mode = "")
        {
            localModel.TYPE_RATE_MODEL = BindTyprRate();
            localModel.APP_CODE_MODEL = BindAppCode();
            localModel.ISSUE_TYPE_MODEL = BindIssueType();
        }
        private List<DDLCenterModel> BindIssueType()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_ISSUETYPE, new VSMParameter(SessionHelper.SYS_COM_CODE.Trim()));
        }
        private List<DDLCenterModel> BindTyprRate()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_TYPE_RATE);
        }

        //----------------------------------------------//
        private DTOResult SaveData(string mode, object model)
        {
            var da = new MSTS01P001DA();
            SetStandardField(da.DTO.Model);
            //ในกรณีที่มีการ SaveLog ให้ Include SetStandardLog ด้วย
            //SetStandardLog(
            //   da.DTO,
            //   model,
            //   GetSaveLogConfig("dbo", "VSMS_COMPANY", "COM_CODE"));


            if (mode == StandardActionName.SaveCreate)
            {
                SetStandardField(model);
                da.DTO.Model = (MSTS01P001Model)model;

                da.InsertNoEF(da.DTO);
            }
            else if (mode == StandardActionName.SaveModify)
            {
                SetStandardField(model);
                da.DTO.Model = (MSTS01P001Model)model;
                
                da.UpdateNoEF(da.DTO);
            }
            else if (mode == StandardActionName.Delete)
            {
                da.DTO.Models = (List<MSTS01P001Model>)model;
                da.DeleteNoEF(da.DTO);
            }
           
            return da.DTO.Result;
        }

        #endregion
    }
}