/*!
 * Bootstrap-select v1.12.4 (http://silviomoreto.github.io/bootstrap-select)
 *
 * Copyright 2013-2017 bootstrap-select
 * Licensed under MIT (https://github.com/silviomoreto/bootstrap-select/blob/master/LICENSE)
 */

(function (root, factory) {
  if (typeof define === 'function' && define.amd) {
    // AMD. Register as an anonymous module unless amdModuleId is set
    define(["jquery"], function (a0) {
      return (factory(a0));
    });
  } else if (typeof module === 'object' && module.exports) {
    // Node. Does not work with strict CommonJS, but
    // only CommonJS-like environments that support module.exports,
    // like Node.
    module.exports = factory(require("jquery"));
  } else {
    factory(root["jQuery"]);
  }
}(this, function (jQuery) {

(function ($) {
  $.fn.selectpicker.defaults = {
    noneSelectedText: '',
    noneResultsText: 'ไม่มีผลลัพธ์ที่ตรงกับ {0}',
    countSelectedText: function (numSelected, numTotal) {
      return "{0} รายการที่เลือก";
    },
    maxOptionsText: function (numAll, numGroup) {
      return [
        'สุงสุด ({n} รายการ)',
        'กลุ่มสูงสุด ({n} รายการ)'
      ];
    },
    selectAllText: 'เลือกทั้งหมด',
    deselectAllText: 'ไม่เลือกทั้งหมด',
    multipleSeparator: ', '
  };
})(jQuery);


}));
