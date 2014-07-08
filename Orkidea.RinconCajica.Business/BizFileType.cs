using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orkidea.RinconCajica.DataAccessEF;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.Business
{
    public class BizFileType
    {
        public FileType GetFileTypebyKey(FileType fileType)
        {
            FileType oFileType = new FileType();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oFileType = ctx.FileType.Where(x => x.tipoMIME.Equals(fileType.tipoMIME)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oFileType;
        }
    }
}
