using UpgradeEquipment.Items;
using Terraria;
using Terraria.ModLoader;
using System;
using UpgradeEquipment.UI;

namespace UpgradeEquipment.Prefixes
{
    class UpgradeEquipmentPrefixMelee : ModPrefix
    {
        private readonly byte _power;
        internal static bool disableSizeChange;
        internal static bool disableKnockbackChange;

        public UpgradeEquipmentPrefixMelee()
        {
        }

        public UpgradeEquipmentPrefixMelee(byte power)
        {
            _power = power;
        }

        // Allow multiple prefix autoloading this way (permutations of the same prefix)
        public override bool Autoload(ref string name)
        {
            if (!base.Autoload(ref name))
            {
                return false;
            }

            for (var i =1; i < 41; i++)
            {
                mod.AddPrefix("melee +"+i, new UpgradeEquipmentPrefixMelee((byte)i));
            }
            return false;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<UpgradeEquipmentGlobalItem>().upgradeTier = _power;
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            float multiplier = 1f + 0.01f * _power;
            damageMult *= PrefixHelper.getDamageMult(_power);

            if (PrefixHelper.getSpeedMult(_power) + 0.05f < 1f)
            {
                float negmult = PrefixHelper.getSpeedMult(_power);
                useTimeMult = negmult;
            }
            else
            {
                useTimeMult = 0.90f - (_power / 100f);
            }
            if (PrefixHelper.getVelocityMult(_power) > 1)
            {
                shootSpeedMult *= PrefixHelper.getVelocityMult(_power);
            }
            else
            {
                shootSpeedMult = 1;
            }
            critBonus = (int)_power;

            if (!disableKnockbackChange && _power > 1)
            {
                knockbackMult *= multiplier;
            }
            if (!disableSizeChange)
            {
                scaleMult *= multiplier;
            }
        }

        public override void ModifyValue(ref float valueMult)
        {
            float multiplier = _power / 7f + 1f;
            valueMult = multiplier;
        }
    }
}
