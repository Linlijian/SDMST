using DataAccess.Ux;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using UtilityLib;
using WEBAPP.Controllers;
using WEBAPP.Helper;

namespace WEBAPP.Areas.Ux.Controllers
{
    public class ValidateExcelController : BaseController
    {
        [HttpPost]
        public ActionResult GetValidateExcel(DataAccess.StandardModel model, string PRG_CODE)
        {
            //string PRG_CODE = "ZQM013P";//SessionHelper.SYS_CurrentPRG_CODE;

            //GetData
            DataAccess.EXC001.EXC001DA da = new DataAccess.EXC001.EXC001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = DataAccess.EXC001.EXC001ExecuteType.GetQueryAllVal;
            da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;
            da.DTO.Model.PRG_CODE = PRG_CODE;
            da.Select(da.DTO);

            //var r = model.EXCEL_UPLOAD[0].File.ToArrayByte();
            var a =model.EXCEL_UPLOAD[0].FILE_NAME;

            EXC001Model result = WEBAPP.Helper.ExcelData.ValidateExcel(model.EXCEL_UPLOAD[0].File.ToArrayByte(), model.EXCEL_UPLOAD[0].FILE_NAME, SessionHelper.SYS_COM_CODE, PRG_CODE, da.DTO.Models);

            return JsonAllowGet(result);
        }
        public ActionResult GetDataValidateExcel(string pE = "N", string pC = "N")
        {
            var data = new List<Dictionary<string, object>>();

            string strWhere = "";
            if (pE == "Y" && pC == "Y")
            {
            }
            else if (pE == "N" && pC == "N")
            {
                strWhere += "(1=2)";
            }
            else if (pE == "Y")
            {
                strWhere += "COM_COL = 'E'";
            }
            else if (pC == "Y")
            {
                strWhere += "COM_COL = 'C'";
            }

            foreach (DataRow dr in WEBAPP.Helper.ExcelData.TBL_SELECT.Tables[0].Select(strWhere, "IDX ASC"))
            {
                var item = new Dictionary<string, object>();
                foreach (DataColumn column in WEBAPP.Helper.ExcelData.TBL_SELECT.Tables[0].Columns)
                {
                    item.Add(column.ColumnName.ToUpper(), dr[column.ColumnName]);
                }

                data.Add(item);
            }

            return Json(new WEBAPP.Models.AjaxGridResult
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataSumValidateExcel(string pE = "N", string pC = "N")
        {
            //var data = new List<Dictionary<string, object>>();

            //string strWhere = "";
            //if (pE == "Y" && pC == "Y")
            //{
            //}
            //else if (pE == "N" && pC == "N")
            //{
            //    strWhere += "(1=2)";
            //}
            //else if (pE == "Y")
            //{
            //    strWhere += "COM_COL = 'E'";
            //}
            //else if (pC == "Y")
            //{
            //    strWhere += "COM_COL = 'C'";
            //}

            //foreach (DataRow dr in WEBAPP.Helper.ExcelData.TBL_SELECT.Tables[0].Select(strWhere, "IDX ASC"))
            //{
            //    var item = new Dictionary<string, object>();
            //    foreach (DataColumn column in WEBAPP.Helper.ExcelData.TBL_SELECT.Tables[0].Columns)
            //    {
            //        item.Add(column.ColumnName, dr[column.ColumnName]);
            //    }

            //    data.Add(item);
            //}

            return Json(new WEBAPP.Models.AjaxGridResult
            {
                data = WEBAPP.Helper.ExcelData.TBL_SELECT_SUM
            }, JsonRequestBehavior.AllowGet);
        }
    }
}