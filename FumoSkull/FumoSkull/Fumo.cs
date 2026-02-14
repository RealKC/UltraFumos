namespace FumoSkull;

public enum Fumo
{
    Cirno,
    Reimu,
    Yuyuko,
    Koishi,
    Sakuya,
    Youmu,
    Mokou,
}

public static class FumoExtensions
{
    public static string GameObjectName(this Fumo fumo) => fumo switch
    {
        Fumo.Yuyuko => "YuYuGO",
        _ => $"{fumo}GO",
    };
}
