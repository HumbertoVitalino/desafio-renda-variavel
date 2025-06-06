using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Asset> Assets { get; set; } = default!;
    public DbSet<Operation> Operations { get; set; } = default!;
    public DbSet<Position> Positions { get; set; } = default!;
    public DbSet<Quote> Quotes { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("usuarios");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id_usuario").ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt).HasColumnName("data_criacao").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnName("data_atualizacao").IsRequired();

            entity.Property(e => e.Name).HasColumnName("nome").IsRequired().HasMaxLength(255);
            entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasColumnName("senha_hash").IsRequired();
            entity.Property(e => e.PasswordSalt).HasColumnName("senha_salt").IsRequired();
            entity.Property(e => e.BrokerageRate).HasColumnName("taxa_corretagem").HasColumnType("decimal(5, 4)").IsRequired();

            entity.HasIndex(e => e.Email).IsUnique();

            entity.HasMany(e => e.Operations).WithOne(o => o.User).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.Positions).WithOne(p => p.User).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.ToTable("ativos");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id_ativo").ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt).HasColumnName("data_criacao").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnName("data_atualizacao").IsRequired();

            entity.Property(e => e.TickerSymbol).HasColumnName("codigo_ativo").IsRequired().HasMaxLength(10);
            entity.Property(e => e.Name).HasColumnName("nome_ativo").IsRequired().HasMaxLength(255);

            entity.HasIndex(e => e.TickerSymbol).IsUnique();

            entity.HasMany(e => e.Quotes).WithOne(q => q.Asset).HasForeignKey(q => q.AssetId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Operations).WithOne(o => o.Asset).HasForeignKey(o => o.AssetId).OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.Positions).WithOne(p => p.Asset).HasForeignKey(p => p.AssetId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Operation>(entity =>
        {
            entity.ToTable("operacoes");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id_operacao").ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt).HasColumnName("data_criacao").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnName("data_atualizacao").IsRequired();

            entity.Property(e => e.UserId).HasColumnName("id_usuario").IsRequired();
            entity.Property(e => e.AssetId).HasColumnName("id_ativo").IsRequired();
            entity.Property(e => e.Quantity).HasColumnName("quantidade").IsRequired();
            entity.Property(e => e.UnitPrice).HasColumnName("preco_unitario").HasColumnType("decimal(18, 8)").IsRequired();
            entity.Property(e => e.Type).HasColumnName("tipo_operacao").IsRequired();
            entity.Property(e => e.BrokerageFee).HasColumnName("valor_corretagem").HasColumnType("decimal(18, 8)").IsRequired();
            entity.Property(e => e.DateTime).HasColumnName("data_hora_operacao").IsRequired();

            entity.HasIndex(e => new { e.UserId, e.AssetId, e.DateTime }).HasDatabaseName("ix_operacoes_usuario_ativo_data");
        });

        modelBuilder.Entity<Quote>(entity =>
        {
            entity.ToTable("cotacoes");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id_cotacao").ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt).HasColumnName("data_criacao").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnName("data_atualizacao").IsRequired();

            entity.Property(e => e.AssetId).HasColumnName("id_ativo").IsRequired();
            entity.Property(e => e.UnitPrice).HasColumnName("preco_unitario").HasColumnType("decimal(18, 8)").IsRequired();
            entity.Property(e => e.DateTime).HasColumnName("data_hora_cotacao").IsRequired();

            entity.HasIndex(e => new { e.AssetId, e.DateTime }).IsUnique();
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("posicoes");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id_posicao").ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt).HasColumnName("data_criacao").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnName("data_atualizacao").IsRequired();

            entity.Property(e => e.UserId).HasColumnName("id_usuario").IsRequired();
            entity.Property(e => e.AssetId).HasColumnName("id_ativo").IsRequired();
            entity.Property(e => e.Quantity).HasColumnName("quantidade").IsRequired();
            entity.Property(e => e.AveragePrice).HasColumnName("preco_medio").HasColumnType("decimal(18, 8)").IsRequired();
            entity.Property(e => e.ProfitAndLoss).HasColumnName("lucro_prejuizo_atual").HasColumnType("decimal(18, 8)").IsRequired();

            entity.HasIndex(e => new { e.UserId, e.AssetId }).IsUnique();
        });
    }
}
