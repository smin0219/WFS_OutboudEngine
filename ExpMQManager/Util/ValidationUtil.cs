using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Util
{
    public class ValidationUtil
    {
        public static bool isThereSitaReciever(string msg)
        {
            bool result = false;

            try
            {
                string[] stringSeparators = new string[] { "\r\n" };
                string text = msg;
                string[] lines = text.Split(stringSeparators, StringSplitOptions.None);

                if (lines.Length > 0)
                {
                    string sita = lines[1];
                    sita = sita.Replace("QD", "");
                    sita = sita.Replace(" ", "");
                    if (sita.Length > 0)
                    {
                        result = true;
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
            
        }

    }
}
