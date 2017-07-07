using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.Data;
using System.Data;

namespace ExpMQManager.DAL
{
    public class EmailDAC : BaseDAC
    {
        public EmailEntity GetEmailInfoDAC(int mailQueueId)
        {
            string strSql = "";
            strSql = @" SELECT Subject, Contents FROM Email_Notification_Queue WHERE SeqNo = {0}";
            strSql = string.Format(strSql, mailQueueId);

            EmailEntity emailEntity = GetEmailFromReader(mailQueueId, ExecuteReader(strSql));

            strSql = @" SELECT EmailAddr FROM Email_Notification_Receiver WHERE QueueId = {0}";
            strSql = string.Format(strSql, mailQueueId);

            return AddEmailReceiver(emailEntity, ExecuteReader(strSql));
        }

        protected EmailEntity GetEmailFromReader(int mailQueueId, IDataReader reader)
        {
            if (reader.Read())
            {
                EmailEntity emailEntity = new EmailEntity(
                    mailQueueId,
                    reader["Subject"].ToString().Trim(),
                    reader["Contents"].ToString().Trim()
                );

                reader.Close();
                reader.Dispose();
                disConnect_dbcn_ExcuteReader();

                return emailEntity;
            }
            else
            {
                EmailEntity dlvEntity = new EmailEntity();
                reader.Close();
                reader.Dispose();
                disConnect_dbcn_ExcuteReader();
                return dlvEntity;
            }
        }

        protected EmailEntity AddEmailReceiver(EmailEntity emailEntity, IDataReader reader)
        {
            while (reader.Read())
                emailEntity.receiver.Add(reader["EmailAddr"].ToString().Trim());
            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return emailEntity;
        }
    }
}
