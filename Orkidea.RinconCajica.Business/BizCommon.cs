using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orkidea.RinconCajica.Utilities;

namespace Orkidea.RinconCajica.Business
{
    public class BizCommon
    {
        public void sendContactMessage(string from, string subject, string message)
        {
            List<System.Net.Mail.MailAddress> to = new List<System.Net.Mail.MailAddress>();
            to.Add(new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailContactFrontAddress"].ToString()));

            MailingHelper.SendMail(to,subject, message);
        }
    }
}
