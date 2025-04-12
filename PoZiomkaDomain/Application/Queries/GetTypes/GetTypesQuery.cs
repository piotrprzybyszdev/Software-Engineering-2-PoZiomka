using MediatR;
using PoZiomkaDomain.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaDomain.Application.Queries.GetTypes;
public record GetTypesQuery() : IRequest<IEnumerable<ApplicationTypeModel>>;
