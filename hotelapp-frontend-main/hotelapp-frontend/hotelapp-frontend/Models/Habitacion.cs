using System.ComponentModel.DataAnnotations;

namespace hotelapp_frontend.Models
{
    public class Habitacion
    {
        [Key]
        public int IDHabitacion { get; set; }
        public string NombreHabitacion { get; set; }

        public string Descripcion { get; set; }

        public string Path_Img { get; set; }
        public int CantPersonas { get; set; }

        public int CantCamas { get; set; }

        public bool Disponible { get; set; }
        public decimal CostoNoche { get; set; }

        
    }
}
