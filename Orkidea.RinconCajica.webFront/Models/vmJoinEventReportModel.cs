
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Orkidea.RinconCajica.webFront.Models
{
    [Serializable]
    public class vmJoinEventReportModel
    {
        [XmlIgnore]
        public int id { get; set; }
        [XmlIgnore]
        public int idEvento { get; set; }
        [XmlAttribute("id-socio")]
        public int idSocio { get; set; }
        [XmlAttribute("tipo-identificacion")]
        public string tipoIdentificacion { get; set; }
        [XmlAttribute("identificacion")]
        public string identificacion { get; set; }
        [XmlAttribute("nombre")]
        public string nombre { get; set; }
        [XmlAttribute("telefono-fijo")]
        public string telefonoFijo { get; set; }
        [XmlAttribute("telefono-celular")]
        public string telefonoCelular { get; set; }
        [XmlAttribute("correo-electronico")]
        public string email { get; set; }
        [XmlAttribute("Pregunta 1")]
        public string campoLibre1 { get; set; }
        [XmlAttribute("Pregunta 2")]
        public string campoLibre2 { get; set; }
        [XmlAttribute("Pregunta 3")]
        public string campoLibre3 { get; set; }
        [XmlAttribute("fecha-inscripcion")]
        public System.DateTime fechaInscripcion { get; set; }
    }
}