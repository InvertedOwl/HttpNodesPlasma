using Behavior;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace HttpNodes
{
    public class HttpPatchAgent : Agent
    {
        protected override void OnSetupFinished()
        {
            this._v1 = this._runtimeProperties[3];
            this._v2 = this._runtimeProperties[4];
            this._v3 = this._runtimeProperties[5];
        }

        [SketchNodePortOperation(1)]
        public void Patch(SketchNode node)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), this._v1.GetValue().stringValue);

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
                request.Headers.Add(k, jsonHeaders[k]);
            }
            UnityModManager.Logger.Log("About to send " + jsonBody.ToString() + " to " + this._v1.GetValue().stringValue);
            try
            {
                Main.client.SendAsync(request).GetAwaiter().GetResult();
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
