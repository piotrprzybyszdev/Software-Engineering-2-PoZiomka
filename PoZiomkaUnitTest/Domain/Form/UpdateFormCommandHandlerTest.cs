using Moq;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Form;
using PoZiomkaDomain.Form.Commands.DeleteForm;
using PoZiomkaDomain.Form.Commands.UpdateForm;
using PoZiomkaDomain.Form.Dtos;
using PoZiomkaDomain.Form.Exceptions;

namespace PoZiomkaUnitTest.Domain.Form;

public class UpdateFormCommandHandlerTest
{
    [Fact]
    public async Task OnIdNotFound_ThrowsException()
    {
        var formRepository = new Mock<IFormRepository>();
        formRepository.Setup(repo => repo.UpdateForm(It.IsAny<FormUpdate>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new IdNotFoundException());
        var handler = new UpdateFormCommandHandler(formRepository.Object);
        var command = new UpdateFormCommand(1, "Form", []);

        await Assert.ThrowsAsync<FormNotFoundException>(() => handler.Handle(command, new CancellationToken()));
    }

    [Fact]
    public async Task UpdatesForm()
    {
        var formRepository = new Mock<IFormRepository>();
        var handler = new UpdateFormCommandHandler(formRepository.Object);
        var command = new UpdateFormCommand(1, "Form", []);
        await handler.Handle(command, new CancellationToken());
        formRepository.Verify(x => x.UpdateForm(It.Is<FormUpdate>(x =>
            x.Id == command.Id && x.Title == command.Title && x.ObligatoryPreferences.Equals(command.ObligatoryPreferences)
        ), It.IsAny<CancellationToken>()), Times.Once);
    }
}
