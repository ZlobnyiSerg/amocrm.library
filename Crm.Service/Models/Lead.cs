﻿using amocrm.library.Interfaces;
using amocrm.library.Models.Fields;
using System;
using System.Collections.Generic;

namespace amocrm.library.Models
{
    public class Lead : EntityCore, IValidate<Lead>
    {
        public string Name { get; set; } = String.Empty;

        public int Status { get; set; }

        public int Price { get; set; }

        public int LossReason { get; set; }

        public DateTime ClosestTaskAt { get; set; } = default;

        public bool? IsDeleted { get; set; }

        public DateTime ClosedAt { get; set; } = default;

        public int PipelineId { get; set; }

        public List<SimpleObject> Tags { get; set; } = new List<SimpleObject>();

        public List<Field> Fields { get; set; } = new List<Field>();

        public SimpleObject Company { get; set; }

        public List<int> Contacts { get; set; } = new List<int>();

        public int MainContact { get; set; }

        public int Pipeline { get; set; }

        public bool Validate(IValidateRules<Lead> validateRules)
        {
            return validateRules.Validate(this);
        }
    }
}