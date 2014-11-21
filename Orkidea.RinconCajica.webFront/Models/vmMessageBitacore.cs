using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.webFront.Models
{
    public class vmMessageBitacore : MessageBitacore
    {
        BizMessageActor bizMessageActor = new BizMessageActor();

        public List<MessageActor> lsProveedores { get; set; }
        public List<MessageActor> lsDependencias { get; set; }
        public SortedDictionary<string, string> lsTipoDoc { get; set; }
        public SortedDictionary<byte, string> lsPrioridad { get; set; }
        public string strOrigen { get; set; }
        public string strDestino { get; set; }

        public vmMessageBitacore()
        {
            lsProveedores = new List<MessageActor>();
            lsDependencias = new List<MessageActor>();

            lsProveedores.Add(new MessageActor());
            lsDependencias.Add(new MessageActor());

            lsProveedores.AddRange( bizMessageActor.GetMessageActorListByType("P").OrderBy(x => x.descripcion).ToList());
            lsDependencias.AddRange(bizMessageActor.GetMessageActorListByType("D").OrderBy(x => x.descripcion).ToList());

            lsTipoDoc = new SortedDictionary<string, string>();
            lsPrioridad = new SortedDictionary<byte, string>();

            string[] tiposDoc = ConfigurationManager.AppSettings["tipoDocumento"].ToString().Split('|');

            lsTipoDoc.Add("", "");

            foreach (string item in tiposDoc)
            {
                lsTipoDoc.Add(item, item);
            }
            
            lsPrioridad.Add(1, "Normal");
            lsPrioridad.Add(2, "Alta");
        }


        public vmMessageBitacore(MessageBitacore messageBitacore)
        {
            fecha = messageBitacore.fecha;
            strOrigen = bizMessageActor.GetMessageActorbyKey(new MessageActor() { id = messageBitacore.origen }).descripcion;
            strDestino = bizMessageActor.GetMessageActorbyKey(new MessageActor() { id = messageBitacore.destino }).descripcion;
            descripcionCorta = messageBitacore.descripcionCorta;
            descripcionLarga = messageBitacore.descripcionLarga;
            idRegistro = messageBitacore.idRegistro;
        }
    }
}