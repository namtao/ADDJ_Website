using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Xml;
using System.Security;
using System.Text;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
/// <summary>
/// Summary description for Utils
/// </summary>
public class Utils
{
    public Utils()
    {

    }

    public Boolean ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    public String getHTML(String myURL, String certPath, String post)
    {
        try
        {

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            HttpWebRequest getCategory;
            Uri myUri = new Uri(myURL);
            getCategory = (HttpWebRequest)WebRequest.Create(myUri);
            getCategory.CookieContainer = new CookieContainer();
            getCategory.Timeout = 1000 * 30;
            //Them vao cho scoreStand
            getCategory.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.1.4322)";

            if (certPath != "")
            {
                getCategory.AllowWriteStreamBuffering = true;
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateCertificate);
                X509Certificate x509 = X509Certificate.CreateFromCertFile
                    (certPath);
                getCategory.ClientCertificates.Add(x509);
            }
            if (post != "")
            {
                getCategory.Method = "POST";
                getCategory.ContentLength = post.Length;
                getCategory.ContentType = "application/x-www-form-urlencoded";
                StreamWriter swRequestWriter = new StreamWriter(getCategory.GetRequestStream());
                swRequestWriter.Write(post);
                swRequestWriter.Close();
            }
            HttpWebResponse res = (HttpWebResponse)getCategory.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream());
            String pageHTML = sr.ReadToEnd();
            res.Close();
            sr.Close();
            return pageHTML;
        }
        catch (Exception e2)
        {
            return e2.Message;
        }
    }



    public String getUserID(String CASHOST, String certPath, HttpResponse Response, HttpRequest Request)
    {
        try
        {
            String servicePath = Request.Url.GetLeftPart(UriPartial.Path);
            String casLogout = CASHOST + "logout?service=" + servicePath;
            String casLogin = CASHOST + "login?service=" + servicePath;

            // Look for the "ticket=" after the "?" in the URL
            string tkt = Request.QueryString["ticket"];
            // First time through there is no ticket=, so redirect to CAS login
            if (tkt == null || tkt.Length == 0)
            {
                Response.Redirect(casLogin);
                return "";
            }

            

            // Second time (back from CAS) there is a ticket= to validate
            string validateurl = CASHOST + "serviceValidate?" +
              "ticket=" + tkt + "&" +
              "service=" + servicePath;
            
            string resp = getHTML(validateurl, certPath, "");
            NameTable nt = new NameTable();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);
            XmlParserContext context = new XmlParserContext(null, nsmgr, null, XmlSpace.None);
            XmlTextReader reader = new XmlTextReader(resp, XmlNodeType.Element, context);

                       
            // A very dumb use of XML. Just scan for the "user". If it isn't there, its an error.
            String netid = "";
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    string tag = reader.LocalName;
                    if (tag == "user")
                    {
                        netid += reader.ReadString();

                    }
                }
            }
            reader.Close();
            return netid;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            return "";
        }
    }


    public HttpWebRequest GetRequestObject(string uri, string method)
    {

        HttpWebRequest webrequest;

        Uri _uri;

        // Initiate a new WebRequest to the given URI.

        _uri = new Uri(uri);

        webrequest = (HttpWebRequest)System.Net.WebRequest.Create(_uri);

        webrequest.CookieContainer = new CookieContainer();

        webrequest.Method = method;

        return webrequest;

    }

}
