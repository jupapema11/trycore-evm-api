using System;
using System.Collections.Generic;
using System.Text;
using Trycore.EVM.Application.DTOs;
using Trycore.EVM.Application.Interfaces;
using Trycore.EVM.Domain.Entitites;

namespace Trycore.EVM.Application.Services
{
    public class ProjectService : IProjectService
    {

        private readonly IProjectRepository _repository;
        private readonly IEvmCalculationService _evmService;

        public ProjectService(IProjectRepository repository, IEvmCalculationService evmService)
        {
            _repository = repository;
            _evmService = evmService;
        }

        public async Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto)
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = dto.Name
            };

            await _repository.CreateAsync(project);

            await _repository.SaveChangesAsync();

            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name
            };
        }

        public async Task<List<ProjectResponseDto>> GetAllAsync()
        {
            var projects = await _repository.GetAllAsync();

            return projects.Select(project => new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name
            }).ToList();
        }

        public async Task<ProjectSummaryDto?> GetSummaryAsync(Guid projectId)
        {
            var project = await _repository.GetByIdAsync(projectId);

            if (project is null)
                return null;

            decimal totalPV = 0;
            decimal totalEV = 0;
            decimal totalAC = 0;
            decimal totalBAC = 0;

            foreach (var activity in project.Activities)
            {
                var pv = _evmService.CalculatePV(
                    activity.PlannedProgressPercent,
                    activity.BudgetAtCompletion);

                var ev = _evmService.CalculateEV(
                    activity.ActualProgressPercent,
                    activity.BudgetAtCompletion);

                totalPV += pv;
                totalEV += ev;
                totalAC += activity.ActualCost;
                totalBAC += activity.BudgetAtCompletion;
            }

            var totalCV = _evmService.CalculateCV(totalEV, totalAC);

            var totalSV = _evmService.CalculateSV(totalEV, totalPV);

            var totalCPI = _evmService.CalculateCPI(totalEV, totalAC);

            var totalSPI = _evmService.CalculateSPI(totalEV, totalPV);

            var totalEAC = _evmService.CalculateEAC(totalBAC, totalCPI);

            var totalVAC = _evmService.CalculateVAC(totalBAC, totalEAC);

            return new ProjectSummaryDto
            {
                ProjectId = project.Id,
                ProjectName = project.Name,
                TotalPV = totalPV,
                TotalEV = totalEV,
                TotalAC = totalAC,
                TotalCV = totalCV,
                TotalSV = totalSV,
                TotalCPI = totalCPI,
                TotalSPI = totalSPI,
                TotalEAC = totalEAC,
                TotalVAC = totalVAC
            };
        }
    }
}
