using UpgradeEquipment.Items;
using Terraria;
using Terraria.ModLoader;
using System;
using UpgradeEquipment.UI;

namespace UpgradeEquipment.Prefixes
{
    class UpgradeEquipmentPrefixGeneric : ModPrefix
    {
        private readonly byte _power;

        public UpgradeEquipmentPrefixGeneric()
        {
        }

        public UpgradeEquipmentPrefixGeneric(byte power)
        {
            _power = power;
        }

        public override float RollChance(Item item)
        {
            return 0f;
        }

        // determines if it can roll at all.
        // use this to control if a prefixes can be rolled or not
        public override bool CanRoll(Item item)
        {
            return true;
        }

        // Allow multiple prefix autoloading this way (permutations of the same prefix)
        public override bool Autoload(ref string name)
        {
            if (!base.Autoload(ref name))
            {
                return false;
            }

            for (var i =1; i < 31; i++)
            {
                mod.AddPrefix("+"+i, new UpgradeEquipmentPrefixGeneric((byte)i));
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
            useTimeMult = 0.85f - _power / 100f;
            damageMult = 1.15f;
            shootSpeedMult = 1f + _power / 100f;

            critBonus = (int)_power;
        }

        public override void ModifyValue(ref float valueMult)
        {
            float multiplier = _power / 7f + 1f;
            valueMult = multiplier;
        }

        public override void ValidateItem(Item item, ref bool invalid)
        {
            invalid = false;
        }
    }
}
