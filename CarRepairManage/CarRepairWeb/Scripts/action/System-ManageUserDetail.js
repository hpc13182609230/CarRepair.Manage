//保存 配件商信息
$('#save').click(function () {
    debugger;
    var ID = $('#form').attr("ModelID");
    var LoginName = $('#LoginName').val();
    var Openid = $('#Openid').val();

    var codeID = $('#codeID').find(".label-success").eq(0).attr("id");
    if (!codeID) {
        alert("所属省份未选择");
    }

    //var radio = $('[type="radio"]:checked');

    $.ajax({
        type: 'POST',
        url: '/System/SaveManageUser',
        data: { ID: ID, LoginName: LoginName, Openid: Openid, AreaCodeID: codeID },
        success: function (data) {
            if (data > 0) {
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


$('#Permission').find(".accordion-inner p").click(function () {
    debugger;
    var $this = $(this);

    //codeLbael.removeClass("label-success");
    //$this.removeClass("label-default ").addClass("label-success");

})