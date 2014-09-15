using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICompanyService
    {
        IQueryable<Company> GetQueryable();
        ICompanyValidator GetValidator();
        IList<Company> GetAll();
        Company GetObjectById(int Id);
        Company GetObjectByName(string Name);
        //Company FindOrCreateBaseCompany();
        Company CreateObject(Company company);
        Company CreateObject(string Name, string Address, string ContactNo, string Logo, string Email);
        Company UpdateObject(Company company);
        Company SoftDeleteObject(Company company);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(Company company);
    }
}