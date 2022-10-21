using HarmonyLib;
using System.Reflection;
using UnityModManagerNet;
using System.Net.Http;
using System.Collections.Generic;
using UnityEngine;
using PlasmaModding;
using Behavior;
using System.Linq;
using Sirenix.Serialization;
using System.IO;
using Visor;
using System.Threading.Tasks;

namespace HttpNodes
{
#if DEBUG
	[EnableReloading]
#endif
    public static class Main
    {
        static Harmony harmony;
        public static readonly HttpClient client = new HttpClient();

        public static bool Load(UnityModManager.ModEntry entry)
        {
            InitNodes();

            harmony = new Harmony(entry.Info.Id);

            entry.OnToggle = OnToggle;
#if DEBUG
            entry.OnUnload = OnUnload;
            
#endif
             
            return true;
        }


        public static void InitNodes()
        {
            AgentCategoryEnum httpCategory = CustomNodeManager.CustomCategory("Networking");

            AgentGestalt httpnode = CustomNodeManager.CreateGestalt(typeof(HttpAgent), "HTTP", "Executes a http request with the given requirements", httpCategory);

            string name = "HTTP";
            CustomNodeManager.CreateCommandPort(httpnode, "POST", "Executes a POST request on the provided URL", 1);
            CustomNodeManager.CreateCommandPort(httpnode, "GET", "Executes a GET request on the provided URL", 2);
            CustomNodeManager.CreateCommandPort(httpnode, "PUT", "Executes a PUT request on the provided URL", 3);
            CustomNodeManager.CreateCommandPort(httpnode, "PATCH", "Executes a PATCH request on the provided URL", 4);
            CustomNodeManager.CreateCommandPort(httpnode, "DELETE", "Executes a DELETE request on the provided URL", 5);
            
            CustomNodeManager.CreatePropertyPort(httpnode, "Url", "Executes a " + name + " request on the provided URL", Data.Types.String, true, new Data(""));
            CustomNodeManager.CreatePropertyPort(httpnode, "Headers", "Headers for the request", Data.Types.String, true, new Data(""));
            CustomNodeManager.CreatePropertyPort(httpnode, "Payload", "Payload for the request", Data.Types.String, true, new Data(""));
            CustomNodeManager.CreateOutputPort(httpnode, "Result", "Result of the request", Data.Types.String);
            CustomNodeManager.CreateOutputPort(httpnode, "Headers", "Headers of the response", Data.Types.String);
            CustomNodeManager.CreateOutputPort(httpnode, "Cookies", "Cookies of the response", Data.Types.String);
            CustomNodeManager.CreateNode(httpnode, name);

        }

        static bool OnToggle(UnityModManager.ModEntry entry, bool active)
        {
            if (active)
            {
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            else
            {
                harmony.UnpatchAll(entry.Info.Id);
            }

            return true;
        }

#if DEBUG
		static bool OnUnload(UnityModManager.ModEntry entry) {
			return true;
		}
#endif

        // This is for the warning message when you run a device with http nodes
        // I cannot get it to release the mouse. So for now it is disabled
        
        /*
        [HarmonyPatch(typeof(Device), "ToggleState")]
        public class DevicePatch
        {
            public static bool Prefix(ComponentHandler origin, Device __instance, bool quietly = false)
            {
                if (__instance.state == Device.State.Solid)
                {
                    return true;
                }

                UnityModManager.Logger.Log("Loading Device.. with " + Controllers.worldController.targetDevice.agents.Count() + " agents");
                foreach (Agent a in Controllers.worldController.targetDevice.agents)
                {
                    if (a.agentId.displayName == "HTTP GET")
                    {

                        // Doesnt unlock mouse or pause game, two things that it should be doing.
                        // I think its because the player is holding on to a device at this point and you can free your mouse when the player has a 

                        Controllers.worldController.PauseGame();
                        Controllers.worldController.visor.OpenDialogBox("Warning", "The sketch about to be run contains HTTP nodes which could expose your IP address to external servers not associated with Plasma. Do you wish to continue?", "No", "Yes", Main.HandleVisorResponse, true, false, 0);
                    

                        return false;

                    }
                }

                return true;
            }
        }*/
    }
}
