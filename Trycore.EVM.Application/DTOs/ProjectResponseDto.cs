using System;
using System.Collections.Generic;
using System.Text;

namespace Trycore.EVM.Application.DTOs
{
    public class ProjectResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<ActivityResponseDto> Activities { get; set; } = new();
    }
}
