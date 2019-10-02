﻿using amocrm.library.DTO;
using amocrm.library.Extensions;
using amocrm.library.Mappings;
using amocrm.library.Models;
using amocrm.library.Models.Fields;
using Crm.Tests.Data;
using Mapster;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Crm.Tests.Mappings
{
    [TestClass]
    public class CompanyToCompanyUpdateDtoTest
    {
        Company array;

        public CompanyToCompanyUpdateDtoTest()
        {
            new InitMappings();

            array = CompanyMockData.GetCompany().First();
        }

        [TestMethod]
        public void TheSameResultFromAdaptedAndNew()
        {
            //Statement - All incoming data become defaul value even if data is null. 
            //    Equality ensures that all properties have been converted correctly.


            var dtoFromCompany = new Company().Adapt<CompanyUpdateDTO>();
            var dtoFromNew = new CompanyUpdateDTO();

            var leadDto1 = JsonConvert.SerializeObject(dtoFromCompany).ToString();
            var leadDto2 = JsonConvert.SerializeObject(dtoFromNew).ToString();

            Assert.AreEqual(leadDto1, leadDto2);
        }


        [TestMethod] public void Id() => Assert.AreEqual(array.Adapt<CompanyUpdateDTO>().Id, 8663699);

        [TestMethod] public void Name() => Assert.AreEqual(array.Adapt<CompanyUpdateDTO>().Name, "Company Name");
        [TestMethod] public void NameIsNotNull() => Assert.IsNull(new Company().Adapt<CompanyUpdateDTO>().Name);

        [TestMethod] public void CreatedAt() => Assert.AreEqual(array.Adapt<CompanyUpdateDTO>().CreatedAt, 1527690442);
        [TestMethod] public void CreatedAtZero() => Assert.AreEqual(new Company().Adapt<CompanyUpdateDTO>().CreatedAt, 0);

        [TestMethod] public void UpdatedAt() => Assert.AreEqual(array.Adapt<CompanyUpdateDTO>().UpdatedAt, 1567519347);
        [TestMethod] public void UpdatedAtZero() => Assert.AreEqual(new Company().Adapt<CompanyUpdateDTO>().UpdatedAt, 0);

        [TestMethod] public void CreatedBy() => Assert.AreEqual(array.Adapt<CompanyUpdateDTO>().CreatedBy, 2081797);

        [TestMethod] public void ResponsibleUserId() => Assert.AreEqual(array.Adapt<CompanyUpdateDTO>().ResponsibleUserId, 2997712);

        [TestMethod] public void TagsIsNotNull() => Assert.AreNotEqual(array.Adapt<CompanyUpdateDTO>().Tags, null);
        [TestMethod] public void TagsHasValue() => Assert.AreEqual(array.Adapt<CompanyUpdateDTO>().Tags, "tag1,tag2");
        [TestMethod] public void TagsIsNull() => Assert.IsNull(new Company().Adapt<CompanyUpdateDTO>().Tags);

        [TestMethod] public void ContactsTypeIs() => Assert.IsInstanceOfType(array.Adapt<CompanyAddDTO>().Contacts, typeof(IEnumerable<int>));
        [TestMethod] public void ContactsIsNotNull() => Assert.IsNotNull(array.Adapt<CompanyAddDTO>().Contacts);
        [TestMethod] public void ContactsHasValues() => Assert.AreEqual(array.Adapt<CompanyAddDTO>().Contacts.Count(), 3);
        [TestMethod] public void CompanysFirstValue() => Assert.AreEqual(array.Adapt<CompanyAddDTO>().Contacts.First(), 888);
        [TestMethod] public void ContactsIsNull() => Assert.IsNull(new Company().Adapt<CompanyAddDTO>().Contacts);

        [TestMethod] public void LeadsTypeIs() => Assert.IsInstanceOfType(array.Adapt<CompanyUpdateDTO>().Leads, typeof(IEnumerable<int>));
        [TestMethod] public void LeadsIsNotNull() => Assert.IsNotNull(array.Adapt<CompanyUpdateDTO>().Leads);
        [TestMethod] public void LeadsHasValues() => Assert.AreEqual(array.Adapt<CompanyUpdateDTO>().Leads.Count(), 3);
        [TestMethod] public void LeadsFirstValue() => Assert.AreEqual(array.Adapt<CompanyUpdateDTO>().Leads.First(), 111);
        [TestMethod] public void LeadsIsNull() => Assert.IsNull(new Company().Adapt<CompanyUpdateDTO>().Leads);

        [TestMethod] public void CustomersTypeIs() => Assert.IsInstanceOfType(array.Adapt<CompanyUpdateDTO>().Customers, typeof(IEnumerable<int>));
        [TestMethod] public void CustomersIsNotNull() => Assert.IsNotNull(array.Adapt<CompanyUpdateDTO>().Customers);
        [TestMethod] public void CustomersHasValues() => Assert.AreEqual(array.Adapt<CompanyUpdateDTO>().Customers.Count(), 3);
        [TestMethod] public void CustomersFirstValue() => Assert.AreEqual(array.Adapt<CompanyUpdateDTO>().Customers.First(), 444);
        [TestMethod] public void CustomersIsNull() => Assert.IsNull(new Company().Adapt<CompanyUpdateDTO>().Customers);

        [TestMethod] public void FieldsIsNotNull() => Assert.AreNotEqual(array.Adapt<CompanyUpdateDTO>().CustomFields, null);
        [TestMethod] public void FieldsHasValues() => Assert.AreEqual(array.Adapt<CompanyUpdateDTO>().CustomFields.Count, 3);
        [TestMethod] public void FieldsFirstValue() => Assert.AreEqual(array.Adapt<CompanyUpdateDTO>().CustomFields[0].Id, 66339);
        [TestMethod] public void FieldsIsNull() => Assert.IsNull(new Company().Adapt<CompanyUpdateDTO>().CustomFields);


        [TestMethod]
        public void FieldsToArrayOfEnumsIfMultiselect()
        {
            var company = new Company();
            company.Fields.Add(new Field
            {
                FieldType = 5,
                Id = 555,
                IsSystem = false,
                Name = "",
                Values = new List<FieldValue>
                {
                 new FieldValue { Enum = 654, Value = "OOO" },
                 new FieldValue { Enum = 345, Value = "GGG" },
                 new FieldValue { Enum = 678, Value = "EEE" }
                }
            });

            Assert.IsInstanceOfType(array.Adapt<CompanyUpdateDTO>().CustomFields[0].Values, typeof(ArrayList));
            Assert.AreEqual(company.Adapt<CompanyUpdateDTO>().CustomFields[0].Values.Count, 3);
            Assert.AreEqual(company.Adapt<CompanyUpdateDTO>().CustomFields[0].Values.ToArray()[0], "654");
        }

        [TestMethod]
        public void FieldsToArrayOfObject()
        {
            var company = new Company();
            company.Fields.Add(new Field
            {
                FieldType = 3,
                Id = 555,
                IsSystem = false,
                Name = "",
                Values = new List<FieldValue>
                {
                 new FieldValue { Enum = 654, Value = "OOO" },
                 new FieldValue { Enum = 345, Value = "GGG" },
                 new FieldValue { Enum = 678, Value = "EEE" }
                }
            });

            Assert.IsInstanceOfType(array.Adapt<CompanyUpdateDTO>().CustomFields[0].Values, typeof(ArrayList));
            Assert.AreEqual(company.Adapt<CompanyUpdateDTO>().CustomFields[0].Values.Count, 3);
            Assert.AreEqual(((FieldValue)company.Adapt<CompanyUpdateDTO>().CustomFields[0].Values.ToArray()[0]).Value, "OOO");
        }
    }
}
