using BepInEx;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using System;

namespace FumoSkull;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class FumoSkulls : BaseUnityPlugin
{
    static readonly Dictionary<Fumo, GameObject> allFumos = [];
    public new static readonly FumoConfig Config = new();

    Harmony harmony;

    public static AssetBundle FumoBundle;

    public void Awake()
    {
        var stream = typeof(FumoSkulls).Assembly.GetManifestResourceStream("fumoskulls");
        FumoBundle = AssetBundle.LoadFromStream(stream);
        FumoBundle.LoadAllAssets();

        Config.Awake();

        harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll();

        foreach (Fumo fumo in Enum.GetValues(typeof(Fumo)))
        {
            allFumos.Add(fumo, FumoBundle.LoadAsset<GameObject>(fumo.GameObjectName()));
        }
    }

    public static void CreateFumo(Fumo fumoType, Transform masterSkull, Vector3 position, Quaternion rotation, Vector3 scale, Shader shader)
    {
        Debug.Log("Swapping " + masterSkull.name + " to " + fumoType);

        GameObject fumo = allFumos[fumoType];

        GameObject skullFumo = Instantiate(fumo, masterSkull);
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
