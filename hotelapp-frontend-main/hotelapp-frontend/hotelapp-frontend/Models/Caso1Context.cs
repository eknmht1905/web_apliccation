using Microsoft.EntityFrameworkCore;

namespace Caso1.Models
{
    public class Caso1Context : DbContext
    {
        public Caso1Context(DbContextOptions<Caso1Context> opciones)
            : base(opciones)
        {
        }

        //Entidades o las Tables de la DB
        public DbSet<Producto> Producto { get; set; }
    }
}