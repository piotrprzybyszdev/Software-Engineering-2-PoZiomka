using MediatR;
using PoZiomkaDomain.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using PoZiomkaDomain.Exceptions;

namespace PoZiomkaDomain.Application.Queries.GetTypes;
public class GetTypesQueryHandler(IApplicationRepository applicationRepository): 
	IRequestHandler<GetTypesQuery, IEnumerable<ApplicationTypeModel>>
{
	public async Task<IEnumerable<ApplicationTypeModel>> Handle(GetTypesQuery request, CancellationToken cancellationToken)
	{
		try
		{
			return await applicationRepository.GetTypes();
		}
		catch
		{
			throw new SqlException("Error while getting application types");
		}
	}
}


