using Microsoft.EntityFrameworkCore;
using MyLittlePony_Conexy.Domain;

namespace MyLittlePony_Conexy.Infrastructure;

public class QuizDbContext(DbContextOptions<QuizDbContext> options) : DbContext(options)
{
    public DbSet<Pony> Ponies => Set<Pony>();

    public DbSet<PonyTrait> PonyTraits => Set<PonyTrait>();

    public DbSet<Question> Questions => Set<Question>();

    public DbSet<AnswerOption> AnswerOptions => Set<AnswerOption>();

    public DbSet<PonyWeight> PonyWeights => Set<PonyWeight>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigurePony(modelBuilder);
        ConfigurePonyTrait(modelBuilder);
        ConfigureQuestion(modelBuilder);
        ConfigureAnswerOption(modelBuilder);
        ConfigurePonyWeight(modelBuilder);
    }

    private static void ConfigurePony(ModelBuilder modelBuilder)
    {
        var pony = modelBuilder.Entity<Pony>();

        pony.ToTable("Ponies");

        pony.HasKey(p => p.Id);

        pony.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        pony.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(500);

        pony.Property(p => p.ImageUrl)
            .HasMaxLength(500);

        pony.HasMany(p => p.Traits)
            .WithOne(t => t.Pony)
            .HasForeignKey(t => t.PonyId)
            .OnDelete(DeleteBehavior.Cascade);

        pony.HasMany(p => p.Weights)
            .WithOne(w => w.Pony)
            .HasForeignKey(w => w.PonyId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigurePonyTrait(ModelBuilder modelBuilder)
    {
        var trait = modelBuilder.Entity<PonyTrait>();

        trait.ToTable("PonyTraits");

        trait.HasKey(t => t.Id);

        trait.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);
    }

    private static void ConfigureQuestion(ModelBuilder modelBuilder)
    {
        var question = modelBuilder.Entity<Question>();

        question.ToTable("Questions");

        question.HasKey(q => q.Id);

        question.Property(q => q.Text)
            .IsRequired()
            .HasMaxLength(500);

        question.HasMany(q => q.Options)
            .WithOne(o => o.Question)
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureAnswerOption(ModelBuilder modelBuilder)
    {
        var option = modelBuilder.Entity<AnswerOption>();

        option.ToTable("AnswerOptions");

        option.HasKey(o => o.Id);

        option.Property(o => o.Text)
            .IsRequired()
            .HasMaxLength(300);

        option.HasMany(o => o.PonyWeights)
            .WithOne(w => w.AnswerOption)
            .HasForeignKey(w => w.AnswerOptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigurePonyWeight(ModelBuilder modelBuilder)
    {
        var weight = modelBuilder.Entity<PonyWeight>();

        weight.ToTable("PonyWeights");

        weight.HasKey(w => w.Id);

        weight.Property(w => w.Weight)
            .IsRequired();

        weight.HasIndex(w => new { w.AnswerOptionId, w.PonyId })
            .IsUnique();
    }
}

