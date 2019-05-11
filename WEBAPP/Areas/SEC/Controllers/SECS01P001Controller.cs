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
    public class SECS01P001Controller : SECBaseController
    {
        #region Property
        private SECS01P001Model localModel = new SECS01P001Model();
        private SECS01P001Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new SECS01P001Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as SECS01P001Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private SECS01P001Model TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new SECS01P001Model();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as SECS01P001Model;
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
            return View(StandardActionName.Index, localModel);
        }

        public ActionResult Search(SECS01P001Model model)
        {
            var da = new SECS01P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS01P001ExecuteType.GetAll;
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
        public ActionResult DeleteSearch(List<SECS01P001Model> data)
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
        public ActionResult SaveCreate(SECS01P001Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                TempModel.COM_CODE = model.COM_CODE;
                model.COM_BRANCH = model.COM_BRANCH;
                model.COM_POST_CODE = model.COM_POST_CODE_E;   //เฟิวที่เก็บชื่อไม่เหมือนกัน 
                model.COM_FAC_POST = model.COM_FAC_POST_E;


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
        public ActionResult Edit(string COM_CODE, string COM_BRANCH, SECS01P001Model model)
        {
            SetDefaulButton(StandardButtonMode.Modify);

            var da = new SECS01P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS01P001ExecuteType.GetByID;
            TempModel.COM_CODE = da.DTO.Model.COM_CODE = COM_CODE.ToString();
            TempModel.COM_BRANCH = da.DTO.Model.COM_BRANCH = COM_BRANCH.ToString();
            da.Select(da.DTO);
            if (da.DTO.Model != null)
            {
                model.COM_POST_CODE_E = da.DTO.Model.COM_CODE;   //เอาค่ารหัสไปษณีออกมาเเสดง 
                model.COM_FAC_POST_E = da.DTO.Model.COM_CODE;
                localModel = da.DTO.Model;
                if (da.DTO.Model.COM_USE_LANGUAGE == "T")
                {
                    localModel.COM_USE_LANGUAGE = "T    ";
                }
                else if(da.DTO.Model.COM_USE_LANGUAGE == "E")
                {
                    localModel.COM_USE_LANGUAGE = "E    ";
                }


            }
            SetDefaultData();   //set ค่า DDL


            return View(StandardActionName.Edit, localModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveModify(SECS01P001Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                model.COM_CODE = TempModel.COM_CODE;
                model.COM_BRANCH = TempModel.COM_BRANCH;

                //รหัสไปสณี 
                model.COM_POST_CODE = model.COM_POST_CODE_E;
                model.COM_FAC_POST = model.COM_FAC_POST_E;

                var result = SaveData(StandardActionName.SaveModify, model);
                jsonResult = Success(result, StandardActionName.SaveModify, Url.Action(StandardActionName.Index, new { page = 1 }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveModify);
            }
            return jsonResult;
        }

        public ActionResult Bind_DetailAdd()
        {
            var da = new SECS01P001DA();
            SetStandardErrorLog(da.DTO);

            return JsonAllowGet(da.DTO.Model.Details);
        }
        public ActionResult Bind_Detail()
        {
            var da = new SECS01P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS01P001ExecuteType.GetDetailByID;
            da.DTO.Model.COM_CODE = TempModel.COM_CODE;

            da.Select(da.DTO);
            return JsonAllowGet(da.DTO.Model.Details);
        }
        public ActionResult ConfMod(string COM_CODE)
        {
            SetDefaulButton(StandardButtonMode.Other);
            AddStandardButton(StandardButtonName.SaveModify, url: Url.Action("SaveConfMod"));

            localModel.USER_ID_MODEL = BindUserId();
            localModel.COM_CODE = TempModel.COM_CODE = COM_CODE;
            
            return View("ConfMod", localModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveConfMod(SECS01P001Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                var result = SaveData("SaveConfMod", model);
                jsonResult = Success(result, StandardActionName.SaveModify, Url.Action(StandardActionName.Index, new { page = 1 }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveModify);
            }
            return jsonResult;
        }
        [HttpPost]
        public ActionResult DeleteDetails(List<SECS01P001Model> data)
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

        #region Mehtod  
        //----------------------- DDL-----------------------
        private void SetDefaultData(string mode = "")
        {
            localModel.COM_USE_LANGUAGE_MODEL = BindLANGUAGE();
            localModel.COM_PROVINCE_T_MODEL = BindPROVINCE_T();
            localModel.COM_PROVINCE_E_MODEL = BindPROVINCE_E();
            localModel.COM_FAC_PRV_T_MODEL = BindFAC_PRV_T();
            localModel.COM_FAC_PRV_E_MODEL = BindFAC_PRV_E();
        }

        private List<DDLCenterModel> BindLANGUAGE()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_OPTIONDETAIL_002, new VSMParameter(FIXOptionID.FixOpt_80));
        }
        private List<DDLCenterModel> BindPROVINCE_T()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_PROVINCE_TH_001);
        }
        private List<DDLCenterModel> BindPROVINCE_E()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_PROVINCE_EN_001);
        }
        private List<DDLCenterModel> BindFAC_PRV_T()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_PROVINCE_TH_001);
        }
        private List<DDLCenterModel> BindUserId()
        {
            return GetDDLCenter(DDLCenterKey.DD_SECS01P001_001);
        }
        private List<DDLCenterModel> BindFAC_PRV_E()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_PROVINCE_EN_001);
        }

        //----------------------------------------------//
        private DTOResult SaveData(string mode, object model)
        {
            var da = new SECS01P001DA();
            //ในกรณีที่มีการ SaveLog ให้ Include SetStandardLog ด้วย
            SetStandardLog(
               da.DTO,
               model,
               GetSaveLogConfig("dbo", "VSMS_COMPANY", "COM_CODE"));


            if (mode == StandardActionName.SaveCreate)
            {
                SetStandardField(model);
                da.DTO.Model = (SECS01P001Model)model;
                da.DTO.Model.COM_CODE = TempModel.COM_CODE;
                da.DTO.Execute.ExecuteType = SECS01P001ExecuteType.Insert;

                da.Insert(da.DTO);
            }
            else if (mode == StandardActionName.SaveModify)
            {
                SetStandardField(model);
                da.DTO.Model = (SECS01P001Model)model;

                da.DTO.Model.COM_CODE = TempModel.COM_CODE;
                //da.DTO.Model.COM_BRANCH = TempModel.COM_BRANCH.TrimEnd();
                da.Update(da.DTO);
            }
            else if (mode == StandardActionName.Delete)
            {
                da.DTO.Execute.ExecuteType = SECS01P001ExecuteType.Delete;
                da.DTO.Models = (List<SECS01P001Model>)model;
                da.Delete(da.DTO);
            }
            else if (mode == "DeleteDetails")
            {
                da.DTO.Execute.ExecuteType = SECS01P001ExecuteType.DeleteDetail;
                da.DTO.Models = (List<SECS01P001Model>)model;
                da.Delete(da.DTO);
            }
            else if (mode == "SaveConfMod")
            {
                SetStandardField(model);
                da.DTO.Model = (SECS01P001Model)model;
                da.DTO.Model.COM_CODE = TempModel.COM_CODE;
                da.DTO.Execute.ExecuteType = SECS01P001ExecuteType.InsertDetail;

                da.Insert(da.DTO);
            }
            return da.DTO.Result;
        }

        #endregion
    }
}