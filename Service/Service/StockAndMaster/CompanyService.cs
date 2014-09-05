using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class CompanyService : ICompanyService
    {
        private ICompanyRepository _repository;
        private ICompanyValidator _validator;
        public CompanyService(ICompanyRepository _companyRepository, ICompanyValidator _companyValidator)
        {
            _repository = _companyRepository;
            _validator = _companyValidator;
        }

        public ICompanyValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<Company> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Company> GetAll()
        {
            return _repository.GetAll();
        }

        public Company GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Company GetObjectByName(string name)
        {
            return _repository.FindAll(c => c.Name == name && !c.IsDeleted).FirstOrDefault();
        }

        public Company CreateObject(string Name, string Address, string ContactNo, string Logo, string Email)
        {
            Company company = new Company
            {
                Name = Name,
                Address = Address,
                ContactNo = ContactNo,
                Logo = Logo,
                Email = Email
            };
            return this.CreateObject(company);
        }

        public Company CreateObject(Company company)
        {
            company.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(company, this) ? _repository.CreateObject(company) : company);
        }

        public Company UpdateObject(Company company)
        {
            return (company = _validator.ValidUpdateObject(company, this) ? _repository.UpdateObject(company) : company);
        }

        public Company SoftDeleteObject(Company company)
        {
            return (company = _validator.ValidDeleteObject(company) ?
                    _repository.SoftDeleteObject(company) : company);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNameDuplicated(Company company)
        {
            IQueryable<Company> companys = _repository.FindAll(x => x.Name == company.Name && !x.IsDeleted && x.Id != company.Id);
            return (companys.Count() > 0 ? true : false);
        }
    }
}