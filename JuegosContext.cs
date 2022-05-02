using Microsoft.EntityFrameworkCore;

namespace Juegos.Models
{
    public class JuegosCatalogoContext : DbContext
    {
        public JuegosCatalogoContext(DbContextOptions<JuegosCatalogoContext> options) : base(options) { }

        public DbSet<CatalogoJuegos> CatalogoJuegos { get; set; }
        public string connString { get; private set; }

        public JuegosCatalogoContext()
        {
            connString =
                $"Server=185.60.40.210\\SQLEXPRESS,58015;Database=ProyectoCatalogoJuegos;User Id=sa;Password=Pa88word;";
            //connString = $"Server=localhost;Database=EFPrueba;User Id=sa;Password=Pa88word;";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) =>
            options.UseSqlServer(connString);
    }
}
