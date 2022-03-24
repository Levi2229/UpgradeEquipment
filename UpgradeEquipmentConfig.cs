using System.ComponentModel;
using Terraria.ModLoader.Config;
using UpgradeEquipment.UI;

namespace UpgradeEquipment
{
	/// <summary>
	/// This config operates on a per-client basis.
	/// These parameters are local to this computer and are NOT synced from the server.
	/// </summary>
	public class UpgradeEquipmentclient : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Label("Number of upgrades accessible pre-hardmode")]
		[Increment(5)]
		[Range(5, 255)]
		[DefaultValue(10)]
		[Slider]
		public int preHardmodeTiers;

		[Label("Maximum number of upgrades")]
		[Increment(5)]
		[Range(0, 255)]
		[DefaultValue(255)]
		[Slider]
		public int maxTiers;

		public override void OnChanged()
		{
			UpgraderUI.preHardmodeTiers = preHardmodeTiers;
			if (preHardmodeTiers > maxTiers)
			{
				maxTiers = preHardmodeTiers;
			}
			UpgraderUI.maxTiers = maxTiers;
		}
	}
}