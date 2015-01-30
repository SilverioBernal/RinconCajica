using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.RinconCajica.Entities
{
    public class Partner
    {
        public int docid { get; set; }
        public string doccl { get; set; }
        public string docto { get; set; }
        public string carnet { get; set; }
        public string accion { get; set; }
        public string cod_clie { get; set; }
        public string rel_tit { get; set; }
        public string nombre { get; set; }
        public string doreco { get; set; }
        public string celular { get; set; }
        public string teleco { get; set; }
        public string telefn { get; set; }
        public string correo { get; set; }
        public string ciudad { get; set; }
        public string fec_nac { get; set; }
        public string estado { get; set; }
        public string sexo { get; set; }
        public string fecmatr { get; set; }
        public string acta { get; set; }
        public string cod_est { get; set; }
        public string looker { get; set; }
        public string direcc { get; set; }
        public Nullable<int> idUsuario { get; set; }
        public List<Partner> lsGrupoFamiliar { get; set; }
        public IDictionary<string, string> lsTipoDocumento { get; set; }
        public IDictionary<string, string> lsSexo { get; set; }
        public IDictionary<string, string> lsEstadoCivil { get; set; }
        public DateTime fechaActualizacion { get; set; }

        public Partner()
        {
            lsGrupoFamiliar = new List<Partner>();
            lsTipoDocumento = new Dictionary<string, string>();

            lsTipoDocumento.Add("C", "Cédula de Ciudadanía");
            lsTipoDocumento.Add("T", "Tarjeta de Identidad");
            lsTipoDocumento.Add("R", "Registro Civil");
            lsTipoDocumento.Add("P", "Pasaporte");
            lsTipoDocumento.Add("E", "Cédula de Extranjería");
            lsTipoDocumento.Add("O", "Otro");

            lsSexo = new Dictionary<string, string>();
            lsSexo.Add("F", "Femenino");
            lsSexo.Add("M", "Masculino");

            lsEstadoCivil = new Dictionary<string, string>();
            lsEstadoCivil.Add("CA", "Casado(a)");
            lsEstadoCivil.Add("SO", "Soltero(a)");
            lsEstadoCivil.Add("DI", "Divirciado(a)");
            lsEstadoCivil.Add("VI", "Viudo(a)");
            lsEstadoCivil.Add("UN", "Union Libre");
        }
    }
}

