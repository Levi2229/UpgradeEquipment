using UpgradeEquipment.Items;
using Terraria;
using Terraria.ModLoader;
using System;

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

        // determines if it can roll at all.
        // use this to control if a prefixes can be rolled or not
        public override bool CanRoll(Item item)
        {
            if (item.melee)
            {
                return true;
            }
            return false;
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
            => item.GetGlobalItem<UpgradeEquipmentGlobalItem>().upgradeTier = _power;

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            float multiplier = 1f + 0.06f * _power;
            damageMult *= multiplier * 1.3f;
            useTimeMult = 1 - Convert.ToSingle(Math.Sqrt(multiplier)) / 8;
            shootSpeedMult *= multiplier / 2;
            critBonus = (int)_power;
            if (multiplier /3 < 1)
            {
                if (!disableSizeChange)
                {
                    scaleMult = 1;
                }
                if (!disableKnockbackChange)
                {
                    knockbackMult = 1;
                }
            }
            else
            {
                if (!disableKnockbackChange)
                {
                    knockbackMult *= multiplier / 3;
                }
                if (!disableSizeChange)
                {
                    scaleMult *= multiplier / 3;
                }
            }
          
        }

        public override void ModifyValue(ref float valueMult)
        {
            float multiplier = _power / 7f + 1f;
            valueMult = multiplier;
        }
    }
}
