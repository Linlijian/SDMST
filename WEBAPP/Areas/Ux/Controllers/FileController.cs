using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WEBAPP.Helper;
using UtilityLib;
using WEBAPP.Controllers;
using DataAccess.Ux;
using DataAccess;
using WEBAPP.Models;

namespace WEBAPP.Areas.Ux.Controllers
{
    public class FileController : BaseController
    {
        private FileModel TempModel
        {
            get
            {
                if (TempData["ModelUxFile"] == null)
                {
                    TempData["ModelUxFile"] = new FileModel();
                }
                TempData.Keep("ModelUxFile");
                return TempData["ModelUxFile"] as FileModel;
            }
            set
            {
                TempData["ModelUxFile"] = value;
                TempData.Keep("ModelUxFile");
            }
        }


        public ActionResult GetTokenID()
        {
            //var files = GetFile();
            //List<FileModel> data = null;
            var status = false;
            //SessionHelper.FileWaitingSign = files;
            //if (files != null)
            //{
            //    data = files.Select(m => new FileModel { ID = m.ID, Data = Convert.ToBase64String(m.BLOB_FILE), Name = SessionHelper.FileSignParameterName }).ToList();
            //    status = true;
            //}
            return Json(new WEBAPP.Models.AjaxGridResult
            {
                Mode = StandardActionName.Search,
                Status = status//,
                //data = data
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Download(decimal? ID)
        {
            var da = new FileDA();
            da.DTO.Execute.ExecuteType = FileExecuteType.GetBlobFileByID;
            da.DTO.ExecuteFile.COM_CODE = SessionHelper.SYS_COM_CODE;
            da.DTO.ExecuteFile.ID = ID;
            da.Select(da.DTO);

            if (da.DTO.ExecuteFile.File.BLOB_FILE != null)
            {
                ExportHelper.ExportFile(Response, da.DTO.ExecuteFile.File.FILE_NAME, da.DTO.ExecuteFile.File.BLOB_FILE);
            }
            else
            {
                return Content(Translation.CenterLang.Center.FileNotFound);
            }
            return Content("ExportFile");
        }
        [HttpPost]
        public ActionResult GetFileData()
        {
            var da = new FileDA();
            da.DTO.Execute.ExecuteType = FileExecuteType.GetBlobFileList;
            da.DTO.ExecuteFile = SessionHelper.FileModel.CloneObject();
            da.Select(da.DTO);
            object data = null;
            var status = false;
            SessionHelper.FileWaitingSign = da.DTO.ExecuteFile.Files;
            if (da.DTO.ExecuteFile.Files != null)
            {
                data = da.DTO.ExecuteFile.Files.Select(m => new FileModel { ROW_ID = m.ROW_ID, ID = m.ID, Name = SessionHelper.FileModel.Name }).ToList();
                status = true;
            }
            return Json(new AjaxGridResult
            {
                Mode = StandardActionName.Search,
                Status = status,
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFileInfo(FileModel model)
        {
            model.COM_CODE = SessionHelper.SYS_COM_CODE;
            if (model.PRG_CODE.IsNullOrEmpty())
            {
                model.PRG_CODE = SessionHelper.SYS_CurrentPRG_CODE;
            }

            return JsonAllowGet(GetBlobList(model));
        }
        public ActionResult GetFileInfo_ElectronicSign()
        {
            return JsonAllowGet(GetBlobList(TempModel.CloneObject()));
        }

        [HttpPost]
        public ActionResult GetFileDataById(decimal? ID)
        {
            var da = new FileDA();
            da.DTO.Execute.ExecuteType = FileExecuteType.GetBlobFileByID;
            da.DTO.ExecuteFile.COM_CODE = SessionHelper.SYS_COM_CODE;
            da.DTO.ExecuteFile.ID = ID;
            da.Select(da.DTO);
            var ajgRes = new AjaxGridResult();
            if (da.DTO.ExecuteFile.File.BLOB_FILE != null)
            {
                ajgRes.data = Convert.ToBase64String(da.DTO.ExecuteFile.File.BLOB_FILE);
                ajgRes.Status = true;
            }
            else
            {
                ajgRes.data = string.Empty;
                ajgRes.Status = false;
                ajgRes.Style = AlertStyles.Error;
                ajgRes.Message = Translation.CenterLang.Center.DataNotFound;

            }
            var json = Json(ajgRes, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult CheckHasFile()
        {
            var files = GetBlobList(SessionHelper.FileModel.CloneObject());
            var status = false;
            if (files != null && files.Count > 0)
            {
                status = true;
            }
            return Json(new WEBAPP.Models.AjaxResult
            {
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ElectronicSign(FileModel model)
        {
            if (model.PRG_CODE.IsNullOrEmpty())
            {
                model.PRG_CODE = SessionHelper.SYS_CurrentPRG_CODE;
            }

            model.COM_CODE = SessionHelper.SYS_COM_CODE;

            //SET DATA TO GetFileInfo()
            TempModel = model.CloneObject();
            //SET PRG CODE FOR GRID
            //TempModel.PRG_CODE = SessionHelper.SYS_CurrentPRG_CODE;


            //TEMP SAVE
            var da = new FileDA();
            da.DTO.Execute.ExecuteType = FileExecuteType.GetElectronicSign;
            da.DTO.ExecuteFile = model;
            da.Select(da.DTO);
            return View(da.DTO.ESignModel);
        }

        private List<FileUpload> GetBlobList(FileModel model)
        {
            var da = new FileDA();
            da.DTO.Execute.ExecuteType = FileExecuteType.GetBlobList;
            da.DTO.ExecuteFile = model;
            da.Select(da.DTO);
            return da.DTO.ExecuteFile.Files;
        }
    }
}