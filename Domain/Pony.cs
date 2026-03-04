namespace MyLittlePony_Conexy.Domain;

public class Pony
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public string? ImageUrl { get; set; }

    public List<PonyTrait> Traits { get; set; } = new();

    public List<PonyWeight> Weights { get; set; } = new();
}

