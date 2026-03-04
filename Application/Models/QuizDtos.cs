namespace MyLittlePony_Conexy.Application.Models;

public sealed class QuestionDto
{
    public required int Id { get; init; }

    public required string Text { get; init; }

    public required List<AnswerOptionDto> Options { get; init; }
}

public sealed class AnswerOptionDto
{
    public required int Id { get; init; }

    public required string Text { get; init; }
}

public sealed class QuizResultRequestDto
{
    public required List<int> SelectedOptionIds { get; init; }
}

public sealed class PonyTraitDto
{
    public required int Id { get; init; }

    public required string Name { get; init; }
}

public sealed class PonyResultDto
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public string? ImageUrl { get; init; }

    public required List<PonyTraitDto> Traits { get; init; }

    public required int TotalScore { get; init; }
}

public sealed class QuizResultDto
{
    public required PonyResultDto Pony { get; init; }
}

