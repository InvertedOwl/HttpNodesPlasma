using Behavior;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;
using PlasmaModding;

namespace HttpNodes
{
    public class HttpDeleteAgent : CustomAgent
    {

        [SketchNodePortOperation(1)]
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
                Main.client.DeleteAsync(GetProperty("Url").GetValueString()).GetAwaiter().GetResult();

                WriteOutput("Result", new Data("200 OK"));
            }
            catch (Exception e)
            {
                WriteOutput("Result", new Data("An error has occured"));
            }
        }
    }
}
