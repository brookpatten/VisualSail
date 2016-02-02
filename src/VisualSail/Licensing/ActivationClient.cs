using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace AmphibianSoftware.VisualSail.Licensing
{
    public static class ActivationClient
    {
        public static byte[] Activate(string hardwareId, string orderNumber, string billingZipCode, string version)
        {
            string baseUrl = "http://test.visualsail.com/Activate.aspx";
            string parms = "?HardwareID=" + hardwareId + "&OrderNumber=" + orderNumber + "&BillingZipCode=" + billingZipCode;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl+parms);
            request.Timeout = 30000;
            request.UserAgent = "VisualSail";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            MemoryStream memoryStream = new MemoryStream(0x10000);

            using (Stream responseStream = request.GetResponse().GetResponseStream())
            {
                byte[] buffer = new byte[0x1000];
                int bytes;
                while ((bytes = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    memoryStream.Write(buffer, 0, bytes);
                }
            }
            return memoryStream.ToArray();
        }
    }
}
