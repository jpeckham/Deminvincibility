using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using Deminvincibility.Model;
using Deminvincibility.Patches;

namespace Deminvincibility
{
    [BepInPlugin("com.hazelify.deminvincibility", "Deminvincibility", "3.0.0")]
    public class DeminvicibilityPlugin : BasePlugin
    {
        // private static string credits = "Thanks Props <3 Ily https://github.com/dvize/DadGamerMode";

        public static ConfigEntry<bool> Keep1Health { get; private set; }
        public static ConfigEntry<bool> Allow0HpLimbs { get; private set; }
        public static ConfigEntry<bool> AllowBlacking { get; private set; }
        public static ConfigEntry<bool> AllowBlackingHeadAndThorax { get; private set; }
        public static ConfigEntry<bool> MedicineBool { get; private set; }
        public static ConfigEntry<int> CustomDamageModeVal { get; private set; }
        public static ConfigEntry<bool> SecondChanceProtection { get; private set; }
        public static ConfigEntry<bool> SecondChanceEffectRemoval { get; private set; }
        public static ConfigEntry<SecondChanceRestoreEnum> SecondChanceHealthRestoreAmount { get; private set; }

        public override void Load()
        {
            InitConfig();
            ApplyPatches();
        }

        private void ApplyPatches()
        {
            new DestroyBodyPartPatch().Enable();
            new ApplyDamage().Enable();
            new ChangeHealth().Enable();
            new DoFracture().Enable();
            new Kill().Enable();
        }

        private void InitConfig()
        {
            // 1. 生命值
            Keep1Health = Config.Bind("1. 生命值", "1 点生命模式", false,
                new ConfigDescription("启用后你不会被击杀，生命值最低保留在设定值。",
                    null,
                    new ConfigurationManagerAttributes { IsAdvanced = false, Order = 7 }));

            Allow0HpLimbs = Config.Bind("1. 生命值", "允许四肢降至 0 点生命", false,
                new ConfigDescription("启用后，在开启「1 点生命模式」时，手臂和腿部可以降到 0 点生命。",
                    null,
                    new ConfigurationManagerAttributes { IsAdvanced = false, Order = 6 }));

            AllowBlacking = Config.Bind("1. 生命值", "允许四肢变黑", false,
                new ConfigDescription("启用后，在开启「1 点生命模式」时，四肢降到 0 点生命会被打黑。",
                    null,
                    new ConfigurationManagerAttributes { IsAdvanced = false, Order = 5 }));

            AllowBlackingHeadAndThorax = Config.Bind("1. 生命值", "允许头部和胸腔变黑", false,
                new ConfigDescription("启用后，头部和胸腔降到 0 点生命会被打黑。若「允许四肢变黑」未开启，本项会被忽略。",
                    null,
                    new ConfigurationManagerAttributes { IsAdvanced = false, Order = 4 }));

            MedicineBool = Config.Bind("1. 生命值", "忽略生命值负面效果", false,
                new ConfigDescription("启用后，骨折、流血等生命值负面效果会被自动治疗。\n\n提示：关闭此项可能产生意料之外的效果，请谨慎使用。",
                    null,
                    new ConfigurationManagerAttributes { IsAdvanced = false, Order = 3 }));

            CustomDamageModeVal = Config.Bind("1. 生命值", "受到伤害百分比", 100, new ConfigDescription(
                "设置实际受到的伤害百分比。",
                new AcceptableValueRange<int>(1, 99),
                new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = true, Order = 2 }));

            // 2. 死亡
            SecondChanceProtection = Config.Bind("2. 死亡", "启用第二次机会保护", false,
                new ConfigDescription("启用后，当你受到本应致命的伤害时，会获得一次免死机会。",
                    null,
                    new ConfigurationManagerAttributes { IsAdvanced = false, Order = 3 }));

            SecondChanceEffectRemoval = Config.Bind("2. 死亡", "第二次机会：移除生命值负面效果", true,
                new ConfigDescription("启用后，第二次机会触发时会移除角色身上所有生命值负面效果（流血、骨折等）。",
                    null,
                    new ConfigurationManagerAttributes { IsAdvanced = false, Order = 2 }));

            SecondChanceHealthRestoreAmount = Config.Bind("2. 死亡",
                "第二次机会：关键部位恢复的生命值", SecondChanceRestoreEnum.OneHealth,
                new ConfigDescription(
                    "选择第二次机会触发时，头部和胸腔应被恢复到的生命值。若该部位当前生命值高于设定值，则不会扣血。\n\n若「移除生命值负面效果」已关闭，「无」和「1 点生命」选项风险较高。",
                    null,
                    new ConfigurationManagerAttributes { IsAdvanced = false, Order = 1 }));
        }
    }
}
