using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using UpgradeEquipment.Items;
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

        [Label("Disable weapon size upgrading. Recommended for combining with WeaponsOut mod.")]
        public bool disableSizeChange;

        [Label("Disable weapon knockback upgrading.")]
        public bool disableKnockbackChange;

        [Label("Make upgrades overpowered - Not balanced and might break the game")]
        public bool overpoweredUpgrades;

        [Label("Reduce the values of the item upgrader. warning: less balanced")]
        public bool reduceValues;

        [Label("Remove the +40 limit, you can now upgrade to +255")]
        public bool removeTierCap;

        public override void OnChanged()
        {
            // Here we use the OnChanged hook to initialize ExampleUI.visible with the new values.
            // We maintain both ExampleUI.visible and ShowCoinUI as separate values so ShowCoinUI can act as a default while ExampleUI.visible can change within a play session.
            PrefixHelper.reducedValues = reduceValues;
            if (overpoweredUpgrades)
            {
                PrefixHelper.opBonus = 1f;
            } else
            {
                PrefixHelper.opBonus = 0f;
            }
            UpgradeEquipmentGlobalItem.disableSizeChange = disableSizeChange;
            UpgradeEquipmentGlobalItem.disableKnockbackChange = disableKnockbackChange;
            if (removeTierCap)
            {
                WeaponUpgraderUI.tierCap = 255;
            } else
            {
                WeaponUpgraderUI.tierCap = 40;
            }
        }
    }
}
