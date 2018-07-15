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


