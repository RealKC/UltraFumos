using System;
using HarmonyLib;
using UnityEngine;

namespace FumoSkull.Patches;

[HarmonyPatch(typeof(Skull), "Awake")]
static class FumofiySkull
{
    static void Postfix(Skull __instance)
    {
        var skullType = __instance.GetComponent<ItemIdentifier>().itemType;

        switch (skullType)
        {
            case ItemType.SkullBlue:
                if (FumoSkulls.Config.IsCirnoDisabled)
                {
                    return;
                }

                break;
            case ItemType.SkullRed:
                if (FumoSkulls.Config.IsReimuDisabled)
                {
                    return;
                }

                break;

            default:
                return;
        }

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
            switch (skullType)
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

            FumoSkulls.CreateFumo(
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
