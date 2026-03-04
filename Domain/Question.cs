namespace MyLittlePony_Conexy.Domain;

public class Question
{
    public int Id { get; set; }

    public required string Text { get; set; }

    public List<AnswerOption> Options { get; set; } = new();
}

