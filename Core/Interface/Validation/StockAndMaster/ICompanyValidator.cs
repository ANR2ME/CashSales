using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ICompanyValidator
    {
        Company VHasUniqueName(Company company, ICompanyService _companyService);
        Company VHasAddress(Company company);
        Company VHasContactNo(Company company);
        //Company VHasLogo(Company company);
        Company VHasEmail(Company company);
        Company VCreateObject(Company company, ICompanyService _companyService);
        Company VUpdateObject(Company company, ICompanyService _companyService);
        Company VDeleteObject(Company company);
        bool ValidCreateObject(Company company, ICompanyService _companyService);
        bool ValidUpdateObject(Company company, ICompanyService _companyService);
        bool ValidDeleteObject(Company company);
        bool isValid(Company company);
        string PrintError(Company company);
    }
}