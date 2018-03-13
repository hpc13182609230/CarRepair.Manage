jQuery(document).ready(function () {
    if (jQuery().datepicker) {
        $('.date-picker').datepicker();
    }
    App.initFancybox();

});

//新增配件商 打开隐藏层
$('#refresh').click(function () {
    $.ajax({
        type: 'GET',
        url: '/user/Sync_WXUser',
        success: function (data) {
            if (data > 0) {
                //$('#form').attr('partsID', data);
                alert('同步成功');
                location.reload();
            }
        }
    });
})

