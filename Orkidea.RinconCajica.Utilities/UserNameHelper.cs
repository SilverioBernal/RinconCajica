using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.RinconCajica.Utilities
{
    public static class UserNameHelper
    {
        public static string userNameGenerator(string user, bool inverse)
        {
            string res = "";

            string userName = evaluateUser(user, inverse);
            string[] userNameArray = userName.Split('|');

            // 2 names, 2 surnames
            if (userNameArray.Count() == 4)
            {
                res += userNameArray[0][0].ToString() + userNameArray[1][0].ToString() + userNameArray[2].ToString() + userNameArray[3][0].ToString() + "|";
            }

            // 1 names, 2 surnames
            if (userNameArray.Count() == 3)
            {
                res += (userNameArray[0][0].ToString() + userNameArray[1].ToString() + userNameArray[2][0].ToString()) + "|";
                res += (userNameArray[0][0].ToString() + userNameArray[2].ToString() + userNameArray[1][0].ToString()) + "|";
            }            

            // 2 names, 1 surnames
            if (userNameArray.Count() == 3)
            {
                res += (userNameArray[0][0].ToString() + userNameArray[1][0].ToString() + userNameArray[2].ToString()) + "|";
                res += (userNameArray[0][0].ToString() + userNameArray[2][0].ToString() + userNameArray[1].ToString()) + "|";
            }

            // 1 names, 1 surnames
            if (userNameArray.Count() == 3)
            {
                res += (userNameArray[0][0].ToString() + userNameArray[1].ToString()) + "|";
            }

            return res.Substring(0, (res.Length - 1));
        }

        public static string evaluateUser(string user, bool inverse)
        {
            string res = "";
            string[] userNameArray = user.Split(' ');


            if (!inverse)
            {

                for (int i = 0; i < userNameArray.Count(); i++)
                {
                    if (evaluateLength(userNameArray[i]))
                        res += userNameArray[i].ToLower() + (i != (userNameArray.Count() - 1) ? "|" : "");
                }
            }
            else
            {
                for (int i = (userNameArray.Count() - 1); i >= 0; i--)
                {
                    if (evaluateLength(userNameArray[i]))
                    {
                        res += userNameArray[i].ToLower() + (i != 0 ? "|" : "");
                    }
                }
            }

            return res;

        }

        private static bool evaluateLength(string stringToEvaluate)
        {
            if (stringToEvaluate.Length <= 2)
                return false;
            else
                return true;
        }
    }
}
