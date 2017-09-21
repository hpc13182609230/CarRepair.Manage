jQuery(document).ready(function () {
    if (jQuery().datepicker) {
        $('.date-picker').datepicker();
    }
    App.initFancybox();

});

$('#add').click(function () {
    $('#portlet-config').removeClass('hide')
})

$('#portlet-config .close').click(function () {

    $('#portlet-config').addClass('hide')
})


$('#save').click(function () {
    debugger;

    var ID = $(this).attr("PartsClassifyID");
    var OptionID = $(this).attr("OptionID");
    var Content = $('#Content').val();
    var PicURL = $('#PicURL').val();

    $.ajax({
        type: 'POST',
        url: '/PartsType/AddPartsClassify',
        data: { ID: ID, Content: Content, PicURL: PicURL, OptionID: OptionID },
        success: function (data) {
            if (data > 0) {
                //$('#form').attr('partsID', data);
                alert('保存成功');
                location.reload();
            }
        }
    });
});