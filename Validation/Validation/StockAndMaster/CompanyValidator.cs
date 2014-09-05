using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class CompanyValidator : ICompanyValidator
    {

        public Company VHasUniqueName(Company company, ICompanyService _companyService)
        {
            if (String.IsNullOrEmpty(company.Name) || company.Name.Trim() == "")
            {
                company.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_companyService.IsNameDuplicated(company))
            {
                company.Errors.Add("Name", "Tidak boleh ada duplikasi");
            }
            return company;
        }

        public Company VHasAddress(Company company)
        {
            if (String.IsNullOrEmpty(company.Address) || company.Address.Trim() == "")
            {
                company.Errors.Add("Address", "Tidak boleh kosong");
            }
            return company;
        }

        public Company VHasContactNo(Company company)
        {
            if (String.IsNullOrEmpty(company.ContactNo) || company.ContactNo.Trim() == "")
            {
                company.Errors.Add("ContactNo", "Tidak boleh kosong");
            }
            return company;
        }

        /*public Company VHasLogo(Company company)
        {
            if (String.IsNullOrEmpty(company.Logo) || company.Logo.Trim() == "")
            {
                company.Errors.Add("Logo", "Tidak boleh kosong");
            }
            return company;
        }*/

        public Company VHasEmail(Company company)
        {
            if (String.IsNullOrEmpty(company.Email) || company.Email.Trim() == "")
            {
                company.Errors.Add("Email", "Tidak boleh kosong");
            }
            return company;
        }

        public Company VCreateObject(Company company, ICompanyService _companyService)
        {
            VHasUniqueName(company, _companyService);
            if (!isValid(company)) { return company; }
            VHasAddress(company);
            if (!isValid(company)) { return company; }
            VHasContactNo(company);
            //if (!isValid(company)) { return company; }
            //VHasLogo(company);
            if (!isValid(company)) { return company; }
            VHasEmail(company);
            return company;
        }

        public Company VUpdateObject(Company company, ICompanyService _companyService)
        {
            VCreateObject(company, _companyService);
            return company;
        }

        public Company VDeleteObject(Company company)
        {
            return company;
        }

        public bool ValidCreateObject(Company company, ICompanyService _companyService)
        {
            VCreateObject(company, _companyService);
            return isValid(company);
        }

        public bool ValidUpdateObject(Company company, ICompanyService _companyService)
        {
            company.Errors.Clear();
            VUpdateObject(company, _companyService);
            return isValid(company);
        }

        public bool ValidDeleteObject(Company company)
        {
            company.Errors.Clear();
            VDeleteObject(company);
            return isValid(company);
        }

        public bool isValid(Company obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Company obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
