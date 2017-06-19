using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    public abstract class RazorPage
    {
        private readonly StringBuilder _content = new StringBuilder();
        private string _body;

        protected RazorPage() {

            Html = new HtmlHelper(this);
        }

        public RazorPage Layout { get; protected set; }

        public UrlHelper Url { get; private set; }

        public HtmlHelper Html { get; set; }

        public LayimStorage Storage { get; internal set; }

        public string AppPath { get; internal set; }

        internal LayimRequest Request { private get; set; }
        internal LayimResponse Response { private get; set; }

        public string RequestPath => Request.Path;

        public abstract void Execute();

        public string Query(string key)
        {
            return Request.GetQuery(key);
        }

        public override string ToString()
        {
            return TransformText(null);
        }

        public void Assign(RazorPage parentPage)
        {
            Request = parentPage.Request;
            Response = parentPage.Response;
            Storage = parentPage.Storage;
            AppPath = parentPage.AppPath;
            Url = parentPage.Url;
        }

        internal void Assign(LayimContext context)
        {
            Request = context.Request;
            Response = context.Response;
            Storage = context.Storage;
            AppPath = context.Options.AppPath;

            Url = new UrlHelper(context);
        }

        protected void WriteLiteral(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend)) return;

            _content.Append(textToAppend);
        }

        protected virtual void Write(object value)
        {
            if (value == null) { return; }

            var html = value;

            WriteLiteral(html?.ToString() ?? Encode(value.ToString()));
        }

        protected virtual object RenderBody()
        {
            return _body;
        }

        private string TransformText(string body)
        {
            _body = body;

            Execute();

            if (Layout != null) {
                Layout.Assign(this);
                return Layout.TransformText(_content.ToString());
            }
            return _content.ToString();
        }

        private static string Encode(string text)
        {
            return string.IsNullOrEmpty(text)
                       ? string.Empty
                       : WebUtility.HtmlEncode(text);
        }
    }
}
