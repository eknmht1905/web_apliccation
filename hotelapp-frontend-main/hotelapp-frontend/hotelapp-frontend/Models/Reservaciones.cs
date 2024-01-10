using System.ComponentModel.DataAnnotations;

namespace hotelapp_frontend.Models
{
    public class Reservaciones
    {
        [Key]
        public int IDReservacion { get; set; }

        public int IDHabitacion { get; set; }
        public int IDUsuario { get; set; }
        public int CantidadNoches { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal Costo_Total { get; set; }
    }
}
