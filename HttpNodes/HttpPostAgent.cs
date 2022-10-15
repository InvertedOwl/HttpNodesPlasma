using Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;
using Newtonsoft.Json;

namespace HttpNodes
{
    public class HttpPostAgent : Agent
    {
        protected override void OnSetupFinished()
        {
            foreach (int id in this._runtimeProperties.Keys)  
            {
                UnityModManager.Logger.Log(id + ": " + this._runtimeProperties[id]);
            }

            this._v1 = this._runtimeProperties[3];
            this._v2 = this._runtimeProperties[4];
            this._v3 = this._runtimeProperties[5];

        }

        [SketchNodePortOperation(1)]
        public void Post(SketchNode node)
        {

                UnityModManager.Logger.Log("V1 " + _v1.GetValueString());
                UnityModManager.Logger.Log("V2 " + _v2.GetValueString());
                UnityModManager.Logger.Log("V3 " + _v3.GetValueString());

                var jsonBody = new Dictionary<string, string>();
                var jsonHeaders = new Dictionary<string, string>();
                if (_v2.GetValueString() != "")
                {
                    jsonHeaders = JsonConvert.DeserializeObject<Dictionary<string, string>>(_v2.GetValueString());
                }
                if (_v3.GetValueString() != "")
                {
                    jsonBody = JsonConvert.DeserializeObject<Dictionary<string, string>>(_v3.GetValueString());
                }
                if (_v1.GetValueString() == "")
                {
                    return;
                }

                foreach (var k in jsonHeaders.Keys)
                {
                    UnityModManager.Logger.Log("Adding to header " + k + ": " + jsonHeaders[k]);
                    Main.client.DefaultRequestHeaders.Add(k, jsonHeaders[k]);
                }
                UnityModManager.Logger.Log("About to send " + jsonBody.ToString() + " to " + this._v1.GetValue().stringValue);
            try
            {
                Main.client.PostAsync(this._v1.GetValue().stringValue, new FormUrlEncodedContent(jsonBody)).GetAwaiter().GetResult();
                node.ports.Values.Last().Commit(new Data());
            }
            catch (Exception e)
            {
                node.ports.Values.Last().Commit(new Data("Error 503 server unavailable " + e.ToString()));
            }
        }

        private AgentProperty _v1;
        private AgentProperty _v2;
        private AgentProperty _v3;



    }
}