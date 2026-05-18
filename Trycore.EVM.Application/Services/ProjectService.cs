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

        public ProjectService(IProjectRepository repository)
        {
            _repository = repository;
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
    }
}
