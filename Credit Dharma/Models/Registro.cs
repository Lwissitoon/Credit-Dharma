using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Credit_Dharma.Models
{
    public class Registro
    {
        [Key]
        [DisplayName("Numero de identificacion")]
        public int IdNotification { get; set; }
        [Required]
        [DisplayName("Fecha Registro")]
        public string NotificationDate { get; set; }
        [Required]
        [DisplayName("Cuenta del cliente")]
        public string UserAccountNumber { get; set; }
        [Required]
        [DisplayName("Detalles")]
        public string NotificationDetails { get; set; }
        [Required]
        [DisplayName("Realizada por")]
        public string Username { get; set; }

    }
}
