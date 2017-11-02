/// <reference path="PartsType-PartsClassfy.js" />
jQuery(document).ready(function () {
    Search.init();
    if (jQuery().datepicker) {
        $('.date-picker').datepicker();
    }
    App.initFancybox();
});


//分类 打开 分类 层 并初始化
$('[name="ChangeType"]').click(function () {
    var $this = $(this);//修改分类 按钮
    var model = $('#ChangeTypeModel');//分类层 隐藏
    var PartsCompanyID = $this.attr("PartsCompanyID");//赋值 配件商 id
    model.find('[name = "save"]').attr("PartsCompanyID",PartsCompanyID);
    //调用ajax 初始化 分类 
    $.ajax({
        type: 'Get',
        url: '/PartsSuppliers/PartsCompanyClassifyGets?PartsCompanyID=' + PartsCompanyID,
        success: function (data) {
            if (data) {
                debugger;
                //var model = $('#ChangeTypeModel');
                model.find('[name = "save"]').attr("PartsClassifyCompanyID", data.ID);//赋值id

                var PartsClassifyIDs = data.PartsClassifyIDs.split(',');

                for (var i = 0; i < PartsClassifyIDs.length; i++) {
                    model.find("input[name=PartsClassify]").each(function () {
                        debugger;
                        if ($(this).val() == PartsClassifyIDs[i]) {
                            $(this).attr("checked", true);
                            $(this).parent().addClass('checked');
                        }
                    });
                }

            }
        }
    });

    model.removeClass('hide');
});
//关闭分类层
    $('#ChangeTypeModel .close').click(function () {
        var model = $('#ChangeTypeModel');
        model.addClass('hide'); 
    });

//保存分类层
    $('#ChangeTypeModel [name="save"]').click(function () {
        //调用ajax 保存 商户的  分类信息
        debugger
        var $this = $(this);//保存按钮
        var model = $('#ChangeTypeModel');//分类层 隐藏

        var ID = $this.attr("PartsClassifyCompanyID") == "" ? 0 : $this.attr("PartsClassifyCompanyID");
        var PartsCompanyID = $this.attr("PartsCompanyID");
        var PartsClassifyIDs = "";
        var Note = "";

        var checkboxs=model.find('input:checkbox:checked');
        if (checkboxs.length > 0) {
            checkboxs.each(function () {
                if ($(this).attr("checked")) {
                    PartsClassifyIDs += "," + $(this).val();
                    Note += "," + $(this).attr('txt');
                }
            });
            PartsClassifyIDs =PartsClassifyIDs.substr(1, PartsClassifyIDs.length - 1);
            Note= Note.substr(1, Note.length - 1);
            $.ajax({
                type: 'POST',
                url: '/PartsSuppliers/PartsCompanyClassifySave',
                data: { ID: ID, PartsCompanyID: PartsCompanyID, Note: Note, PartsClassifyIDs: PartsClassifyIDs },
                success: function (data) {
                    if (data > 0) {
                        //$('#form').attr('partsID', data);
                        alert('保存成功');
                        location.reload();
                    }
                }
            });
        }
        else {
            alert("请先选择分类!!!");
        }
    });


//删除 配件商
    $('[name="Delete"]').click(function () {
        //初始化，获取 分类的信息
        var $this = $(this);
        var parentTR = $this.parents('tr').eq(0);
        var ID = parentTR.attr('PartsCompanyID');

        if (confirm("确定要删除该数据吗？删除后无法恢复！")) {
            $.ajax({
                type: 'Get',
                url: '/PartsSuppliers/DeletePartsCompany?ID=' + ID,
                success: function (data) {
                    if (data) {
                        alert("删除成功");
                        location.reload();
                    }
                }
            });
        }
    })

//过期时间暂时不写
