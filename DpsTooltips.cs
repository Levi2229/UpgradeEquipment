using System.Collections.Generic;
using MagicStorageExtra.Sorting;
using Terraria;
using Terraria.ModLoader;

namespace MagicStorageExtra
{
	public class DpsTooltips : GlobalItem
	{
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (!MagicStorageConfig.ShowItemDps)
				return;

			double dps = CompareDps.GetDps(item);
			if (dps > 1f)
				tooltips.Add(new TooltipLine(MagicStorageExtra.Instance, "DPS", dps.ToString("F") + " DPS"));
		}
	}
}
