using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using UpgradeEquipment.UI;
using Terraria;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;


namespace UpgradeEquipment
{
    public class UpgradeEquipment : Mod
    {
        internal UserInterface WeaponUpgraderUserInterface;
        public UpgradeEquipment()
        {
        }

        public override void Load()
        {
            // Will show up in client.log under the ExampleMod name
            Logger.InfoFormat("{0} upgrade equipment logger", Name);

            // UserInterface can only show 1 UIState at a time. If you want different "pages" for a UI, switch between UIStates on the same UserInterface instance. 
            // We want both the Coin counter and the Example Person UI to be independent and coexist simultaneously, so we have them each in their own UserInterface.
            WeaponUpgraderUserInterface = new UserInterface();
                // We will call .SetState later in ExamplePerson.OnChatButtonClicked
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
                    "ExampleMod: Example Person UI",
                    delegate {
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