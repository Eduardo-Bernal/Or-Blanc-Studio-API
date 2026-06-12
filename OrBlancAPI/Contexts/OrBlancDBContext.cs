using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OrBlancAPI.Domains;

namespace OrBlancAPI.Contexts;

public partial class OrBlancDBContext : DbContext
{
    public OrBlancDBContext()
    {
    }

    public OrBlancDBContext(DbContextOptions<OrBlancDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agendamento> Agendamento { get; set; }

    public virtual DbSet<Cliente> Cliente { get; set; }

    public virtual DbSet<Profissional> Profissional { get; set; }

    public virtual DbSet<Servico> Servico { get; set; }

    public virtual DbSet<VW_AgendaCompleta> VW_AgendaCompleta { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=OrBlancDB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agendamento>(entity =>
        {
            entity.HasKey(e => e.id_agendamento);

            entity.HasIndex(e => e.id_cliente, "IX_Agendamento_Cliente");

            entity.HasIndex(e => e.data_hora_inicio, "IX_Agendamento_DataHora");

            entity.Property(e => e.data_hora_fim).HasColumnType("datetime");
            entity.Property(e => e.data_hora_inicio).HasColumnType("datetime");
            entity.Property(e => e.observacao)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Agendado");

            entity.HasOne(d => d.id_clienteNavigation).WithMany(p => p.Agendamento)
                .HasForeignKey(d => d.id_cliente)
                .HasConstraintName("FK_Agendamento_Cliente");

            entity.HasOne(d => d.id_servicoNavigation).WithMany(p => p.Agendamento)
                .HasForeignKey(d => d.id_servico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agendamento_Servico");

            entity.HasMany(d => d.id_profissional).WithMany(p => p.id_agendamento)
                .UsingEntity<Dictionary<string, object>>(
                    "Agendamento_Profissional",
                    r => r.HasOne<Profissional>().WithMany()
                        .HasForeignKey("id_profissional")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_AgPro_Profissional"),
                    l => l.HasOne<Agendamento>().WithMany()
                        .HasForeignKey("id_agendamento")
                        .HasConstraintName("FK_AgPro_Agendamento"),
                    j =>
                    {
                        j.HasKey("id_agendamento", "id_profissional");
                        j.ToTable(tb => tb.HasTrigger("TRG_VerificaConflito"));
                        j.HasIndex(new[] { "id_profissional" }, "IX_AgendamentoProfissional_Profissional");
                    });
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.id_cliente);

            entity.HasIndex(e => e.email, "UQ_Cliente_Email").IsUnique();

            entity.HasIndex(e => e.telefone, "UQ_Cliente_Fone").IsUnique();

            entity.Property(e => e.id_cliente).HasDefaultValueSql("(newid())");
            entity.Property(e => e.email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.senha).HasMaxLength(32);
            entity.Property(e => e.telefone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Profissional>(entity =>
        {
            entity.HasKey(e => e.id_profissional);

            entity.Property(e => e.id_profissional).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ativo).HasDefaultValue(true);
            entity.Property(e => e.email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.especialidade)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.senha).HasMaxLength(32);
            entity.Property(e => e.telefone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Servico>(entity =>
        {
            entity.HasKey(e => e.id_servico);

            entity.Property(e => e.ativo).HasDefaultValue(true);
            entity.Property(e => e.descricao)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.valor).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<VW_AgendaCompleta>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_AgendaCompleta");

            entity.Property(e => e.data_hora_fim).HasColumnType("datetime");
            entity.Property(e => e.data_hora_inicio).HasColumnType("datetime");
            entity.Property(e => e.especialidade)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.nome_cliente)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.nome_profissional)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.nome_servico)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.observacao)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.status)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.telefone_cliente)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.valor_servico).HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
