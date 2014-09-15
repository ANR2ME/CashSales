using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class CompanyRepository : EfRepository<Company>, ICompanyRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public CompanyRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<Company> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<Company> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public Company GetObjectById(int Id)
        {
            Company company = Find(x => x.Id == Id && !x.IsDeleted);
            if (company != null) { company.Errors = new Dictionary<string, string>(); }
            return company;
        }

        public Company CreateObject(Company company)
        {
            company.IsDeleted = false;
            company.CreatedAt = DateTime.Now;
            return Create(company);
        }

        public Company UpdateObject(Company company)
        {
            company.UpdatedAt = DateTime.Now;
            Update(company);
            return company;
        }

        public Company SoftDeleteObject(Company company)
        {
            company.IsDeleted = true;
            company.DeletedAt = DateTime.Now;
            Update(company);
            return company;
        }

        public bool DeleteObject(int Id)
        {
            Company company = Find(x => x.Id == Id);
            return (Delete(company) == 1) ? true : false;
        }
    }
}