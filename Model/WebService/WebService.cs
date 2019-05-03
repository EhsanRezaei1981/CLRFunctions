using System;
using System.IO;
using System.Net;
using System.Security;
using System.Text;
using System.Xml;

namespace CLRFunctions.WebService
{
    public class InvokeWebServiceDynamically
    {
        public string Invoke(string webServiceUrl, string headerXmlModel, string contentType,
            string webMethod, int timeout,
            string data, string queryString, out int errorCode)
        {
            try
            {
                errorCode = 0;
                var result = "";
                headerXmlModel = "<root>" + headerXmlModel + "</root>";
                webMethod = string.IsNullOrWhiteSpace(webMethod) ? "POST" : webMethod;
                contentType = string.IsNullOrWhiteSpace(contentType)
                    ? "application/x-www-form-urlencoded"
                    : contentType;

                // prepare data
                data = data ?? "";
                var tmpData = Encoding.UTF8.GetBytes(data);
                var seperator = "?";
                if (webServiceUrl.IndexOf("?", StringComparison.Ordinal) != -1)
                {
                    seperator = "&";
                }
                // set webServvice URL , method , contentLenght ,contentType,
                var request =
                    (HttpWebRequest) WebRequest.Create(webServiceUrl +
                                                       (string.IsNullOrWhiteSpace(queryString)
                                                           ? ""
                                                           : seperator + queryString));
                request.Method = webMethod;
                request.ContentLength = tmpData.Length;
                request.ContentType = contentType;
                request.MaximumResponseHeadersLength = -1;
                // add xml header to request header
                var xml = new XmlDocument();
                xml.LoadXml(headerXmlModel); // suppose that myXmlString contains "<Names>...</Names>"
                var xmlNodeList = xml.SelectNodes("/root");
                //request.Credentials = CredentialCache.;
                if (xmlNodeList != null)
                    foreach (XmlNode xn in xmlNodeList)
                        for (var i = 0; i < xn.ChildNodes.Count; i++)
                        {
                            switch (xn.ChildNodes[i].Name.ToLower())
                            {
                                case "accept":
                                    request.Accept = xn.ChildNodes[i].InnerText;
                                    break;
                                case "referer":
                                    request.Referer = xn.ChildNodes[i].InnerText;
                                    break;
                                default:
                                    request.Headers.Add(xn.ChildNodes[i].Name, xn.ChildNodes[i].InnerText);
                                    break;
                            }
                        }
                if (timeout > 0)
                    request.Timeout = timeout;
                if (webMethod.ToLower() != "get")
                {
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(tmpData, 0, tmpData.Length);
                    }
                }
                var response = (HttpWebResponse) request.GetResponse();
                // call

                var getResponse = response.GetResponseStream();
                // prepare response 

                var reader = new StreamReader(getResponse).ReadToEnd();
                result = Unescape(reader);
                return result;

            }
            catch (Exception ex)
            {
                errorCode = -1;
                return ex.Message + (ex.InnerException != null ? ". InnerException:" + ex.InnerException.Message : "");
            }
        }

        public string Unescape(string s)

        {
            if (string.IsNullOrEmpty(s)) return s;
            var returnString = s;
            returnString = returnString.Replace("&apos;", "'");
            returnString = returnString.Replace("&quot;", "\"");
            returnString = returnString.Replace("&gt;", ">");
            returnString = returnString.Replace("&lt;", "<");
            returnString = returnString.Replace("&amp;", "&");
            return returnString;
        }

    }
}