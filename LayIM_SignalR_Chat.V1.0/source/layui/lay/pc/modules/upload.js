/*!

 @Title: layui.upload 单文件上传 - 全浏览器兼容版
 @Author: 贤心
 @License：LGPL

 */
 
layui.define(['jquery', 'layer'], function(exports){
  var $ = layui.jquery, layer = layui.layer;
  
  exports('upload', function(options){
    options = options || {};
    
    var body = $('body'), IE = navigator.userAgent.toLowerCase().match(/msie\s(\d+)/) || [];
    var file = $(options.file || '.layui-upload-file');
    var iframe = $('<iframe id="layui-upload-iframe" class="layui-upload-iframe" name="layui-upload-iframe"></iframe>');
    
    options.check = options.check || 'images';
    $('#layui-upload-iframe')[0] || body.append(iframe);
    
    file.each(function(){
      var item = $(this), form = '<form target="layui-upload-iframe" method="'+ (options.method||'post') +'" key="set-mine" enctype="multipart/form-data" action="'+ (options.url||'') +'"></form>';
      
      if(!options.unwrap){
        form = '<div class="layui-upload-button">' + form + '<span class="layui-upload-icon"><i class="layui-icon">&#xe608;</i>'+ (item.attr('layui-text')||'上传图片') +'</span></div>';
      }
      
      form = $(form);
      
      if(item.parent('form').attr('target') === 'layui-upload-iframe'){
        if(options.unwrap){
          item.unwrap();
        } else {
          item.parent().next().remove();
          item.unwrap().unwrap();
        }
      };

      item.wrap(form);

      item.off('change').on('change', function () {
        var othis = $(this), val = othis.val();
        var iframe = $('#layui-upload-iframe');
        if(!val) return;
        if(options.check === 'images' && !/\w\.(jpg|png|gif|bmp|jpeg)$/.test(escape(val))){
          layer.msg('图片格式不对');
          return othis.val('');
        }
        item.parent().submit();
        options.before && options.before();

        var timer = setInterval(function() {
          try{
            var parames = iframe.contents().find('body').text();
          } catch(e) {
            layer.msg('上传接口存在跨域');
            clearInterval(timer);
          }
          if(parames){
            clearInterval(timer);
            typeof options.success === 'function' && options.success(parames);
            iframe.contents().find('body').html('');
          }
        }, 30);
        othis.val('');
      });
    });
  });
});