using Trycore.EVM.Application.DTOs;

namespace Trycore.EVM.Application.Interfaces;

public interface IProjectService
{
    Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto);

    Task<List<ProjectResponseDto>> GetAllAsync();

    Task<ProjectResponseDto?> UpdateAsync(Guid id, UpdateProjectDto dto);

    Task<bool> DeleteAsync(Guid id);

    Task<ProjectSummaryDto?> GetSummaryAsync(Guid projectId);
}
