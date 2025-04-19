

using PoZiomkaDomain.StudentAnswers;
using PoZiomkaDomain.StudentAnswers.Dtos;

namespace PoZiomkaInfrastructure.Repositories;

public class StudentAnswerRepository : IStudentAnswerRepository
{
    public Task CreateAnswer(int studentId, int FormId, IEnumerable<(string Name, bool IsHidden)> ChoosableAnswers, IEnumerable<(int ObligatoryPreferenceId, int ObligatoryPreferenceOptionId, bool IsHidden)> ObligatoryAnswers, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAnswer(int formId, int studentId, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<StudentAnswerDisplay> GetStudentAnswer(int formId, int studentId, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<StudentAnswerStatus>> GetStudentFormAnswerStatus(int studentId, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAnswer(int studentId, int FormId, IEnumerable<(string Name, bool IsHidden)> ChoosableAnswers, IEnumerable<(int ObligatoryPreferenceId, int ObligatoryPreferenceOptionId, bool IsHidden)> ObligatoryAnswers, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }
}
