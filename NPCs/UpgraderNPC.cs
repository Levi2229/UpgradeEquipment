using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace UpgradeEquipment.NPCs
{
	[AutoloadHead]
	public class UpgraderNPC : ModNPC
	{
		public override bool Autoload(ref string name)
		{
			name = "Upgrader";
			return mod.Properties.Autoload;
		}

		public override void SetStaticDefaults()
		{
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
			switch (WorldGen.genRand.Next(8))
			{
				case 0:
					return "Bongo Man";

				case 1:
					return "Dango";

				case 2:
					return "Fedex";

				case 3:
					return "Peppar";

				case 4:
					return "Lion King";

				case 5:
					return "Hamberger";

				case 6:
					return "High School DxD";

				default:
					return "Equipmentino";
			}
		}

		public override string GetChat()
		{
			if (Main.rand.Next(2) == 0)
			{
				return getRandomChatMessage();
			}
			else
			{
				return getRandomChatMessage();
			}
		}

		private string getRandomChatMessage()
		{
			// add more lines maybe
			return "How about an upgrade?";

			/* og lines
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
			*/
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
				Main.playerInventory = true;
				Main.npcChatText = "";
				GetInstance<UpgradeEquipment>().WeaponUpgraderUserInterface.SetState(new UI.UpgraderUI());
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