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
using System.Security.Policy;

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

            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;

            using (var client = new HttpClient(handler))
            {


                var url = GetProperty("Url").GetValueString();
                var response = client.PostAsync(url, new FormUrlEncodedContent(jsonBody)).Result;


                Uri uri = new Uri(url);
                IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
                Dictionary<string, string> cookie = new Dictionary<string, string>();
                foreach (Cookie cook in responseCookies)
                    cookie[cook.Name] = cook.Value;

                // by calling .Result you are performing a synchronous call
                var responseContent = response.Content;

                // by calling .Result you are synchronously reading the result
                string responseString = responseContent.ReadAsStringAsync().Result;
                WriteOutput("Result", new Data(responseString));

                //IEnumerable<string> cookies = response.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

                WriteOutput("Headers", new Data(headersToJson(response.Headers)));
                WriteOutput("Cookies", new Data(JsonConvert.SerializeObject(cookie)));

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
                CookieContainer cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = cookies;

                using (var client = new HttpClient(handler))
                {
                    var url = GetProperty("Url").GetValueString();
                    var response = client.GetAsync(url).Result;

                    Uri uri = new Uri(url);
                    IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
                    Dictionary<string, string> cookie = new Dictionary<string, string>();
                    foreach (Cookie cook in responseCookies)
                        cookie[cook.Name] = cook.Value;

                    UnityModManager.Logger.Log("" + JsonConvert.SerializeObject(cookie));

                    // by calling .Result you are performing a synchronous call
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    WriteOutput("Result", new Data(responseString));

                    //IEnumerable<string> cookies = response.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

                    WriteOutput("Headers", new Data(headersToJson(response.Headers)));
                    WriteOutput("Cookies", new Data(JsonConvert.SerializeObject(cookie)));

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

            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;

            using (var client = new HttpClient(handler))
            {
                var url = GetProperty("Url").GetValueString();
                var response = client.PutAsync(url, new FormUrlEncodedContent(jsonBody)).Result;

                Uri uri = new Uri(url);
                IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
                Dictionary<string, string> cookie = new Dictionary<string, string>();
                foreach (Cookie cook in responseCookies)
                    cookie[cook.Name] = cook.Value;

                // by calling .Result you are performing a synchronous call
                var responseContent = response.Content;

                // by calling .Result you are synchronously reading the result
                string responseString = responseContent.ReadAsStringAsync().Result;
                WriteOutput("Result", new Data(responseString));

                //IEnumerable<string> cookies = response.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

                WriteOutput("Headers", new Data(headersToJson(response.Headers)));
                WriteOutput("Cookies", new Data(JsonConvert.SerializeObject(cookie)));

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

            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;

            using (var client = new HttpClient(handler))
            {
                var url = GetProperty("Url").GetValueString();
                var response = client.SendAsync(request).Result;

                Uri uri = new Uri(url);
                IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
                Dictionary<string, string> cookie = new Dictionary<string, string>();
                foreach (Cookie cook in responseCookies)
                    cookie[cook.Name] = cook.Value;

                // by calling .Result you are performing a synchronous call
                var responseContent = response.Content;

                // by calling .Result you are synchronously reading the result
                string responseString = responseContent.ReadAsStringAsync().Result;
                WriteOutput("Result", new Data(responseString));

                //IEnumerable<string> cookies = response.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

                WriteOutput("Headers", new Data(headersToJson(response.Headers)));
                WriteOutput("Cookies", new Data(JsonConvert.SerializeObject(cookie)));

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
                CookieContainer cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = cookies;

                using (var client = new HttpClient(handler))
                {
                    var url = GetProperty("Url").GetValueString();
                    var response = client.DeleteAsync(url).Result;

                    Uri uri = new Uri(url);
                    IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
                    Dictionary<string, string> cookie = new Dictionary<string, string>();
                    foreach (Cookie cook in responseCookies)
                        cookie[cook.Name] = cook.Value;

                    // by calling .Result you are performing a synchronous call
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    WriteOutput("Result", new Data(responseString));

                    //IEnumerable<string> cookies = response.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

                    WriteOutput("Headers", new Data(headersToJson(response.Headers)));
                    WriteOutput("Cookies", new Data(JsonConvert.SerializeObject(cookie)));

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