using Behavior;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityModManagerNet;

namespace HttpNodes
{
    public class HttpGetAgent : Agent
    {
        protected override void OnSetupFinished()
        {
            foreach (int id in this._runtimeProperties.Keys)
            {
                UnityModManager.Logger.Log(id + ": " + this._runtimeProperties[id]);
            }

            this._v1 = this._runtimeProperties[3];
            this._v2 = this._runtimeProperties[4];

        }

        [SketchNodePortOperation(1)]
        public void Get(SketchNode node)
        {
            var jsonHeaders = new Dictionary<string, string>();
            if (_v2.GetValueString() != "")
            {
                jsonHeaders = JsonConvert.DeserializeObject<Dictionary<string, string>>(_v2.GetValueString());
            }
            foreach (var k in jsonHeaders.Keys)
            {
                UnityModManager.Logger.Log("Adding to header " + k + ": " + jsonHeaders[k]);
                Main.client.DefaultRequestHeaders.Add(k, jsonHeaders[k]);
            }

            try
            {
                string result = Main.client.GetStringAsync(this._v1.GetValueString()).GetAwaiter().GetResult();
                
                node.ports.Values.Last().Commit(new Data(result));
            }
            catch (Exception e)
            {
                node.ports.Values.Last().Commit(new Data("Error 503 server unavailable"));
            }
        }
        private AgentProperty _v1;
        private AgentProperty _v2;
    }
}
