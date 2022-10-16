using Behavior;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;
using PlasmaModding;

namespace HttpNodes
{
    public class HttpPatchAgent : CustomAgent
    {

        [SketchNodePortOperation(1)]
        public void Patch(SketchNode node)
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
            try
            {
                Main.client.SendAsync(request).GetAwaiter().GetResult();
                WriteOutput("Continue", new Data());
            }
            catch (Exception e)
            {
                WriteOutput("Continue", new Data("An error has occured"));
            }
        }
    }
}
