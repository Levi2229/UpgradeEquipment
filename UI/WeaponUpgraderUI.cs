using UpgradeEquipment.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using static Terraria.ModLoader.ModContent;
using UpgradeEquipment.Prefixes;

namespace UpgradeEquipment.UI
{
    // This class represents the UIState for our ExamplePerson Awesomeify chat function. It is similar to the Goblin Tinkerer's Reforge function, except it only gives Awesome and ReallyAwesome prefixes. 
    internal class WeaponUpgraderUI : UIState
    {
        private VanillaItemSlotWrapper _vanillaItemSlot;

        public override void OnInitialize()
        {
            _vanillaItemSlot = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f)
            {
                Left = { Pixels = 50 },
                Top = { Pixels = 270 },
                ValidItemFunc = item => item.IsAir || !item.IsAir && item.Prefix(-3)
            };
            // Here we limit the items that can be placed in the slot. We are fine with placing an empty item in or a non-empty item that can be prefixed. Calling Prefix(-3) is the way to know if the item in question can take a prefix or not.
            Append(_vanillaItemSlot);
        }

        // OnDeactivate is called when the UserInterface switches to a different state. In this mod, we switch between no state (null) and this state (ExamplePersonUI).
        // Using OnDeactivate is useful for clearing out Item slots and returning them to the player, as we do here.
        public override void OnDeactivate()
        {
            if (!_vanillaItemSlot.Item.IsAir)
            {
                // QuickSpawnClonedItem will preserve mod data of the item. QuickSpawnItem will just spawn a fresh version of the item, losing the prefix.
                Main.LocalPlayer.QuickSpawnClonedItem(_vanillaItemSlot.Item, _vanillaItemSlot.Item.stack);
                // Now that we've spawned the item back onto the player, we reset the item by turning it into air.
                _vanillaItemSlot.Item.TurnToAir();
            }
            // Note that in ExamplePerson we call .SetState(new UI.ExamplePersonUI());, thereby creating a new instance of this UIState each time. 
            // You could go with a different design, keeping around the same UIState instance if you wanted. This would preserve the UIState between opening and closing. Up to you.
        }

        // Update is called on a UIState while it is the active state of the UserInterface.
        // We use Update to handle automatically closing our UI when the player is no longer talking to our Example Person NPC.
        public override void Update(GameTime gameTime)
        {
            // Don't delete this or the UIElements attached to this UIState will cease to function.
            base.Update(gameTime);

            // talkNPC is the index of the NPC the player is currently talking to. By checking talkNPC, we can tell when the player switches to another NPC or closes the NPC chat dialog.
            if (Main.LocalPlayer.talkNPC == -1 || Main.npc[Main.LocalPlayer.talkNPC].type != NPCType<WeaponUpgrader>())
            {
                // When that happens, we can set the state of our UserInterface to null, thereby closing this UIState. This will trigger OnDeactivate above.
                GetInstance<UpgradeEquipment>().WeaponUpgraderUserInterface.SetState(null);
            }
        }

        private bool tickPlayed;
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            // This will hide the crafting menu similar to the reforge menu. For best results this UI is placed before "Vanilla: Inventory" to prevent 1 frame of the craft menu showing.
            Main.HidePlayerCraftingMenu = true;

            // Here we have a lot of code. This code is mainly adapted from the vanilla code for the reforge option.
            // This code draws "Place an item here" when no item is in the slot and draws the reforge cost and a reforge button when an item is in the slot.
            // This code could possibly be better as different UIElements that are added and removed, but that's not the main point of this example.
            // If you are making a UI, add UIElements in OnInitialize that act on your ItemSlot or other inputs rather than the non-UIElement approach you see below.

            const int slotX = 50;
            const int slotY = 270;
            if (!_vanillaItemSlot.Item.IsAir && (_vanillaItemSlot.Item.melee || _vanillaItemSlot.Item.ranged || _vanillaItemSlot.Item.magic))
            {
                Item item = _vanillaItemSlot.Item;
                byte prefix = item.prefix;

                WeaponUpgraderPrefix moddedPrefix = PrefixHelper.FindCurrentPrefix(item.HoverName, item.Name);

                int awesomePrice = moddedPrefix.price;

                string upgradeCostText = "";

                upgradeCostText = "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + "Cost: " + awesomePrice + " Upgrade Tokens] " ;
                  
                int refundX = slotX + 150;
                int refundY = slotY + 30;
                bool hoveringOverRefundButton = Main.mouseX > refundX - 10 && Main.mouseX < refundX + 175 && Main.mouseY > refundY - 5 && Main.mouseY < refundY + 20 && !PlayerInput.IgnoreMouseInterface;

                Color c = Color.White;

                if(hoveringOverRefundButton)
                {
                    c = Color.Green;
                }

                if (moddedPrefix.getNameAsTier() > 5)
                {
                    hoveringOverRefundButton = Main.mouseX > refundX - 55 && Main.mouseX < refundX + 125 && Main.mouseY > refundY - 5 && Main.mouseY < refundY + 20 && !PlayerInput.IgnoreMouseInterface;
                    if (hoveringOverRefundButton)
                    {
                        c = Color.Green;
                    }
                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, "Refund ( " + (PrefixHelper.getTotalSpent(moddedPrefix.getNameAsTier()) / 2) + " tokens )", new Vector2(refundX - 50, refundY), c, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
                }
                if (moddedPrefix == null || moddedPrefix.getNameAsTier() <= 39)
                {
                    int reforgeX = slotX + 70;
                    int reforgeY = slotY + 40;
                    bool hoveringOverReforgeButton = Main.mouseX > reforgeX - 15 && Main.mouseX < reforgeX + 15 && Main.mouseY > reforgeY - 15 && Main.mouseY < reforgeY + 15 && !PlayerInput.IgnoreMouseInterface;

                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, upgradeCostText, new Vector2(slotX + 50, (float)slotY), Color.White, 0f, Vector2.Zero, Vector2.One, -1f, 2f);


                    Texture2D reforgeTexture = Main.reforgeTexture[hoveringOverReforgeButton ? 1 : 0];

                    Main.spriteBatch.Draw(reforgeTexture, new Vector2(reforgeX, reforgeY), null, Color.White, 0f, reforgeTexture.Size() / 2f, 0.8f, SpriteEffects.None, 0f);
                    if (hoveringOverReforgeButton)
                    {
                        Main.hoverItemName = Language.GetTextValue("LegacyInterface.19");
                        if (!tickPlayed)
                        {
                            Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                        }
                        tickPlayed = true;
                        Main.LocalPlayer.mouseInterface = true;
                        if (Main.mouseLeftRelease && Main.mouseLeft && PrefixHelper.CanBuyUpgrade(awesomePrice) && moddedPrefix.getNameAsTier() <= 39)
                        {
                            int upgradeTokenIndex = Main.LocalPlayer.FindItem(ItemType<Items.UpgradeToken>());
                            Main.LocalPlayer.inventory[upgradeTokenIndex].stack -= awesomePrice;
                            bool favorited = _vanillaItemSlot.Item.favorited;
                            int stack = _vanillaItemSlot.Item.stack;
                            Item reforgeItem = new Item();
                            reforgeItem.netDefaults(_vanillaItemSlot.Item.netID);
                            reforgeItem = reforgeItem.CloneWithModdedDataFrom(_vanillaItemSlot.Item);
                            // This is the main effect of this slot. Giving the Awesome prefix 90% of the time and the ReallyAwesome prefix the other 10% of the time. All for a constant 1 gold. Useless, but informative.


                            string nextTier = PrefixHelper.getNextTier(moddedPrefix);
                            if (reforgeItem.melee)
                            {
                                reforgeItem.Prefix(GetInstance<UpgradeEquipment>().PrefixType("melee " + nextTier));
                            }
                            if (reforgeItem.ranged)
                            {
                                reforgeItem.Prefix(GetInstance<UpgradeEquipment>().PrefixType("ranged " + nextTier));
                            }
                            if (reforgeItem.magic)
                            {
                                reforgeItem.Prefix(GetInstance<UpgradeEquipment>().PrefixType("magic " + nextTier));
                            }

                            _vanillaItemSlot.Item = reforgeItem.Clone();
                            _vanillaItemSlot.Item.position.X = Main.LocalPlayer.position.X + (float)(Main.LocalPlayer.width / 2) - (float)(_vanillaItemSlot.Item.width / 2);
                            _vanillaItemSlot.Item.position.Y = Main.LocalPlayer.position.Y + (float)(Main.LocalPlayer.height / 2) - (float)(_vanillaItemSlot.Item.height / 2);
                            _vanillaItemSlot.Item.favorited = favorited;
                            _vanillaItemSlot.Item.stack = stack;
                            ItemLoader.PostReforge(_vanillaItemSlot.Item);
                            ItemText.NewText(_vanillaItemSlot.Item, _vanillaItemSlot.Item.stack, true, false);
                            Main.PlaySound(SoundID.Item37, -1, -1);
                        }
                    }
                    else
                    {
                        tickPlayed = false;
                    }
                } else
                {
                    string message = "Max level reached!";
                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, message, new Vector2(slotX + 50, slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
                }

                if (hoveringOverRefundButton)
                {
                    Main.LocalPlayer.mouseInterface = true;
                    if (Main.mouseLeftRelease && Main.mouseLeft)
                    {
                        int upgradeTokenIndex = Main.LocalPlayer.FindItem(ItemType<Items.UpgradeToken>());
                        if (upgradeTokenIndex == -1)
                        {
                            Item.NewItem(Main.LocalPlayer.getRect(), ItemType<Items.UpgradeToken>(), (PrefixHelper.getTotalSpent(moddedPrefix.getNameAsTier()) / 2));
                        }
                        else
                        {
                            Main.LocalPlayer.inventory[upgradeTokenIndex].stack += PrefixHelper.getTotalSpent(moddedPrefix.getNameAsTier()) / 2;
                        }
                        bool favorited = _vanillaItemSlot.Item.favorited;
                        int stack = _vanillaItemSlot.Item.stack;
                        Item reforgeItem = new Item();
                        reforgeItem.netDefaults(_vanillaItemSlot.Item.netID);
                        reforgeItem = reforgeItem.CloneWithModdedDataFrom(_vanillaItemSlot.Item);

                        string nextTier = PrefixHelper.getNextTier(moddedPrefix);
                        reforgeItem.Prefix(0);
                        _vanillaItemSlot.Item = reforgeItem.Clone();
                        _vanillaItemSlot.Item.position.X = Main.LocalPlayer.position.X + (float)(Main.LocalPlayer.width / 2) - (float)(_vanillaItemSlot.Item.width / 2);
                        _vanillaItemSlot.Item.position.Y = Main.LocalPlayer.position.Y + (float)(Main.LocalPlayer.height / 2) - (float)(_vanillaItemSlot.Item.height / 2);
                        _vanillaItemSlot.Item.favorited = favorited;
                        _vanillaItemSlot.Item.stack = stack;
                        ItemLoader.PostReforge(_vanillaItemSlot.Item);
                        ItemText.NewText(_vanillaItemSlot.Item, _vanillaItemSlot.Item.stack, true, false);
                        Main.PlaySound(SoundID.Item37, -1, -1);
                    }
                }
            }
            else
            {
                string message = "Place an item here to Upgrade";
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, message, new Vector2(slotX + 50, slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
            }
        }
    }
}