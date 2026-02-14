using HarmonyLib;
using UnityEngine;

namespace FumoSkull.Patches;

[HarmonyPatch(typeof(Soap), "Start")]
static class FumofiySoap
{
    static void Prefix(Soap __instance)
    {
        if (FumoSkulls.Config.IsKoishiDisabled)
        {
            return;
        }

        Renderer masterSkull = __instance.gameObject.GetComponentInChildren<MeshRenderer>();
        if (masterSkull)
        {
            masterSkull.enabled = false;
            FumoSkulls.CreateFumo(
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
