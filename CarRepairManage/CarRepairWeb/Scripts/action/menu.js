$('#add_menu').click(function () {
    debugger
    var menu_num = $('.tabs-nav').find('li').length;
    if (menu_num > 3) {
        alert('一级菜单最多只能三个，无法继续添加');
    }
    else {
        $('.wx-mc-modal').show();
    }
});

$('.wx-mc-modal-close').click(function () {
    $('.wx-mc-modal').hide();
});

$('.wx-mc-modal').find('.wx-cancel').click(function () {
    $('.wx-mc-modal').hide();
});

$('.wx-mc-modal').find('.wx-ok').click(function () {
    var model = $('.wx-mc-modal');
    debugger;
    var ID = 0;
    var parentID = model.attr('parentMenuID');
    var MenuType = model.find('input[type="radio"][name="MenuType"]:checked').val();
    var Name = $('#Name').val();
    var KeyForClick = $('#KeyForClick').val();
    var Url = $('#Url').val();
    var MediaID = '';
    $.ajax({
        type: 'POST',
        url: '/Wechat/SaveMenu',
        data: { ID: ID, parentID: parentID, MenuType: MenuType, Name: Name, KeyForClick: KeyForClick, Url: Url, MediaID: MediaID },
        success: function (data) {
            //alert(data);
            if (data > 0) {
                location.reload();
            }
            else {
                alert('保存失败');
            }

        }
    });
});

$('.wx-reply-list').find('.wx-ok').click(function () {
    debugger;
    var model = $(this).parents('.wx-reply-list-item');

    var ID = model.find('.wx-reply-list-item-title').attr('menuid');
    var parentID = model.find('.wx-reply-list-item-title').attr('parentmenuid');
    var MenuType = model.find('input[type="radio"]:checked').val();
    var Name = model.find('.wx-reply-list-item-title').find('input').val();
    var KeyForClick = model.find('textarea').val();
    var Url = model.find('.mc-modal-content-replycontent').find('.wx-reply-keyword-body').val();
    var MediaID = '';
    $.ajax({
        type: 'POST',
        url: '/Wechat/SaveMenu',
        data: { ID: ID, parentID: parentID, MenuType: MenuType, Name: Name, KeyForClick: KeyForClick, Url: Url, MediaID: MediaID },
        success: function (data) {
            //alert(data);
            if (data > 0) {
                location.reload();
            }
            else {
                alert('保存失败');
            }

        }
    });
});



$('input[type="radio"]').click(function () {
    debugger;
    var $this = $(this);
    var next = $this.parent().nextAll('div').eq(0);

    var MenuType = $this.val();
    if (MenuType == 'click') {//点击发送消息
        next.find('div').eq(0).show();
        next.find('div').eq(1).hide();
    }
    else if (MenuType == 'view') {//点击页面跳转
        next.find('div').eq(1).show();
        next.find('div').eq(0).hide();
    }
    else if (MenuType == 'miniprogram') {//点击页面跳转
        next.find('div').eq(1).show();
        next.find('div').eq(1).hide();
    }
    

})

//添加子菜单
$('#childMenuList').find('.wx-reply-list-item-title').find('input').click(function () {
    debugger;
    var childMenu_num = $('#childMenuList').find('.wx-reply-list-item').length;
    if (childMenu_num >= 5) {
        alert('已达到子菜单上限');
        return;
    }
    var parentMenuID = $('.wx-reply-list').eq(0).find('.wx-reply-list-item-title').eq(1).attr('menuid');
    $('.wx-mc-modal').attr('parentMenuID', parentMenuID);
    $('.wx-mc-modal').show();
})

$('.wx-reply-list').find('.wx-cancel').click(function () {
    debugger;
    var model = $(this).parents('.wx-reply-list-item');
    var ID = model.find('.wx-reply-list-item-title').attr('menuid');
    deleteMenu(ID);

});


//删除菜单
function deleteMenu(id) {
    $.ajax({
        type: 'POST',
        url: '/Wechat/DeleteMenu',
        data: { id: id },
        success: function (data) {
            //alert(data);
            if (data) {
                location.reload();
            }
            else {
                alert('删除失败');
            }

        }
    });
}