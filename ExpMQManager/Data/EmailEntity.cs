﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class EmailEntity
    {
        public EmailEntity()
        {
            //Empty Constructor
        }

        public EmailEntity(int __mailQueueId, string __subject, string __contents)
        {
            this.mailQueueId = __mailQueueId;
            this.subject = __subject;
            this.contents = __contents;
        }

        private int _mailQueueId = 0;
        public int mailQueueId
        {
            get { return _mailQueueId; }
            set { _mailQueueId = value; }
        }

        private string _subject = "";
        public string subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        private string _contents = "";
        public string contents
        {
            get { return _contents; }
            set { _contents = value; }
        }

        private string _sender = "";
        public string sender
        {
            get 
            {
                if (_sender == "")
                    _sender = "ePicSupport@casusa.com";

                return _sender; 
            }
            set { _sender = value; }
        }

        private List<string> _receiver = default(List<string>);
        public List<string> receiver
        {
            get 
            {
                if (_receiver == default(List<string>))
                    _receiver = new List<string>();

                return _receiver; 
            }
        }
    }
}
