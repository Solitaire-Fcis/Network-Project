using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
        StatusCode code;
        List<string> headerLines = new List<string>();
        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {
            //throw new NotImplementedException();
            string Date = DateTime.Now.ToString();
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            string ResponseHeader;
            if (redirectoinPath!=null)
            {
                ResponseHeader = "Content-Type => " + contentType + "\r\n" + "Content-Length => " + content.Length + "\r\n" + "Date => " + Date + "\r\n" + "Location => " + redirectoinPath + "\r\n";
            }
            else
            {
                ResponseHeader = "Content-Type => " + contentType + "\r\n" + "Content-Length => " + content.Length + "\r\n" + "Date => " + Date + "\r\n";
            }


            // TODO: Create the response string
            switch (code)
            {
                case StatusCode.OK:
                    responseString = GetStatusLine(code) + " OK" + "\r\n" + ResponseHeader + "\r\n" + content;
                    break;
                case StatusCode.InternalServerError:
                    responseString = GetStatusLine(code) + " Internal Server Error" + "\r\n" + ResponseHeader + "\r\n" + content;
                    break;
                case StatusCode.NotFound:
                    responseString = GetStatusLine(code) + " Not Found" + "\r\n" + ResponseHeader + "\r\n" + content;
                    break;
                case StatusCode.BadRequest:
                    responseString = GetStatusLine(code) + " Bad Request" + "\r\n" + ResponseHeader + "\r\n" + content;
                    break;
                case StatusCode.Redirect:
                    responseString = GetStatusLine(code) + " Redirect" + "\r\n" + ResponseHeader + "\r\n" + content;
                    break;
                default:
                    responseString = "Something gone error in Switch or Response Class";
                    break;
            }
        }

        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
            string httpVersion = Configuration.ServerHTTPVersion;
            string statusLine = httpVersion+code.ToString();
            return statusLine;
        }
    }
}
