using MediatR;
using PoZiomkaDomain.Form.Dtos;

namespace PoZiomkaDomain.Form.Commands;

public record CreateFormCommand(string Title, IEnumerable<ObligatoryPreferenceCreate> ObligatoryPreferences) : IRequest;
