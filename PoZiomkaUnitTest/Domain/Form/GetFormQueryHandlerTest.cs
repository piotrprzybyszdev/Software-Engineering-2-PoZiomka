using Moq;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Form;
using PoZiomkaDomain.Form.Commands.GetForm;
using PoZiomkaDomain.Form.Dtos;
using PoZiomkaDomain.Form.Exceptions;

namespace PoZiomkaUnitTest.Domain.Form;

public class GetFormQueryHandlerTest
{
    [Fact]
    public async Task ShouldReturnForm()
    {
        var formId = 1;
        var form = new FormDisplay(formId, "Test Form", []);
        var formRepository = new Mock<IFormRepository>();
        formRepository.Setup(repo => repo.GetFormDisplay(formId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(form);
        var handler = new GetFormQueryHandler(formRepository.Object);
        var query = new GetFormQuery(formId);
        var result = await handler.Handle(query, new CancellationToken());
        formRepository.Verify(x => x.GetFormDisplay(formId, It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(formId, result.Id);
        Assert.Equal("Test Form", result.Title);
    }

    [Fact]
    public async Task OnIdNotFound_ThrowsException()
    {
        var formId = 1;
        var form = new FormDisplay(formId, "Test Form", []);
        var formRepository = new Mock<IFormRepository>();
        formRepository.Setup(repo => repo.GetFormDisplay(formId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new IdNotFoundException());
        var handler = new GetFormQueryHandler(formRepository.Object);
        var query = new GetFormQuery(formId);

        await Assert.ThrowsAsync<FormNotFoundException>(() => handler.Handle(query, new CancellationToken()));
    }
}
