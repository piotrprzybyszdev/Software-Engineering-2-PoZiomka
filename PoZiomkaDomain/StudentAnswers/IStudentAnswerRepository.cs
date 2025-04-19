using PoZiomkaDomain.StudentAnswers.Dtos;

namespace PoZiomkaDomain.StudentAnswers;

public interface IStudentAnswerRepository
{
    // Will be used during update of form, because if we update form
    // and there will be no more preference with this id, we need to delete all amswers to this preference
    Task DeleteAllAnswersToPreference(int preferenceId, CancellationToken? cancellationToken);
}

