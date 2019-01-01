using D3.Blog.Domain.Entitys;

namespace D3.Blog.Domain.Infrastructure.IRepositorys
{
    public interface ICustomerRepository: IRepository<Customer>
    {
        Customer GetByEmail(string email);
    }
}
