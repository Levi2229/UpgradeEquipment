using UpgradeEquipment.Items;
using Terraria;
using Terraria.ModLoader;
using System;
using UpgradeEquipment.UI;

namespace UpgradeEquipment.Prefixes
{
    class UpgradeEquipmentPrefixRanged : ModPrefix
    {
        private readonly byte _power;
        Item _item;

        public UpgradeEquipmentPrefixRanged()
        {
        }

        public UpgradeEquipmentPrefixRanged(byte power)
        {
            _power = power;
        }

        // determines if it can roll at all.
        // use this to control if a prefixes can be rolled or not
        public override bool CanRoll(Item item)
        {
            _item = item;
            if (item.ranged)
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
                mod.AddPrefix("ranged +"+i, new UpgradeEquipmentPrefixRanged((byte)i));
            }
            return false;
        }

        public override void Apply(Item item)
            => item.GetGlobalItem<UpgradeEquipmentGlobalItem>().upgradeTier = _power;

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            float multiplier = 1f + 0.01f * _power;
            damageMult *= PrefixHelper.getDamageMult(_power);

            useTimeMult = 1 - Convert.ToSingle(Math.Sqrt(multiplier)) / 4;
            if (PrefixHelper.getVelocityMult(_power) > 1)
            {
                shootSpeedMult *= PrefixHelper.getVelocityMult(_power);
            }
            else
            {
                shootSpeedMult = 1;
            }
            if (PrefixHelper.getNegativeMult(_power) + 0.05f < 1f)
            {
                float negmult = PrefixHelper.getNegativeMult(_power);
                useTimeMult = negmult;
            }
            else
            {
                useTimeMult = 0.90f - (_power / 100f);
            }
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
