using MediatR;
using PoZiomkaDomain.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaDomain.Application.Queries.GetStudent;
public record GetStudentQuery(ClaimsPrincipal User) : IRequest<IEnumerable<ApplicationDisplay>>;
