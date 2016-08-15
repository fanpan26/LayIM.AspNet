
/*
    
*/
layui.define(['autohub'], function (exports) {
    var $ = layui.jquery;

    (function ($) {

        //ready  receiveMsg

        var call = {};
        var csHub = {
            on: function (events, callback) {
                if (typeof callback === 'function') {
                    call[events] ? call[events].push(callback) : call[events] = [callback];
                }
            },
            option: {
                serverUrl: ''//singalr服务器url
            },
            proxy: {
                proxyCS: null,//singalr客户端代理类
            },
            chatType: {
                one: 'friend',
                group: 'group'
            },
            messageType: {
                system: 1,
                custom: 2
            },
            //client
            client: {
                init: function () {
                    //客户端 receiveMessage 方法
                    _this.proxy.proxyCS.client.receiveMessage = function (result) {
                        //receiveMessage方法内部又调用了下面这个方法。。。
                        if (call.receiveMsg) {
                            for (var i = 0; i < call.receiveMsg.length; i++) {
                                call.receiveMsg[i](result);
                            }
                        }
                    };
                }
            },
            init: function (option) {
                $.extend(_this.option, option);
                console.log(_this.option);
                _this.server.init();//服务端代码初始化  
                _this.client.init();//客户端代码初始化

            },
            //server
            server: {
                //server初始化
                init: function () {
                    this.connect();
                    _this.proxy.proxyCS.client.clientOnConnectedCallBack = this.connectCallBack;
                },
                //连接服务器
                connect: function () {
                    $.connection.hub.url = _this.option.serverUrl;
                    _this.proxy.proxyCS = $.connection.layimHub;
                    console.log(_this.proxy.proxyCS.client);
                    $.connection.hub.start({ jsonp: true }).done(function () {
                        //连接服务器
                        //TODO处理聊天界面之前的逻辑
                        //console.log('自定义连接服务器成功方法:' + success);
                        if (call.ready) {
                            for (var i = 0; i < call.ready.length; i++) {
                                call.ready[i]({ connected: true });
                            }
                        }
                        console.log('连接服务器成功');
                    }).fail(function () {
                        //console.log('自定义连接服务器成功方法:' + success);
                        if (call.failed) {
                            for (var i = 0; i < call.failed.length; i++) {
                                call.failed[i]({ connected: false });
                            }
                        }
                    });
                },
                //单人聊天
                ctoc: function (sid, rid, t) {
                    //调用hub的clientToClient方法
                    if (true) {

                        //如果没有连接过，进行连接
                        console.log("用户 " + rid + "没有连接过...");
                        if (t == csHub.chatType.one) {
                            //一对一聊天连接服务器
                            _this.proxy.proxyCS.server.clientToClient(sid, rid);
                        } else {
                            //一对多（群组）聊天连接服务器
                            _this.proxy.proxyCS.server.clientToGroup(sid, rid);
                        }
                    } else {
                        console.log("用户 " + rid + "已经连接过了，不需要连接了...");
                    }
                },
                //发送，增加最后一个参数t， one ，group 群聊还是单体聊天
                send: function (msg) {

                    var t = msg.to.type;
                    switch (t) {
                        case csHub.chatType.one:
                            this.ctocsend(msg);
                            break;
                        case csHub.chatType.group:
                            this.ctogsend(msg);
                            break;
                        default:
                            alert('无效的消息类型');
                    }
                },
                //单独
                ctocsend: function (sendObj) {
                    _this.proxy.proxyCS.server.clientSendMsgToClient(sendObj);
                },
                //群组
                ctogsend: function (sendObj) {
                    _this.proxy.proxyCS.server.clientSendMsgToGroup(sendObj);
                },
                //连接成功之后回调
                connectCallBack: function (result) {
                    console.log(result);
                }
            }
        };
        var _this = csHub;
        //聊天信息处理
        var chat = {
            cache: {},
            cacheGroup: {},
            handleSystemMsg: function (result) {
                if (result.type == csHub.chatType.one) {
                    this.cache[result.data.rid] = "ok";//代表我已经和当前聊天人已经连接上了，下次点击没必要再次连接
                } else {
                    this.cacheGroup[result.data.rid] = "ok";
                }
                //然后在这里处理历史记录 2016-3-7
                if (result.data.history && result.data.history.length) {
                    $(result.data.history).each(function (i, item) {
                        //追加消息
                        chat.handleCustomMsg(item);//每一个item就是一条消息，这里格式是通用的，所以，直接调用 handleCustomMsg方法就可以了。
                    });
                }
                //处理好友逻辑信息 增加：2016-5-4
                var isfriend = result.data.isfriend;
                var sid = result.data.sid;
                var rid = result.data.rid;
                this.handleFriendTips(isfriend, sid, rid);
            },
            handleFriendTips: function (isfriend, sid, rid) {
                if (isfriend) {
                    var handle = isfriend.handle;//0 1 2 null 0好友申请 1 同意 2 拒绝 3 正常流程
                    var friend = isfriend.friend;
                    var logid = isfriend.logid;//申请主键
                    var txt = '';
                    if (handle == "0") {
                        txt = '对方申请加你为好友，备注：[' + (isfriend.reason || '无') + '] 是否同意?<input type="button" value="是" onclick="csClient.chat.agreeWithFriend(' + sid + ',' + rid + ',true,' + logid + ')"/>   <input type="button" value="否" onclick="csClient.chat.agreeWithFriend(' + sid + ',' + rid + ',false,' + logid + ')"/>';
                    } else if (handle == "1") { }
                    else if (handle == "2") { }
                    else {
                        if (friend == "0") {
                            txt = '您和TA还不是好友，是否添加对方为好友?<input type="button" value="是" onclick="csClient.chat.addFriend(' + sid + ',' + rid + ')"/>   <input type="button" value="否" onclick="csClient.chat.addFriend(0,0)"/>';
                        }
                    }
                    if (txt) {
                        var $ul = $('#layim_areaone' + rid);
                        var addHtml = '<li id="chat_li_isfriend" class="layim_chateme"><div style="text-align:center"></div><div id="chat_isfriend_text" style="text-align:center;margin-top:10px;">' + txt + '</div></li>';
                        $ul.append(addHtml);
                    }
                }
            },
            agreeWithFriend: function (sid, rid, agree, logid) {
                $.post('/executeapply', { userid: sid, applyid: rid, isagree: agree, logid: logid }, function (result) {
                    if (agree) {
                        $('#chat_isfriend_text').html('对方和您已经是好友啦，赶紧聊聊天吧');
                        $('#friend_apply_' + rid).html("已同意该好友申请");
                    } else {
                        $('#chat_isfriend_text').html('您已经拒绝了对方的好友请求');
                        $('#friend_apply_' + rid).html("已拒绝该好友申请");
                    }
                });
            },
            addFriend: function (sid, rid) {
                if (sid && rid) {
                    layer.prompt("", function (val) {
                        $.post('/apply', { userid: rid, applyid: sid, reason: val }, function (result) {
                            $('#chat_isfriend_text').html('您的好友申请已发送，请等待对方操作');
                        });
                    });
                } else {
                    $('#chat_li_isfriend').remove();
                }
            },
            handleCustomMsg: function (result) {
                var log = {};
                //接收人
                var keys = result.type + result.touser.id;
                //发送人
                var keys1 = result.type + result.fromuser.id;
                //这里一定要注意，这个keys是会变的，也就是说，如果只取一个的话，会造成 log.imarea[0]为undefined的情况，至于为什么会变，看看代码好好思考一下吧
                log.imarea = $('#layim_area' + keys);//layim_areaone0
                if (!log.imarea.length) {
                    log.imarea = $('#layim_area' + keys1);//layim_areaone0
                }
                if (!log.imarea.length) {
                    //这里只有在连接过一次之后，并且关闭消息框才会提示
                    if (result.touser.userid == currentUser.id) {
                        alert("您收到消息啦...");
                    }
                }
                //拼接html模板
                log.html = function (param, type) {
                    return '<li data-msgid="' + param.msgid + '" class="' + (type === 'me' ? 'layim_chateme' : '') + '">'
                        + '<div class="layim_chatuser">'
                            + function () {
                                if (type === 'me') {
                                    return '<span class="layim_chattime">' + param.time + '</span>'
                                           + '<span class="layim_chatname">' + param.name + '</span>'
                                           + '<img data-img="img_face_' + param.id + '" src="' + param.face + '" >';
                                } else {
                                    return '<img data-img="img_face_' + param.id + '" src="' + param.face + '" >'
                                           + '<span class="layim_chatname">' + param.name + '</span>'
                                           + '<span class="layim_chattime">' + param.time + '</span>';
                                }
                            }()
                        + '</div>'
                        + '<div class="layim_chatsay">' + param.content + '<em class="layim_zero"></em></div>'
                    + '</li>';
                };
                //images 为图片数组  files为附件数组  content为原消息
                log.handleMessage = function (content, images, files) {
                    //处理替换图片消息
                    if (images && images.length > 0) {
                        for (var i = 0; i < images.length; i++) {
                            content = content.replace(images[i].name, '<img src="' + images[i].url + '" width="200" height="200"/>');
                        }
                    }
                    //处理替换附件消息
                    if (files && files.length > 0) {
                        for (var i = 0; i < files.length; i++) {
                            var ext = files[i].name.split('.')[1];
                            ext = ext.substr(0, ext.length - 1);
                            var img = '<img src="/images/' + ext + '.png" width="30" height="30"/>';
                            content = content.replace(files[i].name, '<a href="' + files[i].url + '">' + img + '' + files[i].name + '</a>');
                        }
                    }
                    return content;
                }
                //上述代码还是layim里的代码，只不过拼接html的时候，参数采用signalR返回的参数
                var type = result.fromuser.id == currentUser.id ? "me" : "";//如果发送人的id==当前用户的id，那么这条消息类型为me
                //拼接html 直接调用layim里的代码
                log.imarea.append(log.html({
                    time: result.addtime,
                    id: result.fromuser.id,
                    name: result.fromuser.name,
                    face: result.fromuser.face,
                    msgid: result.msgid,
                    //content: replace_em(result.message)
                    content: log.handleMessage(result.message, result.images, result.files)
                }, type));
                //滚动条处理
                if (log.imarea.length) {
                    log.imarea.scrollTop(log.imarea[0].scrollHeight);
                }

                //清空输入框
                $('#layim_write').val('');
            },
            isConnected: function (rid, t) {
                ///单人聊天
                if (t == csHub.chatType.one) {
                    console.log(this.cache);
                    /*2016-3-7修改
                      增加聊天框的判断，如果聊天框被关闭了，需要重新连接，用来读取历史记录
                    */
                    var chatbox = $('#layim_areaone' + rid);
                    return this.cache[rid] === "ok" && chatbox.length > 0;
                }
                //群组聊天
                if (t == 'group') {
                    return this.cacheGroup[rid] === "ok";
                }
            }
        };
        _this.chat = chat;
        window.csClient = _this;
    })($);

    exports('hub', csClient);
});