using DataAccess.Ux;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UtilityLib;
using WEBAPP.Controllers;

namespace WEBAPP.Areas.Ux.Controllers
{
    public class AutocompleteController : BaseController
    {
        // GET: Ux/Autocomplete
        public JsonResult Index(string keySource, string searchTerm, string sort, int pageIndex = 0, int pageSize = 10, string extraParam = "")
        {
            var da = new AutocompleteDA();
            da.DTO.Execute.ExecuteType = AutocompleteExecuteType.GetAll;
            da.DTO.pageSize = pageSize;
            da.DTO.pageIndex = pageIndex;
            da.DTO.Parameter.KeySource = keySource;
            da.DTO.Parameter.SearchTerm = searchTerm;
            if (!extraParam.IsNullOrEmpty())
            {
                var paramValue = string.Empty;
                var param = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(extraParam);
                foreach (var item in param)
                {
                    paramValue += item.Value + ",";
                }
                da.DTO.Parameter.ParameterValue = paramValue.TrimEnd(',');
            }
            da.DTO.Parameter.Sort = sort;
            da.SelectNoEF(da.DTO);
            return Json(da.DTO, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetDataMultiSelect(string keySource, string extraGridShow = "", string extraParam = "", int start = 0, int length = 10, int draw = 1, List<AutocompleteSearchModel> columns = null)
        {
            var da = new AutocompleteDA();

            #region get value column search
            string strColSearch = "";
            if (columns != null)
            {
                foreach (var col in columns)
                {
                    if (!col.search.value.IsNullOrEmpty())
                    {
                        strColSearch += " and " + col.data + " like '%" + col.search.value + "%'";
                    }
                }
            }
            #endregion

            #region insert table notin
            string ClientID = System.Guid.NewGuid().ToString();
            da.DTO.Parameter.ClientID = ClientID;

            if (!extraGridShow.IsNullOrEmpty())
            {
                var DataNotIn = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(extraGridShow);
                da.DTO.Parameter.DataNotIn = new List<AutocompleteNotInParameterModel>();
                foreach (var item in DataNotIn)
                {
                    string columnName = "";
                    string dataValue = "";
                    foreach (var key in item.Keys)
                    {
                        columnName += key.ToString() + ",";
                        dataValue += item[key].ToString().Trim();
                    }
                    da.DTO.Parameter.DataNotIn.Add(new AutocompleteNotInParameterModel() { COLUMN_NAME = columnName.TrimEnd(','), DATA_VALUE = dataValue });
                }

                SetStandardField(da.DTO.Parameter);

                da.DTO.Execute.ExecuteType = AutocompleteExecuteType.InsertNotInTable;
                da.InsertNoEF(da.DTO);
            }
            #endregion

            da.DTO.Execute.ExecuteType = AutocompleteExecuteType.GetDataOnly;
            da.DTO.Parameter.KeySource = keySource;
            if (!extraParam.IsNullOrEmpty())
            {
                var paramValue = string.Empty;
                var param = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(extraParam);
                foreach (var item in param)
                {
                    paramValue += item.Value + ",";
                }
                da.DTO.Parameter.ParameterValue = paramValue.TrimEnd(',');
            }

            da.DTO.pageSize = length;
            //Fix Bug start ส่ง 0 ใน sp เอา -1 ออก
            da.DTO.pageIndex = (start / length);
            da.DTO.Parameter.FitterData = strColSearch;

            da.SelectNoEF(da.DTO);

            #region delete table not in
            da.DeleteNoEF(da.DTO);
            #endregion

            #region not in by code
            //var data = new List<Dictionary<string, object>>();
            //if (da.DTO.rows != null && !extraGridShow.IsNullOrEmpty())
            //{
            //    var dataGird = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(extraGridShow);

            //    foreach (var dic in da.DTO.rows)
            //    {
            //        List<bool> chk = new List<bool>();
            //        foreach (var item in dataGird)
            //        {
            //            var exist = true;
            //            foreach (var key in item.Keys)
            //            {
            //                if (dic[key].AsString() != item[key].AsString())
            //                {
            //                    exist = false;
            //                    break;
            //                }
            //            }
            //            chk.Add(exist);
            //        }
            //        if (!chk.Any(m => m))
            //        {
            //            data.Add(dic);
            //        }
            //    }
            //}
            //else
            //{
            //    data = da.DTO.rows;
            //}
            #endregion

            var data = new List<Dictionary<string, object>>();
            data = da.DTO.rows;

            return JsonAllowGet(data, da.DTO.Result, da.DTO.totalcount, draw);

            //return Json(new WEBAPP.Models.AjaxGridResult
            //{
            //    data = data
            //}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsValidStore(string keySource, string parameterValue)
        {
            var status = true;
            var da = new AutocompleteDA();
            da.DTO.Execute.ExecuteType = AutocompleteExecuteType.GetValidate;
            da.DTO.Parameter.KeySource = keySource;
            da.DTO.Parameter.ParameterValue = parameterValue;
            da.SelectNoEF(da.DTO);

            if (da.DTO.rows != null && da.DTO.rows.Count > 0)
            {
                if (da.DTO.rows[0]["EXISTDATA"].AsInt() > 0)
                {
                    status = false;
                }
            }

            return Json(new WEBAPP.Models.AjaxResult
            {
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }
    }
}