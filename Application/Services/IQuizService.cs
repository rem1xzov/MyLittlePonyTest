using MyLittlePony_Conexy.Application.Models;

namespace MyLittlePony_Conexy.Application.Services;

public interface IQuizService
{
    Task<IReadOnlyCollection<QuestionDto>> GetQuizAsync(CancellationToken cancellationToken = default);

    Task<QuizResultDto> CalculateResultAsync(
        IReadOnlyCollection<int> selectedOptionIds,
        CancellationToken cancellationToken = default);
}

