namespace Trycore.EVM.Application.DTOs;

public class ProjectSummaryDto
{
    public Guid ProjectId { get; set; }

    public string ProjectName { get; set; } = string.Empty;

    public decimal TotalPV { get; set; }

    public decimal TotalEV { get; set; }

    public decimal TotalAC { get; set; }

    public decimal TotalBAC { get; set; }

    public decimal TotalCV { get; set; }

    public decimal TotalSV { get; set; }

    public decimal TotalCPI { get; set; }

    public decimal TotalSPI { get; set; }

    public decimal TotalEAC { get; set; }

    public decimal TotalVAC { get; set; }

    public string CpiInterpretation { get; set; } = string.Empty;

    public string SpiInterpretation { get; set; } = string.Empty;
}
