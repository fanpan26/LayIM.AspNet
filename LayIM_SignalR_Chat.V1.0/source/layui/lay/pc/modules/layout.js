/**

 @Name：layui.layout 布局
 @Author：贤心
 @License：LGPL
    
*/

layui.define('jquery', function(exports){
  var $ = layui.jquery;
  
  exports('layout', {
    
    //Tab选项卡
    tab: function(callback){
      var tabs = $('.layui-tab'), tabThis = 'layui-tab-this'
      
      ,html = '<div class="layui-tab-bar"><span class="layui-icon layui-tab-left">&#xe603;</span>'
      +'<span class="layui-icon layui-tab-right">&#xe602;</span></div>'
      
      ,auto = function(title, othis){
        var num = 0, left = 0, list = title.find('li'), length = list.length;
        
        title[0].scrollWidth > othis.width() ? (
           title.hasClass('layui-tab-scroll') || title.addClass('layui-tab-scroll'),
           othis.append(html)
        ) : (
          title.removeClass('layui-tab-scroll').css('left', 0),
          othis.find('.layui-tab-bar').remove()
        );
        
        othis.find('.layui-tab-bar>span').off('click').on('click', function(){
          var index = $(this).index();
          index === 0 ? num-- : num++;
          if(num < 0) return num = 0;
          if(num > length - 1) return num = length - 1;
          left += (index === 0 ? 1 : -1)*(list.eq(num-(index === 0 ? 0 : 1)).outerWidth() + 1);
          list.eq(0).css('margin-left', left);
        });
      };
      
      $.each(tabs, function(index){
        var othis = $(this), title = othis.find('.layui-tab-title');
        var item = othis.find('.layui-tab-item');

        title.find('>li').on('click', function(){
          var othis = $(this), index = othis.index();
          if(!othis.hasClass(tabThis)){
            othis.addClass(tabThis).siblings('li').removeClass(tabThis);
            item.eq(index).addClass('layui-show').siblings('.layui-tab-item').removeClass('layui-show');
          }
        });

        auto(title, othis);
        
        $(window).on('resize', function(){
          auto(title, othis);
        });
        
        typeof callback === 'funciton' && callback(index);
      });
      
    }

    //固定块
    ,fixBlock: function(options){  
      options = options || {};
      var tools = {
        ask: '&#xe606;'
        ,help: '&#xe607;'
      }, html = $('<ul class="layui-fixBlock">' 
        +'<li class="layui-icon layui-fixBlock-tool" style="'+ (options.bgcolor ? 'background-color:'+options.bgcolor : '') +'">'+ tools[options.toolType||'ask'] +'</li>'
        +'<li class="layui-icon layui-fixBlock-top" style="'+ (options.bgcolor ? 'background-color:'+options.bgcolor : '') +'">&#xe604;</li>'
      +'</ul>');
      
      $('.layui-fixBlock')[0] || $('body').append(html);
      
      //事件
      html.find('.layui-fixBlock-tool').on('click', options.toolClick);
      html.find('.layui-fixBlock-top').on('click', function(){
        $('html, body').animate({scrollTop: 0}, 200);
      });
      
      //滚动
      var log = 0, topbar = $('.layui-fixBlock-top'), scroll = function(){
        var stop = $(window).scrollTop();
        stop >= (options.showHeight || 200) ? (
          log || (topbar.show(), log = 1)
        ) : (
          log && (topbar.hide(), log = 0)
        );
        return arguments.callee;
      }();
      $(window).on('scroll', scroll);
    }
    
  })
});