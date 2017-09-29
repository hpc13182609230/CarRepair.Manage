$('#btnLogin').click(function () {
    var username = $('#username').val();
    var password = $('#password').val();

    $.ajax({
        type: 'POST',
        url: '/Account/LoginFunction',
        data: { username: username, password: password },
        success: function (data) {
            debugger;
            if (data.result==1) {
                window.location.href = "/Home/Index";
            }
            else {
                alert(data.message);
            }
        }
    });

});