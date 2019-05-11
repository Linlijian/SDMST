using System.Web.Optimization;

namespace WEBAPP
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.2.4.min.js")
                        );

            bundles.Add(new ScriptBundle("~/bundles/jqueryIE").Include(
                       "~/Scripts/jquery-1.12.4.min.js")
                       );

            bundles.Add(new ScriptBundle("~/bundles/myProject").Include(
                       "~/Scripts/jquery.globalize/globalize.js",
                       "~/Scripts/jquery.globalize/cultures/globalize.culture.en-US.js",
                       "~/Scripts/jquery.unobtrusive-ajax.min.js", //My Includes
                       "~/Scripts/sizzle.min.js" //My Includes
                       ));

            bundles.Add(new ScriptBundle("~/bundles/myCustom").Include(
                        "~/Scripts/App.js",
                        "~/Scripts/CustomClosePage.js") //My Includes
                       );

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/additional-methods.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.bootstrap.min.js",
                        "~/Scripts/CostomValidate.js"
                        )
                        );

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));



            //Original
            bundles.Add(new StyleBundle("~/bundlesContent/css").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/bootstrap-datepicker3.min.css",  //update nuget
                     "~/Content/font-awesome.min.css"//v4.7.0
                     //"~/Content/fontawesome.min.css"//v5.7.2
                    ));

            bundles.Add(new StyleBundle("~/bundlesContent/Wizard").Include(
                    "~/Content/wizard.css"));

            bundles.Add(new StyleBundle("~/bundlesContent/WizardWithoutHD").Include(
                    "~/Content/wizard-withouthd.css"));

            bundles.Add(new StyleBundle("~/bundlesContent/Custom").Include(
                    "~/Content/StyleSheet.css"));

            //Add New Theme ACE By Jubpas OnGithub

            //<!--[if lte IE 8]>
            bundles.Add(new ScriptBundle("~/bundles/IE8").Include(
                  "~/Content/template/ace/assets/js/html5shiv.min.js",
                  "~/Content/template/ace/assets/js/respond.min.js",
                  "~/Content/template/ace/assets/js/excanvas.min.js"));

            //<!--ace scripts-- >
            bundles.Add(new ScriptBundle("~/bundles/template").Include(
                    "~/Content/template/ace/assets/js/ace-elements.min.js",
                    "~/Content/template/ace/assets/js/ace.min.js"));


            //<!--ace styles -- >
            bundles.Add(new StyleBundle("~/bundlesContent/template").Include(
                     "~/Content/template/ace/assets/css/ace.css"));

            bundles.Add(new StyleBundle("~/bundlesContent/templateIE9").Include(
                     "~/Content/template/ace/assets/css/ace-part2.min.css",
                     "~/Content/template/ace/assets/css/ace-ie.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/templateSetting").Include(
                     "~/Content/template/ace/assets/js/ace-extra.min.js"));


            //<!--ace styles plugin-- >
            bundles.Add(new StyleBundle("~/bundlesContent/plugin").Include(
                     "~/Content/template/ace/assets/css/jquery-ui.custom.min.css",
                     "~/Content/template/ace/assets/css/chosen.min.css",
                      "~/Content/template/ace/assets/css/jquery.gritter.min.css",                     "~/Content/template/ace/assets/css/select2.min.css",
                     //"~/Content/template/ace/assets/css/datepicker.min.css",
                     //"~/Content/template/ace/assets/css/bootstrap-timepicker.min.css",
                     "~/Content/template/ace/assets/css/daterangepicker.min.css",
                     //"~/Content/template/ace/assets/css/bootstrap-datetimepicker.min.css",
                     "~/Content/template/ace/assets/css/colorpicker.min.css",
                     "~/Content/template/ace/assets/css/bootstrap-editable.min.css",
                     "~/Scripts/plugin/jquery-combogrid/resources/css/smoothness/jquery-ui-1.10.1.custom.css",
                     "~/Scripts/plugin/jquery-combogrid/resources/css/smoothness/jquery.ui.combogrid.css",
                     "~/Scripts/plugin/jquery-confirm/dist/jquery-confirm.min.css",
                     "~/Content/bootstrap-multiselect.css",
                     "~/Content/bootstrap-treeview.min.css"//CSS TreeView
                    ));

            //<!--bootstrap scripts plugin-- >
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                     "~/Scripts/bootstrap.js",
                     //"~/Content/template/ace/assets/js/bootstrap-datepicker.min.js",
                     //"~/Content/template/ace/assets/js/bootstrap-timepicker.min.js",
                     "~/Scripts/bootstrap-datepicker.min.js", //update nuget
                                                              //"~/Scripts/bootstrap-datepicker-globalize.js", //update nuget
                     "~/Scripts/respond.js"
                    ));
            bundles.Add(new ScriptBundle("~/bundles/waiting").Include("~/Scripts/bootstrap-waitingfor.min.js"));
            //<!--ace scripts plugin-- >
            bundles.Add(new ScriptBundle("~/bundles/plugin").Include(
                    "~/Content/template/ace/assets/js/jquery-ui.custom.min.js",
                    "~/Content/template/ace/assets/js/jquery.ui.touch-punch.min.js",
                    "~/Content/template/ace/assets/js/chosen.jquery.min.js",
                    "~/Content/template/ace/assets/js/fuelux.spinner.min.js",
                    "~/Content/template/ace/assets/js/moment.min.js",
                    "~/Content/template/ace/assets/js/bootbox.min.js",
                    "~/Content/template/ace/assets/js/spin.min.js",
                    "~/Content/template/ace/assets/js/daterangepicker.min.js",
                    "~/Content/template/ace/assets/js/bootstrap-colorpicker.min.js",
                    "~/Content/template/ace/assets/js/jquery.knob.min.js",
                    "~/Content/template/ace/assets/js/jquery.autosize.min.js",
                    "~/Content/template/ace/assets/js/jquery.inputlimiter.1.3.1.min.js",
                    "~/Content/template/ace/assets/js/jquery.maskedinput.min.js",
                    "~/Content/template/ace/assets/js/jquery.mobile.custom.min.js",
                    "~/Content/template/ace/assets/js/jquery.easypiechart.min.js",
                    "~/Content/template/ace/assets/js/jquery.gritter.min.js",
                    "~/Scripts/plugin/bootstrap-multiselect.min.js",
                    "~/Scripts/bootstrap-treeview.min.js",//Script TreeView

                    //External Include
                    "~/Scripts/plugin/jquery-combogrid/resources/plugin/jquery.ui.combogrid-1.6.3.js", //Plugun for jquery.ui.combogrid-1.6.3.js

                    //"~/Scripts/plugin/confirm/jquery.confirm.js", //Plugun for modal dialog confirm //https://myclabs.github.io/jquery.confirm/
                    "~/Scripts/plugin/jquery-browser/jquery.browser.min.js",//Import FixBug Jquery-Confirm
                    "~/Scripts/plugin/jquery-confirm/dist/jquery-confirm.min.js", //jquery-confirm plugin//https://github.com/craftpip/jquery-confirm
                    "~/Scripts/plugin/numeral/min/numeral.min.js", //Plugin for number format My Friend #CPE RU
                    "~/Scripts/plugin/NumberFormatKeyin.js", //Plugin for number format My Friend #CPE RU
                    "~/Scripts/plugin/cascading-dropdown/src/jquery.cascadingdropdown.js", //Plugin for cascadingdropdown
                    "~/Scripts/plugin/jquery.bootpag.min.js", //Require for Jubpas Plugin
                    "~/Scripts/plugin/forge.min.js",
                    "~/Scripts/plugin/jquery.jubpas*")); //Jubpas Plugin for suport team

            //bundles.Add(new ScriptBundle("~/bundles/Jubpas").Include(
            //      "~/Scripts/App.js" //My Includes
            //    ));


            //Jquery Webgrid By DataTables CSS
            bundles.Add(new StyleBundle("~/bundlesContent/DataTables").Include(
                    "~/Content/DataTables/css/dataTables.bootstrap.min.css",
                    "~/Content/DataTables/css/select.dataTables.min.css",
                    "~/Content/DataTables/css/buttons.dataTables.min.css",
                    "~/Content/DataTables/css/buttons.bootstrap.min.css",
                    "~/Content/DataTables/css/fixedColumns.bootstrap.min.css",
                    "~/Content/DataTables/css/scroller.bootstrap.min.css"
                    ));


            //Jquery Webgrid By DataTables Table plug-in for jQuery add By Jubpas
            bundles.Add(new ScriptBundle("~/bundles/DataTables").Include(
                 "~/Scripts/DataTables/jquery.dataTables.min.js",
                 "~/Scripts/DataTables/dataTables.bootstrap.min.js",
                 "~/Scripts/DataTables/ace/assets/js/dataTables.tableTools.min.js",
                 "~/Scripts/DataTables/ace/assets/js/dataTables.colVis.min.js",
                 "~/Scripts/DataTables/dataTables.select.min.js",
                 "~/Scripts/DataTables/dataTables.buttons.min.js",
                 "~/Scripts/DataTables/buttons.bootstrap.min.js",
                 "~/Scripts/DataTables/buttons.flash.min.js",
                 "~/Scripts/DataTables/jszip/jszip.min.js",
                 "~/Scripts/DataTables/buttons.html5.min.js",
                 "~/Scripts/DataTables/dataRender/datetime.js",
                 "~/Scripts/DataTables/dataTables.scroller.min.js",
                 "~/Scripts/DataTables/dataTables.fixedColumns.min.js",
                 "~/Scripts/DataTables/datatables.extend.js"
                ));

            //Script Packekg By Nuget



            //Responsive WYSIWYG Text Editor with jQuery and Bootstrap - LineControl Editor
            bundles.Add(new StyleBundle("~/bundlesContent/Editor").Include(
                    "~/Content/SCEditor/themes/default.min.css"
                    ));


            //Responsive WYSIWYG Text Editor with jQuery and Bootstrap - LineControl Editor
            bundles.Add(new ScriptBundle("~/bundles/Editor").Include(
                 "~/Scripts/plugin/SCEditor/jquery.sceditor.bbcode.min.js",
                 "~/Content/template/ace/assets/js/jquery.hotkeys.min.js",
                 "~/Content/template/ace/assets/js/bootstrap-wysiwyg.min.js"
                ));

            //Report Script
            bundles.Add(new ScriptBundle("~/bundles/Report").Include(
                 "~/Scripts/plugin/jquery-browser/jquery.browser.min.js",
                 "~/Scripts/plugin/iframe-auto-height/jquery-iframe-auto-height.js",
                 "~/Scripts/CustomReport.js"
                ));

            //BundleTable.EnableOptimizations = true;

        }
    }
}
