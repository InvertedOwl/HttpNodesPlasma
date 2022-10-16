using Behavior;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityModManagerNet;
using PlasmaModding;

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
                string result = Main.client.GetStringAsync(GetProperty("Url").GetValueString()).GetAwaiter().GetResult();

                WriteOutput("Result", new Data(result));
            }
            catch (Exception e)
            {
                WriteOutput("Result", new Data("An error has occured"));
            }
        }
    }
}
