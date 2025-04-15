﻿namespace PoZiomkaApi.Requests.StudentAnswer;

public record StudentAnswerUpdateRequest(int Id, IEnumerable<StudentChoosableAnswerCreateRequest> ChoosableAnswers, IEnumerable<StudentObligatoryAnswerCreateRequest> ObligatoryAnswers);
