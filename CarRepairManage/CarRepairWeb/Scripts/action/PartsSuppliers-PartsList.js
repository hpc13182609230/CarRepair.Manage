jQuery(document).ready(function () {
    Search.init();
    if (jQuery().datepicker) {
        $('.date-picker').datepicker();
    }
    App.initFancybox();
});

$('[name="add"]').click(function () {
    //调用ajax 初始化 分类 

    $('#portlet-config').removeClass('hide');
})

$('#portlet-config .close').click(function () {
    $('#portlet-config').addClass('hide');
})
