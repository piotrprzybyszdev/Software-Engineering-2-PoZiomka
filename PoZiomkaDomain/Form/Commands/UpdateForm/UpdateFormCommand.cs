using MediatR;
using PoZiomkaDomain.Form.Dtos;

namespace PoZiomkaDomain.Form.Commands.UpdateForm;

public record UpdateFormCommand(int Id, string Title, IEnumerable<ObligatoryPreferenceCreate> ObligatoryPreferences) : IRequest;