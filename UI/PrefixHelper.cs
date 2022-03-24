using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace UpgradeEquipment_hrr.UI
{
	internal class PrefixHelper
	{
		public static UpgraderPrefix FindCurrentPrefix(string itemNamePrefixed, string itemNameNoPrefix)
		{
			string prefix = itemNamePrefixed.Replace(itemNameNoPrefix, "");

			int upgradeLevel = GetCurrentUpgradeLevel(prefix);
			int price = DeterminePriceForNextUpgrade(upgradeLevel);
			return new UpgraderPrefix(prefix, true, price);
		}

		// determines cost of next upgrade
		public static int DeterminePriceForNextUpgrade(int upgradeLevel)
		{
			int total = (int)(upgradeLevel + Convert.ToSingle(Math.Pow(upgradeLevel, 2)));
			total += 60;

			if (upgradeLevel <= 10)
			{
				return 1 + upgradeLevel;
			}
			else if (upgradeLevel > 10)
			{
				total += (upgradeLevel - 10) * 40;
			}
			else if (upgradeLevel > 20)
			{
				total += (upgradeLevel - 20) * 70;
			}
			else if (upgradeLevel > 30)
			{
				total += (upgradeLevel - 30) * 100;
			}
			else if (upgradeLevel > 40)
			{
				total += (upgradeLevel - 40) * 165;
			}
			else
			{
				total += (upgradeLevel - 50) * 250;
			}

			return total / 17; // why 17... nvm i'm keeping it
		}

		// gets current tier
		private static int GetCurrentUpgradeLevel(string prefix)
		{
			var result = prefix.Substring(prefix.LastIndexOf('+') + 1);
			string[] splitResult = result.Split(' ');
			if (Int32.TryParse(splitResult[0], out int res))
			{
				return res;
			}
			return 0;
		}

		// check to see if player has enough tokens in inventory
		internal static List<int> CanBuyUpgrade(int price, int attempt = 0)
		{
			int totalTokens = 0;
			List<int> TokenPositions = new List<int>();
			Item[] inventory = Main.LocalPlayer.inventory;

			for (int i = 0; i < inventory.Length; i++)
			{
				if (inventory[i].netID == ItemType<Items.UpgradeToken>())
				{
					totalTokens += inventory[i].stack;
					TokenPositions.Add(i);
				}
			}

			if (totalTokens >= price)
			{
				return TokenPositions;
			}
			else
			{
				return new List<int>();
			}
		}

		// total tokens spent
		internal static int GetTotalSpent(int upgradeTier)
		{
			int totalSpent = 0;
			for (int i = 0; i < upgradeTier; i++)
			{
				totalSpent += DeterminePriceForNextUpgrade(i);
			}
			return totalSpent;
		}

		/* +1 to +10: 3% fd / 2% crit / 2% use speed
		 * +11 onwards: 1% fd */

		// final damage increase
		internal static float GetFinalDamageMult(int power)
		{
			float mult;

			if (power < 10)
			{
				mult = 1f + 0.04f * power;
			}
			else
			{
				mult = 1.3f + 0.01f * (power - 10);
			}

			return mult;
		}

		// crit chance increase
		internal static int GetCriticalMult(int power)
		{
			int mult;

			if (power < 10)
			{
				mult = 2 * power;
			}

			else
			{
				mult = 20;
			}

			return mult;
		}

		// use speed increase
		internal static float GetSpeedMult(int power)
		{
			float mult;

			if (power < 10)
			{
				mult = 1f + 0.02f * power;
			}

			else
			{
				mult = 1.2f;
			}

			return mult;
		}

		// tier color
		internal static Color GetTierColor(int tier)
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
			if (tier > 40)
			{
				tierColor = new Color(222, 67, 58);
			}
			return tierColor;
		}
	}
}