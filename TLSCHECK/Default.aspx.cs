using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TLSCHECK
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private bool SendReqeust(string Url, SecurityProtocolType protocolType, StringBuilder Logging)
        {
            bool TLSDisabled = false;
            string Detail = "";
            try
            {
                try
                {
                    Logging.Append("<BR><B>");
                    Logging.Append(protocolType.ToString());
                    Logging.Append(" : ");
                    ServicePointManager.SecurityProtocol = protocolType;
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(TextBox1.Text);
                    request.GetResponse();
                }
                catch (WebException webex)
                {
                    if ((webex.InnerException != null) && (webex.InnerException is System.IO.IOException))
                    {
                        if ((webex.InnerException.InnerException != null) && (webex.InnerException.InnerException is System.Net.Sockets.SocketException))
                        {
                            TLSDisabled = true;
                        }
                    }
                    Detail = webex.ToString();
                }
            }
            finally
            {
                // OUTPUT
                if (TLSDisabled) {
                    Logging.Append("Disabled");
                }
                else
                {
                    Logging.Append("Enabled");
                }
                Logging.AppendLine("</B><BR>");
                Logging.AppendLine(Detail);
                Logging.AppendLine("<BR>");
            }
            return TLSDisabled;
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            StringBuilder Logging = new StringBuilder();
            Logging.Clear();

            Label1.Text = "";
            if ((SendReqeust(TextBox1.Text, SecurityProtocolType.Tls, Logging)) && 
                (SendReqeust(TextBox1.Text, SecurityProtocolType.Tls11, Logging)) &&
                (!SendReqeust(TextBox1.Text, SecurityProtocolType.Tls12, Logging)))
            {
                Label1.Text = "<font color='RED'>TLS 1.0 & 1.1 BEING DISABLED<b></b></font><BR>";
            }
            Label1.Text += Logging.ToString();


        }
    }
}