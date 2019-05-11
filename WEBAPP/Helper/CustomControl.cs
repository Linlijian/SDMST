using DataAccess;
using DataAccess.Ux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using UtilityLib;

namespace WEBAPP.Helper
{
    public static class CustomControl
    {
        #region MultiSelect
        public static MvcHtmlString GetVSMMultiSelectFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string keySource, bool isRequired = false, int labelWidth = 4, bool IsReadOnly = false, bool showLabel = true, FluentBootstrap.TableType type = FluentBootstrap.TableType.Multiselect, string fixColDate = "")
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string label = GetControlLabel(metadata, expressionText);
            string name = GetControlName(html, expressionText);

            var sb = new StringBuilder();
            if (showLabel)
            {
                sb.Append("<div class=\"form-group");
                if (isRequired)
                {
                    sb.Append(" required\" aria-required=\"true");
                }
                sb.AppendLine("\">");
                sb.AppendLine("<label class=\"control-label col-md-" + labelWidth + "\">");
                sb.Append(label);
                sb.Append("</label>");
            }
            else
            {
                labelWidth = 0;
                sb.AppendLine("<div class=\"row\">");
            }
            sb.AppendLine("<div class=\"col-md-" + (12 - labelWidth) + "\">");

            #region tool bar
            sb.AppendLine("<div class=\"gridIncludeToolbar\">");
            sb.AppendLine("<div class=\"row gridToolbar\">");
            sb.AppendLine("<div class=\"col -md-12 no-padding\">");
            if (!IsReadOnly)
            {
                sb.AppendLine("<a id=\"btnAdd" + name + "\" value=\"Add\" class=\"btn btn-xs btn-white btn-round\" href=\"javascript:void(0)\" data-toggle=\"tooltip\" title=\"" + Translation.CenterLang.Center.Add + "\" >");
                sb.AppendLine("<i class=\"ace-icon fa fa-plus-circle fa-lg green bigger-125 icon-only\"></i>");
                sb.AppendLine("</a>");
                sb.AppendLine("<a id=\"btnDelete" + name + "\" value =\"Add\" class=\"btn btn-xs btn-white btn-round\" style=\"display: none;\" href=\"javascript:void(0)\" data-toggle=\"tooltip\" title=\"" + Translation.CenterLang.Center.Delete + "\" >");
                sb.AppendLine("<i class=\"ace-icon fa fa-trash-o fa-lg red bigger-125 icon-only\"></i>");
                sb.AppendLine("</a>");
            }
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            #endregion

            sb.Append("<table id=\"Grid" + name + "\" class=\"table table-striped table-bordered table-hover\" data-tabletype=\"" + type.GetDescription() + "\"");
            if (!IsReadOnly)
            {
                //column pk exp:TEST,TEST2:date
                //Column type without date :TEST
                //Column Type Date :TEST2:date
                string colPk = string.Empty;
                string colPkDate = string.Empty;
                var da = new AutocompleteDA();
                da.DTO.Execute.ExecuteType = AutocompleteExecuteType.GetStructure;
                da.DTO.Parameter.KeySource = keySource;
                da.SelectNoEF(da.DTO);
                if (da.DTO.colKeyModel != null)
                {
                    var cPkd = da.DTO.colKeyModel.Where(m => m.ColumnName.Contains(":")).ToList();
                    if (cPkd.Count > 0)
                    {
                        colPk = string.Join(",", da.DTO.colKeyModel.Where(m => !m.ColumnName.Contains(":")).Select(m => m.ColumnName));
                        colPk += ",";
                        var i = 0;
                        foreach (var item in da.DTO.colKeyModel.Where(m => m.ColumnName.Contains(":")))
                        {
                            i++;
                            var cols = item.ColumnName.Split(':');
                            colPk += cols[0];
                            colPkDate += cols[0];
                            if (i != cPkd.Count)
                            {
                                colPk += ",";
                                colPkDate += ",";
                            }
                        }
                    }
                    else
                    {
                        colPk = string.Join(",", da.DTO.colKeyModel.Select(m => m.ColumnName));
                    }
                }
                sb.Append(" data-col=\"");
                sb.Append(colPk);
                sb.Append("\"");
                sb.Append(" data-coldate=\"");
                sb.Append(colPkDate + fixColDate);
                sb.Append("\"");
            }
            sb.Append(" data-isrequired=\"" + isRequired.ToString().ToLower() + "\"");
            sb.AppendLine(">");
            sb.AppendLine("</table>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString GetVSMMultiSelectModalFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool isSingleSelect = false, ModalSize modalSize = ModalSize.Xl, bool confirmClose = true, string keySource = "")
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string label = GetControlLabel(metadata, expressionText);
            string name = GetControlName(html, expressionText);
            var sb = new StringBuilder();
            if (modalSize == ModalSize.Fullscreen)
            {
                sb.AppendLine("<div class=\"modal fade modal-fullscreen\" id=\"mdAdd" + name + "\" role=\"dialog\">");
                sb.AppendLine("<div class=\"modal-dialog\">");
            }
            else
            {
                sb.AppendLine("<div class=\"modal fade\" id=\"mdAdd" + name + "\" role=\"dialog\">");
                sb.Append("<div class=\"modal-dialog ");
                sb.Append(modalSize.GetDescription());
                sb.AppendLine("\">");
            }
            sb.AppendLine("<div class=\"modal-content\">");
            sb.AppendLine("<div class=\"modal-header\">");
            sb.Append("<button type=\"button\" class=\"close");
            if (confirmClose)
            {
                sb.AppendLine(" std-modal-confirmclose\">&times;</button>");
            }
            else
            {
                sb.AppendLine("\" data-dismiss=\"modal\">&times;</button>");
            }
            sb.AppendLine("<h4 class=\"modal-title\" id=\"mdTitle" + name + "\">");
            sb.Append("</h4>");
            sb.AppendLine("</div>");
            if (!isSingleSelect)
            {
                sb.AppendLine("<div class=\"modal-buttontoolbar\">");
                sb.AppendLine("<a id=\"btnSave" + name + "\" value=\"Save" + name + "\" class=\"btn btn-xs btn-white btn-info btn-round\" href=\"javascript:void(0)\">");
                sb.AppendLine("<i class=\"ace-icon fa fa-save align-top bigger-125\"></i>");
                sb.Append(Translation.CenterLang.Center.Save);
                sb.AppendLine("</a>");
                sb.Append("<a id=\"btnCancel" + name + "\" value=\"Cancel" + name + "\" class=\"btn btn-xs btn-white btn-info btn-round");
                if (confirmClose)
                {
                    sb.AppendLine(" std-modal-confirmcancel\" href=\"javascript:void(0)\">");
                }
                else
                {
                    sb.AppendLine("\" data-dismiss=\"modal\">");
                }
                sb.AppendLine("<i class=\"ace-icon fa fa-remove red2 align-top bigger-125\"></i>");
                sb.Append(Translation.CenterLang.Center.Cancel);
                sb.AppendLine("</a>");
                sb.AppendLine("</div>");
            }
            sb.AppendLine("<div class=\"modal-body\">");
            sb.AppendLine("<div id=\"notAdd" + name + "\" style=\"display: none;\"></div>");
            sb.AppendLine("<div class=\"row\">");
            sb.AppendLine("<div class=\"col-md-12\">");
            sb.AppendLine("<div class=\"table-responsive\">");
            sb.AppendLine("<table id=\"gridAdd" + name + "\" class=\"table table-striped table-bordered table-hover\" width=\"100%\">");
            ///   
            if (!keySource.IsNullOrEmpty())
            {
                var da = new AutocompleteDA();
                da.DTO.Execute.ExecuteType = AutocompleteExecuteType.GetStructure;
                da.DTO.Parameter.KeySource = keySource;
                da.SelectNoEF(da.DTO);

                sb.AppendLine("<thead>");
                sb.AppendLine("<tr data-rowtype=\"filter\">");
                sb.AppendLine("<th rowspan=\"2\"><input type=\"checkbox\" name=\"selectAllgridAdd" + name + "\" id=\"selectAllgridAdd" + name + "\" class=\"checkbox\"></th>");
                for (int i = 0; i < da.DTO.colModel.Count; i++)
                {
                    sb.AppendLine("<th><input type=\"text\" id=\"ftCol" + i.ToString() + "\" class=\"form-control input-sm\"></th>");
                }
                sb.AppendLine("</tr>");
                sb.AppendLine("<tr>");
                for (int i = 0; i < da.DTO.colModel.Count; i++)
                {
                    sb.AppendLine("<th></th>");
                }
                sb.AppendLine("</tr>");
                sb.AppendLine("</thead>");
            }
            ///
            sb.AppendLine("</table>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString GetVSMMultiSelectScriptFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, VSMMultiSelectConfig config, bool visibleCheckBox = true, bool IsReadOnly = false, bool dataEmptyHideGrid = false, bool isSingleSelect = false)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string label = GetControlLabel(metadata, expressionText);
            string name = GetControlName(html, expressionText);
            var sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("var Grid" + name + " = {};");
            sb.AppendLine("var gridAdd" + name + " = {};");
            if (!IsReadOnly)
            {
                if (!isSingleSelect)
                {
                    #region OnClick_btnSave
                    sb.AppendLine("function OnClick_btnSave" + name + "() {");
                    sb.AppendLine("var btn = this.$target;");
                    sb.AppendLine("var data = gridAdd" + name + ".rows({ selected: true }).data();");
                    sb.AppendLine("if (data.length > 0) {");

                    if (config.CustomsDataOnSave.IsNullOrEmpty())
                    {
                        sb.AppendLine("$.each(data, function(index, value) {");
                        foreach (var col in config.ColumnsShow)
                        {
                            if (col.type == ColumnsType.Date)
                            {
                                sb.AppendLine("var date = $.stringToDate(value." + col.data + ");");
                                sb.AppendLine("if (date != undefined && date != null)");
                                sb.AppendLine("{");
                                sb.AppendLine("value." + col.data + " = \"/Date(\" + date.getTime() + \")/\";");
                                sb.AppendLine("}");

                            }
                        }
                        sb.AppendLine("});");
                    }
                    else
                    {//Ex. var CustomsDataOnSave  = function (data) { };
                        sb.AppendLine(config.CustomsDataOnSave + "(data);");
                    }

                    sb.AppendLine("Grid" + name + ".rows.add(data).draw();");
                    sb.AppendLine("gridAdd" + name + ".rows({ selected: true }).remove().draw();");
                    sb.AppendLine("$.confirm({");
                    sb.AppendLine("title: null,");
                    sb.Append("content: \"");
                    sb.Append(Translation.CenterLang.Center.SaveCompleted);
                    sb.AppendLine("\",");
                    sb.AppendLine("columnClass: 'medium',");
                    sb.AppendLine("buttons: {");
                    sb.AppendLine("confirm: {");
                    sb.Append("text: '");
                    sb.Append(Translation.CenterLang.Center.OK);
                    sb.AppendLine("',");
                    sb.AppendLine("btnClass: \"btn-primary\",");
                    sb.AppendLine("action: function () {");
                    sb.AppendLine("$(\"#mdAdd" + name + "\").modal('hide');");
                    sb.AppendLine("}");
                    sb.AppendLine("}");
                    sb.AppendLine("}");
                    sb.AppendLine("});");
                    sb.AppendLine("}");
                    sb.AppendLine("else{");
                    sb.AppendLine("var content = '<div class=\"alert alert-danger alert-dismissable alert-dangernew\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\"><i class=\"ace-icon fa fa-times\"></i></button><h2><i class=\"ace-icon fa fa-times-circle bigger-130\"> " + Translation.CenterLang.Center.PleaseSelectData + "</h2></div>';");
                    sb.AppendLine("$(\"#notAdd" + name + "\").html(content).fadeTo(2000, 500);");
                    sb.AppendLine("}");
                    sb.AppendLine("}");
                    #endregion
                }
                #region OnClick_btnAdd
                sb.AppendLine("function OnClick_btnAdd" + name + "(e) {");
                sb.AppendLine("bindGridAdd" + name + "();");
                sb.Append("$(\"#mdTitle" + name + "\").text(\"");
                sb.Append(Translation.CenterLang.Center.Add + " " + label);
                sb.AppendLine("\");");
                sb.AppendLine("$(\"#mdAdd" + name + "\").modal();");
                sb.AppendLine("return false;");
                sb.AppendLine("}");
                #endregion

                #region OnClick_btnDelete
                sb.AppendLine("function OnClick_btnDelete" + name + "(e) {");
                sb.AppendLine("$.confirm({");
                sb.AppendLine("title: null,");
                sb.AppendLine("content: '" + Translation.CenterLang.Center.ConfirmDelete + "',");
                sb.AppendLine("columnClass: 'medium',");
                sb.AppendLine("buttons: {");
                sb.AppendLine("confirm: {");
                sb.Append("text: '");
                sb.Append(Translation.CenterLang.Center.OK);
                sb.AppendLine("',");
                sb.AppendLine("btnClass: \"btn-primary\",");
                sb.AppendLine("action: function () {");
                sb.AppendLine("var data = Grid" + name + ".rows({ selected: true }).data();");
                sb.AppendLine("if (data.length > 0) {");
                sb.AppendLine("Grid" + name + ".rows({ selected: true }).remove().draw();");
                sb.AppendLine("}");
                sb.AppendLine("else {");
                sb.AppendLine("var result = {");
                sb.AppendLine("Status: false,");
                sb.AppendLine("Style: '" + AlertStyles.Error + "',");
                sb.AppendLine("Message: '" + Translation.CenterLang.Center.PleaseSelectData + "'");
                sb.AppendLine("};");
                sb.AppendLine("OnSuccess(result);");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("},");
                sb.AppendLine("cancel: {");
                sb.Append("text: '");
                sb.Append(Translation.CenterLang.Center.Cancel);
                sb.AppendLine("',");
                sb.AppendLine("btnClass: \"btn-primary\"");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("});");
                sb.AppendLine("return false;");
                sb.AppendLine("}");
                #endregion

            }
            #region bindGrid
            sb.AppendLine("function bindGrid" + name + "() {");

            if (config.DefaultShowConfig != null)
            {
                sb.AppendLine("var result = {};");
                if (config.DefaultShowConfig.Parameters != null && config.DefaultShowConfig.Parameters.Count > 0)
                {
                    for (int i = 0; i < config.DefaultShowConfig.Parameters.Count; i++)
                    {
                        if (config.DefaultShowConfig.Parameters[i].Type == VSMParameterType.ByModelData)
                        {
                            sb.AppendLine("result[\"" + config.DefaultShowConfig.Parameters[i].Name + "\"] =\"" + config.DefaultShowConfig.Parameters[i].Value + "\";");
                        }
                        else
                        {
                            sb.AppendLine("result[\"" + config.DefaultShowConfig.Parameters[i].Name + "\"] = $(\"#" + config.DefaultShowConfig.Parameters[i].Value + "\").val();");
                        }
                    }
                }
            }

            sb.AppendLine("Grid" + name + " = $(\"#Grid" + name + "\").DataTable({");
            sb.AppendLine("\"deferRender\": true,");
            sb.AppendLine("\"paging\": true,");
            sb.AppendLine("\"processing\": true,");
            sb.AppendLine("\"destroy\": !$.isEmptyObject(Grid" + name + "),");
            //sb.AppendLine("\"scrollX\": true,");
            if (!config.Searching)
            {
                sb.AppendLine("\"searching\": false,");
            }
            else
            {
                sb.AppendLine("\"searching\": true,");
            }
            //sb.AppendLine("\"pagingType\": 'simple',");
            sb.AppendLine("\"lengthChange\": false,");
            //sb.AppendLine("\"dom\": \"<'row'<'col-sm-12 colGridInform'tr>><'row'<'col-sm-5 colGridInform'i><'col-sm-7 colGridInform'p>>\",");
            sb.Append("\"dom\": \"");
            sb.Append("<'table-responsive'tr><'row'<'col-sm-5'");
            sb.AppendLine("<'pull-left'l><'pull-left'i>><'col-sm-7'p>>\",");

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
            sb.Append("\"infoFiltered\": \"");
            sb.Append(Translation.CenterLang.Center.gridInfoFiltered);
            sb.AppendLine("\",");
            sb.Append("\"loadingRecords\": \"");
            sb.Append(Translation.CenterLang.Center.gridLoadingRecords);
            sb.AppendLine("\",");
            sb.Append("\"processing\": \"");
            sb.Append(Translation.CenterLang.Center.gridProcessing);
            sb.AppendLine("\",");
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
            if (config.DefaultShowConfig != null)
            {
                sb.AppendLine("\"ajax\": {");
                sb.Append("url: \"");
                sb.Append(config.DefaultShowConfig.Url);
                sb.Append("\",");
                sb.AppendLine("type: \"GET\",");
                sb.AppendLine("data: result");
                //sb.AppendLine("\"dataSrc\": \"rows\"");            
                sb.AppendLine("},");
            }

            sb.Append("\"order\": [");
            var columnsShow = config.ColumnsShow.Where(m => m.visible != "false").ToList();
            string orderShow = string.Empty;
            for (int i = 1; i < columnsShow.Count; i++)
            {
                if (config.ColumnsShow[i].orderable)
                {
                    orderShow += "[" + i + ", \"asc\"],";
                }
            }
            sb.Append(orderShow.TrimEnd(','));
            sb.AppendLine("],");
            if (visibleCheckBox && !IsReadOnly)
            {
                sb.AppendLine("select: {");
                sb.AppendLine("style: 'multi',");
                sb.AppendLine("selector: 'td:first-child'");
                sb.AppendLine("},");

                sb.AppendLine("\"createdRow\": function(row, data, index) {");
                sb.AppendLine("if (data.Is_Selected)");
                sb.AppendLine("{");
                sb.AppendLine("Grid" + name + ".row(row).select();");
                sb.AppendLine("}");
                sb.AppendLine("},");
            }
            sb.AppendLine("\"columns\": [");

            if (visibleCheckBox && !IsReadOnly)
            {
                sb.AppendLine("{");
                sb.AppendLine("data: null,");
                sb.AppendLine("defaultContent: '',");
                sb.AppendLine("width: '2%',");
                sb.AppendLine("className: 'dt-head-center select-checkbox',");
                sb.AppendLine("orderable: false,");
                sb.AppendLine("title : '<input type=\"checkbox\" name=\"selectAllGrid" + name + "\" id=\"selectAllGrid" + name + "\" class=\"checkbox\">'");
                sb.Append("}");
            }

            if (columnsShow != null)
            {
                if (visibleCheckBox && !IsReadOnly)
                {
                    sb.AppendLine(",");
                }
                int c = 0;
                foreach (var dtC in columnsShow)
                {
                    c++;
                    sb.AppendLine("{");
                    var allProperties = dtC.GetType().GetProperties().Where(m =>
                                                                        m.GetValue(dtC, null) != null &&
                                                                        m.Name != "IsKey" &&
                                                                        m.Name != "CaseSensitive" &&
                                                                        m.Name != "IsOrderColumn" &&
                                                                        m.Name != "IsButtonColumn" &&
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
                                else
                                {
                                    sb.Append("\"" + prop.Name + "\":\"" + value + "\"");
                                }
                            }
                        }
                        else if (prop.PropertyType == typeof(bool))
                        {
                            sb.Append("\"" + prop.Name + "\":" + Convert.ToString(value).ToLower());
                        }
                        else
                        {
                            sb.Append("\"" + prop.Name + "\":\"" + Convert.ToString(value).ToLower() + "\"");
                        }
                        if (i != allProperties.Count)
                        {
                            sb.AppendLine(",");
                        }
                    }
                    sb.Append("}");

                    if (c != columnsShow.Count)
                    {
                        sb.AppendLine(",");
                    }
                }
            }

            sb.Append("]");


            if (!IsReadOnly)
            {
                sb.AppendLine(",");
                sb.AppendLine("drawCallback: function () {");

                sb.AppendLine("$('[data-toggle=\"tooltip\"]').tooltip({");
                sb.AppendLine("trigger: 'hover'");
                sb.AppendLine("});");

                sb.AppendLine("if ($('#Grid" + name + " tbody .dataTables_empty').length) {");
                if (visibleCheckBox)
                {
                    sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", false);");
                }
                if (dataEmptyHideGrid)
                {
                    sb.AppendLine("$('#Grid" + name + "_wrapper').hide();");
                }
                sb.AppendLine("$('#btnDelete" + name + "').hide();");
                sb.AppendLine("}");
                sb.AppendLine("else {");
                if (dataEmptyHideGrid)
                {
                    sb.AppendLine("$('#Grid" + name + "_wrapper').show();");
                }
                sb.AppendLine("$('#btnDelete" + name + "').show();");
                sb.AppendLine("}");
                sb.AppendLine("}");
            }
            else
            {
                #region ToolTip
                sb.AppendLine(",");
                sb.AppendLine("drawCallback: function() {");
                sb.AppendLine("$('[data-toggle=\"tooltip\"]').tooltip({");
                sb.AppendLine("trigger: 'hover'");
                sb.AppendLine("});");
                sb.AppendLine("}");
                #endregion
            }

            sb.AppendLine("});");

            #region select all grid
            sb.AppendLine("$('#Grid" + name + "').on('init.dt', function() {");

            sb.AppendLine("var rows = Grid" + name + ".rows({ selected: true });");
            sb.AppendLine("rows.select();");
            sb.AppendLine("var totalSelected = rows.data().count();");
            sb.AppendLine("var total = Grid" + name + ".rows().data().count();");
            sb.AppendLine("if (totalSelected == total)");
            sb.AppendLine("{");
            sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", true);");
            sb.AppendLine("}");
            sb.AppendLine("else");
            sb.AppendLine("{");
            sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", false);");
            sb.AppendLine("}");
            sb.AppendLine("});");

            sb.AppendLine("$(\"#selectAllGrid" + name + "\").change(function() {");
            sb.AppendLine("if (this.checked) {");
            sb.AppendLine("Grid" + name + ".rows().select();");
            sb.AppendLine("}");
            sb.AppendLine("else {");
            sb.AppendLine("Grid" + name + ".rows().deselect();");
            sb.AppendLine("}");
            sb.AppendLine("});");

            sb.AppendLine("Grid" + name + ".on('select', function(e, dt, type, indexes) {");
            sb.AppendLine("var allRows = Grid" + name + ".rows().data().count();");
            sb.AppendLine("var selectedRows = Grid" + name + ".rows({ selected: true }).data().count();");
            sb.AppendLine("if (allRows == selectedRows)");
            sb.AppendLine("{");
            sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", true);");
            sb.AppendLine("}");
            sb.AppendLine("})");
            sb.AppendLine(".on('deselect', function(e, dt, type, indexes) {");
            sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", false);");
            sb.AppendLine("});");
            #endregion

            sb.AppendLine("}");
            #endregion
            if (!IsReadOnly)
            {
                #region bindGridAdd
                sb.AppendLine("function bindGridAdd" + name + "() {");
                sb.AppendLine("var result = {");
                sb.Append("keySource:\"");
                sb.Append(config.KeySource + "\"");
                if (config.Parameters != null && config.Parameters.Count > 0)
                {
                    sb.AppendLine(",");
                    sb.Append("extraParam:'{");
                    for (int i = 0; i < config.Parameters.Count; i++)
                    {
                        sb.Append(config.Parameters[i].Name + ":");
                        if (config.Parameters[i].Type == VSMParameterType.ByModelData)
                        {
                            sb.Append("\"" + config.Parameters[i].Value + "\"");
                        }
                        else
                        {
                            sb.Append("\"'+$(\"#" + config.Parameters[i].Value + "\").val()+'\"");
                        }
                        if (i != config.Parameters.Count - 1)
                        {
                            sb.Append(",");
                        }
                    }
                    sb.AppendLine("}'");
                }
                sb.AppendLine("}");

                sb.Append("var dataGridShow = ");
                sb.AppendLine("Grid" + name + ".rows().data();");
                sb.AppendLine("if(dataGridShow.length>0){");

                if (config.ColumnKey != null && config.ColumnKey.Count > 0)
                {
                    sb.AppendLine("var dataGridShowN = [];");
                    sb.AppendLine("$.each(dataGridShow.toArray(), function (i, item) {");
                    sb.AppendLine("var nItem = {");
                    var k = 0;
                    foreach (var item in config.ColumnKey)
                    {
                        if (item.data.Contains(":"))
                        {
                            var cols = item.data.Split(':');
                            sb.Append(cols[0]);
                            sb.Append(" : $.gridDateToISOString(item.");
                            sb.Append(cols[0]);
                            sb.Append(")");
                        }
                        else
                        {
                            sb.Append(item.data);
                            sb.Append(" : item.");
                            sb.Append(item.data);
                        }
                        if (k != config.ColumnKey.Count - 1)
                        {
                            sb.AppendLine(",");
                        }
                        k++;
                    }

                    sb.AppendLine("}");
                    sb.AppendLine("dataGridShowN.push(nItem);");
                    sb.AppendLine("});");
                    sb.AppendLine("if (dataGridShowN.length > 0) {");
                    sb.AppendLine("result[\"extraGridShow\"] = JSON.stringify(dataGridShowN);");
                    sb.AppendLine("}");
                }
                else
                {
                    sb.AppendLine("result[\"extraGridShow\"] = JSON.stringify(dataGridShow.toArray());");
                }

                sb.AppendLine("}");

                sb.AppendLine("gridAdd" + name + " = $('#gridAdd" + name + "').DataTable({");
                sb.AppendLine("\"deferRender\": true,");
                sb.AppendLine("\"paging\": true,");
                sb.AppendLine("\"processing\": true,");
                sb.AppendLine("\"serverSide\": true,");
                sb.AppendLine("\"destroy\": !$.isEmptyObject(gridAdd" + name + "),");
                sb.Append("\"order\": [");
                var columns = config.Columns.Where(m => m.visible != "false").ToList();
                string order = string.Empty;
                for (int i = 1; i < columns.Count; i++)
                {
                    if (config.Columns[i].orderable)
                    {
                        order += "[" + i + ", \"asc\"],";
                    }
                }
                sb.Append(order.TrimEnd(','));
                sb.AppendLine("],");
                sb.AppendLine("\"ajax\": {");
                var urlAdd = new UrlHelper(html.ViewContext.RequestContext);
                sb.AppendLine("url: \"" + urlAdd.Action("GetDataMultiSelect", "Autocomplete", new
                {
                    Area = "Ux"
                }) + "\",");
                sb.AppendLine("type: \"POST\",");
                sb.AppendLine("error: OnAjaxError,");
                sb.AppendLine("data: result");
                //sb.AppendLine("\"dataSrc\": \"rows\"");            
                sb.AppendLine("},");
                //sb.AppendLine("\"scrollX\": true,");
                sb.AppendLine("\"dom\": \"<'row'<'col-sm-6'><'col-sm-6'<'pull-right'l>>><'row'<'col-sm-12'tr>><'row'<'col-sm-5'i><'col-sm-7'p>>\",");
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
                sb.Append("\"infoFiltered\": \"");
                sb.Append(Translation.CenterLang.Center.gridInfoFiltered);
                sb.AppendLine("\",");
                sb.Append("\"loadingRecords\": \"");
                sb.Append(Translation.CenterLang.Center.gridLoadingRecords);
                sb.AppendLine("\",");
                sb.Append("\"processing\": \"");
                sb.Append(Translation.CenterLang.Center.gridProcessing);
                sb.AppendLine("\",");
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
                sb.AppendLine("select: {");
                sb.AppendLine("style: '" + (isSingleSelect ? "single" : "multi") + "',");
                sb.AppendLine("selector: 'td:first-child'");
                sb.AppendLine("},");

                sb.AppendLine("\"createdRow\": function(row, data, index) {");
                sb.AppendLine("if (data.Is_Selected)");
                sb.AppendLine("{");
                sb.AppendLine("gridAdd" + name + ".row(row).select();");
                sb.AppendLine("}");
                sb.AppendLine("},");

                sb.AppendLine("\"columns\": [");
                if (visibleCheckBox)
                {
                    sb.AppendLine("{");
                    sb.AppendLine("data: null,");
                    sb.AppendLine("defaultContent: '',");
                    sb.AppendLine("width: '2%',");
                    sb.AppendLine("className: 'dt-head-center select-checkbox',");
                    sb.AppendLine("orderable: false,");
                    sb.AppendLine("title : '<input type=\"checkbox\" name=\"gridAdd" + name + "SelectAll\" id=\"gridAdd" + name + "SelectAll\" class=\"checkbox\">'");
                    sb.Append("}");
                }

                if (columns != null)
                {
                    sb.AppendLine(",");
                    int c = 0;
                    foreach (var dtC in columns)
                    {
                        c++;
                        sb.AppendLine("{");
                        var allProperties = dtC.GetType().GetProperties().Where(m =>
                                                                        m.GetValue(dtC, null) != null &&
                                                                        m.Name != "IsKey" &&
                                                                        m.Name != "CaseSensitive" &&
                                                                        m.Name != "IsOrderColumn" &&
                                                                        m.Name != "IsButtonColumn" &&
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
                                    else
                                    {
                                        sb.Append("\"" + prop.Name + "\":\"" + value + "\"");
                                    }
                                }
                            }
                            else if (prop.PropertyType == typeof(bool))
                            {
                                sb.Append("\"" + prop.Name + "\":" + Convert.ToString(value).ToLower());
                            }
                            else
                            {
                                sb.Append("\"" + prop.Name + "\":\"" + Convert.ToString(value).ToLower() + "\"");
                            }
                            if (i != allProperties.Count)
                            {
                                sb.AppendLine(",");
                            }
                        }
                        sb.Append("}");

                        if (c != columns.Count)
                        {
                            sb.AppendLine(",");
                        }
                    }
                }

                sb.AppendLine("]");
                sb.AppendLine("});");

                if (visibleCheckBox)
                {
                    #region event check all
                    sb.AppendLine("$('#gridAdd" + name + "').on('init.dt', function() {");

                    sb.AppendLine("var rows = gridAdd" + name + ".rows({ selected: true });");
                    sb.AppendLine("rows.select();");
                    sb.AppendLine("var totalSelected = rows.data().count();");
                    sb.AppendLine("var total = gridAdd" + name + ".rows().data().count();");
                    sb.AppendLine("if (totalSelected == total)");
                    sb.AppendLine("{");
                    sb.AppendLine("$(\"#gridAdd" + name + "SelectAll\").prop(\"checked\", true);");
                    sb.AppendLine("}");
                    sb.AppendLine("else");
                    sb.AppendLine("{");
                    sb.AppendLine("$(\"#gridAdd" + name + "SelectAll\").prop(\"checked\", false);");
                    sb.AppendLine("}");
                    sb.AppendLine("});");

                    sb.AppendLine("$(\"#gridAdd" + name + "SelectAll\").change(function() {");
                    sb.AppendLine("if (this.checked) {");
                    sb.AppendLine("gridAdd" + name + ".rows().select();");
                    sb.AppendLine("}");
                    sb.AppendLine("else {");
                    sb.AppendLine("gridAdd" + name + ".rows().deselect();");
                    sb.AppendLine("}");
                    sb.AppendLine("});");

                    sb.AppendLine("gridAdd" + name + ".on('select', function(e, dt, type, indexes) {");
                    sb.AppendLine("var allRows = gridAdd" + name + ".rows().data().count();");
                    sb.AppendLine("var selectedRows = gridAdd" + name + ".rows({ selected: true }).data().count();");
                    sb.AppendLine("if (allRows == selectedRows)");
                    sb.AppendLine("{");
                    sb.AppendLine("$(\"#gridAdd" + name + "SelectAll\").prop(\"checked\", true);");
                    sb.AppendLine("}");
                    sb.AppendLine("})");
                    sb.AppendLine(".on('deselect', function(e, dt, type, indexes) {");
                    sb.AppendLine("$(\"#gridAdd" + name + "SelectAll\").prop(\"checked\", false);");
                    sb.AppendLine("});");
                    #endregion
                }

                if (isSingleSelect)
                {
                    sb.AppendLine("gridAdd" + name + ".on('select', function (e, dt, type, indexes) {");
                    sb.AppendLine("var data = gridAdd" + name + ".rows(indexes).data();");
                    sb.AppendLine("if (data != null) {");
                    sb.AppendLine("Grid" + name + ".rows().remove().draw();");
                    sb.AppendLine("Grid" + name + ".rows.add(data).draw();");
                    sb.AppendLine("$(\"#mdAdd" + name + "\").modal('hide');");
                    sb.AppendLine("}");
                    sb.AppendLine("});");
                }

                sb.AppendLine("}");
                #endregion
            }
            #region ready
            sb.AppendLine("$(document).ready(function () {");
            sb.AppendLine("bindGrid" + name + "();");
            if (!IsReadOnly)
            {
                sb.AppendLine("$(\"#btnAdd" + name + "\").click(OnClick_btnAdd" + name + ");");
                if (!isSingleSelect)
                {
                    //sb.AppendLine("$(\"#btnSave" + name + "\").click(OnClick_btnSave" + name + ");");
                    sb.AppendLine("$(\"#btnSave" + name + "\").vsmConfirm({");
                    sb.AppendLine("title: null,");
                    sb.AppendLine("closeIcon: true,");
                    sb.AppendLine("closeIconClass: 'fa fa-close',");
                    sb.AppendLine("columnClass: 'medium',");
                    sb.AppendLine("buttons: {");
                    sb.AppendLine("confirm: {");
                    sb.Append("text: '");
                    sb.Append(Translation.CenterLang.Center.OK);
                    sb.AppendLine("',");
                    sb.AppendLine("btnClass: \"btn-primary\",");
                    sb.AppendLine("action: OnClick_btnSave" + name + "");
                    sb.AppendLine("},");
                    sb.AppendLine("cancel: {");
                    sb.Append("text: '");
                    sb.Append(Translation.CenterLang.Center.Cancel);
                    sb.AppendLine("',");
                    sb.AppendLine("btnClass: \"btn-primary\"");
                    sb.AppendLine("}");
                    sb.AppendLine("}");
                    sb.AppendLine("});");
                }

                sb.AppendLine("$(\"#btnDelete" + name + "\").click(OnClick_btnDelete" + name + ");");
            }

            #region oncolumn search
            sb.Append("$('");
            for (int i = 0; i < config.Columns.Count; i++)
            {
                sb.Append("#ftCol" + i.ToString());
                if (i != config.Columns.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.AppendLine("').on('keyup change', function(e) {");
            sb.AppendLine("var ctrl = $(this);");
            sb.AppendLine("var idx = ctrl.parent().index();");
            sb.AppendLine("gridAdd" + name);
            sb.AppendLine(".column(idx)");
            sb.AppendLine(".search(this.value, false, true)");
            sb.AppendLine(".draw();");
            sb.AppendLine("});");
            #endregion

            sb.AppendLine("});");
            #endregion

            sb.AppendLine("</script>");
            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// Get Config Multi Select
        /// </summary>
        /// <param name="keySource">key in DB SEC_Autocomplte</param>
        /// <param name="showConfig">set config of grid show</param>
        /// <param name="columnShow">Add Column Show of Selected</param>
        /// <returns>DataAccess.VSMMultiSelectConfig</returns>
        public static VSMMultiSelectConfig GetMultiSelectConfig(string keySource, DefaultConfig showConfig, string columnShow, params string[] columnShows)
        {
            return GetMultiSelectConfig(keySource, showConfig, "", null, columnShow, columnShows);
        }

        /// <summary>
        /// Get Config Multi Select
        /// </summary>
        /// <param name="keySource">key in DB SEC_Autocomplte</param>
        /// <param name="parameters">Parameter To DataBase Config</param>
        /// <param name="columnShow">Add Column Show of Selected</param>
        /// <returns>DataAccess.VSMMultiSelectConfig</returns>
        public static VSMMultiSelectConfig GetMultiSelectConfig(string keySource, List<VSMParameter> parameters, string columnShow, params string[] columnShows)
        {
            return GetMultiSelectConfig(keySource, null, "", parameters, columnShow, columnShows);
        }

        /// <summary>
        /// Get Config Multi Select
        /// </summary>
        /// <param name="keySource">key in DB SEC_Autocomplte</param>
        /// <param name="showConfig">set config of grid show</param>
        /// <param name="parameters">Parameter To DataBase Config</param>
        /// <param name="columnShow">Add Column Show of Selected</param>
        /// <returns>DataAccess.VSMMultiSelectConfig</returns>
        public static VSMMultiSelectConfig GetMultiSelectConfig(string keySource, DefaultConfig showConfig, string customsDataOnSave, List<VSMParameter> parameters, string columnShow, params string[] columnShows)
        {
            var mus = new VSMMultiSelectConfig();
            mus.KeySource = keySource;
            mus.Parameters = parameters;
            mus.DefaultShowConfig = showConfig;
            mus.CustomsDataOnSave = customsDataOnSave;

            var da = new AutocompleteDA();
            da.DTO.Execute.ExecuteType = AutocompleteExecuteType.GetStructure;
            da.DTO.Parameter.KeySource = keySource;
            da.SelectNoEF(da.DTO);
            if (da.DTO.colModel != null)
            {
                var newCol = da.DTO.colModel.Select(m => new GridColumn() { data = m.ColumnName, title = m.HeaderName }).ToList();
                mus.Columns = newCol;
                mus.ColumnsShow = newCol.Where(m => m.data == columnShow).ToList();
                if (columnShows != null)
                {
                    foreach (var col in columnShows)
                    {
                        var nCol = newCol.FirstOrDefault(m => m.data == col);
                        if (nCol != null)
                        {
                            mus.ColumnsShow.Add(nCol);
                        }
                    }
                }
            }
            if (da.DTO.colKeyModel != null)
            {
                mus.ColumnKey = da.DTO.colKeyModel.Select(m => new GridColumn() { data = m.ColumnName }).ToList();
            }
            return mus;
        }

        /// <summary>
        /// Get Config Multi Select
        /// </summary>
        /// <param name="keySource">key in DB SEC_Autocomplte</param>
        /// <param name="columnShow">Add Column Show of Selected</param>
        /// <returns>VSMMultiSelectConfig</returns>
        public static VSMMultiSelectConfig GetMultiSelectConfig(string keySource, GridColumn columnShow, params GridColumn[] columnShows)
        {
            return GetMultiSelectConfig(keySource, null, "", false, null, columnShow, columnShows);
        }

        /// <summary>
        /// Get Config Multi Select
        /// </summary>
        /// <param name="keySource">key in DB SEC_Autocomplte</param>
        /// <param name="showConfig">set config of grid show</param>
        /// <param name="columnShow">Add Column Show of Selected</param>
        /// <returns>DataAccess.VSMMultiSelectConfig</returns>
        public static VSMMultiSelectConfig GetMultiSelectConfig(string keySource, DefaultConfig showConfig, GridColumn columnShow, params GridColumn[] columnShows)
        {
            return GetMultiSelectConfig(keySource, showConfig, "", false, null, columnShow, columnShows);
        }

        /// <summary>
        /// Get Config Multi Select
        /// </summary>
        /// <param name="keySource">key in DB SEC_Autocomplte</param>
        /// <param name="parameters">Parameter To DataBase Config</param>
        /// <param name="columnShow">Add Column Show of Selected</param>
        /// <returns>DataAccess.VSMMultiSelectConfig</returns>
        public static VSMMultiSelectConfig GetMultiSelectConfig(string keySource, List<VSMParameter> parameters, GridColumn columnShow, params GridColumn[] columnShows)
        {
            return GetMultiSelectConfig(keySource, null, "", false, parameters, columnShow, columnShows);
        }

        /// <summary>
        /// Get Config Multi Select
        /// </summary>
        /// <param name="keySource">key in DB SEC_Autocomplte</param>
        /// <param name="showConfig">set config of grid show</param>
        /// <param name="parameters">Parameter To DataBase Config</param>
        /// <param name="columnShow">Add Column Show of Selected</param>
        /// <returns>DataAccess.VSMMultiSelectConfig</returns>
        public static VSMMultiSelectConfig GetMultiSelectConfig(string keySource, DefaultConfig showConfig, string customsDataOnSave, bool searching, List<VSMParameter> parameters, GridColumn columnShow, params GridColumn[] columnShows)
        {
            var mus = new VSMMultiSelectConfig();
            mus.KeySource = keySource;
            mus.Parameters = parameters;
            mus.DefaultShowConfig = showConfig;
            mus.CustomsDataOnSave = customsDataOnSave;
            mus.Searching = searching;

            var da = new AutocompleteDA();
            da.DTO.Execute.ExecuteType = AutocompleteExecuteType.GetStructure;
            da.DTO.Parameter.KeySource = keySource;
            da.SelectNoEF(da.DTO);
            if (da.DTO.colModel != null)
            {
                var newCol = da.DTO.colModel.Select(m => new GridColumn() { data = m.ColumnName, title = m.HeaderName, type = StrToColumnsType(m.COLUMNFORMAT), className = StrToColumnsTextAlign(m.COLUMNFORMAT) }).ToList();
                mus.Columns = newCol;
                mus.ColumnsShow = new List<GridColumn>();
                mus.ColumnsShow.Add(columnShow);
                if (columnShows != null)
                {
                    mus.ColumnsShow.AddRange(columnShows);
                }
            }

            if (da.DTO.colKeyModel != null)
            {
                mus.ColumnKey = da.DTO.colKeyModel.Select(m => new GridColumn() { data = m.ColumnName }).ToList();
            }
            return mus;
        }

        /// <summary>
        /// Get Config Multi Select
        /// </summary>
        /// <param name="keySource">key in DB SEC_Autocomplte</param>
        /// <param name="columnShow">Add Column Show of Selected</param>
        /// <returns>VSMMultiSelectConfig</returns>
        public static VSMMultiSelectConfig GetMultiSelectConfig(string keySource, string columnShow, params string[] columnShows)
        {
            return GetMultiSelectConfig(keySource, null, "", null, columnShow, columnShows);
        }

        public static VSMMultiSelectConfig GetMultiSelectConfig(string keySource, DefaultConfig showConfig, string customsDataOnSave, GridColumn columnShow, params GridColumn[] columnShows)
        {
            return GetMultiSelectConfig(keySource, showConfig, customsDataOnSave, false, null, columnShow, columnShows);
        }

        public static VSMMultiSelectConfig GetMultiSelectConfig(string keySource, DefaultConfig showConfig, string customsDataOnSave, bool searching, GridColumn columnShow, params GridColumn[] columnShows)
        {
            return GetMultiSelectConfig(keySource, showConfig, customsDataOnSave, searching, null, columnShow, columnShows);
        }
        #endregion

        #region Detail
        public static MvcHtmlString GetVSMDetailFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool IsReadOnly = false, bool isRequired = false, WidgetColor color = WidgetColor.None, bool searching = false)
        {
            return GetVSMDetailFor(html, expression, IsReadOnly, color, isRequired, searching);
        }
        public static MvcHtmlString GetVSMDetailFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool IsReadOnly = false, bool isRequired = false, WidgetColor color = WidgetColor.None, bool visibleDelete = true, bool visibleAdd = true, bool searching = false, params ButtonConfig[] otherLinks)
        {
            return GetVSMDetailFor(html, expression, IsReadOnly, color, isRequired, visibleDelete, visibleAdd, searching, otherLinks);
        }
        public static MvcHtmlString GetVSMDetailFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool IsReadOnly = false, bool isRequired = false, WidgetColor color = WidgetColor.None, bool searching = false, params ButtonConfig[] otherLinks)
        {
            return GetVSMDetailFor(html, expression, IsReadOnly, color, isRequired, true, true, searching, otherLinks);
        }
        public static MvcHtmlString GetVSMDetailFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool IsReadOnly = false, WidgetColor color = WidgetColor.None, bool isRequired = false, bool searching = false, params ButtonConfig[] otherLinks)
        {
            return GetVSMDetailFor(html, expression, IsReadOnly, color, isRequired, true, true, searching, otherLinks);
        }
        public static MvcHtmlString GetVSMDetailFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool IsReadOnly = false, WidgetColor color = WidgetColor.None, bool isRequired = false, bool visibleDelete = true, bool visibleAdd = true, bool searching = false, params ButtonConfig[] otherLinks)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string label = GetControlLabel(metadata, expressionText);
            string name = GetControlName(html, expressionText);

            return GetVSMDetail(html, name, label, IsReadOnly, color, isRequired, visibleDelete, visibleAdd, searching, otherLinks);
        }

        public static MvcHtmlString GetVSMDetail<TModel>(this HtmlHelper<TModel> html,
            string name, string title, bool IsReadOnly = false, WidgetColor color = WidgetColor.None, bool isRequired = false, bool visibleDelete = true, bool visibleAdd = true, bool searching = false, params ButtonConfig[] otherLinks)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div id=\"wg" + name + "\" class=\"row\">");
            sb.AppendLine("<div class=\"col-md-12 widget-container-col\">");
            sb.Append("<div class=\"widget-box");
            if (color != WidgetColor.None)
            {
                sb.Append(" " + color);
            }
            if (isRequired)
            {
                sb.Append(" required");
            }
            sb.AppendLine("\">");
            sb.AppendLine("<div class=\"widget-header widget-header-small\">");
            sb.AppendLine("<h6 class=\"widget-title\">");
            sb.AppendLine(title);
            sb.AppendLine("</h6>");
            sb.AppendLine("<div class=\"widget-toolbar\">");
            sb.AppendLine("<a href=\"javascript:void(0)\" data-action=\"collapse\">");
            sb.AppendLine("<i class=\"ace-icon fa fa-chevron-up\"></i>");
            sb.AppendLine("</a>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div class=\"widget-body\">");
            sb.AppendLine("<div class=\"widget-main\">");
            sb.AppendLine("<div id=\"notWidget" + name + "\" style=\"display: none;\"></div>");
            sb.AppendLine("<div class=\"row\">");
            sb.AppendLine("<div class=\"col-md-12\">");
            sb.AppendLine("<div class=\"gridIncludeToolbar\">");
            var links = new List<ButtonConfig>();
            if (visibleAdd && !IsReadOnly)
            {
                links.Add(new ButtonConfig("btnAdd" + name)
                {
                    IconCssClass = FaIcons.FaPlusCircle + " " + FaIcons.FaLg,
                    IconColor = TextColor.green,
                    Title = Translation.CenterLang.Center.Add,
                    Tooltip = true
                });
            }
            if (visibleDelete && !IsReadOnly)
            {
                links.Add(new ButtonConfig("btnDelete" + name)
                {
                    IconCssClass = FaIcons.FaTrashO + " " + FaIcons.FaLg,
                    IconColor = TextColor.red,
                    Title = Translation.CenterLang.Center.Delete,
                    Tooltip = true
                }.AddAttributes(new
                {
                    style = "display: none;"
                }));
            }
            if (otherLinks != null)
            {
                foreach (var item in otherLinks)
                {
                    if (item.Index != -1)
                    {
                        links.Insert(item.Index, item);
                    }
                    else
                    {
                        links.Add(item);
                    }

                }
            }
            if (links != null && links.Count > 0)
            {
                sb.AppendLine("<div class=\"row gridToolbar\">");
                var width = 12;
                if (searching)
                {
                    width = 8;
                }
                sb.AppendLine("<div class=\"col-md-" + width + " no-padding\">");
                foreach (var link in links)
                {
                    sb.Append("<a ");
                    if (!link.Name.IsNullOrEmpty())
                    {
                        sb.Append("id =\"");
                        sb.Append(link.Name);
                        sb.Append("\" value=\"");
                        sb.Append(link.Name);
                        sb.Append("\"");
                    }
                    sb.Append(" class=\"btn btn-xs btn-white btn-round");
                    if (!link.CssClass.IsNullOrEmpty())
                    {
                        sb.Append(" " + link.CssClass);
                    }
                    sb.Append("\"");

                    if (!link.Url.IsNullOrEmpty())
                    {
                        sb.Append(" href=\"");
                        sb.Append(link.Url);
                        sb.Append("\"");
                    }
                    else
                    {
                        sb.Append(" href=\"javascript:void(0)\"");
                    }
                    if (link.HtmlAttribute != null)
                    {
                        foreach (var item in link.HtmlAttribute)
                        {
                            sb.Append(" ");
                            sb.Append(item.Key);
                            sb.Append("=\"");
                            sb.Append(item.Value);
                            sb.Append("\"");
                        }
                    }
                    if (link.IsNewWindow)
                    {
                        sb.Append(" target=\"_blank\"");
                    }
                    if (link.Tooltip)
                    {
                        sb.Append(" data-toggle=\"tooltip\"");
                    }
                    if (!link.Title.IsNullOrEmpty())
                    {
                        sb.Append(" title=\"" + link.Title + "\"");
                    }
                    sb.AppendLine(">");
                    if (!link.IconCssClass.IsNullOrEmpty() && link.IconPosition == StandardIconPosition.BeforeText)
                    {
                        sb.Append("<i class=\"ace-icon fa ");
                        sb.Append(link.IconCssClass);
                        if (link.IconColor != TextColor.None)
                        {
                            sb.Append(" " + link.IconColor.GetDescription());
                        }
                        sb.Append(" bigger-125 icon-only\"></i>");
                    }
                    if (!link.Text.IsNullOrEmpty())
                    {
                        sb.AppendLine(link.Text);
                    }
                    if (!link.IconCssClass.IsNullOrEmpty() && link.IconPosition == StandardIconPosition.AfterText)
                    {
                        sb.Append("<i class=\"ace-icon fa ");
                        sb.Append(link.IconCssClass);
                        if (link.IconColor != TextColor.None)
                        {
                            sb.Append(" " + link.IconColor.GetDescription());
                        }
                        sb.Append(" bigger-125 icon-only\"></i>");
                    }
                    sb.AppendLine("</a>");
                }
                sb.AppendLine("</div>");
                if (searching)
                {
                    sb.AppendLine("<div class=\"col-md-4 no-padding\">");
                    sb.AppendLine("<div class=\"form-group no-margin\">");
                    sb.AppendLine("<label for=\"" + name + "Searching\" class=\"control-label col-md-4 no-padding-right\">");
                    sb.AppendLine(Translation.CenterLang.Center.Search);
                    sb.AppendLine("</label>");
                    sb.AppendLine("<div class=\"col-md-8 no-padding-right\">");
                    sb.AppendLine("<input id=\"Searching" + name + "\" type=\"text\" class=\"form-control input-sm\">");
                    sb.AppendLine("</div>");
                    sb.AppendLine("</div>");
                    sb.AppendLine("</div>");
                }
                sb.AppendLine("</div>");
            }
            sb.AppendLine("<table id=\"Grid" + name + "\" class=\"table table-striped table-bordered table-hover\" data-col=\"\" data-coldate=\"\" data-tabletype=\"detail\"");
            sb.Append(" data-isrequired=\"" + isRequired.ToString().ToLower() + "\"");
            sb.AppendLine(">");
            sb.AppendLine("</table>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString GetVSMModalDetailBeginFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, ModalSize modalSize = ModalSize.Xl)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string label = GetControlLabel(metadata, expressionText);
            string name = GetControlName(html, expressionText);
            return GetVSMModalBegin(html, name, label, modalSize, true, true, new ButtonConfig(StandardButtonName.Save)
            {
                Text = Translation.CenterLang.Center.Save,
                IconCssClass = FaIcons.FaSave
            });
        }
        public static MvcHtmlString GetVSMModalDetailEnd<TModel>(this HtmlHelper<TModel> html)
        {
            return GetVSMModalEnd(html);
        }
        public static MvcHtmlString GetVSMDetailScriptFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, params GridColumn[] columns)
        {
            return GetVSMDetailScriptFor(html, expression, null, columns);
        }
        public static MvcHtmlString GetVSMDetailScriptFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, VSMDetailConfig config, params GridColumn[] columns)
        {
            if (config == null)
            {
                config = new VSMDetailConfig();
            }
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string label = GetControlLabel(metadata, expressionText);
            string name = GetControlName(html, expressionText);
            var sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("var Grid" + name + " = {};");
            var existsKey = columns.Where(m => m.IsKey);

            if ((config.VisibleAdd || config.VisibleEditColumn) && !config.IsReadOnly)
            {
                #region resetForm 
                sb.AppendLine("function resetForm" + name + "() {");
                sb.AppendLine("var form" + name + " = $(\"#form" + name + "\");");
                sb.AppendLine("form" + name + ".validate().resetForm();");
                sb.AppendLine("form" + name + ".clearForm();");
                sb.AppendLine("$(\"#notModal" + name + "\").empty().hide();");
                sb.AppendLine("$.SlidingTime();");
                sb.AppendLine("}");
                #endregion

                #region OnClick_btnSave
                if (!config.CustomSave)
                {
                    //ติดไว้ OnBeforeSave และ OnSave
                    sb.AppendLine("function OnClick_btnSave" + name + "() {");
                    sb.AppendLine("var btn = this.$target;");
                    sb.AppendLine("var form" + name + " = $(\"#form" + name + "\");");
                    sb.AppendLine("if(form" + name + ".valid()){");
                    sb.AppendLine("var mode = btn.data(\"mode\");");
                    sb.AppendLine("var index;");
                    sb.AppendLine("if (mode == \"edit\") {");
                    sb.AppendLine("index = btn.data(\"index\");");
                    sb.AppendLine("}");
                    sb.AppendLine("var data = form" + name + ".serializeObjectGrid();");
                    if (existsKey != null && existsKey.Any())
                    {
                        var keyCase = string.Empty;
                        var i = 1;
                        var cK = existsKey.Count();
                        foreach (var item in existsKey)
                        {
                            if (item.type == ColumnsType.Date || item.type == ColumnsType.DateTime)
                            {
                                keyCase += "jsonDateToFormat(item." + item.data + ") == jsonDateToFormat(data." + item.data + ")";
                            }
                            else
                            {
                                if (item.CaseSensitive)
                                {
                                    keyCase += "item." + item.data + ".toString().trim() == data." + item.data + ".toString().trim()";
                                }
                                else
                                {
                                    keyCase += "item." + item.data + ".toString().trim().toUpperCase() == data." + item.data + ".toString().trim().toUpperCase()";
                                }
                            }
                            if (i != cK)
                            {
                                keyCase += "&&";
                            }
                            i++;
                        }

                        sb.AppendLine("var isDup = false;");
                        sb.Append("var gData =");
                        sb.AppendLine("Grid" + name + ".rows({order:'index'}).data();");
                        sb.AppendLine("if(gData.length > 0){");
                        sb.AppendLine("$.each(gData.toArray(), function (i, item) {");
                        sb.AppendLine("if (mode == \"edit\") {");
                        sb.AppendLine("if(i!=index) {");
                        sb.Append("if(");
                        sb.Append(keyCase);
                        sb.Append("){");
                        sb.AppendLine("isDup=true;");
                        sb.AppendLine("return false;");
                        sb.AppendLine("}");
                        sb.AppendLine("}");
                        sb.AppendLine("}");
                        sb.AppendLine("else{");
                        sb.Append("if(");
                        sb.Append(keyCase);
                        sb.Append("){");
                        sb.AppendLine("isDup=true;");
                        sb.AppendLine("return false;");
                        sb.AppendLine("}");
                        sb.AppendLine("}");
                        sb.AppendLine("});");
                        sb.AppendLine("}");

                        var colData = columns.Select(m => m.data).Distinct().ToList();
                        var emptyRow = Newtonsoft.Json.JsonConvert.SerializeObject(colData);
                        emptyRow = emptyRow.Replace("[", "{").Replace("]", "}").Replace("\",\"", "\":null,\"").Replace("\"}", "\":null}");
                        sb.AppendLine("if(!isDup){");
                        if (config.VisibleEditColumn)
                        {
                            if (!config.OnSave.IsNullOrEmpty())
                            {
                                sb.Append("var defaultRow =");
                                sb.Append(emptyRow);
                                sb.AppendLine(";");
                                sb.AppendLine(config.OnSave + "(btn,data,defaultRow);");
                            }
                            else
                            {
                                sb.AppendLine("if (mode == \"edit\") {");
                                sb.AppendLine("var rData = Grid" + name + ".row(index).data();");
                                sb.AppendLine("$.each(data, function(key, value) {");
                                sb.AppendLine("rData[key] = value;");
                                sb.AppendLine("});");
                                sb.AppendLine("Grid" + name + ".row(index).data(rData).draw();");
                                sb.AppendLine("}");
                                sb.AppendLine("else {");
                                sb.Append("var defaultRow =");
                                sb.Append(emptyRow);
                                sb.AppendLine(";");
                                sb.AppendLine("$.each(data, function(key, value) {");
                                sb.AppendLine("defaultRow[key] = value;");
                                sb.AppendLine("});");
                                sb.AppendLine("Grid" + name + ".row.add(defaultRow).draw();");
                                sb.AppendLine("}");
                            }
                        }
                        else
                        {
                            if (!config.OnSave.IsNullOrEmpty())
                            {
                                sb.AppendLine(config.OnSave + "(btn,data,defaultRow);");
                            }
                            sb.Append("var defaultRow =");
                            sb.Append(emptyRow);
                            sb.AppendLine(";");
                            sb.AppendLine("$.each(data, function(key, value) {");
                            sb.AppendLine("defaultRow[key] = value;");
                            sb.AppendLine("});");
                            sb.AppendLine("Grid" + name + ".row.add(defaultRow).draw();");
                        }

                        if (!config.OnSave.IsNullOrEmpty() && config.OnAfterSave.IsNullOrEmpty())
                        {
                        }
                        else if (!config.OnAfterSave.IsNullOrEmpty())
                        {
                            sb.AppendLine("if (mode == \"edit\") {");
                            sb.AppendLine("var rData = Grid" + name + ".row(index).data();");
                            sb.AppendLine("$.each(data, function(key, value) {");
                            sb.AppendLine("rData[key] = value;");
                            sb.AppendLine("});");
                            sb.AppendLine(config.OnAfterSave + "(rData);");
                            sb.AppendLine("} else {");
                            sb.Append("var defaultRow =");
                            sb.Append(emptyRow);
                            sb.AppendLine(";");
                            sb.AppendLine("$.each(data, function(key, value) {");
                            sb.AppendLine("defaultRow[key] = value;");
                            sb.AppendLine("});");
                            sb.AppendLine(config.OnAfterSave + "(defaultRow);");
                            sb.AppendLine("}");
                        }
                        else
                        {
                            sb.AppendLine("$.confirm({");
                            sb.AppendLine("title: null,");
                            sb.Append("content: \"");
                            sb.Append(Translation.CenterLang.Center.SaveCompleted);
                            sb.AppendLine("\",");
                            sb.AppendLine("columnClass: 'medium',");
                            sb.AppendLine("buttons: {");
                            sb.AppendLine("confirm: {");
                            sb.Append("text: '");
                            sb.Append(Translation.CenterLang.Center.OK);
                            sb.AppendLine("',");
                            sb.AppendLine("btnClass: \"btn-primary\",");
                            sb.AppendLine("action: function () {");
                            sb.AppendLine("$(\"#md" + name + "\").modal('hide');");
                            sb.AppendLine("}");
                            sb.AppendLine("}");
                            sb.AppendLine("}");
                            sb.AppendLine("});");
                        }

                        sb.AppendLine("}");
                        sb.AppendLine("else{");
                        sb.AppendLine("var content = '<div class=\"alert alert-danger alert-dismissable alert-dangernew\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\"><i class=\"ace-icon fa fa-times\"></i></button><h2><i class=\"ace-icon fa fa-times-circle bigger-130\"> " + Translation.CenterLang.Center.DuplcateData + "</h2></div>';");
                        sb.AppendLine("$(\"#notModal" + name + "\").html(content).fadeTo(2000, 500);");
                        sb.AppendLine("}");
                        sb.AppendLine("}");
                    }
                    else
                    {
                        if (config.VisibleEditColumn)
                        {
                            if (!config.OnSave.IsNullOrEmpty())
                            {
                                sb.AppendLine(config.OnSave + "(btn,data);");
                            }
                            else
                            {
                                sb.AppendLine("if (mode == \"edit\") {");
                                sb.AppendLine("var rData = Grid" + name + ".row(index).data();");
                                sb.AppendLine("$.each(data, function(key, value) {");
                                sb.AppendLine("rData[key] = value;");
                                sb.AppendLine("});");
                                sb.AppendLine("Grid" + name + ".row(index).data(rData).draw();");
                                sb.AppendLine("}");
                                sb.AppendLine("else {");
                                sb.AppendLine("Grid" + name + ".row.add(data).draw();");
                                sb.AppendLine("}");
                            }
                        }
                        else
                        {
                            if (!config.OnSave.IsNullOrEmpty())
                            {
                                sb.AppendLine(config.OnSave + "(btn,data);");
                            }
                            else
                            {
                                sb.AppendLine("Grid" + name + ".row.add(data).draw();");
                            }
                        }

                        if (!config.OnSave.IsNullOrEmpty() && config.OnAfterSave.IsNullOrEmpty())
                        {
                        }
                        else if (!config.OnAfterSave.IsNullOrEmpty())
                        {
                            sb.AppendLine("if (mode == \"edit\") {");
                            sb.AppendLine("var rData = Grid" + name + ".row(index).data();");
                            sb.AppendLine("$.each(data, function(key, value) {");
                            sb.AppendLine("rData[key] = value;");
                            sb.AppendLine("});");
                            sb.AppendLine(config.OnAfterSave + "(rData);");
                            sb.AppendLine("}");
                        }
                        else
                        {
                            sb.AppendLine("$.confirm({");
                            sb.AppendLine("title: null,");
                            sb.Append("content: \"");
                            sb.Append(Translation.CenterLang.Center.SaveCompleted);
                            sb.AppendLine("\",");
                            sb.AppendLine("columnClass: 'medium',");
                            sb.AppendLine("buttons: {");
                            sb.AppendLine("confirm: {");
                            sb.Append("text: '");
                            sb.Append(Translation.CenterLang.Center.OK);
                            sb.AppendLine("',");
                            sb.AppendLine("btnClass: \"btn-primary\",");
                            sb.AppendLine("action: function () {");
                            sb.AppendLine("$(\"#md" + name + "\").modal('hide');");
                            sb.AppendLine("}");
                            sb.AppendLine("}");
                            sb.AppendLine("}");
                            sb.AppendLine("});");
                        }
                        sb.AppendLine("}");
                    }
                    sb.AppendLine("}");
                }
                #endregion

            }
            #region OnClick_btnAdd
            if (config.VisibleAdd && !config.IsReadOnly)
            {
                sb.AppendLine("function OnClick_btnAdd" + name + "(e) {");
                sb.AppendLine("var ReturnValidate = true;");
                if (!config.OnBeforeAdd.IsNullOrEmpty())
                {
                    //OnBeforeAdd(e);
                    sb.Append("ReturnValidate = ");
                    sb.Append(config.OnBeforeAdd);
                    sb.AppendLine("(e);");
                }
                sb.AppendLine("if(ReturnValidate || ReturnValidate == undefined) {");
                sb.Append("$(\"#mdTitle" + name + "\").text(\"");
                sb.Append(Translation.CenterLang.Center.Add + " " + label);
                sb.AppendLine("\");");
                sb.AppendLine("$(\"#btnSave" + name + "\").data(\"mode\", \"add\");");
                sb.AppendLine("$(\"#md" + name + "\").modal();");
                sb.AppendLine("}");
                sb.AppendLine("return false;");
                sb.AppendLine("}");
            }
            #endregion

            #region OnEdit
            if (config.VisibleEditColumn && !config.IsReadOnly && !config.CustomEdit)
            {
                sb.AppendLine("function OnEdit" + name + "(i,name) {");
                sb.AppendLine("var data = Grid" + name + ".row(i).data();");
                sb.AppendLine("$.each($(\"#form" + name + "\").serializeArray(), function (i, element) {");
                sb.AppendLine("var ctrl = $(\"#form" + name + " [name='\" + element.name+\"']\");");
                sb.AppendLine("if(ctrl!= undefined && ctrl != null && data[element.name]!=undefined && data[element.name]!=null && !isNullOrEmpty(data[element.name])){");
                sb.AppendLine("if(ctrl[0].type == \"checkbox\"){");
                sb.AppendLine("ctrl.prop(\"checked\",data[element.name]==\"Y\");");
                sb.AppendLine("ctrl.val(data[element.name] + \"\");");
                sb.AppendLine("}else if(ctrl[0].type == \"text\" && ctrl.hasClass(\"datepicker-input\")){");
                sb.AppendLine("ctrl.val(jsonDateToFormat(data[element.name]));");
                sb.AppendLine("if(ctrl.hasClass(\"datepicker\")){");
                sb.AppendLine("ctrl.datepicker(\"setDate\", $.stringToDate(jsonDateToFormat(data[element.name])));");
                sb.AppendLine("}");
                sb.AppendLine("}else if(ctrl[0].type == \"text\" && ctrl.hasClass(\"number-format\")){");
                sb.AppendLine("ctrl.val(toNumberFormat(data[element.name]));");
                sb.AppendLine("ctrl.focusout();");
                sb.AppendLine("}else{");
                sb.AppendLine("ctrl.val(data[element.name] + \"\");");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("});");
                if (!config.OnBeforeEdit.IsNullOrEmpty())
                {
                    //OnBeforeEdit(i,row);
                    sb.Append(config.OnBeforeEdit);
                    sb.AppendLine("(i,name,data);");
                }
                sb.Append("$(\"#mdTitle" + name + "\").text(\"");
                sb.Append(Translation.CenterLang.Center.Edit);
                sb.Append(" " + label);
                sb.AppendLine("\");");
                sb.AppendLine("$(\"#btnSave" + name + "\").data(\"index\", i);");
                sb.Append("$(\"#btnSave" + name + "\").data(\"mode\", \"edit\");");
                sb.AppendLine("$(\"#md" + name + "\").modal();");
                if (!config.OnAfterModalShow.IsNullOrEmpty())
                {
                    sb.AppendLine("$(\"#md" + name + "\").on(\"shown.bs.modal\", function() {");
                    sb.Append(config.OnAfterModalShow);
                    sb.AppendLine("(i,name,data);");
                    sb.AppendLine("});");
                }
                sb.AppendLine("return false;");
                sb.AppendLine("}");
            }
            #endregion

            #region OnClick_btnDelete
            if (config.VisibleDelete && !config.IsReadOnly)
            {
                sb.AppendLine("function OnClick_btnDelete" + name + "() {");
                sb.AppendLine("var data = Grid" + name + ".rows({ selected: true }).data();");
                sb.AppendLine("if (data.length > 0) {");
                if (!config.OnDelete.IsNullOrEmpty())
                {
                    sb.Append(config.OnDelete);
                    sb.AppendLine("(data);");
                }
                else
                {
                    sb.AppendLine("Grid" + name + ".rows({ selected: true }).remove().draw();");
                }
                sb.AppendLine("}");
                sb.AppendLine("else {");
                sb.Append("var msg = $('<p id =\"validError" + name + "\" class=\"text-danger no-margin\">");
                sb.Append(Translation.CenterLang.Center.PleaseSelectData);
                sb.AppendLine("</p>');");
                sb.AppendLine("if($('#Grid" + name + "').closest('.gridIncludeToolbar').length > 0){");
                sb.AppendLine("msg.insertBefore($('#Grid" + name + "').closest('.gridIncludeToolbar'));");
                sb.AppendLine("}");
                sb.AppendLine("else{");
                sb.AppendLine("msg.insertBefore($('#Grid" + name + "').closest('.dataTables_wrapper'));");
                sb.AppendLine("}");
                sb.AppendLine("$('#Grid" + name + "').click(function(){");
                sb.AppendLine("$(\"#validError" + name + "\").remove();");
                sb.AppendLine("});");
                sb.AppendLine("}");
                sb.AppendLine("}");
            }
            #endregion

            #region bindGrid
            sb.AppendLine("function bindGrid" + name + "() {");

            sb.AppendLine("var option = {");
            sb.AppendLine("\"deferRender\": true,");
            sb.AppendLine("\"paging\": true,");
            sb.AppendLine("\"processing\": true,");
            sb.AppendLine("\"destroy\": !$.isEmptyObject(Grid" + name + "),");
            sb.Append("\"searching\": " + config.Searching.AsString().ToLower() + ",");
            if (config.ScrollX)
            {
                sb.AppendLine("\"scrollX\": true,");
            }
            sb.Append("\"dom\": \"");
            sb.Append("<'table-responsive'tr><'row'<'col-sm-5'");
            if (config.VisibleExportButton)
            {
                sb.Append(" B");
            }
            sb.AppendLine("<'pull-left'l><'pull-left'i>><'col-sm-7'p>>\",");
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
            sb.Append("\"infoFiltered\": \"");
            sb.Append(Translation.CenterLang.Center.gridInfoFiltered);
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

            if (config.VisibleExportButton)
            {
                sb.AppendLine("buttons: [");
                //sb.AppendLine("{");
                //sb.AppendLine("className: 'btn btn-default btn-xs btn-white btn-success btn-round',");
                //sb.AppendLine("text: '<i class=\"fa fa-file-excel-o bigger-120 green\" data-toggle=\"tooltip\" data-placement=\"bottom\" title=\"\"></i>',");
                //sb.AppendLine("charset: 'UTF16-LE',");
                //sb.AppendLine("action: function ( e, dt, node, config ) {");
                //#region action
                //sb.AppendLine("var dataGrid = Grid" + name + ".rows().data();");
                ////sb.AppendLine("var formName = '"+ config.CustomsExportConfig.FormName + "';");
                ////sb.AppendLine("var form = $(\"form[id=\" + formName + \"]\");");
                ////sb.AppendLine("var formData = form.serializeFormData();");
                ////sb.AppendLine("$.ajax({");
                ////sb.AppendLine("url: '" + config.CustomsExportConfig.Url + "',");
                ////sb.AppendLine("type: 'get',");
                ////sb.AppendLine("contentType: false,");
                ////sb.AppendLine("processData: false,");
                ////sb.AppendLine("data: formData,");
                ////sb.AppendLine("success: {");
                ////sb.AppendLine("},");
                ////sb.AppendLine("error: OnAjaxError");
                ////sb.AppendLine("});");
                //#endregion
                //sb.AppendLine("}");
                //sb.AppendLine("}");
                sb.AppendLine("{");
                sb.AppendLine("extend: 'excel',");
                sb.AppendLine("text: '<i class=\"fa fa-file-excel-o bigger-120 green\"></i>',");
                sb.AppendLine("className: 'btn btn-xs btn-white btn-success btn-round',");
                sb.AppendLine("exportOptions: { columns: ExportNotIcon },");
                sb.AppendLine("charset: 'UTF16-LE',");
                sb.Append("}");
                sb.AppendLine("],");
            }

            int startIdx = 0;
            var col = columns.Where(m => m != null && (m.visible != "false" || (m.visible == "false" && m.visiblerender))).ToList();
            //if (config.DeleteConfig == null || !prgConfig.IsROLE_DEL)
            //{
            //    config.VisibleCheckBox = false;
            //}
            if ((config.VisibleCheckBox || config.VisibleDelete) && !config.IsReadOnly)
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

            if ((config.VisibleCheckBox || config.VisibleDelete) && !config.IsReadOnly)
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

            if (config.VisibleEditColumn && !config.IsReadOnly)
            {
                if (startIdx > 0)
                {
                    sb.AppendLine(",");
                }
                sb.AppendLine("{");
                sb.AppendLine("data: null,");
                sb.AppendLine("orderable: false,");
                sb.AppendLine("width: '3%',");
                sb.AppendLine("className: \"dt-head-center dt-head-nowrap\",");
                sb.AppendLine("render: function (data, type, full, meta) {");
                sb.AppendLine("var tag = '';");
                if (!config.ConditionShowEdit.IsNullOrEmpty())
                {
                    sb.AppendLine("if(" + config.ConditionShowEdit + ")");
                    sb.AppendLine("{");
                    sb.AppendLine("tag = '<a name=\"Edit\" href=\\\"#\\\" ><i class=\\\"ace-icon fa fa-pencil bigger-130 orange2\\\" data-toggle=\"tooltip\" data-placement=\"right\" title=" + Translation.CenterLang.Center.Edit + " ></i></a>';");
                    sb.AppendLine("}");
                }
                else
                {
                    sb.AppendLine("tag = '<a name=\"Edit\" href=\\\"#\\\" ><i class=\\\"ace-icon fa fa-pencil bigger-130 orange2\\\" data-toggle=\"tooltip\" data-placement=\"right\" title=" + Translation.CenterLang.Center.Edit + " ></i></a>';");
                }
                sb.AppendLine("return tag;");
                sb.AppendLine("}");
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
                        orderIdx.Add(startIdx + c);
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
                                                                        m.Name != "IsHeadNoWrap" &&
                                                                        m.Name != "visible").ToList();
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
                                    sb.Append("\"" + prop.Name + "\":\"" + value + "\"");
                                }
                            }
                            else if (prop.Name == "data")
                            {
                                sb.Append("\"" + prop.Name + "\":null");
                            }
                        }
                        else if (prop.PropertyType == typeof(bool))
                        {
                            if (prop.Name == "visiblerender")
                            {
                                if (value.ToString().ToLower() == "true")
                                {
                                    sb.Append("\"visible\": false");
                                }
                            }
                            else
                            {
                                sb.Append("\"" + prop.Name + "\":" + Convert.ToString(value).ToLower());
                            }
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
                            else if (prop.Name != "type" && !(prop.Name == "visiblerender" && value.ToString().ToLower() != "true"))
                            {
                                sb.AppendLine(",");
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
            if (!config.OnRowCallback.IsNullOrEmpty())
            {
                //function( row, data, index )
                sb.Append("\"rowCallback\":");
                sb.Append(config.OnRowCallback);
                sb.AppendLine(",");
            }
            sb.AppendLine("drawCallback: function () {");
            if (config.VisibleDelete && !config.IsReadOnly)
            {
                sb.AppendLine("if ($('#Grid" + name + " tbody .dataTables_empty').length) {");
                if (config.VisibleCheckBox || config.VisibleDelete)
                {
                    sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", false);");
                }
                sb.AppendLine("$('#btnDelete" + name + "').hide();");
                sb.AppendLine("}");
                sb.AppendLine("else {");
                if (config.IsRequired)
                {
                    sb.AppendLine("$('.widget-box').removeClass('has-error');");
                    sb.AppendLine("$(\"#notWidget" + name + "\").empty().hide();");
                }
                sb.AppendLine("$('#btnDelete" + name + "').show();");
                sb.AppendLine("}");
            }
            sb.AppendLine("$('[data-toggle=\"tooltip\"]').tooltip({");
            sb.AppendLine("trigger: 'hover'");
            sb.AppendLine("});");
            if (!config.OnDrawCallback.IsNullOrEmpty())
            {
                sb.Append(config.OnDrawCallback);
                sb.AppendLine("();");
            }
            sb.AppendLine("}");
            sb.AppendLine("};");

            if (config.DefaultConfig != null)
            {
                sb.AppendLine("var result = {};");
                if (config.DefaultConfig.Parameters != null)
                {
                    foreach (var item in config.DefaultConfig.Parameters)
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
                //sb.AppendLine("if (result.mode != \"R\") {");
                //sb.AppendLine("$(\"#rowGrid" + name + "\").removeClass(\"hidden\");");
                sb.AppendLine("option[\"ajax\"]= {");
                sb.Append("url: \"");
                sb.Append(config.DefaultConfig.Url);
                sb.AppendLine("\",");
                sb.AppendLine("type: \"post\",");
                sb.AppendLine("error: OnAjaxError,");
                sb.AppendLine("data: result");
                sb.AppendLine("};");
                //sb.AppendLine("}");
            }
            sb.AppendLine("Grid" + name + " = $(\"#Grid" + name + "\").DataTable(option);");

            if (config.VisibleCheckBox && !config.IsReadOnly)
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
                sb.AppendLine("});");
                sb.AppendLine("$(\"#selectAllGrid" + name + "\").change(function () {");
                sb.AppendLine("if (this.checked) {");
                sb.AppendLine("Grid" + name + ".rows().select();");
                sb.AppendLine("}");
                sb.AppendLine("else {");
                sb.AppendLine("Grid" + name + ".rows().deselect();");
                sb.AppendLine("}");
                sb.AppendLine("});");
                sb.AppendLine("Grid" + name + ".on('select', function (e, dt, type, indexes) {");
                sb.AppendLine("var allRows = Grid" + name + ".rows().data().count();");
                sb.AppendLine("var selectedRows = Grid" + name + ".rows({ selected: true }).data().count();");
                sb.AppendLine("if (allRows == selectedRows) {");
                sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", true);");
                sb.AppendLine("}");
                sb.AppendLine("})");
                sb.AppendLine(".on('deselect', function (e, dt, type, indexes) {");
                sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", false);");
                sb.AppendLine("});");
            }


            if (config.VisibleEditColumn && !config.IsReadOnly)
            {
                sb.AppendLine("$(\"#Grid" + name + " tbody\").on(\"click\", \"a\", function (e) {");
                sb.AppendLine("var idx = Grid" + name + ".row($(this).parent()).index();");
                sb.AppendLine("return OnEdit" + name + "(idx ,this.name);");
                sb.AppendLine("});");
            }
            if (config.Searching)
            {
                sb.AppendLine("$('#Searching" + name + "').on('keyup change', function (e) {");
                sb.AppendLine("Grid" + name + ".search(this.value).draw();");
                sb.AppendLine("});");
            }
            sb.AppendLine("}");
            #endregion

            #region ready
            sb.AppendLine("$(document).ready(function () {");
            var colDate = columns.Where(m => m != null && (m.type == ColumnsType.Date || m.type == ColumnsType.DateTime)).Select(m => m.data).ToList();
            if (colDate != null && colDate.Count > 0)
            {
                sb.Append("$(\"#Grid" + name + "\").data('coldate','");
                sb.Append(string.Join(",", colDate));
                sb.AppendLine("');");
            }
            sb.AppendLine("bindGrid" + name + "();");
            if (config.VisibleAdd && !config.IsReadOnly)
            {
                sb.AppendLine("$(\"#btnAdd" + name + "\").click(OnClick_btnAdd" + name + ");");
            }
            if ((config.VisibleAdd || config.VisibleEditColumn) && !config.IsReadOnly)
            {
                sb.AppendLine("$(\"#btnSave" + name + "\").vsmConfirm({");
                sb.AppendLine("title: null,");
                sb.AppendLine("closeIcon: true,");
                sb.AppendLine("closeIconClass: 'fa fa-close',");
                sb.AppendLine("columnClass: 'medium',");
                sb.AppendLine("buttons: {");
                sb.AppendLine("confirm: {");
                sb.Append("text: '");
                sb.Append(Translation.CenterLang.Center.OK);
                sb.AppendLine("',");
                sb.AppendLine("btnClass: \"btn-primary\",");
                sb.AppendLine("action: OnClick_btnSave" + name + "");
                sb.AppendLine("},");
                sb.AppendLine("cancel: {");
                sb.Append("text: '");
                sb.Append(Translation.CenterLang.Center.Cancel);
                sb.AppendLine("',");
                sb.AppendLine("btnClass: \"btn-primary\"");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("});");
                sb.AppendLine("$(\"#md" + name + "\").on('hidden.bs.modal', function(e) {");
                sb.AppendLine("resetForm" + name + "();");
                sb.AppendLine("});");
            }
            if (config.VisibleDelete && !config.IsReadOnly)
            {
                sb.AppendLine("$(\"#btnDelete" + name + "\").confirm({");
                sb.AppendLine("title: null,");
                sb.AppendLine("content: \"<br/>" + Translation.CenterLang.Center.ConfirmDelete + "\",");
                sb.AppendLine("columnClass: 'medium',");
                sb.AppendLine("buttons: {");
                sb.AppendLine("confirm: {");
                sb.Append("text: '");
                sb.Append(Translation.CenterLang.Center.OK);
                sb.AppendLine("',");
                sb.AppendLine("btnClass: \"btn-primary\",");
                sb.AppendLine("action: OnClick_btnDelete" + name + "");
                sb.AppendLine("},");
                sb.AppendLine("cancel: {");
                sb.Append("text: '");
                sb.Append(Translation.CenterLang.Center.Cancel);
                sb.AppendLine("',");
                sb.AppendLine("btnClass: \"btn-primary\"");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("});");
            }
            sb.AppendLine("});");
            #endregion

            sb.AppendLine("</script>");
            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region Captcha
        public static MvcHtmlString GetVSMCAPTCHA(this HtmlHelper html)
        {
            var sb = new StringBuilder();
            sb.Append("<div id=\"divCaptcha\" class=\"captcha\"></div>");
            sb.Append("<a href=\"javascript:void(0)\" id=\"btnCaptchaRefresh\" name=\"btnCaptchaRefresh\" >");
            sb.Append("<i class=\"fa fa-refresh bigger-130\"></i>");
            sb.Append("</a>");
            //sb.Append("<img src=\"");
            //sb.Append(VirtualPathUtility.ToAbsolute("~/Uploads/Image/RefreshCaptcha-icon.png"));
            //sb.Append("\" class=\"img-responsive\" alt=\"Captcha\" id=\"btnCaptchaRefresh\" />");
            sb.Append("<input type=\"text\" name=\"CaptchaImg\" id=\"CaptchaImg\" autocomplete=\"off\" class=\"form-control\" />");

            return MvcHtmlString.Create(sb.ToString());
        }

        #endregion

        #region Widgets
        public static MvcHtmlString GetVSMWidgetsBegin(this HtmlHelper html,
            string title, WidgetColor color = WidgetColor.None, bool collapsed = false, int col = 12, bool isRequired = false, string name = "")
        {
            return GetVSMWidgetsBegin(html, title, color.GetDescription(), collapsed, col, isRequired, name);
        }
        public static MvcHtmlString GetVSMWidgetsBegin(this HtmlHelper html,
            string title, WidgetColor color = WidgetColor.None, bool collapsed = false, int col = 12, bool isRequired = false, string name = "", params ButtonConfig[] links)
        {
            return GetVSMWidgetsBegin(html, title, color.GetDescription(), collapsed, col, isRequired, name, links);
        }
        public static MvcHtmlString GetVSMWidgetsBegin(this HtmlHelper html,
            string title, string color = "", bool collapsed = false, int col = 12, bool isRequired = false, string name = "", params ButtonConfig[] links)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div class=\"row\">");
            sb.Append("<div class=\"col-md-");
            sb.Append(col.AsString());
            sb.AppendLine(" widget-container-col\">");
            sb.Append("<div");
            if (!name.IsNullOrEmpty())
            {
                sb.Append(" id=\"wg" + name + "\"");
            }
            sb.Append(" class=\"widget-box");
            if (!color.IsNullOrEmpty())
            {
                sb.Append(" " + color);
            }
            if (collapsed)
            {
                sb.Append(" collapsed");
            }
            if (isRequired)
            {
                sb.Append(" required");
            }
            sb.AppendLine("\">");
            sb.AppendLine("<div class=\"widget-header widget-header-small\">");
            sb.AppendLine("<h6 class=\"widget-title\">");
            sb.AppendLine(title);
            sb.AppendLine("</h6>");
            sb.AppendLine("<div class=\"widget-toolbar\">");
            if (links != null)
            {
                foreach (var link in links)
                {
                    sb.Append("<a ");
                    if (!link.Name.IsNullOrEmpty())
                    {
                        sb.Append("id =\"");
                        sb.Append(link.Name);
                        sb.Append("\" value=\"");
                        sb.Append(link.Name);
                        sb.Append("\"");
                    }
                    if (!link.CssClass.IsNullOrEmpty())
                    {
                        sb.Append(" class=\"");
                        sb.Append(link.CssClass);
                        sb.Append("\"");
                    }
                    //else
                    //{
                    //    sb.Append(" class=\"btn btn-sm btn-info\"");
                    //}
                    if (!link.Url.IsNullOrEmpty())
                    {
                        sb.Append(" href=\"");
                        sb.Append(link.Url);
                        sb.Append("\"");
                    }
                    else
                    {
                        sb.Append(" href=\"javascript:void(0)\"");
                    }
                    if (link.Tooltip)
                    {
                        sb.Append(" data-toggle=\"tooltip\" data-placement=\"right\"");
                    }
                    if (!link.Title.IsNullOrEmpty())
                    {
                        sb.Append(" title=\"");
                        sb.Append(link.Title);
                        sb.Append("\"");
                    }
                    if (link.HtmlAttribute.Count > 0)
                    {
                        foreach (var item in link.HtmlAttribute)
                        {
                            sb.Append(" ");
                            sb.Append(item.Key);
                            sb.Append("=\"");
                            sb.Append(item.Value);
                            sb.Append("\"");
                        }
                    }

                    sb.AppendLine("\">");
                    if (!link.IconCssClass.IsNullOrEmpty() && link.IconPosition == StandardIconPosition.BeforeText)
                    {
                        sb.Append("<i class=\"ace-icon fa ");
                        sb.Append(link.IconCssClass);
                        sb.Append("\"></i>");
                    }
                    if (!link.Text.IsNullOrEmpty())
                    {
                        sb.AppendLine(link.Text);
                    }
                    if (!link.IconCssClass.IsNullOrEmpty() && link.IconPosition == StandardIconPosition.AfterText)
                    {
                        sb.Append("<i class=\"ace-icon fa ");
                        sb.Append(link.IconCssClass);
                        sb.Append("\"></i>");
                    }
                    sb.AppendLine("</a>");
                }
            }
            sb.AppendLine("<a href=\"javascript:void(0)\" data-action=\"collapse\">");
            if (collapsed)
            {
                sb.AppendLine("<i class=\"ace-icon fa fa-chevron-down\"></i>");
            }
            else
            {
                sb.AppendLine("<i class=\"ace-icon fa fa-chevron-up\"></i>");
            }
            sb.AppendLine("</a>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div class=\"widget-body\">");
            sb.AppendLine("<div class=\"widget-main\">");
            sb.AppendLine("<div class=\"row\">");
            sb.AppendLine("<div class=\"col-md-12\">");
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString GetVSMWidgetsEnd(this HtmlHelper html)
        {
            var sb = new StringBuilder();
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region ListVSMParameter
        public static List<VSMParameter> GetListVSMParameter<TModel>(this HtmlHelper<TModel> html, params VSMParameter[] parameter)
        {
            return GetListVSMParameter(parameter);
        }
        public static List<VSMParameter> GetListVSMParameter(params VSMParameter[] parameter)
        {
            List<VSMParameter> ls = null;
            if (parameter != null)
            {
                ls = new List<VSMParameter>();
                ls.AddRange(parameter);
            }
            return ls;
        }
        #endregion

        #region FileUpload
        public static MvcHtmlString GetVSMFileUploadFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool isRequired = false, int labelWidth = 4, bool showLabel = true, bool multiple = false, string customType = "", bool isReadOnly = false, string prgCode = "")
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string label = GetControlLabel(metadata, expressionText);
            string name = GetControlName(html, expressionText);
            var sysConfig = /*html.ViewContext.HttpContext.Session[SessionSystemName.SYS_ConfigFile] as*/ new SystemConfigFile();
            if (prgCode.IsNullOrEmpty())
            {
                prgCode = html.ViewContext.HttpContext.Session[SessionSystemName.SYS_CurrentPRG_CODE].AsString();
            }
            var allowFile = AllowFile(sysConfig, customType, prgCode);
            var sb = new StringBuilder();
            if (showLabel)
            {
                sb.Append("<div class=\"form-group");
                if (isRequired)
                {
                    sb.Append(" required\" aria-required=\"true\"");
                }
                else
                {
                    sb.AppendLine("\"");
                }
                sb.AppendLine(">");
                sb.AppendLine("<label class=\"control-label col-md-" + labelWidth + "\">");
                sb.Append(label);
                sb.Append("</label>");
            }
            else
            {
                labelWidth = 0;
                sb.AppendLine("<div class=\"row\">");
            }
            sb.AppendLine("<div class=\"col-md-" + (12 - labelWidth) + "\">");
            if (!isReadOnly)
            {
                sb.Append("<input type=\"file\" id=\"f" + name + "\" name=\"" + name + "\" style=\"display:none;\" data-vsmctrl=\"true\"");
                if (!allowFile.IsNullOrEmpty())
                {
                    sb.Append(" accept=\"" + allowFile + "\"");
                }
                if (multiple)
                {
                    sb.Append(" data-multiple=\"true\" multiple");
                }

                sb.Append(" data-isrequired=\"" + isRequired.ToString().ToLower() + "\"");
                sb.AppendLine("/>");
            }
            sb.AppendLine("<div class=\"gridIncludeToolbar\">");
            if (!isReadOnly)
            {
                sb.AppendLine("<div class=\"gridToolbar\">");
                sb.Append("<a href=\"javascript:void(0)\" id=\"btnAdd" + name + "\" data-toggle=\"tooltip\"");
                sb.Append(" title=\"" + Translation.CenterLang.Center.AddFile + "\"");
                sb.AppendLine(">");
                sb.AppendLine("<i class=\"ace-icon fa fa-plus-circle green bigger-125\"></i>");
                sb.AppendLine("</a>");
                sb.Append("<a href=\"javascript:void(0)\" id=\"btnDelete" + name + "\" data-toggle=\"tooltip\"");
                sb.Append(" title=\"" + Translation.CenterLang.Center.DeleteFile + "\"");
                sb.AppendLine(">");
                sb.AppendLine("<i class=\"ace-icon fa fa-trash red bigger-125\"></i>");
                sb.AppendLine("</a>");
                if (!allowFile.IsNullOrEmpty())
                {
                    var nSysConfig = sysConfig.Details;
                    if (!prgCode.IsNullOrEmpty())
                    {
                        var confByPrgCode = nSysConfig.Where(m => m.PRG_CODE == prgCode).ToList();
                        if (confByPrgCode != null && confByPrgCode.Count > 0)
                        {
                            nSysConfig = confByPrgCode;
                        }
                    }
                    if (!customType.IsNullOrEmpty())
                    {
                        var ctTypes = customType.Replace(".", "").Split(',');
                        var conf = nSysConfig.Where(m => ctTypes.Contains(m.FILE_TYPE)).ToList();
                        if (conf.Count > 0)
                        {
                            nSysConfig = conf;
                        }
                    }
                    var types = new List<string>();
                    string tplType = ".{0}(&le; {1}MB)";
                    foreach (var item in nSysConfig.Distinct())
                    {
                        types.Add(string.Format(tplType, item.FILE_TYPE.Replace(".", ""), item.FILE_SIZE));
                    }
                    var nType = string.Join(",", types);
                    sb.Append("<span class=\"help-block\">");
                    sb.Append(Translation.CenterLang.Center.SupportFileType.Replace("{tplFileType}", nType));
                    sb.AppendLine("</span>");
                }
                sb.AppendLine("</div>");
            }
            sb.AppendLine("<table id=\"Grid" + name + "\" class=\"table table-striped table-bordered table-hover\" data-tabletype=\"fileupload\"></table>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString GetVSMFileUploadScriptFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool multiple = false, string customType = "", DefaultConfig config = null, string onChangeName = "", bool isReadOnly = false, DefaultConfig downloadConfig = null, string prgCode = "")
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string label = GetControlLabel(metadata, expressionText);
            string name = GetControlName(html, expressionText);
            var sysConfig = new SystemConfigFile();//html.ViewContext.HttpContext.Session["SYS_ConfigFile"] as SystemConfigFile;
            if (prgCode.IsNullOrEmpty())
            {
                prgCode = html.ViewContext.HttpContext.Session[SessionSystemName.SYS_CurrentPRG_CODE].AsString();
            }
            var allowFile = AllowFile(sysConfig, customType, prgCode);

            var sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\" > ");

            sb.AppendLine("var Grid" + name + " = {};");
            if (!isReadOnly)
            {
                #region OnClick_btnAdd
                sb.AppendLine("function OnClick_btnAdd" + name + "(e) {");
                sb.AppendLine("if ($(\"#validError" + name + "\").length > 0) {");
                sb.AppendLine("$(\"#validError" + name + "\").remove();");
                sb.AppendLine("}");
                sb.AppendLine("$(\"#f" + name + "\").click();");
                sb.AppendLine("return false;");
                sb.AppendLine("}");
                #endregion

                #region OnChange_f
                sb.AppendLine("function OnChange_f" + name + "(e) {");
                sb.AppendLine("if ($(\"#validError" + name + "\").length > 0) {");
                sb.AppendLine("$(\"#validError" + name + "\").remove();");
                sb.AppendLine("}");
                sb.AppendLine("var f = this;");
                sb.Append("if (f.files.length > 0) {");
                sb.Append("var allowFile = [");
                if (sysConfig.Details != null && sysConfig.Details.Count > 0)
                {
                    var nSysConfig = sysConfig.Details;
                    if (!prgCode.IsNullOrEmpty())
                    {
                        var confByPrgCode = nSysConfig.Where(m => m.PRG_CODE == prgCode).ToList();
                        if (confByPrgCode != null && confByPrgCode.Count > 0)
                        {
                            nSysConfig = confByPrgCode;
                        }
                    }
                    if (!customType.IsNullOrEmpty())
                    {
                        var types = customType.Replace(".", "").Split(',');
                        var conf = nSysConfig.Where(m => types.Contains(m.FILE_TYPE)).ToList();
                        if (conf.Count > 0)
                        {
                            nSysConfig = conf;
                        }
                    }
                    var i = 1;
                    foreach (var item in nSysConfig)
                    {
                        sb.Append("{");
                        sb.Append("type:'");
                        sb.Append(item.FILE_TYPE.Replace(".", ""));
                        sb.AppendLine("',");
                        sb.Append("size:");
                        sb.Append(item.FILE_SIZE.AsString());

                        if (i != nSysConfig.Count)
                        {
                            sb.AppendLine("},");
                        }
                        else
                        {
                            sb.Append("}");
                        }
                        i++;
                    }
                }
                sb.AppendLine("];");
                if (multiple)
                {
                    sb.AppendLine("var oldData = Grid" + name + ".rows().data().toArray();");
                    sb.Append("if ((oldData.length + f.files.length) <= ");
                    sb.Append(sysConfig.NO_OF_FILE);
                    sb.AppendLine(") {");
                    sb.AppendLine("var totalSize = 0;");
                    sb.AppendLine("var fileExists = [];");
                    sb.AppendLine("$.each(oldData, function (j, obj){");
                    sb.AppendLine("if(obj.FILE_NAME!=null&&obj.FILE_NAME!=\"\"){");
                    sb.AppendLine("fileExists.push(obj.FILE_NAME);");
                    sb.AppendLine("}");
                    sb.AppendLine("totalSize+=toNumber(obj.FILE_SIZE);");
                    sb.AppendLine("});");
                    sb.AppendLine("var nData = [];");
                    sb.AppendLine("var fileDup = [];");
                    sb.AppendLine("var fileNotAllowType = [];");
                    sb.AppendLine("var fileNotAllowSize = [];");
                    sb.AppendLine("$.each(f.files, function (i, file) {");
                    sb.AppendLine("var exists = false;");
                    sb.AppendLine("if ($.inArray(file.name, fileExists) != -1) {");
                    sb.AppendLine("exists = true;");
                    sb.AppendLine("if ($.inArray(file.name, fileDup) == -1) {");
                    sb.AppendLine("fileDup.push(file.name);");
                    sb.AppendLine("}");
                    sb.AppendLine("}");
                    sb.AppendLine("var ext = file.name.split('.').pop();");
                    sb.AppendLine("var size = file.size / 1048576;");
                    sb.AppendLine("var allowType = false;");
                    sb.AppendLine("var allowSize = false;");
                    sb.AppendLine("$.each(allowFile, function (tIdx, obj) {");
                    sb.AppendLine("if (obj.type == ext) {");
                    sb.AppendLine("allowType = true;");
                    sb.AppendLine("if (size <= obj.size) {");
                    sb.AppendLine("allowSize = true;");
                    sb.AppendLine("} else {");
                    sb.AppendLine("fileNotAllowSize.push(obj);");
                    sb.AppendLine("}");
                    sb.AppendLine("return false;");
                    sb.AppendLine("}");
                    sb.AppendLine("});");
                    sb.AppendLine("if (!allowType && $.inArray(ext, fileNotAllowType) == -1) {");
                    sb.AppendLine("fileNotAllowType.push(ext);");
                    sb.AppendLine("}");
                    sb.AppendLine("if (!exists && allowType && allowSize) {");
                    sb.AppendLine("var info = {};");
                    sb.AppendLine("info[\"FILE_NAME\"] = file.name;");
                    sb.AppendLine("info[\"File\"] = file;");
                    sb.AppendLine("info[\"FILE_SIZE\"] = size;");
                    sb.AppendLine("info[\"FILE_DATE\"] = '/Date('+file.lastModifiedDate.getTime()+')/';");
                    sb.AppendLine("nData.push(info);");
                    sb.AppendLine("totalSize += toNumber(size);");
                    sb.AppendLine("}");
                    sb.AppendLine("});");
                    sb.Append("if (nData.length > 0 && fileDup.length == 0 && fileNotAllowType.length == 0 && fileNotAllowSize.length == 0 && totalSize<=");
                    sb.Append(sysConfig.FILE_SIZE);
                    sb.AppendLine(") {");
                    sb.AppendLine("Grid" + name + ".rows.add(nData).draw();");
                    sb.AppendLine("}");
                    sb.AppendLine("else if (fileNotAllowType.length != 0) {");
                    sb.Append("$(\"<span id =\\\"validError" + name + "\\\" class=\\\"text-danger\\\">");
                    sb.Append(Translation.CenterLang.Center.FileNotExistsSystemConfig);
                    sb.AppendLine("</span>\").insertBefore(\"#f" + name + "\");");
                    sb.AppendLine("}");
                    sb.AppendLine("else if (fileDup.length != 0) {");
                    sb.Append("$(\"<span id =\\\"validError" + name + "\\\" class=\\\"text-danger\\\">");
                    sb.Append(Translation.CenterLang.Center.FileAlreadyExists);
                    sb.AppendLine("</span>\").insertBefore(\"#f" + name + "\");");
                    sb.AppendLine("}");
                    sb.AppendLine("else if (fileNotAllowSize.length != 0) {");
                    sb.Append("$(\"<span id =\\\"validError" + name + "\\\" class=\\\"text-danger\\\">");
                    sb.Append(Translation.CenterLang.Center.FileMaximumSizeLimit);
                    sb.AppendLine("</span>\").insertBefore(\"#f" + name + "\");");
                    sb.AppendLine("}");
                    sb.AppendLine("else {");
                    sb.Append("$(\"<span id =\\\"validError" + name + "\\\" class=\\\"text-danger\\\">");
                    sb.Append(Translation.CenterLang.Center.AttachFileMaximumSizeLimit);
                    sb.AppendLine("</span>\").insertBefore(\"#f" + name + "\");");
                    sb.AppendLine("}");
                    sb.AppendLine("}");
                    sb.AppendLine("else {");
                    sb.Append("$(\"<span id =\\\"validError" + name + "\\\" class=\\\"text-danger\\\">");
                    sb.Append(Translation.CenterLang.Center.AttachFileMaximumLimit);
                    sb.AppendLine("</span>\").insertBefore(\"#f" + name + "\");");
                    sb.AppendLine("}");
                }
                else
                {
                    sb.AppendLine("Grid" + name + ".rows().remove().draw();");
                    sb.AppendLine("var info = {};");
                    sb.AppendLine("$.each(f.files, function (i, file) {");
                    sb.AppendLine("var ext = file.name.split('.').pop();");
                    sb.AppendLine("var size = file.size / 1048576;");
                    sb.AppendLine("var allowType = false;");
                    sb.AppendLine("var allowSize = true;");
                    sb.AppendLine("if(allowFile.length <= 0) {");
                    sb.AppendLine("allowType = true;");
                    sb.AppendLine("allowSize = true;");
                    sb.AppendLine("}");
                    sb.AppendLine("$.each(allowFile, function (tIdx, obj) {");
                    sb.AppendLine("if (obj.type == ext) {");
                    sb.AppendLine("allowType = true;");
                    sb.AppendLine("if (size <= obj.size) {");
                    sb.AppendLine("allowSize = true;");
                    sb.AppendLine("}");
                    sb.AppendLine("return false;");
                    sb.AppendLine("}");
                    sb.AppendLine("});");
                    sb.AppendLine("info[\"FILE_NAME\"] = file.name;");
                    sb.AppendLine("info[\"File\"] = file;");
                    sb.AppendLine("info[\"FILE_SIZE\"] = size;");
                    sb.AppendLine("info[\"FILE_DATE\"] = '/Date('+file.lastModifiedDate.getTime()+')/';");
                    sb.AppendLine("if(allowType && allowSize){");
                    sb.AppendLine("Grid" + name + ".row.add(info).draw();");
                    sb.AppendLine("}");
                    sb.AppendLine("else if (!allowType) {");
                    sb.Append("$(\"<span id =\\\"validError" + name + "\\\" class=\\\"text-danger\\\">");
                    sb.Append(Translation.CenterLang.Center.FileNotExistsSystemConfig);
                    sb.AppendLine("</span>\").insertBefore(\"#f" + name + "\");");
                    sb.AppendLine("}");
                    sb.AppendLine("else if (!allowSize) {");
                    sb.Append("$(\"<span id =\\\"validError" + name + "\\\" class=\\\"text-danger\\\">");
                    sb.Append(Translation.CenterLang.Center.FileMaximumSizeLimit);
                    sb.AppendLine("</span>\").insertBefore(\"#f" + name + "\");");
                    sb.AppendLine("}");
                    sb.AppendLine("});");
                }
                sb.AppendLine("var oldCtrl = $(\"#f" + name + "\");");
                sb.Append("var newCtrl = $('<input type=\"file\" id=\"f" + name + "\" name=\"" + name + "\" style=\"display:none;\" data-vsmctrl=\"true\"");
                if (!allowFile.IsNullOrEmpty())
                {
                    sb.Append(" accept=\"");
                    sb.Append(allowFile);
                    sb.Append("\"");
                }
                if (multiple)
                {
                    sb.Append(" data-multiple=\"true\" multiple");
                }
                sb.AppendLine("/>');");
                sb.AppendLine("var isrequired = oldCtrl.data(\"isrequired\");");
                sb.AppendLine("if(isrequired){");
                sb.AppendLine("newCtrl.data(\"isrequired\",isrequired);");
                sb.AppendLine("}");
                sb.AppendLine("newCtrl.change(OnChange_f" + name + ");");
                sb.AppendLine("oldCtrl.replaceWith(newCtrl);");
                if (!onChangeName.IsNullOrEmpty())
                {
                    sb.Append(onChangeName);
                    sb.AppendLine("();");
                }
                sb.AppendLine("}");
                sb.AppendLine("}");
                #endregion
            }
            #region bindGrid
            sb.AppendLine("function bindGrid" + name + "() {");

            if (config != null)
            {
                sb.AppendLine("var result = {};");
                if (config.Parameters != null && config.Parameters.Count > 0)
                {
                    for (int i = 0; i < config.Parameters.Count; i++)
                    {
                        if (config.Parameters[i].Type == VSMParameterType.ByModelData)
                        {
                            sb.AppendLine("result[\"" + config.Parameters[i].Name + "\"] =\"" + config.Parameters[i].Value + "\";");
                        }
                        else
                        {
                            sb.AppendLine("result[\"" + config.Parameters[i].Name + "\"] = $(\"#" + config.Parameters[i].Value + "\").val();");
                        }
                    }
                }
            }

            sb.AppendLine("Grid" + name + " = $(\"#Grid" + name + "\").DataTable({");
            sb.AppendLine("\"deferRender\": true,");
            sb.AppendLine("\"paging\": false,");
            sb.AppendLine("\"processing\": true,");
            sb.AppendLine("\"destroy\": !$.isEmptyObject(Grid" + name + "),");
            //sb.AppendLine("\"order\": [[1, \"asc\"]],");
            sb.AppendLine("\"info\": false,");
            sb.AppendLine("\"ordering\": false,");
            sb.AppendLine("\"searching\": false,");
            sb.AppendLine("\"lengthChange\": false,");
            sb.AppendLine("\"autoWidth\": false,");
            sb.AppendLine("\"scrollCollapse\": true,");
            sb.AppendLine("\"dom\": \"<'table-responsive'tr>\",");
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
            sb.Append("\"infoFiltered\": \"");
            sb.Append(Translation.CenterLang.Center.gridInfoFiltered);
            sb.AppendLine("\",");
            sb.Append("\"loadingRecords\": \"");
            sb.Append(Translation.CenterLang.Center.gridLoadingRecords);
            sb.AppendLine("\",");
            sb.Append("\"processing\": \"");
            sb.Append(Translation.CenterLang.Center.gridProcessing);
            sb.AppendLine("\",");
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

            var url = new UrlHelper(html.ViewContext.RequestContext);
            if (config != null)
            {
                sb.AppendLine("\"ajax\": {");
                sb.Append("url: \"");
                sb.Append(config.Url);
                sb.Append("\",");
                sb.AppendLine("type: \"GET\",");
                sb.AppendLine("data: result");
                //sb.AppendLine("\"dataSrc\": \"rows\"");            
                sb.AppendLine("},");
            }
            if (multiple && !isReadOnly)
            {
                sb.AppendLine("select: {");
                sb.AppendLine("style: 'multi',");
                sb.AppendLine("selector: 'td:first-child'");
                sb.AppendLine("},");
            }
            sb.AppendLine("\"columns\": [");
            if (multiple && !isReadOnly)
            {
                sb.AppendLine("{");
                sb.AppendLine("data: null,");
                sb.AppendLine("defaultContent: '',");
                sb.AppendLine("width: '2%',");
                sb.AppendLine("className: 'dt-head-center select-checkbox',");
                sb.AppendLine("orderable: false,");
                sb.AppendLine("title: '<input type=\"checkbox\" name=\"selectAllGrid" + name + "\" id=\"selectAllGrid" + name + "\" class=\"checkbox\">'");
                sb.AppendLine("},");
            }
            sb.AppendLine("{");
            sb.AppendLine("\"data\": \"ID\",");
            sb.AppendLine("\"orderable\": true,");
            sb.AppendLine("\"title\": \"\",");
            sb.AppendLine("\"width\": \"3%\",");
            sb.AppendLine("className: 'dt-head-center dt-head-nowrap',");
            sb.AppendLine("\"render\": function (data, type, full, meta) {");
            sb.AppendLine("var tag = '';");
            sb.AppendLine("if (data != null && data != '') {");
            sb.Append("tag += '<a data-toggle=\"tooltip\" data-placement=\"right\" target=\"_blank\" title=\"");
            sb.Append(Translation.CenterLang.Center.Download);
            sb.Append("\" href=\\\"");
            if (downloadConfig != null)
            {
                sb.Append(downloadConfig.Url);
            }
            else
            {
                sb.Append(url.Action("Download", "File", new { Area = "Ux" }));
            }
            sb.AppendLine("?ID=' + toUrlString(data);");
            sb.AppendLine("tag += '\\\" ><i class=\\\"ace-icon fa fa-download bigger-130\\\"></i></a>';");
            sb.AppendLine("}");
            sb.AppendLine("return tag;");
            sb.AppendLine("}");
            sb.AppendLine("},");
            sb.AppendLine("{");
            sb.AppendLine("\"data\": \"FILE_NAME\",");
            sb.Append("\"title\": \"");
            sb.Append(Translation.CenterLang.Center.FileName);
            sb.AppendLine("\",");
            sb.AppendLine("className: 'dt-head-center dt-head-nowrap',");
            sb.AppendLine("\"width\": \"59%\"");
            sb.AppendLine("},");
            sb.AppendLine("{");
            sb.AppendLine("\"data\": \"FILE_SIZE\",");
            sb.Append("\"title\": \"");
            sb.Append(Translation.CenterLang.Center.FileSize);
            sb.AppendLine("\",");
            sb.AppendLine("\"width\": \"9%\",");
            sb.AppendLine("className: 'dt-head-center dt-head-nowrap dt-body-right',");
            sb.AppendLine("\"render\": function (data, type, full, meta) {");
            sb.AppendLine("var nData = toNumberFormat(data, \"#,###,##0.####\");");
            sb.AppendLine("return (data != undefined && data != null) ? nData + \" MB\" : \"\";");
            sb.AppendLine("}");
            sb.AppendLine("},");
            sb.AppendLine("{");
            sb.AppendLine("\"data\": \"FILE_DATE\",");
            sb.Append("\"title\": \"");
            sb.Append(Translation.CenterLang.Center.FileDateModified);
            sb.AppendLine("\",");
            sb.AppendLine("\"width\": \"15%\",");
            sb.AppendLine("className: 'dt-head-center dt-head-nowrap dt-body-center',");
            sb.AppendLine("\"render\": function (data, type, full, meta) {");
            sb.Append("var nData ='';");
            sb.AppendLine("if(data!=null&&data!=''){");
            sb.AppendLine("nData = jsonDateToFormat(data, \"DD/MM/YYYY HH:mm:ss\");");
            sb.AppendLine("}");
            sb.AppendLine("return nData;");
            sb.AppendLine("}");
            sb.AppendLine("},");
            sb.AppendLine("{");
            sb.AppendLine("\"data\": \"CRET_DATE\",");
            sb.Append("\"title\": \"");
            sb.Append(Translation.CenterLang.Center.FileUploadDate);
            sb.AppendLine("\",");
            sb.AppendLine("\"width\": \"15%\",");
            sb.AppendLine("className: 'dt-head-center dt-head-nowrap dt-body-center',");
            sb.AppendLine("\"render\": function (data, type, full, meta) {");
            sb.Append("var nData ='<span class=\"text-danger\">");
            sb.Append(Translation.CenterLang.Center.FileWaitingUpload);
            sb.AppendLine("</span>';");
            sb.AppendLine("if(data!=null&&data!=''){");
            sb.AppendLine("nData = jsonDateToFormat(data, \"DD/MM/YYYY HH:mm:ss\");");
            sb.AppendLine("}");
            sb.AppendLine("return nData;");
            sb.AppendLine("}");
            sb.AppendLine("}],");
            sb.AppendLine("drawCallback: function () {");
            if (!isReadOnly)
            {
                sb.AppendLine("if ($('#Grid" + name + " tbody .dataTables_empty').length) {");
                if (multiple)
                {
                    sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", false);");
                }
                sb.AppendLine("$('#btnDelete" + name + "').hide();");
                sb.AppendLine("}");
                sb.AppendLine("else {");
                sb.AppendLine("$('[data-toggle=\"tooltip\"]').tooltip({");
                sb.AppendLine("trigger: 'hover'");
                sb.AppendLine("});");
                sb.AppendLine("$('#btnDelete" + name + "').show();");
                sb.AppendLine("}");
            }
            else
            {
                sb.AppendLine("$('[data-toggle=\"tooltip\"]').tooltip({");
                sb.AppendLine("trigger: 'hover'");
                sb.AppendLine("});");
            }
            sb.AppendLine("}");
            sb.AppendLine("});");
            if (multiple && !isReadOnly)
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
                sb.AppendLine("});");
                sb.AppendLine("$(\"#selectAllGrid" + name + "\").change(function () {");
                sb.AppendLine("if (this.checked) {");
                sb.AppendLine("Grid" + name + ".rows().select();");
                sb.AppendLine("}");
                sb.AppendLine("else {");
                sb.AppendLine("Grid" + name + ".rows().deselect();");
                sb.AppendLine("}");
                sb.AppendLine("});");
                sb.AppendLine("Grid" + name + ".on('select', function (e, dt, type, indexes) {");
                sb.AppendLine("var allRows = Grid" + name + ".rows().data().count();");
                sb.AppendLine("var selectedRows = Grid" + name + ".rows({ selected: true }).data().count();");
                sb.AppendLine("if (allRows == selectedRows) {");
                sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", true);");
                sb.AppendLine("}");
                sb.AppendLine("})");
                sb.AppendLine(".on('deselect', function (e, dt, type, indexes) {");
                sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", false);");
                sb.AppendLine("});");
            }
            sb.AppendLine("}");
            #endregion

            #region ready
            sb.AppendLine("$(document).ready(function () {");
            sb.AppendLine("bindGrid" + name + "();");
            if (!isReadOnly)
            {
                sb.AppendLine("$(\"#f" + name + "\").change(OnChange_f" + name + ");");
                sb.AppendLine("$(\"#btnAdd" + name + "\").click(OnClick_btnAdd" + name + ");");
                sb.AppendLine("$(\"#btnDelete" + name + "\").confirm({");
                sb.AppendLine("title: null,");
                sb.Append("content: '");
                sb.Append(Translation.CenterLang.Center.ConfirmDelete);
                sb.AppendLine("',");
                sb.AppendLine("columnClass: 'medium',");
                sb.AppendLine("buttons: {");
                sb.AppendLine("confirm: {");
                sb.Append("text: '");
                sb.Append(Translation.CenterLang.Center.OK);
                sb.AppendLine("',");
                sb.AppendLine("btnClass: \"btn-primary\",");
                sb.AppendLine("action: function () {");
                sb.AppendLine("if ($(\"#validError" + name + "\").length > 0) {");
                sb.AppendLine("$(\"#validError" + name + "\").remove();");
                sb.AppendLine("}");
                if (multiple)
                {
                    sb.AppendLine("var data = Grid" + name + ".rows({ selected: true }).data();");
                    sb.AppendLine("if (data.length > 0) {");
                    sb.AppendLine("Grid" + name + ".rows({ selected: true }).remove().draw();");
                    sb.AppendLine("}");
                    sb.AppendLine("else {");
                    sb.Append("$(\"<span id =\\\"validError" + name + "\\\" class=\\\"text-danger\\\">");
                    sb.Append(Translation.CenterLang.Center.PleaseSelectData);
                    sb.AppendLine("</span>\").insertBefore(\"#f" + name + "\");");
                    sb.AppendLine("}");
                }
                else
                {
                    sb.AppendLine("Grid" + name + ".rows().remove().draw();");
                }
                if (!onChangeName.IsNullOrEmpty())
                {
                    sb.Append(onChangeName);
                    sb.AppendLine("();");
                }
                sb.AppendLine("}");
                sb.AppendLine("},");
                sb.AppendLine("cancel: {");
                sb.Append("text: '");
                sb.Append(Translation.CenterLang.Center.Cancel);
                sb.AppendLine("',");
                sb.AppendLine("btnClass: \"btn-primary\"");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("});");
            }
            sb.AppendLine("});");
            #endregion

            sb.AppendLine("</script>");
            return MvcHtmlString.Create(sb.ToString());
        }

        /*----------------------------------------------------------------------------------------------------------*/
        //public static MvcHtmlString GetVSMFileUploadExcelFor<TModel, TValue>(this HtmlHelper<TModel> html,
        //    Expression<Func<TModel, TValue>> expression, bool isRequired = false, int labelWidth = 4, bool showLabel = true, string customType = "", bool isReadOnly = false, string prgCode = "")
        //{
        //    ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
        //    string expressionText = ExpressionHelper.GetExpressionText(expression);
        //    string label = GetControlLabel(metadata, expressionText);
        //    string name = GetControlName(html, expressionText);
        //    var sysConfig = new SystemConfigFile();

        //    var allowFile = ".xlsx";
        //    var sb = new StringBuilder();
        //    if (showLabel)
        //    {
        //        sb.Append("<div class=\"form-group");
        //        if (isRequired)
        //        {
        //            sb.Append(" required\" aria-required=\"true\"");
        //        }
        //        else
        //        {
        //            sb.AppendLine("\"");
        //        }
        //        sb.AppendLine(">");
        //        sb.AppendLine("<label class=\"control-label col-md-" + labelWidth + "\">");
        //        sb.Append(label);
        //        sb.Append("</label>");
        //    }
        //    else
        //    {
        //        labelWidth = 0;
        //        sb.AppendLine("<div class=\"row\">");
        //    }
        //    sb.AppendLine("<div class=\"col-md-" + (12 - labelWidth) + "\">");
        //    if (!isReadOnly)
        //    {
        //        sb.Append("<input type=\"file\" id=\"f" + name + "\" name=\"" + name + "\" style=\"display:none;\" data-vsmctrl=\"true\"");
        //        if (!allowFile.IsNullOrEmpty())
        //        {
        //            sb.Append(" accept=\"" + allowFile + "\"");
        //        }

        //        sb.Append(" data-isrequired=\"" + isRequired.ToString().ToLower() + "\"");
        //        sb.AppendLine("/>");
        //    }
        //    sb.AppendLine("<div class=\"gridIncludeToolbar\">");
        //    if (!isReadOnly)
        //    {
        //        sb.AppendLine("<div class=\"gridToolbar\">");
        //        sb.Append("<a href=\"javascript:void(0)\" id=\"btnAdd" + name + "\" data-toggle=\"tooltip\"");
        //        sb.Append(" title=\"" + Translation.CenterLang.Center.AddFile + "\"");
        //        sb.AppendLine(">");
        //        sb.AppendLine("<i class=\"ace-icon fa fa-plus-circle green bigger-125\"></i>");
        //        sb.AppendLine("</a>");
        //        sb.Append("<a href=\"javascript:void(0)\" id=\"btnDelete" + name + "\" data-toggle=\"tooltip\"");
        //        sb.Append(" title=\"" + Translation.CenterLang.Center.DeleteFile + "\"");
        //        sb.AppendLine(">");
        //        sb.AppendLine("<i class=\"ace-icon fa fa-trash red bigger-125\"></i>");
        //        sb.AppendLine("</a>");
        //        sb.Append("<a href=\"javascript:void(0)\" id=\"btnValidate" + name + "\" data-toggle=\"tooltip\"");
        //        sb.Append(" title=\"" + "test" + "\"");
        //        sb.AppendLine(">");
        //        sb.AppendLine("<i class=\"ace-icon fa fa-check bigger-125\"></i>");
        //        sb.AppendLine("</a>");
        //        if (!allowFile.IsNullOrEmpty())
        //        {
        //            sb.Append("<span class=\"help-block\">");
        //            sb.Append(Translation.CenterLang.Center.SupportFileType.Replace("{tplFileType}", ".xlsx"));
        //            sb.AppendLine("</span>");
        //        }
        //        sb.AppendLine("</div>");
        //    }
        //    sb.AppendLine("<table id=\"Grid" + name + "\" class=\"table table-striped table-bordered table-hover\" data-tabletype=\"fileupload\"></table>");
        //    sb.AppendLine("</div>");
        //    sb.AppendLine("</div>");
        //    sb.AppendLine("</div>");
        //    return MvcHtmlString.Create(sb.ToString());
        //}

        //public static MvcHtmlString GetVSMFileUploadExcelScriptFor<TModel, TValue>(this HtmlHelper<TModel> html,
        //    Expression<Func<TModel, TValue>> expression, string PRG_CODE = "", string formName = "form1")
        //{
        //    var sb = new StringBuilder();

        //    ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
        //    string expressionText = ExpressionHelper.GetExpressionText(expression);
        //    string label = GetControlLabel(metadata, expressionText);
        //    string name = GetControlName(html, expressionText);
        //    var url = new UrlHelper(html.ViewContext.RequestContext);

        //    sb.AppendLine("<script type=\"text/javascript\"> ");

        //    #region bindGrid
        //    sb.AppendLine("var Grid" + name + " = {};");
        //    sb.AppendLine("function bindGrid" + name + "(result) {");

        //    sb.AppendLine("var option = {");
        //    sb.AppendLine("\"deferRender\": true,");

        //    sb.AppendLine("\"processing\": true,");
        //    sb.AppendLine("\"destroy\": !$.isEmptyObject(Grid" + name + "),");
        //    sb.AppendLine("\"searching\": false,");
        //    sb.AppendLine("\"scrollX\": false,");

        //    sb.Append("\"dom\": \"");


        //    sb.Append("<'table-responsive'tr>");

        //    sb.Append("<'row'<'col-md-5'");

        //    sb.Append("<'pull-left'l><'pull-left'i>>");

        //    sb.Append("<'col-md-7'p>");
        //    sb.AppendLine(">\",");
        //    sb.AppendLine("\"language\": {");
        //    sb.Append("\"lengthMenu\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridLengthMenu);
        //    sb.AppendLine("\",");
        //    sb.Append("\"emptyTable\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridEmptyTable);
        //    sb.AppendLine("\",");
        //    sb.Append("\"info\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridInfo);
        //    sb.AppendLine("\",");
        //    sb.Append("\"infoEmpty\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridInfoEmpty);
        //    sb.AppendLine("\",");

        //    sb.Append("\"loadingRecords\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridLoadingRecords);
        //    sb.AppendLine("\",");
        //    sb.Append("\"processing\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridProcessing);
        //    sb.AppendLine("\",");

        //    sb.AppendLine("\"paginate\": {");
        //    sb.Append("\"first\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridPaginateFirst);
        //    sb.AppendLine("\",");
        //    sb.Append("\"last\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridPaginateLast);
        //    sb.AppendLine("\",");
        //    sb.Append("\"next\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridPaginateNext);
        //    sb.AppendLine("\",");
        //    sb.Append("\"previous\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridPaginatePrevious);
        //    sb.AppendLine("\"");
        //    sb.AppendLine("},");
        //    sb.AppendLine("},");


        //    int startIdx = 0;
        //    //var col = columns.Where(m => m != null && m.visible != "false").ToList();


        //    sb.AppendLine("\"columns\": [");

        //    bool incStartIdx = true;
        //    int c = 0;

        //    #region get col
        //    DataAccess.EXC001.EXC001DA da = new DataAccess.EXC001.EXC001DA();
        //    da.DTO.Execute.ExecuteType = DataAccess.EXC001.EXC001ExecuteType.GetQueryAllVal;
        //    da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;
        //    da.DTO.Model.PRG_CODE = PRG_CODE;
        //    da.Select(da.DTO);
        //    #endregion

        //    foreach (var dtC in da.DTO.Models)
        //    {
        //        if (startIdx > 0)
        //        {
        //            sb.AppendLine(",");
        //        }
        //        if (incStartIdx)
        //        {
        //            incStartIdx = false;
        //            startIdx++;
        //        }
        //        sb.AppendLine("{");

        //        sb.Append("\"" + "title" + "\":\"" + dtC.COL_NAME + "\"");
        //        sb.Append(",");
        //        sb.Append("\"data\":\"" + dtC.COL_NAME + "\"");
        //        sb.Append(",");
        //        sb.Append("\"render\": function (data, type, full, meta) {");
        //        sb.Append("var tag = '';");
        //        sb.Append("if(data != null) { ");
        //        sb.Append("tag += '<span class=\"\">'+data+'</span>'");
        //        sb.Append("}");
        //        sb.Append("return tag;");
        //        sb.Append("}");
        //        sb.Append(",");
        //        sb.Append("\"className\": \"highlight\"");

        //        sb.Append("}");
        //        c++;
        //    }

        //    sb.Append("]");
        //    sb.AppendLine(",");

        //    sb.Append("\"order\": [],");
        //    sb.AppendLine("drawCallback: function () {");
        //    sb.AppendLine("if ($('#Grid" + name + " tbody .dataTables_empty').length) {");
        //    sb.AppendLine("$('#Grid" + name + "_wrapper').find('.dt-buttons').addClass('hidden');");
        //    sb.AppendLine("}");
        //    sb.AppendLine("else {");
        //    sb.AppendLine("$('#Grid" + name + "_wrapper').find('.dt-buttons').removeClass('hidden');");
        //    sb.AppendLine("}");
        //    sb.AppendLine("$('[data-toggle=\"tooltip\"]').tooltip({");
        //    sb.AppendLine("trigger: 'hover'");
        //    sb.AppendLine("});");

        //    sb.AppendLine("}");

        //    sb.AppendLine(",rowCallback: function(row, data, index){");
        //    sb.AppendLine("if(data.ERR_COL != null) {");
        //    sb.AppendLine("var colError = data.ERR_COL - 1;");
        //    sb.AppendLine("var test = 2;");
        //    sb.AppendLine("$(row).find('td:eq('+colError+')').css('background-color', '#ff9999');");
        //    sb.AppendLine("}");
        //    sb.AppendLine("}");


        //    sb.AppendLine("};");

        //    sb.AppendLine("if (result.mode != \"standby\") {");
        //    sb.AppendLine("$(\"#rowGrid" + name + "\").removeClass(\"hidden\");");

        //    sb.AppendLine("option[\"ajax\"]= {");
        //    //sb.Append("url: \"");

        //    sb.AppendLine("url: \"" + url.Action("GetDataValidateExcel", "ValidateExcel", new
        //    {
        //        Area = "Ux"
        //    }) + "\",");
        //    //sb.Append(config.GetConfig.Url);
        //    sb.AppendLine("type: \"post\",");
        //    sb.AppendLine("error: OnAjaxError,");
        //    sb.AppendLine("data: result");
        //    sb.AppendLine("};");

        //    sb.AppendLine("}");
        //    sb.AppendLine("Grid" + name + " = $(\"#Grid" + name + "\").DataTable(option);");

        //    sb.AppendLine("}");
        //    #endregion

        //    #region bindGrid SUM
        //    sb.AppendLine("var GridSum" + name + " = {};");
        //    sb.AppendLine("function bindGridSum" + name + "(result) {");

        //    sb.AppendLine("var option = {");
        //    sb.AppendLine("\"deferRender\": true,");

        //    sb.AppendLine("\"processing\": true,");
        //    sb.AppendLine("\"destroy\": !$.isEmptyObject(GridSum" + name + "),");
        //    sb.AppendLine("\"searching\": false,");
        //    sb.AppendLine("\"scrollX\": false,");

        //    sb.Append("\"dom\": \"");


        //    sb.Append("<'table-responsive'tr>");

        //    sb.Append("<'row'<'col-md-5'");

        //    sb.Append("<'pull-left'l><'pull-left'i>>");

        //    sb.Append("<'col-md-7'p>");
        //    sb.AppendLine(">\",");
        //    sb.AppendLine("\"language\": {");
        //    sb.Append("\"lengthMenu\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridLengthMenu);
        //    sb.AppendLine("\",");
        //    sb.Append("\"emptyTable\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridEmptyTable);
        //    sb.AppendLine("\",");
        //    sb.Append("\"info\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridInfo);
        //    sb.AppendLine("\",");
        //    sb.Append("\"infoEmpty\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridInfoEmpty);
        //    sb.AppendLine("\",");

        //    sb.Append("\"loadingRecords\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridLoadingRecords);
        //    sb.AppendLine("\",");
        //    sb.Append("\"processing\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridProcessing);
        //    sb.AppendLine("\",");

        //    sb.AppendLine("\"paginate\": {");
        //    sb.Append("\"first\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridPaginateFirst);
        //    sb.AppendLine("\",");
        //    sb.Append("\"last\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridPaginateLast);
        //    sb.AppendLine("\",");
        //    sb.Append("\"next\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridPaginateNext);
        //    sb.AppendLine("\",");
        //    sb.Append("\"previous\": \"");
        //    sb.Append(Translation.CenterLang.Center.gridPaginatePrevious);
        //    sb.AppendLine("\"");
        //    sb.AppendLine("},");
        //    sb.AppendLine("},");


        //    startIdx = 0;
        //    //var col = columns.Where(m => m != null && m.visible != "false").ToList();


        //    sb.AppendLine("\"columns\": [");

        //    incStartIdx = true;
        //    c = 0;

        //    #region get col
        //    da = new DataAccess.EXC001.EXC001DA();
        //    da.DTO.Execute.ExecuteType = DataAccess.EXC001.EXC001ExecuteType.GetQueryAllVal;
        //    da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;
        //    da.DTO.Model.PRG_CODE = PRG_CODE;
        //    da.Select(da.DTO);

        //    var ModelSum = da.DTO.Models.Where(m => (m.COL_TYPE == 2 || m.COL_TYPE == 3)).ToList();
        //    #endregion

        //    foreach (var dtC in ModelSum)
        //    {
        //        if (startIdx > 0)
        //        {
        //            sb.AppendLine(",");
        //        }
        //        if (incStartIdx)
        //        {
        //            incStartIdx = false;
        //            startIdx++;
        //        }
        //        sb.AppendLine("{");

        //        sb.Append("\"" + "title" + "\":\"" + dtC.COL_NAME + "\"");
        //        sb.Append(",");
        //        sb.Append("\"data\":\"" + dtC.COL_NAME + "\"");
        //        sb.Append(",");
        //        sb.Append("\"render\": function (data, type, full, meta) {");
        //        sb.Append("var tag = '';");
        //        sb.Append("if(data != null) { ");
        //        sb.Append("tag += '<span class=\"\">'+data+'</span>'");
        //        sb.Append("}");
        //        sb.Append("return tag;");
        //        sb.Append("}");
        //        sb.Append(",");
        //        sb.Append("\"className\": \"highlight\"");

        //        sb.Append("}");
        //        c++;
        //    }

        //    sb.Append("]");
        //    sb.AppendLine(",");

        //    sb.Append("\"order\": [],");
        //    sb.AppendLine("drawCallback: function () {");
        //    sb.AppendLine("if ($('#GridSum" + name + " tbody .dataTables_empty').length) {");
        //    sb.AppendLine("$('#GridSum" + name + "_wrapper').find('.dt-buttons').addClass('hidden');");
        //    sb.AppendLine("}");
        //    sb.AppendLine("else {");
        //    sb.AppendLine("$('#GridSum" + name + "_wrapper').find('.dt-buttons').removeClass('hidden');");
        //    sb.AppendLine("}");
        //    sb.AppendLine("$('[data-toggle=\"tooltip\"]').tooltip({");
        //    sb.AppendLine("trigger: 'hover'");
        //    sb.AppendLine("});");

        //    sb.AppendLine("}");

        //    //sb.AppendLine(",rowCallback: function(row, data, index){");
        //    //sb.AppendLine("if(data.ERR_COL != null) {");
        //    //sb.AppendLine("var colError = data.ERR_COL - 1;");
        //    //sb.AppendLine("var test = 2;");
        //    //sb.AppendLine("$(row).find('td:eq('+colError+')').css('background-color', '#ff9999');");
        //    //sb.AppendLine("}");
        //    //sb.AppendLine("}");

        //    sb.AppendLine("};");

        //    sb.AppendLine("if (result.mode != \"standby\") {");
        //    sb.AppendLine("$(\"#rowGridSum" + name + "\").removeClass(\"hidden\");");

        //    sb.AppendLine("option[\"ajax\"]= {");
        //    //sb.Append("url: \"");

        //    sb.AppendLine("url: \"" + url.Action("GetDataSumValidateExcel", "ValidateExcel", new
        //    {
        //        Area = "Ux"
        //    }) + "\",");
        //    //sb.Append(config.GetConfig.Url);
        //    sb.AppendLine("type: \"post\",");
        //    sb.AppendLine("error: OnAjaxError,");
        //    sb.AppendLine("data: result");
        //    sb.AppendLine("};");

        //    sb.AppendLine("}");

        //    if (ModelSum.Count() > 0)
        //    {
        //        sb.AppendLine("GridSum" + name + " = $(\"#GridSum" + name + "\").DataTable(option);");
        //    }
        //    sb.AppendLine("}");
        //    #endregion

        //    #region OnChange Checkbox
        //    sb.AppendLine("$('#ValViewError ,#ValViewCom').change(function () {");
        //    sb.AppendLine("var E = 'N'; ");
        //    sb.AppendLine("var C = 'N'; ");
        //    sb.AppendLine("if ($('#ValViewError').is(\":checked\")) {");
        //    sb.AppendLine(" E = \"Y\"; ");
        //    sb.AppendLine("}");
        //    sb.AppendLine("if ($('#ValViewCom').is(\":checked\")) {");
        //    sb.AppendLine("C = \"Y\"; ");
        //    sb.AppendLine("}");
        //    sb.AppendLine("bindGridEXCEL_UPLOAD({ pE: E, pC: C });");
        //    if (ModelSum.Count() > 0)
        //    {
        //        sb.AppendLine("bindGridEXCEL_UPLOAD({ pE: E, pC: C });");
        //    }
        //    sb.AppendLine("});");
        //    #endregion

        //    #region Temp Change File

        //    sb.AppendLine("var OnUploadFile = function () {");

        //    //sb.AppendLine("$('#EXCEL_UPLOAD').change(function () {");
        //    sb.AppendLine("var form = $(\"form[id=" + formName + "]\");");
        //    sb.AppendLine("var formData = form.serializeFormData();");
        //    sb.AppendLine("formData.append(\"PRG_CODE\", '" + PRG_CODE + "');");
        //    sb.AppendLine("if (form.valid()) {");
        //    sb.AppendLine("$.ajax({");
        //    sb.AppendLine("url: '" + url.Action("GetValidateExcel", "ValidateExcel", new
        //    {
        //        Area = "Ux"
        //    }) + "',");
        //    sb.AppendLine("type: 'POST',");
        //    sb.AppendLine("data: formData,");
        //    sb.AppendLine("contentType: false,");
        //    sb.AppendLine("processData: false,");
        //    sb.AppendLine("success: function (response) {");

        //    sb.AppendLine("$(\"#ValViewError\").prop('checked', true); ");
        //    sb.AppendLine("$(\"#ValViewCom\").prop('checked', false); ");

        //    sb.AppendLine("$(\"#ValFileName\").val(response.data.filename); ");
        //    sb.AppendLine("$(\"#ValRecordAll\").val(response.data.total); ");
        //    sb.AppendLine("$(\"#Valcomplete\").val(response.data.complete); ");
        //    sb.AppendLine("$(\"#ValError\").val(response.data.error); ");

        //    sb.AppendLine("bindGridEXCEL_UPLOAD({ pE: \"Y\" }); ");

        //    sb.AppendLine("$(\"#mdValExcel\").modal(); ");
        //    sb.AppendLine("},");
        //    sb.AppendLine("error: OnAjaxError");
        //    sb.AppendLine("});");
        //    sb.AppendLine("}");
        //    sb.AppendLine("return false; ");
        //    //sb.AppendLine("});");
        //    sb.AppendLine("}");
        //    #endregion

        //    #region ready
        //    sb.AppendLine("$(document).ready(function () {");
        //    sb.AppendLine("bindGrid" + name + "({mode:'standby'});");
        //    if (ModelSum.Count() > 0)
        //    {
        //        sb.AppendLine("bindGridSum" + name + "({mode:'standby'});");
        //    }

        //    //tab change
        //    sb.AppendLine("$('a[data-toggle=\"tab\"]').on('shown.bs.tab', function (e) { ");
        //    sb.AppendLine("var tab = $(e.target); ");
        //    sb.AppendLine("var actived = tab.data('actived'); ");
        //    sb.AppendLine("var tabId = e.target.id; ");
        //    sb.AppendLine("if (tabId == \"tabValDetail\" && !actived) { ");
        //    sb.AppendLine("var E = 'N'; ");
        //    sb.AppendLine("var C = 'N'; ");
        //    sb.AppendLine("if ($('#ValViewError').is(\":checked\")) {");
        //    sb.AppendLine(" E = \"Y\"; ");
        //    sb.AppendLine("}");
        //    sb.AppendLine("if ($('#ValViewCom').is(\":checked\")) {");
        //    sb.AppendLine("C = \"Y\"; ");
        //    sb.AppendLine("}");
        //    sb.AppendLine("bindGridEXCEL_UPLOAD({ pE: E, pC: C });");
        //    sb.AppendLine("tab.data('actived', true);");
        //    sb.AppendLine("}");
        //    sb.AppendLine("else if (tabId == \"tabValSummary\" && !actived) { ");
        //    if (ModelSum.Count() > 0)
        //    {
        //        sb.AppendLine("bindGridSumEXCEL_UPLOAD({ pE: E, pC: C });");
        //        sb.AppendLine("tab.data('actived', true); ");
        //    }
        //    sb.AppendLine("}");
        //    sb.AppendLine("});");

        //    sb.AppendLine("});");
        //    #endregion

        //    sb.AppendLine("</script>");

        //    return MvcHtmlString.Create(sb.ToString());
        //}
        /*----------------------------------------------------------------------------------------------------------*/

        public static MvcHtmlString GetVSMFileUploadExcelFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool isRequired = false, int labelWidth = 4, bool showLabel = true, string customType = ".xlsx", bool isReadOnly = false, string prgCode = "")
        {
            bool multiple = false;
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string label = GetControlLabel(metadata, expressionText);
            string name = GetControlName(html, expressionText);
            var sysConfig = /*html.ViewContext.HttpContext.Session[SessionSystemName.SYS_ConfigFile] as*/ new SystemConfigFile();
            //if (prgCode.IsNullOrEmpty())
            //{
            //    prgCode = html.ViewContext.HttpContext.Session[SessionSystemName.SYS_CurrentPRG_CODE].AsString();
            //}
            var allowFile = customType;/*AllowFile(sysConfig, customType, prgCode);*/
            var sb = new StringBuilder();
            if (showLabel)
            {
                sb.Append("<div class=\"form-group");
                if (isRequired)
                {
                    sb.Append(" required\" aria-required=\"true\"");
                }
                else
                {
                    sb.AppendLine("\"");
                }
                sb.AppendLine(">");
                sb.AppendLine("<label class=\"control-label col-md-" + labelWidth + "\">");
                sb.Append(label);
                sb.Append("</label>");
            }
            else
            {
                labelWidth = 0;
                sb.AppendLine("<div class=\"row\">");
            }
            sb.AppendLine("<div class=\"col-md-" + (12 - labelWidth) + "\">");
            if (!isReadOnly)
            {
                sb.Append("<input type=\"file\" id=\"f" + name + "\" name=\"" + name + "\" style=\"display:none;\" data-vsmctrl=\"true\"");
                if (!allowFile.IsNullOrEmpty())
                {
                    sb.Append(" accept=\"" + allowFile + "\"");
                }
                if (multiple)
                {
                    sb.Append(" data-multiple=\"true\" multiple");
                }

                sb.Append(" data-isrequired=\"" + isRequired.ToString().ToLower() + "\"");
                sb.AppendLine("/>");
            }
            sb.AppendLine("<div class=\"gridIncludeToolbar\">");
            if (!isReadOnly)
            {
                sb.AppendLine("<div class=\"gridToolbar\">");
                sb.Append("<a href=\"javascript:void(0)\" id=\"btnAdd" + name + "\" data-toggle=\"tooltip\"");
                sb.Append(" title=\"" + Translation.CenterLang.Center.AddFile + "\"");
                sb.AppendLine(">");
                sb.AppendLine("<i class=\"ace-icon fa fa-plus-circle green bigger-125\"></i>");
                sb.AppendLine("</a>");
                sb.Append("<a href=\"javascript:void(0)\" id=\"btnDelete" + name + "\" data-toggle=\"tooltip\"");
                sb.Append(" title=\"" + Translation.CenterLang.Center.DeleteFile + "\"");
                sb.AppendLine(">");
                sb.AppendLine("<i class=\"ace-icon fa fa-trash red bigger-125\"></i>");
                sb.AppendLine("</a>");
                if (!allowFile.IsNullOrEmpty())
                {
                    //var nSysConfig = sysConfig.Details;
                    //if (!prgCode.IsNullOrEmpty())
                    //{
                    //    var confByPrgCode = nSysConfig.Where(m => m.PRG_CODE == prgCode).ToList();
                    //    if (confByPrgCode != null && confByPrgCode.Count > 0)
                    //    {
                    //        nSysConfig = confByPrgCode;
                    //    }
                    //}
                    //if (!customType.IsNullOrEmpty())
                    //{
                    //    var ctTypes = customType.Replace(".", "").Split(',');
                    //    var conf = nSysConfig.Where(m => ctTypes.Contains(m.FILE_TYPE)).ToList();
                    //    if (conf.Count > 0)
                    //    {
                    //        nSysConfig = conf;
                    //    }
                    //}
                    var types = new List<string>();
                    string tplType = ".{0}";
                    //foreach (var item in nSysConfig.Distinct())
                    //{
                    types.Add(string.Format(tplType, customType.Replace(".", "")));
                    //}
                    var nType = string.Join(",", types);
                    sb.Append("<span class=\"help-block\">");
                    sb.Append(Translation.CenterLang.Center.SupportFileType.Replace("{tplFileType}", nType));
                    sb.AppendLine("</span>");
                }
                sb.AppendLine("</div>");
            }
            sb.AppendLine("<table id=\"Grid" + name + "\" class=\"table table-striped table-bordered table-hover\" data-tabletype=\"fileupload\"></table>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString GetVSMFileUploadExcelScriptFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string customType = ".xlsx", DefaultConfig config = null, string onChangeName = "", bool isReadOnly = false, DefaultConfig downloadConfig = null, string prgCode = "", string formName = "form1", string customsBtnUploadName = "LoadFile")
        {
            bool multiple = false;
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string label = GetControlLabel(metadata, expressionText);
            string name = GetControlName(html, expressionText);
            //var sysConfig = html.ViewContext.HttpContext.Session["SYS_ConfigFile"] as SystemConfigFile;
            //if (prgCode.IsNullOrEmpty())
            //{
            //    prgCode = html.ViewContext.HttpContext.Session[SessionSystemName.SYS_CurrentPRG_CODE].AsString();
            //}
            //var allowFile = AllowFile(sysConfig, customType, prgCode);
            var allowFile = customType;

            var sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\" > ");

            sb.AppendLine("var Grid" + name + " = {};");
            if (!isReadOnly)
            {
                #region OnClick_btnAdd
                sb.AppendLine("function OnClick_btnAdd" + name + "(e) {");
                sb.AppendLine("if ($(\"#validError" + name + "\").length > 0) {");
                sb.AppendLine("$(\"#validError" + name + "\").remove();");
                sb.AppendLine("}");
                sb.AppendLine("$(\"#f" + name + "\").click();");
                sb.AppendLine("return false;");
                sb.AppendLine("}");
                #endregion

                #region OnChange_f
                sb.AppendLine("function OnChange_f" + name + "(e) {");
                sb.AppendLine("if ($(\"#validError" + name + "\").length > 0) {");
                sb.AppendLine("$(\"#validError" + name + "\").remove();");
                sb.AppendLine("}");
                sb.AppendLine("var f = this;");
                sb.Append("if (f.files.length > 0) {");
                sb.Append("var allowFile = [");

                sb.Append("{");
                sb.Append("type:'");
                sb.Append(".xlsx".Replace(".", ""));
                sb.AppendLine("'");
                sb.Append("}");

                sb.AppendLine("];");

                sb.AppendLine("Grid" + name + ".rows().remove().draw();");
                sb.AppendLine("var info = {};");
                sb.AppendLine("$.each(f.files, function (i, file) {");
                sb.AppendLine("var ext = file.name.split('.').pop();");
                sb.AppendLine("var size = file.size / 1048576;");
                sb.AppendLine("var allowType = false;");
                sb.AppendLine("var allowSize = true;");
                sb.AppendLine("$.each(allowFile, function (tIdx, obj) {");
                sb.AppendLine("if (obj.type == ext) {");
                sb.AppendLine("allowType = true;");
                sb.AppendLine("if (size <= obj.size) {");
                sb.AppendLine("allowSize = true;");
                sb.AppendLine("}");
                sb.AppendLine("return false;");
                sb.AppendLine("}");
                sb.AppendLine("});");
                sb.AppendLine("info[\"FILE_NAME\"] = file.name;");
                sb.AppendLine("info[\"File\"] = file;");
                sb.AppendLine("info[\"FILE_SIZE\"] = size;");
                sb.AppendLine("info[\"FILE_DATE\"] = '/Date('+file.lastModifiedDate.getTime()+')/';");
                sb.AppendLine("if(allowType && allowSize){");
                sb.AppendLine("Grid" + name + ".row.add(info).draw();");
                sb.AppendLine("}");
                sb.AppendLine("else if (!allowType) {");
                sb.Append("$(\"<span id =\\\"validError" + name + "\\\" class=\\\"text-danger\\\">");
                sb.Append(Translation.CenterLang.Center.FileNotExistsSystemConfig);
                sb.AppendLine("</span>\").insertBefore(\"#f" + name + "\");");
                sb.AppendLine("}");
                //sb.AppendLine("else if (!allowSize) {");
                //sb.Append("$(\"<span id =\\\"validError" + name + "\\\" class=\\\"text-danger\\\">");
                //sb.Append(Translation.CenterLang.Center.FileMaximumSizeLimit);
                //sb.AppendLine("</span>\").insertBefore(\"#f" + name + "\");");
                //sb.AppendLine("}");
                sb.AppendLine("});");

                sb.AppendLine("var oldCtrl = $(\"#f" + name + "\");");
                sb.Append("var newCtrl = $('<input type=\"file\" id=\"f" + name + "\" name=\"" + name + "\" style=\"display:none;\" data-vsmctrl=\"true\"");
                if (!allowFile.IsNullOrEmpty())
                {
                    sb.Append(" accept=\"");
                    sb.Append(allowFile);
                    sb.Append("\"");
                }
                if (multiple)
                {
                    sb.Append(" data-multiple=\"true\" multiple");
                }
                sb.AppendLine("/>');");
                sb.AppendLine("var isrequired = oldCtrl.data(\"isrequired\");");
                sb.AppendLine("if(isrequired){");
                sb.AppendLine("newCtrl.data(\"isrequired\",isrequired);");
                sb.AppendLine("}");
                sb.AppendLine("newCtrl.change(OnChange_f" + name + ");");
                sb.AppendLine("oldCtrl.replaceWith(newCtrl);");
                if (!onChangeName.IsNullOrEmpty())
                {
                    sb.Append(onChangeName);
                    sb.AppendLine("();");
                }
                sb.AppendLine("}");
                sb.AppendLine("}");
                #endregion
            }
            #region bindGrid
            sb.AppendLine("function bindGrid" + name + "() {");

            if (config != null)
            {
                sb.AppendLine("var result = {};");
                if (config.Parameters != null && config.Parameters.Count > 0)
                {
                    for (int i = 0; i < config.Parameters.Count; i++)
                    {
                        if (config.Parameters[i].Type == VSMParameterType.ByModelData)
                        {
                            sb.AppendLine("result[\"" + config.Parameters[i].Name + "\"] =\"" + config.Parameters[i].Value + "\";");
                        }
                        else
                        {
                            sb.AppendLine("result[\"" + config.Parameters[i].Name + "\"] = $(\"#" + config.Parameters[i].Value + "\").val();");
                        }
                    }
                }
            }

            sb.AppendLine("Grid" + name + " = $(\"#Grid" + name + "\").DataTable({");
            sb.AppendLine("\"deferRender\": true,");
            sb.AppendLine("\"paging\": false,");
            sb.AppendLine("\"processing\": true,");
            sb.AppendLine("\"destroy\": !$.isEmptyObject(Grid" + name + "),");
            //sb.AppendLine("\"order\": [[1, \"asc\"]],");
            sb.AppendLine("\"info\": false,");
            sb.AppendLine("\"ordering\": false,");
            sb.AppendLine("\"searching\": false,");
            sb.AppendLine("\"lengthChange\": false,");
            sb.AppendLine("\"autoWidth\": false,");
            sb.AppendLine("\"scrollCollapse\": true,");
            sb.AppendLine("\"dom\": \"<'table-responsive'tr>\",");
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
            sb.Append("\"infoFiltered\": \"");
            sb.Append(Translation.CenterLang.Center.gridInfoFiltered);
            sb.AppendLine("\",");
            sb.Append("\"loadingRecords\": \"");
            sb.Append(Translation.CenterLang.Center.gridLoadingRecords);
            sb.AppendLine("\",");
            sb.Append("\"processing\": \"");
            sb.Append(Translation.CenterLang.Center.gridProcessing);
            sb.AppendLine("\",");
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

            var url = new UrlHelper(html.ViewContext.RequestContext);
            if (config != null)
            {
                sb.AppendLine("\"ajax\": {");
                sb.Append("url: \"");
                sb.Append(config.Url);
                sb.Append("\",");
                sb.AppendLine("type: \"GET\",");
                sb.AppendLine("data: result");
                //sb.AppendLine("\"dataSrc\": \"rows\"");            
                sb.AppendLine("},");
            }
            if (multiple && !isReadOnly)
            {
                sb.AppendLine("select: {");
                sb.AppendLine("style: 'multi',");
                sb.AppendLine("selector: 'td:first-child'");
                sb.AppendLine("},");
            }
            sb.AppendLine("\"columns\": [");
            if (multiple && !isReadOnly)
            {
                sb.AppendLine("{");
                sb.AppendLine("data: null,");
                sb.AppendLine("defaultContent: '',");
                sb.AppendLine("width: '2%',");
                sb.AppendLine("className: 'dt-head-center select-checkbox',");
                sb.AppendLine("orderable: false,");
                sb.AppendLine("title: '<input type=\"checkbox\" name=\"selectAllGrid" + name + "\" id=\"selectAllGrid" + name + "\" class=\"checkbox\">'");
                sb.AppendLine("},");
            }
            sb.AppendLine("{");
            sb.AppendLine("\"data\": \"ID\",");
            sb.AppendLine("\"orderable\": true,");
            sb.AppendLine("\"title\": \"\",");
            sb.AppendLine("\"width\": \"3%\",");
            sb.AppendLine("className: 'dt-head-center dt-head-nowrap',");
            sb.AppendLine("\"render\": function (data, type, full, meta) {");
            sb.AppendLine("var tag = '';");
            sb.AppendLine("if (data != null && data != '') {");
            sb.Append("tag += '<a data-toggle=\"tooltip\" data-placement=\"right\" target=\"_blank\" title=\"");
            sb.Append(Translation.CenterLang.Center.Download);
            sb.Append("\" href=\\\"");
            if (downloadConfig != null)
            {
                sb.Append(downloadConfig.Url);
            }
            else
            {
                sb.Append(url.Action("Download", "File", new { Area = "Ux" }));
            }
            sb.AppendLine("?ID=' + toUrlString(data);");
            sb.AppendLine("tag += '\\\" ><i class=\\\"ace-icon fa fa-download bigger-130\\\"></i></a>';");
            sb.AppendLine("}");
            sb.AppendLine("return tag;");
            sb.AppendLine("}");
            sb.AppendLine("},");
            sb.AppendLine("{");
            sb.AppendLine("\"data\": \"FILE_NAME\",");
            sb.Append("\"title\": \"");
            sb.Append(Translation.CenterLang.Center.FileName);
            sb.AppendLine("\",");
            sb.AppendLine("className: 'dt-head-center dt-head-nowrap',");
            sb.AppendLine("\"width\": \"59%\"");
            sb.AppendLine("},");
            sb.AppendLine("{");
            sb.AppendLine("\"data\": \"FILE_SIZE\",");
            sb.Append("\"title\": \"");
            sb.Append(Translation.CenterLang.Center.FileSize);
            sb.AppendLine("\",");
            sb.AppendLine("\"width\": \"9%\",");
            sb.AppendLine("className: 'dt-head-center dt-head-nowrap dt-body-right',");
            sb.AppendLine("\"render\": function (data, type, full, meta) {");
            sb.AppendLine("var nData = toNumberFormat(data, \"#,###,##0.####\");");
            sb.AppendLine("return (data != undefined && data != null) ? nData + \" MB\" : \"\";");
            sb.AppendLine("}");
            sb.AppendLine("},");
            sb.AppendLine("{");
            sb.AppendLine("\"data\": \"FILE_DATE\",");
            sb.Append("\"title\": \"");
            sb.Append(Translation.CenterLang.Center.FileDateModified);
            sb.AppendLine("\",");
            sb.AppendLine("\"width\": \"15%\",");
            sb.AppendLine("className: 'dt-head-center dt-head-nowrap dt-body-center',");
            sb.AppendLine("\"render\": function (data, type, full, meta) {");
            sb.Append("var nData ='';");
            sb.AppendLine("if(data!=null&&data!=''){");
            sb.AppendLine("nData = jsonDateToFormat(data, \"DD/MM/YYYY HH:mm:ss\");");
            sb.AppendLine("}");
            sb.AppendLine("return nData;");
            sb.AppendLine("}");
            sb.AppendLine("},");
            sb.AppendLine("{");
            sb.AppendLine("\"data\": \"CRET_DATE\",");
            sb.Append("\"title\": \"");
            sb.Append(Translation.CenterLang.Center.FileUploadDate);
            sb.AppendLine("\",");
            sb.AppendLine("\"width\": \"15%\",");
            sb.AppendLine("className: 'dt-head-center dt-head-nowrap dt-body-center',");
            sb.AppendLine("\"render\": function (data, type, full, meta) {");
            sb.Append("var nData ='<span class=\"text-danger\">");
            sb.Append(Translation.CenterLang.Center.FileWaitingUpload);
            sb.AppendLine("</span>';");
            sb.AppendLine("if(data!=null&&data!=''){");
            sb.AppendLine("nData = jsonDateToFormat(data, \"DD/MM/YYYY HH:mm:ss\");");
            sb.AppendLine("}");
            sb.AppendLine("return nData;");
            sb.AppendLine("}");
            sb.AppendLine("}],");
            sb.AppendLine("drawCallback: function () {");
            if (!isReadOnly)
            {
                sb.AppendLine("if ($('#Grid" + name + " tbody .dataTables_empty').length) {");
                if (multiple)
                {
                    sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", false);");
                }
                sb.AppendLine("$('#btnDelete" + name + "').hide();");
                sb.AppendLine("}");
                sb.AppendLine("else {");
                sb.AppendLine("$('[data-toggle=\"tooltip\"]').tooltip({");
                sb.AppendLine("trigger: 'hover'");
                sb.AppendLine("});");
                sb.AppendLine("$('#btnDelete" + name + "').show();");
                sb.AppendLine("}");
            }
            else
            {
                sb.AppendLine("$('[data-toggle=\"tooltip\"]').tooltip({");
                sb.AppendLine("trigger: 'hover'");
                sb.AppendLine("});");
            }
            sb.AppendLine("}");
            sb.AppendLine("});");
            if (multiple && !isReadOnly)
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
                sb.AppendLine("});");
                sb.AppendLine("$(\"#selectAllGrid" + name + "\").change(function () {");
                sb.AppendLine("if (this.checked) {");
                sb.AppendLine("Grid" + name + ".rows().select();");
                sb.AppendLine("}");
                sb.AppendLine("else {");
                sb.AppendLine("Grid" + name + ".rows().deselect();");
                sb.AppendLine("}");
                sb.AppendLine("});");
                sb.AppendLine("Grid" + name + ".on('select', function (e, dt, type, indexes) {");
                sb.AppendLine("var allRows = Grid" + name + ".rows().data().count();");
                sb.AppendLine("var selectedRows = Grid" + name + ".rows({ selected: true }).data().count();");
                sb.AppendLine("if (allRows == selectedRows) {");
                sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", true);");
                sb.AppendLine("}");
                sb.AppendLine("})");
                sb.AppendLine(".on('deselect', function (e, dt, type, indexes) {");
                sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", false);");
                sb.AppendLine("});");
            }
            sb.AppendLine("}");
            #endregion

            #region bindGrid in modals
            #region bindGrid
            sb.AppendLine("var GridVal" + name + " = {};");
            sb.AppendLine("function bindGridVal" + name + "(result) {");

            sb.AppendLine("var option = {");
            sb.AppendLine("\"deferRender\": true,");

            sb.AppendLine("\"processing\": true,");
            sb.AppendLine("\"destroy\": !$.isEmptyObject(GridVal" + name + "),");
            sb.AppendLine("\"searching\": false,");
            sb.AppendLine("\"scrollX\": false,");

            sb.Append("\"dom\": \"");


            sb.Append("<'table-responsive'tr>");

            sb.Append("<'row'<'col-md-5'");

            sb.Append("<'pull-left'l><'pull-left'i>>");

            sb.Append("<'col-md-7'p>");
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

            sb.Append("\"loadingRecords\": \"");
            sb.Append(Translation.CenterLang.Center.gridLoadingRecords);
            sb.AppendLine("\",");
            sb.Append("\"processing\": \"");
            sb.Append(Translation.CenterLang.Center.gridProcessing);
            sb.AppendLine("\",");

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


            int startIdx = 0;
            //var col = columns.Where(m => m != null && m.visible != "false").ToList();


            sb.AppendLine("\"columns\": [");

            bool incStartIdx = true;
            int c = 0;

            #region get col
            DataAccess.EXC001.EXC001DA da = new DataAccess.EXC001.EXC001DA();
            da.DTO.Execute.ExecuteType = DataAccess.EXC001.EXC001ExecuteType.GetQueryAllVal;
            da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;
            da.DTO.Model.PRG_CODE = prgCode;
            da.Select(da.DTO);
            #endregion

            foreach (var dtC in da.DTO.Models)
            {
                if (startIdx > 0)
                {
                    sb.AppendLine(",");
                }
                if (incStartIdx)
                {
                    incStartIdx = false;
                    startIdx++;
                }
                sb.AppendLine("{");

                sb.Append("\"" + "title" + "\":\"" + dtC.COL_NAME + "\"");
                sb.Append(",");
                if (dtC.DB_COLUMN == null)
                {
                    sb.Append("\"data\":\"" + dtC.COL_NAME.ToUpper() + "\"");
                }
                else
                {
                    sb.Append("\"data\":\"" + dtC.DB_COLUMN.ToUpper() + "\"");
                }
                sb.Append(",");
                if (dtC.COL_TYPE == 2)
                {//int
                    sb.AppendLine("\"render\": function (data, type, full, meta) {");
                    sb.AppendLine("return digitsFormat(toNumberFormat(data),0);");
                    sb.AppendLine("},");
                    sb.AppendLine("\"className\" : \"" + ColumnsTextAlign.Right.GetDescription() + "\",");
                }
                else if (dtC.COL_TYPE == 3)
                {//decimal
                    if (dtC.MAX_LENGTH.AsString().IndexOf(',') > -1)
                    {
                        string digit = dtC.MAX_LENGTH.Split(',')[1];
                        sb.AppendLine("\"render\": function (data, type, full, meta) {");
                        sb.AppendLine("return digitsFormat(toNumberFormat(data)," + digit + ");");
                        sb.AppendLine("},");
                        sb.AppendLine("\"className\" : \"" + ColumnsTextAlign.Right.GetDescription() + "\",");
                    }
                    else
                    {
                        sb.AppendLine("\"render\": function (data, type, full, meta) {");
                        sb.AppendLine("return toNumberFormat(data);");
                        sb.AppendLine("},");
                        sb.AppendLine("\"className\" : \"" + ColumnsTextAlign.Right.GetDescription() + "\",");
                    }
                }
                else if (dtC.COL_TYPE == 4)
                {//date
                    sb.AppendLine("\"className\" : \"" + ColumnsTextAlign.Center.GetDescription() + "\",");
                }

                sb.Append("}");
                c++;
            }

            sb.Append("]");
            sb.AppendLine(",");

            sb.Append("\"order\": [],");
            sb.AppendLine("drawCallback: function () {");
            sb.AppendLine("if ($('#GridVal" + name + " tbody .dataTables_empty').length) {");
            sb.AppendLine("$('#GridVal" + name + "_wrapper').find('.dt-buttons').addClass('hidden');");
            sb.AppendLine("}");
            sb.AppendLine("else {");
            sb.AppendLine("$('#GridVal" + name + "_wrapper').find('.dt-buttons').removeClass('hidden');");
            sb.AppendLine("}");
            sb.AppendLine("$('[data-toggle=\"tooltip\"]').tooltip({");
            sb.AppendLine("trigger: 'hover'");
            sb.AppendLine("});");

            sb.AppendLine("}");

            sb.AppendLine(",rowCallback: function(row, data, index){");
            sb.AppendLine("if(data.ERR_COL != null) {");
            sb.AppendLine("if(data.ERR_COL.indexOf(',') > -1) {");
            sb.AppendLine("var ERR_COLs = data.ERR_COL.split(\",\");");
            sb.AppendLine("$.each(ERR_COLs, function (i, item) {");
            sb.AppendLine("var colError = item - 1;");
            sb.AppendLine("$(row).find('td:eq('+colError+')').css('background-color', '#ff9999');");
            sb.AppendLine("});");
            sb.AppendLine("}");
            sb.AppendLine("else {");
            sb.AppendLine("var colError = data.ERR_COL - 1;");
            sb.AppendLine("$(row).find('td:eq('+colError+')').css('background-color', '#ff9999');");
            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine("}");


            sb.AppendLine("};");

            sb.AppendLine("if (result.mode != \"standby\") {");
            sb.AppendLine("$(\"#rowGridVal" + name + "\").removeClass(\"hidden\");");

            sb.AppendLine("option[\"ajax\"]= {");
            //sb.Append("url: \"");

            sb.AppendLine("url: \"" + url.Action("GetDataValidateExcel", "ValidateExcel", new
            {
                Area = "Ux"
            }) + "\",");
            //sb.Append(config.GetConfig.Url);
            sb.AppendLine("type: \"post\",");
            sb.AppendLine("error: OnAjaxError,");
            sb.AppendLine("data: result");
            sb.AppendLine("};");

            sb.AppendLine("}");
            sb.AppendLine("GridVal" + name + " = $(\"#GridVal" + name + "\").DataTable(option);");

            sb.AppendLine("}");
            #endregion

            #region bindGrid SUM
            sb.AppendLine("var GridValSum" + name + " = {};");
            sb.AppendLine("function bindGridValSum" + name + "(result) {");

            sb.AppendLine("var option = {");
            sb.AppendLine("\"deferRender\": true,");

            sb.AppendLine("\"processing\": true,");
            sb.AppendLine("\"destroy\": !$.isEmptyObject(GridValSum" + name + "),");
            sb.AppendLine("\"searching\": false,");
            sb.AppendLine("\"scrollX\": false,");

            sb.Append("\"dom\": \"");


            sb.Append("<'table-responsive'tr>");

            sb.Append("<'row'<'col-md-5'");

            sb.Append("<'pull-left'l><'pull-left'i>>");

            sb.Append("<'col-md-7'p>");
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

            sb.Append("\"loadingRecords\": \"");
            sb.Append(Translation.CenterLang.Center.gridLoadingRecords);
            sb.AppendLine("\",");
            sb.Append("\"processing\": \"");
            sb.Append(Translation.CenterLang.Center.gridProcessing);
            sb.AppendLine("\",");

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


            startIdx = 0;
            sb.AppendLine("\"columns\": [");

            incStartIdx = true;
            c = 0;

            #region get col
            da = new DataAccess.EXC001.EXC001DA();
            da.DTO.Execute.ExecuteType = DataAccess.EXC001.EXC001ExecuteType.GetQueryAllVal;
            da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;
            da.DTO.Model.PRG_CODE = prgCode;
            da.Select(da.DTO);

            var ModelSum = da.DTO.Models.Where(m => (m.COL_TYPE == 2 || m.COL_TYPE == 3)).ToList();
            #endregion

            foreach (var dtC in ModelSum)
            {
                if (startIdx > 0)
                {
                    sb.AppendLine(",");
                }
                if (incStartIdx)
                {
                    incStartIdx = false;
                    startIdx++;
                }
                sb.AppendLine("{");

                sb.Append("\"" + "title" + "\":\"" + dtC.COL_NAME + "\"");
                sb.Append(",");
                if (dtC.DB_COLUMN == null)
                {
                    sb.Append("\"data\":\"" + dtC.COL_NAME.ToUpper() + "\"");
                }
                else
                {
                    sb.Append("\"data\":\"" + dtC.DB_COLUMN.ToUpper() + "\"");
                }
                sb.Append(",");
                if (dtC.COL_TYPE == 2)
                {//int
                    sb.AppendLine("\"render\": function (data, type, full, meta) {");
                    sb.AppendLine("return digitsFormat(toNumberFormat(data),0);");
                    sb.AppendLine("},");
                    sb.AppendLine("\"className\" : \"" + ColumnsTextAlign.Right.GetDescription() + "\",");
                }
                else if (dtC.COL_TYPE == 3)
                {//decimal
                    if (dtC.MAX_LENGTH.AsString().IndexOf(',') > -1)
                    {
                        string digit = dtC.MAX_LENGTH.Split(',')[1];
                        sb.AppendLine("\"render\": function (data, type, full, meta) {");
                        sb.AppendLine("return digitsFormat(toNumberFormat(data)," + digit + ");");
                        sb.AppendLine("},");
                        sb.AppendLine("\"className\" : \"" + ColumnsTextAlign.Right.GetDescription() + "\",");
                    }
                    else
                    {
                        sb.AppendLine("\"render\": function (data, type, full, meta) {");
                        sb.AppendLine("return toNumberFormat(data);");
                        sb.AppendLine("},");
                        sb.AppendLine("\"className\" : \"" + ColumnsTextAlign.Right.GetDescription() + "\",");
                    }
                }
                else if (dtC.COL_TYPE == 4)
                {//date
                    sb.AppendLine("\"className\" : \"" + ColumnsTextAlign.Center.GetDescription() + "\",");
                }

                sb.Append("}");
                c++;
            }

            sb.Append("]");
            sb.AppendLine(",");

            sb.Append("\"order\": [],");
            sb.AppendLine("drawCallback: function () {");
            sb.AppendLine("if ($('#GridValSum" + name + " tbody .dataTables_empty').length) {");
            sb.AppendLine("$('#GridValSum" + name + "_wrapper').find('.dt-buttons').addClass('hidden');");
            sb.AppendLine("}");
            sb.AppendLine("else {");
            sb.AppendLine("$('#GridValSum" + name + "_wrapper').find('.dt-buttons').removeClass('hidden');");
            sb.AppendLine("}");
            sb.AppendLine("$('[data-toggle=\"tooltip\"]').tooltip({");
            sb.AppendLine("trigger: 'hover'");
            sb.AppendLine("});");

            sb.AppendLine("}");
            sb.AppendLine("};");
            sb.AppendLine("if (result.mode != \"standby\") {");
            sb.AppendLine("$(\"#rowGridValSum" + name + "\").removeClass(\"hidden\");");
            sb.AppendLine("option[\"ajax\"]= {");
            sb.AppendLine("url: \"" + url.Action("GetDataSumValidateExcel", "ValidateExcel", new
            {
                Area = "Ux"
            }) + "\",");
            sb.AppendLine("type: \"post\",");
            sb.AppendLine("error: OnAjaxError,");
            sb.AppendLine("data: result");
            sb.AppendLine("};");
            sb.AppendLine("}");

            if (ModelSum.Count() > 0)
            {
                sb.AppendLine("GridValSum" + name + " = $(\"#GridValSum" + name + "\").DataTable(option);");
            }
            sb.AppendLine("}");
            #endregion
            #endregion

            #region OnChange Checkbox
            sb.AppendLine("$('#ValViewError ,#ValViewCom').change(function () {");
            sb.AppendLine("var E = 'N'; ");
            sb.AppendLine("var C = 'N'; ");
            sb.AppendLine("if ($('#ValViewError').is(\":checked\")) {");
            sb.AppendLine(" E = \"Y\"; ");
            sb.AppendLine("}");
            sb.AppendLine("if ($('#ValViewCom').is(\":checked\")) {");
            sb.AppendLine("C = \"Y\"; ");
            sb.AppendLine("}");
            sb.AppendLine("bindGridVal" + name + "({ pE: E, pC: C });");
            if (ModelSum.Count() > 0)
            {
                sb.AppendLine("bindGridValSum" + name + "({ pE: E, pC: C });");
            }
            sb.AppendLine("});");
            #endregion

            #region Change File
            sb.AppendLine("var OnUploadFile = function () {");
            sb.AppendLine("var form = $(\"form[id=" + formName + "]\");");
            sb.AppendLine("var formData = form.serializeFormData();");
            sb.AppendLine("formData.append(\"PRG_CODE\", '" + prgCode + "');");
            sb.AppendLine("if (form.validFile() && form.valid()) {");
            sb.AppendLine("$.ajax({");
            sb.AppendLine("url: '" + url.Action("GetValidateExcel", "ValidateExcel", new
            {
                Area = "Ux"
            }) + "',");
            sb.AppendLine("type: 'POST',");
            sb.AppendLine("data: formData,");
            sb.AppendLine("contentType: false,");
            sb.AppendLine("processData: false,");
            sb.AppendLine("success: function (response) {");

            sb.AppendLine("$(\"#ValViewError\").prop('checked', true); ");
            sb.AppendLine("$(\"#ValViewCom\").prop('checked', false); ");

            sb.AppendLine("$(\"#ValFileName\").val(response.data.filename); ");
            sb.AppendLine("$(\"#ValRecordAll\").val(response.data.total); ");
            sb.AppendLine("$(\"#Valcomplete\").val(response.data.complete); ");
            sb.AppendLine("$(\"#ValError\").val(response.data.error); ");

            sb.AppendLine("bindGridVal" + name + "({ pE: \"Y\" }); ");
            sb.AppendLine("bindGridValSum" + name + "({ pE: \"Y\" }); ");

            sb.AppendLine("$(\"#mdValExcel\").modal(); ");
            sb.AppendLine("},");
            sb.AppendLine("error: OnAjaxError");
            sb.AppendLine("});");
            sb.AppendLine("}");
            sb.AppendLine("return false; ");
            sb.AppendLine("}");
            #endregion

            #region ready
            sb.AppendLine("$(document).ready(function () {");
            #region load grid
            sb.AppendLine("bindGrid" + name + "();");
            if (!isReadOnly)
            {
                sb.AppendLine("$(\"#f" + name + "\").change(OnChange_f" + name + ");");
                sb.AppendLine("$(\"#btnAdd" + name + "\").click(OnClick_btnAdd" + name + ");");
                sb.AppendLine("$(\"#btnDelete" + name + "\").confirm({");
                sb.AppendLine("title: null,");
                sb.Append("content: '");
                sb.Append(Translation.CenterLang.Center.ConfirmDelete);
                sb.AppendLine("',");
                sb.AppendLine("columnClass: 'medium',");
                sb.AppendLine("buttons: {");
                sb.AppendLine("confirm: {");
                sb.Append("text: '");
                sb.Append(Translation.CenterLang.Center.OK);
                sb.AppendLine("',");
                sb.AppendLine("btnClass: \"btn-primary\",");
                sb.AppendLine("action: function () {");
                sb.AppendLine("if ($(\"#validError" + name + "\").length > 0) {");
                sb.AppendLine("$(\"#validError" + name + "\").remove();");
                sb.AppendLine("}");
                if (multiple)
                {
                    sb.AppendLine("var data = Grid" + name + ".rows({ selected: true }).data();");
                    sb.AppendLine("if (data.length > 0) {");
                    sb.AppendLine("Grid" + name + ".rows({ selected: true }).remove().draw();");
                    sb.AppendLine("}");
                    sb.AppendLine("else {");
                    sb.Append("$(\"<span id =\\\"validError" + name + "\\\" class=\\\"text-danger\\\">");
                    sb.Append(Translation.CenterLang.Center.PleaseSelectData);
                    sb.AppendLine("</span>\").insertBefore(\"#f" + name + "\");");
                    sb.AppendLine("}");
                }
                else
                {
                    sb.AppendLine("Grid" + name + ".rows().remove().draw();");
                }
                if (!onChangeName.IsNullOrEmpty())
                {
                    sb.Append(onChangeName);
                    sb.AppendLine("();");
                }
                sb.AppendLine("}");
                sb.AppendLine("},");
                sb.AppendLine("cancel: {");
                sb.Append("text: '");
                sb.Append(Translation.CenterLang.Center.Cancel);
                sb.AppendLine("',");
                sb.AppendLine("btnClass: \"btn-primary\"");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("});");
            }

            sb.AppendLine("bindGridVal" + name + "({mode:'standby'});");
            if (ModelSum.Count() > 0)
            {
                sb.AppendLine("bindGridValSum" + name + "({mode:'standby'});");
            }
            #endregion

            #region on tab change
            sb.AppendLine("$('a[data-toggle=\"tab\"]').on('shown.bs.tab', function (e) { ");
            sb.AppendLine("var tab = $(e.target); ");
            sb.AppendLine("var actived = tab.data('actived'); ");
            sb.AppendLine("var tabId = e.target.id; ");
            sb.AppendLine("if (tabId == \"tabValDetail\") { ");
            sb.AppendLine("var E = 'N'; ");
            sb.AppendLine("var C = 'N'; ");
            sb.AppendLine("if ($('#ValViewError').is(\":checked\")) {");
            sb.AppendLine(" E = \"Y\"; ");
            sb.AppendLine("}");
            sb.AppendLine("if ($('#ValViewCom').is(\":checked\")) {");
            sb.AppendLine("C = \"Y\"; ");
            sb.AppendLine("}");
            sb.AppendLine("bindGridVal" + name + "({ pE: E, pC: C });");
            sb.AppendLine("tab.data('actived', true);");
            sb.AppendLine("}");
            sb.AppendLine("else if (tabId == \"tabValSummary\") { ");
            if (ModelSum.Count() > 0)
            {
                sb.AppendLine("bindGridValSum" + name + "({ pE: E, pC: C });");
                sb.AppendLine("tab.data('actived', true); ");
            }
            sb.AppendLine("}");
            sb.AppendLine("});");
            #endregion

            #region on btn upload click
            sb.AppendLine("$(\"#" + customsBtnUploadName + "\").click(function() {");
            sb.AppendLine("OnUploadFile();");
            sb.AppendLine("});");
            #endregion
            sb.AppendLine("});");
            #endregion

            sb.AppendLine("</script>");
            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region Modal
        public static MvcHtmlString GetVSMModalBegin<TModel>(this HtmlHelper<TModel> html,
            string name, string title = "", params ButtonConfig[] links)
        {
            return GetVSMModalBegin(html, name, title, ModalSize.Xl, true, true, links);
        }
        public static MvcHtmlString GetVSMModalBegin<TModel>(this HtmlHelper<TModel> html,
            string name, string title = "", ModalSize modalSize = ModalSize.Xl, bool includeCancel = true, bool confirmClose = true, params ButtonConfig[] links)
        {
            var sb = new StringBuilder();
            if (modalSize == ModalSize.Fullscreen)
            {
                sb.AppendLine("<div class=\"modal fade modal-fullscreen\" id=\"md" + name + "\" role=\"dialog\">");
                sb.AppendLine("<div class=\"modal-dialog\">");
            }
            else
            {
                sb.AppendLine("<div class=\"modal fade\" id=\"md" + name + "\" role=\"dialog\">");
                sb.Append("<div class=\"modal-dialog ");
                sb.Append(modalSize.GetDescription());
                sb.AppendLine("\">");
            }
            sb.AppendLine("<div class=\"modal-content\">");
            sb.AppendLine("<div class=\"modal-header\">");
            sb.Append("<button type=\"button\" class=\"close");
            if (confirmClose)
            {
                sb.AppendLine(" std-modal-confirmclose\">&times;</button>");
            }
            else
            {
                sb.AppendLine("\" data-dismiss=\"modal\">&times;</button>");
            }
            sb.AppendLine("<h4 class=\"modal-title\" id=\"mdTitle" + name + "\">");
            if (!title.IsNullOrEmpty())
            {
                sb.Append(title);
            }
            sb.Append("</h4>");
            sb.AppendLine("</div>");

            if ((links != null && links.Count() > 0) || includeCancel)
            {
                sb.AppendLine("<div class=\"modal-buttontoolbar\">");
                if (links != null)
                {
                    foreach (var link in links)
                    {
                        sb.Append("<a ");
                        if (!link.Name.IsNullOrEmpty())
                        {
                            sb.Append("id =\"btn");
                            sb.Append(link.Name);
                            sb.Append(name);
                            sb.Append("\" value=\"");
                            sb.Append(link.Name);
                            sb.Append(name);
                            sb.Append("\"");
                        }
                        if (!link.CssClass.IsNullOrEmpty())
                        {
                            sb.Append(" class=\"btn btn-xs btn-white btn-round ");
                            sb.Append(link.CssClass);
                            sb.Append("\"");
                        }
                        else
                        {
                            sb.Append(" class=\"btn btn-xs btn-white btn-info btn-round\"");
                        }
                        if (!link.Url.IsNullOrEmpty())
                        {
                            sb.Append(" href=\"");
                            sb.Append(link.Url);
                            sb.Append("\"");
                        }
                        else
                        {
                            sb.Append(" href=\"javascript:void(0)\"");
                        }
                        if (link.Tooltip)
                        {
                            sb.Append(" data-toggle=\"tooltip\" data-placement=\"right\"");
                        }
                        if (!link.Title.IsNullOrEmpty())
                        {
                            sb.Append(" title=\"");
                            sb.Append(link.Title);
                            sb.Append("\"");
                        }
                        if (link.HtmlAttribute.Count > 0)
                        {
                            foreach (var item in link.HtmlAttribute)
                            {
                                sb.Append(" ");
                                sb.Append(item.Key);
                                sb.Append("=\"");
                                sb.Append(item.Value);
                                sb.Append("\"");
                            }
                        }
                        sb.AppendLine(">");
                        if (!link.IconCssClass.IsNullOrEmpty() && link.IconPosition == StandardIconPosition.BeforeText)
                        {
                            sb.Append("<i class=\"ace-icon fa ");
                            sb.Append(link.IconCssClass);
                            if (link.IconColor != TextColor.None)
                            {
                                sb.Append(" ");
                                sb.Append(link.IconColor.GetDescription());
                            }
                            sb.Append(" align-top bigger-125\"></i>");
                        }
                        if (!link.Text.IsNullOrEmpty())
                        {
                            sb.AppendLine(link.Text);
                        }
                        if (!link.IconCssClass.IsNullOrEmpty() && link.IconPosition == StandardIconPosition.AfterText)
                        {
                            sb.Append("<i class=\"ace-icon fa ");
                            sb.Append(link.IconCssClass);
                            if (link.IconColor != TextColor.None)
                            {
                                sb.Append(" ");
                                sb.Append(link.IconColor.GetDescription());
                            }
                            sb.Append(" align-top bigger-125\"></i>");
                        }
                        sb.AppendLine("</a>");
                    }
                }
                if (includeCancel)
                {
                    sb.Append("<a id=\"btnCancel" + name + "\" value=\"Cancel" + name + "\" class=\"btn btn-xs btn-white btn-info btn-round");
                    if (confirmClose)
                    {
                        sb.AppendLine(" std-modal-confirmcancel\" href=\"javascript:void(0)\">");
                    }
                    else
                    {
                        sb.AppendLine("\" data-dismiss=\"modal\">");
                    }
                    sb.AppendLine("<i class=\"ace-icon fa fa-remove red2 align-top bigger-125\"></i>");
                    sb.Append(Translation.CenterLang.Center.Cancel);
                    sb.AppendLine("</a>");
                }
                sb.AppendLine("</div>");
            }

            sb.AppendLine("<div class=\"modal-body\">");
            sb.AppendLine("<div id=\"notModal" + name + "\" style=\"display: none;\"></div>");
            sb.AppendLine("<div class=\"row\">");
            sb.AppendLine("<div class=\"col-md-12\">");
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString GetVSMModalEnd<TModel>(this HtmlHelper<TModel> html)
        {
            var sb = new StringBuilder();
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString GetVSMModalEnd<TModel>(this HtmlHelper<TModel> html,
            string name, params ButtonConfig[] links)
        {
            var sb = new StringBuilder();
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");

            if (links != null)
            {
                sb.AppendLine("<div class=\"modal-footer\">");
                foreach (var link in links)
                {
                    sb.Append("<a ");
                    if (!link.Name.IsNullOrEmpty())
                    {
                        sb.Append("id =\"btn");
                        sb.Append(link.Name);
                        sb.Append(name);
                        sb.Append("\" value=\"");
                        sb.Append(link.Name);
                        sb.Append(name);
                        sb.Append("\"");
                    }
                    if (!link.CssClass.IsNullOrEmpty())
                    {
                        sb.Append(" class=\"btn btn-xs btn-white btn-round ");
                        sb.Append(link.CssClass);
                        sb.Append("\"");
                    }
                    else
                    {
                        sb.Append(" class=\"btn btn-xs btn-white btn-info btn-round\"");
                    }
                    if (!link.Url.IsNullOrEmpty())
                    {
                        sb.Append(" href=\"");
                        sb.Append(link.Url);
                        sb.Append("\"");
                    }
                    else
                    {
                        sb.Append(" href=\"javascript:void(0)\"");
                    }
                    if (link.Tooltip)
                    {
                        sb.Append(" data-toggle=\"tooltip\" data-placement=\"right\"");
                    }
                    if (!link.Title.IsNullOrEmpty())
                    {
                        sb.Append(" title=\"");
                        sb.Append(link.Title);
                        sb.Append("\"");
                    }
                    if (link.HtmlAttribute.Count > 0)
                    {
                        foreach (var item in link.HtmlAttribute)
                        {
                            sb.Append(" ");
                            sb.Append(item.Key);
                            sb.Append("=\"");
                            sb.Append(item.Value);
                            sb.Append("\"");
                        }
                    }
                    sb.AppendLine(">");
                    if (!link.IconCssClass.IsNullOrEmpty() && link.IconPosition == StandardIconPosition.BeforeText)
                    {
                        sb.Append("<i class=\"ace-icon fa ");
                        sb.Append(link.IconCssClass);
                        sb.Append(" align-top bigger-125\"></i>");
                    }
                    if (!link.Text.IsNullOrEmpty())
                    {
                        sb.AppendLine(link.Text);
                    }
                    if (!link.IconCssClass.IsNullOrEmpty() && link.IconPosition == StandardIconPosition.AfterText)
                    {
                        sb.Append("<i class=\"ace-icon fa ");
                        sb.Append(link.IconCssClass);
                        sb.Append(" align-top bigger-125\"></i>");
                    }
                    sb.AppendLine("</a>");
                }
                sb.AppendLine("</div>");
            }
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            return MvcHtmlString.Create(sb.ToString());
        }


        public static MvcHtmlString GetVSMModalValidate<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string name = "ValExcel", string title = "Validate", ModalSize modalSize = ModalSize.Xl, bool includeCancel = true, bool confirmClose = true)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string label = GetControlLabel(metadata, expressionText);
            string crtl_name = GetControlName(html, expressionText);

            var sb = new StringBuilder();
            if (modalSize == ModalSize.Fullscreen)
            {
                sb.AppendLine("<div class=\"modal fade modal-fullscreen\" id=\"md" + name + "\" role=\"dialog\">");
                sb.AppendLine("<div class=\"modal-dialog\">");
            }
            else
            {
                sb.AppendLine("<div class=\"modal fade\" id=\"md" + name + "\" role=\"dialog\">");
                sb.Append("<div class=\"modal-dialog ");
                sb.Append(modalSize.GetDescription());
                sb.AppendLine("\">");
            }
            sb.AppendLine("<div class=\"modal-content\">");
            sb.AppendLine("<div class=\"modal-header\">");
            sb.Append("<button type=\"button\" class=\"close");
            if (confirmClose)
            {
                sb.AppendLine(" std-modal-confirmclose\">&times;</button>");
            }
            else
            {
                sb.AppendLine("\" data-dismiss=\"modal\">&times;</button>");
            }
            sb.AppendLine("<h4 class=\"modal-title\" id=\"mdTitle" + name + "\">");
            if (!title.IsNullOrEmpty())
            {
                sb.Append(title);
            }
            sb.Append("</h4>");
            sb.AppendLine("</div>");

            ButtonConfig btn = new ButtonConfig("Continue")
            {
                Text = "Continue",
                IconCssClass = FaIcons.FaForward,
                IconPosition = StandardIconPosition.AfterText
            };
            ButtonConfig[] links = new ButtonConfig[] { btn };

            if ((links != null && links.Count() > 0) || includeCancel)
            {
                sb.AppendLine("<div class=\"modal-buttontoolbar\">");
                if (links != null)
                {
                    foreach (var link in links)
                    {
                        sb.Append("<a ");
                        if (!link.Name.IsNullOrEmpty())
                        {
                            sb.Append("id =\"btn");
                            sb.Append(link.Name);
                            sb.Append(name);
                            sb.Append("\" value=\"");
                            sb.Append(link.Name);
                            sb.Append(name);
                            sb.Append("\"");
                        }
                        if (!link.CssClass.IsNullOrEmpty())
                        {
                            sb.Append(" class=\"btn btn-xs btn-white btn-round ");
                            sb.Append(link.CssClass);
                            sb.Append("\"");
                        }
                        else
                        {
                            sb.Append(" class=\"btn btn-xs btn-white btn-info btn-round\"");
                        }
                        if (!link.Url.IsNullOrEmpty())
                        {
                            sb.Append(" href=\"");
                            sb.Append(link.Url);
                            sb.Append("\"");
                        }
                        else
                        {
                            sb.Append(" href=\"javascript:void(0)\"");
                        }
                        if (link.Tooltip)
                        {
                            sb.Append(" data-toggle=\"tooltip\" data-placement=\"right\"");
                        }
                        if (!link.Title.IsNullOrEmpty())
                        {
                            sb.Append(" title=\"");
                            sb.Append(link.Title);
                            sb.Append("\"");
                        }
                        if (link.HtmlAttribute.Count > 0)
                        {
                            foreach (var item in link.HtmlAttribute)
                            {
                                sb.Append(" ");
                                sb.Append(item.Key);
                                sb.Append("=\"");
                                sb.Append(item.Value);
                                sb.Append("\"");
                            }
                        }
                        sb.AppendLine(">");
                        if (!link.IconCssClass.IsNullOrEmpty() && link.IconPosition == StandardIconPosition.BeforeText)
                        {
                            sb.Append("<i class=\"ace-icon fa ");
                            sb.Append(link.IconCssClass);
                            if (link.IconColor != TextColor.None)
                            {
                                sb.Append(" ");
                                sb.Append(link.IconColor.GetDescription());
                            }
                            sb.Append(" align-top bigger-125\"></i>");
                        }
                        if (!link.Text.IsNullOrEmpty())
                        {
                            sb.AppendLine(link.Text);
                        }
                        if (!link.IconCssClass.IsNullOrEmpty() && link.IconPosition == StandardIconPosition.AfterText)
                        {
                            sb.Append("<i class=\"ace-icon fa ");
                            sb.Append(link.IconCssClass);
                            if (link.IconColor != TextColor.None)
                            {
                                sb.Append(" ");
                                sb.Append(link.IconColor.GetDescription());
                            }
                            sb.Append(" align-top bigger-125\"></i>");
                        }
                        sb.AppendLine("</a>");
                    }
                }
                if (includeCancel)
                {
                    sb.Append("<a id=\"btnCancel" + name + "\" value=\"Cancel" + name + "\" class=\"btn btn-xs btn-white btn-info btn-round");
                    if (confirmClose)
                    {
                        sb.AppendLine(" std-modal-confirmcancel\" href=\"javascript:void(0)\">");
                    }
                    else
                    {
                        sb.AppendLine("\" data-dismiss=\"modal\">");
                    }
                    sb.AppendLine("<i class=\"ace-icon fa fa-remove red2 align-top bigger-125\"></i>");
                    sb.Append(Translation.CenterLang.Center.Cancel);
                    sb.AppendLine("</a>");
                }
                sb.AppendLine("</div>");
            }

            sb.AppendLine("<div class=\"modal-body\">");
            sb.AppendLine("<div id=\"notModal" + name + "\" style=\"display: none;\"></div>");
            sb.AppendLine("<div class=\"row\">");
            sb.AppendLine("<div class=\"col-md-12\">");

            #region From
            sb.AppendLine("<form role=\"form\" method=\"post\" id=\"formValExcel\" class=\"form-horizontal\">");
            sb.AppendLine("<div class=\"row\">");
            sb.AppendLine("<div class=\"col-md-12 col-xs-12\">");
            //row 1
            sb.AppendLine("<div class=\"row\">");
            sb.AppendLine("<div class=\"col-md-2\"></div>");
            sb.AppendLine("<div class=\"col-md-6\">");
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendLine("<label for=\"ValFileName\" class=\"control-label col-md-4\">" + Translation.CenterLang.Center.ValFileName + "</label>");
            sb.AppendLine("<div class=\"col-md-8\">");
            sb.AppendLine("<input type=\"text\" name=\"ValFileName\" readonly=\"\" id=\"ValFileName\" class=\"form-control input-sm\">");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            //row 2
            sb.AppendLine("<div class=\"row\">");
            sb.AppendLine("<div class=\"col-md-4\">");
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendLine("<label for=\"ValRecordAll\" class=\"control-label col-md-4\">" + Translation.CenterLang.Center.ValRecordAll + "</label>");
            sb.AppendLine("<div class=\"col-md-8\">");
            sb.AppendLine("<input type=\"text\" name=\"ValRecordAll\" readonly=\"\" id=\"ValRecordAll\" class=\"form-control input-sm\">");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div class=\"col-md-4\">");
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendLine("<label for=\"Valcomplete\" class=\"control-label col-md-4\">" + Translation.CenterLang.Center.ValComplete + "</label>");
            sb.AppendLine("<div class=\"col-md-8\">");
            sb.AppendLine("<input type=\"text\" name=\"Valcomplete\" readonly=\"\" id=\"Valcomplete\" class=\"form-control input-sm\">");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div class=\"col-md-4\">");
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendLine("<label for=\"ValError\" class=\"control-label col-md-4\">" + Translation.CenterLang.Center.ValError + "</label>");
            sb.AppendLine("<div class=\"col-md-8\">");
            sb.AppendLine("<input type=\"text\" name=\"ValError\" readonly=\"\" id=\"ValError\" class=\"form-control input-sm\">");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            //row 3
            sb.AppendLine("<div class=\"row\">");
            sb.AppendLine("<div class=\"col-md-4\">");
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendLine("<label for=\"ValViewError\" class=\"control-label col-md-4\">" + Translation.CenterLang.Center.ValDisplayData + "</label>");
            sb.AppendLine("<div class=\"col-md-8\">");
            sb.AppendLine("<div class=\"checkbox\" style=\"padding-top: 0;\">");
            sb.AppendLine("<label class=\"checkbox\">");
            sb.AppendLine("<input type=\"checkbox\" name=\"ValViewError\" id=\"ValViewError\">" + Translation.CenterLang.Center.ValError);
            sb.AppendLine("</label>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendLine("<div class=\"col-md-8 col-md-offset-4\">");
            sb.AppendLine("<div class=\"checkbox\" style=\"padding-top: 0;\">");
            sb.AppendLine("<label class=\"checkbox\">");
            sb.AppendLine("<input type=\"checkbox\" name=\"ValViewCom\" id=\"ValViewCom\">" + Translation.CenterLang.Center.ValComplete);
            sb.AppendLine("</label>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendLine("<div class=\"col-md-8 col-md-offset-4\">");
            sb.AppendLine("<div class=\"form-control-static text-danger\"></div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</form>");

            #region tab
            sb.AppendLine("<ul class=\"nav nav-tabs padding-12 tab-color-blue background-blue\" id=\"vsmTab\">");
            sb.AppendLine("<li class=\"active\">");
            sb.AppendLine("<a data-toggle=\"tab\" id=\"tabValDetail\" href=\"#ValDetail\">Detail</a>");
            sb.AppendLine("</li>");
            sb.AppendLine("<li>");
            sb.AppendLine("<a data-toggle=\"tab\" id=\"tabValSummary\" href=\"#ValSummary\">Summary</a>");
            sb.AppendLine("</li>");
            sb.AppendLine("</ul>");
            sb.AppendLine("<div class=\"tab-content\">");
            sb.AppendLine("<div id=\"ValDetail\" class=\"tab-pane in active\">");
            //tab 1
            sb.AppendLine("<div id=\"rowGridVal" + crtl_name + "\" class=\"row hidden\">");
            sb.AppendLine("<div class=\"col-md-12\">");
            sb.AppendLine("<table id=\"GridVal" + crtl_name + "\" class=\"table table-striped table-bordered table-hover\"></table>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            //end tab 1
            sb.AppendLine("</div>");
            sb.AppendLine("<div id=\"ValSummary\" class=\"tab-pane\">");
            //tab 2
            sb.AppendLine("<div id=\"rowGridValSum" + crtl_name + "\" class=\"row hidden\">");
            sb.AppendLine("<div class=\"col-md-12\">");
            sb.AppendLine("<table id=\"GridValSum" + crtl_name + "\" class=\"table table-striped table-bordered table-hover\"></table>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            //end tab 2
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            #endregion

            #endregion

            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region Private

        private static string GetControlName<TModel>(HtmlHelper<TModel> html, string expressionText)
        {
            return html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText);
        }
        private static string GetControlLabel(ModelMetadata metadata, string expressionText)
        {
            string label = metadata.DisplayName;
            if (label == null)
            {
                label = metadata.PropertyName;
                if (label == null)
                {
                    char[] chrArray = new char[] { '.' };
                    label = expressionText.Split(chrArray).Last<string>();
                }
            }
            return label;
        }
        /// <summary>
        /// Add by Waiantarai
        /// </summary>
        /// <param name="fileextension"></param>
        /// <returns></returns>
        private static string ContentType(string fileextension)
        {
            IDictionary<string, string> _mappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {
            #region Big freaking list of mime types
            // combination of values from Windows 7 Registry and 
            // from C:\Windows\System32\inetsrv\config\applicationHost.config
            // some added, including .7z and .dat
            {".323", "text/h323"},
            {".3g2", "video/3gpp2"},
            {".3gp", "video/3gpp"},
            {".3gp2", "video/3gpp2"},
            {".3gpp", "video/3gpp"},
            {".7z", "application/x-7z-compressed"},
            {".aa", "audio/audible"},
            {".AAC", "audio/aac"},
            {".aaf", "application/octet-stream"},
            {".aax", "audio/vnd.audible.aax"},
            {".ac3", "audio/ac3"},
            {".aca", "application/octet-stream"},
            {".accda", "application/msaccess.addin"},
            {".accdb", "application/msaccess"},
            {".accdc", "application/msaccess.cab"},
            {".accde", "application/msaccess"},
            {".accdr", "application/msaccess.runtime"},
            {".accdt", "application/msaccess"},
            {".accdw", "application/msaccess.webapplication"},
            {".accft", "application/msaccess.ftemplate"},
            {".acx", "application/internet-property-stream"},
            {".AddIn", "text/xml"},
            {".ade", "application/msaccess"},
            {".adobebridge", "application/x-bridge-url"},
            {".adp", "application/msaccess"},
            {".ADT", "audio/vnd.dlna.adts"},
            {".ADTS", "audio/aac"},
            {".afm", "application/octet-stream"},
            {".ai", "application/postscript"},
            {".aif", "audio/x-aiff"},
            {".aifc", "audio/aiff"},
            {".aiff", "audio/aiff"},
            {".air", "application/vnd.adobe.air-application-installer-package+zip"},
            {".amc", "application/x-mpeg"},
            {".application", "application/x-ms-application"},
            {".art", "image/x-jg"},
            {".asa", "application/xml"},
            {".asax", "application/xml"},
            {".ascx", "application/xml"},
            {".asd", "application/octet-stream"},
            {".asf", "video/x-ms-asf"},
            {".ashx", "application/xml"},
            {".asi", "application/octet-stream"},
            {".asm", "text/plain"},
            {".asmx", "application/xml"},
            {".aspx", "application/xml"},
            {".asr", "video/x-ms-asf"},
            {".asx", "video/x-ms-asf"},
            {".atom", "application/atom+xml"},
            {".au", "audio/basic"},
            {".avi", "video/x-msvideo"},
            {".axs", "application/olescript"},
            {".bas", "text/plain"},
            {".bcpio", "application/x-bcpio"},
            {".bin", "application/octet-stream"},
            {".bmp", "image/bmp"},
            {".c", "text/plain"},
            {".cab", "application/octet-stream"},
            {".caf", "audio/x-caf"},
            {".calx", "application/vnd.ms-office.calx"},
            {".cat", "application/vnd.ms-pki.seccat"},
            {".cc", "text/plain"},
            {".cd", "text/plain"},
            {".cdda", "audio/aiff"},
            {".cdf", "application/x-cdf"},
            {".cer", "application/x-x509-ca-cert"},
            {".chm", "application/octet-stream"},
            {".class", "application/x-java-applet"},
            {".clp", "application/x-msclip"},
            {".cmx", "image/x-cmx"},
            {".cnf", "text/plain"},
            {".cod", "image/cis-cod"},
            {".config", "application/xml"},
            {".contact", "text/x-ms-contact"},
            {".coverage", "application/xml"},
            {".cpio", "application/x-cpio"},
            {".cpp", "text/plain"},
            {".crd", "application/x-mscardfile"},
            {".crl", "application/pkix-crl"},
            {".crt", "application/x-x509-ca-cert"},
            {".cs", "text/plain"},
            {".csdproj", "text/plain"},
            {".csh", "application/x-csh"},
            {".csproj", "text/plain"},
            {".css", "text/css"},
            {".csv", "text/csv"},
            {".cur", "application/octet-stream"},
            {".cxx", "text/plain"},
            {".dat", "application/octet-stream"},
            {".datasource", "application/xml"},
            {".dbproj", "text/plain"},
            {".dcr", "application/x-director"},
            {".def", "text/plain"},
            {".deploy", "application/octet-stream"},
            {".der", "application/x-x509-ca-cert"},
            {".dgml", "application/xml"},
            {".dib", "image/bmp"},
            {".dif", "video/x-dv"},
            {".dir", "application/x-director"},
            {".disco", "text/xml"},
            {".dll", "application/x-msdownload"},
            {".dll.config", "text/xml"},
            {".dlm", "text/dlm"},
            {".doc", "application/msword"},
            {".docm", "application/vnd.ms-word.document.macroEnabled.12"},
            {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            {".dot", "application/msword"},
            {".dotm", "application/vnd.ms-word.template.macroEnabled.12"},
            {".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
            {".dsp", "application/octet-stream"},
            {".dsw", "text/plain"},
            {".dtd", "text/xml"},
            {".dtsConfig", "text/xml"},
            {".dv", "video/x-dv"},
            {".dvi", "application/x-dvi"},
            {".dwf", "drawing/x-dwf"},
            {".dwp", "application/octet-stream"},
            {".dxr", "application/x-director"},
            {".eml", "message/rfc822"},
            {".emz", "application/octet-stream"},
            {".eot", "application/octet-stream"},
            {".eps", "application/postscript"},
            {".etl", "application/etl"},
            {".etx", "text/x-setext"},
            {".evy", "application/envoy"},
            {".exe", "application/octet-stream"},
            {".exe.config", "text/xml"},
            {".fdf", "application/vnd.fdf"},
            {".fif", "application/fractals"},
            {".filters", "Application/xml"},
            {".fla", "application/octet-stream"},
            {".flr", "x-world/x-vrml"},
            {".flv", "video/x-flv"},
            {".fsscript", "application/fsharp-script"},
            {".fsx", "application/fsharp-script"},
            {".generictest", "application/xml"},
            {".gif", "image/gif"},
            {".group", "text/x-ms-group"},
            {".gsm", "audio/x-gsm"},
            {".gtar", "application/x-gtar"},
            {".gz", "application/x-gzip"},
            {".h", "text/plain"},
            {".hdf", "application/x-hdf"},
            {".hdml", "text/x-hdml"},
            {".hhc", "application/x-oleobject"},
            {".hhk", "application/octet-stream"},
            {".hhp", "application/octet-stream"},
            {".hlp", "application/winhlp"},
            {".hpp", "text/plain"},
            {".hqx", "application/mac-binhex40"},
            {".hta", "application/hta"},
            {".htc", "text/x-component"},
            {".htm", "text/html"},
            {".html", "text/html"},
            {".htt", "text/webviewhtml"},
            {".hxa", "application/xml"},
            {".hxc", "application/xml"},
            {".hxd", "application/octet-stream"},
            {".hxe", "application/xml"},
            {".hxf", "application/xml"},
            {".hxh", "application/octet-stream"},
            {".hxi", "application/octet-stream"},
            {".hxk", "application/xml"},
            {".hxq", "application/octet-stream"},
            {".hxr", "application/octet-stream"},
            {".hxs", "application/octet-stream"},
            {".hxt", "text/html"},
            {".hxv", "application/xml"},
            {".hxw", "application/octet-stream"},
            {".hxx", "text/plain"},
            {".i", "text/plain"},
            {".ico", "image/x-icon"},
            {".ics", "application/octet-stream"},
            {".idl", "text/plain"},
            {".ief", "image/ief"},
            {".iii", "application/x-iphone"},
            {".inc", "text/plain"},
            {".inf", "application/octet-stream"},
            {".inl", "text/plain"},
            {".ins", "application/x-internet-signup"},
            {".ipa", "application/x-itunes-ipa"},
            {".ipg", "application/x-itunes-ipg"},
            {".ipproj", "text/plain"},
            {".ipsw", "application/x-itunes-ipsw"},
            {".iqy", "text/x-ms-iqy"},
            {".isp", "application/x-internet-signup"},
            {".ite", "application/x-itunes-ite"},
            {".itlp", "application/x-itunes-itlp"},
            {".itms", "application/x-itunes-itms"},
            {".itpc", "application/x-itunes-itpc"},
            {".IVF", "video/x-ivf"},
            {".jar", "application/java-archive"},
            {".java", "application/octet-stream"},
            {".jck", "application/liquidmotion"},
            {".jcz", "application/liquidmotion"},
            {".jfif", "image/pjpeg"},
            {".jnlp", "application/x-java-jnlp-file"},
            {".jpb", "application/octet-stream"},
            {".jpe", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".jpg", "image/jpeg"},
            {".js", "application/x-javascript"},
            {".jsx", "text/jscript"},
            {".jsxbin", "text/plain"},
            {".latex", "application/x-latex"},
            {".library-ms", "application/windows-library+xml"},
            {".lit", "application/x-ms-reader"},
            {".loadtest", "application/xml"},
            {".lpk", "application/octet-stream"},
            {".lsf", "video/x-la-asf"},
            {".lst", "text/plain"},
            {".lsx", "video/x-la-asf"},
            {".lzh", "application/octet-stream"},
            {".m13", "application/x-msmediaview"},
            {".m14", "application/x-msmediaview"},
            {".m1v", "video/mpeg"},
            {".m2t", "video/vnd.dlna.mpeg-tts"},
            {".m2ts", "video/vnd.dlna.mpeg-tts"},
            {".m2v", "video/mpeg"},
            {".m3u", "audio/x-mpegurl"},
            {".m3u8", "audio/x-mpegurl"},
            {".m4a", "audio/m4a"},
            {".m4b", "audio/m4b"},
            {".m4p", "audio/m4p"},
            {".m4r", "audio/x-m4r"},
            {".m4v", "video/x-m4v"},
            {".mac", "image/x-macpaint"},
            {".mak", "text/plain"},
            {".man", "application/x-troff-man"},
            {".manifest", "application/x-ms-manifest"},
            {".map", "text/plain"},
            {".master", "application/xml"},
            {".mda", "application/msaccess"},
            {".mdb", "application/x-msaccess"},
            {".mde", "application/msaccess"},
            {".mdp", "application/octet-stream"},
            {".me", "application/x-troff-me"},
            {".mfp", "application/x-shockwave-flash"},
            {".mht", "message/rfc822"},
            {".mhtml", "message/rfc822"},
            {".mid", "audio/mid"},
            {".midi", "audio/mid"},
            {".mix", "application/octet-stream"},
            {".mk", "text/plain"},
            {".mmf", "application/x-smaf"},
            {".mno", "text/xml"},
            {".mny", "application/x-msmoney"},
            {".mod", "video/mpeg"},
            {".mov", "video/quicktime"},
            {".movie", "video/x-sgi-movie"},
            {".mp2", "video/mpeg"},
            {".mp2v", "video/mpeg"},
            {".mp3", "audio/mpeg"},
            {".mp4", "video/mp4"},
            {".mp4v", "video/mp4"},
            {".mpa", "video/mpeg"},
            {".mpe", "video/mpeg"},
            {".mpeg", "video/mpeg"},
            {".mpf", "application/vnd.ms-mediapackage"},
            {".mpg", "video/mpeg"},
            {".mpp", "application/vnd.ms-project"},
            {".mpv2", "video/mpeg"},
            {".mqv", "video/quicktime"},
            {".ms", "application/x-troff-ms"},
            {".msi", "application/octet-stream"},
            {".mso", "application/octet-stream"},
            {".mts", "video/vnd.dlna.mpeg-tts"},
            {".mtx", "application/xml"},
            {".mvb", "application/x-msmediaview"},
            {".mvc", "application/x-miva-compiled"},
            {".mxp", "application/x-mmxp"},
            {".nc", "application/x-netcdf"},
            {".nsc", "video/x-ms-asf"},
            {".nws", "message/rfc822"},
            {".ocx", "application/octet-stream"},
            {".oda", "application/oda"},
            {".odc", "text/x-ms-odc"},
            {".odh", "text/plain"},
            {".odl", "text/plain"},
            {".odp", "application/vnd.oasis.opendocument.presentation"},
            {".ods", "application/oleobject"},
            {".odt", "application/vnd.oasis.opendocument.text"},
            {".one", "application/onenote"},
            {".onea", "application/onenote"},
            {".onepkg", "application/onenote"},
            {".onetmp", "application/onenote"},
            {".onetoc", "application/onenote"},
            {".onetoc2", "application/onenote"},
            {".orderedtest", "application/xml"},
            {".osdx", "application/opensearchdescription+xml"},
            {".p10", "application/pkcs10"},
            {".p12", "application/x-pkcs12"},
            {".p7b", "application/x-pkcs7-certificates"},
            {".p7c", "application/pkcs7-mime"},
            {".p7m", "application/pkcs7-mime"},
            {".p7r", "application/x-pkcs7-certreqresp"},
            {".p7s", "application/pkcs7-signature"},
            {".pbm", "image/x-portable-bitmap"},
            {".pcast", "application/x-podcast"},
            {".pct", "image/pict"},
            {".pcx", "application/octet-stream"},
            {".pcz", "application/octet-stream"},
            {".pdf", "application/pdf"},
            {".pfb", "application/octet-stream"},
            {".pfm", "application/octet-stream"},
            {".pfx", "application/x-pkcs12"},
            {".pgm", "image/x-portable-graymap"},
            {".pic", "image/pict"},
            {".pict", "image/pict"},
            {".pkgdef", "text/plain"},
            {".pkgundef", "text/plain"},
            {".pko", "application/vnd.ms-pki.pko"},
            {".pls", "audio/scpls"},
            {".pma", "application/x-perfmon"},
            {".pmc", "application/x-perfmon"},
            {".pml", "application/x-perfmon"},
            {".pmr", "application/x-perfmon"},
            {".pmw", "application/x-perfmon"},
            {".png", "image/png"},
            {".pnm", "image/x-portable-anymap"},
            {".pnt", "image/x-macpaint"},
            {".pntg", "image/x-macpaint"},
            {".pnz", "image/png"},
            {".pot", "application/vnd.ms-powerpoint"},
            {".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12"},
            {".potx", "application/vnd.openxmlformats-officedocument.presentationml.template"},
            {".ppa", "application/vnd.ms-powerpoint"},
            {".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12"},
            {".ppm", "image/x-portable-pixmap"},
            {".pps", "application/vnd.ms-powerpoint"},
            {".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
            {".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
            {".ppt", "application/vnd.ms-powerpoint"},
            {".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
            {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
            {".prf", "application/pics-rules"},
            {".prm", "application/octet-stream"},
            {".prx", "application/octet-stream"},
            {".ps", "application/postscript"},
            {".psc1", "application/PowerShell"},
            {".psd", "application/octet-stream"},
            {".psess", "application/xml"},
            {".psm", "application/octet-stream"},
            {".psp", "application/octet-stream"},
            {".pub", "application/x-mspublisher"},
            {".pwz", "application/vnd.ms-powerpoint"},
            {".qht", "text/x-html-insertion"},
            {".qhtm", "text/x-html-insertion"},
            {".qt", "video/quicktime"},
            {".qti", "image/x-quicktime"},
            {".qtif", "image/x-quicktime"},
            {".qtl", "application/x-quicktimeplayer"},
            {".qxd", "application/octet-stream"},
            {".ra", "audio/x-pn-realaudio"},
            {".ram", "audio/x-pn-realaudio"},
            {".rar", "application/octet-stream"},
            {".ras", "image/x-cmu-raster"},
            {".rat", "application/rat-file"},
            {".rc", "text/plain"},
            {".rc2", "text/plain"},
            {".rct", "text/plain"},
            {".rdlc", "application/xml"},
            {".resx", "application/xml"},
            {".rf", "image/vnd.rn-realflash"},
            {".rgb", "image/x-rgb"},
            {".rgs", "text/plain"},
            {".rm", "application/vnd.rn-realmedia"},
            {".rmi", "audio/mid"},
            {".rmp", "application/vnd.rn-rn_music_package"},
            {".roff", "application/x-troff"},
            {".rpm", "audio/x-pn-realaudio-plugin"},
            {".rqy", "text/x-ms-rqy"},
            {".rtf", "application/rtf"},
            {".rtx", "text/richtext"},
            {".ruleset", "application/xml"},
            {".s", "text/plain"},
            {".safariextz", "application/x-safari-safariextz"},
            {".scd", "application/x-msschedule"},
            {".sct", "text/scriptlet"},
            {".sd2", "audio/x-sd2"},
            {".sdp", "application/sdp"},
            {".sea", "application/octet-stream"},
            {".searchConnector-ms", "application/windows-search-connector+xml"},
            {".setpay", "application/set-payment-initiation"},
            {".setreg", "application/set-registration-initiation"},
            {".settings", "application/xml"},
            {".sgimb", "application/x-sgimb"},
            {".sgml", "text/sgml"},
            {".sh", "application/x-sh"},
            {".shar", "application/x-shar"},
            {".shtml", "text/html"},
            {".sit", "application/x-stuffit"},
            {".sitemap", "application/xml"},
            {".skin", "application/xml"},
            {".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12"},
            {".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide"},
            {".slk", "application/vnd.ms-excel"},
            {".sln", "text/plain"},
            {".slupkg-ms", "application/x-ms-license"},
            {".smd", "audio/x-smd"},
            {".smi", "application/octet-stream"},
            {".smx", "audio/x-smd"},
            {".smz", "audio/x-smd"},
            {".snd", "audio/basic"},
            {".snippet", "application/xml"},
            {".snp", "application/octet-stream"},
            {".sol", "text/plain"},
            {".sor", "text/plain"},
            {".spc", "application/x-pkcs7-certificates"},
            {".spl", "application/futuresplash"},
            {".src", "application/x-wais-source"},
            {".srf", "text/plain"},
            {".SSISDeploymentManifest", "text/xml"},
            {".ssm", "application/streamingmedia"},
            {".sst", "application/vnd.ms-pki.certstore"},
            {".stl", "application/vnd.ms-pki.stl"},
            {".sv4cpio", "application/x-sv4cpio"},
            {".sv4crc", "application/x-sv4crc"},
            {".svc", "application/xml"},
            {".swf", "application/x-shockwave-flash"},
            {".t", "application/x-troff"},
            {".tar", "application/x-tar"},
            {".tcl", "application/x-tcl"},
            {".testrunconfig", "application/xml"},
            {".testsettings", "application/xml"},
            {".tex", "application/x-tex"},
            {".texi", "application/x-texinfo"},
            {".texinfo", "application/x-texinfo"},
            {".tgz", "application/x-compressed"},
            {".thmx", "application/vnd.ms-officetheme"},
            {".thn", "application/octet-stream"},
            {".tif", "image/tiff"},
            {".tiff", "image/tiff"},
            {".tlh", "text/plain"},
            {".tli", "text/plain"},
            {".toc", "application/octet-stream"},
            {".tr", "application/x-troff"},
            {".trm", "application/x-msterminal"},
            {".trx", "application/xml"},
            {".ts", "video/vnd.dlna.mpeg-tts"},
            {".tsv", "text/tab-separated-values"},
            {".ttf", "application/octet-stream"},
            {".tts", "video/vnd.dlna.mpeg-tts"},
            {".txt", "text/plain"},
            {".u32", "application/octet-stream"},
            {".uls", "text/iuls"},
            {".user", "text/plain"},
            {".ustar", "application/x-ustar"},
            {".vb", "text/plain"},
            {".vbdproj", "text/plain"},
            {".vbk", "video/mpeg"},
            {".vbproj", "text/plain"},
            {".vbs", "text/vbscript"},
            {".vcf", "text/x-vcard"},
            {".vcproj", "Application/xml"},
            {".vcs", "text/plain"},
            {".vcxproj", "Application/xml"},
            {".vddproj", "text/plain"},
            {".vdp", "text/plain"},
            {".vdproj", "text/plain"},
            {".vdx", "application/vnd.ms-visio.viewer"},
            {".vml", "text/xml"},
            {".vscontent", "application/xml"},
            {".vsct", "text/xml"},
            {".vsd", "application/vnd.visio"},
            {".vsi", "application/ms-vsi"},
            {".vsix", "application/vsix"},
            {".vsixlangpack", "text/xml"},
            {".vsixmanifest", "text/xml"},
            {".vsmdi", "application/xml"},
            {".vspscc", "text/plain"},
            {".vss", "application/vnd.visio"},
            {".vsscc", "text/plain"},
            {".vssettings", "text/xml"},
            {".vssscc", "text/plain"},
            {".vst", "application/vnd.visio"},
            {".vstemplate", "text/xml"},
            {".vsto", "application/x-ms-vsto"},
            {".vsw", "application/vnd.visio"},
            {".vsx", "application/vnd.visio"},
            {".vtx", "application/vnd.visio"},
            {".wav", "audio/wav"},
            {".wave", "audio/wav"},
            {".wax", "audio/x-ms-wax"},
            {".wbk", "application/msword"},
            {".wbmp", "image/vnd.wap.wbmp"},
            {".wcm", "application/vnd.ms-works"},
            {".wdb", "application/vnd.ms-works"},
            {".wdp", "image/vnd.ms-photo"},
            {".webarchive", "application/x-safari-webarchive"},
            {".webtest", "application/xml"},
            {".wiq", "application/xml"},
            {".wiz", "application/msword"},
            {".wks", "application/vnd.ms-works"},
            {".WLMP", "application/wlmoviemaker"},
            {".wlpginstall", "application/x-wlpg-detect"},
            {".wlpginstall3", "application/x-wlpg3-detect"},
            {".wm", "video/x-ms-wm"},
            {".wma", "audio/x-ms-wma"},
            {".wmd", "application/x-ms-wmd"},
            {".wmf", "application/x-msmetafile"},
            {".wml", "text/vnd.wap.wml"},
            {".wmlc", "application/vnd.wap.wmlc"},
            {".wmls", "text/vnd.wap.wmlscript"},
            {".wmlsc", "application/vnd.wap.wmlscriptc"},
            {".wmp", "video/x-ms-wmp"},
            {".wmv", "video/x-ms-wmv"},
            {".wmx", "video/x-ms-wmx"},
            {".wmz", "application/x-ms-wmz"},
            {".wpl", "application/vnd.ms-wpl"},
            {".wps", "application/vnd.ms-works"},
            {".wri", "application/x-mswrite"},
            {".wrl", "x-world/x-vrml"},
            {".wrz", "x-world/x-vrml"},
            {".wsc", "text/scriptlet"},
            {".wsdl", "text/xml"},
            {".wvx", "video/x-ms-wvx"},
            {".x", "application/directx"},
            {".xaf", "x-world/x-vrml"},
            {".xaml", "application/xaml+xml"},
            {".xap", "application/x-silverlight-app"},
            {".xbap", "application/x-ms-xbap"},
            {".xbm", "image/x-xbitmap"},
            {".xdr", "text/plain"},
            {".xht", "application/xhtml+xml"},
            {".xhtml", "application/xhtml+xml"},
            {".xla", "application/vnd.ms-excel"},
            {".xlam", "application/vnd.ms-excel.addin.macroEnabled.12"},
            {".xlc", "application/vnd.ms-excel"},
            {".xld", "application/vnd.ms-excel"},
            {".xlk", "application/vnd.ms-excel"},
            {".xll", "application/vnd.ms-excel"},
            {".xlm", "application/vnd.ms-excel"},
            {".xls", "application/vnd.ms-excel"},
            {".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
            {".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"},
            {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            {".xlt", "application/vnd.ms-excel"},
            {".xltm", "application/vnd.ms-excel.template.macroEnabled.12"},
            {".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
            {".xlw", "application/vnd.ms-excel"},
            {".xml", "text/xml"},
            {".xmta", "application/xml"},
            {".xof", "x-world/x-vrml"},
            {".XOML", "text/plain"},
            {".xpm", "image/x-xpixmap"},
            {".xps", "application/vnd.ms-xpsdocument"},
            {".xrm-ms", "text/xml"},
            {".xsc", "application/xml"},
            {".xsd", "text/xml"},
            {".xsf", "text/xml"},
            {".xsl", "text/xml"},
            {".xslt", "text/xml"},
            {".xsn", "application/octet-stream"},
            {".xss", "application/xml"},
            {".xtp", "application/octet-stream"},
            {".xwd", "image/x-xwindowdump"},
            {".z", "application/x-compress"},
            {".zip", "application/x-zip-compressed"},
            #endregion
            };

            string contentType = "application/octet-stream";
            try
            {
                contentType = _mappings[fileextension];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return contentType;
        }

        private static string AllowFile(SystemConfigFile sysConfig, string customType = "", string prgCode = "")
        {
            var allowType = new List<string>();
            if (sysConfig != null && sysConfig.Details != null && sysConfig.Details.Count > 0)
            {
                var nSysConfig = sysConfig.Details;
                if (!prgCode.IsNullOrEmpty())
                {
                    var confByPrgCode = nSysConfig.Where(m => m.PRG_CODE == prgCode).ToList();
                    if (confByPrgCode != null && confByPrgCode.Count > 0)
                    {
                        nSysConfig = confByPrgCode;
                    }
                }
                if (!customType.IsNullOrEmpty())
                {
                    var types = customType.Replace(".", "").Split(',');
                    var conf = nSysConfig.Where(m => types.Contains(m.FILE_TYPE)).ToList();
                    if (conf.Count > 0)
                    {
                        nSysConfig = conf;
                    }
                }
                foreach (var item in nSysConfig.Distinct())
                {
                    var type = string.Empty;
                    if (item.FILE_TYPE.IndexOf('.') == -1)
                    {
                        type = ContentType("." + item.FILE_TYPE);
                    }
                    else
                    {
                        type = ContentType(item.FILE_TYPE);
                    }
                    if (!allowType.Contains(type))
                    {
                        allowType.Add(type);
                    }
                }
            }
            return string.Join(",", allowType);
        }

        private static ColumnsType StrToColumnsType(string text)
        {
            ColumnsType Type = ColumnsType.None;

            if (text.IsNullOrEmpty() || text == "None")
            {
            }
            else if (text == "Date")
            {
                Type = ColumnsType.Date;
            }
            else if (text == "NumberFormat")
            {
                Type = ColumnsType.NumberFormat;
            }

            return Type;
        }
        private static string StrToColumnsTextAlign(string text)
        {
            var align = ColumnsTextAlign.None;

            if (text == "NumberFormat")
            {
                align = ColumnsTextAlign.Right;
            }
            else if (text == "Date")
            {
                align = ColumnsTextAlign.Center;
            }

            return (align != ColumnsTextAlign.None ? align.GetDescription() : "");
        }
        #endregion

        #region GetReportGroupDetailScript
        public static MvcHtmlString GetReportGroupDetailScriptFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, VSMMultiSelectConfig config)
        {

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string label = GetControlLabel(metadata, expressionText);
            string name = GetControlName(html, expressionText);
            var sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("var gridAdd" + name + " = {};");

            #region OnClick_btnAdd
            sb.AppendLine("function OnClick_btnAdd" + name + "(e) {");
            sb.AppendLine("var ctrl = $(this).closest('.input-group').find('select option:selected');");
            sb.AppendLine("if (ctrl.val()!=\"\") {");
            sb.AppendLine("bindGridAdd" + name + "();");
            sb.Append("$(\"#mdTitle" + name + "\").text(\"");
            sb.Append(Translation.CenterLang.Center.ReportGroupDetail + " : ");
            sb.AppendLine("\"+ctrl.text());");
            sb.AppendLine("$(\"#mdAdd" + name + "\").modal();");
            sb.AppendLine("}");
            sb.AppendLine("else { ");
            sb.AppendLine("var content = '<div class=\"alert alert-danger alert-dismissable alert-dangernew\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\"><i class=\"ace-icon fa fa-times\"></i></button><h2>" + Translation.CenterLang.Center.PleaseSelectReportGroup + "</h2></div>';");
            sb.AppendLine("$(\"#notification\").html(content).fadeTo(2000, 500);");
            sb.AppendLine("}");
            sb.AppendLine("return false;");
            sb.AppendLine("}");
            #endregion
            #region bindGridAdd
            sb.AppendLine("function bindGridAdd" + name + "() {");
            sb.AppendLine("var result = {");
            sb.Append("keySource:\"");
            sb.Append(config.KeySource + "\"");
            if (config.Parameters != null && config.Parameters.Count > 0)
            {
                sb.AppendLine(",");
                sb.Append("extraParam:'{");
                for (int i = 0; i < config.Parameters.Count; i++)
                {
                    sb.Append(config.Parameters[i].Name + ":");
                    if (config.Parameters[i].Type == VSMParameterType.ByModelData)
                    {
                        sb.Append("\"" + config.Parameters[i].Value + "\"");
                    }
                    else
                    {
                        sb.Append("\"'+$(\"#" + config.Parameters[i].Value + "\").val()+'\"");
                    }
                    if (i != config.Parameters.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
                sb.AppendLine("}'");
            }
            sb.AppendLine("}");

            sb.AppendLine("gridAdd" + name + " = $('#gridAdd" + name + "').DataTable({");
            sb.AppendLine("\"deferRender\": true,");
            sb.AppendLine("\"paging\": true,");
            sb.AppendLine("\"processing\": true,");
            sb.AppendLine("\"destroy\": !$.isEmptyObject(gridAdd" + name + "),");
            sb.Append("\"order\": [");
            var columns = config.Columns.Where(m => m.visible != "false").ToList();
            string order = string.Empty;
            for (int i = 1; i < columns.Count; i++)
            {
                if (config.Columns[i].orderable)
                {
                    order += "[" + i + ", \"asc\"],";
                }
            }
            sb.Append(order.TrimEnd(','));
            sb.AppendLine("],");
            sb.AppendLine("\"ajax\": {");
            var urlAdd = new UrlHelper(html.ViewContext.RequestContext);
            sb.AppendLine("url: \"" + urlAdd.Action("GetDataMultiSelect", "Autocomplete", new
            {
                Area = "Ux"
            }) + "\",");
            sb.AppendLine("type: \"POST\",");
            sb.AppendLine("data: result");
            //sb.AppendLine("\"dataSrc\": \"rows\"");            
            sb.AppendLine("},");
            //sb.AppendLine("\"scrollX\": true,");
            sb.AppendLine("\"dom\": \"<'row'<'col-sm-6'<'pull-left'f>><'col-sm-6'<'pull-right'l>>><'row'<'col-sm-12'tr>><'row'<'col-sm-5'i><'col-sm-7'p>>\",");
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
            sb.Append("\"infoFiltered\": \"");
            sb.Append(Translation.CenterLang.Center.gridInfoFiltered);
            sb.AppendLine("\",");
            sb.Append("\"loadingRecords\": \"");
            sb.Append(Translation.CenterLang.Center.gridLoadingRecords);
            sb.AppendLine("\",");
            sb.Append("\"processing\": \"");
            sb.Append(Translation.CenterLang.Center.gridProcessing);
            sb.AppendLine("\",");
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

            sb.AppendLine("\"columns\": [");
            if (columns != null)
            {
                int c = 0;
                foreach (var dtC in columns)
                {
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
                                else
                                {
                                    sb.Append("\"" + prop.Name + "\":\"" + value + "\"");
                                }
                            }
                            else if (prop.Name == "data")
                            {
                                sb.Append("\"" + prop.Name + "\":null");
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
                                sb.AppendLine(",");
                            }
                        }
                    }
                    sb.Append("}");

                    if (c != columns.Count)
                    {
                        sb.AppendLine(",");
                    }
                }
            }

            sb.AppendLine("]");
            sb.AppendLine("});");

            sb.AppendLine("}");
            #endregion
            #region ready
            sb.AppendLine("$(document).ready(function () {");
            sb.AppendLine("$(\"#btnAdd" + name + "\").click(OnClick_btnAdd" + name + ");");
            sb.AppendLine("});");
            #endregion

            sb.AppendLine("</script>");
            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region GridEditable

        public static MvcHtmlString GridEditableFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool IsReadOnly = false, bool isRequired = false)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string label = GetControlLabel(metadata, expressionText);
            string name = GetControlName(html, expressionText);

            var sb = new StringBuilder();
            if (!IsReadOnly)
            {
                sb.AppendLine("<div class=\"gridIncludeToolbar\">");
                sb.AppendLine("<div class=\"gridToolbar\">");
                sb.Append("<a href=\"javascript:void(0)\" id=\"btnAdd" + name + "\" data-toggle=\"tooltip\" title=\"");
                sb.Append(Translation.CenterLang.Center.Add);
                sb.AppendLine("\">");
                sb.AppendLine("<i class=\"ace-icon fa fa-plus-circle align-top bigger-130 green\"></i>");
                sb.AppendLine("</a>");
                sb.Append("<a href=\"javascript:void(0)\" id=\"btnDelete" + name + "\" data-toggle=\"tooltip\" title=\"");
                sb.Append(Translation.CenterLang.Center.Delete);
                sb.AppendLine("\">");
                sb.AppendLine("<i class=\"ace-icon fa fa-trash align-top bigger-130 red\"></i>");
                sb.AppendLine("</a>");
                sb.AppendLine("</div>");
            }
            sb.Append("<table id=\"Grid" + name + "\" class=\"table table-striped table-bordered table-hover\" data-tabletype=\"editable\"");
            if (isRequired)
            {
                sb.Append(" data-isrequired=\"true\"");
            }
            sb.AppendLine("></table>");
            if (!IsReadOnly)
            {
                sb.AppendLine("</div>");
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString GridEditableScriptFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, GridEditableConfig config, params GridColumn[] columns)
        {
            if (config == null)
            {
                config = new GridEditableConfig();
            }
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string label = GetControlLabel(metadata, expressionText);
            string name = GetControlName(html, expressionText);
            var sysConfig = html.ViewContext.HttpContext.Session["SYS_ConfigFile"] as SystemConfigFile;

            var url = new UrlHelper(html.ViewContext.RequestContext);


            var sb = new StringBuilder();

            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("var Grid" + name + " = {};");
            var existsKey = columns.Where(m => m.IsKey);
            var editCol = columns.Where(m => m.IsEditable).ToList();
            var colDate = columns.Where(m => m != null && (m.type == ColumnsType.Date || m.type == ColumnsType.DateTime)).ToList();
            var colDateEdit = colDate.Where(m => m != null && m.visible != "false" && m.IsEditable && (m.type == ColumnsType.Date || m.type == ColumnsType.DateTime)).ToList();
            var colFile = columns.Where(m => m != null && (m.type == ColumnsType.File || m.type == ColumnsType.FileMultiple)).ToList();
            var colFileEdit = colFile.Where(m => m.visible != "false" && m.IsEditable).ToList();
            int startIdx = 0;
            var cols = columns.Where(m => m != null && m.visible != "false").ToList();
            bool existsAutocomplete = false;

            var sbCols = new StringBuilder();
            var sbCtrl = new StringBuilder();
            var sbRowCtrl = new StringBuilder();
            var sbFileEvent = new StringBuilder();
            var sbRowBindFile = new StringBuilder();

            #region Colunms
            if (config.VisibleCheckBox && !config.IsReadOnly)
            {
                sbCols.AppendLine("{");
                sbCols.AppendLine("data: null,");
                sbCols.AppendLine("defaultContent: '',");
                sbCols.AppendLine("width: '2%',");
                sbCols.AppendLine("className: 'dt-head-center select-checkbox',");
                sbCols.AppendLine("orderable: false,");
                sbCols.AppendLine("title: '<input type=\"checkbox\" name=\"selectAllGrid" + name + "\" id=\"selectAllGrid" + name + "\" class=\"checkbox\">'");
                sbCols.Append("}");
                startIdx++;
            }

            var orderIdx = new List<int>();
            if (cols != null)
            {
                if (startIdx > 0)
                {
                    sbCols.AppendLine(",");
                }
                bool incStartIdx = true;
                int c = 0;
                foreach (var dtC in cols)
                {
                    dtC.IsOrderColumn = false;
                    if (incStartIdx && !dtC.IsButtonColumn)
                    {
                        incStartIdx = false;
                    }
                    if (incStartIdx)
                    {
                        startIdx++;
                    }
                    else
                    {
                        if (dtC.IsEditable)
                        {
                            sbCtrl.Append("case ");
                            sbCtrl.Append(c + startIdx);
                            sbCtrl.AppendLine(" :");
                            sbCtrl.AppendLine("var options = {");
                            sbCtrl.Append("name: \"");
                            sbCtrl.Append(name);
                            sbCtrl.AppendLine("\",");
                            sbCtrl.Append("colName: \"");
                            sbCtrl.Append(dtC.data);
                            sbCtrl.AppendLine("\",");
                            sbCtrl.Append("value: value");
                            if (dtC.EditableReadOnly)
                            {
                                sbCtrl.AppendLine(",");
                                sbCtrl.Append("readonly: true");
                            }
                            if (dtC.IsRequired)
                            {
                                sbCtrl.AppendLine(",");
                                sbCtrl.Append("required: true");
                            }
                            if (dtC.MaxLength != null && dtC.MaxLength > 0)
                            {
                                sbCtrl.AppendLine(",");
                                sbCtrl.Append("maxLength:");
                                sbCtrl.Append(dtC.MaxLength);
                            }
                            if (dtC.type == ColumnsType.Date || dtC.type == ColumnsType.DateTime)
                            {
                                if (dtC.CustomTrigger)
                                {
                                    sbCtrl.AppendLine(",");
                                    sbCtrl.AppendLine("customTrigger:true");
                                }
                                sbCtrl.AppendLine("};");
                                if (!dtC.CustomOptionEditable.IsNullOrEmpty())
                                {
                                    sbCtrl.Append("$.extend(options,");
                                    sbCtrl.Append(dtC.CustomOptionEditable);
                                    sbCtrl.AppendLine("(colIndex, value, mode, rowIndex,dataRow,options));");
                                }
                                sbCtrl.AppendLine("return getInputDate(options);");
                            }
                            else if (dtC.type == ColumnsType.Select)
                            {
                                sbCtrl.AppendLine(",");
                                sbCtrl.Append("options: ");
                                if (dtC.SelectOptions != null)
                                {
                                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(dtC.SelectOptions);
                                    sbCtrl.Append(json);
                                }
                                else
                                {
                                    sbCtrl.Append("[]");
                                }
                                sbCtrl.AppendLine("};");
                                if (!dtC.CustomOptionEditable.IsNullOrEmpty())
                                {
                                    sbCtrl.Append("$.extend(options,");
                                    sbCtrl.Append(dtC.CustomOptionEditable);
                                    sbCtrl.AppendLine("(colIndex, value, mode, rowIndex,dataRow,options));");
                                }
                                sbCtrl.AppendLine("return getInputSelect(options);");
                            }
                            else if (dtC.type == ColumnsType.File || dtC.type == ColumnsType.FileMultiple)
                            {

                                var newRender = "function (data, type, full, meta) {" + Environment.NewLine;
                                newRender += "tag = '<table class=\"table table-striped table-bordered table-hover gridInGrid\">';" + Environment.NewLine;
                                newRender += "tag += '<thead>';" + Environment.NewLine;
                                newRender += "tag += '<tr>';" + Environment.NewLine;
                                newRender += "tag += '<th class=\"dt-head-center dt-head-nowrap\" style=\"width: 3%;\"></th>';" + Environment.NewLine;
                                newRender += "tag += '<th class=\"dt-head-center dt-head-nowrap\" style=\"width: 59%;\">" + Translation.CenterLang.Center.FileName + "</th>';" + Environment.NewLine;
                                newRender += "tag += '<th class=\"dt-head-center dt-head-nowrap\" style=\"width: 9%;\">" + Translation.CenterLang.Center.FileSize + "</th>';";
                                newRender += "tag += '<th class=\"dt-head-center dt-head-nowrap\" style=\"width: 15%;\">" + Translation.CenterLang.Center.FileDateModified + "</th>';" + Environment.NewLine;
                                newRender += "tag += '<th class=\"dt-head-center dt-head-nowrap\" style=\"width: 15%;\">" + Translation.CenterLang.Center.FileUploadDate + "</th>';" + Environment.NewLine;
                                newRender += "tag += '</tr>';" + Environment.NewLine;
                                newRender += "tag += '</thead>';" + Environment.NewLine;
                                newRender += "if (full." + dtC.data + ".length > 0 && $.isArray(full." + dtC.data + ")) {" + Environment.NewLine;
                                newRender += "tag += '<tbody>';" + Environment.NewLine;
                                newRender += "$.each(full." + dtC.data + ", function(i, item) {" + Environment.NewLine;
                                newRender += "tag += '<tr>';" + Environment.NewLine;
                                newRender += "tag += '<td class=\"dt-body-center\">';" + Environment.NewLine;
                                newRender += "if (!$.isNullOrEmpty(item.ID)) {" + Environment.NewLine;
                                newRender += "tag += '<a data-toggle=\"tooltip\" data-placement=\"right\" target=\"_blank\" title=\"";
                                newRender += Translation.CenterLang.Center.Download;
                                newRender += "\" href=\\\"";
                                newRender += url.Action("Download", "File", new { Area = "Ux" });
                                newRender += "?ID=' + toUrlString(item.ID);" + Environment.NewLine;
                                newRender += "tag += '\\\" ><i class=\\\"ace-icon fa fa-download bigger-130\\\"></i></a>';" + Environment.NewLine;
                                newRender += "}" + Environment.NewLine;
                                newRender += "tag += '</td>';" + Environment.NewLine;
                                newRender += "tag += '<td class=\"dt-body-left\">' + ($.isNullOrEmpty(item.FILE_NAME) ? '' : item.FILE_NAME) +'</td>';" + Environment.NewLine;
                                newRender += "tag += '<td class=\"dt-body-rigth\">' + ($.isNullOrEmpty(item.FILE_SIZE) ? '' : toNumberFormat(item.FILE_SIZE, \"#,###,##0.####\")) +'</td>';" + Environment.NewLine;
                                newRender += "tag += '<td class=\"dt-body-center\">' + ($.isNullOrEmpty(item.FILE_DATE) ? '' : jsonDateToFormat(item.FILE_DATE, \"DD/MM/YYYY HH:mm:ss\")) +'</td>';" + Environment.NewLine;
                                newRender += "tag += '<td class=\"dt-body-center\">' + ($.isNullOrEmpty(item.CRET_DATE) ? '<span class=\"text-danger\">" + Translation.CenterLang.Center.FileWaitingUpload + "</span>' : jsonDateToFormat(item.FILE_DATE, \"DD/MM/YYYY HH:mm:ss\")) +'</td>';" + Environment.NewLine;
                                newRender += "tag += '</tr>';" + Environment.NewLine;
                                newRender += "});" + Environment.NewLine;
                                newRender += "tag += '</tbody>';" + Environment.NewLine;
                                newRender += "}" + Environment.NewLine;
                                newRender += "tag += '</table>';" + Environment.NewLine;
                                newRender += "return tag;" + Environment.NewLine;
                                newRender += "}";

                                dtC.render = newRender;

                                var allowFile = AllowFile(sysConfig, dtC.FileType, config.PRG_CODE);
                                sbCtrl.AppendLine(",");
                                if (dtC.type == ColumnsType.FileMultiple)
                                {
                                    sbCtrl.AppendLine("multiple:true,");
                                }
                                sbCtrl.AppendLine("mode:mode,");
                                sbCtrl.Append("allowType:'");
                                sbCtrl.Append(allowFile);
                                sbCtrl.AppendLine("',");
                                if (!allowFile.IsNullOrEmpty())
                                {
                                    var nSysConfig = sysConfig.Details;
                                    if (!config.PRG_CODE.IsNullOrEmpty())
                                    {
                                        var confByPrgCode = nSysConfig.Where(m => m.PRG_CODE == config.PRG_CODE).ToList();
                                        if (confByPrgCode != null && confByPrgCode.Count > 0)
                                        {
                                            nSysConfig = confByPrgCode;
                                        }
                                    }
                                    if (!dtC.FileType.IsNullOrEmpty())
                                    {
                                        var ctTypes = dtC.FileType.Replace(".", "").Split(',');
                                        var conf = nSysConfig.Where(m => ctTypes.Contains(m.FILE_TYPE)).ToList();
                                        if (conf.Count > 0)
                                        {
                                            nSysConfig = conf;
                                        }
                                    }
                                    var types = new List<string>();
                                    string tplType = ".{0}(&le; {1}MB)";
                                    foreach (var item in nSysConfig)
                                    {
                                        types.Add(string.Format(tplType, item.FILE_TYPE.Replace(".", ""), item.FILE_SIZE));
                                    }
                                    var nType = string.Join(",", types);
                                    sbCtrl.Append("allowTypeHelp:'");
                                    sbCtrl.Append(Translation.CenterLang.Center.SupportFileType.Replace("{tplFileType}", nType));
                                    sbCtrl.AppendLine("',");
                                }
                                sbCtrl.AppendLine("rowIndex:rowIndex");
                                sbCtrl.AppendLine("};");

                                if (!dtC.CustomOptionEditable.IsNullOrEmpty())
                                {
                                    sbCtrl.Append("$.extend(options,");
                                    sbCtrl.Append(dtC.CustomOptionEditable);
                                    sbCtrl.AppendLine("(colIndex, value, mode, rowIndex,dataRow,options));");
                                }
                                sbCtrl.AppendLine("return getInputFile(options);");

                                sbFileEvent.AppendLine("var Grid" + name + dtC.data + " = {};");

                                #region OnClick_btnAddFile
                                sbFileEvent.AppendLine("function OnClick_btnAdd" + name + dtC.data + "(e) {");
                                sbFileEvent.AppendLine("$.clearValidError('" + name + dtC.data + "');");
                                sbFileEvent.AppendLine("$(\"#f" + name + dtC.data + "\").click();");
                                sbFileEvent.AppendLine("return false;");
                                sbFileEvent.AppendLine("}");
                                #endregion

                                #region OnChange_f
                                sbFileEvent.AppendLine("function OnChange_f" + name + dtC.data + "(e) {");
                                sbFileEvent.AppendLine("var f = this;");
                                sbFileEvent.Append("if (f.files.length > 0) {");
                                sbFileEvent.AppendLine("var allowFile = [");
                                if (sysConfig.Details != null && sysConfig.Details.Count > 0)
                                {
                                    var nSysConfig = sysConfig.Details;
                                    if (!config.PRG_CODE.IsNullOrEmpty())
                                    {
                                        var confByPrgCode = nSysConfig.Where(m => m.PRG_CODE == config.PRG_CODE).ToList();
                                        if (confByPrgCode != null && confByPrgCode.Count > 0)
                                        {
                                            nSysConfig = confByPrgCode;
                                        }
                                    }
                                    if (!dtC.FileType.IsNullOrEmpty())
                                    {
                                        var types = dtC.FileType.Replace(".", "").Split(',');
                                        var conf = nSysConfig.Where(m => types.Contains(m.FILE_TYPE)).ToList();
                                        if (conf.Count > 0)
                                        {
                                            nSysConfig = conf;
                                        }
                                    }
                                    var sci = 1;
                                    foreach (var sysConf in nSysConfig)
                                    {
                                        sbFileEvent.Append("{");
                                        sbFileEvent.Append("type:'");
                                        sbFileEvent.Append(sysConf.FILE_TYPE.Replace(".", ""));
                                        sbFileEvent.Append("',");
                                        sbFileEvent.Append("size:");
                                        sbFileEvent.Append(sysConf.FILE_SIZE.AsString());

                                        if (sci != nSysConfig.Count)
                                        {
                                            sbFileEvent.AppendLine("},");
                                        }
                                        else
                                        {
                                            sbFileEvent.AppendLine("}");
                                        }
                                        sci++;
                                    }
                                }
                                sbFileEvent.AppendLine("];");
                                if (dtC.type == ColumnsType.FileMultiple)
                                {
                                    sbFileEvent.AppendLine("var oldData = Grid" + name + dtC.data + ".rows().data().toArray();");
                                    sbFileEvent.Append("if ((oldData.length + f.files.length) <= ");
                                    sbFileEvent.Append(sysConfig.NO_OF_FILE);
                                    sbFileEvent.AppendLine(") {");
                                    sbFileEvent.AppendLine("var totalSize = 0;");
                                    sbFileEvent.AppendLine("var fileExists = [];");
                                    sbFileEvent.AppendLine("$.each(oldData, function (j, obj){");
                                    sbFileEvent.AppendLine("if(obj.FILE_NAME!=null&&obj.FILE_NAME!=\"\"){");
                                    sbFileEvent.AppendLine("fileExists.push(obj.FILE_NAME);");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("totalSize+=toNumber(obj.FILE_SIZE);");
                                    sbFileEvent.AppendLine("});");
                                    sbFileEvent.AppendLine("var nData = [];");
                                    sbFileEvent.AppendLine("var fileDup = [];");
                                    sbFileEvent.AppendLine("var fileNotAllowType = [];");
                                    sbFileEvent.AppendLine("var fileNotAllowSize = [];");
                                    sbFileEvent.AppendLine("$.each(f.files, function (i, file) {");
                                    sbFileEvent.AppendLine("var exists = false;");
                                    sbFileEvent.AppendLine("if ($.inArray(file.name, fileExists) != -1) {");
                                    sbFileEvent.AppendLine("exists = true;");
                                    sbFileEvent.AppendLine("if ($.inArray(file.name, fileDup) == -1) {");
                                    sbFileEvent.AppendLine("fileDup.push(file.name);");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("var ext = file.name.split('.').pop();");
                                    sbFileEvent.AppendLine("var size = file.size / 1048576;");
                                    sbFileEvent.AppendLine("var allowType = false;");
                                    sbFileEvent.AppendLine("var allowSize = false;");
                                    sbFileEvent.AppendLine("$.each(allowFile, function (tIdx, obj) {");
                                    sbFileEvent.AppendLine("if (obj.type == ext) {");
                                    sbFileEvent.AppendLine("allowType = true;");
                                    sbFileEvent.AppendLine("if (size <= obj.size) {");
                                    sbFileEvent.AppendLine("allowSize = true;");
                                    sbFileEvent.AppendLine("} else {");
                                    sbFileEvent.AppendLine("fileNotAllowSize.push(obj);");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("return false;");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("});");
                                    sbFileEvent.AppendLine("if (!allowType && $.inArray(ext, fileNotAllowType) == -1) {");
                                    sbFileEvent.AppendLine("fileNotAllowType.push(ext);");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("if (!exists && allowType && allowSize) {");
                                    sbFileEvent.AppendLine("var info = {};");
                                    sbFileEvent.AppendLine("info[\"FILE_NAME\"] = file.name;");
                                    sbFileEvent.AppendLine("info[\"File\"] = file;");
                                    sbFileEvent.AppendLine("info[\"FILE_SIZE\"] = size;");
                                    sbFileEvent.AppendLine("info[\"FILE_DATE\"] = '/Date('+file.lastModifiedDate.getTime()+')/';");
                                    sbFileEvent.AppendLine("nData.push(info);");
                                    sbFileEvent.AppendLine("totalSize += toNumber(size);");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("});");
                                    sbFileEvent.Append("if (nData.length > 0 && fileDup.length == 0 && fileNotAllowType.length == 0 && fileNotAllowSize.length == 0 && totalSize<=");
                                    sbFileEvent.Append(sysConfig.FILE_SIZE);
                                    sbFileEvent.AppendLine(") {");
                                    sbFileEvent.AppendLine("Grid" + name + dtC.data + ".rows.add(nData).draw();");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("else if (fileNotAllowType.length != 0) {");
                                    sbFileEvent.Append("$(\"<span id =\\\"validError" + name + dtC.data + "\\\" class=\\\"text-danger\\\">");
                                    sbFileEvent.Append(Translation.CenterLang.Center.FileNotExistsSystemConfig);
                                    sbFileEvent.AppendLine("</span>\").insertBefore(\"#f" + name + dtC.data + "\");");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("else if (fileDup.length != 0) {");
                                    sbFileEvent.Append("$(\"<span id =\\\"validError" + name + dtC.data + "\\\" class=\\\"text-danger\\\">");
                                    sbFileEvent.Append(Translation.CenterLang.Center.FileAlreadyExists);
                                    sbFileEvent.AppendLine("</span>\").insertBefore(\"#f" + name + dtC.data + "\");");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("else if (fileNotAllowSize.length != 0) {");
                                    sbFileEvent.Append("$(\"<span id =\\\"validError" + name + dtC.data + "\\\" class=\\\"text-danger\\\">");
                                    sbFileEvent.Append(Translation.CenterLang.Center.FileMaximumSizeLimit);
                                    sbFileEvent.AppendLine("</span>\").insertBefore(\"#f" + name + dtC.data + "\");");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("else {");
                                    sbFileEvent.Append("$(\"<span id =\\\"validError" + name + dtC.data + "\\\" class=\\\"text-danger\\\">");
                                    sbFileEvent.Append(Translation.CenterLang.Center.AttachFileMaximumSizeLimit);
                                    sbFileEvent.AppendLine("</span>\").insertBefore(\"#f" + name + dtC.data + "\");");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("else {");
                                    sbFileEvent.Append("$(\"<span id =\\\"validError" + name + dtC.data + "\\\" class=\\\"text-danger\\\">");
                                    sbFileEvent.Append(Translation.CenterLang.Center.AttachFileMaximumLimit);
                                    sbFileEvent.AppendLine("</span>\").insertBefore(\"#f" + name + dtC.data + "\");");
                                    sbFileEvent.AppendLine("}");
                                }
                                else
                                {
                                    sbFileEvent.AppendLine("Grid" + name + dtC.data + ".rows().remove().draw();");
                                    sbFileEvent.AppendLine("var info = {};");
                                    sbFileEvent.AppendLine("$.each(f.files, function (i, file) {");
                                    sbFileEvent.AppendLine("var ext = file.name.split('.').pop();");
                                    sbFileEvent.AppendLine("var size = file.size / 1048576;");
                                    sbFileEvent.AppendLine("var allowType = false;");
                                    sbFileEvent.AppendLine("var allowSize = false;");
                                    sbFileEvent.AppendLine("$.each(allowFile, function (tIdx, obj) {");
                                    sbFileEvent.AppendLine("if (obj.type == ext) {");
                                    sbFileEvent.AppendLine("allowType = true;");
                                    sbFileEvent.AppendLine("if (size <= obj.size) {");
                                    sbFileEvent.AppendLine("allowSize = true;");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("return false;");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("});");
                                    sbFileEvent.AppendLine("info[\"FILE_NAME\"] = file.name;");
                                    sbFileEvent.AppendLine("info[\"File\"] = file;");
                                    sbFileEvent.AppendLine("info[\"FILE_SIZE\"] = size;");
                                    sbFileEvent.AppendLine("info[\"FILE_DATE\"] = '/Date('+file.lastModifiedDate.getTime()+')/';");
                                    sbFileEvent.AppendLine("if(allowType && allowSize){");
                                    sbFileEvent.AppendLine("Grid" + name + dtC.data + ".row.add(info).draw();");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("else if (!allowType) {");
                                    sbFileEvent.Append("$(\"<span id =\\\"validError" + name + dtC.data + "\\\" class=\\\"text-danger\\\">");
                                    sbFileEvent.Append(Translation.CenterLang.Center.FileNotExistsSystemConfig);
                                    sbFileEvent.AppendLine("</span>\").insertBefore(\"#f" + name + dtC.data + "\");");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("else if (!allowSize) {");
                                    sbFileEvent.Append("$(\"<span id =\\\"validError" + name + dtC.data + "\\\" class=\\\"text-danger\\\">");
                                    sbFileEvent.Append(Translation.CenterLang.Center.FileMaximumSizeLimit);
                                    sbFileEvent.AppendLine("</span>\").insertBefore(\"#f" + name + dtC.data + "\");");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("});");
                                }
                                sbFileEvent.AppendLine("var oldCtrl = $(\"#f" + name + dtC.data + "\");");
                                sbFileEvent.Append("var newCtrl = $('<input type=\"file\" id=\"f" + name + dtC.data + "\" name=\"" + dtC.data + "\" style=\"display:none;\" data-vsmctrl=\"true\"");
                                if (!allowFile.IsNullOrEmpty())
                                {
                                    sbFileEvent.Append(" accept=\"");
                                    sbFileEvent.Append(allowFile);
                                    sbFileEvent.Append("\"");
                                }
                                if (dtC.type == ColumnsType.FileMultiple)
                                {
                                    sbFileEvent.Append(" data-multiple=\"true\" multiple");
                                }
                                sbFileEvent.AppendLine("/>');");
                                sbFileEvent.AppendLine("var isrequired = oldCtrl.data(\"isrequired\");");
                                sbFileEvent.AppendLine("if(isrequired){");
                                sbFileEvent.AppendLine("newCtrl.data(\"isrequired\",isrequired);");
                                sbFileEvent.AppendLine("}");
                                sbFileEvent.AppendLine("var mode = oldCtrl.data(\"mode\");");
                                sbFileEvent.AppendLine("if (!isNullOrEmpty(mode)) {");
                                sbFileEvent.AppendLine("newCtrl.data(\"mode\", mode);");
                                sbFileEvent.AppendLine("}");
                                sbFileEvent.AppendLine("var rowIndex = oldCtrl.data(\"rowindex\");");
                                sbFileEvent.AppendLine("if (!isNullOrEmpty(rowIndex)) {");
                                sbFileEvent.AppendLine("newCtrl.data(\"rowindex\", rowIndex);");
                                sbFileEvent.AppendLine("}");
                                sbFileEvent.AppendLine("newCtrl.change(OnChange_f" + name + dtC.data + ");");
                                sbFileEvent.AppendLine("oldCtrl.replaceWith(newCtrl);");
                                //if (!onChangeName.IsNullOrEmpty())
                                //{
                                //    sbFileEvent.Append(onChangeName);
                                //    sbFileEvent.AppendLine("();");
                                //}
                                sbFileEvent.AppendLine("}");
                                sbFileEvent.AppendLine("}");
                                #endregion

                                #region bindGridFile
                                sbFileEvent.AppendLine("function bindGrid" + name + dtC.data + "(row,dataSet) {");
                                sbFileEvent.AppendLine("Grid" + name + dtC.data + " = $(\"#Grid" + name + dtC.data + "\",row).DataTable({");
                                sbFileEvent.AppendLine("\"deferRender\": true,");
                                sbFileEvent.AppendLine("\"paging\": false,");
                                sbFileEvent.AppendLine("\"processing\": true,");
                                sbFileEvent.AppendLine("\"destroy\": !$.isEmptyObject(Grid" + name + dtC.data + "),");
                                //sbFileEvent.AppendLine("\"order\": [[1, \"asc\"]],");
                                sbFileEvent.AppendLine("\"info\": false,");
                                sbFileEvent.AppendLine("\"ordering\": false,");
                                sbFileEvent.AppendLine("\"searching\": false,");
                                sbFileEvent.AppendLine("\"lengthChange\": false,");
                                sbFileEvent.AppendLine("\"autoWidth\": false,");
                                sbFileEvent.AppendLine("\"scrollCollapse\": true,");
                                sbFileEvent.AppendLine("\"data\": dataSet,");
                                sbFileEvent.AppendLine("\"dom\": \"<'table-responsive'tr>\",");
                                sbFileEvent.AppendLine("\"language\": {");
                                sbFileEvent.Append("\"lengthMenu\": \"");
                                sbFileEvent.Append(Translation.CenterLang.Center.gridLengthMenu);
                                sbFileEvent.AppendLine("\",");
                                sbFileEvent.Append("\"emptyTable\": \"");
                                sbFileEvent.Append(Translation.CenterLang.Center.gridEmptyTable);
                                sbFileEvent.AppendLine("\",");
                                sbFileEvent.Append("\"info\": \"");
                                sbFileEvent.Append(Translation.CenterLang.Center.gridInfo);
                                sbFileEvent.AppendLine("\",");
                                sbFileEvent.Append("\"infoEmpty\": \"");
                                sbFileEvent.Append(Translation.CenterLang.Center.gridInfoEmpty);
                                sbFileEvent.AppendLine("\",");
                                sbFileEvent.Append("\"infoFiltered\": \"");
                                sbFileEvent.Append(Translation.CenterLang.Center.gridInfoFiltered);
                                sbFileEvent.AppendLine("\",");
                                sbFileEvent.Append("\"loadingRecords\": \"");
                                sbFileEvent.Append(Translation.CenterLang.Center.gridLoadingRecords);
                                sbFileEvent.AppendLine("\",");
                                sbFileEvent.Append("\"processing\": \"");
                                sbFileEvent.Append(Translation.CenterLang.Center.gridProcessing);
                                sbFileEvent.AppendLine("\",");
                                sbFileEvent.AppendLine("\"paginate\": {");
                                sbFileEvent.Append("\"first\": \"");
                                sbFileEvent.Append(Translation.CenterLang.Center.gridPaginateFirst);
                                sbFileEvent.AppendLine("\",");
                                sbFileEvent.Append("\"last\": \"");
                                sbFileEvent.Append(Translation.CenterLang.Center.gridPaginateLast);
                                sbFileEvent.AppendLine("\",");
                                sbFileEvent.Append("\"next\": \"");
                                sbFileEvent.Append(Translation.CenterLang.Center.gridPaginateNext);
                                sbFileEvent.AppendLine("\",");
                                sbFileEvent.Append("\"previous\": \"");
                                sbFileEvent.Append(Translation.CenterLang.Center.gridPaginatePrevious);
                                sbFileEvent.AppendLine("\"");
                                sbFileEvent.AppendLine("},");
                                sbFileEvent.AppendLine("},");

                                if (dtC.type == ColumnsType.FileMultiple)
                                {
                                    sbFileEvent.AppendLine("select: {");
                                    sbFileEvent.AppendLine("style: 'multi',");
                                    sbFileEvent.AppendLine("selector: 'td:first-child'");
                                    sbFileEvent.AppendLine("},");
                                }
                                sbFileEvent.AppendLine("\"columns\": [");
                                if (dtC.type == ColumnsType.FileMultiple)
                                {
                                    sbFileEvent.AppendLine("{");
                                    sbFileEvent.AppendLine("data: null,");
                                    sbFileEvent.AppendLine("defaultContent: '',");
                                    sbFileEvent.AppendLine("width: '2%',");
                                    sbFileEvent.AppendLine("className: 'dt-head-center select-checkbox',");
                                    sbFileEvent.AppendLine("orderable: false,");
                                    sbFileEvent.AppendLine("title: '<input type=\"checkbox\" name=\"selectAllGrid" + name + dtC.data + "\" id=\"selectAllGrid" + name + dtC.data + "\" class=\"checkbox\">'");
                                    sbFileEvent.AppendLine("},");
                                }
                                sbFileEvent.AppendLine("{");
                                sbFileEvent.AppendLine("\"data\": \"ID\",");
                                sbFileEvent.AppendLine("\"orderable\": true,");
                                sbFileEvent.AppendLine("\"title\": \"\",");
                                sbFileEvent.AppendLine("\"width\": \"3%\",");
                                sbFileEvent.AppendLine("className: 'dt-head-center dt-head-nowrap',");
                                sbFileEvent.AppendLine("\"render\": function (data, type, full, meta) {");
                                sbFileEvent.AppendLine("var tag = '';");
                                sbFileEvent.AppendLine("if (data != null && data != '') {");
                                sbFileEvent.Append("tag += '<a data-toggle=\"tooltip\" data-placement=\"right\" target=\"_blank\" title=\"");
                                sbFileEvent.Append(Translation.CenterLang.Center.Download);
                                sbFileEvent.Append("\" href=\\\"");
                                sbFileEvent.Append(url.Action("Download", "File", new { Area = "Ux" }));
                                sbFileEvent.AppendLine("?ID=' + toUrlString(data);");
                                sbFileEvent.AppendLine("tag += '\\\" ><i class=\\\"ace-icon fa fa-download bigger-130\\\"></i></a>';");
                                sbFileEvent.AppendLine("}");
                                sbFileEvent.AppendLine("return tag;");
                                sbFileEvent.AppendLine("}");
                                sbFileEvent.AppendLine("},");
                                sbFileEvent.AppendLine("{");
                                sbFileEvent.AppendLine("\"data\": \"FILE_NAME\",");
                                sbFileEvent.Append("\"title\": \"");
                                sbFileEvent.Append(Translation.CenterLang.Center.FileName);
                                sbFileEvent.AppendLine("\",");
                                sbFileEvent.AppendLine("className: 'dt-head-center dt-head-nowrap',");
                                sbFileEvent.AppendLine("\"width\": \"59%\"");
                                sbFileEvent.AppendLine("},");
                                sbFileEvent.AppendLine("{");
                                sbFileEvent.AppendLine("\"data\": \"FILE_SIZE\",");
                                sbFileEvent.Append("\"title\": \"");
                                sbFileEvent.Append(Translation.CenterLang.Center.FileSize);
                                sbFileEvent.AppendLine("\",");
                                sbFileEvent.AppendLine("\"width\": \"9%\",");
                                sbFileEvent.AppendLine("className: 'dt-head-center dt-head-nowrap dt-body-right',");
                                sbFileEvent.AppendLine("\"render\": function (data, type, full, meta) {");
                                sbFileEvent.AppendLine("var nData = toNumberFormat(data, \"#,###,##0.####\");");
                                sbFileEvent.AppendLine("return (data != undefined && data != null) ? nData + \" MB\" : \"\";");
                                sbFileEvent.AppendLine("}");
                                sbFileEvent.AppendLine("},");
                                sbFileEvent.AppendLine("{");
                                sbFileEvent.AppendLine("\"data\": \"FILE_DATE\",");
                                sbFileEvent.Append("\"title\": \"");
                                sbFileEvent.Append(Translation.CenterLang.Center.FileDateModified);
                                sbFileEvent.AppendLine("\",");
                                sbFileEvent.AppendLine("\"width\": \"15%\",");
                                sbFileEvent.AppendLine("className: 'dt-head-center dt-head-nowrap dt-body-center',");
                                sbFileEvent.AppendLine("\"render\": function (data, type, full, meta) {");
                                sbFileEvent.Append("var nData ='';");
                                sbFileEvent.AppendLine("if(data!=null&&data!=''){");
                                sbFileEvent.AppendLine("nData = jsonDateToFormat(data, \"DD/MM/YYYY HH:mm:ss\");");
                                sbFileEvent.AppendLine("}");
                                sbFileEvent.AppendLine("return nData;");
                                sbFileEvent.AppendLine("}");
                                sbFileEvent.AppendLine("},");
                                sbFileEvent.AppendLine("{");
                                sbFileEvent.AppendLine("\"data\": \"CRET_DATE\",");
                                sbFileEvent.Append("\"title\": \"");
                                sbFileEvent.Append(Translation.CenterLang.Center.FileUploadDate);
                                sbFileEvent.AppendLine("\",");
                                sbFileEvent.AppendLine("\"width\": \"15%\",");
                                sbFileEvent.AppendLine("className: 'dt-head-center dt-head-nowrap dt-body-center',");
                                sbFileEvent.AppendLine("\"render\": function (data, type, full, meta) {");
                                sbFileEvent.Append("var nData ='<span class=\"text-danger\">");
                                sbFileEvent.Append(Translation.CenterLang.Center.FileWaitingUpload);
                                sbFileEvent.AppendLine("</span>';");
                                sbFileEvent.AppendLine("if(data!=null&&data!=''){");
                                sbFileEvent.AppendLine("nData = jsonDateToFormat(data, \"DD/MM/YYYY HH:mm:ss\");");
                                sbFileEvent.AppendLine("}");
                                sbFileEvent.AppendLine("return nData;");
                                sbFileEvent.AppendLine("}");
                                sbFileEvent.AppendLine("}],");
                                sbFileEvent.AppendLine("drawCallback: function () {");
                                sbFileEvent.AppendLine("if ($('#Grid" + name + dtC.data + " tbody .dataTables_empty',row).length) {");
                                if (dtC.type == ColumnsType.FileMultiple)
                                {
                                    sbFileEvent.AppendLine("$(\"#selectAllGrid" + name + dtC.data + "\",row).prop(\"checked\", false);");
                                }
                                sbFileEvent.AppendLine("$('#btnDel" + name + dtC.data + "',row).hide();");
                                sbFileEvent.AppendLine("}");
                                sbFileEvent.AppendLine("else {");
                                sbFileEvent.AppendLine("$('[data-toggle=\"tooltip\"]',row).tooltip({");
                                sbFileEvent.AppendLine("trigger: 'hover'");
                                sbFileEvent.AppendLine("});");
                                sbFileEvent.AppendLine("$('#btnDel" + name + dtC.data + "',row).show();");
                                sbFileEvent.AppendLine("}");
                                sbFileEvent.AppendLine("}");
                                sbFileEvent.AppendLine("});");
                                if (dtC.type == ColumnsType.FileMultiple)
                                {
                                    sbFileEvent.AppendLine("$('#Grid" + name + dtC.data + "',row).on('init.dt', function () {");
                                    sbFileEvent.AppendLine("var rows = Grid" + name + dtC.data + ".rows({ selected: true });");
                                    sbFileEvent.AppendLine("rows.select();");
                                    sbFileEvent.AppendLine("var totalSelected = rows.data().count();");
                                    sbFileEvent.AppendLine("var total = Grid" + name + dtC.data + ".rows().data().count();");
                                    sbFileEvent.AppendLine("if (total > 0 && totalSelected == total) {");
                                    sbFileEvent.AppendLine("$(\"#selectAllGrid" + name + dtC.data + "\",row).prop(\"checked\", true);");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("else {");
                                    sbFileEvent.AppendLine("$(\"#selectAllGrid" + name + dtC.data + "\",row).prop(\"checked\", false);");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("});");
                                    sbFileEvent.AppendLine("$(\"#selectAllGrid" + name + dtC.data + "\",row).change(function () {");
                                    sbFileEvent.AppendLine("if (this.checked) {");
                                    sbFileEvent.AppendLine("Grid" + name + dtC.data + ".rows().select();");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("else {");
                                    sbFileEvent.AppendLine("Grid" + name + dtC.data + ".rows().deselect();");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("});");
                                    sbFileEvent.AppendLine("Grid" + name + dtC.data + ".on('select', function (e, dt, type, indexes) {");
                                    sbFileEvent.AppendLine("var allRows = Grid" + name + dtC.data + ".rows().data().count();");
                                    sbFileEvent.AppendLine("var selectedRows = Grid" + name + dtC.data + ".rows({ selected: true }).data().count();");
                                    sbFileEvent.AppendLine("if (allRows == selectedRows) {");
                                    sbFileEvent.AppendLine("$(\"#selectAllGrid" + name + dtC.data + "\",row).prop(\"checked\", true);");
                                    sbFileEvent.AppendLine("}");
                                    sbFileEvent.AppendLine("})");
                                    sbFileEvent.AppendLine(".on('deselect', function (e, dt, type, indexes) {");
                                    sbFileEvent.AppendLine("$(\"#selectAllGrid" + name + dtC.data + "\",row).prop(\"checked\", false);");
                                    sbFileEvent.AppendLine("});");
                                }
                                sbFileEvent.AppendLine("}");
                                #endregion

                            }
                            else if (dtC.type == ColumnsType.Number ||
                                     dtC.type == ColumnsType.NumberFormat ||
                                     dtC.type == ColumnsType.NumberFormat2 ||
                                     dtC.type == ColumnsType.NumberFormat4 ||
                                     dtC.type == ColumnsType.NumberFormat6 ||
                                     dtC.type == ColumnsType.NumberFormat8 ||
                                     dtC.type == ColumnsType.NumberFormat10)
                            {
                                sbCtrl.AppendLine(",");
                                if (dtC.type == ColumnsType.Number)
                                {
                                    if (dtC.IsIdCard)
                                    {
                                        sbCtrl.AppendLine("valididcard: true,");
                                        sbCtrl.AppendLine("isidcard: true,");
                                    }
                                    sbCtrl.AppendLine("type:'number'");
                                }
                                else
                                {
                                    sbCtrl.Append("type:'number-format'");
                                    if (dtC.type == ColumnsType.NumberFormat2)
                                    {
                                        sbCtrl.AppendLine(",");
                                        sbCtrl.Append("digits:2");
                                    }
                                    else if (dtC.type == ColumnsType.NumberFormat4)
                                    {
                                        sbCtrl.AppendLine(",");
                                        sbCtrl.Append("digits:4");
                                    }
                                    else if (dtC.type == ColumnsType.NumberFormat6)
                                    {
                                        sbCtrl.AppendLine(",");
                                        sbCtrl.Append("digits:6");
                                    }
                                    else if (dtC.type == ColumnsType.NumberFormat8)
                                    {
                                        sbCtrl.AppendLine(",");
                                        sbCtrl.Append("digits:8");
                                    }
                                    else if (dtC.type == ColumnsType.NumberFormat10)
                                    {
                                        sbCtrl.AppendLine(",");
                                        sbCtrl.Append("digits:10");
                                    }
                                    sbCtrl.AppendLine();
                                }
                                sbCtrl.AppendLine("};");
                                if (!dtC.CustomOptionEditable.IsNullOrEmpty())
                                {
                                    sbCtrl.Append("$.extend(options,");
                                    sbCtrl.Append(dtC.CustomOptionEditable);
                                    sbCtrl.AppendLine("(colIndex, value, mode, rowIndex,dataRow,options));");
                                }
                                sbCtrl.AppendLine("return getInputText(options);");
                            }
                            else if (dtC.type == ColumnsType.Checkbox)
                            {
                                sbCtrl.AppendLine(",");
                                sbCtrl.AppendLine("type:'checkbox'");
                                sbCtrl.AppendLine("};");
                                if (!dtC.CustomOptionEditable.IsNullOrEmpty())
                                {
                                    sbCtrl.Append("$.extend(options,");
                                    sbCtrl.Append(dtC.CustomOptionEditable);
                                    sbCtrl.AppendLine("(colIndex, value, mode, rowIndex,dataRow,options));");
                                }
                                sbCtrl.AppendLine("return getInputCheckbox(options);");
                            }
                            else if (dtC.type == ColumnsType.Autocomlete)
                            {
                                existsAutocomplete = true;
                                if (!dtC.AutocompleteConfig.Key.IsNullOrEmpty())
                                {
                                    sbCtrl.AppendLine(",");
                                    sbCtrl.Append("keysource:'");
                                    sbCtrl.Append(dtC.AutocompleteConfig.Key);
                                    sbCtrl.Append("'");
                                }
                                if (!dtC.AutocompleteConfig.BindField.IsNullOrEmpty())
                                {
                                    sbCtrl.AppendLine(",");
                                    sbCtrl.Append("bindfield:'");
                                    sbCtrl.Append(dtC.AutocompleteConfig.BindField);
                                    sbCtrl.Append("'");
                                }
                                if (dtC.AutocompleteConfig.FieldToInput != null)
                                {
                                    var fieldToInput = Newtonsoft.Json.JsonConvert.SerializeObject(dtC.AutocompleteConfig.FieldToInput).Replace("\"", "'");
                                    sbCtrl.AppendLine(",");
                                    sbCtrl.Append("fieldtoinput:\"");
                                    sbCtrl.Append(fieldToInput);
                                    sbCtrl.Append("\"");
                                }
                                if (dtC.AutocompleteConfig.ExtraParams != null)
                                {
                                    var extraParams = Newtonsoft.Json.JsonConvert.SerializeObject(dtC.AutocompleteConfig.ExtraParams).Replace("\"", "'");
                                    sbCtrl.AppendLine(",");
                                    sbCtrl.Append("extraparams:\"");
                                    sbCtrl.Append(extraParams);
                                    sbCtrl.Append("\"");
                                }
                                if (!dtC.AutocompleteConfig.ParamsCtrl.IsNullOrEmpty())
                                {
                                    sbCtrl.AppendLine(",");
                                    sbCtrl.Append("paramsctrl:'");
                                    sbCtrl.Append(dtC.AutocompleteConfig.ParamsCtrl);
                                    sbCtrl.Append("'");
                                }
                                sbCtrl.AppendLine("};");
                                if (!dtC.CustomOptionEditable.IsNullOrEmpty())
                                {
                                    sbCtrl.Append("$.extend(options,");
                                    sbCtrl.Append(dtC.CustomOptionEditable);
                                    sbCtrl.AppendLine("(colIndex, value, mode, rowIndex,dataRow,options));");
                                }
                                sbCtrl.AppendLine("return getInputAutocomplete(options);");
                            }
                            else
                            {
                                sbCtrl.AppendLine("};");
                                if (!dtC.CustomOptionEditable.IsNullOrEmpty())
                                {
                                    sbCtrl.Append("$.extend(options,");
                                    sbCtrl.Append(dtC.CustomOptionEditable);
                                    sbCtrl.AppendLine("(colIndex, value, mode, rowIndex,dataRow,options));");
                                }
                                sbCtrl.AppendLine("return getInputText(options);");
                            }

                            sbRowCtrl.AppendLine("col = $('<td></td>');");
                            sbRowCtrl.Append("col.append(GetCtrl" + name + "(");
                            sbRowCtrl.Append(c + startIdx);
                            sbRowCtrl.Append(", dataRow.");
                            sbRowCtrl.Append(dtC.data);
                            sbRowCtrl.AppendLine(", mode, rowIndex,dataRow));");
                            sbRowCtrl.AppendLine("row.append(col);");
                        }
                        else
                        {
                            sbRowCtrl.AppendLine("col = $('<td></td>');");
                            if (!dtC.data.IsNullOrEmpty())
                            {
                                sbRowCtrl.Append("col.append(dataRow.");
                                sbRowCtrl.Append(dtC.data);
                                sbRowCtrl.AppendLine(");");
                            }
                            sbRowCtrl.AppendLine("row.append(col);");
                        }
                    }
                    if (dtC.IsOrderColumn)
                    {
                        orderIdx.Add(startIdx + c);
                    }
                    c++;
                    sbCols.AppendLine("{");

                    if (dtC.data.IsNullOrEmpty())
                    {
                        sbCols.Append("\"data\":null");
                    }
                    else
                    {
                        if (dtC.type == ColumnsType.Select)
                        {
                            sbCols.Append("\"data\":\"" + dtC.data + "_TEXT\"");
                        }
                        else
                        {
                            sbCols.Append("\"data\":\"" + dtC.data + "\"");
                        }
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
                        sbCols.AppendLine(",");
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
                                    sbCols.Append("\"" + prop.Name + "\":" + value + "");
                                }
                                else if (prop.Name == "width" && config.WidthType == ColumnsWidthType.Percentage && !value.AsString().Contains("%"))
                                {
                                    sb.Append("\"" + prop.Name + "\":\"" + value + "%\"");
                                }
                                else
                                {
                                    sbCols.Append("\"" + prop.Name + "\":\"" + value + "\"");
                                }
                            }
                            else if (prop.Name == "data")
                            {
                                sbCols.Append("\"" + prop.Name + "\":null");
                            }
                        }
                        else if (prop.PropertyType == typeof(bool))
                        {
                            sbCols.Append("\"" + prop.Name + "\":" + Convert.ToString(value).ToLower());
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
                                sbCols.Append("\"" + prop.Name + "\":\"" + ((ColumnsType)value).GetDescription() + "\"");
                            }
                        }
                        else
                        {
                            sbCols.Append("\"" + prop.Name + "\":\"" + Convert.ToString(value).ToLower() + "\"");
                        }
                        if (i != allProperties.Count)
                        {
                            if (prop.Name == "type" &&
                                ((ColumnsType)value != ColumnsType.None &&
                                (ColumnsType)value != ColumnsType.File &&
                                (ColumnsType)value != ColumnsType.FileMultiple &&
                                (ColumnsType)value != ColumnsType.Select &&
                                (ColumnsType)value != ColumnsType.Checkbox &&
                                (ColumnsType)value != ColumnsType.Autocomlete))
                            {
                                sbCols.AppendLine(",");
                            }
                            else if (prop.Name != "type")
                            {
                                sbCols.AppendLine(",");
                            }
                        }
                    }
                    sbCols.Append("}");

                    if (c != cols.Count)
                    {
                        sbCols.AppendLine(",");
                    }
                }
            }
            #endregion

            if (!config.IsReadOnly)
            {
                #region OnClick_btnCancel
                sb.AppendLine("function OnClick_btnCancel" + name + "() {");
                sb.AppendLine("$.clearValidError('" + name + "');");
                sb.AppendLine("var btn = $(this);");
                sb.AppendLine("Grid" + name + ".draw();");
                sb.AppendLine("$.SlidingTime();");
                sb.AppendLine("return false;");
                sb.AppendLine("}");
                #endregion

                #region OnClick_btnSave
                if (!config.CustomSave)
                {
                    sb.AppendLine("function OnClick_btnSave" + name + "() {");
                    sb.AppendLine("$.clearValidError('" + name + "');");
                    sb.AppendLine("var btn = $(this);");
                    sb.AppendLine("var mode = btn.data(\"mode\");");
                    sb.AppendLine("var row = btn.closest(\"tr\");");
                    sb.AppendLine("if(row.validTableRow()){");
                    sb.AppendLine("var rowIdx = btn.data('rowindex');");
                    sb.AppendLine("var newData = row.serializeRowToObjectGrid();");
                    //sb.AppendLine("if (mode == \"edit\") {");
                    //sb.AppendLine("var oldData = Grid" + name + ".row(rowIdx).data();");
                    //sb.AppendLine("newData = $.extend({}, oldData, newData);");
                    //sb.AppendLine("}");
                    if (existsKey != null && existsKey.Any())
                    {
                        var keyCase = string.Empty;
                        var i = 1;
                        var cK = existsKey.Count();
                        foreach (var item in existsKey)
                        {
                            if (item.type == ColumnsType.Date || item.type == ColumnsType.DateTime)
                            {
                                keyCase += "jsonDateToFormat(item." + item.data + ") == jsonDateToFormat(newData." + item.data + ")";
                            }
                            else
                            {
                                if (item.CaseSensitive)
                                {
                                    keyCase += "item." + item.data + " == newData." + item.data;
                                }
                                else
                                {
                                    keyCase += "item." + item.data + ".toString().toUpperCase() == newData." + item.data + ".toString().toUpperCase()";
                                }
                            }
                            if (i != cK)
                            {
                                keyCase += "&&";
                            }
                            i++;
                        }

                        sb.AppendLine("var isDup = false;");
                        sb.AppendLine("var gData = Grid" + name + ".rows({order:'index'}).data();");
                        sb.AppendLine("if(gData.length > 0){");
                        sb.AppendLine("$.each(gData.toArray(), function (i, item) {");
                        sb.AppendLine("if (mode == \"Edit\") {");
                        sb.AppendLine("if(i!=rowIdx) {");
                        sb.Append("if(");
                        sb.Append(keyCase);
                        sb.Append("){");
                        sb.AppendLine("isDup=true;");
                        sb.AppendLine("return false;");
                        sb.AppendLine("}");
                        sb.AppendLine("}");
                        sb.AppendLine("}");
                        sb.AppendLine("else{");
                        sb.Append("if(");
                        sb.Append(keyCase);
                        sb.Append("){");
                        sb.AppendLine("isDup=true;");
                        sb.AppendLine("return false;");
                        sb.AppendLine("}");
                        sb.AppendLine("}");
                        sb.AppendLine("});");
                        sb.AppendLine("}");

                        sb.AppendLine("if(!isDup){");
                        if (config.VisibleEditColumn)
                        {
                            sb.AppendLine("if (mode == \"Edit\") {");
                            sb.AppendLine("var oldData = Grid" + name + ".row(rowIdx).data();");
                            sb.AppendLine("$.extend(oldData, newData);");
                            sb.AppendLine("Grid" + name + ".row(rowIdx).data(oldData).draw();");
                            sb.AppendLine("}");
                            sb.AppendLine("else {");
                            sb.AppendLine("Grid" + name + ".row.add(newData).draw();");
                            sb.AppendLine("}");
                        }
                        else
                        {
                            sb.AppendLine("Grid" + name + ".row.add(newData).draw();");
                        }
                        sb.AppendLine("var currentPageIndex = Grid" + name + ".page.info().page;");
                        sb.AppendLine("Grid" + name + ".page(currentPageIndex).draw(false);");
                        ////กรณี Confirm Save Complete
                        //sb.AppendLine("$.confirm({");
                        //sb.AppendLine("title: null,");
                        //sb.Append("content: \"");
                        //sb.Append(Translation.CenterLang.Center.SaveCompleted);
                        //sb.AppendLine("\",");
                        //sb.AppendLine("columnClass: 'medium',");
                        //sb.AppendLine("buttons: {");
                        //sb.AppendLine("confirm: {");
                        //sb.AppendLine("text: \"OK\",");
                        //sb.AppendLine("btnClass: \"btn-primary\",");
                        //sb.AppendLine("action: function () {");
                        //sb.AppendLine("var currentPageIndex = Grid" + name + ".page.info().page;");
                        //sb.AppendLine("Grid" + name + ".page(currentPageIndex).draw(false);");
                        //sb.AppendLine("}");
                        //sb.AppendLine("}");
                        //sb.AppendLine("}");
                        //sb.AppendLine("});");
                        sb.AppendLine("}");
                        sb.AppendLine("else{");
                        sb.Append("var msg = $('<p id =\"validError" + name + "\" class=\"text-danger no-margin\">");
                        sb.Append(Translation.CenterLang.Center.DuplcateData);
                        sb.AppendLine("</p>');");
                        sb.AppendLine("if($('#Grid" + name + "').closest('.gridIncludeToolbar').length > 0){");
                        sb.AppendLine("msg.insertBefore($('#Grid" + name + "').closest('.gridIncludeToolbar'));");
                        sb.AppendLine("}");
                        sb.AppendLine("else{");
                        sb.AppendLine("msg.insertBefore($('#Grid" + name + "').closest('.dataTables_wrapper'));");
                        sb.AppendLine("}");
                        sb.AppendLine("}");
                    }
                    else
                    {
                        if (config.VisibleEditColumn)
                        {
                            sb.AppendLine("if (mode == \"Edit\") {");
                            sb.AppendLine("var oldData = Grid" + name + ".row(rowIdx).data();");
                            sb.AppendLine("$.extend(oldData, newData);");
                            sb.AppendLine("Grid" + name + ".row(rowIdx).data(oldData).draw();");
                            sb.AppendLine("}");
                            sb.AppendLine("else {");
                            sb.AppendLine("Grid" + name + ".row.add(newData).draw();");
                            sb.AppendLine("}");
                        }
                        else
                        {
                            sb.AppendLine("Grid" + name + ".row.add(newData).draw();");
                        }
                        sb.AppendLine("var currentPageIndex = Grid" + name + ".page.info().page;");
                        sb.AppendLine("Grid" + name + ".page(currentPageIndex).draw(false);");
                        ////กรณี Confirm Save Complete
                        //sb.AppendLine("$.confirm({");
                        //sb.AppendLine("title: null,");
                        //sb.Append("content: \"");
                        //sb.Append(Translation.CenterLang.Center.SaveCompleted);
                        //sb.AppendLine("\",");
                        //sb.AppendLine("columnClass: 'medium',");
                        //sb.AppendLine("buttons: {");
                        //sb.AppendLine("confirm: {");
                        //sb.AppendLine("text: \"OK\",");
                        //sb.AppendLine("btnClass: \"btn-primary\",");
                        //sb.AppendLine("action: function () {");
                        //sb.AppendLine("var currentPageIndex = Grid" + name + ".page.info().page;");
                        //sb.AppendLine("Grid" + name + ".page(currentPageIndex).draw(false);");
                        //sb.AppendLine("}");
                        //sb.AppendLine("}");
                        //sb.AppendLine("}");
                        //sb.AppendLine("});");
                    }
                    sb.AppendLine("}");
                    sb.AppendLine("$.SlidingTime();");
                    //กรณี Not Confirm Save Complete
                    sb.AppendLine("return false;");
                    sb.AppendLine("}");
                }
                #endregion

                if (colFileEdit.Count > 0)
                {
                    sb.AppendLine(sbFileEvent.ToString());
                }

                #region GetCtrl
                sb.AppendLine("function GetCtrl" + name + "(colIndex, value, mode, rowIndex,dataRow) {");
                sb.AppendLine("switch (colIndex) {");
                //Dynamic
                sb.AppendLine(sbCtrl.ToString());
                sb.AppendLine("}");
                sb.AppendLine("}");
                #endregion

                #region GetRowCtrl
                sb.AppendLine("function GetRowCtrl" + name + "(mode, rowIndex) {");
                sb.AppendLine("var row = $('<tr></tr>');");
                sb.Append("var dataRow = {");

                var colRow = cols.Where(m => !m.data.IsNullOrEmpty()).Select(m => m.data).ToList();
                var cr = 0;
                foreach (var item in colRow)
                {
                    sb.Append(item);
                    sb.Append(":null");
                    if (cr != colRow.Count - 1)
                    {
                        sb.Append(",");
                    }
                    sb.AppendLine();
                    cr++;
                }
                sb.AppendLine("};");
                sb.AppendLine("if (mode == 'Edit') {");
                sb.AppendLine("var oldDataRow = Grid" + name + ".row(rowIndex).data();");
                sb.AppendLine("$.extend(dataRow, oldDataRow);");
                sb.AppendLine("}");
                sb.AppendLine("row.data('rowctrl', true);");
                sb.AppendLine("row.empty();");
                sb.Append("var col = $('<td colspan=\"");
                sb.Append(startIdx);
                sb.AppendLine("\" class=\"text-center\"></td>');");
                sb.Append("var btnSave = $('<a id=\"btnSave" + name + "\" href=\"javascript:void(0)\" data-toggle=\"tooltip\" title=\"");
                sb.Append(Translation.CenterLang.Center.Save);
                sb.AppendLine("\" data-mode=\"' + mode + '\" class=\"datatables-btn\"><i class=\"ace-icon fa fa-save align-top bigger-150\"></i></a>');");
                sb.Append("var btnCancel = $('<a id=\"btnCancel" + name + "\" href=\"javascript:void(0)\" data-toggle=\"tooltip\" title=\"");
                sb.Append(Translation.CenterLang.Center.Cancel);
                sb.AppendLine("\" data-mode=\"' + mode + '\" class=\"datatables-btn\"><i class=\"ace-icon fa fa-times align-top bigger-150 red\"></i></a>');");
                sb.AppendLine("if (mode == 'Edit') {");
                sb.AppendLine("btnSave.data(\"rowindex\", rowIndex);");
                sb.AppendLine("btnCancel.data(\"rowindex\", rowIndex);");
                sb.AppendLine("}");
                sb.AppendLine("btnSave.click(OnClick_btnSave" + name + ");");
                sb.AppendLine("btnCancel.click(OnClick_btnCancel" + name + ");");
                sb.AppendLine("var p = $('<p class=\"no-margin\">');");
                sb.AppendLine("p.append(btnSave);");
                sb.AppendLine("col.append(p);");
                sb.AppendLine("p = $('<p class=\"no-margin\">');");
                sb.AppendLine("p.append(btnCancel);");
                sb.AppendLine("col.append(p);");
                sb.AppendLine("row.append(col);");
                //Dynamic
                sb.AppendLine(sbRowCtrl.ToString());
                sb.AppendLine("return row;");
                sb.AppendLine("}");
                #endregion

                #region initAfterClick 
                sb.Append("function initAfterClick" + name + "(row");
                if (colFileEdit.Count > 0)
                {
                    sb.Append(",fileOptions");
                }
                sb.AppendLine(") {");

                if (colDateEdit.Count > 0)
                {
                    sb.AppendLine("$('.datepicker',row).datepicker('destroy');");
                    sb.AppendLine("setTimeout(function () {");
                    sb.AppendLine("$('.datepicker',row).datepicker({");
                    sb.AppendLine("locale: \"en\",");
                    sb.AppendLine("format: \"dd/mm/yyyy\",");
                    sb.AppendLine("autoclose: true,");
                    sb.AppendLine("todayHighlight: true,");
                    sb.AppendLine("orientation: 'bottom'");
                    sb.AppendLine("}).next().on(ace.click_event, function () {");
                    sb.AppendLine("$(this).prev().focus();");
                    sb.AppendLine("});");
                    sb.AppendLine("}, 100);");
                }

                if (colFileEdit.Count > 0)
                {
                    sb.AppendLine("$.each(fileOptions, function (i, item) {");
                    sb.AppendLine("window['bindGrid" + name + "' + item.colName](row,item.dataSet);");
                    sb.AppendLine("$('#f" + name + "' + item.colName,row).change(window['OnChange_f" + name + "' + item.colName]);");
                    sb.AppendLine("$('#btnAdd" + name + "' + item.colName,row).click(window['OnClick_btnAdd" + name + "' + item.colName]);");
                    sb.AppendLine("$('#btnDel" + name + "' + item.colName,row).confirm({");
                    sb.AppendLine("title: null,");
                    sb.Append("content: '");
                    sb.Append(Translation.CenterLang.Center.ConfirmDelete);
                    sb.AppendLine("',");
                    sb.AppendLine("columnClass: 'medium',");
                    sb.AppendLine("buttons: {");
                    sb.AppendLine("confirm: {");
                    sb.Append("text: '");
                    sb.Append(Translation.CenterLang.Center.OK);
                    sb.AppendLine("',");
                    sb.AppendLine("btnClass: 'btn-primary',");
                    sb.AppendLine("action: function () {");
                    sb.AppendLine("$.clearValidError('" + name + "' + item.colName);");
                    sb.AppendLine("var btn = this.$target;");
                    sb.AppendLine("var data = window['Grid" + name + "' + item.colName].rows({ selected: true }).data();");
                    sb.AppendLine("if (data.length > 0) {");
                    sb.AppendLine("window['Grid" + name + "' + item.colName].rows({ selected: true }).remove().draw();");
                    sb.AppendLine("}");
                    sb.AppendLine("else {");
                    sb.Append("$('<span id =\"validError" + name + "' + item.colName + '\" class=\"text-danger\">");
                    sb.Append(Translation.CenterLang.Center.PleaseSelectData);
                    sb.AppendLine("</span>').insertBefore('#f" + name + "' + item.colName,row);");
                    sb.AppendLine("}");
                    sb.AppendLine("}");
                    sb.AppendLine("},");
                    sb.AppendLine("cancel: {");
                    sb.Append("text: '");
                    sb.Append(Translation.CenterLang.Center.Cancel);
                    sb.AppendLine("',");
                    sb.AppendLine("btnClass: 'btn-primary'");
                    sb.AppendLine("}");
                    sb.AppendLine("}");
                    sb.AppendLine("});");
                    sb.AppendLine("});");
                }
                if (existsAutocomplete)
                {
                    sb.AppendLine("$(\".autocomplete-input\").JAutocomplete({");
                    sb.Append("url: '");
                    sb.Append(url.Action("Index", "Autocomplete", new { Area = "Ux" }));
                    sb.AppendLine("'");
                    sb.AppendLine("});");
                }

                sb.AppendLine("$('[data-toggle=\"tooltip\"]',row).tooltip({");
                sb.AppendLine("trigger: 'hover'");
                sb.AppendLine("});");
                if (!config.CustomInitAfterClick.IsNullOrEmpty())
                {
                    sb.Append(config.CustomInitAfterClick);
                    sb.AppendLine("(row);");
                }
                sb.AppendLine("}");
                #endregion

                var sbFileOptionEdit = new StringBuilder();

                #region OnClick_btnAdd
                if (!config.IsReadOnly)
                {
                    sb.AppendLine("function OnClick_btnAdd" + name + "(e) {");
                    sb.AppendLine("$.clearValidError('" + name + "');");
                    sb.AppendLine("var firstRow = $('#Grid" + name + " tbody tr:first');");
                    sb.AppendLine("if (!$('#Grid" + name + "').hasRowCtrl()) {");
                    sb.AppendLine("var row = GetRowCtrl" + name + "('Add');");
                    sb.AppendLine("firstRow.before(row);");

                    if (colFileEdit.Count > 0)
                    {
                        sb.AppendLine("initAfterClick" + name + "(row,[");
                        var i = 0;
                        foreach (var item in colFileEdit)
                        {
                            sb.Append("{colName:'");
                            sb.Append(item.data);
                            sb.Append("',dataSet:[]}");

                            sbFileOptionEdit.Append("{colName:'");
                            sbFileOptionEdit.Append(item.data);
                            sbFileOptionEdit.Append("',dataSet:dataRow.");
                            sbFileOptionEdit.Append(item.data);
                            sbFileOptionEdit.Append("}");

                            if (i != colFile.Count - 1)
                            {
                                sb.AppendLine(",");
                                sbFileOptionEdit.AppendLine(",");
                            }
                        }
                        sb.AppendLine("]);");
                    }
                    else
                    {
                        sb.AppendLine("initAfterClick" + name + "(row);");
                    }
                    sb.AppendLine("if ($('.dataTables_empty', firstRow).not($('table tbody .dataTables_empty', firstRow)).length) {");
                    sb.AppendLine("firstRow.remove();");
                    sb.AppendLine("}");
                    sb.AppendLine("}");
                    sb.AppendLine("else{");
                    sb.Append("var msg = $('<p id =\"validError" + name + "\" class=\"text-danger no-margin\">");
                    sb.Append(Translation.CenterLang.Center.PleaseSaveData);
                    sb.AppendLine("</p>');");
                    sb.AppendLine("if($('#Grid" + name + "').closest('.gridIncludeToolbar').length > 0){");
                    sb.AppendLine("msg.insertBefore($('#Grid" + name + "').closest('.gridIncludeToolbar'));");
                    sb.AppendLine("}");
                    sb.AppendLine("else{");
                    sb.AppendLine("msg.insertBefore($('#Grid" + name + "').closest('.dataTables_wrapper'));");
                    sb.AppendLine("}");
                    sb.AppendLine("}");
                    sb.AppendLine("return false;");
                    sb.AppendLine("}");
                }
                #endregion

                #region OnClick_btnEdit
                if (config.VisibleEditColumn)
                {
                    sb.AppendLine("function OnClick_btnEdit" + name + "(e) {");
                    sb.AppendLine("$.clearValidError('" + name + "');");
                    sb.AppendLine("if(!$('#Grid" + name + "').hasRowCtrl()){");
                    sb.AppendLine("var oldRow = $(this).closest('tr');");
                    sb.AppendLine("var idx = Grid" + name + ".row(oldRow).index();");
                    sb.AppendLine("var row = GetRowCtrl" + name + "('Edit', idx);");
                    sb.AppendLine("oldRow.before(row);");
                    sb.AppendLine("var dataRow = Grid" + name + ".row(idx).data();");
                    if (colFileEdit.Count > 0)
                    {
                        sb.AppendLine("initAfterClick" + name + "(row,[");
                        sb.AppendLine(sbFileOptionEdit.ToString());
                        sb.AppendLine("]);");
                    }
                    else
                    {
                        sb.AppendLine("initAfterClick" + name + "(row);");
                    }
                    sb.AppendLine("$('[role=\"tooltip\"]', oldRow).remove();");
                    sb.AppendLine("oldRow.remove();");
                    sb.AppendLine("}");
                    sb.AppendLine("else{");
                    sb.Append("var msg = $('<p id =\"validError" + name + "\" class=\"text-danger no-margin\">");
                    sb.Append(Translation.CenterLang.Center.PleaseSaveData);
                    sb.AppendLine("</p>');");
                    sb.AppendLine("if($('#Grid" + name + "').closest('.gridIncludeToolbar').length > 0){");
                    sb.AppendLine("msg.insertBefore($('#Grid" + name + "').closest('.gridIncludeToolbar'));");
                    sb.AppendLine("}");
                    sb.AppendLine("else{");
                    sb.AppendLine("msg.insertBefore($('#Grid" + name + "').closest('.dataTables_wrapper'));");
                    sb.AppendLine("}");
                    sb.AppendLine("}");
                    sb.AppendLine("return false;");
                    sb.AppendLine("}");
                }
                #endregion

                #region OnClick_btnDelete
                sb.AppendLine("function OnClick_btnDelete" + name + "() {");
                sb.AppendLine("$.clearValidError('" + name + "');");
                sb.AppendLine("var data = Grid" + name + ".rows({ selected: true }).data();");
                sb.AppendLine("if (data.length > 0) {");
                sb.AppendLine("Grid" + name + ".rows({ selected: true }).remove().draw();");
                sb.AppendLine("}");
                sb.AppendLine("else {");
                sb.Append("var msg = $('<p id =\"validError" + name + "\" class=\"text-danger no-margin\">");
                sb.Append(Translation.CenterLang.Center.PleaseSelectData);
                sb.AppendLine("</p>');");
                sb.AppendLine("if($('#Grid" + name + "').closest('.gridIncludeToolbar').length > 0){");
                sb.AppendLine("msg.insertBefore($('#Grid" + name + "').closest('.gridIncludeToolbar'));");
                sb.AppendLine("}");
                sb.AppendLine("else{");
                sb.AppendLine("msg.insertBefore($('#Grid" + name + "').closest('.dataTables_wrapper'));");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("}");
                #endregion
            }
            #region bindGrid
            sb.AppendLine("function bindGrid" + name + "() {");

            sb.AppendLine("var option = {");
            sb.AppendLine("\"deferRender\": true,");
            sb.AppendLine("\"paging\": true,");
            sb.AppendLine("\"processing\": true,");
            sb.AppendLine("\"destroy\": !$.isEmptyObject(Grid" + name + "),");
            sb.AppendLine("\"ordering\": false,");
            sb.AppendLine("\"searching\": false,");
            sb.AppendLine("\"lengthChange\": false,");
            sb.AppendLine("\"autoWidth\": false,");
            sb.AppendLine("\"scrollCollapse\": true,");
            //ใช่ไม่ได้ Layout ไม่ได้
            //if (config.ScrollX)
            //{
            //    sb.AppendLine("\"scrollX\": true,");
            //}
            sb.Append("\"dom\": \"<'table-responsive'tr><'row'<'col-sm-5'");
            //if (config.VisibleExportButton)
            //{
            //    sb.Append("B");
            //}
            sb.AppendLine("<'pull-left'l><'pull-left'i>><'col-sm-7'p>>\",");
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
            sb.Append("\"infoFiltered\": \"");
            sb.Append(Translation.CenterLang.Center.gridInfoFiltered);
            sb.AppendLine("\",");
            sb.Append("\"loadingRecords\": \"");
            sb.Append(Translation.CenterLang.Center.gridLoadingRecords);
            sb.AppendLine("\",");
            sb.Append("\"processing\": \"");
            sb.Append(Translation.CenterLang.Center.gridProcessing);
            sb.AppendLine("\",");
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
            if (config.VisibleCheckBox && !config.IsReadOnly)
            {
                sb.AppendLine("select: {");
                sb.AppendLine("style: 'multi',");
                sb.AppendLine("selector: 'td:first-child'");
                sb.AppendLine("},");
            }
            sb.AppendLine("\"columns\": [");
            sb.AppendLine(sbCols.ToString());


            sb.Append("]");
            sb.AppendLine(",");
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
            sb.AppendLine("drawCallback: function () {");
            if (!config.IsReadOnly)
            {
                sb.AppendLine("var firstRow = $('#Grid" + name + " tbody tr:first');");
                sb.AppendLine("if ($('.dataTables_empty', firstRow).not($('table tbody .dataTables_empty', firstRow)).length) {");
                if (config.VisibleCheckBox)
                {
                    sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", false);");
                }
                sb.AppendLine("$('#btnDelete" + name + "').hide();");
                sb.AppendLine("}");
                sb.AppendLine("else {");
                if (config.IsRequired)
                {
                    sb.AppendLine("$('.widget-box').removeClass('has-error');");
                    sb.AppendLine("$(\"#validError" + name + "\").remove();");
                }
                sb.AppendLine("$('#btnDelete" + name + "').show();");
                sb.AppendLine("}");
            }
            sb.AppendLine("$('[data-toggle=\"tooltip\"]').tooltip({");
            sb.AppendLine("trigger: 'hover'");
            sb.AppendLine("});");
            if (!config.OnDrawCallback.IsNullOrEmpty())
            {
                sb.Append(config.OnDrawCallback);
                sb.AppendLine("();");
            }
            sb.AppendLine("}");
            sb.AppendLine("};");

            if (config.DefaultConfig != null)
            {
                sb.AppendLine("var result = {};");
                if (config.DefaultConfig.Parameters != null)
                {
                    foreach (var item in config.DefaultConfig.Parameters)
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
                //sb.AppendLine("if (result.mode != \"R\") {");
                //sb.AppendLine("$(\"#rowGrid" + name + "\").removeClass(\"hidden\");");
                sb.AppendLine("option[\"ajax\"]= {");
                sb.Append("url: \"");
                sb.Append(config.DefaultConfig.Url);
                sb.AppendLine("\",");
                sb.AppendLine("type: \"post\",");
                sb.AppendLine("error: OnAjaxError,");
                sb.AppendLine("data: result");
                sb.AppendLine("};");
                //sb.AppendLine("}");
            }
            sb.AppendLine("Grid" + name + " = $(\"#Grid" + name + "\").DataTable(option);");

            if (config.VisibleCheckBox && !config.IsReadOnly)
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
                sb.AppendLine("});");
                sb.AppendLine("$(\"#selectAllGrid" + name + "\").change(function () {");
                sb.AppendLine("if (this.checked) {");
                sb.AppendLine("Grid" + name + ".rows().select();");
                sb.AppendLine("}");
                sb.AppendLine("else {");
                sb.AppendLine("Grid" + name + ".rows().deselect();");
                sb.AppendLine("}");
                sb.AppendLine("});");
                sb.AppendLine("Grid" + name + ".on('select', function (e, dt, type, indexes) {");
                sb.AppendLine("var allRows = Grid" + name + ".rows().data().count();");
                sb.AppendLine("var selectedRows = Grid" + name + ".rows({ selected: true }).data().count();");
                sb.AppendLine("if (allRows == selectedRows) {");
                sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", true);");
                sb.AppendLine("}");
                sb.AppendLine("})");
                sb.AppendLine(".on('deselect', function (e, dt, type, indexes) {");
                sb.AppendLine("$(\"#selectAllGrid" + name + "\").prop(\"checked\", false);");
                sb.AppendLine("});");
            }


            if (config.VisibleEditColumn && !config.IsReadOnly)
            {
                sb.AppendLine("$(\"#Grid" + name + " tbody\").on(\"click\", \"a[name=Edit]\", OnClick_btnEdit" + name + ");");
            }
            if (!config.OnAfterBinding.IsNullOrEmpty())
            {
                sb.Append(config.OnAfterBinding);
                sb.AppendLine("();");
            }
            sb.AppendLine("}");
            #endregion

            #region ready
            sb.AppendLine("$(document).ready(function () {");
            if (colDate != null && colDate.Count > 0 && !config.IsReadOnly)
            {
                sb.Append("$(\"#Grid" + name + "\").data('coldate','");
                sb.Append(string.Join(",", colDate.Select(m => m.data)));
                sb.AppendLine("');");
            }
            if (colFile != null && colFile.Count > 0 && !config.IsReadOnly)
            {
                sb.Append("$(\"#Grid" + name + "\").data('colfile','");
                sb.Append(string.Join(",", colFile.Select(m => m.data)));
                sb.AppendLine("');");
            }
            sb.AppendLine("bindGrid" + name + "();");
            if (!config.IsReadOnly)
            {
                if (config.VisibleAdd)
                {
                    sb.AppendLine("$(\"#btnAdd" + name + "\").click(OnClick_btnAdd" + name + ");");
                }
                if (config.VisibleDelete)
                {
                    sb.AppendLine("$(\"#btnDelete" + name + "\").confirm({");
                    sb.AppendLine("title: null,");
                    sb.AppendLine("content: \"" + Translation.CenterLang.Center.ConfirmDelete + "\",");
                    sb.AppendLine("columnClass: 'medium',");
                    sb.AppendLine("buttons: {");
                    sb.AppendLine("confirm: {");
                    sb.Append("text:'");
                    sb.Append(Translation.CenterLang.Center.OK);
                    sb.AppendLine("',");
                    sb.AppendLine("btnClass: \"btn-primary\",");
                    sb.AppendLine("action:OnClick_btnDelete" + name);
                    sb.AppendLine("},");
                    sb.AppendLine("cancel: {");
                    sb.Append("text: '");
                    sb.Append(Translation.CenterLang.Center.Cancel);
                    sb.AppendLine("',");
                    sb.AppendLine("btnClass: \"btn-primary\"");
                    sb.AppendLine("}");
                    sb.AppendLine("}");
                    sb.AppendLine("});");
                }
            }
            sb.AppendLine("});");
            #endregion

            sb.AppendLine("</script>");

            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region Dashboard

        public static MvcHtmlString GetVSMInfoBoxFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string colorcode = "", string icon = FaIcons.FaTasks)
        {
            var sb = new StringBuilder();

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string label = GetControlLabel(metadata, expressionText);
            string name = GetControlName(html, expressionText);

            Func<TModel, TValue> method = expression.Compile();
            string value = (method(html.ViewData.Model)).ToString();

            sb.AppendLine("<div class=\"panel panel-primary\" style=\"border-color: " + colorcode + " !important; background-color: " + colorcode + " !important; border-color:" + colorcode + " !important;\" id=\"Panel_" + name + "\">");
            sb.AppendLine("<div class=\"panel-heading\" style=\"background-color: " + colorcode + " !important; height: 65px;  border-color:" + colorcode + " !important;\">");
            sb.AppendLine("<div class=\"row\">");
            sb.AppendLine("<div class=\"col-xs-3\">");
            sb.AppendLine("<i class=\"fa " + icon + " fa-3x\" style=\"position: relative; bottom: -4px;\"></i>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div class=\"col-xs-9 text-right\">");
            sb.AppendLine("<div class=\"h2\" id=\"" + name + "\" style=\"position: relative; bottom: 12px;\">" + value + "</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("<a href=\"javascript:void(0)\">");
            sb.AppendLine("<div class=\"panel-footer\" style=\"color: " + colorcode + " !important;\">");
            sb.AppendLine("<span class=\"pull-left\">" + label + "</span>");
            sb.AppendLine("<span class=\"pull-right\"><i class=\"glyphicon glyphicon-filter\"></i></span>");
            sb.AppendLine("<div class=\"clearfix\"></div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</a>");
            sb.AppendLine("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString GetVSMChatBox(this HtmlHelper html,
            List<ChatModel> chatModel)
        {
            var sb = new StringBuilder();

            string tmpSEND_NO = "";

            sb.AppendLine("<div class=\"panel-body\">");
            sb.AppendLine("<ul class=\"chat\">");

            foreach (var item in chatModel)
            {
                if (tmpSEND_NO != item.SEND_NO)
                {//send
                    sb.AppendLine("<li class=\"left clearfix\">");
                    sb.AppendLine("<span class=\"chat-img pull-left\">");
                    sb.AppendLine("<div class=\"circleSend pull-left\"><p>" + item.MS_CORP_NAME_TH + "</p></div>");
                    sb.AppendLine("</span>");
                    sb.AppendLine("<div class=\"chat-body clearfix\">");
                    sb.AppendLine("<div class=\"header\">");
                    sb.AppendLine("<strong class=\"primary-font\">" + item.ROW1ST + "</strong> <small class=\"pull-right text-muted\">");
                    sb.AppendLine("<span class=\"glyphicon\"></span>");
                    sb.AppendLine("</small>");
                    sb.AppendLine("</div>");
                    sb.AppendLine("<p>");
                    sb.AppendLine(item.ROW2ND);
                    sb.AppendLine("</p>");
                    sb.AppendLine("</div>");
                    sb.AppendLine("</li>");
                }
                else
                {//rec
                    sb.AppendLine("<li class=\"right clearfix\">");
                    sb.AppendLine("<span class=\"chat-img pull-right\">");
                    sb.AppendLine("<div class=\"circleRec pull-left\"><p>" + item.MS_CORP_NAME_TH + "</p></div>");
                    sb.AppendLine("</span>");
                    sb.AppendLine("<div class=\"chat-body clearfix\">");
                    sb.AppendLine("<div class=\"header\">");
                    sb.AppendLine("<small class=\" text-muted\"><span class=\"glyphicon \"></span></small>");
                    sb.AppendLine("<strong class=\"pull-right primary-font\">" + item.ROW1ST + "</strong>");
                    sb.AppendLine("</div>");
                    sb.AppendLine("<p style=\"text-align: right;\">");
                    sb.AppendLine(item.ROW2ND);
                    sb.AppendLine("</p>");
                    sb.AppendLine("</div>");
                    sb.AppendLine("</li>");
                }

                tmpSEND_NO = item.SEND_NO;
            }

            sb.AppendLine("</ul>");
            sb.AppendLine("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region Autocomplete
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression">m =>m.XXX</param>
        /// <param name="config">
        /// var OnSelectXXX = function(e, selectValue, rowIndex, dataset){
        ///  //Todo
        /// };
        /// new AutocompleteScriptConfig{
        /// OnSelect="OnSelectXXX"
        /// }
        /// </param>
        /// <returns></returns>
        public static MvcHtmlString AutocompleteScriptFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, AutocompleteScriptConfig config)
        {
            if (config == null)
            {
                config = new AutocompleteScriptConfig();
            }
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string name = GetControlName(html, expressionText);
            var url = new UrlHelper(html.ViewContext.RequestContext);


            var sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");

            #region ready
            sb.AppendLine("$(document).ready(function () {");
            sb.AppendLine("$('#" + name + "').JAutocomplete({");
            var urlGet = url.Action("Index", "Autocomplete", new { Area = "Ux" });
            sb.Append("url: '");
            sb.Append(urlGet);
            sb.Append("'");
            if (!config.OnSelect.IsNullOrEmpty())
            {
                sb.AppendLine(",");
                sb.Append("select:");
                //function(e, selectValue, rowIndex, dataset)
                sb.Append(config.OnSelect);
            }
            sb.AppendLine("});");
            sb.AppendLine("});");
            #endregion

            sb.AppendLine("</script>");

            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region Timepicker
        public static MvcHtmlString GetVSMTimePickerFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            var sb = new StringBuilder();

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string label = GetControlLabel(metadata, expressionText);
            string name = GetControlName(html, expressionText);

            Func<TModel, TValue> method = expression.Compile();
            //string value = (method(html.ViewData.Model)).AsString();

            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendLine("<label class=\"control-label col-md-4\">" + label + "</label>");
            sb.AppendLine("<div class=\"col-md-8\">");
            sb.AppendLine("<div class=\"input-group bootstrap-timepicker timepicker\">");
            sb.AppendLine("<input name=\"" + name + "\" id=\"" + name + "\" type=\"text\" class=\"form-control\">");
            sb.AppendLine("<span class=\"input-group-addon\"><i class=\"glyphicon glyphicon-time\"></i></span>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString GetVSMTimePickerScriptFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool isFormat24 = true)
        {
            var sb = new StringBuilder();

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string label = GetControlLabel(metadata, expressionText);
            string name = GetControlName(html, expressionText);

            sb.AppendLine("<script type=\"text/javascript\"> ");
            sb.AppendLine("$(document).ready(function () {");
            sb.AppendLine("$(\"[name=" + name + "]\").timepicker({");
            sb.AppendLine("showMeridian: " + (!isFormat24 ? "true" : "false"));
            sb.AppendLine("}).val('');");
            sb.AppendLine("});");
            sb.AppendLine("</script> ");

            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region Icon Grid Process
        public static MvcHtmlString GetVSMIconProcess(this HtmlHelper html,
            StandardIconProcess status = StandardIconProcess.WAITING, int size = 300)
        {
            string sb = string.Empty;

            if (status != StandardIconProcess.WAITING)
            {
                sb += "<i class=\"ace-icon fa ";
                if (status == StandardIconProcess.PROCESSING)
                {
                    sb += FaIcons.FaSpinner + " fa-spin align-top bigger-" + size.AsString() + " blue\">";
                }
                else if (status == StandardIconProcess.COMPLETE)
                {
                    sb += FaIcons.FaCheckCircle + " align-top bigger-" + size.AsString() + " green\">";
                }
                else if (status == StandardIconProcess.INCOMPLETE)
                {
                    sb += FaIcons.FaTimesCircle + " align-top bigger-" + size.AsString() + " red\">";
                }
                else if (status == StandardIconProcess.WARNING)
                {
                    sb += FaIcons.FaExclamationTriangle + " align-top bigger-" + size.AsString() + " orange\">";
                }
                sb += "</i>";
            }

            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion
    }
}