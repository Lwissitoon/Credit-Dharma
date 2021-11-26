using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Credit_Dharma.Models
{
    public class Client
    {


            [Key]
            public string Identification { get; set; }
            public string Status { get; set; }
            public string Currency { get; set; }
            public string AccountSubType { get; set; }
            public string Nickname { get; set; }
            public string OpeningDate { get; set; }
            public double Amount { get; set; }
            public string Name { get; set; }
            public string Lastname { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
    }
}
