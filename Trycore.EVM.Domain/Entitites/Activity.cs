using System;
using System.Collections.Generic;
using System.Text;

namespace Trycore.EVM.Domain.Entitites
{
    public class Activity
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public string Name { get; set; } = string.Empty;

        // BAC
        public decimal BudgetAtCompletion { get; set; }

        // % planificado
        public decimal PlannedProgressPercent { get; set; }

        // % real
        public decimal ActualProgressPercent { get; set; }

        // AC
        public decimal ActualCost { get; set; }

        public Project Project { get; set; } = null!;
    }
}
