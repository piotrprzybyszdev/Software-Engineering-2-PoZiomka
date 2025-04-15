using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaApi.Requests.StudentAnswer;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.StudentAnswers.Dtos;

namespace PoZiomkaApi.Controllers;

[Route("/answer")]
[ApiController]
public class StudentAnswerController(IMediator mediator) : ControllerBase
{
    [HttpGet("get-student")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IEnumerable<StudentAnswerStatus>> GetStudent()
    {
        return [
            new StudentAnswerStatus(new(1, "Podstawowa ankieta dotycząca współlokatora"), FormStatus.Filled)
        ];
    }

    [HttpGet("get/{formId}/{studentId}")]
    [Authorize(Roles = Roles.Student)]
    public async Task<StudentAnswerDisplay> Get(int formId, int studentId)
    {
        return new StudentAnswerDisplay(1, formId, studentId, [
            new(1, "Nie pali (przynajmniej nie w pokoju XD)", false),
            new(2, "Nie zamawia tajskiego z dowozem", false)
        ], [
            new(1, new(1, "Czy wolisz dzielić pokój z osobą tej samej płci?", [
                new(1, "Tak"), new(2, "Nie"), new(3, "Nie mam preferencji")
            ]), 3, false),
            new(2, new(2, "Jakie cechy są dla Ciebie najważniejsze u współlokatora?", [
                new(4, "Czystość i porządek"), new(5, "Towarzyskość"), new(6, "Cichy tryb życia"),
                new(7, "Wspólne zainteresowania"), new(8, "Brak nałogów (np. palenie, alkohol)")
            ]), 5, true),
            new(3, new(3, "Jakie godziny aktywności Ci odpowiadają?", [
                new(9, "Poranne (6:00–10:00)"), new(10, "Dziennie (10:00–18:00)"),
                new(11, "Wieczorne (18:00–24:00)"), new(12, "Nocne (24:00–6:00)")
            ]), 12, true),
            new(4, new(4, "Czy ważne jest dla Ciebie, aby współlokator był studentem tego samego kierunku?", [
                new(13, "Tak"), new(14, "Nie"), new(15, "Nie mam preferencji")
            ]), 13, false),
            new(5, new(5, "Czy współlokator powinien przestrzegać ciszy nocnej?", [
                new(16, "Zdecydowanie tak"), new(16, "Raczej tak"), new(17, "Raczej nie"), new(18, "Zdecydowanie nie")
            ]), 17, false)
        ]);
    }

    [HttpPost("create")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> Create([FromBody] StudentAnswerCreateRequest createRequest)
    {
        return NotFound();
    }

    [HttpPut("update")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> Update([FromBody] StudentAnswerUpdateRequest updateRequest)
    {
        return NotFound();
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> Delete(int id)
    {
        return NotFound();
    }
}
