using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.DAL;
using ExpMQManager.Data;

namespace ExpMQManager.BLL
{
    public class GenerateEmail : GenerateBase
    {
        public override string doBuildUp(string msgType, string subType, int mid, int flightSeq, int queueId)
        {
            //Do nothing
            return string.Empty;
        }

        public int sendEamil(int mailQueueId, string subType)
        {
            GenerateEmail email = new GenerateEmail();

            string receiverAddr = "";
            EmailEntity emailEntity = new EmailDAC().GetEmailInfoDAC(mailQueueId);

            if (emailEntity.receiver != null && emailEntity.receiver.Count() > 0)
            {

                foreach (string receiver in emailEntity.receiver)
                {
                    if (receiverAddr != "")
                        receiverAddr += ";";
                    receiverAddr += receiver;
                }
            }
            else
            {
                return -2;
            }

            // change mail format HTML => Plain text
            string emailBody = emailEntity.contents;
            bool needHTMLFormat = true;
            if (subType == null && subType.ToUpper() != "TTN" && subType.ToUpper() != "IAC" && subType.ToUpper() != "CCSF")
            {
                needHTMLFormat = false;
                emailBody = emailBody.Replace("</p>", Environment.NewLine);
                emailBody = emailBody.Replace("<br>", Environment.NewLine);
                emailBody = emailBody.Replace("<br >", Environment.NewLine);
                emailBody = emailBody.Replace("<br />", Environment.NewLine);
                emailBody = emailBody.Replace("\r\n\r\n", Environment.NewLine);

                emailBody = emailBody.Replace("&nbsp;", "");
                emailBody = System.Text.RegularExpressions.Regex.Replace(emailBody, "<[^>]*>", "");
            }

            if (ExpMQManager.baseMail.mailSend(receiverAddr, emailBody, emailEntity.subject, needHTMLFormat))
                return 0;
            else
                return -1;

        }
    }
}
