using Moq;
using PoZiomkaDomain.Form.Commands.GetForm;
using PoZiomkaDomain.Form.Dtos;
using PoZiomkaDomain.Form;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Form.Exceptions;
using PoZiomkaDomain.Form.Commands.DeleteForm;
using PoZiomkaDomain.Form.Commands.GetAllForms;

namespace PoZiomkaUnitTest.Domain.Form;

public class DeleteFormCommandHandlerTest
{
    [Fact]
    public async Task OnIdNotFound_ThrowsException()
    {
        var formId = 1;
        var formRepository = new Mock<IFormRepository>();
        formRepository.Setup(repo => repo.DeleteForm(formId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new IdNotFoundException());
        var handler = new DeleteFormCommandHandler(formRepository.Object);
        var command = new DeleteFormCommand(formId);

        await Assert.ThrowsAsync<FormNotFoundException>(() => handler.Handle(command, new CancellationToken()));
    }

    [Fact]
    public async Task DeletesForm()
    {
        var formId = 1;
        var formRepository = new Mock<IFormRepository>();
        var handler = new DeleteFormCommandHandler(formRepository.Object);
        var command = new DeleteFormCommand(formId);
        await handler.Handle(command, new CancellationToken());
        formRepository.Verify(x => x.DeleteForm(formId, It.IsAny<CancellationToken>()), Times.Once);
    }
}