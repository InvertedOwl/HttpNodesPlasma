using Behavior;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityModManagerNet;
using PlasmaModding;
using System.Threading.Tasks;
using System.Net.Http;
using NodeCanvas.Tasks.Actions;
using System.Net;
using Newtonsoft.Json.Linq;

namespace HttpNodes
{
    public class HttpGetAgent : CustomAgent
    {

        [SketchNodePortOperation(1)]
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

                    WriteOutput("Headers", new Data(response.Headers.ToString()));
                }
            }
            catch (Exception e)
            {
                WriteOutput("Result", new Data("An error has occured " + e.ToString()));
            }
        }
    }
}
