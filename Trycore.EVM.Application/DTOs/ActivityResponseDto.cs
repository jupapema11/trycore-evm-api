using System;
using System.Collections.Generic;
using System.Text;

namespace Trycore.EVM.Application.DTOs
{
    public class ActivityResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal PV { get; set; }

        public decimal EV { get; set; }

        public decimal CV { get; set; }

        public decimal SV { get; set; }

        public decimal CPI { get; set; }

        public decimal SPI { get; set; }

        public decimal EAC { get; set; }

        public decimal VAC { get; set; }
    }
}
