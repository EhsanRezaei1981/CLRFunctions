// ReSharper disable once CheckNamespace
public static class WebService
{
    /// <summary>
    /// </summary>
    /// <param name="webServiceUrl"></param>
    /// <param name="headerXmlModel"></param>
    /// <param name="contentType">application/x-www-form-urlencoded</param>
    /// <param name="webMethod">POST,GET</param>
    /// <param name="timeout"></param>
    /// <param name="data"></param>
    /// <param name="queryString"></param>
    /// <param name="errorCode"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static void InvokeWebService(string webServiceUrl, string headerXmlModel, string contentType, string webMethod, int timeout, string data,
        string queryString, out int errorCode, out string result)
    {
        result = new CLRFunctions.WebService.InvokeWebServiceDynamically().Invoke(webServiceUrl, headerXmlModel, contentType,
            webMethod, timeout, data, queryString, out errorCode);
    }
}