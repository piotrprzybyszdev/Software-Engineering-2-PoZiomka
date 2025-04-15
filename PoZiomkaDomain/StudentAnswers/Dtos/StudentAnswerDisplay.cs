﻿using PoZiomkaDomain.Form.Dtos;

namespace PoZiomkaDomain.StudentAnswers.Dtos;

public record StudentAnswerChoosableDisplay(int Id, string Name, bool IsHidden);
public record StudentAnswerObligatoryDisplay(int Id, ObligatoryPreferenceDisplay ObligatoryPreference, int ObligatoryPreferenceOptionId, bool IsHidden);
public record StudentAnswerDisplay(int Id, int FormId, int StudentId, IEnumerable<StudentAnswerChoosableDisplay> ChoosableAnswers, IEnumerable<StudentAnswerObligatoryDisplay> ObligatoryAnswers);

public enum FormStatus
{
    NotFilled, InProgress, Filled
}

public record StudentAnswerStatus(FormModel Form, FormStatus Status);