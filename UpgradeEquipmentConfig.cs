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

        [Label("Change maximum upgrade cap, 40 is recommended.")]
        [Increment(5)]
        [Range(20, 255)]
        [DefaultValue(40)]
        [Slider] 
        public int tierCap;

        public override void OnChanged()
        {
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
            WeaponUpgraderUI.tierCap = tierCap;
        }
    }
}
