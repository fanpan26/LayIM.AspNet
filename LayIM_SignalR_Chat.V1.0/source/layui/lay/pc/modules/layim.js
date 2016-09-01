/**

 @Name：layim v2.0.83 商用版
 @Author：贤心
 @Site：http://layim.layui.com
 @License：LGPL
    
 */
 
layui.define(['jquery', 'layer', 'laytpl', 'upload'], function(exports){
  
    var v = '2.0.83';
    var $ = layui.jquery;
    var layer = layui.layer;
    var laytpl = layui.laytpl;
  
    var SHOW = 'layim-show', THIS = 'layim-this';
  
    //回调
    var call = {};
    var global = {
        othis:null
    };
    //对外API
    var LAYIM = function(){
        this.v = v;
        $('body').on('click', '*[layim-event]', function(e){
            var othis = $(this), methid = othis.attr('layim-event');
            events[methid] ? events[methid].call(this, othis, e) : '';
        });
    };
  
    //基础配置
    LAYIM.prototype.config = function(options){
        var skin = [
          layui.cache.dir+'css/pc/layim/skin/01.jpg'
          ,layui.cache.dir+'css/pc/layim/skin/02.jpg'
          ,layui.cache.dir+'css/pc/layim/skin/03.jpg'
          ,layui.cache.dir+'css/pc/layim/skin/04.jpg'
        ];
        options = options || {};
        options.skin = options.skin || [];
        layui.each(options.skin, function(index, item){
            skin.unshift(item);
        });
        options.skin = skin;
        options = $.extend({
            isfriend: true
          ,isgroup: true
        }, options);
        if(!window.JSON || !window.JSON.parse) return;
        init(options);
        return this;
    };
  
    //监听事件
    LAYIM.prototype.on = function(events, callback){
        if(typeof callback === 'function'){
            call[events] ? call[events].push(callback) : call[events] = [callback];
        }
        return this;
    };

    //获取所有缓存数据
    LAYIM.prototype.cache = function(){
        return cache;
    };
  
    //打开一个自定义的聊天界面
    LAYIM.prototype.chat = function(data){
        if(!window.JSON || !window.JSON.parse) return;
        return popchat(data), this;
    };
  
    //设置聊天界面最小化
    LAYIM.prototype.setChatMin = function(){
        return setChatMin(), this;
    };
  
    //接受消息
    LAYIM.prototype.getMessage = function(data){
        return getMessage(data), this;
    };
  
    //添加好友/群
    LAYIM.prototype.addList = function(data){
        return addList(data), this;
    };
  
    //删除好友/群
    LAYIM.prototype.removeList = function(data){
        return removeList(data), this;
    };
  
    //解析聊天内容
    LAYIM.prototype.content = function(content){
        return layui.laytpl.content(content);
    };
    //添加系统消息
    LAYIM.prototype.sysmsg = function (msg) {
        return addSystemMsg(msg), this;
    }

    //对外开放系统消息功能
    var addSystemMsg = function (msg) {
        var local = layui.data('layim')[cache.mine.id] || {};
        var thatChat = thisChat(), chatlog = local.chatlog || {};
        var ul = thatChat.elem.find('.layim-chat-main ul');

        ul.append('<div class="layim-chat-system"><span>' + msg + '</span></div>');
    }

    //主模板
    var listTpl = function(options){
        var nodata = {
            friend: "该分组下暂无好友"
          ,group: "暂无群组"
          ,history: "暂无历史会话"
        };

        options = options || {};
        options.item = options.item || ('d.' + options.type);

        return ['{{# var length = 0; layui.each('+ options.item +', function(i, data){ length++; }}'
          ,'<li layim-event="chat" data-type="'+ options.type +'" data-index="{{ '+ (options.index||'i') +' }}" id="layim-'+ options.type +'{{ data.id }}"><img data-status="{{ data.status||"" }}" src="{{ data.avatar }}"><span>{{ data.username||data.groupname||data.name||"佚名" }}</span><p>{{ data.remark||data.sign||"" }}</p></li>'
        ,'{{# }); if(length === 0){ }}'
          ,'<li class="layim-null">'+ (nodata[options.type] || "暂无数据") +'</li>'
        ,'{{# } }}'].join('');
    };

    var elemTpl = ['<div class="layui-layim-main">'
      ,'<div class="layui-layim-info">'
        ,'<div class="layui-layim-user">{{ d.mine.username }}</div>'
        ,'<div class="layui-layim-status">'
          ,'{{# if(d.mine.status === "online"){ }}'
          ,'<span class="layui-icon layim-status-online" layim-event="status" lay-type="show">&#xe617;</span>'
          ,'{{# } else if(d.mine.status === "hide") { }}'
          ,'<span class="layui-icon layim-status-hide" layim-event="status" lay-type="show">&#xe60f;</span>'
          ,'{{# } }}'
          ,'<ul class="layui-anim layim-menu-box">'
            ,'<li {{d.mine.status === "online" ? "class=layim-this" : ""}} layim-event="status" lay-type="online"><i class="layui-icon">&#xe618;</i><cite class="layui-icon layim-status-online">&#xe617;</cite>在线</li>'
            ,'<li {{d.mine.status === "hide" ? "class=layim-this" : ""}} layim-event="status" lay-type="hide"><i class="layui-icon">&#xe618;</i><cite class="layui-icon layim-status-hide">&#xe60f;</cite>隐身</li>'
          ,'</ul>'
        ,'</div>'
        , '<p class="layui-layim-remark" title="{{# if(d.mine.sign){ }}{{d.mine.sign}}{{# } }}"><input id="layim_user_sign_box" type="text" value="{{ d.mine.remark||d.mine.sign||"我的签名就是没有签名" }}" class="layim-sign-box layim-sign-hide"></p>'
      ,'</div>'
      ,'<ul class="layui-layim-tab{{# if(!d.base.isfriend || !d.base.isgroup){ }}'
        ,' layim-tab-two'
      ,'{{# } }}">'
        ,'<li class="layui-icon'
        ,'{{# if(!d.base.isfriend){ }}'
        ,' layim-hide'
        ,'{{# } else { }}'
        ,' layim-this'
        ,'{{# } }}'
        ,'" title="联系人" layim-event="tab" lay-type="friend">&#xe612;</li>'
        ,'<li class="layui-icon'
        ,'{{# if(!d.base.isgroup){ }}'
        ,' layim-hide'
        ,'{{# } else if(!d.base.isfriend) { }}'
        ,' layim-this'
        ,'{{# } }}'
        ,'" title="群组" layim-event="tab" lay-type="group">&#xe613;</li>'
        ,'<li class="layui-icon" title="历史会话" layim-event="tab" lay-type="history">&#xe611;</li>'
      ,'</ul>'
      ,'<ul class="layim-tab-content {{# if(d.base.isfriend){ }}layim-show{{# } }} layim-list-friend">'
      ,'{{# layui.each(d.friend, function(index, item){ var spread = d.local["spread"+index]; }}'
        ,'<li>'
          , '<h5 layim-event="spread" lay-type="{{ spread }}" id="{{ "group"+item.id }}"><i class="layui-icon">{{# if(spread === "true"){ }}&#xe61a;{{# } else {  }}&#xe602;{{# } }}</i><span>{{ item.groupname||"未命名分组"+index }}</span><em>(<cite class="layim-count"> {{ (item.list||[]).length }}</cite>)</em></h5>'
          ,'<ul class="layui-layim-list {{# if(spread === "true"){ }}'
          ,' layim-show'
          ,'{{# } }}">'
          ,listTpl({
              type: "friend"
            ,item: "item.list"
            ,index: "index"
          })
          ,'</ul>'
        ,'</li>'
      ,'{{# }); if(d.friend.length === 0){ }}'
        ,'<li><ul class="layui-layim-list layim-show"><li class="layim-null">暂无联系人</li></ul>'
      ,'{{# } }}'
      ,'</ul>'
      ,'<ul class="layim-tab-content {{# if(!d.base.isfriend && d.base.isgroup){ }}layim-show{{# } }}">'
        ,'<li>'
          ,'<ul class="layui-layim-list layim-show layim-list-group">'
          ,listTpl({
              type: 'group'
          })
          ,'</ul>'
        ,'</li>'
      ,'</ul>'
      ,'<ul class="layim-tab-content  {{# if(!d.base.isfriend && !d.base.isgroup){ }}layim-show{{# } }}">'
        ,'<li>'
          ,'<ul class="layui-layim-list layim-show layim-list-history">'
          ,listTpl({
              type: 'history'
          })
          ,'</ul>'
        ,'</li>'
      ,'</ul>'
      ,'<ul class="layim-tab-content">'
        ,'<li>'
          ,'<ul class="layui-layim-list layim-show" id="layui-layim-search"></ul>'
        ,'</li>'
      ,'</ul>'
      ,'<ul class="layui-layim-tool">'
        ,'<li class="layui-icon layim-tool-search" layim-event="search" title="搜索">&#xe615;</li>'
        ,'<li class="layui-icon layim-tool-skin" layim-event="skin" title="换肤">&#xe61b;</li>'
        ,'{{# if(d.base.find){ }}'
        ,'<li class="layui-icon layim-tool-find" layim-event="find" title="查找">&#xe61f;</li>'
        , '{{# } }}'
         , '{{# if(d.base.apply){ }}'
        , '<li class="layui-icon layim-tool-apply" layim-event="apply" title="消息">&#xe606;</li>'
        , '{{# } }}'
        ,'{{# if(!d.base.copyright){ }}'
        ,'<li class="layui-icon layim-tool-about" layim-event="about" title="关于">&#xe60b;</li>'
        ,'{{# } }}'
      ,'</ul>'
      ,'<div class="layui-layim-search"><input><label class="layui-icon" layim-event="closeSearch">&#x1007;</label></div>'
    ,'</div>'].join('');
  
    //换肤模版
    var elemSkinTpl = ['<ul class="layui-layim-skin">'
    ,'{{# layui.each(d.skin, function(index, item){ }}'
      ,'<li><img layim-event="setSkin" src="{{ item }}"></li>'
    ,'{{# }); }}'
    ,'<li layim-event="setSkin"><cite>默认</cite></li>'
    ,'</ul>'].join('');
  
    //聊天主模板
    var elemChatTpl = ['<div class="layim-chat layim-chat-{{d.data.type}}">'
      ,'<div class="layim-chat-title">'
        ,'<a class="layim-chat-other">'
          ,'<img src="{{ d.data.avatar }}"><span layim-event="{{ d.data.type==="group" ? \"groupMembers\" : \"\" }}">{{ d.data.name||"佚名" }} {{# if(d.data.type==="group"){ }} <em class="layim-chat-members"></em><i class="layui-icon">&#xe61a;</i> {{# } }}</span>'
        ,'</a>'
      ,'</div>'
      ,'<div class="layim-chat-main">'
        ,'<ul></ul>'
      ,'</div>'
      ,'<div class="layim-chat-footer">'
        ,'<div class="layim-chat-tool" data-json="{{encodeURIComponent(JSON.stringify(d.data))}}">'
          ,'<span class="layui-icon layim-tool-face" title="选择表情" layim-event="face">&#xe60c;</span>'
          , '<span class="layui-icon layim-tool-image layim-pz-images" title="上传图片" layim-event="image"><i>&#xe60d;</i><img src="/content/loading.gif" width="20" height="20" style="display:none;"/><input type="file" name="file"></span>'
          ,'{{# if(d.base && d.base.uploadFile){ }}'
          , '<span class="layui-icon layim-tool-image layim-pz-file" title="发送文件" layim-event="image" data-type="file"><i>&#xe61d;</i><img src="/content/loading.gif" width="20" height="20" style="display:none;"/><input type="file" name="file"></span>'
           ,'{{# }; }}'
          ,'{{# if(d.base && d.base.chatLog){ }}'
          ,'<span class="layim-tool-log" layim-event="chatLog"><i class="layui-icon">&#xe60e;</i>聊天记录</span>'
          ,'{{# }; }}'
        ,'</div>'
        ,'<div class="layim-chat-textarea"><textarea></textarea></div>'
        ,'<div class="layim-chat-bottom">'
          ,'<div class="layim-chat-send">'
            ,'{{# if(!d.base.brief){ }}'
            ,'<span class="layim-send-close" layim-event="closeThisChat">关闭</span>'
            ,'{{# } }}'
            ,'<span class="layim-send-btn" layim-event="send">发送</span>'
            ,'<span class="layim-send-set" layim-event="setSend" lay-type="show"><em class="layui-edge"></em></span>'
            ,'<ul class="layui-anim layim-menu-box">'
              ,'<li {{d.local.sendHotKey !== "Ctrl+Enter" ? "class=layim-this" : ""}} layim-event="setSend" lay-type="Enter"><i class="layui-icon">&#xe618;</i>按Enter键发送消息</li>'
              ,'<li {{d.local.sendHotKey === "Ctrl+Enter" ? "class=layim-this" : ""}} layim-event="setSend"  lay-type="Ctrl+Enter"><i class="layui-icon">&#xe618;</i>按Ctrl+Enter键发送消息</li>'
            ,'</ul>'
          ,'</div>'
        ,'</div>'
      ,'</div>'
    ,'</div>'].join('');
  
    //转换时间 时间格式
    layui.laytpl.date = function(timestamp){
        return new Date(timestamp||new Date()).toLocaleString()
        .replace(/\//g, "-").replace(/(\s)\D*(\d)/g, "$1$2");
    };
  
    //转换内容
    layui.laytpl.content = function(content){
        //支持的html标签
        var html = function(end){
            return new RegExp('\\n*\\['+ (end||'') +'(pre|div|p|table|thead|th|tbody|tr|td|ul|li|ol|li|dl|dt|dd|h2|h3|h4|h5)([\\s\\S]*?)\\]\\n*', 'g');
        };
        content = (content||'').replace(/&(?!#?[a-zA-Z0-9]+;)/g, '&amp;')
        .replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/'/g, '&#39;').replace(/"/g, '&quot;') //XSS
        .replace(/@(\S+)(\s+?|$)/g, '@<a href="javascript:;">$1</a>$2') //转义@
        .replace(/\s{2}/g, '&nbsp') //转义空格
        .replace(/img\[([^\s]+?)\]/g, function(img){  //转义图片
            return '<img class="layui-layim-photos" src="' + img.replace(/(^img\[)|(\]$)/g, '') + '">';
        })
        .replace(/file\([\s\S]+?\)\[[\s\S]*?\]/g, function(str){ //转义文件
            var href = (str.match(/file\(([\s\S]+?)\)\[/)||[])[1];
            var text = (str.match(/\)\[([\s\S]*?)\]/)||[])[1];
            if(!href) return str;
            return '<a class="layui-layim-file" href="'+ href +'" target="_blank"><i class="layui-icon">&#xe61e;</i><cite>'+ (text||href) +'</cite></a>';
        })
        .replace(/face\[([^\s\[\]]+?)\]/g, function(face){  //转义表情
            var alt = face.replace(/^face/g, '');
            return '<img alt="'+ alt +'" title="'+ alt +'" src="' + faces[alt] + '">';
        }).replace(/a\([\s\S]+?\)\[[\s\S]*?\]/g, function(str){ //转义链接
            var href = (str.match(/a\(([\s\S]+?)\)\[/)||[])[1];
            var text = (str.match(/\)\[([\s\S]*?)\]/)||[])[1];
            if(!href) return str;
            return '<a href="'+ href +'" target="_blank">'+ (text||href) +'</a>';
        }).replace(html(), '\<$1 $2\>').replace(html('/'), '\</$1\>') //转移代码
        .replace(/\n/g, '<br>') //转义换行 
        return content;
    };
  
    var elemChatMain = [
       '{{# if(d.system){ }}'
       , '<div class="layim-chat-system"><span>{{ d.content }}</span></div>'
        , '{{# } else { }}'
      , '<li {{ d.mine ? "class=layim-chat-mine" : "" }}>'
     , '<div class="layim-chat-user"><img src="{{ d.avatar }}"><cite>'
     , '{{# if(d.mine){ }}'
       , '<i>{{ layui.laytpl.date(d.timestamp) }}</i>{{ d.username||"佚名" }}'
      , '{{# } else { }}'
       , '{{ d.username||"佚名" }}<i>{{ layui.laytpl.date(d.timestamp) }}</i>'
      , '{{# } }}'
       , '</cite></div>'
     , '<div class="layim-chat-text">{{ layui.laytpl.content(d.content||"&nbsp") }}</div>'
   , '</li>', '{{# } }}'].join('');
  
    var elemChatList = '<li class="layim-chatlist-{{ d.data.type }}{{ d.data.id }} layim-this" layim-event="tabChat"><img src="{{ d.data.avatar }}"><span>{{ d.data.name||"佚名" }}</span>{{# if(!d.base.brief){ }}<i class="layui-icon" layim-event="closeChat">&#x1007;</i>{{# } }}</li>';
  
    //Ajax
    var post = function(options, callback, tips){
        options = options || {};
        return $.ajax({
            url: options.url
          ,type: options.type || 'get'
          ,data: options.data
          ,dataType: options.dataType || 'json'
          ,cache: false
          ,success: function(res){
              res.code == 0
                ? callback && callback(res.data || {})
              : res.msg == 'unauthorized' ? (function () { location.href = '/home/login?msg=' + res.msg; })() : layer.msg(res.msg || ((tips || 'Error') + ': LAYIM_NOT_GET_DATA'), {
                  time: 5000
              });
          },error: function(err, msg){
              window.console && console.log && console.error('LAYIM_DATE_ERROR：' + msg);
          }
        });
    };
  
    //请求基础信息
    var cache = {message: {}, chat: []}, init = function(options){
        var mine = options.mine || {}, local = layui.data('layim')[mine.id] || {}, obj = {
            base: options
          ,local: local
          ,mine: mine
          ,history: local.history || {}
        };
        cache = $.extend(cache, obj);
        if(options.brief){
            return layui.each(call.ready, function(index, item){
                item && item(obj);
            });
        };
        post(options.init, function(data){
            var mine = options.mine || data.mine || {};
            var local = layui.data('layim')[mine.id] || {}, obj = {
                base: options //基础配置信息
              ,local: local //本地数据
              ,mine:  mine //我的用户信息
              ,friend: data.friend || [] //联系人信息
              ,group: data.group || [] //群组信息
              ,history: local.history || {} //历史会话信息
            };
            cache = $.extend(cache, obj);
            popim(laytpl(elemTpl).render(obj));
            if(local.close){
                popmin();
            }
            layui.each(call.ready, function(index, item){
                item && item(obj);
            });
        }, 'INIT');
    };
  
    //显示主面板
    var layimMain, popim = function(content){
        return layer.open({
            type: 1
          ,area: ['260px', '520px']
          ,skin: 'layui-box layui-layim'
          ,title: '&#8203;'
          ,offset: 'rb'
          ,id: 'layui-layim'
          ,shade: false
          ,moveType: 1
          ,shift: 2
          ,content: content
          ,success: function(layero){
              var local = layui.data('layim')[cache.mine.id] || {}, skin = local.skin;
        
              layimMain = layero;
              layimMain.css({
                  'background-image': skin ? 'url('+ skin +')' : 'none'
              });
              if(cache.base.right){
                  layero.css('margin-left', '-' + cache.base.right);
              }
              if(layimClose){
                  layer.close(layimClose.attr('times'));
              }

              //按最新会话重新排列
              var arr = [], historyElem = layero.find('.layim-list-history');
              historyElem.find('li').each(function(){
                  arr.push($(this).prop('outerHTML'))
              });
              if(arr.length > 0){
                  arr.reverse();
                  historyElem.html(arr.join(''));
              }
        
              banRightMenu();
          }
          ,cancel: function(index){
              popmin();
              var local = layui.data('layim')[cache.mine.id] || {};
              local.close = true;
              layui.data('layim', {
                  key: cache.mine.id
                ,value: local
              });
              return false;
          }
        });
    };
  
    //屏蔽主面板右键菜单
    var banRightMenu = function(){
        layimMain.on('contextmenu', function(event){
            event.cancelBubble = true 
            event.returnValue = false;
            return false; 
        });
    
        var hide = function(){
            layer.closeAll('tips');
        };
        var resetposition = function (obj, t, l) {
            if (obj.length && t && l) {
                var top = obj.css('top').toLowerCase().replace('px', '');
                var left = obj.css('left').toLowerCase().replace('px', '');
                top = parseInt(top) + t;
                left = parseInt(left) + l;
                obj.css({ 'left': left + 'px', 'top': top + 'px' });
            }
        }
        var space_icon = '&nbsp;&nbsp;';
        var space_no_icon = '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;';
        var showtips = function (html,othis,id, t, l) {
            t = t || 0;
            l = l || 0;
            layer.tips(html, othis, {
                tips: 1
             , time: 0
             , shift: 5
             , fix: true
             , skin: 'layui-box layui-layim-contextmenu'
             , success: function (layero) {
                 var stopmp = function (e) { stope(e); };
                 layero.off('mousedown', stopmp).on('mousedown', stopmp);
                 var layerobj = $('#contextmenu_' + id).parents('.layui-layim-contextmenu');

                 resetposition(layerobj, t, l);
             }
            });
            $(document).off('mousedown', hide).on('mousedown', hide);
            $(window).off('resize', hide).on('resize', hide);
        }
        //自定义历史会话右键菜单
        layimMain.find('.layim-list-history').on('contextmenu', 'li', function(e){
            var othis = $(this);
            var html = '<ul data-id="'+ othis[0].id +'" data-index="'+ othis.data('index') +'"><li layim-event="menuHistory" data-type="one">移除该会话</li><li layim-event="menuHistory" data-type="all">清空全部会话列表</li></ul>';
            if(othis.hasClass('layim-null')) return;
      
            layer.tips(html, this, {
                tips: 1
              ,time: 0
              ,shift: 5
              ,fix: true
              ,skin: 'layui-box layui-layim-contextmenu'
              ,success: function(layero){
                  var stopmp = function(e){ stope(e); };
                  layero.off('mousedown', stopmp).on('mousedown', stopmp);
              }
            });
            $(document).off('mousedown', hide).on('mousedown', hide);
            $(window).off('resize', hide).on('resize', hide);
      
        });
        //好友右键菜单
        layimMain.find('.layim-list-friend').on('contextmenu', '.layui-layim-list li', function (e) {
            var othis = $(this);
            global.othis = othis;
            var id = othis[0].id;
            var html = '<ul id="contextmenu_' + othis[0].id + '" data-id="' + othis[0].id + '" data-index="' + othis.data('index') + '">';
            html += '<li layim-event="menu_chat"><i class="layui-icon" >&#xe611;</i>' + space_icon + '发送即时消息</li>';
            html += '<li layim-event="menu_profile"><i class="layui-icon">&#xe60a;</i>' + space_icon + '查看资料</li>';
            html += '<li layim-event="menu_history"><i class="layui-icon" >&#xe60e;</i>' + space_icon + '消息记录</li>';
            html += '<li layim-event="menu_nomsg">' + space_no_icon + '屏蔽消息</li>';
            html += '<li layim-event="menu_delete">' + space_no_icon + '删除好友</li>';
            html += '<li layim-event="menu_moveto">' + space_no_icon + '移动至</li></ul>';
            if (othis.hasClass('layim-null')) return;
            //注意一下这个方法
            showtips(html, this, id, 160, 25);
       
        });
        //好友分组右键菜单
        layimMain.find('.layim-list-friend').on('contextmenu', 'h5', function (e) {
            var othis = $(this);
            var id = othis[0].id;
       
            var html = '<ul id="contextmenu_' + othis[0].id + '" data-id="' + othis[0].id + '" data-index="' + othis.data('index') + '">';
            html += '<li layim-event="menu_refresh"><i class="layui-icon" >&#x1003;</i>' + space_icon + '刷新好友列表</li>';
            html += '<li layim-event="menu_online"><i class="layui-icon">&#xe610;</i>' + space_icon + '显示在线好友</li>';
            html += '<li layim-event="menu_newgroup">' + space_no_icon + '新建分组</li>';
            html += '<li layim-event="menu_rename">' + space_no_icon + '重命名</li>';
            html += '<li layim-event="menu_delgroup">' + space_no_icon + '删除分组</li></ul>';
            if (othis.hasClass('layim-null')) return;
            showtips(html,this,id,118, 25);
        });
        //群组右键菜单
        layimMain.find('.layim-list-group').on('contextmenu', 'li', function (e) {
            
            var top = e.screenY;
            var moveT = 230;
            if (top >= 760) {
                moveT = moveT - (top - 760);
            }
            var othis = $(this);
            var id = othis[0].id;

            var html = '<ul id="contextmenu_' + othis[0].id + '" data-id="' + othis[0].id + '" data-index="' + othis.data('index') + '">';
            html += '<li><i class="layui-icon">&#xe611;</i>' + space_icon + '发送群消息</li>';
            html += '<li><i class="layui-icon">&#xe60a;</i>' + space_icon + '查看群资料</li>';
            html += '<li><i class="layui-icon">&#xe60e;</i>' + space_icon + '消息记录</li>';
            html += '<li>' + space_no_icon + '修改群图标</li>';
            html += '<li>' + space_no_icon + '群消息设置</li>';
            html += '<li>' + space_no_icon + '修改群备注</li></ul>';
            html += '<li>' + space_no_icon + '转让该群</li></ul>';
            html += '<li>' + space_no_icon + '退出该群</li></ul>';
            html += '<li>' + space_no_icon + '解散该群</li></ul>';
            if (othis.hasClass('layim-null')) return;
            showtips(html, this, id, moveT, 25);
        });
    }
  
    //主面板最小化状态
    var layimClose, popmin = function(content){
        if(layimClose){
            layer.close(layimClose.attr('times'));
        }
        if(layimMain){
            layimMain.hide();
        }
        cache.mine = cache.mine || {};
        return layer.open({
            type: 1
          ,title: false
          ,id: 'layui-layim-close'
          ,skin: 'layui-box layui-layim-min layui-layim-close'
          ,shade: false
          ,closeBtn: false
          ,shift: 2
          ,offset: 'rb'
          ,content: '<img src="'+ (cache.mine.avatar||(layui.cache.dir+'css/pc/layim/skin/logo.jpg')) +'"><span>'+ (content||cache.base.title||'我的LayIM') +'</span>'
          ,success: function(layero, index){
              layimClose = layero;
              if(cache.base.right){
                  layero.css('margin-left', '-' + cache.base.right);
              }
              layero.on('click', function(){
                  layer.close(index);
                  layimMain.show();
                  var local = layui.data('layim')[cache.mine.id] || {};
                  delete local.close;
                  layui.data('layim', {
                      key: cache.mine.id
                    ,value: local
                  });
              });
          }
        });
    };
  
    //显示聊天面板
    var layimChat, layimMin, chatIndex, To = {}, popchat = function(data){
        data = data || {};
    
        var chat = $('#layui-layim-chat'), render = {
            data: data
          ,base: cache.base
          ,local: cache.local
        };

        if(!data.id){
            return layer.msg('非法用户');
        }

        if(chat[0]){
            var list = layimChat.find('.layim-chat-list');
            var listThat = list.find('.layim-chatlist-'+ data.type + data.id);
      
            //如果是最小化，则还原窗口
            if(layimChat.css('display') === 'none'){
                layimChat.show();
            }
      
            if(layimMin){
                layer.close(layimMin.attr('times'));
            }
      
            //如果出现多个聊天面板
            if(list.find('li').length === 1 && !listThat[0]){
                layimChat.css('width', '800px');
                list.css('display', 'inline-block');
            }
      
            //打开的是非当前聊天面板，则新增面板
            if(!listThat[0]){
                list.append(laytpl(elemChatList).render(render));
                chat.append(laytpl(elemChatTpl).render(render));
            }
      
      
            changeChat(list.find('.layim-chatlist-'+ data.type + data.id));
            if(!listThat[0]){
                viewChatlog();
            }
            setHistory(data);
            hotkeySend();
            return chatIndex;
        }
    
        var index = chatIndex = layer.open({
            type: 1
          ,area: ['600px', '520px']
          ,skin: 'layui-box layui-layim-chat'
          ,id: 'layui-layim-chat'
          ,title: '&#8203;'
          ,shade: false
          ,moveType: 1
          ,maxmin: true
          ,closeBtn: cache.base.brief ? false : 1
          ,content: laytpl('<ul class="layim-chat-list">'+ elemChatList +'</ul>' + elemChatTpl).render(render)
          ,success: function(layero){
              var local = layui.data('layim')[cache.mine.id] || {};
              var skin = local.skin;
              layimChat = layero;
              layimChat.css({
                  'background-image': skin ? 'url('+ skin +')' : 'none'
              });
              hotkeySend();
              setHistory(data);
        
              //聊天窗口的切换监听
              layui.each(call.chatChange, function(index, item){
                  item && item(thisChat());
              });
        
              viewChatlog();
              showOffMessage();
        
              //查看大图
              layero.on('click', '.layui-layim-photos', function(){
                  var src = this.src;
                  layer.close(popchat.photosIndex);
                  layer.photos({
                      photos: {
                          data: [{
                              "alt": "大图模式",
                              "src": src
                          }]
                      }
                    ,shade: 0.01
                    ,closeBtn: 2
                    ,shift: 0
                    ,success: function(layero, index){
                        popchat.photosIndex = index;
                    }
                  });
              });
          }
          ,min: function(){
              setChatMin();
              return false;
          }
          ,end: function(){
              layer.closeAll('tips');
          }
        });
        return index;
    };

    //设置聊天窗口最小化 & 新消息提醒
    var setChatMin = function(newMsg){
        var thatChat = newMsg || thisChat().data;
        if(layimChat && !newMsg){
            layimChat.hide();
        }
        if(layimMin){
            layer.close(layimMin.attr('times'));
        }
        layer.open({
            type: 1
          ,title: false
          ,id: 'layui-layim-min'
          ,skin: 'layui-box layui-layim-min'
          ,shade: false
          ,closeBtn: false
          ,shift: thatChat.shift || 2
          ,offset: $(window).height() - 50
          ,content: '<img src="'+ thatChat.avatar +'"><span>'+ thatChat.name +'</span>'
          ,success: function(layero, index){
              if(!newMsg){
                  layimMin = layero;
              }
              layero.on('click', function(){
                  layer.close(index);
                  newMsg ? layui.each(cache.chat, function(i, item){
                      popchat(item);
                  }) : layimChat.show();
                  if(newMsg){
                      cache.chat = [];
                      chatListMore();
                  }
              });
          }
        });
    };
  
    //切换聊天
    var changeChat = function(elem, del){
        elem = elem || $('.layim-chat-list .' + THIS);
        var index = elem.index() === -1 ? 0 : elem.index();
        var str = '.layim-chat', cont = layimChat.find(str).eq(index);
    
        if(del){
      
            //如果关闭的是当前聊天，则切换聊天焦点
            if(elem.hasClass(THIS)){
                changeChat(index === 0 ? elem.next() : elem.prev());
            }
      
            elem.remove();
            cont.remove();
      
            var length = layimChat.find(str).length;
      
            //只剩下1个列表，隐藏左侧区块
            if(length === 1){
                layimChat.find('.layim-chat-list').hide();
                layimChat.css('width', '600px');
            }
      
            //聊天列表不存在，则关闭聊天界面
            if(length === 0){      
                layer.close(chatIndex);
            }
      
            return false;
        }
    
        elem.addClass(THIS).siblings().removeClass(THIS);
        cont.css('display', 'inline-block').siblings(str).hide();
        cont.find('textarea').focus();
    
        //聊天窗口的切换监听
        layui.each(call.chatChange, function(index, item){
            item && item(thisChat());
        });
        showOffMessage();
    };
  
    //展示存在队列中的消息
    var showOffMessage = function(){
        var thatChat = thisChat();
        var message = cache.message[thatChat.data.type + thatChat.data.id];
        if(message){
            //展现后，删除队列中消息
            delete cache.message[thatChat.data.type + thatChat.data.id];
        }
    };
  
    //获取当前聊天面板
    var thisChat = function(){
        var index = $('.layim-chat-list .' + THIS).index();
        var cont = layimChat.find('.layim-chat').eq(index);
        var to = JSON.parse(decodeURIComponent(cont.find('.layim-chat-tool').data('json')));
        return {
            elem: cont
          ,data: to
          ,textarea: cont.find('textarea')
        };
    }

  //记录历史会话
  var setHistory = function(data){
    var local = layui.data('layim')[cache.mine.id] || {};
    var obj = {}, history = local.history || {};
    var is = history[data.type + data.id];
    
    if(!layimMain) return;
    
    var historyElem = layimMain.find('.layim-list-history');

    data.historyTime = new Date().getTime();
    history[data.type + data.id] = data;
  
    local.history = history;
    
    layui.data('layim', {
      key: cache.mine.id
      ,value: local
    });

    if(is) return;

    obj[data.type + data.id] = data;
    var historyList = laytpl(listTpl({
      type: 'history'
      ,item: 'd.data'
    })).render({data: obj});
    historyElem.prepend(historyList);
    historyElem.find('.layim-null').remove();
  };
  
  //发送消息
  var sendMessage = function(){
    var data = {
      username: cache.mine ? cache.mine.username : '访客'
      ,avatar: cache.mine ? cache.mine.avatar : (layui.cache.dir+'css/pc/layim/skin/logo.jpg')
      ,id: cache.mine ? cache.mine.id : null
      ,mine: true
    };
    var thatChat = thisChat(), ul = thatChat.elem.find('.layim-chat-main ul');
    var maxLength = cache.base.maxLength || 3000;
    data.content = thatChat.textarea.val();
    if(data.content.replace(/\s/g, '') !== ''){
      
      if(data.content.length > maxLength){
        return layer.msg('内容最长不能超过'+ maxLength +'个字符')
      }
      
      ul.append(laytpl(elemChatMain).render(data));
      
      var param = {
        mine: data
        ,to: thatChat.data
      }, message = {
        username: param.mine.username
        ,avatar: param.mine.avatar
        ,id: param.to.id
        ,type: param.to.type
        ,content: param.mine.content
        ,timestamp: new Date().getTime()
        ,mine: true
      };
      pushChatlog(message);
      
      layui.each(call.sendMessage, function(index, item){
        item && item(param);
      });
    }
    chatListMore();
    thatChat.textarea.val('').focus();
  };
  
  //接受消息
  var getMessage = function(data){
    data = data || {};
    var elem = $('.layim-chatlist-'+ data.type + data.id);
    var group = {}, index = elem.index();
    
    data.timestamp = data.timestamp || new Date().getTime();
    pushChatlog(data);
    
    if((!layimChat && data.content) || index === -1){
      if(cache.message[data.type + data.id]){
        cache.message[data.type + data.id].push(data)
      } else {
        cache.message[data.type + data.id] = [data];
        
        //记录聊天面板队列
        if(data.type === 'friend'){
          var friend;
          layui.each(cache.friend, function(index1, item1){
            layui.each(item1.list, function(index, item){
              if(item.id == data.id){
                item.type = 'friend';
                item.name = item.username;
                cache.chat.push(item);
                return friend = true;
              }
            });
            if(friend) return true;
          });
          if(!friend){
            data.name = data.username + '<cite>临时会话<cite>';
            cache.chat.push(data);
          }
        } else if(data.type === 'group'){
          var isgroup;
          layui.each(cache.group, function(index, item){
            if(item.id == data.id){
              item.type = 'group';
              item.name = item.groupname;
              cache.chat.push(item);
              return isgroup = true;
            }
          });
          if(!isgroup){
            data.name = data.groupname;
            cache.chat.push(data);
          }
        } else {
          data.name = data.name || data.username || data.groupname;
          cache.chat.push(data);
        }
      }
      if(data.type === 'group'){
        layui.each(cache.group, function(index, item){
          if(item.id == data.id){
            group.avatar = item.avatar;
            return true;
          }
        });
      }
      return setChatMin({
        name: '收到新消息'
        ,avatar: group.avatar || data.avatar
        ,shift: 6
      });
    }
    
    //接受到的消息不在当前Tab
    var thatChat = thisChat();
    if(thatChat.data.type + thatChat.data.id !== data.type + data.id){
      elem.addClass('layui-anim layer-anim-06');
      setTimeout(function(){
        elem.removeClass('layui-anim layer-anim-06')
      }, 300);
    }
    
    var cont = layimChat.find('.layim-chat').eq(index);
    var ul = cont.find('.layim-chat-main ul'); 
    if(data.content.replace(/\s/g, '') !== ''){
      ul.append(laytpl(elemChatMain).render(data));
    }
    chatListMore();
  };
  
  //存储最近50条聊天记录到本地
  var pushChatlog = function(message){
    var local = layui.data('layim')[cache.mine.id] || {};
    var chatlog = local.chatlog || {};
    if(chatlog[message.type + message.id]){
      chatlog[message.type + message.id].push(message);
      if(chatlog[message.type + message.id].length > 50){
        chatlog[message.type + message.id].shift();
      }
    } else {
      chatlog[message.type + message.id] = [message];
    }
    local.chatlog = chatlog;
    layui.data('layim', {
      key: cache.mine.id
      ,value: local
    });
  };
  
  //渲染本地最新聊天记录到相应面板
  var viewChatlog = function(){
    var local = layui.data('layim')[cache.mine.id] || {};
    var thatChat = thisChat(), chatlog = local.chatlog || {};
    var ul = thatChat.elem.find('.layim-chat-main ul');
    layui.each(chatlog[thatChat.data.type + thatChat.data.id], function(index, item){
        ul.append(laytpl(elemChatMain).render(item));
    });
  
    var c = chatlog[thatChat.data.type + thatChat.data.id];
    if (c&&c.length) {
        var lasttime = c[c.length - 1].timestamp;
        var historyTip = getDateDiff(lasttime);
        //时间超过一个小时才出现历史记录提示
        if (historyTip) {
            ul.append('<div class="layim-chat-system"><span>以上是' + historyTip + '历史消息</span></div>');
        }
    }
    chatListMore();
    scrollLoad();
  };
    //滚动条加载
  var scrollLoad = function () {
      var thatChat = thisChat(), chatMain = thatChat.elem.find('.layim-chat-main');
      var ul = chatMain.find('ul');
      chatMain.scroll(function () {
          //var lastid = one.find('li').first().data('msgid') || '';
          var nScrollTop = $(this)[0].scrollTop;
          if (nScrollTop == 0 && chatMain.data('load') != 1) {
              console.log('需要加载历史记录哦');
             
              if (!ul.prev().hasClass('layim-chat-system')) {
                  //点击加载更多，滚动条加载，没有数据了。。
                  // ul.before('<div class="layim-chat-system"><span layim-event=""><img style="margin-top:15px;" src="/Content/loading.gif" />&nbsp;正在加载...</span></div>');
                  ul.before('<div class="layim-chat-system"><img style="margin-top:15px;" src="/Content/loading.gif" /</div>');
              }
              //one.data('load', 1);//加载完毕
          }
      });
  }
  var minute = 1000 * 60;
  var hour = minute * 60;
  var day = hour * 24;
  var halfamonth = day * 15;
  var month = day * 30;
 
  function getDateDiff(dateTimeStamp) {
      var now = new Date().getTime();
      var diffValue = now - dateTimeStamp;

      if (diffValue < 0) {
          //非法操作
          //alert("结束日期不能小于开始日期！");
      }

      var monthC = diffValue / month;
      var weekC = diffValue / (7 * day);
      var dayC = diffValue / day;
      var hourC = diffValue / hour;
      var minC = diffValue / minute;

      if (monthC >= 1) {
          result = parseInt(monthC) + "个月前";
      }
      else if (weekC >= 1) {
          result = parseInt(weekC) + "个星期前";
      }
      else if (dayC >= 1) {
          result = parseInt(dayC) + "天前";
      }
      else if (hourC >= 1) {
          result = parseInt(hourC) + "个小时前";
      }
      else if (minC >= 10) {//至少十分钟提示
          result = parseInt(minC) + "分钟前";
      } else
          result = "";
      return result;
  }

  //添加好友或群
  var addList = function(data){
    var listElem = layimMain.find('.layim-list-'+ data.type);
    var obj = {}, has;
    if(cache[data.type]){
      if(data.type === 'friend'){
        layui.each(cache.friend, function(index, item){
          if(data.groupid == item.id){
            //检查好友是否已经在列表中
            layui.each(cache.friend[index].list, function(idx, itm){
              if(itm.id == data.id){
                return has = true
              }
            });
            if(has) return layer.msg('好友 ['+ (data.username||'') +'] 已经存在列表中',{shift: 6});
            cache.friend[index].list = cache.friend[index].list || [];
            obj[cache.friend[index].list.length] = data;
            data.groupIndex = index;
            cache.friend[index].list.push(data); //在cache的friend里面也增加好友
            return true;
          }
        });
      } else if(data.type === 'group'){
        //检查群组是否已经在列表中
        layui.each(cache.group, function(idx, itm){
          if(itm.id == data.id){
            return has = true
          }
        });
        if(has) return layer.msg('您已是 ['+ (data.groupname||'') +'] 的群成员',{shift: 6});
        obj[cache.group.length] = data;
        cache.group.push(data);
      }
    }
    
    if(has) return;

    var list = laytpl(listTpl({
      type: data.type
      ,item: 'd.data'
      ,index: data.type === 'friend' ? 'data.groupIndex' : null
    })).render({data: obj});

    if(data.type === 'friend'){
      var li = listElem.find('>li').eq(data.groupIndex);
      li.find('.layui-layim-list').append(list);
      li.find('.layim-count').html(cache.friend[data.groupIndex].list.length); //刷新好友数量
      //如果初始没有好友
      if(li.find('.layim-null')[0]){
        li.find('.layim-null').remove();
      }
    } else if(data.type === 'group'){
      listElem.append(list);
      //如果初始没有群组
      if(listElem.find('.layim-null')[0]){
        listElem.find('.layim-null').remove();
      }
    }
  };
  
  //移出好友或群
  var removeList = function(data){
    var listElem = layimMain.find('.layim-list-'+ data.type);
    var obj = {};
    if(cache[data.type]){
      if(data.type === 'friend'){
        layui.each(cache.friend, function(index1, item1){
          layui.each(item1.list, function(index, item){
            if(data.id == item.id){
              var li = listElem.find('>li').eq(index1);
              var list = li.find('.layui-layim-list>li');
              li.find('.layui-layim-list>li').eq(index).remove();
              cache.friend[index1].list.splice(index, 1); //从cache的friend里面也删除掉好友
              li.find('.layim-count').html(cache.friend[index1].list.length); //刷新好友数量  
              //如果一个好友都没了
              if(cache.friend[index1].list.length === 0){
                li.find('.layui-layim-list').html('<li class="layim-null">该分组下已无好友了</li>');
              }
              return true;
            }
          });
        });
      } else if(data.type === 'group'){
        layui.each(cache.group, function(index, item){
          if(data.id == item.id){
            listElem.find('>li').eq(index).remove();
            cache.group.splice(index, 1); //从cache的group里面也删除掉数据
            //如果一个群组都没了
            if(cache.group.length === 0){
              listElem.html('<li class="layim-null">暂无群组</li>');
            }
            return true;
          }
        });
      }
    }
  };
  
  //查看更多记录
  var chatListMore = function(){
    var thatChat = thisChat(), chatMain = thatChat.elem.find('.layim-chat-main');
    var ul = chatMain.find('ul'); 
    
    //if(ul.find('li').length >= 50){
    //  var first = ul.find('li').eq(0);
    //  if(!ul.prev().hasClass('layim-chat-system')){
    //    ul.before('<div class="layim-chat-system"><span layim-event="chatLog">查看更多记录</span></div>');
    //  }
    //  first.remove();
    //}
    chatMain.scrollTop(chatMain[0].scrollHeight);
    chatMain.find('ul li:last').find('img').load(function(){
      chatMain.scrollTop(chatMain[0].scrollHeight);
    });
   
  };
  
  //快捷键发送
  var hotkeySend = function(){
    var thatChat = thisChat(), textarea = thatChat.textarea;
    textarea.focus();
    textarea.off('keydown').on('keydown', function(e){
      var local = layui.data('layim')[cache.mine.id] || {};
      var keyCode = e.keyCode;
      if(local.sendHotKey === 'Ctrl+Enter'){
        if(e.ctrlKey && keyCode === 13){
          sendMessage();
        }
        return;
      }
      if(keyCode === 13){
        if(e.ctrlKey){
          return textarea.val(textarea.val()+'\n');
        }
        if(e.shiftKey) return;
        e.preventDefault();
        sendMessage();
      }
    });
  };
  
  //表情库
  var faces = function(){
    var alt = ["[微笑]", "[嘻嘻]", "[哈哈]", "[可爱]", "[可怜]", "[挖鼻]", "[吃惊]", "[害羞]", "[挤眼]", "[闭嘴]", "[鄙视]", "[爱你]", "[泪]", "[偷笑]", "[亲亲]", "[生病]", "[太开心]", "[白眼]", "[右哼哼]", "[左哼哼]", "[嘘]", "[衰]", "[委屈]", "[吐]", "[哈欠]", "[抱抱]", "[怒]", "[疑问]", "[馋嘴]", "[拜拜]", "[思考]", "[汗]", "[困]", "[睡]", "[钱]", "[失望]", "[酷]", "[色]", "[哼]", "[鼓掌]", "[晕]", "[悲伤]", "[抓狂]", "[黑线]", "[阴险]", "[怒骂]", "[互粉]", "[心]", "[伤心]", "[猪头]", "[熊猫]", "[兔子]", "[ok]", "[耶]", "[good]", "[NO]", "[赞]", "[来]", "[弱]", "[草泥马]", "[神马]", "[囧]", "[浮云]", "[给力]", "[围观]", "[威武]", "[奥特曼]", "[礼物]", "[钟]", "[话筒]", "[蜡烛]", "[蛋糕]"], arr = {};
    layui.each(alt, function(index, item){
      arr[item] = layui.cache.dir + 'images/face/'+ index + '.gif';
    });
    return arr;
  }();
  
  //组件事件冒泡
  var stope = function(e){
    e = e || window.event;
    e.stopPropagation ? e.stopPropagation() : e.cancelBubble = true;
  };
  
  //在焦点处插入内容
  var focusInsert = function(obj, str){
    var result, val = obj.value;
    obj.focus();
    if(document.selection){ //ie
      result = document.selection.createRange(); 
      document.selection.empty(); 
      result.text = str; 
    } else {
       result = [
        val.substring(0, obj.selectionStart),
        str,
        val.substr(obj.selectionEnd)
      ];
      obj.focus();
      obj.value = result.join('');
    }
  };
  
  //事件
  var anim = 'layui-anim-up', events = {
      //在线状态
      status: function (othis, e) {
          var hide = function () {
              othis.next().hide().removeClass(anim);
          };
          var type = othis.attr('lay-type');
          if (type === 'show') {
              stope(e);
              othis.next().show().addClass(anim);
              $(document).off('click', hide).on('click', hide);
          } else {
              var prev = othis.parent().prev();
              othis.addClass(THIS).siblings().removeClass(THIS);
              prev.html(othis.find('cite').html());
              prev.removeClass('layim-status-' + (type === 'online' ? 'hide' : 'online'))
              .addClass('layim-status-' + type);
              layui.each(call.online, function (index, item) {
                  item && item(type);
              });
          }
      }

      //大分组切换
    , tab: function (othis) {
        var index, main = '.layim-tab-content';
        var tabs = layimMain.find('.layui-layim-tab>li');
        typeof othis === 'number' ? (
          index = othis
          , othis = tabs.eq(index)
        ) : (
          index = othis.index()
        );
        index > 2 ? tabs.removeClass(THIS) : (
          events.tab.index = index
          , othis.addClass(THIS).siblings().removeClass(THIS)
        )
        layimMain.find(main).eq(index).addClass(SHOW).siblings(main).removeClass(SHOW);
    }

      //展开联系人分组
    , spread: function (othis) {
        var type = othis.attr('lay-type');
        var spread = type === 'true' ? 'false' : 'true';
        var local = layui.data('layim')[cache.mine.id] || {};
        othis.next()[type === 'true' ? 'removeClass' : 'addClass'](SHOW);
        local['spread' + othis.parent().index()] = spread;
        layui.data('layim', {
            key: cache.mine.id
          , value: local
        });
        othis.attr('lay-type', spread);
        othis.find('.layui-icon').html(spread === 'true' ? '&#xe61a;' : '&#xe602;');
    }

      //搜索
    , search: function (othis) {
        var search = layimMain.find('.layui-layim-search');
        var main = layimMain.find('#layui-layim-search');
        var input = search.find('input'), find = function (e) {
            var val = input.val().replace(/\s/);
            if (val === '') {
                events.tab(events.tab.index | 0);
            } else {
                var data = [], friend = cache.friend || [];
                var group = cache.group || [], html = '';
                for (var i = 0; i < friend.length; i++) {
                    for (var k = 0; k < (friend[i].list || []).length; k++) {
                        if (friend[i].list[k].username.indexOf(val) !== -1) {
                            friend[i].list[k].type = 'friend';
                            friend[i].list[k].index = i;
                            friend[i].list[k].list = k;
                            data.push(friend[i].list[k]);
                        }
                    }
                }
                for (var j = 0; j < group.length; j++) {
                    if (group[j].groupname.indexOf(val) !== -1) {
                        group[j].type = 'group';
                        group[j].index = j;
                        group[j].list = j;
                        data.push(group[j]);
                    }
                }
                if (data.length > 0) {
                    for (var l = 0; l < data.length; l++) {
                        html += '<li layim-event="chat" data-type="' + data[l].type + '" data-index="' + data[l].index + '" data-list="' + data[l].list + '"><img src="' + data[l].avatar + '"><span>' + (data[l].username || data[l].groupname || '佚名') + '</span><p>' + (data[l].remark || data[l].sign || '') + '</p></li>';
                    }
                } else {
                    html = '<li class="layim-null">无搜索结果</li>';
                }
                main.html(html);
                events.tab(3);
            }
        };
        if (!cache.base.isfriend && cache.base.isgroup) {
            events.tab.index = 1;
        } else if (!cache.base.isfriend && !cache.base.isgroup) {
            events.tab.index = 2;
        }
        search.show();
        input.focus();
        input.off('keyup', find).on('keyup', find);
    }

      //关闭搜索
    , closeSearch: function (othis) {
        othis.parent().hide();
        events.tab(events.tab.index | 0);
    }

      //弹出换肤
    , skin: function () {
        layer.open({
            type: 1
          , title: '换肤'
          , shade: false
          , area: '300px'
          , skin: 'layui-box layui-layer-border'
          , id: 'layui-layim-skin'
          , zIndex: 66666666
          , content: laytpl(elemSkinTpl).render({
              skin: cache.base.skin
          })
        });
    }

      //弹出查找页面
    , find: function () {
        layer.open({
            type: 2
          , title: '查找'
          , shade: false
          , area: ['1000px', '520px']
          , skin: 'layui-box layui-layer-border'
          , id: 'layui-layim-find'
          , content: cache.base.find
        });
    }

      //弹出消息处理页面
   , apply: function () {
       layer.open({
           type: 2
 , title: '消息管理器'
 , shade: false
 , area: ['600px', '520px']
 , skin: 'layui-box layui-layer-border'
 , id: 'layui-layim-apply'
 , content: cache.base.apply
       });
       //监听打开消息窗口，让interval停止
       layui.each(call.applyOpen, function (index, item) {
           item && item();
       });
   }

      //关于
    , about: function () {
        layer.alert('版本： ' + v + '<br>版权所有：<a href="http://layim.layui.com" target="_blank">layim.layui.com</a>', {
            title: '关于 LayIM'
          , shade: false
        });

    }

      //生成换肤
    , setSkin: function (othis) {
        var src = othis.attr('src');
        var local = layui.data('layim')[cache.mine.id] || {};
        local.skin = src;
        if (!src) delete local.skin;
        layui.data('layim', {
            key: cache.mine.id
          , value: local
        });
        try {
            layimMain.css({
                'background-image': src ? 'url(' + src + ')' : 'none'
            });
            layimChat.css({
                'background-image': src ? 'url(' + src + ')' : 'none'
            });
        } catch (e) { }
    }

      //弹出聊天面板
    , chat: function (othis) {
        var local = layui.data('layim')[cache.mine.id] || {};
        var type = othis.data('type'), index = othis.data('index');
        var list = othis.attr('data-list') || othis.index(), data = {};
        if (type === 'friend') {
            data = cache[type][index].list[list];
        } else if (type === 'group') {
            data = cache[type][list];
        } else if (type === 'history') {
            data = (local.history || {})[index] || {};
        }
        data.name = data.name || data.username || data.groupname;
        if (type !== 'history') {
            data.type = type;
        }
        popchat(data);
    }

      //切换聊天
    , tabChat: function (othis) {
        changeChat(othis);
    }

      //关闭聊天列表
    , closeChat: function (othis) {
        changeChat(othis.parent(), 1);
    }, closeThisChat: function () {
        changeChat(null, 1);
    }

      //展开群组成员
    , groupMembers: function (othis, e) {
        var icon = othis.find('.layui-icon'), hide = function () {
            icon.html('&#xe61a;');
            othis.data('down', null);
            layer.close(events.groupMembers.index);
        }, stopmp = function (e) { stope(e) };

        if (othis.data('down')) {
            hide();
        } else {
            icon.html('&#xe619;');
            othis.data('down', true);
            events.groupMembers.index = layer.tips('<ul class="layim-members-list"></ul>', othis, {
                tips: 3
              , time: 0
              , shift: 5
              , fix: true
              , skin: 'layui-box layui-layim-members'
              , success: function (layero) {
                  var members = cache.base.members || {}, thatChat = thisChat();
                  var li = '';
                  members.data = $.extend(members.data, {
                      id: thatChat.data.id
                  });
                  post(members, function (res) {
                      layui.each(res.list, function (index, item) {
                          li += '<li><a><img src="' + item.avatar + '"></a><p>' + item.username + '</p></li>';
                      });
                      layero.find('.layim-members-list').html(li);
                      layui.each(call.members, function (index, item) {
                          item && item(res);
                      });
                      othis.find('.layim-chat-members').html((res.list || []).length + '人');
                  });
                  layero.on('mousedown', function (e) {
                      stope(e);
                  });
              }
            });
            $(document).off('mousedown', hide).on('mousedown', hide);
            $(window).off('resize', hide).on('resize', hide);
            othis.off('mousedown', stopmp).on('mousedown', stopmp);

        }
    }

      //发送聊天内容
    , send: function () {
        sendMessage();
    }

      //设置发送聊天快捷键
    , setSend: function (othis, e) {
        var box = othis.siblings('.layim-menu-box'), hide = function () {
            box.hide().removeClass(anim);
        };
        var type = othis.attr('lay-type');
        if (type === 'show') {
            stope(e);
            box.show().addClass(anim);
            $(document).off('click', hide).on('click', hide);
        } else {
            othis.addClass(THIS).siblings().removeClass(THIS);
            var local = layui.data('layim')[cache.mine.id] || {};
            local.sendHotKey = type;
            layui.data('layim', {
                key: cache.mine.id
              , value: local
            });
        }
    }

      //表情
    , face: function (othis, e) {
        var content = '', thatChat = thisChat(), hide = function () {
            layer.close(events.face.index);
        };

        for (var key in faces) {
            content += '<li title="' + key + '"><img src="' + faces[key] + '"></li>';
        }
        content = '<ul class="layui-clear layim-face-list">' + content + '</ul>';

        events.face.index = layer.tips(content, othis, {
            tips: 1
           , time: 0
           , fix: true
           , skin: 'layui-box layui-layim-face'
           , success: function (layero) {
               layero.find('.layim-face-list>li').on('mousedown', function (e) {
                   stope(e);
               }).on('click', function () {
                   focusInsert(thatChat.textarea[0], 'face' + this.title + ' ');
                   layer.close(events.face.index);
               });
           }
        });

        $(document).off('mousedown', hide).on('mousedown', hide);
        $(window).off('resize', hide).on('resize', hide);
        stope(e);
    }

      //图片或一般文件
    , image: function (othis) {
        var type = othis.data('type') || 'images', api = {
            images: 'uploadImage'
          , file: 'uploadFile'
        };
        var thatChat = thisChat(), upload = cache.base[api[type]] || {};
        var uploadObj = $('.layim-pz-' + type);
        var wordstr = type == 'images' ? '&#xe60d;' : '&#xe61d;';
        console.log('打开上传文件窗口。。。。');
        layui.upload({
            url: upload.url || ''
          , method: upload.type
          , file: othis.find('input')[0]
          , unwrap: true
          , check: type
          , before: function () {
              //2016-8-7 panzi add
              uploadObj.find('i').remove();
              uploadObj.find('img').show();
          }
          , success: function (res) {
              console.log('文件上传完毕。。。。');
              uploadObj.prepend('<i>' + wordstr + '</i>');
              uploadObj.find('img').hide();
              try {
                  res = JSON.parse(res);
              } catch (e) {
                  res = {};
                  return layer.msg('请对上传接口返回JSON字符');
              }
              if (res.code == 0) {
                  res.data = res.data || {};
                  if (type === 'images') {
                      focusInsert(thatChat.textarea[0], 'img[' + (res.data.src || '') + ']');
                  } else if (type === 'file') {
                      focusInsert(thatChat.textarea[0], 'file(' + (res.data.src || '') + ')[' + (res.data.name || '下载文件') + ']');
                  }
              } else {
                  layer.msg(res.msg || '上传失败');
              }
          }
        });
    }

      //聊天记录
    , chatLog: function (othis) {
        var thatChat = thisChat();
        if (!cache.base.chatLog) {
            return layer.msg('未开启更多聊天记录');
        }
        layer.close(events.chatLog.index);
        return events.chatLog.index = layer.open({
            type: 2
          , maxmin: true
          , title: '与 ' + thatChat.data.name + ' 的聊天记录'
          , area: ['450px', '100%']
          , shade: false
          , offset: 'rb'
          , skin: 'layui-box'
          , shift: 2
          , id: 'layui-layim-chatlog'
          , content: cache.base.chatLog + '?id=' + thatChat.data.id + '&type=' + thatChat.data.type
        });
    }

      //历史会话右键菜单操作
    , menuHistory: function (othis, e) {
        var local = layui.data('layim')[cache.mine.id] || {};
        var parent = othis.parent(), type = othis.data('type');
        var hisElem = layimMain.find('.layim-list-history');
        var none = '<li class="layim-null">暂无历史会话</li>'

        if (type === 'one') {
            var history = local.history;
            delete history[parent.data('index')];
            local.history = history;
            layui.data('layim', {
                key: cache.mine.id
              , value: local
            });
            $('#' + parent.data('id')).remove();
            if (hisElem.find('li').length === 0) {
                hisElem.html(none);
            }
        } else if (type === 'all') {
            delete local.history;
            layui.data('layim', {
                key: cache.mine.id
              , value: local
            });
            hisElem.html(none);
        }

        layer.closeAll('tips');
    }
      /*
      menu_  chat profile history nomsg delete moveto  refresh online newgroup rename delgroup
      */
     , menu_chat: function () {
         //打开聊天窗口
         events.chat(global.othis);
         //关闭右键菜单
         layer.closeAll('tips');
     },
     menu_profile: function () { layer.alert('查看个人资料，增加个人资料弹层即可'); layer.closeAll('tips'); },
     menu_history: function () { layer.alert('打开历史记录，调用源代码中打开历史记录方法'); layer.closeAll('tips'); },
     menu_nomsg: function () { layer.alert('屏蔽聊天消息'); layer.closeAll('tips'); },
     menu_delete: function () { layer.alert('删除该好友'); layer.closeAll('tips'); },
     menu_moveto: function () { layer.alert('移动至'); layer.closeAll('tips'); },
     menu_refresh: function () { layer.alert('刷新好友列表'); layer.closeAll('tips'); },
     menu_online: function () { layer.alert('只显示在线好友'); layer.closeAll('tips'); },
     menu_newgroup: function () { layer.alert('新建组'); layer.closeAll('tips'); },
     menu_rename: function () { layer.alert('重命名'); layer.closeAll('tips'); },
     menu_delgroup: function () { layer.alert('删除组'); layer.closeAll('tips'); },
  };
  
  //暴露接口
  exports('layim', new LAYIM());

}).addcss(
  'pc/layim/layim.css?v=2.083'
  ,'skinlayimcss'
);