using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewAssistantAPI.DTOs
{
    public record struct CardAddDto(string Front, string Back, bool Known, int TagId, string Example);
}