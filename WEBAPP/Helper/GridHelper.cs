using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using UtilityLib;

namespace WEBAPP.Helper
{
    public static class GridHelper
    {
        public static MvcHtmlString GridScript<TModel>(
            this HtmlHelper<TModel> html,
            string name,
            GridConfig config,
            params GridColumn[] columns)
        {
            var model = html.ViewData.Model;
            //var IsDefaultSearch = Convert.ToBoolean(model.GetType().GetProperty("IsDefaultSearch").GetValue(model, null));
            if (config == null)
            {
                config = new GridConfig();
            }
            var prgConfig = (DataAccess.SEC.MenuModel)html.ViewContext.HttpContext.Session[SessionSystemName.SYS_CurrentPrgConfig];

            var sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");

            #region bindGrid
            sb.AppendLine("var Grid" + name + " = {};");
            sb.AppendLine("function bindGrid" + name + "(result) {");

            sb.AppendLine("var option = {");
            sb.AppendLine("\"deferRender\": true,");
            if (!config.Paging)
            {
                sb.AppendLine("\"paging\": false,");
            }
            sb.AppendLine("\"processing\": true,");
            if (config.ServerSide)
            {
                sb.AppendLine("\"serverSide\": true,");
            }
            if (config.PageLength != null)
            {
                sb.Append("\"pageLength\": \"");
                sb.Append(config.PageLength.AsString());
                sb.AppendLine("\",");
            }
            sb.AppendLine("\"destroy\": !$.isEmptyObject(Grid" + name + "),");
            if (!config.Searching)
            {
                sb.AppendLine("\"searching\": false,");
            }
            if (!config.OninitComplete.IsNullOrEmpty())
            {
                sb.AppendLine("\"tfoot\": true, ");
            }
            if (config.ScrollX)
            {
                sb.AppendLine("\"scrollX\": true,");
            }
            if (!config.ScrollY.IsNullOrEmpty())
            {
                sb.Append("\"scrollY\": '");
                sb.Append(config.ScrollY.AsString());
                sb.AppendLine("px',");
            }
            if (config.ScrollCollapse)
            {
                sb.Append("\"scrollCollapse\": true,");
            }
            if (!config.DataSrc.IsNullOrEmpty())
            {
                sb.AppendLine("\"rowReorder\": {");
                sb.AppendLine("selector: 'tr',");
                sb.AppendLine("dataSrc: '" + config.DataSrc + "'");
                sb.AppendLine("},");
            }

            sb.Append("\"dom\": \"");
            if (config.Searching)
            {
                sb.Append("<'row'<'col-md-6'><'col-md-6'f>>");
            }
            if (config.TableResponsive)
            {
                sb.Append("<'table-responsive'tr>");
            }
            else
            {
                sb.Append("<'row'tr>");
            }
            sb.Append("<'row'<'col-md-5'");
            if (config.VisibleExportButton || config.CustomsExportConfig != null)
            {
                sb.Append("B");
            }
            sb.Append("<'pull-left'l><'pull-left'i>>");
            if (config.Paging)
            {
                sb.Append("<'col-md-7'p>");
            }
            sb.AppendLine(">\",");
            sb.AppendLine("\"language\": {");
            sb.Append("\"lengthMenu\": \"");
            sb.Append(Translation.CenterLang.Center.gridLengthMenu);
            sb.AppendLine("\",");
            sb.Append("\"emptyTable\": \"");
            sb.Append(Translation.CenterLang.Center.gridEmptyTable);
            sb.AppendLine("\",");
            sb.Append("\"info\": \"");
            sb.Append(Translation.CenterLang.Center.gridInfo);
            sb.AppendLine("\",");
            sb.Append("\"infoEmpty\": \"");
            sb.Append(Translation.CenterLang.Center.gridInfoEmpty);
            sb.AppendLine("\",");
            if (config.Searching)
            {
                sb.Append("\"infoFiltered\": \"");
                sb.Append(Translation.CenterLang.Center.gridInfoFiltered);
                sb.AppendLine("\",");
                sb.Append("\"search\": \"");
                sb.Append(Translation.CenterLang.Center.Search);
                sb.AppendLine("\",");
            }
            sb.Append("\"loadingRecords\": \"");
            sb.Append(Translation.CenterLang.Center.gridLoadingRecords);
            sb.AppendLine("\",");
            sb.Append("\"processing\": \"");
            sb.Append(Translation.CenterLang.Center.gridProcessing);
            sb.AppendLine("\",");
            if (config.Paging)
            {
                sb.AppendLine("\"paginate\": {");
                sb.Append("\"first\": \"");
                sb.Append(Translation.CenterLang.Center.gridPaginateFirst);
                sb.AppendLine("\",");
                sb.Append("\"last\": \"");
                sb.Append(Translation.CenterLang.Center.gridPaginateLast);
                sb.AppendLine("\",");
                sb.Append("\"next\": \"");
                sb.Append(Translation.CenterLang.Center.gridPaginateNext);
                sb.AppendLine("\",");
                sb.Append("\"previous\": \"");
                sb.Append(Translation.CenterLang.Center.gridPaginatePrevious);
                sb.AppendLine("\"");
                sb.AppendLine("},");
                sb.AppendLine("},");
            }
            else
            {
                sb.AppendLine("},");
            }
            if (config.VisibleExportButton || config.CustomsExportConfig != null)
            {
                sb.AppendLine("buttons: [");
                if (config.VisibleExportButton && !config.ServerSide)
                {
                    sb.AppendLine("{");
                    sb.AppendLine("extend: 'excel',");
                    sb.AppendLine("text: '<i class=\"fa fa-file-excel-o bigger-120 green\"></i>',");
                    sb.AppendLine("className: 'btn btn-xs btn-white btn-success btn-round',");
                    sb.AppendLine("exportOptions: { columns: ExportNotIcon },");
                    //sb.AppendLine("customizeData: ExportCellFormatText");
                    sb.AppendLine("charset: 'UTF16-LE',");
                    sb.Append("}");
                }

                if (config.CustomsExportConfig != null)
                {
                    sb.AppendLine(",");
                    sb.AppendLine("{");
                    sb.AppendLine("className: 'btn btn-default btn-xs btn-white btn-success btn-round',");
                    sb.AppendLine("text: '<i class=\"fa fa-file-excel-o bigger-120 green\" data-toggle=\"tooltip\" data-placement=\"bottom\" title=\"\"></i>',");
                    sb.AppendLine("action: function ( e, dt, node, config ) {");
                    sb.AppendLine("window.location.href = '" + config.CustomsExportConfig.Url + "';");
                    sb.AppendLine("}");
                    sb.AppendLine("}");
                }
                sb.AppendLine("],");
            }
            int startIdx = 0;
            var col = columns.Where(m => m != null && m.visible != "false").ToList();

            if (config.VisibleCheckBox || (config.DeleteConfig != null && prgConfig.IsROLE_DEL))
            {
                sb.AppendLine("select: {");
                if (config.IsSingleRowSelect)
                {
                    sb.AppendLine("style: 'single',");
                }
                else
                {
                    sb.AppendLine("style: 'multi',");
                }
                sb.AppendLine("selector: 'td:first-child'");
                sb.AppendLine("},");
            }
            sb.AppendLine("\"columns\": [");

            if (config.VisibleCheckBox || (config.DeleteConfig != null && prgConfig.IsROLE_DEL))
            {
                sb.AppendLine("{");
                sb.AppendLine("data: null,");
                sb.AppendLine("defaultContent: '',");
                sb.AppendLine("width: '2%',");
                sb.AppendLine("className: 'dt-head-center select-checkbox',");
                sb.AppendLine("orderable: false,");
                sb.AppendLine("title: '<input type=\"checkbox\" name=\"selectAllGrid" + name + "\" id=\"selectAllGrid" + name + "\" class=\"checkbox\">'");
                sb.Append("}");
                startIdx++;
            }

            var orderIdx = new List<int>();
            if (col != null)
            {
                if (startIdx > 0)
                {
                    sb.AppendLine(",");
                }
                bool incStartIdx = true;
                int c = 0;
                foreach (var dtC in col)
                {
                    if (incStartIdx && !dtC.IsButtonColumn)
                    {
                        incStartIdx = false;
                    }
                    if (incStartIdx)
                    {
                        startIdx++;
                    }
                    if (dtC.IsOrderColumn)
                    {
                        orderIdx.Add(startIdx + (c - 1));
                    }
                    c++;
                    sb.AppendLine("{");

                    var dataValue = dtC.GetType().GetProperty("data").GetValue(dtC, null);
                    if (dataValue == null)
                    {
                        sb.Append("\"data\":null");
                    }
                    else
                    {
                        sb.Append("\"data\":\"" + dataValue + "\"");
                    }

                    var allProperties = dtC.GetType().GetProperties().Where(m =>
                                                                        m.GetValue(dtC, null) != null &&
                                                                        m.Name != "IsKey" &&
                                                                        m.Name != "CaseSensitive" &&
                                                                        m.Name != "IsOrderColumn" &&
                                                                        m.Name != "IsButtonColumn" &&
                                                                        m.Name != "data" &&
                                                                        m.Name != "IsEditable" &&
                                                                        m.Name != "SelectOptions" &&
                                                                        m.Name != "FileType" &&
                                                                        m.Name != "IsRequired" &&
                                                                        m.Name != "CustomTrigger" &&
                                                                        m.Name != "EditableReadOnly" &&
                                                                        m.Name != "AutocompleteConfig" &&
                                                                        m.Name != "MaxLength" &&
                                                                        m.Name != "IsIdCard" &&
                                                                        m.Name != "CustomOptionEditable" &&
                                                                        m.Name != "IsHeadNoWrap").ToList();
                    if (allProperties.Count > 0)
                    {
                        sb.AppendLine(",");
                    }
                    int i = 0;
                    foreach (var prop in allProperties)
                    {
                        i++;
                        var value = prop.GetValue(dtC, null);
                        if (prop.PropertyType == typeof(string))
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(value)))
                            {
                                if (prop.Name == "render")
                                {
                                    sb.Append("\"" + prop.Name + "\":" + value + "");
                                }
                                else if (prop.Name == "width" && config.WidthType == ColumnsWidthType.Percentage && !value.AsString().Contains("%"))
                                {
                                    sb.Append("\"" + prop.Name + "\":\"" + value + "%\"");
                                }
                                else
                                {
                                    if (!(config.IsCustomsTitle && prop.Name == "title"))
                                    {
                                        sb.Append("\"" + prop.Name + "\":\"" + value + "\"");
                                    }
                                }
                            }
                            else
                            {
                                if (!(config.IsCustomsTitle && prop.Name == "title"))
                                {
                                    sb.Append("\"" + prop.Name + "\":\"" + value + "\"");
                                }
                            }
                        }
                        else if (prop.PropertyType == typeof(bool))
                        {
                            sb.Append("\"" + prop.Name + "\":" + Convert.ToString(value).ToLower());
                        }
                        else if (prop.Name == "type")
                        {
                            if ((ColumnsType)value != ColumnsType.None &&
                                (ColumnsType)value != ColumnsType.File &&
                                (ColumnsType)value != ColumnsType.FileMultiple &&
                                (ColumnsType)value != ColumnsType.Select &&
                                (ColumnsType)value != ColumnsType.Checkbox &&
                                (ColumnsType)value != ColumnsType.Autocomlete)
                            {
                                sb.Append("\"" + prop.Name + "\":\"" + ((ColumnsType)value).GetDescription() + "\"");
                            }
                        }
                        else
                        {
                            sb.Append("\"" + prop.Name + "\":\"" + Convert.ToString(value).ToLower() + "\"");
                        }
                        if (i != allProperties.Count)
                        {
                            if (prop.Name == "type" && ((ColumnsType)value != ColumnsType.None &&
                                (ColumnsType)value != ColumnsType.File &&
                                (ColumnsType)value != ColumnsType.FileMultiple &&
                                (ColumnsType)value != ColumnsType.Select &&
                                (ColumnsType)value != ColumnsType.Checkbox &&
                                (ColumnsType)value != ColumnsType.Autocomlete))
                            {
                                sb.AppendLine(",");
                            }
                            else if (prop.Name != "type")
                            {
                                if (!(config.IsCustomsTitle && prop.Name == "title"))
                                {
                                    sb.AppendLine(",");
                                }
                            }
                        }
                    }
                    sb.Append("}");

                    if (c != col.Count)
                    {
                        sb.AppendLine(",");
                    }
                }
            }

            sb.Append("]");
            sb.AppendLine(",");
            //sb.AppendLine("\"fixedColumns\": {");
            //sb.Append("\"leftColumns\":");
            //sb.AppendLine(Convert.ToString(startIdx));
            //sb.Append("}");
            //sb.AppendLine(",");
            if (!config.DisableDefaultSorting)
            {
                var hasOrderCol = columns.Where(m => m != null && m.visible != "false" && m.IsOrderColumn).Any();
                if (hasOrderCol)
                {
                    sb.Append("\"order\": [");
                    var i = 0;
                    foreach (var item in orderIdx)
                    {
                        sb.Append("[");
                        sb.Append(item);
                        sb.Append(",'asc']");
                        i++;
                        if (i != orderIdx.Count)
                        {
                            sb.Append(",");
                        }
                    }
                    sb.AppendLine("],");
                }
                else
                {
                    sb.Append("\"order\": [[");
                    sb.Append(startIdx);
                    sb.AppendLine(",'asc']],");
                }
            }
            else
            {
                sb.Append("\"order\": [],");
            }
            if (!config.OnRowCallback.IsNullOrEmpty())
            {
                //function( row, data, index )
                sb.Append("\"rowCallback\":");
                sb.Append(config.OnRowCallback);
                sb.AppendLine(",");
            }

            if (!config.OnPreDrawCallback.IsNullOrEmpty())
            {
                //function( settings )
                sb.Append("\"preDrawCallback\":");
                sb.Append(config.OnPreDrawCallback);
                sb.AppendLine(",");
            }
            sb.AppendLine("drawCallback: function () {");
            sb.AppendLine("if ($('#Grid" + name + " tbody .dataTables_empty').length) {");
            if (config.VisibleCheckBox || (config.DeleteConfig != null && prgConfig.IsROLE_DEL))
            {
                sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", false);");
            }
            sb.AppendLine("$('#Grid" + name + "_wrapper').find('.dt-buttons').addClass('hidden');");
            if (config.DeleteConfig != null && prgConfig.IsROLE_DEL)
            {
                sb.AppendLine("$('#Delete" + name + "').hide();");
            }
            sb.AppendLine("}");
            sb.AppendLine("else {");
            sb.AppendLine("$('#Grid" + name + "_wrapper').find('.dt-buttons').removeClass('hidden');");
            if (config.DeleteConfig != null && prgConfig.IsROLE_DEL)
            {
                sb.AppendLine("$('#Delete" + name + "').show();");
            }
            sb.AppendLine("}");
            sb.AppendLine("$('[data-toggle=\"tooltip\"]').tooltip({");
            sb.AppendLine("trigger: 'hover'");
            sb.AppendLine("});");
            if (!config.OnDrawCallback.IsNullOrEmpty())
            {
                sb.Append(config.OnDrawCallback);
                sb.AppendLine("();");
            }
            sb.AppendLine("},");

            if (!config.OninitComplete.IsNullOrEmpty())
            {
                sb.AppendLine("\"initComplete\": function(settings, json) {");
                sb.AppendLine(config.OninitComplete + "(settings, json);");
                sb.AppendLine("}");
            }

            sb.AppendLine("};");

            sb.AppendLine("if (result.mode != \"standby\") {");
            sb.AppendLine("$(\"#rowGrid" + name + "\").removeClass(\"hidden\");");
            if (config.IsAjax && config.GetConfig != null)
            {
                if (config.GetConfig.Parameters != null && config.GetConfig.Parameters.Where(m => m.Type == VSMParameterType.ByControlId || m.Type == VSMParameterType.ByModelData).Any())
                {
                    foreach (var item in config.GetConfig.Parameters.Where(m => m.Type == VSMParameterType.ByControlId || m.Type == VSMParameterType.ByModelData))
                    {
                        if (item.Type == VSMParameterType.ByModelData)
                        {
                            sb.Append("result['");
                            sb.Append(item.Name);
                            sb.Append("'] = '");
                            sb.Append(item.Value);
                            sb.AppendLine("';");
                        }
                        else if (item.Type == VSMParameterType.ByControlId)
                        {
                            sb.Append("result['");
                            sb.Append(item.Name);
                            sb.Append("'] = $('#");
                            sb.Append(item.Value);
                            sb.AppendLine("').val();");
                        }
                    }
                }
                sb.AppendLine("option[\"ajax\"]= {");
                sb.Append("url: \"");
                sb.Append(config.GetConfig.Url);
                sb.AppendLine("\",");
                sb.AppendLine("type: \"post\",");
                sb.AppendLine("error: OnAjaxError,");
                sb.AppendLine("data: result");
                sb.AppendLine("};");
            }
            else if (config.Data != null)
            {
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(config.Data);
                sb.Append("var rawData = ");
                sb.Append(data);
                sb.AppendLine(";");
                sb.AppendLine("option[\"data\"]= rawData;");
            }
            sb.AppendLine("}");
            sb.AppendLine("Grid" + name + " = $(\"#Grid" + name + "\").DataTable(option);");

            if (config.VisibleCheckBox || (config.DeleteConfig != null && prgConfig.IsROLE_DEL))
            {
                sb.AppendLine("$('#Grid" + name + "').on('init.dt', function () {");
                sb.AppendLine("var rows = Grid" + name + ".rows({ selected: true });");
                sb.AppendLine("rows.select();");
                sb.AppendLine("var totalSelected = rows.data().count();");
                sb.AppendLine("var total = Grid" + name + ".rows().data().count();");
                sb.AppendLine("if (total > 0 && totalSelected == total) {");
                sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", true);");
                sb.AppendLine("}");
                sb.AppendLine("else {");
                sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", false);");
                sb.AppendLine("}");
                if (!config.OnInit.IsNullOrEmpty())
                {
                    sb.Append(config.OnInit);
                    sb.AppendLine("();");
                }
                sb.AppendLine("});");

                sb.Append("$(\"#selectAllGrid" + name + "\").change(");
                if (!config.OnSelectAll.IsNullOrEmpty())
                {
                    sb.Append(config.OnSelectAll);
                }
                else
                {
                    sb.AppendLine("function () {");
                    sb.AppendLine("if (this.checked) {");
                    sb.AppendLine("Grid" + name + ".rows().select();");
                    sb.AppendLine("}");
                    sb.AppendLine("else {");
                    sb.AppendLine("Grid" + name + ".rows().deselect();");
                    sb.AppendLine("}");
                    sb.Append("}");
                }
                sb.AppendLine(");");
                sb.AppendLine("Grid" + name);
                if (!config.OnUserSelect.IsNullOrEmpty())
                {
                    sb.Append(".on('user-select',");
                    //function ( e, dt, type, cell, originalEvent )
                    sb.Append(config.OnUserSelect);
                    sb.AppendLine(")");
                }
                sb.AppendLine(".on('select', function (e, dt, type, indexes) {");
                sb.AppendLine("var allRows = Grid" + name + ".rows().data().count();");
                sb.AppendLine("var selectedRows = Grid" + name + ".rows({ selected: true }).data().count();");
                sb.AppendLine("if (allRows == selectedRows) {");
                sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", true);");
                sb.AppendLine("}");
                if (!config.OnSelect.IsNullOrEmpty())
                {
                    sb.Append(config.OnSelect);
                    sb.AppendLine("(e, dt, type, indexes);");
                }
                sb.AppendLine("})");
                sb.AppendLine(".on('deselect', function (e, dt, type, indexes) {");
                sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", false);");
                if (!config.OnDeSelect.IsNullOrEmpty())
                {
                    sb.Append(config.OnDeSelect);
                    sb.AppendLine("(e, dt, type, indexes);");
                }
                sb.AppendLine("});");
            }
            if (!config.OnAfterBinding.IsNullOrEmpty())
            {
                sb.Append(config.OnAfterBinding);
                sb.AppendLine("();");
            }
            sb.AppendLine("}");
            #endregion

            #region Delete
            if (config.DeleteConfig != null && prgConfig.IsROLE_DEL)
            {
                sb.AppendLine("function onDelete" + name + "(button) {");
                sb.AppendLine("var data = Grid" + name + ".rows({ selected: true }).data();");
                sb.AppendLine("if (data != undefined && data != null && data.length > 0) {");
                sb.AppendLine("var nData = { data: [] ,button:button[0].name};");
                sb.AppendLine("$.each(data, function (i, item) {");
                sb.AppendLine("nData.data.push({");
                if (config.DeleteConfig.Parameters != null)
                {
                    var i = 0;
                    foreach (var item in config.DeleteConfig.Parameters)
                    {
                        if (item.Type == VSMParameterType.ByControlId)
                        {
                            sb.Append(item.Name + ":$('#" + item.Value + "').val()");
                        }
                        else if (item.Type == VSMParameterType.ByModelData)
                        {
                            sb.Append(item.Name + ":'" + item.Value + "'");
                        }
                        i++;
                        if (i != config.DeleteConfig.Parameters.Count)
                        {
                            sb.AppendLine(",");
                        }
                    }
                }
                var colKey = columns.Where(m => m.IsKey).ToList();
                if (colKey != null && colKey.Count > 0)
                {
                    if (config.DeleteConfig.Parameters != null && config.DeleteConfig.Parameters.Count > 0)
                    {
                        sb.AppendLine(",");
                    }
                    var i = 0;
                    foreach (var item in colKey)
                    {
                        if (item.type == ColumnsType.Date || item.type == ColumnsType.DateTime)
                        {
                            sb.Append(item.data + ":$.gridDateToISOString(item." + item.data + ")");
                        }
                        else
                        {
                            sb.Append(item.data + ":item." + item.data);
                        }
                        i++;
                        if (i != colKey.Count)
                        {
                            sb.AppendLine(",");
                        }
                    }
                }
                sb.AppendLine("});");
                sb.AppendLine("});");

                sb.AppendLine("$.ajax({");
                sb.Append("url: \"");
                sb.Append(config.DeleteConfig.Url);
                sb.AppendLine("\",");
                sb.AppendLine("type: \"post\",");
                sb.AppendLine("data: nData,");
                sb.AppendLine("success: OnAjaxSuccess,");
                sb.AppendLine("error: OnAjaxError");
                sb.AppendLine("});");
                sb.AppendLine("}");
                sb.AppendLine("else {");
                sb.AppendLine("var result = {");
                sb.AppendLine("Mode: 'Delete',");
                sb.AppendLine("Status: false,");
                sb.Append("Message: '");
                sb.Append(Translation.CenterLang.Center.PleaseSelectData);
                sb.AppendLine("',");
                sb.AppendLine("Style: 'danger'");
                sb.AppendLine("};");
                sb.AppendLine("OnAjaxSuccess(result);");
                sb.AppendLine("}");
                sb.AppendLine("}");
            }
            #endregion

            #region ready
            if ((config.ServerSide && config.DefaultBinding) || !config.ServerSide)
            {
                sb.AppendLine("$(document).ready(function () {");
                if (config.DefaultBinding)
                {
                    sb.AppendLine("bindGrid" + name + "({page:1});");
                }
                else
                {
                    sb.AppendLine("bindGrid" + name + "({mode:'standby'});");
                }
                sb.AppendLine("});");
            }
            else
            {
                if (!config.DefaultBinding)
                {
                    sb.AppendLine("$(document).ready(function () {");
                    sb.AppendLine("$('#DeleteSearch').hide();");
                    sb.AppendLine("});");
                }
            }
            #endregion

            sb.AppendLine("</script>");
            return MvcHtmlString.Create(sb.ToString());
        }

        public static GridColumn GridColumnCommand<TModel>(this HtmlHelper<TModel> html, ButtonConfig command, params ButtonConfig[] commands)
        {
            var prgConfig = (DataAccess.SEC.MenuModel)html.ViewContext.HttpContext.Session[SessionSystemName.SYS_CurrentPrgConfig];
            if (prgConfig == null)
            {
                prgConfig = new DataAccess.SEC.MenuModel();
            }
            bool existsCommand = false;
            var col = new GridColumn();
            col.width = "3%";
            col.orderable = false;
            col.IsButtonColumn = true;
            col.className = ColumnsTextAlign.Center.GetDescription();
            var sb = new StringBuilder();
            sb.AppendLine("function (data, type, full, meta) {");
            sb.AppendLine("var tag = '';");
            bool genScript = true;
            if ((command.Name == StandardButtonName.Edit && !prgConfig.IsROLE_EDIT) ||
                (command.Name == StandardButtonName.Info && !prgConfig.IsROLE_PRINT))
            {
                genScript = false;
            }
            if (genScript)
            {
                existsCommand = true;
                if (!command.Condition.IsNullOrEmpty())
                {
                    sb.Append("if(");
                    sb.Append(command.Condition);
                    sb.AppendLine("){");
                }
                sb.Append("tag += '<p class=\"no-margin\"><a name=\"");
                sb.Append(command.Name);
                sb.Append("\"");
                if (command.Tooltip)
                {
                    sb.Append(" data-toggle=\"tooltip\" data-placement=\"right\"");
                }
                if (!command.Title.IsNullOrEmpty())
                {
                    sb.Append(" title=\"");
                    sb.Append(command.Title);
                    sb.Append("\"");
                }
                if (command.HtmlAttribute.Count > 0)
                {
                    foreach (var item in command.HtmlAttribute)
                    {
                        sb.Append(" ");
                        sb.Append(item.Key);
                        sb.Append("=\"");
                        sb.Append(item.Value);
                        sb.Append("\"");
                    }
                }
                sb.AppendLine("';");
                sb.Append("tag +=' href=\"");
                sb.Append(command.Url);
                if (command.Parameters != null && command.Parameters.Count > 0)
                {
                    sb.AppendLine("?';");
                    var i = 0;
                    foreach (var cmd in command.Parameters)
                    {
                        sb.Append("tag += '");
                        if (i != 0)
                        {
                            sb.Append("&");
                        }
                        sb.Append(cmd.Name);
                        sb.Append("='+ toUrlString(");
                        if (cmd.Type == VSMParameterType.ByGridDate)
                        {
                            sb.Append("$.gridDateToISOString(full." + (cmd.Value.IsNullOrEmpty() ? cmd.Name : cmd.Value) + ")");
                        }
                        else if (cmd.Type == VSMParameterType.ByModelData)
                        {
                            sb.Append("'");
                            sb.Append(cmd.Value);
                            sb.Append("'");
                        }
                        else if (cmd.Type == VSMParameterType.ByControlId)
                        {
                            sb.Append("$('#");
                            sb.Append(cmd.Value);
                            sb.Append("').val()");
                        }
                        else
                        {
                            sb.Append("full." + (cmd.Value.IsNullOrEmpty() ? cmd.Name : cmd.Value));
                        }
                        sb.AppendLine(");");
                        i++;
                    }
                    sb.AppendLine("tag +='\"';");
                }
                else
                {
                    sb.AppendLine("\"';");
                }
                sb.Append("tag +='>");
                if (!command.IconCssClass.IsNullOrEmpty())
                {
                    sb.Append("<i class=\"ace-icon fa");
                    sb.Append(" " + command.IconCssClass);
                    sb.Append(" bigger-130");
                    if (command.IconColor != TextColor.None)
                    {
                        sb.Append(" " + command.IconColor.GetDescription());
                    }
                    sb.Append("\"></i>");
                }
                if (!command.Text.IsNullOrEmpty())
                {
                    sb.Append(command.Text);
                }
                sb.AppendLine("</a></p>';");
                if (!command.Condition.IsNullOrEmpty())
                {
                    sb.AppendLine("}");
                }
            }
            if (commands != null)
            {
                foreach (var item in commands)
                {
                    genScript = true;
                    if ((item.Name == StandardButtonName.Edit && !prgConfig.IsROLE_EDIT) ||
                        (item.Name == StandardButtonName.Info && !prgConfig.IsROLE_PRINT))
                    {
                        genScript = false;
                    }
                    if (genScript)
                    {
                        existsCommand = true;
                        if (!item.Condition.IsNullOrEmpty())
                        {
                            sb.Append("if(");
                            sb.Append(item.Condition);
                            sb.AppendLine("){");
                        }
                        sb.Append("tag += '<p class=\"no-margin\"><a name=\"");
                        sb.Append(item.Name);
                        sb.Append("\"");
                        if (command.Tooltip)
                        {
                            sb.Append(" data-toggle=\"tooltip\" data-placement=\"right\"");
                        }
                        if (!item.Title.IsNullOrEmpty())
                        {
                            sb.Append("title=\"" + item.Title + "\"");
                        }

                        sb.AppendLine("';");
                        sb.Append("tag +=' href=\"");
                        sb.Append(item.Url);
                        if (item.Parameters != null && item.Parameters.Count > 0)
                        {
                            sb.AppendLine("?';");
                            var i = 0;
                            foreach (var cmd in item.Parameters)
                            {
                                sb.Append("tag += '");
                                if (i != 0)
                                {
                                    sb.Append("&");
                                }
                                sb.Append(cmd.Name);
                                sb.Append("='+ toUrlString(");
                                if (cmd.Type == VSMParameterType.ByGridDate)
                                {
                                    sb.Append("$.gridDateToISOString(full." + (cmd.Value.IsNullOrEmpty() ? cmd.Name : cmd.Value) + ")");
                                }
                                else if (cmd.Type == VSMParameterType.ByModelData)
                                {
                                    sb.Append(cmd.Value);
                                }
                                else if (cmd.Type == VSMParameterType.ByControlId)
                                {
                                    sb.Append("$('#");
                                    sb.Append(cmd.Value);
                                    sb.Append("').val()");
                                }
                                else
                                {
                                    sb.Append("full." + (cmd.Value.IsNullOrEmpty() ? cmd.Name : cmd.Value));
                                }
                                sb.AppendLine(");");
                                i++;
                            }
                            sb.AppendLine("tag +='\"';");
                        }
                        else
                        {
                            sb.AppendLine("\"';");
                        }
                        sb.Append("tag +='>");
                        if (!item.IconCssClass.IsNullOrEmpty())
                        {
                            sb.Append("<i class=\"ace-icon fa");
                            sb.Append(" " + item.IconCssClass);
                            sb.Append(" bigger-130");
                            if (item.IconColor != TextColor.None)
                            {
                                sb.Append(" " + item.IconColor.GetDescription());
                            }
                            sb.Append("\"></i>");
                        }
                        if (!item.Text.IsNullOrEmpty())
                        {
                            sb.Append(item.Text);
                        }
                        sb.AppendLine("</a></p>';");
                        if (!item.Condition.IsNullOrEmpty())
                        {
                            sb.AppendLine("}");
                        }
                    }
                }
            }

            sb.AppendLine("return tag;");
            sb.Append("}");
            col.render = sb.ToString();
            if (!existsCommand)
            {
                col = null;
            }
            return col;
        }
        public static ButtonConfig GridCommand<TModel>(this HtmlHelper<TModel> html, string name, string title, string url, string iconCssClass = "", TextColor iconColor = TextColor.None, params VSMParameter[] param)
        {
            return GridCommand(name, title, url, iconCssClass, iconColor, null, param);
        }
        public static ButtonConfig GridCommand<TModel>(this HtmlHelper<TModel> html, string name, string title, string url, string iconCssClass = "", TextColor iconColor = TextColor.None, string condition = null, params VSMParameter[] param)
        {
            return GridCommand(name, title, url, iconCssClass, iconColor, condition, param);
        }
        public static ButtonConfig GridCommand(string name, string title, string url, string iconCssClass = "", TextColor iconColor = TextColor.None, string condition = null, params VSMParameter[] param)
        {
            var lnk = new ButtonConfig(name);
            lnk.Title = title;
            lnk.Url = url;
            lnk.IconCssClass = iconCssClass;
            lnk.IconColor = iconColor;
            lnk.Condition = condition;
            lnk.Tooltip = true;
            if (param != null)
            {
                lnk.AddParameter(param);
            }
            return lnk;
        }

        public static object GetValue<TModel, TValue>(this ViewDataDictionary<TModel> viewData, Expression<Func<TModel, TValue>> expression)
        {
            return ModelMetadata.FromLambdaExpression(expression, viewData).Model;
        }
    }
}