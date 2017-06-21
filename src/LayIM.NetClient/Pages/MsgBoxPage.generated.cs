using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{

    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    public partial class MsgBoxPage : RazorPage
    {
        public override void Execute()
        {
            WriteLiteral("<html>\r\n");
            WriteLiteral("<head>\r\n");
            WriteLiteral("<meta charset=\"utf-8\">\r\n");
            WriteLiteral("<meta name=\"viewport\" content =\"width =device-width, initial-scale = 1, maximum-scale = 1\">\r\n");
            WriteLiteral("<title>消息盒子</title>\r\n");
            WriteLiteral("<link href=\"/Scripts/layui/css/layui.css\" rel=\"stylesheet\" />\r\n");
            WriteLiteral("<link href=\"/css/apply.css\" rel=\"stylesheet\" />\r\n");
            WriteLiteral("</head>\r\n");
            WriteLiteral("<body>\r\n");
            WriteLiteral("<ul class=\"layim-msgbox\" id=\"LAY_view\">\r\n");

            _userId = Query("uid").ToInt64();
            var list = Storage.GetConnection().GetPageApplyList(_userId);

            foreach (var item in list)
            {
                WriteLiteral($"<li data-id=\"{item.id}\" data-uid=\"{item.uid}\" data-name=\"{item.name}\" data-avatar=\"{item.avatar}\" data-sign=\"{item.sign}\" data-type=\"{item.type}\">\r\n");
                WriteLiteral($"<a href=\"#\"><img src=\"{item.avatar}\" class=\"layui-circle layim-msgbox-avatar\"></a>\r\n");
                WriteLiteral($"<p class=\"layim-msgbox-user\"><a href=\"#\">{item.name}</a><span>{item.addtime}</span></p>\r\n");
                WriteLiteral("<p class=\"layim-msgbox-content\">\r\n");
                Write(item.msg);
                if (item.other?.Length > 0 && item.self)
                {
                    WriteLiteral($"<span>附言: {item.other}</span></p>\r\n");
                }
                if (item.self)
                {
                    if (item.result == 0)
                    {

                        WriteLiteral("<p class=\"layim-msgbox-btn\">\r\n");
                        WriteLiteral("<button class=\"layui-btn layui-btn-small\" data-type=\"agree\">同意</button>\r\n");
                        WriteLiteral("<button class=\"layui-btn layui-btn-small layui-btn-primary\" data-type=\"refuse\">拒绝</button>\r\n");
                        WriteLiteral("</p>\r\n");
                    }
                    else if (item.result == 1)
                    {
                        WriteLiteral("<p class=\"layim-msgbox-btn\">\r\n");
                        WriteLiteral("<em>已同意</em>\r\n");
                        WriteLiteral("</p>\r\n");
                    }
                    else
                    {
                        WriteLiteral("<p class=\"layim-msgbox-btn\">\r\n");
                        WriteLiteral("<em>已拒绝</em>\r\n");
                        WriteLiteral("</p>\r\n");
                    }
                }
                WriteLiteral("</li>\r\n");
            }
            WriteLiteral("</ul>\r\n");
            WriteLiteral("<script src=\"/Scripts/layui/layui.js\"></script>\r\n");
            WriteLiteral("<script src=\"/layim/apply.js\"></script>\r\n");
            WriteLiteral("</body>\r\n");
            WriteLiteral("</html>\r\n");

        }
    }
}
#pragma warning restore 1591

