using System;
using System.Collections.Generic;
using System.Text;
using Trycore.EVM.Application.DTOs;

namespace Trycore.EVM.Application.Interfaces
{
    public interface IActivityService
    {
        Task<ActivityResponseDto> CreateAsync(Guid projectId, CreateActivityDto dto);

        Task<List<ActivityResponseDto>> GetByProjectIdAsync(Guid projectId);
    }
}
