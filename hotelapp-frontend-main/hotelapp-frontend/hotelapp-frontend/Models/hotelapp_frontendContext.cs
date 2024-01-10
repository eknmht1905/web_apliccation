using Microsoft.EntityFrameworkCore;

namespace hotelapp_frontend.Models
{
    public class hotelapp_frontendContext : DbContext
    {
        public hotelapp_frontendContext(DbContextOptions<hotelapp_frontendContext> opciones)
            : base(opciones)
        {
        }

        //Entidades o las Tables de la DB
        public DbSet<Habitacion> Habitacion { get; set; }
    }
}