using System;
using D3.Blog.Domain.Validations.CustomerValidations;

namespace D3.Blog.Domain.Commands.Customer
{
    public class RegisterNewCustomerCommand : CustomerCommand
    {
        public RegisterNewCustomerCommand(string name, string email, DateTime birthDate)
        {
            Name      = name;
            Email     = email;
            BirthDate = birthDate;
        }

        /// <summary>
        /// 是否通过验证
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            ValidationResult = new RegisterNewCustomerCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
