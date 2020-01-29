using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace UpgradeEquipment.Items
{
    class UpgradeToken : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Upgrade Token");
            Tooltip.SetDefault("Used to buy upgrades at the Equipment Upgrader NPC");
        }

        public override void SetDefaults()
        {
            item.value = 50;
            item.maxStack = 9999;
        }
    }
}
