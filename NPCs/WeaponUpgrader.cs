using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace UpgradeEquipment.NPCs
{
        // [AutoloadHead] and npc.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
        [AutoloadHead]
        public class WeaponUpgrader : ModNPC
        {
            //public override string Texture => "ExampleMod/NPCs/ExamplePerson";

            //public override string[] AltTextures => new[] { "ExampleMod/NPCs/ExamplePerson_Alt_1" };

            public override bool Autoload(ref string name)
            {
                name = "Equipment Upgrader";
                return mod.Properties.Autoload;
            }

            public override void SetStaticDefaults()
            {
                // DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
                // DisplayName.SetDefault("Example Person");
                Main.npcFrameCount[npc.type] = 25;
                NPCID.Sets.DangerDetectRange[npc.type] = 700;
                NPCID.Sets.AttackType[npc.type] = 0;
                NPCID.Sets.AttackTime[npc.type] = 90;
                NPCID.Sets.AttackAverageChance[npc.type] = 30;
                NPCID.Sets.HatOffsetY[npc.type] = 4;
            }

            public override void SetDefaults()
            {
                npc.townNPC = true;
                npc.friendly = true;
                npc.width = 40;
                npc.height = 46;
                npc.aiStyle = 7;
                npc.damage = 10;
                npc.defense = 50;
                npc.lifeMax = 1337;
                npc.HitSound = SoundID.NPCHit1;
                npc.DeathSound = SoundID.NPCDeath1;
                npc.knockBackResist = 0.5f;
                animationType = NPCID.Guide;
            }

            public override string TownNPCName()
            {
                switch (WorldGen.genRand.Next(4))
                {
                    case 0:
                        return "Goblin Dinkerer";
                    case 1:
                        return "Upgrademundo";
                    case 2:
                        return "Upyours";
                    default:
                        return "Equipmentino";
                }
            }

        public override string GetChat()
        {
            if (Main.rand.Next(2)==0)
            {
                return getRandomChatMessage();

            } else
            {
                return getRandomChatMessage();
            }
        }

        private string getRandomChatMessage()
        {
            switch (Main.rand.Next(6))
            {
                case 0:
                    return "You ever smack a bat with a +40? It feels great.";
                case 1:
                    return "Upgrading became pretty cheap after my wife left me.";
                case 2:
                    return "I don't trust that Tinkerer guy, he's been ripping us off.";
                case 3:
                    return "If +40 isn't enough for you, you can change the limit to 255 in the configuration of this mod.";
                case 4:
                    return "I will refund half of your spent tokens for any items that have +6 or higher!";
                default:
                    return "I see you're quite poor, but +1 is better then + none";
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return true;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Upgrade";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                // If the 2nd button is pressed, open the inventory...
                Main.playerInventory = true;
                // remove the chat window...
                Main.npcChatText = "";
                // and start an instance of our UIState.
                GetInstance<UpgradeEquipment>().WeaponUpgraderUserInterface.SetState(new UI.WeaponUpgraderUI());
                // Note that even though we remove the chat window, Main.LocalPlayer.talkNPC will still be set correctly and we are still technically chatting with the npc.
            }
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 1;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }
        }
    }


/* 
// Consider using this alternate approach to choosing a random thing. Very useful for a variety of use cases.
// The WeightedRandom class needs "using Terraria.Utilities;" to use
public override string GetChat()
{
    WeightedRandom<string> chat = new WeightedRandom<string>();
    int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
    if (partyGirl >= 0 && Main.rand.NextBool(4))
    {
        chat.Add("Can you please tell " + Main.npc[partyGirl].GivenName + " to stop decorating my house with colors?");
    }
    chat.Add("Sometimes I feel like I'm different from everyone else here.");
    chat.Add("What's your favorite color? My favorite colors are white and black.");
    chat.Add("What? I don't have any arms or legs? Oh, don't be ridiculous!");
    chat.Add("This message has a weight of 5, meaning it appears 5 times more often.", 5.0);
    chat.Add("This message has a weight of 0.1, meaning it appears 10 times as rare.", 0.1);
    return chat; // chat is implicitly cast to a string. You can also do "return chat.Get();" if that makes you feel better
}
*/
