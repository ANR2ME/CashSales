using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        IQueryable<Company> GetQueryable();
        IList<Company> GetAll();
        Company GetObjectById(int Id);
        Company CreateObject(Company company);
        Company UpdateObject(Company company);
        Company SoftDeleteObject(Company company);
        bool DeleteObject(int Id);
    }
}