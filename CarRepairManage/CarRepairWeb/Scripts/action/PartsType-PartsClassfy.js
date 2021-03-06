﻿jQuery(document).ready(function () {
    if (jQuery().datepicker) {
        $('.date-picker').datepicker();
    }
    App.initFancybox();

});

//新增配件商 打开隐藏层
$('#add').click(function () {
    $('#portlet-config').removeClass('hide')
})

//关闭隐藏层
$('#portlet-config .close').click(function () {
    $('#portlet-config').addClass('hide')
})

//获取信息 初始化 配件商隐藏层
$('[name="UpdateInfo"]').click(function () {
    //初始化，获取 分类的信息
    var $this = $(this);
    var parentTR = $this.parents('tr').eq(0);

    var PartsClassifyID = parentTR.attr('id');
    //var OptionID = $(this).attr("OptionID"); 无需初始化
    var Content = parentTR.find('td').eq(2).text();
    var Order = parentTR.find('td').eq(3).text();
    var Url_Show = parentTR.find('img').eq(0).attr('src');
    var Url_Path = parentTR.find('img').eq(0).attr('Url_Path');
    
    //给隐藏层赋值
    var model = $('#portlet-config');

    $('#save').attr("PartsClassifyID", PartsClassifyID);
    //var OptionID = $(this).attr("OptionID");
    $('#Content').val(Content);
    $('#Order').val(Order);
    model.find('#PicURL').val(Url_Path);
    model.find('img').eq(0).attr('src',Url_Show);

    model.removeClass('hide')
})

//删除分类
$('[name="Delete"]').click(function () {
    //初始化，获取 分类的信息

    var $this = $(this);
    var parentTR = $this.parents('tr').eq(0);
    var PartsClassifyID = parentTR.attr('id');

    if (confirm("确定要删除该数据吗？删除后无法恢复！")) {
        $.ajax({
            type: 'Get',
            url: '/PartsType/DeletePartsClassify?PartsClassifyID=' + PartsClassifyID,
            success: function (data) {
                if (data) {
                    alert("删除成功");
                    location.reload();
                }
            }
        });
    }
})


$('#save').click(function () {
    debugger;

    var ID = $(this).attr("PartsClassifyID");
    var OptionID = $(this).attr("OptionID");
    var Content = $('#Content').val();
    var PicURL = $('#PicURL').val();
    var Order = $('#Order').val();

    $.ajax({
        type: 'POST',
        url: '/PartsType/AddPartsClassify',
        data: { ID: ID, Content: Content, PicURL: PicURL, OptionID: OptionID, Order: Order },
        success: function (data) {
            if (data > 0) {
                //$('#form').attr('partsID', data);
                alert('保存成功');
                location.reload();
            }
        }
    });
});