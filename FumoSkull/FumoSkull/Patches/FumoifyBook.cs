using HarmonyLib;
using UnityEngine;

namespace FumoSkull.Patches;

[HarmonyPatch(typeof(Readable), "Awake")]
class FumofiyBook
{
    static void Prefix(Readable __instance)
    {
        if (FumoSkulls.Config.IsPatchouliDisabled)
        {
            return;
        }


        var identifier = __instance.gameObject.GetComponentInChildren<ItemIdentifier>();

        if (identifier.name == "Book")
        {
            ReplaceBook(__instance);
        }
        else if (identifier.name == "BookTablet")
        {
            ReplaceTablet(__instance);
        }
    }

    static void ReplaceBook(Readable readable)
    {
        Renderer meshRenderer = readable.gameObject.GetComponentInChildren<MeshRenderer>();
        if (meshRenderer)
        {
            meshRenderer.enabled = false;
            FumoSkulls.CreateFumo(
                Fumo.Patchouli,
                meshRenderer.transform,
                position: new Vector3(1.75f, -0.1f, -1.5f),
                rotation: Quaternion.Euler(0, 180, 0),
                scale: new Vector3(1, 1, 1) * 2.5f,
                meshRenderer.material.shader
            );
        }
    }

    static void ReplaceTablet(Readable readable)
    {
        var cube = readable.gameObject.transform.GetChild(0);

        Renderer meshRenderer = cube.gameObject.GetComponentInChildren<MeshRenderer>();
        if (meshRenderer)
        {
            meshRenderer.enabled = false;
            FumoSkulls.CreateFumo(
                Fumo.Patchouli,
                meshRenderer.transform,
                position: new Vector3(1f, -0.1f, -1f),
                rotation: Quaternion.Euler(0, 180, 0),
                scale: new Vector3(1, 1, 1) * 2.5f,
                meshRenderer.material.shader
            );
        }
    }
}
