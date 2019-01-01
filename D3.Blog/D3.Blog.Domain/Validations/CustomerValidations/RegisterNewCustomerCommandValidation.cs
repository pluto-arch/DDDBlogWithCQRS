using D3.Blog.Domain.Commands.Customer;

namespace D3.Blog.Domain.Validations.CustomerValidations
{
    public class RegisterNewCustomerCommandValidation: CustomerValidation<RegisterNewCustomerCommand>
    {
        public RegisterNewCustomerCommandValidation()
        {
            ValidateName();
            ValidateBirthDate();
            ValidateEmail();
        }
    }
}
