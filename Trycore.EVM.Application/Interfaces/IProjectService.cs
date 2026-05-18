using System;
using System.Collections.Generic;
using System.Text;
using Trycore.EVM.Application.DTOs;

namespace Trycore.EVM.Application.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto);

        Task<List<ProjectResponseDto>> GetAllAsync();

        Task<ProjectSummaryDto?> GetSummaryAsync(Guid projectId);
    }
}
