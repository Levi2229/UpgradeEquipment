using System.Text.RegularExpressions;

namespace UpgradeEquipment.UI
{
	internal class UpgraderPrefix
	{
		public string name;
		public bool isModded;
		public int price = 0;

		public UpgraderPrefix(string name, bool isModded, int price)
		{
			this.name = name;
			this.isModded = isModded;
			this.price = price;
		}

		public int getNameAsTier()
		{
			int.TryParse(Regex.Replace(name, "[^0-9]", ""), out int res);
			return res;
		}
	}
}