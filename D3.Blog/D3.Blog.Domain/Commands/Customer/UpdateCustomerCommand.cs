using System;
using System.Collections.Generic;
using System.Text;
using D3.Blog.Domain.Validations.CustomerValidations;

namespace D3.Blog.Domain.Commands.Customer
{
    /// <summary>
    /// 更新命令
    /// </summary>
    public class UpdateCustomerCommand:CustomerCommand
    {
        public UpdateCustomerCommand (int id,string name,string email,DateTime birthDate)
        {
            Id        = id;
            Name      = name;
            Email     = email;
            BirthDate = birthDate;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateCustomerCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
