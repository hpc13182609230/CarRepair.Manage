﻿jQuery(document).ready(function () {
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
    var LoginToken = $('#LoginToken').val();
    var Contract = $('#Contract').val(); 
    var Mobile = $('#Mobile').val();
    var Tel = $('#Tel').val();
    var Phone = $('#Phone').val();
    var Order = $('#Order').val();
    var Address = $('#Address').val()||'';
    //var Content = $('#Content').val();
    var editor = CKEDITOR.instances.content_CKEditor;
    var Content = editor.getData();
    Content = encodeURIComponent(Content);

    var codeID = $('#codeID').find(".label-success").eq(0).attr("id");
    if (!codeID) {
        alert("所属省份未选择");
    }

    var radio =  $('[type="radio"]:checked');
    var PartsClassifyID = radio.val();
    var PartsClassifyIDNote = radio.attr('txt');

    var PicURL = $('#PicURL').val();

    $.ajax({
        type: 'POST',
        url: '/PartsSuppliers/PartsCompanySave',
        data: { ID: ID, Name: Name, LoginToken: LoginToken, Contract: Contract, Mobile: Mobile, Address: Address, Tel: Tel, Phone: Phone, Order: Order, Content: Content, PicURL: PicURL, codeID: codeID, PartsClassifyID: PartsClassifyID, PartsClassifyIDNote: PartsClassifyIDNote},
        success: function (data) {
            if (data > 0) {
                //$('#form').attr('partsID', data);
                alert('保存成功');
                location.reload();
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

