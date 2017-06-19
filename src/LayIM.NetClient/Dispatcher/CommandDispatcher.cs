using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    internal class CommandDispatcher : ILayimDispatcher
    {

        private readonly Func<LayimContext, bool> _command;
        /// <summary>
        /// 命令只准使用POST执行
        /// </summary>
        private const string CommandMethod = "POST";

        public CommandDispatcher(Func<LayimContext, bool> command)
        {
            _command = command;
        }

        /// <summary>
        /// 实现方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Dispatch(LayimContext context)
        {
            var request = context.Request;
            var response = context.Response;

            if (!CommandMethod.Equals(request.Method, StringComparison.OrdinalIgnoreCase))
            {
                response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                return Task.FromResult(false);
            }

            //执行命令
            if (_command(context))
            {
                response.StatusCode = (int)HttpStatusCode.OK;
            }
            else {
                response.StatusCode = 422;
            }
            return Task.FromResult(true);
        }
    }
}
