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
    public class MISS01P003Controller : MISBaseController
    {
        #region Property
        private MISS01P003Model localModel = new MISS01P003Model();
        private MISS01P003Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new MISS01P003Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as MISS01P003Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private MISS01P003Model TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new MISS01P003Model();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as MISS01P003Model;
            }
            set { TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = value; }
        }
        #endregion

        #region Action 
        public ActionResult Index()
        {
            SetDefaulButton(StandardButtonMode.Index);
            RemoveStandardButton("DeleteSearch");
            RemoveStandardButton("Add");
            if (TempSearch.IsDefaultSearch && !Request.GetRequest("page").IsNullOrEmpty())
            {
                localModel = TempSearch.CloneObject();
            }
            
            SetDefaultData(StandardActionName.Index);

            return View(StandardActionName.Index, localModel);
        }
        public ActionResult Search(MISS01P003Model model)
        {
            var da = new MISS01P003DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P003ExecuteType.GetAll;

            if (Request.GetRequest("page").IsNullOrEmpty())
            {
                model.IsDefaultSearch = true;
                TempSearch = model;
            }

            da.DTO.Model = TempSearch;
            da.DTO.Model.COM_CODE = model.APP_CODE;
            da.DTO.Model.STATUS = model.STATUS;
            da.DTO.Model.CRET_BY = SessionHelper.SYS_USER_ID;
            da.SelectNoEF(da.DTO);
            return JsonAllowGet(da.DTO.Models, da.DTO.Result);
        }
        public ActionResult Agree(MISS01P003Model model)
        {
            var da = new MISS01P003DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P003ExecuteType.ConfirmStatus;

            da.DTO.Model = model;
            da.DTO.Model.FALG = "A";
            da.DTO.Model.CRET_BY = SessionHelper.SYS_USER_ID;
            da.DTO.Model.CRET_DATE = DateTime.Now;
            da.UpdateNoEF(da.DTO);

            return JsonAllowGet(da.DTO.Model);
        }
        public ActionResult Cancel(MISS01P003Model model)
        {
            var da = new MISS01P003DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P003ExecuteType.ConfirmStatus;

            da.DTO.Model = model;
            da.DTO.Model.FALG = "C";
            da.DTO.Model.CRET_BY = SessionHelper.SYS_USER_ID;
            da.DTO.Model.CRET_DATE = DateTime.Now;
            da.UpdateNoEF(da.DTO);


            return JsonAllowGet(da.DTO.Model);
        }
        public ActionResult Done(MISS01P003Model model)
        {
            var da = new MISS01P003DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = MISS01P003ExecuteType.ConfirmStatus;

            da.DTO.Model = model;
            da.DTO.Model.FALG = "D";
            da.DTO.Model.CRET_BY = SessionHelper.SYS_USER_ID;
            da.DTO.Model.CRET_DATE = DateTime.Now;
            da.UpdateNoEF(da.DTO);


            return JsonAllowGet(da.DTO.Model);
        }
        #endregion

        #region Mehtod  
        //----------------------- DDL-----------------------

        private void SetDefaultData(string mode = "")
        {
            if (mode == "Index")
            {
                localModel.STATUS_MODEL = BindStatus();
                localModel.APP_CODE_MODEL = BindAppCode();
            }
        }
        private List<DDLCenterModel> BindStatus()
        {
            return GetDDLCenter(DDLCenterKey.DD_MISS01P003_001);
        }
        private List<DDLCenterModel> BindAppCode()
        {
            return GetDDLCenter(DDLCenterKey.DD_APPLICATION);
        }
        
        #endregion
    }
}