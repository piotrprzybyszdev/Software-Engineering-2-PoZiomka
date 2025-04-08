using PoZiomkaDomain.Application.Dtos;
using System.Collections;

namespace PoZiomkaDomain.Application;

public interface IApplicationRepository
{
	public Task<IEnumerable<ApplicationTypeModel>> GetTypes();
	//public Task<IEnumerable<ApplicationModel>> GetAll(/* Filters here*/);
	//public Task Submit(ApplicationModel model);
	//public Task Update(int applicationId, ApplicationStatus status);
	//public Task<ApplicationModel> Get(int applicationId);
}
