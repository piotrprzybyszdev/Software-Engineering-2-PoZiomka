using Moq;
using PoZiomkaDomain.Form;
using PoZiomkaDomain.Form.Commands.CreateForm;
using PoZiomkaDomain.Form.Dtos;

namespace PoZiomkaUnitTest.Domain.Form;

public class CreateFormCommandHandlerTest
{
    [Fact]
    public async Task CreatesForm()
    {
        var formRepository = new Mock<IFormRepository>();
        var handler = new CreateFormCommandHandler(formRepository.Object);
        var command = new CreateFormCommand("Form", []);
        await handler.Handle(command, new CancellationToken());
        formRepository.Verify(x => x.CreateForm(It.Is<FormCreate>(x =>
            x.Title == command.Title && x.ObligatoryPreferences.Equals(command.ObligatoryPreferences)
        ), It.IsAny<CancellationToken>()), Times.Once);
    }
}
