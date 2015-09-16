using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AzureUtilities
{
    public static class TimeZoneOrgHelper
    {
        public static DateTime GetZoneNowDateTime(string latitude, string longitude)
        {
            string uri = string.Format("http://new.earthtools.org/timezone/{0}/{1}", latitude, longitude);

            string xml = PostRequest(uri, string.Empty, MediaTypeNames.Text.Html);

            timezone instance = Deserialize(xml);
            DateTime localTime = instance.GetLocalTime();

            return localTime;
        }

        private static timezone Deserialize(string xmlText)
        {
            using (MemoryStream memStream =
               new MemoryStream(Encoding.UTF8.GetBytes(xmlText)))
            {
                XmlSerializer ser = new XmlSerializer(typeof(timezone));
                return (timezone)ser.Deserialize(memStream);
            }
        }

        private static string PostRequest(string uri, string msgBody, string contentType)
        {
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(uri);
            webReq.Timeout = 60 * 1000;
            webReq.Method = "POST";
            webReq.ContentType = contentType;
            webReq.ContentLength = msgBody.Length;

            using (StreamWriter streamOut =
               new StreamWriter(webReq.GetRequestStream(), System.Text.Encoding.ASCII))
            {
                streamOut.Write(msgBody);
                streamOut.Flush();
                streamOut.Close();
            }

            string response = null;
            using (StreamReader streamIn =
               new StreamReader(webReq.GetResponse().GetResponseStream()))
            {
                response = streamIn.ReadToEnd();
                streamIn.Close();
            }

            return response;
        }
    }

    [Serializable()]
    public class GpsCoordinates
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

    [Serializable()]
    public class timezone
    {
        public string version { get; set; }
        public GpsCoordinates location { get; set; }
        public string offset { get; set; }
        public string suffix { get; set; }
        public string localtime { get; set; }
        public string isotime { get; set; }
        public string utctime { get; set; }
        public string dst { get; set; }

        public DateTime GetLocalTime()
        {
            return DateTime.Parse(localtime);
        }
    }
}
