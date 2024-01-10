using System.ComponentModel.DataAnnotations;
namespace hotelapp_frontend.Models
{
    public class Rol
    {
        [Key]
        public int IDRol { get; set; }
        public string NombreRol { get; set;}
    }
}
