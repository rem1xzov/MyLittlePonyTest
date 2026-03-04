using Microsoft.EntityFrameworkCore;
using MyLittlePony_Conexy.Domain;

namespace MyLittlePony_Conexy.Infrastructure.Repositories;

public class QuestionRepository(QuizDbContext dbContext) : IQuestionRepository
{
    public async Task<List<Question>> GetAllWithOptionsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Questions
            .AsNoTracking()
            .Include(q => q.Options)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<AnswerOption>> GetOptionsByIdsAsync(
        IReadOnlyCollection<int> optionIds,
        CancellationToken cancellationToken = default)
    {
        if (optionIds.Count == 0)
        {
            return [];
        }

        return await dbContext.AnswerOptions
            .AsNoTracking()
            .Include(o => o.PonyWeights)
                .ThenInclude(w => w.Pony)
                    .ThenInclude(p => p.Traits)
            .Where(o => optionIds.Contains(o.Id))
            .ToListAsync(cancellationToken);
    }
}

