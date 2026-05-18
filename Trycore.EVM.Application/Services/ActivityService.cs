using System;
using System.Collections.Generic;
using System.Text;
using Trycore.EVM.Application.DTOs;
using Trycore.EVM.Application.Interfaces;
using Trycore.EVM.Domain.Entitites;

namespace Trycore.EVM.Application.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IEvmCalculationService _evmService;

        public ActivityService(
            IActivityRepository activityRepository,
            IEvmCalculationService evmService)
        {
            _activityRepository = activityRepository;
            _evmService = evmService;
        }

        public async Task<ActivityResponseDto> CreateAsync(
            Guid projectId,
            CreateActivityDto dto)
        {
            var activity = new Activity
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                Name = dto.Name,
                BudgetAtCompletion = dto.BudgetAtCompletion,
                PlannedProgressPercent = dto.PlannedProgressPercent,
                ActualProgressPercent = dto.ActualProgressPercent,
                ActualCost = dto.ActualCost
            };

            await _activityRepository.CreateAsync(activity);

            await _activityRepository.SaveChangesAsync();

            return BuildResponse(activity);
        }

        public async Task<List<ActivityResponseDto>> GetByProjectIdAsync(Guid projectId)
        {
            var activities = await _activityRepository.GetByProjectIdAsync(projectId);

            return activities
                .Select(BuildResponse)
                .ToList();
        }

        private ActivityResponseDto BuildResponse(Activity activity)
        {
            var pv = _evmService.CalculatePV(
                activity.PlannedProgressPercent,
                activity.BudgetAtCompletion);

            var ev = _evmService.CalculateEV(
                activity.ActualProgressPercent,
                activity.BudgetAtCompletion);

            var cv = _evmService.CalculateCV(ev, activity.ActualCost);

            var sv = _evmService.CalculateSV(ev, pv);

            var cpi = _evmService.CalculateCPI(ev, activity.ActualCost);

            var spi = _evmService.CalculateSPI(ev, pv);

            var eac = _evmService.CalculateEAC(
                activity.BudgetAtCompletion,
                cpi);

            var vac = _evmService.CalculateVAC(
                activity.BudgetAtCompletion,
                eac);

            return new ActivityResponseDto
            {
                Id = activity.Id,
                Name = activity.Name,
                PV = pv,
                EV = ev,
                CV = cv,
                SV = sv,
                CPI = cpi,
                SPI = spi,
                EAC = eac,
                VAC = vac
            };
        }
    }
}
