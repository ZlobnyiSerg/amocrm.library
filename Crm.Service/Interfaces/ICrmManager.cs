﻿
using amocrm.library.Models;
using amocrm.library.Models.Account;

namespace amocrm.library.Interfaces
{
    public interface ICrmManager
    {
        System.Threading.Tasks.Task DirectAuthorization();
        IQueryableRepository<Lead> Leads { get; }
        IQueryableRepository<Contact> Contacts { get; }
        IQueryableRepository<Company> Companies { get; }
        IQueryableRepository<Task> Tasks { get; }
        IQueryableRepository<Note> Notes { get; }
        System.Threading.Tasks.Task<CustomFieldInfo> CustomFields { get; }
    }
}
