using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using D3.Blog.Domain.Entitys;
using D3.Blog.Domain.Infrastructure.IRepositorys;
using Infrastructure.Data.Database;
using Infrastructure.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository.Repositorys
{
    public class CustomerRepository: Repository<Customer>, ICustomerRepository
    {

        internal   D3BlogDbContext _context;
        internal DbSet<Customer>       _dbSet;

        public CustomerRepository(D3BlogDbContext context)
        {
            _context = context;
            _dbSet   = context.Set<Customer>();
        }

        public override void Delete(Customer entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Expression<Func<Customer, bool>> where)
        {
            throw new NotImplementedException();
        }

        public override void DeleteById(object id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteByIds(object[] ids)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override IQueryable<Customer> FindAll()
        {
            throw new NotImplementedException();
        }

        public override Customer FindByClause(Expression<Func<Customer, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public override Task<Customer> FindByIdAsync(object pkValue)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<Customer> FindListByClause<TKey>(Expression<Func<Customer, bool>> predicate, Expression<Func<Customer, TKey>> orderby)
        {
            throw new NotImplementedException();
        }

        public Customer GetByEmail(string email)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(c => c.Email == email);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void Insert(Customer entity)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override void Update(Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}
