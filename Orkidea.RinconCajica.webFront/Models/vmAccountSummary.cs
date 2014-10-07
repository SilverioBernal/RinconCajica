using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.webFront.Models
{
    public class vmAccountSummary : AccountSummary
    {
        public IEnumerable< HttpPostedFileBase> File { get; set; }
    }
}