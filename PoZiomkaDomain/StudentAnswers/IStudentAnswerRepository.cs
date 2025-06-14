﻿using PoZiomkaDomain.StudentAnswers.Dtos;

namespace PoZiomkaDomain.StudentAnswers;

public interface IStudentAnswerRepository
{
    Task<IEnumerable<StudentAnswerStatus>> GetStudentFormAnswerStatus(int studentId, CancellationToken? cancellationToken);
    Task<StudentAnswerDisplay> GetStudentAnswer(int formId, int studentId, CancellationToken? cancellationToken);
    Task CreateAnswer(int studentId, int FormId, FormStatus status,
        IEnumerable<(string Name, bool IsHidden)> ChoosableAnswers,
        IEnumerable<(int ObligatoryPreferenceId, int ObligatoryPreferenceOptionId, bool IsHidden)> ObligatoryAnswers,
        CancellationToken? cancellationToken);
    Task UpdateAnswer(int studentId, int FormId, FormStatus status,
        IEnumerable<(string Name, bool IsHidden)> ChoosableAnswers,
        IEnumerable<(int ObligatoryPreferenceId, int ObligatoryPreferenceOptionId, bool IsHidden)> ObligatoryAnswers,
        CancellationToken? cancellationToken);
    Task DeleteAnswer(int answerId, CancellationToken? cancellationToken);
}

