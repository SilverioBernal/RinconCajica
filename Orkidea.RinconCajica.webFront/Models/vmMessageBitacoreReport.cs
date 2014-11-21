
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Orkidea.RinconCajica.webFront.Models
{
    [Serializable]
    [XmlRoot("correo-recibido")]
    public class vmMessageBitacoreReport
    {
        [XmlAttribute("TipoRegistro")]
        public string tipoRegistro { get; set; }
        [XmlAttribute("ID")]
        public long idRegistro { get; set; }
        [XmlAttribute("Fecha")]
        public System.DateTime fecha { get; set; }
        [XmlAttribute("Origen")]
        public string origen { get; set; }
        [XmlAttribute("Destino")]
        public string destino { get; set; }
        [XmlAttribute("Entrega personal")]
        public bool entregaPersonal { get; set; }
        [XmlAttribute("Prioridad")]
        public string prioridad { get; set; }
        [XmlAttribute("Tipo")]
        public string descripcionCorta { get; set; }
        [XmlAttribute("Observaciones")]
        public string descripcionLarga { get; set; }
        [XmlAttribute("Dirección")]
        public string direccion { get; set; }
        [XmlAttribute("Fecha límite")]
        public Nullable<DateTime> fechaLimite { get; set; }
        [XmlAttribute("Enviado")]
        public bool enviado { get; set; }
    }
}