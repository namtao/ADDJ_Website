using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

using SolrNet;
using SolrNet.Utils;
using SolrNet.Exceptions;

namespace ADDJ.Entity
{
    /// <summary>
    /// Author : Phi Hoang Hai
    /// Created date : 19/04/2015
    /// Todo : Tạo class để request solr bằng phương thức Post
    /// </summary>
    public class PostSolrConnection : ISolrConnection
    {
        private readonly ISolrConnection conn;
        private readonly string serverUrl;

        public PostSolrConnection(ISolrConnection conn, string serverUrl)
        {
            this.conn = conn;
            this.serverUrl = serverUrl;
        }

        public string Post(string relativeUrl, string s)
        {
            return conn.Post(relativeUrl, s);
        }

        public string Get(string relativeUrl, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var u = new UriBuilder(serverUrl);
            u.Path += relativeUrl;
            var request = (HttpWebRequest)WebRequest.Create(u.Uri);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var qs = string.Join("&", parameters
                .Select(kv => string.Format("{0}={1}", HttpUtility.UrlEncode(kv.Key), HttpUtility.UrlEncode(kv.Value)))
                .ToArray());
            request.ContentLength = Encoding.UTF8.GetByteCount(qs);
            request.ProtocolVersion = HttpVersion.Version11;
            request.KeepAlive = true;
            try
            {
                using (var postParams = request.GetRequestStream())
                using (var sw = new StreamWriter(postParams))
                    sw.Write(qs);
                using (var response = request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                using (var sr = new StreamReader(responseStream, Encoding.UTF8, true))
                    return sr.ReadToEnd();
            }
            catch (WebException e)
            {
                throw new SolrConnectionException(e);
            }
        }

        public string PostStream(string relativeUrl, string contentType, System.IO.Stream content, IEnumerable<KeyValuePair<string, string>> getParameters)
        {
            return conn.PostStream(relativeUrl, contentType, content, getParameters);
        }
    }
}
