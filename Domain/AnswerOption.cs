namespace MyLittlePony_Conexy.Domain;

public class AnswerOption
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public Question Question { get; set; } = null!;

    public required string Text { get; set; }

    public List<PonyWeight> PonyWeights { get; set; } = new();
}

