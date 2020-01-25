using UpgradeEquipment.Items;
using Terraria;
using Terraria.ModLoader;
using System;

namespace UpgradeEquipment.Prefixes
{
    class UpgradeEquipmentPrefixMagic : ModPrefix
    {
        private readonly byte _power;

        public UpgradeEquipmentPrefixMagic()
        {
        }

        public UpgradeEquipmentPrefixMagic(byte power)
        {
            _power = power;
        }

        // determines if it can roll at all.
        // use this to control if a prefixes can be rolled or not
        public override bool CanRoll(Item item)
        {
            if (item.magic)
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
                mod.AddPrefix("magic +"+i, new UpgradeEquipmentPrefixMagic((byte)i));
            }
            return false;
        }

        public override void Apply(Item item)
            => item.GetGlobalItem<UpgradeEquipmentGlobalItem>().upgradeTier = _power;

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            float multiplier = 1f + 0.06f * _power;
            damageMult *= multiplier * 1.3f;
            useTimeMult = 1 - Convert.ToSingle(Math.Sqrt(multiplier)) /6;
            shootSpeedMult *= multiplier /2;
            critBonus = (int)_power;
            manaMult = 1 - Convert.ToSingle(Math.Sqrt(multiplier)) / 4;
        }

        public override void ModifyValue(ref float valueMult)
        {
            float multiplier = _power / 7f + 1f;
            valueMult = multiplier;
        }
    }
}
