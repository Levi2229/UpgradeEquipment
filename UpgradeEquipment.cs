using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace UpgradeEquipment
{
	public class UpgradeEquipment : Mod
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
						// If the current UIState of the UserInterface is null, nothing will draw. We don't need to track a separate .visible value.
						WeaponUpgraderUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}