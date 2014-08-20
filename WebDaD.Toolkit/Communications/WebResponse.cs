using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace WebDaD.Toolkit.Communications
{
    public class WebResponse
    {
        private int responseCodeInt;
        private HttpStatusCode responseCodeHttp;
        private string responseText;

        public int ResponseCode { get
        {
            if (responseCodeInt != null) return responseCodeInt;
            else
            {
                return (int)responseCodeHttp;
            }
        } }
        public HttpStatusCode HttpResponseCode
        {
            get
            {
                if (responseCodeHttp != null) return responseCodeHttp;
                else
                {
                    return (HttpStatusCode)responseCodeInt;
                }
            }
        }
        public string ReponseText { get { return this.responseText; } }

        public WebResponse(int responseCode, string responseText)
        {
            this.responseCodeInt = responseCode;
            this.responseText = responseText;
        }

        public WebResponse(HttpStatusCode responseCode, string responseText)
        {
            this.responseCodeHttp = responseCode;
            this.responseText = responseText;
        }
    }
}
