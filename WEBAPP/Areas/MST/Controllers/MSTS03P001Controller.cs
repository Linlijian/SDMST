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
    public class MSTS03P001Controller : MSTBaseController
    {
        #region Property
        private MSTS03P001Model localModel = new MSTS03P001Model();
        private MSTS03P001Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new MSTS03P001Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as MSTS03P001Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private MSTS03P001Model TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new MSTS03P001Model();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as MSTS03P001Model;
            }
            set { TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = value; }
        }
        #endregion

        #region Action 
        public ActionResult Index()
        {
            SetDefaulButton(StandardButtonMode.Other);

            if (TempSearch.IsDefaultSearch && !Request.GetRequest("page").IsNullOrEmpty())
            {
                localModel = TempSearch.CloneObject();
            }
            SetDefaultData(StandardActionName.Index);

            return View(StandardActionName.Index, localModel);
        }
        public ActionResult Search(MSTS03P001Model model)
        {
            var da = new MSTS03P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MSTS03P001ExecuteType.GetAll;
            if (Request.GetRequest("page").IsNullOrEmpty())
            {
                model.IsDefaultSearch = true;
                TempSearch = model;
            }
            da.DTO.Model = TempSearch;
            da.SelectNoEF(da.DTO);
            return JsonAllowGet(da.DTO.Models, da.DTO.Result);
        }
        [HttpPost]
        public ActionResult DeleteSearch(List<MSTS03P001Model> data)
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
            SetDefaultData(StandardActionName.Add);

            return View(StandardActionName.Add, localModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCreate(MSTS03P001Model model)
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
        public ActionResult Edit(MSTS03P001Model model)
        {
            var da = new MSTS03P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MSTS03P001ExecuteType.GetByID;
            TempModel.PIT_ID =  da.DTO.Model.PIT_ID = model.PIT_ID;
            SetStandardField(da.DTO.Model);
            da.SelectNoEF(da.DTO);

            localModel = da.DTO.Model;

            if (localModel.IS_USED)
                RemoveStandardButton("SaveModify");
            else
                SetDefaulButton(StandardButtonMode.Modify);

            SetDefaultData(StandardActionName.Edit);   //set ค่า DDL

            return View(StandardActionName.Edit, localModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveModify(MSTS03P001Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                model.PIT_ID = TempModel.PIT_ID;
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
        private void SetDefaultData(string mode = "")
        {
            if(mode == "Index")
            {
                localModel.KEY_ID_MODEL = BindKeyId();
                localModel.APP_CODE_MODEL = BindAppCode();
                localModel.T_RES_TYPE_MODEL = BindTypeTime();
                localModel.RES_TYPE_MODEL = BindTypeTime();
            }
            else if(mode == "Edit" || mode == "Add")
            {
                localModel.APP_CODE_MODEL = BindAppCode();
                localModel.KEY_ID_MODEL = BindKeyId();
                localModel.T_RES_TYPE_MODEL = BindTypeTime();
                localModel.RES_TYPE_MODEL = BindTypeTime();
            }
        }
        private List<DDLCenterModel> BindAppCode()
        {
            return GetDDLCenter(KEY_ID: DDLCenterKey.DD_MISS01P002_001);
        }
        private List<DDLCenterModel> BindKeyId()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_KEY_ID);
        }
        private List<DDLCenterModel> BindTypeTime()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_TYPETIME);
        }

        //----------------------------------------------//
        private DTOResult SaveData(string mode, object model)
        {
            var da = new MSTS03P001DA();
            //ในกรณีที่มีการ SaveLog ให้ Include SetStandardLog ด้วย
            //SetStandardLog(
            //   da.DTO,
            //   model,
            //   GetSaveLogConfig("dbo", "VSMS_COMPANY", "COM_CODE"));


            if (mode == StandardActionName.SaveCreate)
            {
                SetStandardField(model);
                da.DTO.Model = (MSTS03P001Model)model;
                da.DTO.Execute.ExecuteType = MSTS03P001ExecuteType.Insert;
                da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;

                da.InsertNoEF(da.DTO);
            }
            else if (mode == StandardActionName.SaveModify)
            {
                SetStandardField(model);
                da.DTO.Execute.ExecuteType = MSTS03P001ExecuteType.Update;
                da.DTO.Model = (MSTS03P001Model)model;

                da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;
                da.UpdateNoEF(da.DTO);
            }
            else if (mode == StandardActionName.Delete)
            {
                da.DTO.Models = (List<MSTS03P001Model>)model;
                da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;
                da.DeleteNoEF(da.DTO);
            }
           
            return da.DTO.Result;
        }

        #endregion
    }
}