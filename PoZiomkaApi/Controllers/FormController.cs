using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.Form;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Form.Dtos;

namespace PoZiomkaApi.Controllers;

[Route("/form")]
[ApiController]
public class FormController(IMediator mediator) : ControllerBase
{
    [HttpGet("get")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<IEnumerable<FormModel>> Get()
    {
        return [new FormModel(1, "Podstawowa ankieta dotycząca współlokatora")];
    }

    [HttpGet("get-content/{id}")]
    [Authorize(Roles = $"{Roles.Student},{Roles.Administrator}")]
    public async Task<FormDisplay> GetContent(int id)
    {
        return new FormDisplay(id, "Podstawowa ankieta dotycząca współlokatora", [
            new(1, "Czy wolisz dzielić pokój z osobą tej samej płci?", [
                new(1, "Tak"), new(2, "Nie"), new(3, "Nie mam preferencji")
            ]), new(2, "Jakie cechy są dla Ciebie najważniejsze u współlokatora?", [
                new(4, "Czystość i porządek"), new(5, "Towarzyskość"), new(6, "Cichy tryb życia"),
                new(7, "Wspólne zainteresowania"), new(8, "Brak nałogów (np. palenie, alkohol)")
            ]), new(3, "Jakie godziny aktywności Ci odpowiadają?", [
                new(9, "Poranne (6:00–10:00)"), new(10, "Dziennie (10:00–18:00)"),
                new(11, "Wieczorne (18:00–24:00)"), new(12, "Nocne (24:00–6:00)")
            ]), new(4, "Czy ważne jest dla Ciebie, aby współlokator był studentem tego samego kierunku?", [
                new(13, "Tak"), new(14, "Nie"), new(15, "Nie mam preferencji")
            ]), new(5, "Czy współlokator powinien przestrzegać ciszy nocnej?", [
                new(16, "Zdecydowanie tak"), new(16, "Raczej tak"), new(17, "Raczej nie"), new(18, "Zdecydowanie nie")
            ])
        ]);
    }

    [HttpPost("create")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Create([FromBody] FormCreateRequest createRequest)
    {
        return NotFound();
    }

    [HttpPut("update")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update([FromBody] FormUpdateRequest updateRequest)
    {
        return NotFound();
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        return NotFound();
    }
}
