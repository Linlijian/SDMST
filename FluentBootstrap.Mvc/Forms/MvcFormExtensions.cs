using FluentBootstrap.Forms;
using FluentBootstrap.Mvc;
using FluentBootstrap.Mvc.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using FluentBootstrap.Internals;
using System.Web.Mvc.Html;

namespace FluentBootstrap
{
    public static class MvcFormExtensions
    {
        public static ComponentBuilder<MvcBootstrapConfig<TModel>, Form> Form<TComponent, TModel>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, string actionName, string controllerName, FormMethod method = FormMethod.Post, object routeValues = null)
            where TComponent : Component, ICanCreate<Form>
        {
            return helper.Form()
                .SetAction(actionName, controllerName, routeValues)
                .SetFormMethod(method);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, Form> Form<TComponent, TModel>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, FormMethod method)
            where TComponent : Component, ICanCreate<Form>
        {
            return helper.Form()
                .SetAction(null)
                .SetFormMethod(method);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, Form> Form<TComponent, TModel>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, string action, FormMethod method)
            where TComponent : Component, ICanCreate<Form>
        {
            return helper.Form()
                .SetAction(action)
                .SetFormMethod(method);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, TForm> SetFormMethod<TModel, TForm>(
            this ComponentBuilder<MvcBootstrapConfig<TModel>, TForm> builder, FormMethod method)
            where TForm : Form
        {
            builder.GetComponent().MergeAttribute("method", HtmlHelper.GetFormMethodString(method));
            return builder;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, TForm> SetAction<TModel, TForm>(
            this ComponentBuilder<MvcBootstrapConfig<TModel>, TForm> builder, string actionName, string controllerName, object routeValues = null)
            where TForm : Form
        {
            RouteValueDictionary routeValueDictionary = routeValues == null ? new RouteValueDictionary() : routeValues as RouteValueDictionary;
            if (routeValueDictionary == null)
            {
                routeValueDictionary = new RouteValueDictionary(routeValues);
            }
            builder.SetAction(UrlHelper.GenerateUrl(null, actionName, controllerName, routeValueDictionary,
                builder.GetConfig().HtmlHelper.RouteCollection, builder.GetConfig().HtmlHelper.ViewContext.RequestContext, true));
            return builder;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, TForm> SetRoute<TModel, TForm>(
            this ComponentBuilder<MvcBootstrapConfig<TModel>, TForm> builder, string routeName, object routeValues = null)
            where TForm : Form
        {
            RouteValueDictionary routeValueDictionary = routeValues == null ? new RouteValueDictionary() : routeValues as RouteValueDictionary;
            if (routeValueDictionary == null)
            {
                routeValueDictionary = new RouteValueDictionary(routeValues);
            }
            builder.SetAction(UrlHelper.GenerateUrl(routeName, null, null, routeValueDictionary,
                builder.GetConfig().HtmlHelper.RouteCollection, builder.GetConfig().HtmlHelper.ViewContext.RequestContext, false));
            return builder;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, TForm> HideValidationSummary<TModel, TForm>(
            this ComponentBuilder<MvcBootstrapConfig<TModel>, TForm> builder, bool hideValidationSummary = true)
            where TForm : Form
        {
            builder.GetComponent().GetOverride<FormOverride<TModel>>().HideValidationSummary = hideValidationSummary;
            return builder;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, ValidationSummary<TModel>> ValidationSummary<TComponent, TModel>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, bool includePropertyErrors = false)
            where TComponent : Component, ICanCreate<FormControl>
        {
            return new ComponentBuilder<MvcBootstrapConfig<TModel>, ValidationSummary<TModel>>(helper.GetConfig(), new ValidationSummary<TModel>(helper));
        }

        public static ValidationSummary<TModel> IncludePropertyErrors<TModel>(this ValidationSummary<TModel> validationSummary, bool includePropertyErrors = false)
        {
            validationSummary.IncludePropertyErrors = includePropertyErrors;
            return validationSummary;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, FormGroup> FormGroup<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> labelExpression)
            where TComponent : Component, ICanCreate<FormGroup>
        {
            ComponentBuilder<MvcBootstrapConfig<TModel>, FormGroup> builder = helper.FormGroup();
            builder.GetComponent().ControlLabel = builder.GetHelper().ControlLabel(labelExpression).GetComponent();
            return builder;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, FormGroup> SetGroupLabel<TModel, TValue, TThis>(
            this ComponentBuilder<MvcBootstrapConfig<TModel>, FormGroup> builder, Expression<Func<TModel, TValue>> expression, Action<ControlLabel> labelAction = null)
        {
            ControlLabel controlLabel = GetControlLabel(builder.GetHelper(), expression).GetComponent();
            builder.GetComponent().ControlLabel = controlLabel;
            if (labelAction != null)
            {
                labelAction(controlLabel);
            }
            return builder;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, ControlLabel> ControlLabel<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression)
            where TComponent : Component, ICanCreate<ControlLabel>
        {
            return GetControlLabel(helper, expression);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, FormControlFor<TModel, TValue>> DisplayFor<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression,
            bool addHidden = true, bool addDescription = true, bool addValidationMessage = true, string templateName = null, object additionalViewData = null)
            where TComponent : Component, ICanCreate<FormControl>
        {
            return helper.EditorOrDisplayFor(false, expression, addDescription, addValidationMessage, templateName, additionalViewData, addHidden);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, FormControlFor<TModel, TValue>> EditorFor<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression,
            bool addDescription = true, bool addValidationMessage = true, string templateName = null, object additionalViewData = null)
            where TComponent : Component, ICanCreate<FormControl>
        {
            return helper.EditorOrDisplayFor(true, expression, addDescription, addValidationMessage, templateName, additionalViewData);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, FormControlFor<TModel, TValue>> EditorOrDisplayFor<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, bool editor, Expression<Func<TModel, TValue>> expression,
            bool addDescription = true, bool addValidationMessage = true, string templateName = null, object additionalViewData = null, bool addHidden = true)
            where TComponent : Component, ICanCreate<FormControl>
        {
            ComponentBuilder<MvcBootstrapConfig<TModel>, FormControlFor<TModel, TValue>> builder =
                new ComponentBuilder<MvcBootstrapConfig<TModel>, FormControlFor<TModel, TValue>>(helper.GetConfig(), new FormControlFor<TModel, TValue>(helper, editor, expression))
                    .AddHidden(addHidden)
                    .AddDescription(addDescription)
                    .AddValidationMessage(addValidationMessage)
                    .SetTemplateName(templateName)
                    .AddAdditionalViewData(additionalViewData);
            builder.GetComponent().Label = GetControlLabel(helper, expression).GetComponent();
            return builder;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, FormControlListFor<TModel, TValue>> DisplayListFor<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, IEnumerable<TValue>>> expression,
            ListType listType = ListType.Unstyled, bool addHidden = true, bool addDescription = true, bool addValidationMessage = true, string templateName = null, object additionalViewData = null)
            where TComponent : Component, ICanCreate<FormControl>
        {
            return helper.EditorOrDisplayListFor(false, expression, listType, addDescription, addValidationMessage, templateName, additionalViewData, addHidden);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, FormControlListFor<TModel, TValue>> EditorListFor<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, IEnumerable<TValue>>> expression,
            ListType listType = ListType.Unstyled, bool addDescription = true, bool addValidationMessage = true, string templateName = null, object additionalViewData = null)
            where TComponent : Component, ICanCreate<FormControl>
        {
            return helper.EditorOrDisplayListFor(true, expression, listType, addDescription, addValidationMessage, templateName, additionalViewData);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, FormControlListFor<TModel, TValue>> EditorOrDisplayListFor<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, bool editor, Expression<Func<TModel, IEnumerable<TValue>>> expression,
            ListType listType = ListType.Unstyled, bool addDescription = true, bool addValidationMessage = true, string templateName = null, object additionalViewData = null, bool addHidden = true)
            where TComponent : Component, ICanCreate<FormControl>
        {
            ComponentBuilder<MvcBootstrapConfig<TModel>, FormControlListFor<TModel, TValue>> builder =
                new ComponentBuilder<MvcBootstrapConfig<TModel>, FormControlListFor<TModel, TValue>>(helper.GetConfig(), new FormControlListFor<TModel, TValue>(helper, editor, expression, listType))
                    .AddHidden(addHidden)
                    .AddDescription(addDescription)
                    .AddValidationMessage(addValidationMessage)
                    .SetTemplateName(templateName)

                    .AddAdditionalViewData(additionalViewData);
            builder.GetComponent().Label = GetControlLabel(helper, expression).GetComponent();
            return builder;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, TFormControlFor> SetPlaceholderFor<TModel, TFormControlFor>(
           this ComponentBuilder<MvcBootstrapConfig<TModel>, TFormControlFor> builder, string placeholder = "")
           where TFormControlFor : FormControlForBase
        {
            builder.GetComponent().Placeholder = placeholder;
            return builder;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, TFormControlFor> AddHidden<TModel, TFormControlFor>(
            this ComponentBuilder<MvcBootstrapConfig<TModel>, TFormControlFor> builder, bool addHidden = true)
            where TFormControlFor : FormControlForBase
        {
            builder.GetComponent().AddHidden = addHidden;
            return builder;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, TFormControlFor> AddStaticClass<TModel, TFormControlFor>(
            this ComponentBuilder<MvcBootstrapConfig<TModel>, TFormControlFor> builder, bool addStaticClass = true)
            where TFormControlFor : FormControlForBase
        {
            builder.GetComponent().ToggleCss(Css.FormControlStatic, addStaticClass);
            return builder;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, TFormControlFor> AddFormControlClass<TModel, TFormControlFor>(
            this ComponentBuilder<MvcBootstrapConfig<TModel>, TFormControlFor> builder, bool addFormControlClass = true)
            where TFormControlFor : FormControlForBase
        {
            builder.GetComponent().AddFormControlClass = addFormControlClass;
            return builder;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, TFormControlFor> AddDescription<TModel, TFormControlFor>(
            this ComponentBuilder<MvcBootstrapConfig<TModel>, TFormControlFor> builder, bool addDescription = true)
            where TFormControlFor : FormControlForBase
        {
            builder.GetComponent().AddDescription = addDescription;
            return builder;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, TFormControlFor> AddValidationMessage<TModel, TFormControlFor>(
            this ComponentBuilder<MvcBootstrapConfig<TModel>, TFormControlFor> builder, bool addValidationMessage = true)
            where TFormControlFor : FormControlForBase
        {
            builder.GetComponent().AddValidationMessage = addValidationMessage;
            return builder;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, TFormControlFor> SetTemplateName<TModel, TFormControlFor>(
            this ComponentBuilder<MvcBootstrapConfig<TModel>, TFormControlFor> builder, string templateName)
            where TFormControlFor : FormControlForBase
        {
            builder.GetComponent().TemplateName = templateName;

            return builder;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, TFormControlFor> AddAdditionalViewData<TModel, TFormControlFor>(
            this ComponentBuilder<MvcBootstrapConfig<TModel>, TFormControlFor> builder, object additionalViewData)
            where TFormControlFor : FormControlForBase
        {
            builder.GetComponent().AdditionalViewData = additionalViewData;
            return builder;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, Input> InputFor<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression, FormInputType inputType = FormInputType.Text, bool isRequired = false, bool addValidateMessage = false)
            where TComponent : Component, ICanCreate<Input>
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.GetConfig().HtmlHelper.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string name = GetControlName(helper, expressionText);
            string label = GetControlLabel(metadata, expressionText);
            if (isRequired)
            {
                addValidateMessage = isRequired;
            }
            var validMsgTag = addValidateMessage ? GetValidateMessage(helper, expression) : MvcHtmlString.Empty;

            return helper.Input(name, label, metadata.Model, metadata.EditFormatString, inputType, addValidateMessage, validMsgTag, isRequired);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, Input> DatepickerFor<TComponent, TModel, TValue>(
           this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression, FormInputType inputType = FormInputType.Text, bool isRequired = false, bool addValidateMessage = false, bool isCustom = false)
           where TComponent : Component, ICanCreate<Input>
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.GetConfig().HtmlHelper.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string name = GetControlName(helper, expressionText);
            string label = GetControlLabel(metadata, expressionText);
            if (isRequired)
            {
                addValidateMessage = isRequired;
            }
            var validMsgTag = addValidateMessage ? GetValidateMessage(helper, expression) : MvcHtmlString.Empty;
            return helper.Datepicker(name, label, metadata.Model, metadata.EditFormatString, inputType, addValidateMessage, validMsgTag, isRequired, isCustom);
        }

        /// <summary>
        /// Add By Jubpas
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="inputType"></param>
        /// <returns></returns>
        public static ComponentBuilder<MvcBootstrapConfig<TModel>, Input> InputFileuploadFor<TComponent, TModel, TValue>(
           this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression, FormInputType inputType = FormInputType.File, bool isRequired = false, bool addValidateMessage = false, string allowExt = "")
           where TComponent : Component, ICanCreate<Input>
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.GetConfig().HtmlHelper.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string name = GetControlName(helper, expressionText);
            string label = GetControlLabel(metadata, expressionText);
            if (isRequired)
            {
                addValidateMessage = isRequired;
            }
            var validMsgTag = addValidateMessage ? GetValidateMessage(helper, expression) : MvcHtmlString.Empty;

            //var allowType = new List<string>();
            //var sysConfig = helper.GetConfig().HtmlHelper.ViewContext.HttpContext.Session["SYS_ConfigFile"] as UtilityLib.SystemConfigFile;
            //if (sysConfig != null && sysConfig.Details != null && sysConfig.Details.Count > 0)
            //{
            //    var nSysConfig = sysConfig.Details;
            //    if (!string.IsNullOrEmpty(allowExt))
            //    {
            //        var types = allowExt.Replace(".", "").Split(',');
            //        nSysConfig = sysConfig.Details.Where(m => types.Contains(m.FILE_TYPE)).ToList();
            //    }
            //    foreach (var item in nSysConfig)
            //    {
            //        var type = string.Empty;
            //        if (item.FILE_TYPE.IndexOf('.') == -1)
            //        {
            //            type = ContentType("." + item.FILE_TYPE);
            //        }
            //        else
            //        {
            //            type = ContentType(item.FILE_TYPE);
            //        }
            //        if (!allowType.Contains(type))
            //        {
            //            allowType.Add(type);
            //        }
            //    }
            //}
            //var allowFile = string.Join(",", allowType);
            var allowFile = allowExt;
            return helper.InputFileupload(name, label, metadata.Model, metadata.EditFormatString, addValidateMessage, validMsgTag, isRequired, allowFile);
        }

        /// <summary>
        /// Add By Jubpas
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="inputType"></param>
        /// <returns></returns>
        public static ComponentBuilder<MvcBootstrapConfig<TModel>, Input> InputNumberFor<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression, FormInputType inputType = FormInputType.Text, bool isRequired = false, bool addValidateMessage = false, int digits = 0, bool positiveInt = false)
            where TComponent : Component, ICanCreate<Input>
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.GetConfig().HtmlHelper.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string name = GetControlName(helper, expressionText);
            string label = GetControlLabel(metadata, expressionText);
            if (isRequired)
            {
                addValidateMessage = isRequired;
            }
            var validMsgTag = addValidateMessage ? GetValidateMessage(helper, expression) : MvcHtmlString.Empty;
            return helper.InputNumber(name, label, metadata.Model, metadata.EditFormatString, addValidateMessage, validMsgTag, isRequired, digits, positiveInt);
        }
        /// <summary>
        /// Add By Jubpas
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="inputType"></param>
        /// <returns></returns>
        public static ComponentBuilder<MvcBootstrapConfig<TModel>, Input> InputJAutocompleteFor<TComponent, TModel, TValue>(
           this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression, FormInputType inputType = FormInputType.Text, bool isRequired = false, bool addValidateMessage = false, bool isCustom = false)
           where TComponent : Component, ICanCreate<Input>
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.GetConfig().HtmlHelper.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string name = GetControlName(helper, expressionText);
            string label = GetControlLabel(metadata, expressionText);
            if (isRequired)
            {
                addValidateMessage = isRequired;
            }
            var validMsgTag = addValidateMessage ? GetValidateMessage(helper, expression) : MvcHtmlString.Empty;
            return helper.InputJAutocomplete(name, label, metadata.Model, metadata.EditFormatString, addValidateMessage, validMsgTag, isRequired, isCustom);
        }



        public static ComponentBuilder<MvcBootstrapConfig<TModel>, Input> PasswordFor<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression, bool isRequired = false, bool addValidateMessage = false)
            where TComponent : Component, ICanCreate<Input>
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.GetConfig().HtmlHelper.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string name = GetControlName(helper, expressionText);
            string label = GetControlLabel(metadata, expressionText);
            if (isRequired)
            {
                addValidateMessage = isRequired;
            }
            var validMsgTag = addValidateMessage ? GetValidateMessage(helper, expression) : MvcHtmlString.Empty;
            return helper.Input(name, label, null, null, FormInputType.Password, addValidateMessage, validMsgTag, isRequired);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, CheckedControl> CheckBoxFor<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression, bool isNameInLabel = true)
            where TComponent : Component, ICanCreate<CheckedControl>
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.GetConfig().HtmlHelper.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string name = GetControlName(helper, expressionText);
            string label = GetControlLabel(metadata, expressionText);
            bool isChecked = false;
            if (metadata.Model != null)
            {
                if (!bool.TryParse(metadata.Model.ToString(), out isChecked))
                {
                    if (metadata.Model.GetType() == typeof(string) && metadata.Model.ToString() == "Y")
                    {
                        isChecked = true;
                    }
                }
            }
            return isNameInLabel ? helper.CheckBox(name, label, null, isChecked) : helper.CheckBox(name, null, label, isChecked);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, CheckedControl> RadioFor<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression, object value = null, bool isNameInLabel = true)
            where TComponent : Component, ICanCreate<CheckedControl>
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.GetConfig().HtmlHelper.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string name = GetControlName(helper, expressionText);
            string label = GetControlLabel(metadata, expressionText);
            string valueString = Convert.ToString(value, (IFormatProvider)CultureInfo.CurrentCulture);
            bool isChecked = metadata.Model != null && !string.IsNullOrEmpty(name) && string.Equals(metadata.Model.ToString(), valueString, StringComparison.OrdinalIgnoreCase);
            return isNameInLabel ? helper.Radio(name, label, null, value, isChecked) : helper.Radio(name, null, label, value, isChecked);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, CheckedControl> RadioFor<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression, Expression<Func<TModel, TValue>> expressionValue = null, object value = null, bool isNameInLabel = false)
            where TComponent : Component, ICanCreate<CheckedControl>
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.GetConfig().HtmlHelper.ViewData);
            ModelMetadata metadataValue = ModelMetadata.FromLambdaExpression(expressionValue, helper.GetConfig().HtmlHelper.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string expressionValueText = ExpressionHelper.GetExpressionText(expressionValue);
            string name = GetControlName(helper, expressionText);
            string label = GetControlLabel(metadataValue, expressionValueText);
            value = metadataValue.Model != null && value == null ? metadataValue.Model.ToString() : value;
            string valueString = Convert.ToString(value, (IFormatProvider)CultureInfo.CurrentCulture);
            bool isChecked = metadata.Model != null && !string.IsNullOrEmpty(name) && string.Equals(metadata.Model.ToString(), valueString, StringComparison.OrdinalIgnoreCase);
            return isNameInLabel ? helper.Radio(name, label, null, value, isChecked) : helper.Radio(name, null, label, value, isChecked);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, Select> SelectFor<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression, params string[] options)
            where TComponent : Component, ICanCreate<Select>
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.GetConfig().HtmlHelper.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string name = GetControlName(helper, expressionText);
            string label = GetControlLabel(metadata, expressionText);
            ComponentBuilder<MvcBootstrapConfig<TModel>, Select> builder = helper.Select(name, label);
            if (metadata.Model != null && !string.IsNullOrEmpty(name))
            {
                // Add the model value before adding options so they'll get selected on a match
                builder.GetComponent().ModelValue = metadata.Model.ToString();
            }
            if (options == null)
            {
                options = new string[] {

                };
            }
            return builder.AddOptions(options);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, Select> SelectFor<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression, IEnumerable<KeyValuePair<string, string>> options, bool isRequired = false, bool IsSearch = false, bool includeEmptyOptions = true)
            where TComponent : Component, ICanCreate<Select>
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.GetConfig().HtmlHelper.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string name = GetControlName(helper, expressionText);
            string label = GetControlLabel(metadata, expressionText);
            var validMsgTag = isRequired ? GetValidateMessage(helper, expression) : MvcHtmlString.Empty;

            List<KeyValuePair<string, string>> nOptions = null;
            if (options != null)
            {
                nOptions = options.ToList();
            }
            else
            {
                nOptions = new List<KeyValuePair<string, string>>();
            }


            if (includeEmptyOptions)
            {
                if (nOptions != null && IsSearch)
                {
                    nOptions.Insert(0, new KeyValuePair<string, string>(Translation.CenterLang.Center.SelectDefaultSearch, ""));
                }
                else if (nOptions != null && isRequired)
                {
                    nOptions.Insert(0, new KeyValuePair<string, string>(Translation.CenterLang.Center.SelectDefaultRequired, ""));
                }
                else
                {
                    nOptions.Insert(0, new KeyValuePair<string, string>("", ""));
                }
            }

            ComponentBuilder<MvcBootstrapConfig<TModel>, Select> builder = helper.Select(name, label, isRequired: isRequired, validateMassegeTag: validMsgTag);
            if (metadata.Model != null && !string.IsNullOrEmpty(name))
            {
                // Add the model value before adding options so they'll get selected on a match
                builder.GetComponent().ModelValue = metadata.Model.ToString();
            }
            return builder.AddOptions(nOptions);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, Select> SelectFor<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList = null, bool isRequired = false, bool IsSearch = false, bool addValidateMessage = false, bool includeEmptyOptions = true)
            where TComponent : Component, ICanCreate<Select>
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.GetConfig().HtmlHelper.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string name = GetControlName(helper, expressionText);
            string label = GetControlLabel(metadata, expressionText);

            if (isRequired)
            {
                addValidateMessage = isRequired;
            }
            var validMsgTag = addValidateMessage ? GetValidateMessage(helper, expression) : MvcHtmlString.Empty;
            //var validMsgTag = isRequired ? GetValidateMessage(helper, expression) : MvcHtmlString.Empty;

            List<SelectListItem> nSelectList = null;
            if (selectList != null)
            {
                nSelectList = selectList.ToList();
            }
            else
            {
                nSelectList = new List<SelectListItem>();
            }

            if (includeEmptyOptions)
            {
                var sli = new SelectListItem();
                sli.Value = "";
                if (nSelectList != null && IsSearch)
                {
                    sli.Text = Translation.CenterLang.Center.SelectDefaultSearch;
                    nSelectList.Insert(0, sli);
                }
                else if (nSelectList != null && isRequired)
                {
                    sli.Text = Translation.CenterLang.Center.SelectDefaultRequired;
                    nSelectList.Insert(0, sli);
                }
                else
                {
                    nSelectList.Insert(0, sli);
                }
            }


            ComponentBuilder<MvcBootstrapConfig<TModel>, Select> builder = helper.Select(name, label, isRequired: isRequired, validateMassegeTag: validMsgTag);
            if (metadata.Model != null && !string.IsNullOrEmpty(name))
            {
                // Add the model value before adding options so they'll get selected on a match
                builder.GetComponent().ModelValue = metadata.Model.ToString();
            }
            builder.GetComponent().MergeAttribute("data-issearch", IsSearch.ToString().ToLower());
            builder.GetComponent().MergeAttribute("data-isrequired", isRequired.ToString().ToLower());
            return builder.AddOptions(nSelectList);
        }
        public static ComponentBuilder<MvcBootstrapConfig<TModel>, Select> MultiSelectFor<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList = null, bool isRequired = false, bool IsSearch = false, bool isCustom = false, bool dropRight = false)
            where TComponent : Component, ICanCreate<Select>
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.GetConfig().HtmlHelper.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string name = GetControlName(helper, expressionText);
            string label = GetControlLabel(metadata, expressionText);
            var validMsgTag = isRequired ? GetValidateMessage(helper, expression) : MvcHtmlString.Empty;

            string type = string.Empty;

            if (selectList != null && isRequired)
            {
                type += "required";
            }
            else if (IsSearch)
            {
                type += "search";
            }

            ComponentBuilder<MvcBootstrapConfig<TModel>, Select> builder = helper.Select(name, label, isRequired: isRequired, validateMassegeTag: validMsgTag).SetMultiple();
            builder.AddAttribute("data-role", "vsmmultiselect");
            builder.AddAttribute("data-dropright", dropRight.ToString().ToLower());
            if (!string.IsNullOrEmpty(type))
            {
                builder.AddAttribute("data-multiselect_type", type);
            }
            if (metadata.Model != null && !string.IsNullOrEmpty(name))
            {
                // Add the model value before adding options so they'll get selected on a match
                builder.GetComponent().ModelValue = metadata.Model.ToString();
            }
            return builder.AddOptions(selectList);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, Select> Select<TComponent, TModel>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, string name, string label, IEnumerable<SelectListItem> selectList)
            where TComponent : Component, ICanCreate<Select>
        {
            return helper.Select(name, label)
                .AddOptions(selectList);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, Select> AddOptions<TModel>(
            this ComponentBuilder<MvcBootstrapConfig<TModel>, Select> builder, IEnumerable<SelectListItem> selectList)
        {
            if (selectList != null)
            {
                foreach (SelectListItem item in selectList)
                {
                    var item1 = item; // Avoid foreach variable access in closure
                    builder.AddChild(x => x.SelectOption(item1.Text, item1.Value, item1.Selected));
                }
            }
            return builder;
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, TextArea> TextAreaFor<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression, int? rows = null, bool isRequired = false, bool addValidateMessage = false)
            where TComponent : Component, ICanCreate<TextArea>
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.GetConfig().HtmlHelper.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string name = GetControlName(helper, expressionText);
            string label = GetControlLabel(metadata, expressionText);
            if (isRequired)
            {
                addValidateMessage = isRequired;
            }
            var validMsgTag = addValidateMessage ? GetValidateMessage(helper, expression) : MvcHtmlString.Empty;
            return helper.TextArea(name, label, metadata.Model, null, rows, addValidateMessage, validMsgTag, isRequired);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, HiddenFor<TModel, TValue>> HiddenFor<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression)
            where TComponent : Component, ICanCreate<Hidden>
        {
            return new ComponentBuilder<MvcBootstrapConfig<TModel>, HiddenFor<TModel, TValue>>(helper.GetConfig(), new HiddenFor<TModel, TValue>(helper, expression));
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, FormControl> FormControl<TComponent, TModel, TValue>(
            this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> labelExpression)
            where TComponent : Component, ICanCreate<FormControl>
        {
            return new ComponentBuilder<MvcBootstrapConfig<TModel>, FormControl>(helper.GetConfig(), helper.FormControl().GetComponent())
                .SetControlLabel(labelExpression);
        }

        public static ComponentBuilder<MvcBootstrapConfig<TModel>, TFormControl> SetControlLabel<TModel, TValue, TFormControl>(
            this ComponentBuilder<MvcBootstrapConfig<TModel>, TFormControl> builder, Expression<Func<TModel, TValue>> expression, Action<ControlLabel> labelAction = null)
            where TFormControl : FormControl
        {
            ControlLabel controlLabel = GetControlLabel(builder.GetHelper(), expression).For(TagBuilder.CreateSanitizedId(builder.GetComponent().GetAttribute("name"))).GetComponent();
            if (labelAction != null)
            {
                labelAction(controlLabel);
            }
            builder.GetComponent().Label = controlLabel;
            return builder;
        }

        private static ComponentBuilder<MvcBootstrapConfig<TModel>, ControlLabel> GetControlLabel<TComponent, TModel, TValue>(
            BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression)
            where TComponent : Component
        {
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.GetConfig().HtmlHelper.ViewData);
            string name = GetControlName(helper, expressionText);
            string label = GetControlLabel(metadata, expressionText);
            return new MvcBootstrapHelper<TModel>(helper.GetConfig().HtmlHelper).ControlLabel(label).For(TagBuilder.CreateSanitizedId(name));
        }

        private static string GetControlName<TComponent, TModel>(BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, string expressionText)
            where TComponent : Component
        {
            return helper.GetConfig().HtmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText);
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

        public static ComponentBuilder<TConfig, TTag> AddLabelCss<TConfig, TTag>(this ComponentBuilder<TConfig, TTag> builder, params string[] cssClasses)
            where TConfig : BootstrapConfig
            where TTag : FormControl
        {
            var formControl = builder.GetComponent();
            formControl.AddLabelCss(cssClasses);

            return builder;
        }

        private static MvcHtmlString GetValidateMessage<TComponent, TModel, TValue>(BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression)
            where TComponent : Component
        {
            return helper.GetConfig().HtmlHelper.ValidationMessageFor(expression);
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


    }
}
