using BepInEx;
using BepInEx.Logging;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace FumoSkull
{
    [BepInPlugin("UltraFumosTeam.UltraFumos", "UltraFumos", "1.2")]
    public class FumoSkulls : BaseUnityPlugin
    {
        public static Dictionary<string, GameObject> allFumos = new Dictionary<string, GameObject>();

        Harmony fumo;

        public static AssetBundle fumoBundle;

        private void Awake()
        {
            using (var stream = typeof(FumoSkulls).Assembly.GetManifestResourceStream("FumoSkulls.Resources.fumoskulls"))
            {
                fumoBundle = AssetBundle.LoadFromStream(stream);
            }
            fumoBundle.LoadAllAssets();
            fumo = new Harmony("UltraFumosTeam.UltraFumos");
            fumo.PatchAll();
            allFumos.Add("Cirno", fumoBundle.LoadAsset<GameObject>("CirnoGO"));
            allFumos.Add("Reimu", fumoBundle.LoadAsset<GameObject>("ReimuGO"));
            allFumos.Add("YuYu", fumoBundle.LoadAsset<GameObject>("YuYuGO"));
            allFumos.Add("Koishi", fumoBundle.LoadAsset<GameObject>("KoishiGO"));
            allFumos.Add("Sakuya", fumoBundle.LoadAsset<GameObject>("SakuyaGO"));
        }

        [HarmonyPatch(typeof(Skull), "Start")]
        public static class FumofiySkull
        {
            public static void Prefix(Skull __instance)
            {
                Renderer renderer = __instance.gameObject.GetComponent<Renderer>();
                if (renderer)
                {
                    string fumoType;
                    Vector3 fumoPosition = new Vector3(0.15f, 0, 0.15f);
                    Quaternion fumoRotation = Quaternion.Euler(0, 20, 0);
                    Vector3 fumoScale = new Vector3(1.5f, 1.5f, 1.5f);
                    switch (__instance.GetComponent<ItemIdentifier>().itemType)
                    {
                        case ItemType.SkullBlue:
                            fumoType = "Cirno";
                            fumoPosition = new Vector3(0.05f, 0, 0.2f);
                            break;

                        case ItemType.SkullRed:
                            fumoType = "Reimu";
                            break;

                        default:
                            fumoType = "Reimu";
                            return;
                    }
                    renderer.enabled = false;
                    CreateFumo(fumoType, renderer.transform, fumoPosition, fumoRotation, fumoScale, renderer.material.shader);
                }
            }
        }

        [HarmonyPatch(typeof(Grenade), "Start")]
        public static class FumofiyRocket
        {
            public static void Postfix(Grenade __instance)
            {
                Renderer[] meshRenderer = __instance.gameObject.GetComponentsInChildren<MeshRenderer>();
                if (meshRenderer.Length > 0 && __instance.rocket)
                {
                    for (int i = 0; i < meshRenderer.Length; i++)
                    {
                        meshRenderer[i].enabled = false;
                    }
                    Vector3 fumoPosition = new Vector3(0f, 0f, 2f);
                    Quaternion fumoRotation = Quaternion.Euler(0, 0, 60);
                    Vector3 fumoScale = new Vector3(1f, 1f, 1f) * 10f;
                    CreateFumo("Sakuya", __instance.transform, fumoPosition, fumoRotation, fumoScale, meshRenderer[0].material.shader);
                }
            }
        }

        [HarmonyPatch(typeof(Torch), "Start")]
        public static class FumofiyTorch
        {
            public static void Prefix(Torch __instance)
            {
                Renderer meshRenderer = __instance.gameObject.GetComponentInChildren<MeshRenderer>();
                if (meshRenderer)
                {
                    Vector3 fumoPosition = new Vector3(0, 0.1f, 0);
                    Quaternion fumoRotation = Quaternion.Euler(270, 270, 0);
                    Vector3 fumoScale = new Vector3(1, 1, 1) * 2.75f;
                    string fumoType = "YuYu";
                    meshRenderer.enabled = false;
                    CreateFumo(fumoType, meshRenderer.transform.parent.transform, fumoPosition, fumoRotation, fumoScale, meshRenderer.material.shader);
                }
            }
        }

        [HarmonyPatch(typeof(Soap), "Start")]
        public static class FumofiySoap
        {
            public static void Prefix(Soap __instance)
            {
                Renderer masterSkull = __instance.gameObject.GetComponentInChildren<MeshRenderer>();
                if (masterSkull)
                {
                    Vector3 fumoPosition = new Vector3(0, 0.1f, 0);
                    Quaternion fumoRotation = Quaternion.Euler(270, 270, 0);
                    Vector3 fumoScale = new Vector3(1, 1, 1) * 2.75f;
                    string fumoType = "Koishi";
                    masterSkull.enabled = false;
                    CreateFumo(fumoType, masterSkull.transform.parent.transform, fumoPosition, fumoRotation, fumoScale, masterSkull.material.shader);
                }
            }
        }

        public static void CreateFumo(string fumoType, Transform masterSkull, Vector3 position, Quaternion rotation, Vector3 scale, Shader shader)
        {
            Debug.Log("Swapping " + masterSkull.name + " to " + fumoType);
            GameObject fumo = allFumos[fumoType];
            GameObject skullFumo = GameObject.Instantiate(fumo, masterSkull);
            skullFumo.SetActive(true);
            skullFumo.transform.localRotation = rotation;
            skullFumo.transform.localPosition = position;
            skullFumo.transform.localScale = scale;
            Renderer[] fumoRenderers = skullFumo.GetComponentsInChildren<Renderer>(includeInactive: true);
            foreach (Renderer render in fumoRenderers)
            {
                Material[] fumoMaterial = render.materials;
                foreach (Material mat in fumoMaterial)
                {
                    mat.shader = shader;
                }
            }
        }
    }
}
