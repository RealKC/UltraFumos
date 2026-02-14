using HarmonyLib;
using UnityEngine;

namespace FumoSkull.Patches;

[HarmonyPatch(typeof(Torch), "Start")]
static class FumofiyTorch
{
    static void Prefix(Torch __instance)
    {
        if (FumoSkulls.Config.IsYuyukoDisabled)
        {
            return;
        }

        Renderer meshRenderer = __instance.gameObject.GetComponentInChildren<MeshRenderer>();
        if (meshRenderer)
        {
            meshRenderer.enabled = false;
            FumoSkulls.CreateFumo(
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
