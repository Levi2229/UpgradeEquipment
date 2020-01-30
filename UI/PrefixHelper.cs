using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using static Terraria.ModLoader.ModContent;
using UpgradeEquipment.Prefixes;

namespace UpgradeEquipment.UI
{
    class PrefixHelper
    {
        public static bool reducedValues;
        public static float opBonus = 0f;

        public PrefixHelper()
        {

        }

        public static WeaponUpgraderPrefix FindCurrentPrefix(string itemNamePrefixed, string itemNameNoPrefix)
        {
            string prefix = itemNamePrefixed.Replace(itemNameNoPrefix, "");

            int upgradeLevel = getCurrentUpgradeLevel(prefix);
            int price = determinePriceForNextUpgrade(upgradeLevel);
            return new WeaponUpgraderPrefix(prefix, true, price);
        }

        private static int determinePriceForNextUpgrade(int upgradeLevel)
        {
            int total = 1;
            total = (int)(upgradeLevel + Convert.ToSingle((Math.Pow(upgradeLevel, 2))));
            if(upgradeLevel > 10)
            {
                total += (upgradeLevel - 10) * 40;
            }
            if (upgradeLevel > 20)
            {
                total += (upgradeLevel - 20) * 120;
            }
            if (upgradeLevel > 30)
            {
                total += (upgradeLevel - 30) * 250;
            }
            if (reducedValues)
            {
                total /= 2;
            }

            if (upgradeLevel <= 3 && upgradeLevel > 1)
            {
                return 3;
            } else if (upgradeLevel <= 1)
            {
                return 1; 
            }
            else if (upgradeLevel > 3)
            {
                total += 60;
            }

            return total / 17;
        }

        private static int getCurrentUpgradeLevel(string prefix)
        {
            var result = prefix.Substring(prefix.LastIndexOf('+') + 1);
            string[] splitResult = result.Split(' ');
            if (Int32.TryParse(splitResult[0], out int res))
            {
                return res;
            }
            return 0;
        }

        internal static string getNextTier(WeaponUpgraderPrefix weaponUpgraderPrefix)
        {
            return "+" + (weaponUpgraderPrefix.getNameAsTier() + 1);
        }

        internal static bool CanBuyUpgrade(int price)
        {
            int upgradeTokenIndex = Main.LocalPlayer.FindItem(ItemType<Items.UpgradeToken>());
            if (upgradeTokenIndex != -1)
            {
                int tokenAmount = Main.LocalPlayer.inventory[upgradeTokenIndex].stack;
                if (tokenAmount >= price)
                {
                    return true;
                }
            }
            return false;
        }

        internal static int getTotalSpent(int upgradeTier)
        {
            int totalSpent = 0;
            for(int i = 1; i < upgradeTier+1; i++)
            {
                totalSpent += determinePriceForNextUpgrade(i);
            }
            return totalSpent;
        }

        internal static float getDamageMult(int power)
        {
            float multiplier = 1.03f + 0.01f * power;
            float damageMult = 1f + opBonus;
            if (power <= 10)
            {
                multiplier = 1f + 0.005f * power;
                damageMult *= multiplier + 0.005f * power;
            }
            if (power > 10 && power <= 20)
            {
                multiplier = 1f + 0.008f * power;
                damageMult *= multiplier + 0.005f * power;
            }

            if (power > 20 && power <= 30)
            {
                multiplier = 1f + 0.025f * power;
                damageMult *= multiplier + 0.005f * power;
            }

            if (power > 30)
            {
                multiplier = 1f + 0.030f * power;
                damageMult *= multiplier + 0.02f * power;
            }
            return 0.05f + damageMult;
        }

        internal static float getSpeedMult(int power)
        {
            float multiplier = 1f + 0.01f * power;
            if (power <= 10)
            {
                multiplier = 1f + 0.005f * power;
            }
            if (power > 10 && power <= 20)
            {
                multiplier = 1f + 0.008f * power;
            }

            if (power > 20 && power <= 30)
            {
                multiplier = 1f + 0.025f * power;
            }

            if (power > 30)
            {
                multiplier = 1f + 0.030f * power;
            }

            return multiplier + (opBonus * 2f) ;
        }

        internal static float getVelocityMult(int power)
        {
            float multiplier = 1.03f + 0.01f * power;
            float velMult = 1f;
            if (power <= 10)
            {
                multiplier = 1f + 0.005f * power;
            }
            if (power > 10 && power <= 20)
            {
                multiplier = 1f + 0.008f * power;
            }

            if (power > 20 && power <= 30)
            {
                multiplier = 1f + 0.025f * power;
            }

            if (power > 30)
            {
                multiplier = 1f + 0.030f * power;
            }
            velMult = (multiplier + 0.03f * power) + (opBonus / 3f);
            return velMult;
        }
    }
}
