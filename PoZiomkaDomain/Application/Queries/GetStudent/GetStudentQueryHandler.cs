using MediatR;
using PoZiomkaDomain.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Common;

namespace PoZiomkaDomain.Application.Queries.GetStudent;
public class GetStudentQueryHandler(IApplicationRepository applicationRepository): 
	IRequestHandler<GetStudentQuery, IEnumerable<ApplicationDisplay>>
{
	public async Task<IEnumerable<ApplicationDisplay>> Handle(GetStudentQuery request, CancellationToken cancellationToken)
	{
		int loggedInUserId = request.User.GetUserId() ?? throw new DomainException("Id of the user isn't known");

		var types = await applicationRepository.GetTypes();
		try
		{ 
			return (await applicationRepository.GetAll(loggedInUserId)).Select(x => x.ToDisplay(types.First(y => y.Id == x.ApplicationTypeId)));
		}
		catch
		{
			throw new SqlException("Error while getting application types");
		}
	}
}


