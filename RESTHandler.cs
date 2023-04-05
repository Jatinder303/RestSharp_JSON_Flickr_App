using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharp_JSON_Flickr_App
{
    public class RESTHandler
    {
        private string url;
        private RestResponse response;
        private RestRequest request;

        public RESTHandler() 
        {
            url = "";
        }

        public RESTHandler(string url_actual) 
        {
            url = url_actual;
            request = new RestRequest();    
        }

        public void AddParameter(string key, string value) 
        {
            if (request != null) 
            {
                request.AddParameter(key, value);       
            }
        }

        public Root ExecuteRequest()
        {
            var client  = new RestClient(url);
            response    = client.Execute(request);
            Root root = new Root();
            root = JsonConvert.DeserializeObject<Root>(response.Content);
            return root;
        }
    }
}
