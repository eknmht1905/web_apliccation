using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hotelapp_frontend.Models
{
    public class Usuario
    {
        [Key]
        public int IDUsuario { get; set; }
        public string Nombre { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public bool Estado { get; set; }
        public int IDRol { get; set; }

    }
}
