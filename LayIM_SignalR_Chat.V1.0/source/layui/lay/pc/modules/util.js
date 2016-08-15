/**

 @Name：layui.util 工具集
 @Author：贤心
 @License：LGPL
    
 */
 
layui.define(function(exports){

  //暴露模块
  
  exports('util', {

    //组件事件冒泡
    stope: function(e){
      e = e || window.event;
      e.stopPropagation ? e.stopPropagation() : e.cancelBubble = true;
    }
      
    //在焦点处插入内容
    ,focusInsert: function(obj, str){
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
    }

  }); 
  
});