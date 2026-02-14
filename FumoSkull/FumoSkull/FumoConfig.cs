using System;
using System.Collections.Generic;
using PluginConfig.API;
using PluginConfig.API.Fields;
using UnityEngine;

namespace FumoSkull;

public class FumoConfig
{
    private PluginConfigurator config;

    private BoolField cirno;
    private BoolField reimu;
    private BoolField yuyuko;
    private BoolField koishi;
    private BoolField sakuya;
    private BoolField youmu;
    private BoolField mokou;

    public FumoConfig()
    {
    }

    public void Awake()
    {
        config = PluginConfigurator.Create(MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_GUID);

        var texture = FumoSkulls.FumoBundle.LoadAsset<Texture2D>("icon.png");
        config.image = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

        cirno = new BoolField(config.rootPanel, "Replace blue skulls with Cirno", "skull.blue.cirno", true);
        reimu = new BoolField(config.rootPanel, "Replace red skulls with Reimu", "skull.red.reimu", true);
        yuyuko = new BoolField(config.rootPanel, "Replace torches with Yuyuko", "torch.yuyuko", true);
        koishi = new BoolField(config.rootPanel, "Replace soap with Koishi", "soap.koishi", true);
        sakuya = new BoolField(config.rootPanel, "Replace rockets with Sakuya", "grenade.rocket.sakuya", true);
        youmu = new BoolField(config.rootPanel, "Replace mines with Youmu", "mine.youmu", true);
        mokou = new BoolField(config.rootPanel, "Replace core eject with Mokou", "grenade.core_eject.mokou", true);
    }

    public bool IsCirnoDisabled
    {
        get => !cirno.value;
    }

    public bool IsReimuDisabled
    {
        get => !reimu.value;
    }

    public bool IsYuyukoDisabled
    {
        get => !yuyuko.value;
    }

    public bool IsKoishiDisabled
    {
        get => !koishi.value;
    }

    public bool IsSakuyaDisabled
    {
        get => !sakuya.value;
    }

    public bool IsYoumuDisabled
    {
        get => !youmu.value;
    }

    public bool IsMokouDisabled
    {
        get => !mokou.value;
    }
}
