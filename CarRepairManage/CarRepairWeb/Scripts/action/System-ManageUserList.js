//查看详情
$("[name=goDetail]").click(function () {
    var that = $(this);
    var id = that.parents('tr').attr("id");
    //window.location.href = "/system/ManageUserDetail?id=" + id;
    window.open("/system/ManageUserDetail?id=" + id);
});