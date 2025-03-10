﻿using BepInEx;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using System;

namespace FumoSkull;

enum Fumo
{
    Cirno,
    Reimu,
    Yuyuko,
    Koishi,
    Sakuya,
    Youmu,
    Mokou,
}

static class FumoExtensions
{
    public static string GameObjectName(this Fumo fumo) => fumo switch
    {
        Fumo.Yuyuko => "YuYuGO",
        _ => $"{fumo}GO",
    };
}

[BepInPlugin("UltraFumosTeam.UltraFumos", "UltraFumos", "1.3")]
public class FumoSkulls : BaseUnityPlugin
{
    static readonly Dictionary<Fumo, GameObject> allFumos = [];

    Harmony harmony;

    static AssetBundle fumoBundle;

    public void Awake()
    {

        var stream = typeof(FumoSkulls).Assembly.GetManifestResourceStream("fumoskulls");
        fumoBundle = AssetBundle.LoadFromStream(stream);
        fumoBundle.LoadAllAssets();

        harmony = new Harmony("UltraFumosTeam.UltraFumos");
        harmony.PatchAll();

        foreach (Fumo fumo in Enum.GetValues(typeof(Fumo)))
        {
            allFumos.Add(fumo, fumoBundle.LoadAsset<GameObject>(fumo.GameObjectName()));
        }
    }

    [HarmonyPatch(typeof(Skull), "Awake")]
    static class FumofiySkull
    {
        static void Postfix(Skull __instance)
        {
            ModifyMaterial modifyMaterial;
            try
            {
                modifyMaterial = Traverse.Create(__instance).Field<ModifyMaterial>("mod").Value;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to get `mod` field of skull: {e.GetType()} {e.Message}");
                return;
            }


            Renderer renderer;
            try
            {
                var traverse = Traverse.Create(modifyMaterial);
                traverse.Method("SetValues").GetValue();
                renderer = traverse.Field<Renderer>("rend").Value;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to get `rend` field of modifyMaterial: {e.GetType()} {e.Message}");
                return;
            }

            if (renderer)
            {
                Fumo type;
                Vector3 position;
                switch (__instance.GetComponent<ItemIdentifier>().itemType)
                {
                    case ItemType.SkullBlue:
                        type = Fumo.Cirno;
                        position = new Vector3(0.05f, 0.03f, 0.1f);
                        break;

                    case ItemType.SkullRed:
                        type = Fumo.Reimu;
                        position = new Vector3(-0.015f, 0, 0.15f);
                        break;

                    default:
                        return;
                }

                renderer.enabled = false;

                CreateFumo(
                    type,
                    renderer.transform,
                    position: position,
                    rotation: Quaternion.Euler(15, 0, 270),
                    scale: new Vector3(0.8f, 0.8f, 0.8f),
                    renderer.material.shader
                );
            }
            else
            {
                Debug.LogWarning("renderer was null");
            }
        }
    }

    [HarmonyPatch(typeof(Grenade), "Awake")]
    static class FumofiyRocket
    {
        static void Postfix(Grenade __instance)
        {
            if (__instance.rocket)
            {
                PatchRocket(__instance);
            }
            else
            {
                PatchCoreEject(__instance);
            }
        }



        static void PatchRocket(Grenade grenade)
        {
            Renderer[] meshRenderer = grenade.gameObject.GetComponentsInChildren<MeshRenderer>();
            if (meshRenderer.Length > 0)
            {
                for (int i = 0; i < meshRenderer.Length; i++)
                {
                    meshRenderer[i].enabled = false;
                }

                CreateFumo(Fumo.Sakuya, grenade.transform,
                    position: new Vector3(0f, 0f, 2f),
                    rotation: Quaternion.Euler(0, 0, 90),
                    scale: new Vector3(10f, 10f, 10f),
                    meshRenderer[0].material.shader
                );
            }
        }

        static void PatchCoreEject(Grenade grenade)
        {
            Renderer[] meshRenderer = grenade.gameObject.GetComponentsInChildren<MeshRenderer>();
            if (meshRenderer.Length > 0)
            {
                for (int i = 0; i < meshRenderer.Length; i++)
                {
                    meshRenderer[i].enabled = false;
                }

                CreateFumo(Fumo.Mokou, grenade.transform,
                    position: new Vector3(0f, -0.5f, 2f),
                    rotation: Quaternion.Euler(0, 0, 90),
                    scale: new Vector3(3.5f, 3.5f, 3.5f),
                    meshRenderer[0].material.shader
                );
            }
        }
    }

    [HarmonyPatch(typeof(Torch), "Start")]
    static class FumofiyTorch
    {
        static void Prefix(Torch __instance)
        {
            Renderer meshRenderer = __instance.gameObject.GetComponentInChildren<MeshRenderer>();
            if (meshRenderer)
            {
                meshRenderer.enabled = false;
                CreateFumo(
                    Fumo.Yuyuko,
                    meshRenderer.transform.parent.transform,
                    position: new Vector3(0, 0.1f, 0),
                    rotation: Quaternion.Euler(270, 270, 0),
                    scale: new Vector3(1, 1, 1) * 2.75f,
                    meshRenderer.material.shader
                );
            }
        }
    }

    [HarmonyPatch(typeof(Soap), "Start")]
    static class FumofiySoap
    {
        static void Prefix(Soap __instance)
        {
            Renderer masterSkull = __instance.gameObject.GetComponentInChildren<MeshRenderer>();
            if (masterSkull)
            {
                masterSkull.enabled = false;
                CreateFumo(
                    Fumo.Koishi,
                    masterSkull.transform.parent.transform,
                    position: new Vector3(0, 0.1f, 0),
                    rotation: Quaternion.Euler(270, 270, 0),
                    scale: new Vector3(1, 1, 1) * 2.75f,
                    masterSkull.material.shader
                );
            }
        }
    }

    [HarmonyPatch(typeof(Landmine), "Start")]
    static class FumofiyLandmine
    {
        static void Postfix(Landmine __instance)
        {
            var renderer = __instance.gameObject.GetComponentInChildren<MeshRenderer>();

            var lightCylinder = Traverse.Create(__instance).Field<GameObject>("lightCylinder").Value;
            var cylinderRenderer = lightCylinder.GetComponentInChildren<MeshRenderer>();
            cylinderRenderer.enabled = false;


            if (renderer)
            {
                renderer.enabled = false;
                CreateFumo(
                    Fumo.Youmu,
                    lightCylinder.transform,
                    position: new Vector3(0, 0, 0),
                    rotation: Quaternion.Euler(0, 270, 0),
                    scale: new Vector3(1, 1, 1) * 0.001f,
                    renderer.material.shader
                );
            }
        }
    }

    static void CreateFumo(Fumo fumoType, Transform masterSkull, Vector3 position, Quaternion rotation, Vector3 scale, Shader shader)
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
