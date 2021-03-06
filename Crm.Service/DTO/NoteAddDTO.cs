﻿using amocrm.library.Models;
using amocrm.library.Tools;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace amocrm.library.DTO
{
    [SelectDtoAttribute(typeof(Note), ActionEnum.Add)]
    internal class NoteAddDTO
    {
        [Required]
        [JsonProperty(PropertyName = "element_id")]
        public int ElementId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "element_type")]
        public int ElementType { get; set; }

        [Required]
        [JsonProperty(PropertyName = "note_type")]
        public int NoteType { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public int CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public int UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "created_by")]
        public int CreatedBy { get; set; }

        [JsonProperty(PropertyName = "responsible_user_id")]
        public int ResponsibleUserId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "attachment")]
        public string Attachment { get; set; }
    }
}
