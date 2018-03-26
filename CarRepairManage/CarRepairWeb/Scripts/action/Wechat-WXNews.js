//图文推送
$("[name=WXNewsPush]").click(function () {
    var that = $(this);
    var NewsID = that.attr("NewsID");
    debugger;
    $.ajax({
        type: 'GET',
        url: '/Wechat/WXNewsPush',
        data: { NewsID: NewsID },
        success: function (data) {
            //alert(data);
            if (data) {
                alert("推送成功");
            }
            else {
                alert('推送失败');
            }

        }
    });
});