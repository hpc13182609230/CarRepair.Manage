jQuery(document).ready(function () {
    Search.init();
    if (jQuery().datepicker) {
        $('.date-picker').datepicker();
    }
    App.initFancybox();
});


//异步上传图片 此处代码已迁移到 bootstrap-fileupload
$("#UploadExcel").click(function () {
    //var file = $('.btn.btn-file  [type="file"]');
    //if ($("#upImage").val() == "") {
    //    alert("请选择一个图片文件，再点击上传。");
    //    return;
    //}
    debugger
    //var form = $('#FormUpload')[0];
    //var dataString = new FormData(form);
    var formData = new FormData();
    var file = document.getElementById("file").files[0];
    if (file === undefined) {
        alert("请先上传Excel表格");
        return;
    }
    formData.append("file", file);
    $.ajax({
        url: '/Garage/ImportExcel',  //Server script to process data
        type: 'POST',
        data: formData,
        async: true, // 是否异步
        processData: false, //processData 默认为false，当设置为true的时候,jquery ajax 提交的时候不会序列化 data，而是直接使用data
        contentType: false,
        success: function (data) {
            debugger;
            if (data.result) {
                alert("导入成功");
                window.location.reload();
            }
            else {
                alert(data.message);
            }
        }
    });
});

//查看详情
$("[name=goDetail]").click(function () {
    var that = $(this);
    var id = that.parents('tr').attr("id");
    //window.location.href = "/Garage/GarageDetail?id=" + id;
    window.open("/Garage/GarageDetail?id=" + id);
});