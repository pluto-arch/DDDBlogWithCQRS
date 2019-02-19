using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using D3.Blog.Domain.Infrastructure;
using Infrastructure.Logging;
using MediatR;

namespace D3.Blog.Application.Infrastructure
{
    /// <summary>
    /// 请求性能记录
    /// </summary>
    public class RequestPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        /// <summary>
        /// serilog对象
        /// </summary>
//        private readonly Serilog.ILogger _logger;
        /// <summary>
        /// 当前操作用户
        /// </summary>
        private IUser _user;

        /// <summary>
        /// 计数器
        /// </summary>
        private readonly Stopwatch _timer;


        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        public RequestPerformanceBehaviour(IUser user)
        {
            _timer=new Stopwatch();
//            _logger = logger;
            _user = user;
        }

        /// <summary>
        /// 请求预处理-性能分析
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            if (_timer.ElapsedMilliseconds > 500)//请求耗时大于500毫秒时，记录，表示响应慢，性能差
            {
                var name = typeof(TRequest).Name;
                
//                _logger.CustomInformation(_user.Name,other: _timer.ElapsedMilliseconds+"",informationMessage:$@"【{_user.Name}】请求【{name}】。耗时【{_timer.ElapsedMilliseconds}】秒");
            }

            return response;
        }
    }
}