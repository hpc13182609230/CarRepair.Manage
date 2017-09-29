jQuery(document).ready(function () {
    if (jQuery().datepicker) {
        $('.date-picker').datepicker();
    }
    App.initFancybox();

});

//保存 配件商信息
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

//异步上传图片 此处代码已迁移到 bootstrap-fileupload
//$("#btnUpload").click(function () {
//    //var file = $('.btn.btn-file  [type="file"]');
//    //if ($("#upImage").val() == "") {
//    //    alert("请选择一个图片文件，再点击上传。");
//    //    return;
//    //}
//    debugger
//    var form = $('#FormUpload')[0];
//    var dataString = new FormData(form);
//    $.ajax({
//        url: '/PartsSuppliers/Upload',  //Server script to process data
//        type: 'POST',
//        data: dataString,
//        contentType: false,
//        processData: false,
//        success: function (data) {
//            debugger;
//            if (data) {
//                //alert(data.Url_Path);
//                $('#PicURL').val(data.Url_Path);
//                //$('#FormUpload img').attr("src",data.Url_Show);
//            }
//        }
//    });
//});

