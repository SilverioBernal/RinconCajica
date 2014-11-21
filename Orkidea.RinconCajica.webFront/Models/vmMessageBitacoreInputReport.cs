
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Orkidea.RinconCajica.webFront.Models
{
    [Serializable]
    [XmlRoot("correo-recibido")]
    public class vmMessageBitacoreInputReport
    {
        [XmlAttribute("id")]
        public long idRegistro { get; set; }
        [XmlAttribute("Fecha")]
        public DateTime fecha { get; set; }
        [XmlAttribute("Origen")]
        public string origen { get; set; }
        [XmlAttribute("Destino")]
        public string destino { get; set; }
        [XmlAttribute("Tipo")]
        public string descripcionCorta { get; set; }
        [XmlAttribute("Observaciones")]
        public string descripcionLarga { get; set; }        
    }
}