using System;
using System.Collections.Generic;
using System.Text;

namespace Trycore.EVM.Application.DTOs;

public class CreateActivityDto
{
    public string Name { get; set; } = string.Empty;

    public decimal BudgetAtCompletion { get; set; }

    public decimal PlannedProgressPercent { get; set; }

    public decimal ActualProgressPercent { get; set; }

    public decimal ActualCost { get; set; }
}