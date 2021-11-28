using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManager.Models
{
    public class Usuario
    {
        [Key]

        public int UserId { get; set; }
        [DisplayName("Usuario")]
        [Required]
        public string Username { get; set; }
        [DisplayName("Nombre")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Apellido")]
        [Required]
        public string Lastname { get; set; }
        [DisplayName("Perfil")]
        [Required]
        public string Role { get; set; }
        [DisplayName("Clave")]
        [Required]
        public string Password { get; set; }
    }
}
