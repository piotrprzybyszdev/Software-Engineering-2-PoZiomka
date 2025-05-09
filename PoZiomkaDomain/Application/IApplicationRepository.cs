﻿using PoZiomkaDomain.Application.Dtos;

namespace PoZiomkaDomain.Application;

public interface IApplicationRepository
{
    public Task<IEnumerable<ApplicationTypeModel>> GetTypes();
    public Task<IEnumerable<ApplicationDisplay>> GetAll(string? StudentEmail, string? StudentIndex, int? ApplicationTypeId, ApplicationStatus? ApplicationStatus);
    public Task<IEnumerable<ApplicationDisplay>> GetAllStudentsApplications(int StudentId);
    public Task Submit(int StudentId, int ApplicationTypeId, Guid FileGuid, ApplicationStatus status);
    public Task Update(int applicationId, ApplicationStatus status);
    public Task<ApplicationModel?> Get(int applicationId);
}
