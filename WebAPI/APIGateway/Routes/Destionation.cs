﻿using System.Text;

namespace APIGateway.Routes
{
    public class Destionation
    {
        public string Uri { get; set; }

        public bool RequiresAuthentication { get; set; }

        public Destionation(string uri, bool requiresAuthentication)
        {
            Uri = uri;
            RequiresAuthentication = requiresAuthentication;
        }

        public Destionation(string uri) 
            : this(uri, false) 
        {
        }

        private Destionation() 
        {
            Uri = "/";
            RequiresAuthentication = false;
        }

        private string CreateDestinationUri(HttpRequest request) 
        {
            string requestPath = request.Path.ToString();
            string queryString = request.QueryString.ToString();

            string endpoint = "";
            var endpointSplit = requestPath.Substring(1).Split('/');

            if (endpointSplit.Length > 1) 
            {
                endpoint = endpointSplit[1];
            }

            return Uri + endpoint + queryString;
        }

        public async Task<HttpResponseMessage> SendRequest(HttpRequest request) 
        {
            string requestContent;
            using (Stream recieveStream = request.Body) 
            {
                using (StreamReader readStream = new StreamReader(recieveStream, encoding: Encoding.UTF8)) 
                {
                    requestContent = readStream.ReadToEnd();
                }
            }

            HttpClient client = new HttpClient();
            HttpRequestMessage newRequest = new HttpRequestMessage(new HttpMethod(request.Method), CreateDestinationUri(request));
            HttpResponseMessage response = await client.SendAsync(newRequest);

            return response;
        }
    }
}
