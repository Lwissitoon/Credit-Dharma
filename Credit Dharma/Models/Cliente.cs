using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Credit_Dharma.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Identification { get; set; }
        [DisplayName("Estatus")]
        public string Status { get; set; }
        [DisplayName("Divisa")]
        public string Currency { get; set; }
        [DisplayName("Tipo de cuenta")]
        public string AccountSubType { get; set; }
        [DisplayName("Descripcion")]
        public string Nickname { get; set; }
        [DisplayName("Apertura")]
        public string OpeningDate { get; set; }
        [DisplayName("Monto Pagado")]
        public double Amount { get; set; }
        [DisplayName("Nombre")]
        public string Name { get; set; }
        [DisplayName("Apellido")]
        public string Lastname { get; set; }
        [DisplayName("Correo")]
        public string Email { get; set; }
        [DisplayName("Telefono")]
        public string PhoneNumber { get; set; }
        [DisplayName("Monto total")]
        public double TotalAmount { get; set; }
        [DisplayName("Cuotas pagadas")]

        public int Payments { get; set; }
        [DisplayName("Cuotas Pendientes")]
        public int PendingPayments { get; set; }
        [DisplayName("Cuota Mensual")]

        public double MonthlyPay { get; set; }

    }
}
