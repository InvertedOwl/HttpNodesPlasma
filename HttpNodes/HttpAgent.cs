using Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityModManagerNet;
using Newtonsoft.Json;
using PlasmaModding;

namespace HttpNodes
{
    public class HttpAgent : CustomAgent
    {
        [SketchNodePortOperation(1)]
        public void Post(SketchNode node)
        {
            var jsonBody = new Dictionary<string, string>();
            var jsonHeaders = new Dictionary<string, string>();
            if (GetProperty("Headers").GetValueString() != "")
            {
                jsonHeaders = JsonConvert.DeserializeObject<Dictionary<string, string>>(GetProperty("Headers").GetValueString());
            }
            if (GetProperty("Payload").GetValueString() != "")
            {
                jsonBody = JsonConvert.DeserializeObject<Dictionary<string, string>>(GetProperty("Payload").GetValueString());
            }
            if (GetProperty("Url").GetValueString() == "")
            {
                return;
            }

            foreach (var k in jsonHeaders.Keys)
            {
                UnityModManager.Logger.Log("Adding to header " + k + ": " + jsonHeaders[k]);
                Main.client.DefaultRequestHeaders.Add(k, jsonHeaders[k]);
            }
            
            using (var client = new HttpClient())
            {
                var url = GetProperty("Url").GetValueString();
                var response = client.PostAsync(url, new FormUrlEncodedContent(jsonBody)).Result;
                    


                // by calling .Result you are performing a synchronous call
                var responseContent = response.Content;

                // by calling .Result you are synchronously reading the result
                string responseString = responseContent.ReadAsStringAsync().Result;
                WriteOutput("Result", new Data(responseString));

                //IEnumerable<string> cookies = response.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

                WriteOutput("Headers", new Data(headersToJson(response.Headers)));
            }

        }
        
        
        [SketchNodePortOperation(2)]
        public void Get(SketchNode node)
        {
            var jsonHeaders = new Dictionary<string, string>();
            if (GetProperty("Headers").GetValueString() != "")
            {
                jsonHeaders = JsonConvert.DeserializeObject<Dictionary<string, string>>(GetProperty("Headers").GetValueString());
            }
            foreach (var k in jsonHeaders.Keys)
            {
                UnityModManager.Logger.Log("Adding to header " + k + ": " + jsonHeaders[k]);
                Main.client.DefaultRequestHeaders.Add(k, jsonHeaders[k]);
            }

            try
            {
                using (var client = new HttpClient())
                {
                    var url = GetProperty("Url").GetValueString();
                    var response = client.GetAsync(url).Result;
                    


                    // by calling .Result you are performing a synchronous call
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    WriteOutput("Result", new Data(responseString));

                    //IEnumerable<string> cookies = response.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

                    WriteOutput("Headers", new Data(headersToJson(response.Headers)));
                }
            }
            catch (Exception e)
            {
                WriteOutput("Result", new Data("An error has occured " + e.ToString()));
            }
        }
        
        [SketchNodePortOperation(3)]
        public void Put(SketchNode node)
        {
            var jsonBody = new Dictionary<string, string>();
            var jsonHeaders = new Dictionary<string, string>();
            if (GetProperty("Headers").GetValueString() != "")
            {
                jsonHeaders = JsonConvert.DeserializeObject<Dictionary<string, string>>(GetProperty("Headers").GetValueString());
            }
            if (GetProperty("Payload").GetValueString() != "")
            {
                jsonBody = JsonConvert.DeserializeObject<Dictionary<string, string>>(GetProperty("Payload").GetValueString());
            }
            if (GetProperty("Url").GetValueString() == "")
            {
                return;
            }

            foreach (var k in jsonHeaders.Keys)
            {
                UnityModManager.Logger.Log("Adding to header " + k + ": " + jsonHeaders[k]);
                Main.client.DefaultRequestHeaders.Add(k, jsonHeaders[k]);
            }
            
            using (var client = new HttpClient())
            {
                var url = GetProperty("Url").GetValueString();
                var response = client.PutAsync(url, new FormUrlEncodedContent(jsonBody)).Result;
                    


                // by calling .Result you are performing a synchronous call
                var responseContent = response.Content;

                // by calling .Result you are synchronously reading the result
                string responseString = responseContent.ReadAsStringAsync().Result;
                WriteOutput("Result", new Data(responseString));

                //IEnumerable<string> cookies = response.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

                WriteOutput("Headers", new Data(headersToJson(response.Headers)));
            }

        }
        
        [SketchNodePortOperation(4)]
        public void Patch (SketchNode node)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), GetProperty("Url").GetValueString());

            
            var jsonBody = new Dictionary<string, string>();
            var jsonHeaders = new Dictionary<string, string>();
            if (GetProperty("Headers").GetValueString() != "")
            {
                jsonHeaders = JsonConvert.DeserializeObject<Dictionary<string, string>>(GetProperty("Headers").GetValueString());
            }
            if (GetProperty("Payload").GetValueString() != "")
            {
                jsonBody = JsonConvert.DeserializeObject<Dictionary<string, string>>(GetProperty("Payload").GetValueString());
            }
            if (GetProperty("Url").GetValueString() == "")
            {
                return;
            }

            foreach (var k in jsonHeaders.Keys)
            {
                request.Headers.Add(k, jsonHeaders[k]);
            }

            request.Content = new FormUrlEncodedContent(jsonBody);
            
            using (var client = new HttpClient())
            {
                var response = client.SendAsync(request).Result;
                    


                // by calling .Result you are performing a synchronous call
                var responseContent = response.Content;

                // by calling .Result you are synchronously reading the result
                string responseString = responseContent.ReadAsStringAsync().Result;
                WriteOutput("Result", new Data(responseString));

                //IEnumerable<string> cookies = response.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

                WriteOutput("Headers", new Data(headersToJson(response.Headers)));
            }
        }
        
        [SketchNodePortOperation(5)]
        public void Delete(SketchNode node)
        {
            var jsonHeaders = new Dictionary<string, string>();
            if (GetProperty("Headers").GetValueString() != "")
            {
                jsonHeaders = JsonConvert.DeserializeObject<Dictionary<string, string>>(GetProperty("Headers").GetValueString());
            }
            foreach (var k in jsonHeaders.Keys)
            {
                UnityModManager.Logger.Log("Adding to header " + k + ": " + jsonHeaders[k]);
                Main.client.DefaultRequestHeaders.Add(k, jsonHeaders[k]);
            }

            try
            {
                using (var client = new HttpClient())
                {
                    var url = GetProperty("Url").GetValueString();
                    var response = client.DeleteAsync(url).Result;
                    


                    // by calling .Result you are performing a synchronous call
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    WriteOutput("Result", new Data(responseString));

                    //IEnumerable<string> cookies = response.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

                    WriteOutput("Headers", new Data(headersToJson(response.Headers)));
                }
            }
            catch (Exception e)
            {
                WriteOutput("Result", new Data("An error has occured " + e.ToString()));
            }
        }

        public static string headersToJson (HttpResponseHeaders headers)
        {
            Dictionary<string, string> json = new Dictionary<string, string>();
            foreach (var header in headers)
            {
                json[header.Key] = header.Value.First();
            }

            return JsonConvert.SerializeObject(json);
        }
    }
}