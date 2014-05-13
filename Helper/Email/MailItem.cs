using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper.Email
{
    public class EmailItem
    {
        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bcc { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public string FromAccount { get; set; }
        public string FromShowName { get; set; }
        public string EncodingName { get; set; }
        public bool IsPretend { get; set; }
    }

    //public class MailServer
    //{

    //}

    public class EmailServer
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public void Send(EmailItem item)
        {
            EmailManager.Send(item, this);
        }
    }
}
