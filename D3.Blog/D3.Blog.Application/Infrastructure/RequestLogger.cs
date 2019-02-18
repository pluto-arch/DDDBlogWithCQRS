using System.Threading;
using System.Threading.Tasks;
using D3.Blog.Domain.Infrastructure;
using Infrastructure.Logging;
using MediatR.Pipeline;

namespace D3.Blog.Application.Infrastructure
{
    /// <summary>
    /// 请求预处理器-日志记录
    /// </summary>
    public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
    {
        /// <summary>
        /// 日志对象
        /// </summary>
//        private readonly Serilog.ILogger _logger;
        /// <summary>
        /// 当前操作用户
        /// </summary>
        private IUser _user;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        public RequestLogger(IUser user)
        {
//            _logger = logger;
            _user = user;
        }
        /// <summary>
        /// 预处理请求，记录日志
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var name = typeof(TRequest).Name;

            // TODO: 添加请求日志
//            _logger.CustomInformation(_user.Name,name,informationMessage: $@"{name} Request: {request}");

            return Task.CompletedTask;
        }
    }
}