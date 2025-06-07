using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
    public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(cancellationToken) > 0;
    }

    public DbSet<Ativo> Ativos { get; set; } = default!;
    public DbSet<Cotacao> Cotacoes { get; set; } = default!;
    public DbSet<Posicao> Posicoes { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ativo>(entity =>
        {
            entity.ToTable("ativos");
            entity.HasKey(e => e.IdAtivo);
            entity.Property(e => e.IdAtivo).HasColumnName("id_ativo").ValueGeneratedOnAdd();
            entity.Property(e => e.Codigo).HasColumnName("codigo").IsRequired().HasMaxLength(10);
            entity.HasIndex(e => e.Codigo).IsUnique();
            entity.Property(e => e.Nome).HasColumnName("nome").IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<Cotacao>(entity =>
        {
            entity.ToTable("cotacoes");
            entity.HasKey(e => e.IdCotacao);
            entity.Property(e => e.IdCotacao).HasColumnName("id_cotacao").ValueGeneratedOnAdd();
            entity.Property(e => e.FkIdAtivo).HasColumnName("fk_id_ativo").IsRequired();
            entity.Property(e => e.PrecoUnitario).HasColumnName("preco_unitario").HasColumnType("DECIMAL(18, 8)").IsRequired();
            entity.Property(e => e.DataHora).HasColumnName("data_hora").IsRequired();
            entity.HasOne(c => c.Ativo)
                  .WithMany(a => a.Cotacoes)
                  .HasForeignKey(c => c.FkIdAtivo)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Posicao>(entity =>
        {
            entity.ToTable("posicoes");
            entity.HasKey(e => e.IdPosicao);
            entity.Property(e => e.IdPosicao).HasColumnName("id_posicao").ValueGeneratedOnAdd();
            entity.Property(e => e.FkIdUsuario).HasColumnName("fk_id_usuario").IsRequired();
            entity.Property(e => e.FkIdAtivo).HasColumnName("fk_id_ativo").IsRequired();
            entity.Property(e => e.Quantidade).HasColumnName("quantidade").IsRequired();
            entity.Property(e => e.PrecoMedio).HasColumnName("preco_medio").HasColumnType("DECIMAL(18, 8)").IsRequired();
            entity.Property(e => e.PL).HasColumnName("p_l").HasColumnType("DECIMAL(18, 8)").IsRequired();
            entity.HasIndex(e => new { e.FkIdUsuario, e.FkIdAtivo }).IsUnique();
            entity.HasOne(p => p.Ativo)
                  .WithMany(a => a.Posicoes)
                  .HasForeignKey(p => p.FkIdAtivo)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}