(function ($) {

    //* 
    $.fn.JSelecteList = function (options) {

        var defaults = {
            targetTo: "",
            targetUrl: "",
            optionLabel: '',
            setSelectValue: null,
            dataSet: null,
            itemCount: null
    };

        var settings = $.extend({}, defaults, options);

        return this.each(function () {
            // Plugin code would go here...
            var elem = $(this);
            var ctrlId = this.id;
            //elem.addClass("cascading-select");  //add ที่ MVC Helper
            elem.attr("autocomplete", "off");

            //Setting Init()
            // Handler for .ready() called.
            var defaultVal = $(this).val();
            if (defaultVal != "" ) {
                onStart(elem);
            }
           

            $(elem).change(function (e) {

                if (settings.targetTo == undefined) {
                    alert("Crtl target Id is undefined");
                    return;
                }
                if (settings.targetUrl == undefined) {
                    alert("Action target is undefined");
                    return;
                }
                //alert(settings.targetTo);
                //return;

                onStart(elem);
                
                //var html = $.map(lcns, function (lcn) {
                //    return '<option value="' + lcn + '">' + lcn + '</option>'
                //}).join('');

            });

            
            function onStart(elem) {

                settings.targetUrl = elem.data("url");
                settings.targetTo = elem.data("target");

                var val = $(elem).val();
                var target = $('#' + settings.targetTo);

               var $select =  $(target).html('');
               if (settings.optionLabel != null) {
                   $select.append($("<option></option>").val("").html( settings.optionLabel ));
               }

               getDataModel(elem, val, target);
            }

            function getDataModel(th, val, target) {
              
                $.ajax({
                    url:  settings.targetUrl,
                    data: { value: val },
                    type: "GET",
                    dataType: "json"
                })
               .done(function (jsonData) {
                   settings.dataSet = jsonData;

                // 0 => //Disabled : false
                   //Group :null
                   //Selected : false
                   //Text : "Bangkok3"
                   // Value : "1"
                   $.each(jsonData, function ( index , data) {
                       $(target).append($("<option></option>").val(data.Value).html(data.Text));
                       if (data.Selected) {
                           $(target).val(data.Value);
                       }
                   });
               })
               .fail(function (xhr, status, errorThrown) {
                   alert("Sorry, request a problem! (" + status + ")");
                   
               })
               // Code to run regardless of success or failure;
               .always(function (xhr, status) {
                   //alert("The request is complete!");
               });
            }

        });
    };


}(jQuery));

