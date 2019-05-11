using DataAccess;
using DataAccess.MIS;
using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Web.Mvc;
using UtilityLib;
using WEBAPP.Helper;
using System.Text;
using System.Web;

namespace WEBAPP.Areas.MIS.Controllers
{
    public class MISS01P002Controller : MISBaseController
    {
        #region Property
        private MISS01P002Model localModel = new MISS01P002Model();
        private MISS01P002Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new MISS01P002Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as MISS01P002Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private MISS01P002Model TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new MISS01P002Model();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as MISS01P002Model;
            }
            set { TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = value; }
        }
        #endregion

        #region Opening 
        public ActionResult SearchOpening(MISS01P002Model model)
        {
            var da = new MISS01P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.GetAllOpening;

            if (Request.GetRequest("page").IsNullOrEmpty())
            {
                model.IsDefaultSearch = true;
                TempSearch = model;
            }

            //if (model.APP_CODE.IsNullOrEmpty())
            //{ 
            //    var app = BindAppCode();
            //    model.APP_CODE = app[0].Value.ToString();
            //}

            da.DTO.Model = TempSearch;
            da.DTO.Model.COM_CODE = model.APP_CODE;
            da.SelectNoEF(da.DTO);
            return JsonAllowGet(da.DTO.Models, da.DTO.Result);
        }
        [RuleSetForClientSideMessages("Assignment")]
        public ActionResult Assignment(MISS01P002Model model)
        {
            var da = new MISS01P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.GetByIdOpening;

            da.DTO.Model.COM_CODE = model.COM_CODE; //checked
            da.DTO.Model.NO = model.NO;

            da.SelectNoEF(da.DTO);

            #region Set Value  
            var view = string.Empty;
            view = "Assignment";
            SetButton(view);
            localModel = da.DTO.Model;
            TempModel.COM_CODE = da.DTO.Model.COM_CODE.Trim();
            localModel.ASSIGN_USER_MODEL = BindOwner();
            #endregion

            return View(view, localModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAssignMent(MISS01P002Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                model.APP_CODE = model.COM_CODE;
                var result = SaveData("Assignment", model);
                jsonResult = Success(result, StandardActionName.SaveCreate, Url.Action(StandardActionName.Index, new { page = 1 }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveCreate);
            }

            return jsonResult;
        }
        public ActionResult SearchFixed(MISS01P002Model model)
        {
            var da = new MISS01P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.GetAllFixed;

            da.DTO.Model = TempSearch;
            da.DTO.Model.ASSIGN_USER = TempModel.ASSIGN_USER;
            da.SelectNoEF(da.DTO);
            return JsonAllowGet(da.DTO.Models, da.DTO.Result);
        }
        #endregion

        #region OnProcess 
        public ActionResult SearchOnProcess(MISS01P002Model model)
        {
            var da = new MISS01P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.GetAllOnProcess;

            if (Request.GetRequest("page").IsNullOrEmpty())
            {
                model.IsDefaultSearch = true;
                TempSearch = model;
            }

            //if (model.APP_CODE.IsNullOrEmpty())
            //{ 
            //    var app = BindAppCode();
            //    model.APP_CODE = app[0].Value.ToString();
            //}

            da.DTO.Model = TempSearch;
            da.DTO.Model.COM_CODE = model.APP_CODE;
            da.SelectNoEF(da.DTO);
            return JsonAllowGet(da.DTO.Models, da.DTO.Result);
        }
        #endregion

        #region FollowUp 
        public ActionResult SearchFollowUp(MISS01P002Model model)
        {
            var da = new MISS01P002DA();

            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.GetAllFollowUp;

            if (Request.GetRequest("page").IsNullOrEmpty())
            {
                model.IsDefaultSearch = true;
                TempSearch = model;
            }

            //if (model.APP_CODE.IsNullOrEmpty())
            //{ 
            //    var app = BindAppCode();
            //    model.APP_CODE = app[0].Value.ToString();
            //}

            da.DTO.Model = TempSearch;
            da.DTO.Model.COM_CODE = model.APP_CODE;
            da.SelectNoEF(da.DTO);


            return JsonAllowGet(da.DTO.Models, da.DTO.Result);
        }
        [RuleSetForClientSideMessages("FilePacket")]
        public ActionResult FilePacket(MISS01P002Model model)
        {
            SetDefaulButton(StandardButtonMode.Create);
            SetDefaultData();
            SetButton("FilePacket");

            #region set default 
            var view = string.Empty;
            view = "FilePacket";

            var da = new MISS01P002DA();
            da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.GetFilePacket;
            da.DTO.Model.COM_CODE = model.COM_CODE;
            da.DTO.Model.ISE_NO = model.ISE_NO;
            da.SelectNoEF(da.DTO);

            localModel = da.DTO.Model;
            localModel.FILE_ID = da.DTO.Model.FILE_ID.Trim();
            //localModel.COM_CODE = SessionHelper.SYS_COM_CODE;
            #endregion

            return View(view, localModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveFilePacket(MISS01P002Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                var result = SaveData("FilePacket", model);
                jsonResult = Success(result, StandardActionName.SaveCreate, Url.Action(StandardActionName.Index, new { ACTIVE_STEP = 3 }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveCreate);
            }

            return jsonResult;
        }
        #endregion

        #region Golive 
        public ActionResult SearchGolive(MISS01P002Model model)
        {
            var da = new MISS01P002DA();

            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.GetAllGolive;

            if (Request.GetRequest("page").IsNullOrEmpty())
            {
                model.IsDefaultSearch = true;
                TempSearch = model;
            }

            //if (model.APP_CODE.IsNullOrEmpty())
            //{ 
            //    var app = BindAppCode();
            //    model.APP_CODE = app[0].Value.ToString();
            //}

            da.DTO.Model = TempSearch;
            da.DTO.Model.COM_CODE = model.APP_CODE;
            da.SelectNoEF(da.DTO);


            return JsonAllowGet(da.DTO.Models, da.DTO.Result);
        }
        #endregion

        #region Close 
        public ActionResult SearchClose(MISS01P002Model model)
        {
            var da = new MISS01P002DA();

            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.GetAllClose;

            if (Request.GetRequest("page").IsNullOrEmpty())
            {
                model.IsDefaultSearch = true;
                TempSearch = model;
            }

            //if (model.APP_CODE.IsNullOrEmpty())
            //{ 
            //    var app = BindAppCode();
            //    model.APP_CODE = app[0].Value.ToString();
            //}

            da.DTO.Model = TempSearch;
            da.DTO.Model.COM_CODE = model.APP_CODE;
            da.SelectNoEF(da.DTO);


            return JsonAllowGet(da.DTO.Models, da.DTO.Result);
        }
        #endregion

        #region Action 
        public ActionResult Index(string ACTIVE_STEP = "1")
        {
            //ViewBag.UrlToClosePage = Url.Action(StandardActionName.Index, "Default", new { Area = "Admin" });
            #region Set Close Page
            var menu = SessionHelper.SYS_MenuModel;
            if (menu != null)
            {
                var home = menu.Where(m => m.SYS_CODE.AsString().ToUpper() == "HOME").FirstOrDefault();
                if (home != null && AppExtensions.ExistsAction(home.PRG_ACTION, home.PRG_CONTROLLER, home.PRG_AREA))
                {
                    ViewBag.UrlToClosePage = Url.Action(home.PRG_ACTION, home.PRG_CONTROLLER, new { Area = home.PRG_AREA, SYS_SYS_CODE = home.SYS_CODE, SYS_PRG_CODE = home.PRG_CODE });
                }
            }
            #endregion

            var view = string.Empty;
            localModel.ACTIVE_STEP = "5"; //if set 2 then block step3 end
            if (ACTIVE_STEP == "1")
            {
                view = "Status1Open";
                SetButton(ACTIVE_STEP);
                SetDefaultData(ACTIVE_STEP);
                SetClientSideRuleSet("Status1Open");

            }
            else if (ACTIVE_STEP == "2")
            {
                view = "Status2Onprocess";
                SetButton(ACTIVE_STEP);
                SetDefaultData(ACTIVE_STEP);
                SetClientSideRuleSet("Status2Onprocess");

            }
            else if (ACTIVE_STEP == "3")
            {
                view = "Status3Followup";
                SetButton(ACTIVE_STEP);
                SetDefaultData(ACTIVE_STEP);
                SetClientSideRuleSet("Status3Followup");
            }
            else if (ACTIVE_STEP == "4")
            {
                view = "Status4Golive";
                SetButton(ACTIVE_STEP);
                SetDefaultData(ACTIVE_STEP);
                SetClientSideRuleSet("Status4Golive");

            }
            else if (ACTIVE_STEP == "5")
            {
                view = "Status5Close";
                SetButton(ACTIVE_STEP);
                SetDefaultData(ACTIVE_STEP);
                SetClientSideRuleSet("Status5Close");

            }

            SetHeaderWizard(new WizardHelper.WizardHeaderConfig(
                ACTIVE_STEP,
                localModel.ACTIVE_STEP,
                new WizardHelper.WizardHeader(Translation.MIS.MISS01P002.STEP_1, Url.Action("Index", new { ACTIVE_STEP = "1" }), iconCssClass: FaIcons.FaAreaChart),
                new WizardHelper.WizardHeader(Translation.MIS.MISS01P002.STEP_2, Url.Action("Index", new { ACTIVE_STEP = "2" }), iconCssClass: FaIcons.FaFile),
                new WizardHelper.WizardHeader(Translation.MIS.MISS01P002.STEP_3, Url.Action("Index", new { ACTIVE_STEP = "3" }), iconCssClass: FaIcons.FaFile),
                new WizardHelper.WizardHeader(Translation.MIS.MISS01P002.STEP_4, Url.Action("Index", new { ACTIVE_STEP = "4" }), iconCssClass: FaIcons.FaFile),
                new WizardHelper.WizardHeader(Translation.MIS.MISS01P002.STEP_5, Url.Action("Index", new { ACTIVE_STEP = "5" }), iconCssClass: FaIcons.FaFile)));

            return View(view, localModel);
        }
        #endregion

        #region Mehtod  
        private List<DDLCenterModel> BindAppCode()
        {
            return GetDDLCenter(KEY_ID: DDLCenterKey.DD_MISS01P002_001);
        }
        private List<DDLCenterModel> BindTimeOut()
        {
            return GetDDLCenter(KEY_ID: DDLCenterKey.DD_GOLIVE_TIMEOUT);
        }
        private List<DDLCenterModel> BindOwner()
        {
            return GetDDLCenter(DDLCenterKey.DD_MISS01P002_002, new VSMParameter(TempModel.COM_CODE));
        }
        public ActionResult BindOwner(string APP_CODE)
        {
            var model = GetDDLCenter(DDLCenterKey.DD_MISS01P002_003, new VSMParameter(APP_CODE.Trim()));
            return JsonAllowGet(model);
        }
        public ActionResult GetFiexd(string ASSINGMENT)
        {
            var da = new MISS01P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.GetFiexd;
            TempModel.ASSIGN_USER = da.DTO.Model.ASSIGN_USER = ASSINGMENT;

            da.SelectNoEF(da.DTO);

            return JsonAllowGet(da.DTO.Model);
        }
        public ActionResult ConfirmTest(MISS01P002Model model)
        {
            var da = new MISS01P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.ConfirmTest;

            da.DTO.Model = model;
            da.DTO.Model.FLAG = "T";
            da.DTO.Model.CRET_BY = SessionHelper.SYS_USER_ID;
            da.DTO.Model.CRET_DATE = DateTime.Now;
            da.UpdateNoEF(da.DTO);


            return JsonAllowGet(da.DTO.Model);
        }
        public ActionResult FollowUp(MISS01P002Model model)
        {
            var da = new MISS01P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.MoveToFollowUp;

            da.DTO.Model = model;
            da.DTO.Model.FLAG = "F";
            da.DTO.Model.CRET_BY = SessionHelper.SYS_USER_ID;
            da.DTO.Model.CRET_DATE = DateTime.Now;
            da.UpdateNoEF(da.DTO);


            return JsonAllowGet(da.DTO.Model);
        }
        public ActionResult Golive(MISS01P002Model model)
        {
            var da = new MISS01P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.MoveToGolive;

            da.DTO.Model = model;
            da.DTO.Model.FLAG = "G";
            da.DTO.Model.CRET_BY = SessionHelper.SYS_USER_ID;
            da.DTO.Model.CRET_DATE = DateTime.Now;
            da.UpdateNoEF(da.DTO);


            return JsonAllowGet(da.DTO.Model);
        }
        [HttpPost]
        public ActionResult StempTimeQA(List<MISS01P002Model> data)
        {
            var da = new MISS01P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.TimeStemp;

            //da.DTO.Model = model;
            da.DTO.Models = (List<MISS01P002Model>)data;
            da.DTO.Model.FLAG = "QA";
            da.DTO.Model.CRET_BY = SessionHelper.SYS_USER_ID;
            da.UpdateNoEF(da.DTO);


            return JsonAllowGet(da.DTO.Model);
        }
        [HttpPost]
        public ActionResult StempTimePD(List<MISS01P002Model> data)
        {
            var da = new MISS01P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.TimeStemp;

            //da.DTO.Model = model;
            da.DTO.Models = (List<MISS01P002Model>)data;
            da.DTO.Model.FLAG = "PD";
            da.DTO.Model.CRET_BY = SessionHelper.SYS_USER_ID;
            da.UpdateNoEF(da.DTO);


            return JsonAllowGet(da.DTO.Model);
        }
        public ActionResult Close(MISS01P002Model model)
        {
            var da = new MISS01P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.MoveToClose;

            da.DTO.Model = model;
            da.DTO.Model.FLAG = "S";
            da.DTO.Model.CRET_BY = SessionHelper.SYS_USER_ID;
            da.DTO.Model.CRET_DATE = DateTime.Now;
            da.UpdateNoEF(da.DTO);


            return JsonAllowGet(da.DTO.Model);
        }
        public ActionResult Cancel(MISS01P002Model model)
        {
            var da = new MISS01P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.MoveToCancel;

            da.DTO.Model = model;
            da.DTO.Model.CRET_BY = SessionHelper.SYS_USER_ID;
            da.UpdateNoEF(da.DTO);


            return JsonAllowGet(da.DTO.Model);
        }
        public ActionResult ReDo(MISS01P002Model model)
        {
            var da = new MISS01P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.ReDo;

            da.DTO.Model = model;
            da.DTO.Model.CRET_BY = SessionHelper.SYS_USER_ID;
            da.UpdateNoEF(da.DTO);


            return JsonAllowGet(da.DTO.Model);
        }
        public ActionResult ReOpen(MISS01P002Model model)
        {
            string PRG_CODE = "MISS01P001";
            string PRG_ACTTION = "Add";
            string REF_NO = model.ISE_NO.ToString();
            string COM_CODE = model.COM_CODE;

            return RedirectToAction(PRG_ACTTION, PRG_CODE, new { REF_NO = REF_NO, COM_CODE = COM_CODE});
        }
        private void SetDefaultData(string mode = "")
        {
            if (mode == "1")
            {
                localModel.APP_CODE_MODEL = BindAppCode();
            }
            else if (mode == "2")
            {
                localModel.APP_CODE_MODEL = BindAppCode();
            }
            else if (mode == "3")
            {
                localModel.APP_CODE_MODEL = BindAppCode();
            }
            else if (mode == "4")
            {
                localModel.APP_CODE_MODEL = BindAppCode();
                localModel.TIMEOUT_MODEL = BindTimeOut();
            }
            else if (mode == "5")
            {
                localModel.APP_CODE_MODEL = BindAppCode();
                localModel.TIMEOUT_MODEL = BindTimeOut();
            }
        }
        private void SetButton(string ACTIVE_STEP = "")
        {
            SetDefaulButton(StandardButtonMode.Other);
            if (ACTIVE_STEP == "1")
            {
                SetDefaulButton(StandardButtonMode.Index);
                RemoveStandardButton("DeleteSearch");
                RemoveStandardButton("Add");
                // RemoveStandardButton();
            }
            else if (ACTIVE_STEP == "2")
            {
                SetDefaulButton(StandardButtonMode.Index);
                RemoveStandardButton("DeleteSearch");
                RemoveStandardButton("Add");
            }
            else if (ACTIVE_STEP == "3")
            {
                SetDefaulButton(StandardButtonMode.Index);
                RemoveStandardButton("DeleteSearch");
                RemoveStandardButton("Add");
                AddButton(StandButtonType.ButtonAjax, "btnStamTimeQA", Translation.MIS.MISS01P002.TIMESTEMP_QA, iconCssClass: FaIcons.FaClockO, url: Url.Action("StempTimeQA"));
                AddButton(StandButtonType.ButtonAjax, "btnStamTimePD", Translation.MIS.MISS01P002.TIMESTEMP_PD, iconCssClass: FaIcons.FaClockO, url: Url.Action("StempTimePD"));
            }
            else if (ACTIVE_STEP == "4")
            {
                SetDefaulButton(StandardButtonMode.Index);
                RemoveStandardButton("DeleteSearch");
                RemoveStandardButton("Add");
            }
            else if (ACTIVE_STEP == "Assignment")
            {
                AddButton(StandButtonType.ButtonComfirmAjax, "btnSAVEASSIGNMENT", Translation.CenterLang.Center.Save, iconCssClass: FaIcons.FaSave, url: Url.Action("SaveAssignMent"), isValidate: true);
            }
            else if (ACTIVE_STEP == "FilePacket")
            {
                AddButton(StandButtonType.ButtonComfirmAjax, "btnSAVEFILEPACKET", Translation.CenterLang.Center.Save, iconCssClass: FaIcons.FaSave, url: Url.Action("SaveFilePacket"), isValidate: true);
            }
        }
        private DTOResult SaveData(string mode, object model)
        {
            var da = new MISS01P002DA();
            //ในกรณีที่มีการ SaveLog ให้ Include SetStandardLog ด้วย
            //SetStandardLog(
            //   da.DTO,
            //   model,
            //   GetSaveLogConfig("dbo", "VSMS_COMPANY", "COM_CODE"));


            if (mode == StandardActionName.SaveCreate)
            {
                SetStandardField(model);
                da.DTO.Model = (MISS01P002Model)model;
                //da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.Insert;
                da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;

                da.InsertNoEF(da.DTO);
            }
            else if (mode == StandardActionName.Delete)
            {
                da.DTO.Models = (List<MISS01P002Model>)model;
                da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;
                da.DeleteNoEF(da.DTO);
            }
            else if (mode == "Assignment")
            {
                SetStandardField(model);
                da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.UpdateAssignment;
                da.DTO.Model = (MISS01P002Model)model;
                da.UpdateNoEF(da.DTO);
            }
            else if (mode == "FilePacket")
            {
                SetStandardFieldWithoutComCode(model);
                da.DTO.Execute.ExecuteType = MISS01P002ExecuteType.UpdateFilePacket;
                da.DTO.Model = (MISS01P002Model)model;
                da.UpdateNoEF(da.DTO);
            }

            return da.DTO.Result;
        }
        #endregion
    }
}