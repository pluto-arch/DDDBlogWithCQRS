using System;
using System.Collections.Generic;
using System.Text;
using D3.Blog.Domain.Commands.Customer;

namespace D3.Blog.Domain.Validations.CustomerValidations
{
    /// <summary>
    /// 更新命令验证
    /// </summary>
    public class UpdateCustomerCommandValidation
        : CustomerValidation<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidation ()
        {
            ValidateName();
            ValidateEmail();
            ValidateBirthDate();
        }
    }
}
