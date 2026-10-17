using System.ComponentModel;

namespace Deminvincibility.Model;

public enum SecondChanceRestoreEnum
{
    [Description("无")]
    None,
    [Description("1 点生命")]
    OneHealth,
    [Description("部位半血")]
    Half,
    [Description("部位满血")]
    Full
}