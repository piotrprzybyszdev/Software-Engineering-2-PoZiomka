using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoZiomkaDomain.Application;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;

namespace PoZiomkaDomain.Application.Commands.Submit;
public class SubmitCommandHandler(IApplicationRepository applicationRepository, IFileStorage fileStorage) : IRequestHandler<SubmitCommand>
{
	public async Task Handle(SubmitCommand request, CancellationToken cancellationToken)
	{
		int loggedUserId = request.User.GetUserId() ?? throw new DomainException("Id of the user isn't known");
		
		Guid guid = Guid.NewGuid();
		try
		{
			await fileStorage.UploadFile(guid, request.File);
		}
		catch
		{
			throw new DomainException("Error in uploading file");
		}
		
		try
		{
			await applicationRepository.Submit(loggedUserId, request.Id, guid, Dtos.ApplicationStatus.Pending);
		}
		catch
		{
			throw new SqlException("Error while submitting application");
		}
	}
}
