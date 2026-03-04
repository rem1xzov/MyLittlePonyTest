using MyLittlePony_Conexy.Domain;

namespace MyLittlePony_Conexy.Infrastructure.Repositories;

public interface IQuestionRepository
{
    Task<List<Question>> GetAllWithOptionsAsync(CancellationToken cancellationToken = default);

    Task<List<AnswerOption>> GetOptionsByIdsAsync(
        IReadOnlyCollection<int> optionIds,
        CancellationToken cancellationToken = default);
}

