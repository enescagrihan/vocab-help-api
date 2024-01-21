using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewAssistantAPI.DTOs
{
    public record struct TagAddDto() {
        public string TagName { get; set; }
        public List<CardAddDto> Cards { get; set; } 
    }
}