using MediatR;
using PoZiomkaDomain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaDomain.Application.Commands.Resolve;
public class ResolveCommandHandler(IApplicationRepository applicationRepository) : IRequestHandler<ResolveCommand>
{
	public async Task Handle(ResolveCommand request, CancellationToken cancellationToken)
	{
		try
		{
			await applicationRepository.Update(request.Id, request.Status);
		}
		catch
		{
			throw new SqlException("Error while resolving application");
		}
	}
}
