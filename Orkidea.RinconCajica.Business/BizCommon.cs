﻿using System;
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
        public void sendContactMessage(string fromMail, string fromName, string subject, string message, string rootPath)
        {
            List<System.Net.Mail.MailAddress> to = new List<System.Net.Mail.MailAddress>();            

            if (ConfigurationManager.AppSettings["testMail"].ToString() == "N")
                to.Add(new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailContactFrontAddress"].ToString()));
            else
                to.Add(new System.Net.Mail.MailAddress("silverio.bernal@orkidea.co"));


            Dictionary<string, string> dynamicValues = new Dictionary<string, string>();
            dynamicValues.Add("[usuario]", fromName);
            dynamicValues.Add("[correo]", fromMail);
            dynamicValues.Add("[fecha]", DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss"));
            dynamicValues.Add("[mensaje]", message);

            MailingHelper.SendMail(to, subject,
                 rootPath + ConfigurationManager.AppSettings["MailContactoHtml"].ToString(),
                rootPath + ConfigurationManager.AppSettings["MailContactoText"].ToString(),
                rootPath + ConfigurationManager.AppSettings["emailLogoPath"].ToString(), dynamicValues);
        }
    }
}
