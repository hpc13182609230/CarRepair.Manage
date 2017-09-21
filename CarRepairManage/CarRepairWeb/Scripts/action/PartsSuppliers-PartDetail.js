jQuery(document).ready(function () {
    if (jQuery().datepicker) {
        $('.date-picker').datepicker();
    }
    App.initFancybox();

});

$('#save').click(function () {
    debugger;

    var ID = $('#form').attr("partsID");
    var Name = $('#Name').val();
    var Contract = $('#Contract').val();
    var Mobile = $('#Mobile').val();
    var Tel = $('#Tel').val();
    var Content = $('#Content').val();
    var PicURL = $('#PicURL').val();

    $.ajax({
        type: 'POST',
        url: '/PartsSuppliers/PartsCompanySave',
        data: { ID: ID, Name: Name, Contract: Contract, Mobile: Mobile, Tel: Tel, Content: Content, PicURL: PicURL },
        success: function (data) {
            if (data > 0) {
                //$('#form').attr('partsID', data);
                alert('保存成功');
                location.reload();
            }
        }
    });
});