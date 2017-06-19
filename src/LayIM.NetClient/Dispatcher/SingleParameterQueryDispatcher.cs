using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    internal class SingleParameterQueryDispatcher<T> : ILayimDispatcher
    {
        private readonly string _parameterName;
        private const string CommandMethod = "GET";
        private readonly Func<LayimContext, T, object> _command;
        public SingleParameterQueryDispatcher(string parameterName,Func<LayimContext,T,object> command)
        {
            Error.ThrowIfNull(parameterName, nameof(parameterName));
            Error.ThrowIfNull(command, nameof(command));

            _parameterName = parameterName;
            _command = command;
        }

        public async Task Dispatch(LayimContext context)
        {
            var request = context.Request;
            var response = context.Response;

            var parameterValue = request.GetQuery(_parameterName);
            //如果不是Get请求，返回方法不允许
            if (!CommandMethod.Equals(request.Method, StringComparison.OrdinalIgnoreCase))
            {
                response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                await Task.FromResult(false);
            }
            //返回结果为"application/json";
            context.Response.ContentType = "application/json;charset=utf-8";
            //将参数转化为相应的类型，有 null 异常
            T value;
            if (string.IsNullOrEmpty(parameterValue))
            {
                value = default(T);
            }
            else
            {
                value = (T)Convert.ChangeType(parameterValue, typeof(T));
            }
            //执行具体处理函数
            var result = _command(context, value);
            //序列化结果
            var json = context.Options.Serializer.SerializeObject(result);
            //返回
            await context.Response.WriteAsync(json);
        }
    }
}
