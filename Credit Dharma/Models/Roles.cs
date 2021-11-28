using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Credit_Dharma.Models
{
    public enum Roles
    {
        Analista = 1,
        Administrador
    }


    public class Rol
    {
        public Roles roles {get;set;}
        }
}
