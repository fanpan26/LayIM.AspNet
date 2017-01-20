
var api = require("../../utils/api.js");
var pageIndex = 1;
var helper = {
    load: function (THIS) {
        if (pageIndex > 1) {
            THIS.setData({
                isloadMore: true
            });
        }
        //加载数据
        api.loadExper(pageIndex, 1, '', function (res) {
            if (res.result && res.result == 10000) {
                THIS.setData({
                    list: result.data.list
                });
            }
        }, function (res) {
            //先写在failed里面
            var result = [{
                Name: '张斌',
                Title: '唐家岭志愿者招募工作展开',
                platformname: '唐家岭志愿服务队',
                Id: 1484791400860,
                ShowTime: '2017-1-19',
                imgs: 'http://img1.gurucv.com/image/2017/1/19/cee5008fe33a40bf9b7c30e122ab63c6.jpg,http://img1.gurucv.com/image/2017/1/19/20ece0987a7a4b55811cde933d4aa9f5.jpg,http://img1.gurucv.com/image/2017/1/19/8ce1cd49a312407d8007bc7b0d62c019.jpg,http://img1.gurucv.com/image/2017/1/19/40ec6e9f80674a3ab48bfb175b6b6212.jpg,http://img1.gurucv.com/image/2017/1/19/103e1eb6a0e042d3a5d909c60f49d937.jpg'
            }, {
                Name: '张斌',
                Title: '唐家岭志愿者招募工作展开',
                platformname: '唐家岭志愿服务队',
                Id: 1484791400860,
                ShowTime: '2017-1-19',
                imgs: 'http://img1.gurucv.com/image/2017/1/19/cee5008fe33a40bf9b7c30e122ab63c6.jpg,http://img1.gurucv.com/image/2017/1/19/20ece0987a7a4b55811cde933d4aa9f5.jpg,http://img1.gurucv.com/image/2017/1/19/8ce1cd49a312407d8007bc7b0d62c019.jpg,http://img1.gurucv.com/image/2017/1/19/40ec6e9f80674a3ab48bfb175b6b6212.jpg,http://img1.gurucv.com/image/2017/1/19/103e1eb6a0e042d3a5d909c60f49d937.jpg'
            }, {
                Name: '张斌',
                Title: '唐家岭志愿者招募工作展开',
                platformname: '唐家岭志愿服务队',
                Id: 1484791400860,
                ShowTime: '2017-1-19'
            }];
            helper.handleResult(result, THIS);
        })
    }, handleResult(result, THIS) {
        var list = [];
        list = result.map(function (obj) {
            return {
                name: obj.Name,
                title: obj.Title,
                time: obj.ShowTime,
                imgs: obj.imgs && obj.imgs.split(','),
                id: obj.Id,
                plat: obj.platformname
            };
        });
        //如果是第一页，直接赋值
        if (pageIndex == 1) {
            THIS.setData({
                isloadMore: false,
                list: list
            });
        } else {
            var oldList = THIS.data.list;
            list.map(function (obj) {
                oldList.push(obj);
            });
            THIS.setData({
                isloadMore: false,
                list: oldList
            });
        }
    }
};
Page({
    data: {
        windowHeight: 660,
        list: [],
        //是否正在加载
        isloadMore: false
    },
    onLoad: function () {
        var THIS = this;
        wx.getSystemInfo({
            success: function (res) {
                THIS.setData({
                    windowHeight: res.windowHeight
                })
            }
        });
        //加载记忆列表
        helper.load(THIS);
    },
    experScroll: function (e) {
        console.log(e)
    },
    //滚动加载
    upper: function () {
        console.log('upper');
    },
    lower: function () {
        var THIS = this;
        pageIndex++;
        //加载记忆列表
        helper.load(THIS);
    }
});