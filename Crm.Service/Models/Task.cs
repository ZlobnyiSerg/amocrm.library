﻿using amocrm.library.Configurations;
using amocrm.library.Interfaces;
using System;

namespace amocrm.library.Models
{
    public class Task : EntityCore, IValidate<Task>
    {
        public int Duration { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime CompleteTillAt { get; set; } = default;
        public int TaskType { get; set; }
        public string Text { get; set; } = string.Empty;
        public int ElementId { get; set; }
        public ElementTypeEnum ElementType { get; set; }
        public Result Result { get; set; }

        public bool Validate(IValidateRules<Task> validateRules)
        {
            return validateRules.ValidateBool(this);
        }
    }
}