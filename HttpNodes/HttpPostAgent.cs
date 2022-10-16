using Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;
using Newtonsoft.Json;
using PlasmaModding;

namespace HttpNodes
{
    public class HttpPostAgent : CustomAgent
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
            try
            {
                Main.client.PostAsync(GetProperty("Url").GetValueString(), new FormUrlEncodedContent(jsonBody)).GetAwaiter().GetResult();
                WriteOutput("Continue", new Data());
            }
            catch (Exception e)
            {
                WriteOutput("Continue", new Data("An error has occured"));
            }
        }
    }
}