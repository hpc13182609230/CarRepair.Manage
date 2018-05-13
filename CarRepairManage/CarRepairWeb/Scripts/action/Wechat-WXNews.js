//图文推送
$("[name=WXNewsPush]").click(function () {
    var that = $(this);
    var NewsID = that.attr("NewsID");
    var r=confirm("你确定推送图文吗，确认之后无法撤销");
    if (r)
    {
        $.ajax({
            type: 'GET',
            url: '/Wechat/WXNewsPush',
            data: { NewsID: NewsID },
            success: function (data) {
                if (data) {
                    alert("推送成功");
                }
                else {
                    alert('推送失败');
                }
            }
        });
     }
    else
    {
      alert("You pressed Cancel!");
    }
});