layui.use(['jquery', 'layer'], function () {
    var currentUid = localStorage.getItem('layim_uid');
    var $ = layui.jquery;
    var layer = layui.layer;
    var active = {
        //同意
        agree: function (othis) {
            var li = othis.parents('li')
            , id = li.data('id')
            , from_group = li.data('fromGroup')
            , user = { username: li.data('name'), avatar: li.data('avatar'), sign: li.data('sign'),uid:li.data('uid') },
            type = li.data('type');

            if (type == 0) {
                //选择分组
                parent.layui.layim.setFriendGroup({
                    type: 'friend'
                  , username: user.username
                  , avatar: user.avatar
                  , group: parent.layui.layim.cache().friend //获取好友分组数据
                  , submit: function (group, index) {
                      handle(id, currentUid, 1, group, function (res) {
                          if (res.code == 0) {
                              //将好友追加到主面板
                              parent.layui.layim.addList({
                                  type: 'friend'
                                , avatar: user.avatar //好友头像
                                , username: user.username //好友昵称
                                , groupid: group //所在的分组id
                                , id: user.uid //好友ID
                                , sign: user.sign //好友签名
                              });
                              parent.layer.close(index);
                              othis.parent().html('已同意');
                          } else {
                              layer.msg(res.msg);
                          }
                      })
                  }
                });
            } else {
                handle(id, currentUid, 1, 0, function (res) {
                    if (res.code == 0) {
                        othis.parent().html('已同意');
                    } else {
                        layer.msg(res.msg);
                    }
                })
            }
        }

        //拒绝
      , refuse: function (othis) {
          var li = othis.parents('li')
          , id = li.data('id');
          layer.confirm('确定拒绝吗？', function (index) {
              handle(id, currentUid, 2, 0, function (res) {
                  if (res.code != 0) {
                      return layer.msg(res.msg);
                  }
                  layer.close(index);
                  othis.parent().html('<em>已拒绝</em>');
              });
          });
      }
    };

    var handle = function (id, uid, res, gid,callback) {
        $.post('/layim/apply/handle', { id: id, uid: uid, result: res, fgid: (gid || 0) }, function (result) {
            callback && callback(result);
        });
    }

    $('body').on('click', '.layui-btn', function () {
        var othis = $(this), type = othis.data('type');
        active[type] ? active[type].call(this, othis) : '';
    });
});