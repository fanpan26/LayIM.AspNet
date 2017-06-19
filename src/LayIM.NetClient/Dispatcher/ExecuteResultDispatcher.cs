using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace LayIM.NetClient
{
    /*
     *查询命令，非GET方法不能访问
     * 
     */
    internal class ExecuteResultDispatcher : ILayimDispatcher
    {
        private readonly Func<LayimContext, Task<object>> _command;
        /// <summary>
        /// 命令只准使用GET执行
        /// </summary>
        private readonly string _commandMethod;

        public ExecuteResultDispatcher(Func<LayimContext, Task<object>> command, string commandMethod = "GET")
        {
            _command = command;
            _commandMethod = commandMethod;
        }
        public async Task Dispatch(LayimContext context)
        {
            var request = context.Request;
            var response = context.Response;

            if (!_commandMethod.Equals(request.Method, StringComparison.OrdinalIgnoreCase))
            {
                response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                await Task.FromResult(false);
            }

            context.Response.ContentType = "application/json;charset=utf-8";

            var result = await _command(context);

            var json = context.Options.Serializer.SerializeObject(result);

            await context.Response.WriteAsync(json);
        }
    }
}
