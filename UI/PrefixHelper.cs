using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using UpgradeEquipment.Items;
using static Terraria.ModLoader.ModContent;

namespace UpgradeEquipment.UI
{
    class PrefixHelper
    {
        public static bool reducedValues;
        public static int velocityMultiplier;
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

        public static int determinePriceForNextUpgrade(int upgradeLevel)
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

        internal static List<int> CanBuyUpgrade(int price, int attempt = 0)
        {
            int totalTokens = 0;
            List<int> TokenPositions = new List<int>();
            Item[] inventory = Main.LocalPlayer.inventory;
            for (int i = 0; i < inventory.Length; i++)
            {
                if(inventory[i].netID == ItemType<Items.UpgradeToken>())
                {
                    totalTokens += inventory[i].stack;
                    TokenPositions.Add(i);
                }
            }

            if(totalTokens >= price)
            {
                return TokenPositions;
            }
            else
            {
                return new List<int>();
            }
        }

        internal static int getTotalSpent(int upgradeTier)
        {
            int totalSpent = 0;
            for(int i = 0; i < upgradeTier; i++)
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
            float multiplier = 1f;
            if (power <= 10)
            {
                multiplier = 1f + 0.0015f * power;
            }
            if (power > 10 && power <= 20)
            {
                multiplier = 1f + 0.006f * power;
            }

            if (power > 20 && power <= 30)
            {
                multiplier = 1f + 0.008f * power;
            }

            if (power > 30)
            {
                multiplier = 1f + 0.01f * power;
            }

            return multiplier + (opBonus * 3f) ;
        }

        internal static float getVelocityMult(int power)
        {
            float multiplier = 0f;
            float velMult = 1f;
            if (power <= 10)
            {
                multiplier = 0.002f * power;
            }
            if (power > 10 && power <= 20)
            {
                multiplier = 0.003f * power;
            }

            if (power > 20 && power <= 30)
            {
                multiplier =0.006f * power;
            }

            if (power > 30)
            {
                multiplier = 0.009f * power;
            }

            if (velocityMultiplier == 0)
            {
                return 1;
            }

            float multiplierReduction = velocityMultiplier / 100f;
            multiplier = multiplier * multiplierReduction;
            velMult += (float)(multiplier + (opBonus / 3f));

            return velMult;
        }

        internal static float getNegativeMult(int power)
        {
            float multiplier = 1f + 0.01f * power;
            float negMult = 1f;
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
            negMult = (-multiplier + (2.005f + 0.018f * power)) - 0.05f;
            return negMult - (opBonus / 3f);
        }

        internal static Color getTierColor(int tier)
        {
            Color tierColor = new Color(186, 186, 186);
            if (tier > 10)
            {
                tierColor = new Color(68, 131, 220);
            }
            if (tier > 20)
            {
                tierColor = new Color(229, 172, 82);
            }
            if (tier > 30)
            {
                tierColor = new Color(222, 111, 228);
            }
            if (tier >= 35)
            {
                tierColor = new Color(222, 67, 58);
            }
            return tierColor;
        }
    }
}
