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

        public static void InformCustomerAboutDelay(SharedBestellung ord, SharedEmailCustomer cust)
        {

            string mailToString = "mailto:" + cust.email + "?subject=Lieferverzug&body=" +
                "Sehr geehrter Herr/Frau " + cust.lastname + ",%0A%0Aes tut uns leid Ihnen mitteilen zu müssen, dass sich Ihre Bestellung mit der Nummer " + ord.BestellId.ToString() + "sich voraussichtlich um 1 Woche verspäten wird" +
                "Wir entscchuldigen uns für die Unannehmlichkeiten"+
                "%0A%0A Mit freundlichen Grüßen%0A%0AIhr Get Your Cake Team";

           string encodedUrl =  HttpUtility.UrlPathEncode(mailToString);

            System.Diagnostics.Process.Start(encodedUrl);

        }

        public static void InformCustomerAboutDeletedProduct(SharedBestellung ord, SharedEmailCustomer cust, string product)
        {

            string mailToString = "mailto:" + cust.email + "?subject=Produkt entfernt&body=" +
                "Sehr geehrter Herr/Frau " + cust.lastname + ",%0A%0Aes hiermit möchten wir Ihnen mitteilen, dass wir aus Ihrer Bestellung mit der Nummer" + ord.BestellId.ToString() + "folgendes Produkt" + product + "entfernt haben" +
                "%0ADer bereits bezahlte Betrag wird auf Ihr verwendetes Zahlungmittel rückerstattet%0A%0AMit freundlichen Grüßen%0A%0AIhr Get Your Cake Team";

            string encodedUrl = HttpUtility.UrlPathEncode(mailToString);

            System.Diagnostics.Process.Start(encodedUrl);

        }

        public static void InformCustomerAboutCancellation(SharedBestellung ord, SharedEmailCustomer cust)
        {

            string mailToString = "mailto:" + cust.email + "?subject=Stornierung&body=" +
                "Sehr geehrter Herr/Frau " + cust.lastname + ",%0A%0Aes hiermit möchten wir Ihnen mitteilen, dass wir aus Ihrer Bestellung mit der Nummer" + ord.BestellId.ToString() + "storniert haben" +
                "%0ADer bereits bezahlte Betrag wird auf Ihr verwendetes Zahlungmittel rückerstattet%0A%0AMit freundlichen Grüßen%0A%0AIhr Get Your Cake Team";

            string encodedUrl = HttpUtility.UrlPathEncode(mailToString);

            System.Diagnostics.Process.Start(encodedUrl);
        }
    }
}
