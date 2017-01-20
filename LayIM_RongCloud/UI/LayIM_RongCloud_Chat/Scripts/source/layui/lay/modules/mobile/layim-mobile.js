/**

 @Name：layim mobile 1.0.2 Beta 临时版
 @Author：贤心
 @Site：http://layim.layui.com
 @License：LGPL
    
 */
 
layui.define(['laytpl', 'upload-mobile', 'layer-mobile', 'zepto'], function(exports){
  
  var v = '1.0 Beta';
  var $ = layui.zepto;
  var laytpl = layui.laytpl;
  var layer = layui['layer-mobile'];
  var upload = layui['upload-mobile'];
  
  var SHOW = 'layui-show', THIS = 'layim-this', MAX_ITEM = 20;

  //回调
  var call = {};
  
  //对外API
  var LAYIM = function(){
    this.v = v;
    $('body').on('click', '*[layim-event]', function(e){
      var othis = $(this), methid = othis.attr('layim-event');
      events[methid] ? events[methid].call(this, othis, e) : '';
    });
  };
  
  //底部弹出
  layer.popBottom = function(options){
    layer.close(layer.popBottom.index);
    layer.popBottom.index = layer.open($.extend({
      type: 1
      ,content: options.content || ''
      ,shade: false
      ,className: 'layim-layer'
    }, options));
  };
  
  //基础配置
  LAYIM.prototype.config = function(options){
    options = options || {};
    options = $.extend({
      voice: 'default.wav'
      ,chatTitleColor: '#36373C'
    }, options);
    init(options);
  };
  
  //监听事件
  LAYIM.prototype.on = function(events, callback){
    if(typeof callback === 'function'){
      call[events] ? call[events].push(callback) : call[events] = [callback];
    }
    return this;
  };
  
  //打开一个自定义的会话界面
  LAYIM.prototype.chat = function(data){
    if(!window.JSON || !window.JSON.parse) return;
    return popchat(data), this;
  };

  //获取所有缓存数据
  LAYIM.prototype.cache = function(){
    return cache;
  };
  
  //接受消息
  LAYIM.prototype.getMessage = function(data){
    return getMessage(data), this;
  };
  
  //设置当前会话状态
  LAYIM.prototype.setChatStatus = function(str){
    var thatChat = thisChat(), status = thatChat.elem.find('.layim-chat-status');
    return status.html(str), this;
  };
  
  //解析聊天内容
  LAYIM.prototype.content = function(content){
    return layui.data.content(content);
  };
  
  //聊天主模板
  var elemChatTpl = ['<div class="layim-chat layim-chat-{{d.data.type}}{{d.first ? " layui-show" : ""}}">'
    ,'{{# if(d.base.chatTitleColor){ }}'
    ,'<div class="layim-chat-title" style="background-color: {{d.base.chatTitleColor}};">'
      ,'<p>{{ d.data.name }}<span class="layim-chat-status"></span></p>'
    ,'</div>'
    ,'{{# } }}'
    ,'<div class="layim-chat-main" {{d.base.chatTitleColor ? "" : "nonetitle"}}>'
      ,'<ul></ul>'
    ,'</div>'
    ,'<div class="layim-chat-footer">'
      ,'<div class="layim-chat-send"><input type="text" autocomplete="off"><button class="layim-send layui-disabled" layim-event="send">发送</button></div>'
      ,'<div class="layim-chat-tool" data-json="{{encodeURIComponent(JSON.stringify(d.data))}}">'
        ,'<span class="layui-icon layim-tool-face" title="选择表情" layim-event="face">&#xe60c;</span>'
        ,'{{# if(d.base && d.base.uploadImage){ }}'
        ,'<span class="layui-icon layim-tool-image" title="上传图片" layim-event="image">&#xe60d;<input type="file" name="file"></span>'
        ,'{{# }; }}'
        ,'{{# if(d.base && d.base.uploadFile){ }}'
        ,'<span class="layui-icon layim-tool-image" title="发送文件" layim-event="image" data-type="file">&#xe61d;<input type="file" name="file"></span>'
         ,'{{# }; }}'
         ,'{{# layui.each(d.base.tool, function(index, item){ }}'
        ,'<span class="layui-icon layim-tool-{{item.alias}}" title="{{item.title}}" layim-event="extend" lay-filter="{{ item.alias }}">{{item.icon}}</span>'
         ,'{{# }); }}'
      ,'</div>'
    ,'</div>'
  ,'</div>'].join('');
  
  //补齐数位
  var digit = function(num){
    return num < 10 ? '0' + (num|0) : num;
  };
  
  //转换时间
  layui.data.date = function(timestamp){
    var d = new Date(timestamp||new Date());
    return digit(d.getMonth() + 1) + '-' + digit(d.getDate())
    + ' ' + digit(d.getHours()) + ':' + digit(d.getMinutes());
  };
  
  //转换内容
  layui.data.content = function(content){
    //支持的html标签
    var html = function(end){
      return new RegExp('\\n*\\['+ (end||'') +'(pre|div|p|table|thead|th|tbody|tr|td|ul|li|ol|li|dl|dt|dd|h2|h3|h4|h5)([\\s\\S]*?)\\]\\n*', 'g');
    };
    content = (content||'').replace(/&(?!#?[a-zA-Z0-9]+;)/g, '&amp;')
    .replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/'/g, '&#39;').replace(/"/g, '&quot;') //XSS
    .replace(/@(\S+)(\s+?|$)/g, '@<a href="javascript:;">$1</a>$2') //转义@
    .replace(/\s{2}/g, '&nbsp;') //转义空格
    .replace(/img\[([^\s]+?)\]/g, function(img){  //转义图片
      return '<img class="layui-layim-photos" src="' + img.replace(/(^img\[)|(\]$)/g, '') + '">';
    })
    .replace(/file\([\s\S]+?\)\[[\s\S]*?\]/g, function(str){ //转义文件
      var href = (str.match(/file\(([\s\S]+?)\)\[/)||[])[1];
      var text = (str.match(/\)\[([\s\S]*?)\]/)||[])[1];
      if(!href) return str;
      return '<a class="layui-layim-file" href="'+ href +'" download target="_blank"><i class="layui-icon">&#xe61e;</i><cite>'+ (text||href) +'</cite></a>';
    })
    .replace(/face\[([^\s\[\]]+?)\]/g, function(face){  //转义表情
      var alt = face.replace(/^face/g, '');
      return '<img alt="'+ alt +'" title="'+ alt +'" src="' + faces[alt] + '">';
    }).replace(/a\([\s\S]+?\)\[[\s\S]*?\]/g, function(str){ //转义链接
      var href = (str.match(/a\(([\s\S]+?)\)\[/)||[])[1];
      var text = (str.match(/\)\[([\s\S]*?)\]/)||[])[1];
      if(!href) return str;
      return '<a href="'+ href +'" target="_blank">'+ (text||href) +'</a>';
    }).replace(html(), '\<$1 $2\>').replace(html('/'), '\</$1\>') //转移HTML代码
    .replace(/\n/g, '<br>') //转义换行 
    return content;
  };
  
  var elemChatMain = ['<li class="layim-chat-li{{ d.mine ? " layim-chat-mine" : "" }}">'
    ,'<div class="layim-chat-user"><img src="{{ d.avatar }}"><cite>'
      ,'{{ d.username||"佚名" }}'
    ,'</cite></div>'
    ,'<div class="layim-chat-text">{{ layui.data.content(d.content||"&nbsp;") }}</div>'
  ,'</li>'].join('');
  
  //处理初始化信息
  var cache = {message: {}, chat: []}, init = function(options){
    var init = options.init || {}
     mine = init.mine || {}
    ,local = layui.data('layim-mobile')[mine.id] || {}
    ,obj = {
      base: options
      ,local: local
      ,mine: mine
      ,history: local.history || {}
    }, create = function(data){
      var mine = data.mine || {};
      var local = layui.data('layim-mobile')[mine.id] || {}, obj = {
        base: options //基础配置信息
        ,local: local //本地数据
        ,mine:  mine //我的用户信息
        ,friend: data.friend || [] //联系人信息
        ,group: data.group || [] //群组信息
        ,history: local.history || {} //历史会话信息
      };
      cache = $.extend(cache, obj);

      layui.each(call.ready, function(index, item){
        item && item(obj);
      });
    };
    cache = $.extend(cache, obj);
    if(options.brief){
      return layui.each(call.ready, function(index, item){
        item && item(obj);
      });
    };
    create(init)
  };
  
  //显示聊天面板
  var layimChat, layimMin, To = {}, popchat = function(data){
    data = data || {};
    
    var chat = $('#layui-layim-chat'), render = {
      data: data
      ,base: cache.base
      ,local: cache.local
    };

    if(!data.id){
      return layer.msg('非法用户');
    }
    
    layer.close(popchat.index);

    return popchat.index = layer.open({
      type: 1
      ,className: 'layui-layim-chat'
      ,shade: false
      ,anim: -1
      ,content: laytpl(elemChatTpl).render(render)
      ,success: function(elem){
        layimChat = $(elem);
        
        hotkeySend();
        viewChatlog();
        
        //聊天窗口的切换监听
        layui.each(call.chatChange, function(index, item){
          item && item(thisChat());
        });
      }
      ,end: function(){
        layimChat = null;
      }
    });
  };
  
  //获取当前聊天面板
  var thisChat = function(){
    var cont = layimChat.find('.layim-chat');
    var to = JSON.parse(decodeURIComponent(cont.find('.layim-chat-tool').data('json')));
    return {
      elem: cont
      ,data: to
      ,textarea: cont.find('input')
    };
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
    var To = thatChat.data, maxLength = cache.base.maxLength || 3000;
    var time =  new Date().getTime();
    
    data.content = thatChat.textarea.val();
    
    if(data.content === '') return;

    if(data.content.length > maxLength){
      return layer.msg('内容最长不能超过'+ maxLength +'个字符')
    }
    
    if(time - (sendMessage.time||0) > 60*1000){
      ul.append('<li class="layim-chat-system"><span>'+ layui.data.date() +'</span></li>');
      sendMessage.time = time;
    }
    ul.append(laytpl(elemChatMain).render(data));
    
    var param = {
      mine: data
      ,to: To
    }, message = {
      username: param.mine.username
      ,avatar: param.mine.avatar
      ,id: To.id
      ,type: To.type
      ,content: param.mine.content
      ,timestamp: time
      ,mine: true
    };
    pushChatlog(message);
    
    layui.each(call.sendMessage, function(index, item){
      item && item(param);
    });
    
    chatListMore();
    thatChat.textarea.val('').focus();
  };
  
  //消息声音提醒
  var voice = function() {
    var audio = document.createElement("audio");
    audio.src = layui.cache.dir+'css/modules/layim/voice/'+ cache.base.voice;
    audio.play();
  };
  
  //接受消息
  var messageNew = {}, getMessage = function(data){
    data = data || {};
    
    var group = {}
    
    data.timestamp = data.timestamp || new Date().getTime();
    data.system || pushChatlog(data);
    messageNew = JSON.parse(JSON.stringify(data));
    
    if(cache.base.voice){
      voice();
    }
    
    if(!layimChat && data.content){
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
            data.name = data.username;
            data.temporary = true; //临时会话
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
      if(!data.system){
        //显示消息提示区域
      }
    }
    
    if(!layimChat) return;

    var cont = layimChat.find('.layim-chat');
    var ul = cont.find('.layim-chat-main ul');
    
    //系统消息
    if(data.system){
      ul.append('<li class="layim-chat-system"><span>'+ data.content +'</span></li>');
    } else if(data.content.replace(/\s/g, '') !== ''){
      if(data.timestamp - (sendMessage.time||0) > 60*1000){
        ul.append('<li class="layim-chat-system"><span>'+ layui.data.date(data.timestamp) +'</span></li>');
        sendMessage.time = data.timestamp;
      }
      ul.append(laytpl(elemChatMain).render(data));
    }
    
    chatListMore();
  };
  
  //存储最近MAX_ITEM条聊天记录到本地
  var pushChatlog = function(message){
    var local = layui.data('layim-mobile')[cache.mine.id] || {};
    var chatlog = local.chatlog || {};
    if(chatlog[message.type + message.id]){
      chatlog[message.type + message.id].push(message);
      if(chatlog[message.type + message.id].length > MAX_ITEM){
        chatlog[message.type + message.id].shift();
      }
    } else {
      chatlog[message.type + message.id] = [message];
    }
    local.chatlog = chatlog;
    layui.data('layim-mobile', {
      key: cache.mine.id
      ,value: local
    });
  };
  
  //渲染本地最新聊天记录到相应面板
  var viewChatlog = function(){
    var local = layui.data('layim-mobile')[cache.mine.id] || {};
    var thatChat = thisChat(), chatlog = local.chatlog || {};
    var ul = thatChat.elem.find('.layim-chat-main ul');
    layui.each(chatlog[thatChat.data.type + thatChat.data.id], function(index, item){
      if(new Date().getTime() > item.timestamp && item.timestamp - (sendMessage.time||0) > 60*1000){
        ul.append('<li class="layim-chat-system"><span>'+ layui.data.date(item.timestamp) +'</span></li>');
        sendMessage.time = item.timestamp;
      }
      ul.append(laytpl(elemChatMain).render(item));
    });
    chatListMore();
  };
  
  //查看更多记录
  var chatListMore = function(){
    var thatChat = thisChat(), chatMain = thatChat.elem.find('.layim-chat-main');
    var ul = chatMain.find('ul'), li = ul.children('.layim-chat-li'); 
    
    if(li.length >= MAX_ITEM){
      var first = li.eq(0);
      if(!ul.prev().hasClass('layim-chat-system')){
        ul.before('<div class="layim-chat-system"><span layim-event="chatLog">查看更多记录</span></div>');
      }
      first.remove();
    }
    chatMain.scrollTop(chatMain[0].scrollHeight + 1000);
  };
  
  //快捷键发送
  var hotkeySend = function(){
    var thatChat = thisChat(), textarea = thatChat.textarea;
    var btn = textarea.next();
    textarea.off('keyup').on('keyup', function(e){
      var keyCode = e.keyCode;
      if(keyCode === 13){
        e.preventDefault();
        sendMessage();
      }
      btn[textarea.val() === '' ? 'addClass' : 'removeClass']('layui-disabled')
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
  
  
  var stope = layui.stope; //组件事件冒泡
  
  //在焦点处插入内容
  var focusInsert = function(obj, str, nofocus){
    var result, val = obj.value;
    nofocus || obj.focus();
    if(document.selection){ //ie
      result = document.selection.createRange(); 
      document.selection.empty(); 
      result.text = str; 
    } else {
      result = [val.substring(0, obj.selectionStart), str, val.substr(obj.selectionEnd)];
      nofocus || obj.focus();
      obj.value = result.join('');
    }
  };
  
  //事件
  var anim = 'layui-anim-upbit', events = { 
    //弹出聊天面板
    chat: function(othis){
      var local = layui.data('layim-mobile')[cache.mine.id] || {};
      var type = othis.data('type'), index = othis.data('index');
      var list = othis.attr('data-list') || othis.index(), data = {};
      if(type === 'friend'){
        data = cache[type][index].list[list];
      } else if(type === 'group'){
        data = cache[type][list];
      } else if(type === 'history'){
        data = (local.history || {})[index] || {};
      }
      data.name = data.name || data.username || data.groupname;
      if(type !== 'history'){
        data.type = type;
      }
      popchat(data);
    }
    
    //发送聊天内容
    ,send: function(){
      sendMessage();
    }
    
    //表情
    ,face: function(othis, e){
      var content = '', thatChat = thisChat(), input = thatChat.textarea;
      layui.each(faces, function(key, item){
         content += '<li title="'+ key +'"><img src="'+ item +'"></li>';
      });
      content = '<ul class="layui-layim-face">'+ content +'</ul>';
      layer.popBottom({
        content: content
        ,success: function(elem){
          var list = $(elem).find('.layui-layim-face>li')
          list.on('click', function(){
            focusInsert(input[0], 'face' +  this.title + ' ', true);
            input.next()[input.val() === '' ? 'addClass' : 'removeClass']('layui-disabled');
            return false;
          });
        }
      });
      
      $(document).off('click', events.faceHide).on('click', events.faceHide);
      stope(e);
      
    } ,faceHide: function(){
      layer.close(layer.popBottom.index);
    }
    
    //图片或一般文件
    ,image: function(othis){
      var type = othis.data('type') || 'images', api = {
        images: 'uploadImage'
        ,file: 'uploadFile'
      }
      ,thatChat = thisChat(), conf = cache.base[api[type]] || {};
      upload({
        url: conf.url || ''
        ,method: conf.type
        ,elem: othis.find('input')[0]
        ,unwrap: true
        ,type: type
        ,success: function(res){
          if(res.code == 0){
            res.data = res.data || {};
            if(type === 'images'){
              focusInsert(thatChat.textarea[0], 'img['+ (res.data.src||'') +']');
            } else if(type === 'file'){
              focusInsert(thatChat.textarea[0], 'file('+ (res.data.src||'') +')['+ (res.data.name||'下载文件') +']');
            }
            sendMessage();
          } else {
            layer.msg(res.msg||'上传失败');
          }
        }
      });
    }
    
    //扩展工具栏
    ,extend: function(othis){
      var filter = othis.attr('lay-filter')
      ,thatChat = thisChat();
      
      layui.each(call['tool('+ filter +')'], function(index, item){
        item && item(function(content){
          focusInsert(thatChat.textarea[0], content)
        });;
      });
    }
    
    //聊天记录
    ,chatLog: function(othis){
      var thatChat = thisChat();
      layui.each(call.chatlog, function(index, item){
        item && item({
          id: thatChat.data.id
          ,type: thatChat.data.type
          ,elem: thatChat.elem.find('.layim-chat-main>ul')
        });
      });
    }
    
  };
  
  //暴露接口
  exports('layim-mobile', new LAYIM());

}).addcss(
  'modules/layim/mobile/layim.css?v=1.0Beta'
  ,'skinlayim-mobilecss'
);