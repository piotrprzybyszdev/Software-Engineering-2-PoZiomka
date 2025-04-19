using PoZiomkaDomain.StudentAnswers.Dtos;

namespace PoZiomkaDomain.StudentAnswers;

public interface IStudentAnswerRepository
{
    Task<StudentAnswerStatus> GetStudentFormAnswerStatus(int studentId, CancellationToken? cancellationToken);
    Task<StudentAnswerDisplay> GetStudentAnswer(int formId, int studentId, CancellationToken? cancellationToken);
    Task CreateAnswer(StudentAnswerCreateData data, CancellationToken? cancellationToken);
    Task UpdateAnswer(StudentAnswerUpdateData data, CancellationToken? cancellationToken);
    Task DeleteAnswer(int answerId, CancellationToken? cancellationToken);
}

