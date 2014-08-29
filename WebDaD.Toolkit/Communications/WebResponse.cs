using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace WebDaD.Toolkit.Communications
{
    public class WebResponse
    {
        private HttpStatusCode responseCodeHttp;
        private string responseText;

        public int ResponseCode { get
        {
                return (int)responseCodeHttp;
            
        } }
        public HttpStatusCode HttpResponseCode
        {
            get
            {
                 return responseCodeHttp;
               
            }
        }
        public string ReponseText { get { return this.responseText; } }

        public WebResponse(HttpStatusCode responseCode, string responseText)
        {
            this.responseCodeHttp = responseCode;
            this.responseText = responseText;
        }
    }
}
