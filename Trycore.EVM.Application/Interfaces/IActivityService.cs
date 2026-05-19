using Trycore.EVM.Application.DTOs;

namespace Trycore.EVM.Application.Interfaces;

public interface IActivityService
{
    Task<ActivityResponseDto> CreateAsync(Guid projectId, CreateActivityDto dto);

    Task<List<ActivityResponseDto>> GetByProjectIdAsync(Guid projectId);

    Task<ActivityResponseDto?> UpdateAsync(Guid projectId, Guid activityId, UpdateActivityDto dto);

    Task<bool> DeleteAsync(Guid projectId, Guid activityId);
}
