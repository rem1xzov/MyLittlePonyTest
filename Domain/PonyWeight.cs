namespace MyLittlePony_Conexy.Domain;

public class PonyWeight
{
    public int Id { get; set; }

    public int AnswerOptionId { get; set; }

    public AnswerOption AnswerOption { get; set; } = null!;

    public int PonyId { get; set; }

    public Pony Pony { get; set; } = null!;

    public int Weight { get; set; }
}

