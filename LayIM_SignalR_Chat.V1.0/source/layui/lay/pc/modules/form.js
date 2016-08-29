/**

 @Name：layui.form 表单组件
 @Author：贤心
 @License：LGPL
    
 */
 
 
layui.define(['jquery', 'layer', 'util'], function(exports){
  
  var $ = layui.jquery, util = layui.util;
  var layer = layui.layer;
  
  var Class = function(){
    var that = this;
    that.box = $('.layui-form');
  };
  
  //验证
  Class.prototype.check = function(options, callback){
    options = options || {};
    var custom = options.custom = options.custom || {};
    var that = this, gather = {
      check: {
        mobile: function(val){
          if(!/^1\d{10}$/g.test(val)){
            return '手机格式不合法';
          };
        }
        ,email: function(val){
          if(!/^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/.test(val)){
            return '邮箱格式不正确';
          };
        }
        ,identity: function(val){
          if(!/(^\d{15}$)|(^\d{17}(x|X|\d)$)/.test(val)){
            return '请输入正确的身份证号';
          };
        }
      }
      ,submit: function(form){
        form = $(form);
        var required = form.find('[required]'), data = {}, go = true;
        
        gather.check = $.extend(gather.check, custom);
        required.each(function(index, item){
          var item = $(item), check = item.attr('check');
          var name = item.attr('name'), value = item.val();
          if(gather.check[check]){
            var tips = gather.check[check](value);
            if(tips){
              go = false;
              item.focus();
              layer.msg(tips, {icon: 5});
              return false;
            }
          }
          if(value.replace(/\s/g, '') !== ''){
            name && (data[name] = value);
          }
        });
        
        if(go){
          typeof callback === 'function' ? callback.call(this, data, form, required) : function(){
            if(form[0].tagName.toLowerCase() === 'form'){
              form.submit();
            }
          }();
        }
      }
    };
    
    options.form = options.form || '.layui-form';
    options.button = options.button || '.layui-form-button';
    
    $(options.form).each(function(){
      var othis = $(this);
      if(this.tagName.toLowerCase() === 'form'){
        othis.submit(function(){
          gather.submit.call(this, othis);
          return false;
        });
      } else {
        othis.on('click', options.button, function(){
          gather.submit.call(this, othis);
          return false;
        });  
      }
    });
    
    
    return that;
  };
  
  //密码强度
  Class.prototype.pass = function(options){
    options = options || {};
    var that = this, gather = {
      show: function(strength, load){
        strength.find('span').css('width', (load||0) + '%');
      }
      ,event: function(othis, strength){
        othis.on('keyup', function(){
          var val = othis.val().replace(/\s/g, '');
          strength.show();
          
          if(val === '') {
            gather.show(strength);
            setTimeout(function(){
              strength.hide();
            }, 300);
          } 
          
          //如果同时包含数字、字母、下划线，且长度大于6，则强度最高
          else if(/^(?=.*[a-zA-Z])(?=.*[0-9])(?=.*\_)[a-zA-Z0-9_]{6,}$/g.test(val)) {
            gather.show(strength, 100);
          } 
          
          //如果数字或字母长度大于6，则强度中
          else if(/[a-z0-9]{6,}/g.test(val)) {
            gather.show(strength, 50);
          } else if(/[\s\S]{3,6}/g.test(val)){ //强度弱
            gather.show(strength, 20);
          }
        });
      }
    }, pass = options.pass || that.box.find('input[type=password]');
    
    pass.each(function(){
      var othis = $(this), parent = othis.parent();
      var strength = $('<div class="layui-form-strength"><span></span></div>');
      othis.after(strength);
      gather.event.call(this, othis, strength);
    });
    
    return that;
  }
  
  //多选框
  Class.prototype.checkbox = function(options){
    options = options || {};
    var that = this, gather = {
      icon: '&#xe605'
      ,checked: function(parent, sure){
        this.checked = true;
        this.value = 'on';
        parent.addClass('layui-form-checked');
        sure.html(gather.icon);
      }
      ,event: function(parent, sure){
        var elem = this;
        parent.on('click', function(){
          elem.checked ? (
            parent.removeClass('layui-form-checked'),
            sure.html(''),
            elem.checked = false,
            elem.value = ''
          ) : gather.checked.call(elem, parent, sure);
        });
      }
    }, checkbox = options.checkbox || that.box.find('input[type=checkbox]');

    checkbox.each(function(){
      var othis = $(this), parent = othis.parent();
      var sure = $('<span class="layui-icon layui-form-sure"></span>');
      parent.addClass('layui-form-checkbox');
      othis.after(sure);
      othis[0].checked ? gather.checked.call(this, parent, sure) : othis[0].value = '';
      gather.event.call(this, parent, sure);
    });
    
    return that;
  };

  //下拉选择框
  Class.prototype.select = function(options){
    options = options || {};
    var that = this, gather = {};
    var selects = options.select || that.box.find('select');
    
    //下拉选择
    gather.selected = function(elem, option, select){
      var title = elem.find('.layui-form-sltitle');
      title.on('click', function(e){
        var ul = gather.ul = $(this).next();
        util.stope(e);
        $('.layui-form-option').hide();
        ul.show();
        $(document).off('click', gather.hide).on('click', gather.hide);
      });
      elem.find('li').on('click', function(){
        var othis = $(this), val = othis.attr('value');
        title.find('span').html(othis.find('a').html());
        select.val(val);
        typeof options.change === 'function' && options.change(val, select);
      });
    };
    
    //点击任意处关闭下拉
    gather.hide = function(){
      gather.ul.hide()
    };
    
    //遍历所有select
    selects.each(function(){
      var othis = $(this), value = othis.val();
      var selected = $(this.options[this.selectedIndex]);
      var option = $(function(){
        var str = '<ul class="layui-form-option">';
        var option = othis.find('option');
        for(var i = 0; i < option.length; i++){
          str += '<li value="'+ option.eq(i).val() +'"><a href="javascript:;">'+ (option.eq(i).html()||option.eq(i).val()) +'</a></li>';
        }
        return str + '</ul>';
      }());
      var str = $('<div class="layui-form-select">' 
             +'<div class="layui-form-sltitle"><span>'+ (selected.html()||value) +'</span><i class="layui-edge"></i></div>'
             +option.prop('outerHTML')
            +'</div>');
      str.addClass(othis.attr('layui-class')||'');
      othis.after(str);
      gather.selected(str, option, othis);
    });
    
    return that;
  };

  exports('form', new Class());

});
