using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace redeSocial.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
            Usuarios = Set<Usuario>();
            Arquivos = Set<ArquivoMidia>();
            Postagens = Set<Postagem>();
            Comentarios = Set<Comentario>();
            Banimentos = Set<Banimentos>();
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ArquivoMidia> Arquivos { get; set; }
        public DbSet<Postagem> Postagens { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Banimentos> Banimentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definição das chaves primárias
            modelBuilder.Entity<Usuario>().HasKey(x  => x.usuarioID);
            modelBuilder.Entity<Postagem>().HasKey(x => x.postID);
            modelBuilder.Entity<ArquivoMidia>().HasKey(x => x.arquivoID);
            modelBuilder.Entity<Comentario>().HasKey(x => x.comentID);
            modelBuilder.Entity<Banimentos>().HasKey(x => x.banID);

            // Definição dos relacionamentos
            modelBuilder.Entity<Postagem>()
                .HasOne(e => e.usuario)
                .WithMany()
                .HasForeignKey(e => e.usuarioID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ArquivoMidia>()
                .HasOne(e => e.post)
                .WithMany()
                .HasForeignKey(e => e.postID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comentario>()
                .HasOne(e => e.post)
                .WithMany()
                .HasForeignKey(e => e.postID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comentario>()
                .HasOne(e => e.usuario)
                .WithMany()
                .HasForeignKey(e => e.usuarioID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Banimentos>()
                .HasOne(e => e.usuario)
                .WithMany()
                .HasForeignKey(e => e.usuarioID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Banimentos>()
                .HasOne(e => e.usuarioBan)
                .WithMany()
                .HasForeignKey(e => e.usuarioBanID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
