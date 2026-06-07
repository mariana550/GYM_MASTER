// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using PROYECTO_GYM_MASTER.Models;

namespace PROYECTO_GYM_MASTER.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Entrenador> Entrenadores { get; set; }
        public DbSet<Rutina> Rutinas { get; set; }
        public DbSet<Ejercicio> Ejercicios { get; set; }
        public DbSet<RutinaEjercicio> RutinaEjercicios { get; set; }
        public DbSet<RutinaAsignada> RutinasAsignadas { get; set; }
        public DbSet<Progreso> Progresos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Email único
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Usuario tiene una sola RutinaAsignada activa
            modelBuilder.Entity<RutinaAsignada>()
                .HasOne(r => r.Usuario)
                .WithOne(u => u.RutinaAsignada)
                .HasForeignKey<RutinaAsignada>(r => r.UsuarioId);

            // Entrenador -> Usuario (uno a uno)
            modelBuilder.Entity<Entrenador>()
                .HasOne(e => e.Usuario)
                .WithOne()
                .HasForeignKey<Entrenador>(e => e.UsuarioId);

            // Rutina -> Entrenador
            modelBuilder.Entity<Rutina>()
                .HasOne(r => r.Entrenador)
                .WithMany(e => e.Rutinas)
                .HasForeignKey(r => r.EntrenadorId);

            // Progreso -> Usuario
            modelBuilder.Entity<Progreso>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Progresos)
                .HasForeignKey(p => p.UsuarioId);

            // Tabla intermedia RutinaEjercicio
            modelBuilder.Entity<RutinaEjercicio>()
                .HasOne(re => re.Rutina)
                .WithMany(r => r.RutinaEjercicios)
                .HasForeignKey(re => re.RutinaId);

            modelBuilder.Entity<RutinaEjercicio>()
                .HasOne(re => re.Ejercicio)
                .WithMany(e => e.RutinaEjercicios)
                .HasForeignKey(re => re.EjercicioId);
        }
    }
}
