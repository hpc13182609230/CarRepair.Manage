jQuery(document).ready(function () {
    if (jQuery().datepicker) {
        $('.date-picker').datepicker();
    }
    App.initFancybox();

});

//新增 打开隐藏层
$('#add').click(function () {
    var model = $('#portlet-config');
    initPortlet();
    model.removeClass('hide');
})

//关闭隐藏层
$('#portlet-config .close').click(function () {
    $('#portlet-config').addClass('hide')
})

function initPortlet() {
    $('#save').attr("ID", 0);
    $('#Title').val("");
    $('#PicURL').attr("FileKey", "");
    $('#PicURL').attr("FileHash","");
    $('#PicURL').val();
}


//获取信息 初始化 隐藏层
$('[name="UpdateInfo"]').click(function () {
    //初始化，获取 分类的信息
    var $this = $(this);
    var parentTR = $this.parents('tr').eq(0);

    var ID = parentTR.attr('id');
    var Title = parentTR.find('td').eq(1).text();
    var URL = parentTR.find('video').eq(0).attr('src');
    var FileHash = parentTR.find('video').eq(0).attr('FileHash');
    var FileKey = parentTR.find('video').eq(0).attr('FileKey');

    //给隐藏层赋值
    var model = $('#portlet-config');

    $('#save').attr("ID", ID);
    $('#Title').val(Title);
    model.find('#PicURL').val(URL);
    model.find('img').eq(0).attr('src', URL);
    model.find('#PicURL').attr("FileKey", FileKey);
    model.find('#PicURL').attr("FileHash", FileHash);
    model.find('#video').attr("src", URL);

    model.removeClass('hide');
})

//删除
$('[name="Delete"]').click(function () {
  
    var $this = $(this);
    var parentTR = $this.parents('tr').eq(0);
    var ID = parentTR.attr('id');

    if (confirm("确定要删除该数据吗？删除后无法恢复！")) {
        $.ajax({
            type: 'Get',
            url: '/Media/DeleteShortVideo?ID=' + ID,
            success: function (data) {
                if (data) {
                    alert("删除成功");
                    location.reload();
                }
            }
        });
    }
})

//保存
$('#save').click(function () {
    debugger;
    var ID = $(this).attr("ID");
    var Title = $('#Title').val();
    var FileHash = $('#PicURL').attr("FileHash");
    var FileKey = $('#PicURL').attr("FileKey");
    var URL = $('#PicURL').val();

    $.ajax({
        type: 'POST',
        url: '/Media/SaveShortVideo',
        data: { ID: ID, Title: Title,FileKey: FileKey, FileHash: FileHash, URL: URL},
        success: function (data) {
            if (data > 0) {
                alert('保存成功');
                location.reload();
            }
        }
    });
});
