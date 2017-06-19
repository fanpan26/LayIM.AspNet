(function () {
    var _type, _id;
    var param = {
        init: function (type, id) {
            _type = type;
            _id = id;
            helper.loadMsg();
            $('#chatLogMore').on('click', function () {
                helper.loadMsg();
            });
        }
    };

    var record = true;
    var helper = {
        loadMsg: function () {
            if (!record) { return; }
            var uid = parent.layui.layim.cache().mine.id;
            var $li = $('#LAY_view').find('li').first();
            var t = 0;
            if ($li.length > 0) {
                t = $li.data('timestamp');
            }
            $('#chatLogMore').find('span').text('正在加载...');

            $.get('/layim/history', { uid: uid, type: _type, id: _id, stamp: t, page: 20 }, function (res) {
                if (res && res.trim() != '') {
                    if ($li.length) {
                        $(res).insertBefore($li);
                    } else {
                        $(res).insertAfter('#chatLogMore');
                    }
                    //转换 content
                    $('#LAY_view li[trans="0"]').each(function () {
                        var o = $(this).find('.layim-chat-text'), ot = o.text().trim(), n = parent.layui.layim.content(ot);

                        o.html(n);
                        $(this).attr('trans', "1");
                    });
                    $('#chatLogMore').find('span').text('查看更多记录');
                } else {
                    $('#chatLogMore').find('span').text('无更多记录了');
                    record = false;
                }
            });
        }
    }
    window.chatLogParam = param;
})();
