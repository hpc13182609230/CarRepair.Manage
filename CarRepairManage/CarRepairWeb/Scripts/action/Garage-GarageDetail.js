jQuery(document).ready(function () {
    if (jQuery().datepicker) {
        $('.date-picker').datepicker();
    }
    App.initFancybox();
});

//保存 修理厂信息
$('#save').click(function () {
    debugger;
    var ID = $('#form').attr("ModelID");
    var CompanyName = $('#CompanyName').val();
    var BossName = $('#BossName').val();
    var Phone = $('#Phone').val();
    var Mobile = $('#Mobile').val();
 
    var IsCheck = $('#IsCheck').val();
    var Remark = $('#Remark').val()||'';

    //var radio =  $('[type="radio"]:checked');
    //var PartsClassifyID = radio.val();
    //var PartsClassifyIDNote = radio.attr('txt');

    //var PicURL = $('#PicURL').val();

    $.ajax({
        type: 'POST',
        url: '/Garage/GarageSave',
        data: { ID: ID, CompanyName: CompanyName, BossName: BossName, Mobile: Mobile, Phone: Phone, IsCheck: IsCheck, Remark: Remark,},
        success: function (data) {
            if (data.result > 0) {
                //$('#form').attr('partsID', data);
                alert('保存成功');
                location.reload();
            }
            else {
                alert(data.message);
            }
        }
    });
});

$('#codeID').find("span").click(function () {
    debugger;
    var $this = $(this);
    var codeLbael = $('#codeID').find(".label");
    codeLbael.removeClass("label-success");
    $this.removeClass("label-default ").addClass("label-success");

})


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

