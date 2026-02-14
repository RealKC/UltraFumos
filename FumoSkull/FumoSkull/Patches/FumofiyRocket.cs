using HarmonyLib;
using UnityEngine;

namespace FumoSkull.Patches;

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
        if (FumoSkulls.Config.IsSakuyaDisabled)
        {
            return;
        }

        Renderer[] meshRenderer = grenade.gameObject.GetComponentsInChildren<MeshRenderer>();
        if (meshRenderer.Length > 0)
        {
            for (int i = 0; i < meshRenderer.Length; i++)
            {
                meshRenderer[i].enabled = false;
            }

            FumoSkulls.CreateFumo(Fumo.Sakuya, grenade.transform,
                position: new Vector3(0f, 0f, 2f),
                rotation: Quaternion.Euler(0, 0, 90),
                scale: new Vector3(10f, 10f, 10f),
                meshRenderer[0].material.shader
            );
        }
    }

    static void PatchCoreEject(Grenade grenade)
    {
        if (FumoSkulls.Config.IsMokouDisabled)
        {
            return;
        }

        Renderer[] meshRenderer = grenade.gameObject.GetComponentsInChildren<MeshRenderer>();
        if (meshRenderer.Length > 0)
        {
            for (int i = 0; i < meshRenderer.Length; i++)
            {
                meshRenderer[i].enabled = false;
            }

            FumoSkulls.CreateFumo(Fumo.Mokou, grenade.transform,
                position: new Vector3(0f, -0.5f, 2f),
                rotation: Quaternion.Euler(0, 0, 90),
                scale: new Vector3(3.5f, 3.5f, 3.5f),
                meshRenderer[0].material.shader
            );
        }
    }
}
