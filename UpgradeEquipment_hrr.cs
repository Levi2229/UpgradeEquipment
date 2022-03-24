using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace UpgradeEquipment_hrr
{
	public class UpgradeEquipment_hrr : Mod
	{
		internal UserInterface WeaponUpgraderUserInterface;

		public override void Load()
		{
			Logger.InfoFormat("{0} logger", Name);
			WeaponUpgraderUserInterface = new UserInterface();
		}

		public override void UpdateUI(GameTime gameTime)
		{
			WeaponUpgraderUserInterface?.Update(gameTime);
		}
		public override void PostSetupContent()
		{
			// why census support for an npc that can spawn the second the world is created? why not
			Mod censusMod = ModLoader.GetMod("Census");
			if (censusMod != null)
			{
				censusMod.Call("TownNPCCondition", NPCType("Upgrader"), "Exist");
			}
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));

			if (inventoryIndex != -1)
			{
				layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
					"Upgrader UI",
					delegate
					{
						WeaponUpgraderUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}