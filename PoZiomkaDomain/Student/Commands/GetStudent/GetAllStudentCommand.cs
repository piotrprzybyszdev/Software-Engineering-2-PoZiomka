using MediatR;
using PoZiomkaDomain.Student.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaDomain.Student.Commands.GetStudent;

public record GetAllStudentsCommand() : IRequest<IEnumerable<StudentDisplay>>;