using Credit_Dharma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Credit_Dharma.Helper
{
    public static class CustomFuctions
    {
        public static int GetPaymentCount(DateTime start, DateTime end)
        {
            var count = (end.Month + end.Year * 12) - (start.Month + start.Year * 12);

            var count2 = (int)(( end-start ).TotalDays / 30);
            return count2;
        }


        public static double GetMorosidadGeneral(List<Cliente>clientes)
        {

            double amount = 0, totalAmount = 0, pendingBalance = 0;
            foreach (var cliente in clientes)
            {
                try
                {
                    cliente.PendingPayments = CustomFuctions.GetPaymentCount(DateTime.Parse(cliente.OpeningDate), DateTime.Now) - cliente.Payments;
                }
                catch (ArgumentNullException)
                {
                    cliente.PendingPayments = 0;
                }
                if (cliente.PendingPayments > 3)
                {
                    pendingBalance += cliente.TotalAmount - cliente.Amount;
                }
                //  amount += cliente.Amount;
                totalAmount += cliente.TotalAmount;
            }

            return (pendingBalance / totalAmount) * 100;
        }
    

    public static double GetMorosidad(List<Cliente> clientes,Cliente ocliente)
    {

        double  totalAmount = 0, pendingBalance = 0;
        foreach (var cliente in clientes)
        {
            try
            {
                cliente.PendingPayments = CustomFuctions.GetPaymentCount(DateTime.Parse(cliente.OpeningDate), DateTime.Now) - cliente.Payments;
            }
            catch (ArgumentNullException)
            {
                cliente.PendingPayments = 0;
            }
            if (cliente.PendingPayments > 3)
            {
                pendingBalance += cliente.TotalAmount - cliente.Amount;
            }
            //  amount += cliente.Amount;
            totalAmount += cliente.TotalAmount;
        }

            return ((ocliente.TotalAmount - ocliente.Amount) / ((pendingBalance)) * 100);
    }



        public static string CalificarCliente( int cuotas)
        {
            if (cuotas<=1)
            {
                return "A";
            }
            else if(cuotas==2 )
            {
                return "B";
            }
            else if (cuotas == 3)
            {
                return "C";
            }
            else if (cuotas ==4||cuotas==5 ||cuotas==6)
            {
                return "D1";
            }
            else if (cuotas == 7 || cuotas == 8 || cuotas == 9)
            {
                return "D2";
            }
            else
            {
                return "E";
            }
        }
}

}
