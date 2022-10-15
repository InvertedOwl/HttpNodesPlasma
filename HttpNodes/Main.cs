using HarmonyLib;
using System.Reflection;
using UnityModManagerNet;
using System.Net.Http;
using System.Collections.Generic;
using UnityEngine;
using PlasmaModding;
using Behavior;
using System.Linq;

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
            AgentGestalt getGestalt = (AgentGestalt)ScriptableObject.CreateInstance(typeof(AgentGestalt));
            getGestalt.displayName = "Http Get";
            getGestalt.componentCategory = AgentGestalt.ComponentCategories.Behavior;
            getGestalt.properties = new Dictionary<int, AgentGestalt.Property>();
            getGestalt.ports = new Dictionary<int, AgentGestalt.Port>();
            getGestalt.nodeCategory = AgentCategoryEnum.Misc;
            getGestalt.type = AgentGestalt.Types.Logic;

            CustomNodeManager.CreateCommandPort(getGestalt, "Get", "Executes a GET request on the provided URL", 1);

            CustomNodeManager.CreatePropertyPort(getGestalt, "Url", "Executes a GET request on the provided URL", Data.Types.String, true, new Data(""));
            CustomNodeManager.CreatePropertyPort(getGestalt, "Headers", "Headers for the request", Data.Types.String, true, new Data(""));


            CustomNodeManager.CreateOutputPort(getGestalt, "Result", "Result of the request");

            getGestalt.agent = typeof(HttpGetAgent);

            CustomNodeManager.CreateNode(getGestalt, "Http Get");
            


            // Post Request
            AgentGestalt postGestalt = (AgentGestalt)ScriptableObject.CreateInstance(typeof(AgentGestalt));
            postGestalt.displayName = "Http Post";
            postGestalt.componentCategory = AgentGestalt.ComponentCategories.Behavior;
            postGestalt.properties = new Dictionary<int, AgentGestalt.Property>();
            postGestalt.ports = new Dictionary<int, AgentGestalt.Port>();
            postGestalt.nodeCategory = AgentCategoryEnum.Misc;
            postGestalt.type = AgentGestalt.Types.Logic;

            // Ports
            CustomNodeManager.CreateCommandPort(postGestalt, "POST", "Executes a POST request on the provided URL", 1);

            CustomNodeManager.CreatePropertyPort(postGestalt, "Url", "Executes a POST request on the provided URL", Data.Types.String, true, new Data(""));
            CustomNodeManager.CreatePropertyPort(postGestalt, "Headers", "Headers for the request", Data.Types.String, true, new Data(""));
            CustomNodeManager.CreatePropertyPort(postGestalt, "Payload", "Payload for the request", Data.Types.String, true, new Data(""));


            CustomNodeManager.CreateOutputPort(postGestalt, "Result", "Result of the request", Data.Types.String);

            postGestalt.agent = typeof(HttpPostAgent);

            CustomNodeManager.CreateNode(postGestalt, "Http Post");
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

        /*
        // Add Harmony patches down here!
        [HarmonyPatch(typeof(WorldController), "LoadDevice")]
        public class DevicePatch
        {
            public static bool Prefix(Device device, SerializedDevice serializedDevice, SerializedAssetsLibrary serializedAssetsLibrary, bool restoreRuntime, bool asyncLoad)
            {
                UnityModManager.Logger.Log("Loading Device.. with " + serializedDevice.agents.Count() + " agents");
                foreach (SerializedAgent a in serializedDevice.agents)
                {
                    if (a.agentId.displayName == "Http Get")
                    {
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                        Controllers.worldController.visor.OpenDialogBox("Warning", "The device about to be loaded contains HTTP nodes which may expose your IP address to external servers not associated with Plasma. Do you still want to load it?", "No", "Yes", null);
                        Controllers.worldController.PauseGame();
                        return false;


                    }
                    UnityModManager.Logger.Log(a.agentId + "");

                }
                return true;
            }
        }*/
    }
}
