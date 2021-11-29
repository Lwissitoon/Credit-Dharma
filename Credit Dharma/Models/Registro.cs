using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Credit_Dharma.Models
{
    public class Registro
    {
        [Key]
        public int IdNotification { get; set; }
        public string NotificationDate { get; set; }
        public string UserAccountNumber { get; set; }
        public string NotificationDetails { get; set; }
        public string Username { get; set; }

    }
}
