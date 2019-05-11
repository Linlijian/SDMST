using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace WEBAPP.Helper
{
    public static class HelperExtension
    {
        public static GridColumn SetKey(this GridColumn column, bool isKey = true)
        {
            column.IsKey = isKey;
            return column;
        }
        public static GridColumn SetCaseSensitive(this GridColumn column, bool isCaseSensitive = true)
        {
            column.CaseSensitive = isCaseSensitive;
            return column;
        }
        public static GridColumn SetButtonColumn(this GridColumn column, bool isButtonColumn = true)
        {
            if (isButtonColumn)
            {
                column.orderable = false;
                column.orderable = false;
            }
            column.IsButtonColumn = isButtonColumn;
            return column;
        }
        public static GridColumn SetOrderColumn(this GridColumn column, bool isOrderColumn = true)
        {
            column.IsOrderColumn = isOrderColumn;
            return column;
        }
        public static GridColumn SetNotVisible(this GridColumn column, bool visible = false, bool render = false)
        {
            column.visible = visible ? null : "false";
            column.visiblerender = render;
            return column;
        }
        public static GridColumn SetOrderable(this GridColumn column, bool orderable = true)
        {
            column.orderable = orderable;
            return column;
        }
        public static GridColumn SetEditable(this GridColumn column, bool editable = true)
        {
            column.IsEditable = editable;
            return column;
        }
        public static GridColumn SetFileType(this GridColumn column, params string[] type)
        {
            column.FileType = string.Join(",", type);
            return column;
        }
        public static GridColumn SetSelectOptions<T>(this GridColumn column, List<T> options)
        {
            column.SelectOptions = options;
            return column;
        }
        public static GridColumn SetEditableReadOnly(this GridColumn column, bool editableReadOnly = true)
        {
            column.EditableReadOnly = editableReadOnly;
            return column;
        }
        public static GridColumn SetCustomTrigger(this GridColumn column, bool customTrigger = true)
        {
            column.CustomTrigger = customTrigger;
            return column;
        }
        public static GridColumn SetRequired(this GridColumn column, bool required = true)
        {
            column.IsRequired = required;
            return column;
        }
        public static GridColumn SetAutocomplete(this GridColumn column, string Key)
        {
            if (column.AutocompleteConfig == null)
            {
                column.AutocompleteConfig = new GridColumnAutocompleteConfig();
            }
            column.AutocompleteConfig.Key = Key;
            if (!string.IsNullOrEmpty(column.data))
            {
                column.AutocompleteConfig.BindField = column.data;
            }
            return column;
        }
        public static GridColumn SetAutocomplete(this GridColumn column, GridColumnAutocompleteConfig config)
        {
            column.AutocompleteConfig = config;
            return column;
        }
        public static GridColumn SetMaxLength(this GridColumn column, int maxLength)
        {
            column.MaxLength = maxLength;
            return column;
        }
        public static GridColumn SetIdCard(this GridColumn column, bool isIdCard = true)
        {
            column.IsIdCard = isIdCard;
            return column;
        }
        public static GridColumn SetCustomOptionEditable(this GridColumn column, string optionEditable)
        {
            column.CustomOptionEditable = optionEditable;
            return column;
        }
        public static GridColumn AddCssClass(this GridColumn column, string cssClass, params string[] cssClasss)
        {
            column.className += cssClass;
            if (cssClasss != null && cssClasss.Count() > 0)
            {
                foreach (var item in cssClasss)
                {
                    column.className += item;
                }
            }
            return column;
        }
        public static GridColumn SetHeadWrap(this GridColumn column, bool isHeadWrap = true)
        {
            column.IsHeadNoWrap = !isHeadWrap;
            return column;
        }
        public static GridColumn SetNotSearchable(this GridColumn column, bool searchable = false)
        {
            column.searchable = Convert.ToString(searchable).ToLower();
            return column;
        }
        public static ButtonConfig AddCssClass(this ButtonConfig button, string parameter, params string[] parameters)
        {
            button.CssClass += " " + parameter;
            if (parameters != null)
            {
                foreach (var item in parameters)
                {

                    button.CssClass += " " + item;
                }
            }
            return button;
        }
        public static ButtonConfig AddIconCssClass(this ButtonConfig button, string parameter, params string[] parameters)
        {
            button.IconCssClass += " " + parameter;
            if (parameters != null)
            {
                foreach (var item in parameters)
                {

                    button.IconCssClass += " " + item;
                }
            }
            return button;
        }
        public static ButtonConfig AddParameter(this ButtonConfig button, params VSMParameter[] param)
        {
            button.Parameters.AddRange(param);
            return button;
        }
        public static ButtonConfig AddAttribute(this ButtonConfig button, string key, object value)
        {
            button.HtmlAttribute.Add(key, value);
            return button;
        }
        public static ButtonConfig AddAttributes(this ButtonConfig button, object htmlAttributes)
        {
            if (htmlAttributes != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    button.HtmlAttribute.Add(property.Name.Replace('\u005F', '-'), Convert.ToString(property.GetValue(htmlAttributes), CultureInfo.InvariantCulture));
                }
            }
            return button;
        }
    }
}