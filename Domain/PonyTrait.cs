namespace MyLittlePony_Conexy.Domain;

public class PonyTrait
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public int PonyId { get; set; }

    public Pony Pony { get; set; } = null!;
}

