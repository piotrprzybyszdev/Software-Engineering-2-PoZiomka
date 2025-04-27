using Microsoft.SqlServer.Server;
using Moq;
using PoZiomkaDomain.Form;
using PoZiomkaDomain.Form.Commands.GetAllForms;
using PoZiomkaDomain.Form.Dtos;

namespace PoZiomkaUnitTest.Domain.Form;

public class GetAllFormsQueryHandlerTest
{
    [Fact]
    public async Task GetsForms()
    {
        var forms = new List<FormModel>
        {
            new FormModel(1, "Form 1"),
            new FormModel(2, "Form 2")
        };
        var formRepository = new Mock<IFormRepository>();
        var handler = new GetAllFormsQueryHandler(formRepository.Object);
        formRepository.Setup(repo => repo.GetForms(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(forms);
        var query = new GetAllFormsQuery();
        var result = await handler.Handle(query, new CancellationToken());
        formRepository.Verify(x => x.GetForms(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(forms, result);
    }
}
