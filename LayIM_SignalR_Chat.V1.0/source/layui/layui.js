/*!

 @Title: layui
 @Description：跨终端模块化前端框架
 @Site: http://www.layui.com
 @Author: 贤心
 @License：LGPL

 */
 
;!function(win){

var LAY = function(){
  this.v = '0.0.7'; //版本号
};

LAY.fn = LAY.prototype;

var doc = document, gather = {}, config = LAY.fn.cache = {},

//获取layui所在目录
getPath = function(){
  var js = doc.scripts, jsPath = js[js.length - 1].src;
  return jsPath.substring(0, jsPath.lastIndexOf("/") + 1);
}(),

//异常提示
error = function(msg){
  win.console && console.error && console.error('layui Error: ' + msg);
},

isOpera = typeof opera !== 'undefined' && opera.toString() === '[object Opera]';

config.device = 'pc'; //设备
config.modules = { }; //记录模块物理路径
config.status = { }; //记录模块加载状态
config.timeout = 10; //符合规范的模块请求最长等待秒数

//定义模块
LAY.fn.define = function(deps, callback){
  var that = this, type = typeof deps === 'function';
  type && (callback = deps);
  that.use(type ? [] : deps, function(){
    typeof callback === 'function' && callback(function(app, exports){
      layui[app] = exports;
      config.status[app] = true;
    });
  });
  return that;
};

//使用特定模块
LAY.fn.use = function(apps, callback, exports){
  var that = this, dir = config.dir = config.dir ? config.dir : getPath;
  var head = doc.getElementsByTagName('head')[0];

  apps = typeof apps === 'string' ? [apps] : apps;
  
  var item = apps[0], timeout = 0;
  exports = exports || [];

  //扩展目录
  config.host = config.host || (dir.match(/\/\/([\s\S]+?)\//)||['//'+ location.host +'/'])[0]; //静态资源host
  
  if(apps.length === 0){
    return callback();
  }
  
  //加载完毕
  function onScriptLoad(e, url){
    var readyRegExp = navigator.platform === 'PLAYSTATION 3' ? /^complete$/ : /^(complete|loaded)$/
    if (e.type === 'load' || (readyRegExp.test((e.currentTarget || e.srcElement).readyState))) {
      config.modules[item] = url;
      head.removeChild(node);
      (function poll() {
        if(++timeout > config.timeout * 1000 / 4){
          return error(item + ' is not a valid module');
        };
        config.status[item] ? onCallback() : setTimeout(poll, 4);
      }());
    }
  }

  //加载模块
  var node = doc.createElement('script'), url =  (gather.modules[item]
   ? (dir + 'lay/')
   : (config.base || '')
  ) + (that.modules[item] || item) + '.js';
  node.async = true;
  node.src = url + "?v=" + function(){
    return config.version === true 
    ? (config.v || (new Date()).getTime())
    : (config.version||'');
  }();
  
  //首次加载
  if(!config.modules[item]){
    head.appendChild(node);
    if(node.attachEvent && !(node.attachEvent.toString && node.attachEvent.toString().indexOf('[native code') < 0) && !isOpera){
      node.attachEvent('onreadystatechange', function(e){
        onScriptLoad(e, url);
      });
    } else {
      node.addEventListener('load', function(e){
        onScriptLoad(e, url);
      }, false);
    }
  } else {
    (function poll() {
        if (++timeout > config.timeout * 1000 / 4) {
        return error(item + ' is not a valid module');
      };
      (typeof config.modules[item] === 'string' && config.status[item]) 
      ? onCallback() 
      : setTimeout(poll, 4);
    }());
  }
  
  config.modules[item] = url;
  
  //回调
  function onCallback(){
    exports.push(layui[item]);
    apps.length > 1 ?
      that.use(apps.slice(1), callback, exports)
    : ( typeof callback === 'function' && callback.apply(layui, exports) );
  }

  return that;

};

//使用所有内置模块
LAY.fn.all = function(callback){
  var that = this;
  var modules = Object.keys ? Object.keys(that.modules) : function(){
    var arr = [];
    for(var key in that.modules){
      arr.push(key);
    }
    return arr;
  }();
  layui.use(modules, callback);
  return that;
};

//获取外联样式
LAY.fn.getStyle = function(obj, name){
  var style = obj.currentStyle ? obj.currentStyle : win.getComputedStyle(obj, null);
  return style[style.getPropertyValue ? 'getPropertyValue' : 'getAttribute'](name);
};

//css外部加载器
LAY.fn.link = function(href, fn, cssname){
  var that = this, link = doc.createElement('link');
  var head = doc.getElementsByTagName('head')[0];
  if(typeof fn === 'string') cssname = fn;
  var app = (cssname || href).replace(/\.|\//g, '');
  var id = link.id = 'layuicss-'+app, timeout = 0;
  
  link.rel = 'stylesheet';
  link.href = href + (config.debug ? '?v='+new Date().getTime() : '');
  link.media = 'all';
  
  if(!doc.getElementById(id)){
    head.appendChild(link);
  }

  if(typeof fn !== 'function') return ;
  
  //轮询css是否加载完毕
  (function poll() {
    if(++timeout > config.timeout * 1000 / 100){
      return error(href + ' timeout');
    };
    parseInt(that.getStyle(doc.getElementById(id), 'width')) === 1989 ? function(){
      fn();
    }() : setTimeout(poll, 100);
  }());
};

//css内部加载器
LAY.fn.addcss = function(firename, fn, cssname){
  layui.link(config.dir + 'css/' + firename, fn, cssname);
};

//路由
LAY.fn.router = function(hash){
  var hashs = (hash || location.hash).replace(/^#/, '').split('/') || [];
  var item, param = {
    dir: []
  };
  for(var i = 0; i < hashs.length; i++){
    item = hashs[i].split('=');
    /^\w+=/.test(hashs[i]) ? function(){
      if(item[0] !== 'dir'){
        param[item[0]] = item[1];
      }
    }() : param.dir.push(hashs[i]);
    item = null;
  }
  return param;
};

//图片预加载
LAY.fn.img = function(url, callback, error) {   
  var img = new Image();
  img.src = url; 
  if(img.complete){
    return callback(img);
  }
  img.onload = function(){
    img.onload = null;
    callback(img);
  };
  img.onerror = function(e){
    img.onerror = null;
    error(e);
  };  
};

//本地存储
LAY.fn.data = function(sets, options){
  sets = sets || 'layui';
  options = typeof options === 'object' 
    ? options 
  : {key: options};
  if(!window.JSON || !window.JSON.parse) return;
  try{
    var data = JSON.parse(localStorage[sets]);
  } catch(e){
    var data = {};
  }
  if(options.value) data[options.key] = options.value;
  if(options.remove) delete data[options.key];
  localStorage[sets] = JSON.stringify(data);
  return options.key ? data[options.key] : data;
};

//遍历
LAY.fn.each = function(obj, fn){
  if(typeof fn !== 'function'){
    return;
  }
  obj = obj || [];
  if(obj.constructor === Array){
    for(var i = 0; i < obj.length; i++){
      if(fn(i, obj[i])){
        break;
      }
    }
  } else if(obj.constructor === Object){
    for(var key in obj){
      if(fn(key, obj[key])){
        break;
      }
    }
  }
};

//初始化配置
LAY.fn.config = function(options){
  options = options || {};
  for(var key in options){
    config[key] = options[key];
  }
  return this;
};

//内置模块
gather.modules = {
  laytpl: 'lib/laytpl' //模板引擎
  ,laypage: 'lib/laypage' //分页
  ,socket: 'lib/socket' //WebSocket库

  //多终端
  ,terminal: {
    
    //电脑端模块
    pc: {
        jquery: 'pc/lib/jquery' //DOM库
      ,layout: 'pc/modules/layout' //布局（大框架、Tab选项卡、固定块、侧边栏、面包屑）
      ,layer: 'pc/modules/layer' //弹层
      ,laydate: 'pc/modules/laydate' //日期
      ,layim: 'pc/modules/layim' //web通讯
      ,tree: 'pc/modules/tree' //树结构
      ,slide: 'pc/modules/slide' //轮播
      ,editor: 'pc/modules/editor' //富文本编辑器
      ,table: 'pc/modules/table' //富表格
      ,flow: 'pc/modules/flow' //流加载（图片延迟、瀑布流、信息流）
      ,util: 'pc/modules/util' //工具集
      ,form: 'pc/modules/form' //表单（验证、密码强度、下拉选择、输入选择、多选框）
      ,code: 'pc/modules/code' //代码高亮
      ,upload: 'pc/modules/upload' //上传
      ,single: 'pc/modules/single' //单页应用
    }
    
    //移动端模块
    ,mobile: {
      zepto: 'mobile/lib/zepto' //DOM库
      ,layer: 'mobile/modules/layer' //layer mobile
      ,util: 'mobile/modules/util' //基础工具
      ,ui: 'mobile/modules/ui' //U
      ,form: 'mobile/modules/form' //表单组件
      
      ,debug: 'mobile/modules/debug' //调试工具
    }
  }
};

//获取全部模块
LAY.fn.modules = function(){
  var that = this, modules = {};

  //加入当前终端模块
  for(var o in gather.modules){
    if(o === 'terminal'){
      var terminal = gather.modules.terminal[config.device];
      for(var key in terminal){
        modules[key] = terminal[key];
        gather.modules[key] = terminal[key];
      }
      delete gather.modules.terminal;
    } else {
      modules[o] = gather.modules[o];
    }
  }
  return modules;
}();

//拓展模块
LAY.fn.extend = function(options){
  var that = this;

  //验证模块是否被占用
  options = options || {};
  for(var o in options){
    if(that[o] || that.modules[o]){
      error('\u6A21\u5757\u540D '+ o +' \u5DF2\u88AB\u5360\u7528');
    } else {
      that.modules[o] = options[o];
    }
  }
  
  return that;
};

win.layui = new LAY();

}(window);

