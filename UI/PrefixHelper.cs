using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpgradeEquipment.UI
{
    class PrefixHelper
    {
        public static bool reducedValues;

        public PrefixHelper()
        {

        }

        public static WeaponUpgraderPrefix FindCurrentPrefix(string itemNamePrefixed, string itemNameNoPrefix)
        {
            string prefix = itemNamePrefixed.Replace(itemNameNoPrefix, "");

            int[] price = determinePriceForNextUpgrade(prefix);
            return new WeaponUpgraderPrefix(prefix, true, price[0], price[1]);
        }

        private static int[] determinePriceForNextUpgrade(string prefix)
        {
           int upgradeLevel = getCurrentUpgradeLevel(prefix);

            float total = 0;
            int plat = 0;
            int gold = 1;
            total = (int)Math.Round((upgradeLevel * 3) + Convert.ToSingle((Math.Pow(upgradeLevel, 2))));
            if(upgradeLevel > 10)
            {
                total *= 1.5f;
            }
            if (upgradeLevel > 20)
            {
                total *= 2;
            }
            if (upgradeLevel > 30)
            {
                total *= 2;
            }
            if (upgradeLevel > 40)
            {
                total *= 2;
            }
            if (reducedValues)
            {
                total /= 2;
            }
            if (total > 100)
            {
                plat = (int)Math.Round(total / 100);
                gold = (int)Math.Round(total - (plat * 100));
            } else
            {
                gold = (int)Math.Round(total);
            }

            if(total == 0)
            {
                gold = 1;
            }

            int[] priceResult = { gold, plat };

            return priceResult;
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
    }
}
