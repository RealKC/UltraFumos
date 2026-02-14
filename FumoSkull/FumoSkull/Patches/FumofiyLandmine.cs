using HarmonyLib;
using UnityEngine;

namespace FumoSkull.Patches;

[HarmonyPatch(typeof(Landmine), "Start")]
static class FumofiyLandmine
{
    static void Postfix(Landmine __instance)
    {
        if (FumoSkulls.Config.IsYoumuDisabled)
        {
            return;
        }

        var renderer = __instance.gameObject.GetComponentInChildren<MeshRenderer>();

        var lightCylinder = Traverse.Create(__instance).Field<GameObject>("lightCylinder").Value;
        var cylinderRenderer = lightCylinder.GetComponentInChildren<MeshRenderer>();
        cylinderRenderer.enabled = false;


        if (renderer)
        {
            renderer.enabled = false;
            FumoSkulls.CreateFumo(
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
