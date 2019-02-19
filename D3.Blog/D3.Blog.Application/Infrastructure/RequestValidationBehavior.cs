using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using D3.Blog.Domain.Infrastructure;
using FluentValidation;
using Infrastructure.Logging;
using MediatR;
using Newtonsoft.Json;

namespace D3.Blog.Application.Infrastructure
{
    /// <summary>
    /// 请求处理验证
    /// </summary>
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
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
        /// 验证者
        /// </summary>
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="validators"></param>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators,IUser user)
        {
            _validators = validators;
//            _logger = logger;
            _user = user;
        }


        /// <summary>
        /// 验证请求处理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                var name = typeof(TRequest).Name;
                var failure = JsonConvert.SerializeObject(failures);
//                _logger.CustomFatal(_user.Name,fatalMessage:failure);
            }

            return next();
        }
    }
}