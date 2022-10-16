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
            // Get Request
            addHttpNode("GET", typeof(HttpGetAgent), false);

            // Delete Request
            addHttpNode("DELETE", typeof(HttpDeleteAgent), false);

            // Post Request
            addHttpNode("POST", typeof(HttpPostAgent), true);

            // Put Request
            addHttpNode("PUT", typeof(HttpPutAgent), true);

            // Patch Request
            addHttpNode("PATCH", typeof(HttpPatchAgent), true);
        }

        public static void addHttpNode(string name, System.Type agent, bool payload)
        {
            AgentGestalt httpGestalt = (AgentGestalt)ScriptableObject.CreateInstance(typeof(AgentGestalt));
            httpGestalt.displayName = "HTTP " + name;
            httpGestalt.componentCategory = AgentGestalt.ComponentCategories.Behavior;
            httpGestalt.properties = new Dictionary<int, AgentGestalt.Property>();
            httpGestalt.ports = new Dictionary<int, AgentGestalt.Port>();
            httpGestalt.nodeCategory = AgentCategoryEnum.Misc;
            httpGestalt.type = AgentGestalt.Types.Logic;
            
            // Ports
            CustomNodeManager.CreateCommandPort(httpGestalt, name, "Executes a " + name + " request on the provided URL", 1);

            CustomNodeManager.CreatePropertyPort(httpGestalt, "Url", "Executes a " + name + " request on the provided URL", Data.Types.String, true, new Data(""));
            CustomNodeManager.CreatePropertyPort(httpGestalt, "Headers", "Headers for the request", Data.Types.String, true, new Data(""));
            
            if (payload)
                CustomNodeManager.CreatePropertyPort(httpGestalt, "Payload", "Payload for the request", Data.Types.String, true, new Data(""));


            CustomNodeManager.CreateOutputPort(httpGestalt, "Result", "Result of the request", Data.Types.String);

            httpGestalt.agent = agent;

            CustomNodeManager.CreateNode(httpGestalt, "HTTP " + name);
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
