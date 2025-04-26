using MediatR;
using PoZiomkaDomain.Form.Dtos;

namespace PoZiomkaDomain.Form.Commands.CreateForm;

public record CreateFormCommand(string Title, IEnumerable<ObligatoryPreferenceCreate> ObligatoryPreferences) : IRequest;
