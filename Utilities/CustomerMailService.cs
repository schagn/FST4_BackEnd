using SharedClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utilities
{
    public static class CustomerMailService
    {

        public static void InformCustomerAboutDelay(SharedBestellung ord, SharedEmailCustomer cust, string product)
        {

            string mailToString = "mailto:" + cust.email + "?subject=Lieferverzug&body=" +
                "Sehr geehrter Herr/Frau " + cust.lastname + ",%0A%0Aes tut uns leid Ihnen mitteilen zu müssen, dass sich Ihre Bestellung mit der Nummer " + ord.BestellId.ToString() + " aufgrund von Lieferschwierigkeiten des/der Produkte " + product + " voraussichtlich 2 Wochen verspäten wird%0A%0A" +
                "Bitte teilen Sie uns mit ob wir das/die Produkt/e aus der Bestellung entfernen sollen, oder ob Sie die Warezeit in Kauf nehmen." +
                "%0A%0A Mit freundlichen Grüßen%0A%0AIhr Get Your Cake Team";

           string encodedUrl =  HttpUtility.UrlPathEncode(mailToString);

            System.Diagnostics.Process.Start(encodedUrl);

        }


    }
}
