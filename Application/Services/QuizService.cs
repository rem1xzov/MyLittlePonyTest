using MyLittlePony_Conexy.Application.Models;
using MyLittlePony_Conexy.Infrastructure.Repositories;

namespace MyLittlePony_Conexy.Application.Services;

public class QuizService(IQuestionRepository questionRepository) : IQuizService
{
    public async Task<IReadOnlyCollection<QuestionDto>> GetQuizAsync(
        CancellationToken cancellationToken = default)
    {
        var questions = await questionRepository.GetAllWithOptionsAsync(cancellationToken);

        return questions
            .Select(q => new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                Options = q.Options
                    .OrderBy(o => o.Id)
                    .Select(o => new AnswerOptionDto
                    {
                        Id = o.Id,
                        Text = o.Text
                    })
                    .ToList()
            })
            .OrderBy(q => q.Id)
            .ToList();
    }

    public async Task<QuizResultDto> CalculateResultAsync(
        IReadOnlyCollection<int> selectedOptionIds,
        CancellationToken cancellationToken = default)
    {
        if (selectedOptionIds.Count == 0)
        {
            throw new ArgumentException("At least one option id must be provided.", nameof(selectedOptionIds));
        }

        var options = await questionRepository.GetOptionsByIdsAsync(selectedOptionIds, cancellationToken);

        if (options.Count == 0)
        {
            throw new InvalidOperationException("No answer options found for the provided ids.");
        }

        // 1. Aggregate scores per pony
        var ponyScores = new Dictionary<int, int>();

        foreach (var option in options)
        {
            foreach (var weight in option.PonyWeights)
            {
                if (!ponyScores.TryAdd(weight.PonyId, weight.Weight))
                {
                    ponyScores[weight.PonyId] += weight.Weight;
                }
            }
        }

        if (ponyScores.Count == 0)
        {
            throw new InvalidOperationException("No scoring data configured for the selected options.");
        }

        // 2. Find ponies with max score
        var maxScore = ponyScores.Max(p => p.Value);
        var topPonies = ponyScores
            .Where(p => p.Value == maxScore)
            .Select(p => p.Key)
            .ToHashSet();

        // 3. If more than one pony => tie-breaker using matching traits
        var winningPony = options
            .SelectMany(o => o.PonyWeights.Select(w => w.Pony))
            .DistinctBy(p => p.Id)
            .Where(p => topPonies.Contains(p.Id))
            .ToList();

        var selectedTraits = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var pony in winningPony)
        {
            foreach (var trait in pony.Traits)
            {
                selectedTraits.Add(trait.Name);
            }
        }

        var bestPony = winningPony[0];
        var bestScore = CountMatchingTraits(bestPony, selectedTraits);

        foreach (var candidate in winningPony.Skip(1))
        {
            var candidateScore = CountMatchingTraits(candidate, selectedTraits);

            if (candidateScore > bestScore)
            {
                bestPony = candidate;
                bestScore = candidateScore;
            }
        }

        var resultPonyDto = new PonyResultDto
        {
            Id = bestPony.Id,
            Name = bestPony.Name,
            Description = bestPony.Description,
            ImageUrl = bestPony.ImageUrl,
            Traits = bestPony.Traits
                .OrderBy(t => t.Name)
                .Select(t => new PonyTraitDto
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToList(),
            TotalScore = ponyScores[bestPony.Id]
        };

        return new QuizResultDto
        {
            Pony = resultPonyDto
        };
    }

    private static int CountMatchingTraits(Domain.Pony pony, HashSet<string> selectedTraits)
    {
        return pony.Traits.Count(t => selectedTraits.Contains(t.Name));
    }
}

