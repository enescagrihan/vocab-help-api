using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewAssistantAPI.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
        public bool Known { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        public string? Example { get; set; }
    }
}