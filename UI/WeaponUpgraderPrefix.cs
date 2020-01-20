using System.Text.RegularExpressions;

namespace UpgradeEquipment.UI
{
    internal class WeaponUpgraderPrefix
    {
        public string name;
        public bool isModded;
        public int pricePlat = 0;
        public int priceGold = 1;

        public WeaponUpgraderPrefix(string name, bool isModded, int priceGold, int pricePlat)
        {
            this.name = name;
            this.isModded = isModded;
            this.priceGold = priceGold;
            this.pricePlat = pricePlat;
        }

        public int getNameAsTier()
        {
            int.TryParse(Regex.Replace(name, "[^0-9]", ""), out int res);
            return res;
        }
    }
}