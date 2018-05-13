/// <reference path="PartsType-PartsClassfy.js" />
jQuery(document).ready(function () {
    Search.init();
    if (jQuery().datepicker) {
        $('.date-picker').datepicker();
    }
    App.initFancybox();
});


//省份切换
    $('#provinces').find("span").click(function () {
        debugger;
        var $this = $(this);
        var codeLbael = $('#provinces').find(".label");
        codeLbael.removeClass("label-success");
        $this.removeClass("label-default ").addClass("label-success");
        $('#codeID').val($this.attr('id'));
    })
    

