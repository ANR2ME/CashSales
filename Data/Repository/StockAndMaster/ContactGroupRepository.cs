using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;

namespace Data.Repository
{
    public class ContactGroupRepository : EfRepository<ContactGroup>, IContactGroupRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public ContactGroupRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<ContactGroup> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<ContactGroup> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public ContactGroup GetObjectById(int Id)
        {
            ContactGroup contactGroup = Find(x => x.Id == Id && !x.IsDeleted);
            if (contactGroup != null) { contactGroup.Errors = new Dictionary<string, string>(); }
            return contactGroup;
        }

        public ContactGroup GetObjectByIsLegacy(bool IsLegacy)
        {
            ContactGroup contactGroup = FindAll(x => x.IsLegacy == IsLegacy && !x.IsDeleted).FirstOrDefault();
            if (contactGroup != null) { contactGroup.Errors = new Dictionary<string, string>(); }
            return contactGroup;
        }

        public ContactGroup GetObjectByName(string Name)
        {
            ContactGroup contactGroup = FindAll(x => x.Name == Name && !x.IsDeleted).FirstOrDefault();
            if (contactGroup != null) { contactGroup.Errors = new Dictionary<string, string>(); }
            return contactGroup;
        }

        public ContactGroup CreateObject(ContactGroup contactGroup)
        {
            contactGroup.IsDeleted = false;
            contactGroup.CreatedAt = DateTime.Now;
            return Create(contactGroup);
        }

        public ContactGroup UpdateObject(ContactGroup contactGroup)
        {
            contactGroup.UpdatedAt = DateTime.Now;
            Update(contactGroup);
            return contactGroup;
        }

        public ContactGroup SoftDeleteObject(ContactGroup contactGroup)
        {
            contactGroup.IsDeleted = true;
            contactGroup.DeletedAt = DateTime.Now;
            Update(contactGroup);
            return contactGroup;
        }

        public bool DeleteObject(int Id)
        {
            ContactGroup contactGroup = Find(x => x.Id == Id);
            return (Delete(contactGroup) == 1) ? true : false;
        }
    }
}
