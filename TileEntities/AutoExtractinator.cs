using BaseLibrary;
using BaseLibrary.UI;
using ContainerLibrary;
using EnergyLibrary;
using Gelum.Items;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Gelum.TileEntities
{
	public class AutoExtractinator : BaseGelumTE, IItemHandler, IHasUI, IEnergyReceiver
	{
		public override Vector2 InsertionPoint => new Vector2(35, 40);

		public override Type TileType => typeof(Tiles.AutoExtractinator);

		public ItemHandler Handler { get; }
		public EnergyHandler EnergyHandler { get; }

		public Guid UUID { get; set; }
		public BaseUIPanel UI { get; set; }
		public LegacySoundStyle CloseSound { get; }
		public LegacySoundStyle OpenSound { get; }

		private Timer timer;

		public AutoExtractinator()
		{
			timer = new Timer(60, Callback);

			Handler = new ItemHandler(11)
			{
				Modes =
				{
					[1] = SlotMode.Input,
					[2] = SlotMode.Output,
					[3] = SlotMode.Output,
					[4] = SlotMode.Output,
					[5] = SlotMode.Output,
					[6] = SlotMode.Output,
					[7] = SlotMode.Output,
					[8] = SlotMode.Output,
					[9] = SlotMode.Output,
					[10] = SlotMode.Output
				}
			};

			Handler.IsItemValid += (slot, item) =>
			{
				switch (slot)
				{
					case 0:
						return item.modItem is BaseContainmentUnit;
					case 1:
						return ValidItems.Contains(item.type);
					default:
						return false;
				}
			};

			EnergyHandler = new EnergyHandler(1000000, 1000);
		}

		private const int EnergyPerItem = 1000;

		private void Callback()
		{
			if (EnergyHandler.Energy < EnergyPerItem) return;

			int slot = Handler.GetFirstInput();
			if (slot != -1)
			{
				Item item = Handler.GetItemInSlot(slot);

				if (ValidItems.Contains(item.type))
				{
					(int type, int amount) = ExtractinatorUse(item.type);

					if (Handler.OutputSlots.Any((i, _) => i.IsAir || i.type == type && i.stack + amount <= i.maxStack))
					{
						for (int i = 0; i < Handler.Slots; i++)
						{
							if (Handler.Modes[i] != SlotMode.Output) continue;

							if (Handler.Items[i].type == type && Handler.Items[i].stack + amount <= Handler.Items[i].maxStack)
							{
								Handler.Items[i].stack += amount;
								break;
							}

							if (Handler.Items[i].IsAir)
							{
								Handler.Items[i].SetDefaults(type);
								break;
							}
						}
						
						Handler.Shrink(slot, 1);
						EnergyHandler.ExtractEnergy(EnergyPerItem);
					}
				}
			}
		}

		private static readonly List<int> ValidItems = new List<int>
		{
			ItemID.SiltBlock,
			ItemID.SlushBlock,
			ItemID.DesertFossil
		};

		public override void Update()
		{
			timer.Update();
		}

		public override TagCompound Save() => new TagCompound
		{
			["UUID"] = UUID,
			["Items"] = Handler.Save(),
			["Energy"] = EnergyHandler.Save()
		};

		public override void Load(TagCompound tag)
		{
			UUID = tag.Get<Guid>("UUID");
			Handler.Load(tag.GetCompound("Items"));
			EnergyHandler.Load(tag.GetCompound("Energy"));
		}

		private static (int type, int amount) ExtractinatorUse(int extractType)
		{
			int num = 5000;
			int num2 = 25;
			int num3 = 50;
			int num4 = -1;
			if (extractType == ItemID.DesertFossil)
			{
				num /= 3;
				num2 *= 2;
				num3 /= 2;
				num4 = 10;
			}

			int type = -1;
			int amount = 1;
			if (num4 != -1 && Main.rand.Next(num4) == 0)
			{
				type = 3380;
				if (Main.rand.Next(5) == 0) amount += Main.rand.Next(2);

				if (Main.rand.Next(10) == 0) amount += Main.rand.Next(3);

				if (Main.rand.Next(15) == 0) amount += Main.rand.Next(4);
			}
			else if (Main.rand.Next(2) == 0)
			{
				if (Main.rand.Next(12000) == 0)
				{
					type = 74;
					if (Main.rand.Next(14) == 0) amount += Main.rand.Next(0, 2);

					if (Main.rand.Next(14) == 0) amount += Main.rand.Next(0, 2);

					if (Main.rand.Next(14) == 0) amount += Main.rand.Next(0, 2);
				}
				else if (Main.rand.Next(800) == 0)
				{
					type = 73;
					if (Main.rand.Next(6) == 0) amount += Main.rand.Next(1, 21);

					if (Main.rand.Next(6) == 0) amount += Main.rand.Next(1, 21);

					if (Main.rand.Next(6) == 0) amount += Main.rand.Next(1, 21);

					if (Main.rand.Next(6) == 0) amount += Main.rand.Next(1, 21);

					if (Main.rand.Next(6) == 0) amount += Main.rand.Next(1, 20);
				}
				else if (Main.rand.Next(60) == 0)
				{
					type = 72;
					if (Main.rand.Next(4) == 0) amount += Main.rand.Next(5, 26);

					if (Main.rand.Next(4) == 0) amount += Main.rand.Next(5, 26);

					if (Main.rand.Next(4) == 0) amount += Main.rand.Next(5, 26);

					if (Main.rand.Next(4) == 0) amount += Main.rand.Next(5, 25);
				}
				else
				{
					type = 71;
					if (Main.rand.Next(3) == 0) amount += Main.rand.Next(10, 26);

					if (Main.rand.Next(3) == 0) amount += Main.rand.Next(10, 26);

					if (Main.rand.Next(3) == 0) amount += Main.rand.Next(10, 26);

					if (Main.rand.Next(3) == 0) amount += Main.rand.Next(10, 25);
				}
			}
			else if (num != -1 && Main.rand.Next(num) == 0)
			{
				type = 1242;
			}
			else if (num2 != -1 && Main.rand.Next(num2) == 0)
			{
				switch (Main.rand.Next(6))
				{
					case 0:
						type = 181;
						break;
					case 1:
						type = 180;
						break;
					case 2:
						type = 177;
						break;
					case 3:
						type = 179;
						break;
					case 4:
						type = 178;
						break;
					default:
						type = 182;
						break;
				}

				if (Main.rand.Next(20) == 0) amount += Main.rand.Next(0, 2);

				if (Main.rand.Next(30) == 0) amount += Main.rand.Next(0, 3);

				if (Main.rand.Next(40) == 0) amount += Main.rand.Next(0, 4);

				if (Main.rand.Next(50) == 0) amount += Main.rand.Next(0, 5);

				if (Main.rand.Next(60) == 0) amount += Main.rand.Next(0, 6);
			}
			else if (num3 != -1 && Main.rand.Next(num3) == 0)
			{
				type = 999;
				if (Main.rand.Next(20) == 0) amount += Main.rand.Next(0, 2);

				if (Main.rand.Next(30) == 0) amount += Main.rand.Next(0, 3);

				if (Main.rand.Next(40) == 0) amount += Main.rand.Next(0, 4);

				if (Main.rand.Next(50) == 0) amount += Main.rand.Next(0, 5);

				if (Main.rand.Next(60) == 0) amount += Main.rand.Next(0, 6);
			}
			else if (Main.rand.Next(3) == 0)
			{
				if (Main.rand.Next(5000) == 0)
				{
					type = 74;
					if (Main.rand.Next(10) == 0) amount += Main.rand.Next(0, 3);

					if (Main.rand.Next(10) == 0) amount += Main.rand.Next(0, 3);

					if (Main.rand.Next(10) == 0) amount += Main.rand.Next(0, 3);

					if (Main.rand.Next(10) == 0) amount += Main.rand.Next(0, 3);

					if (Main.rand.Next(10) == 0) amount += Main.rand.Next(0, 3);
				}
				else if (Main.rand.Next(400) == 0)
				{
					type = 73;
					if (Main.rand.Next(5) == 0) amount += Main.rand.Next(1, 21);

					if (Main.rand.Next(5) == 0) amount += Main.rand.Next(1, 21);

					if (Main.rand.Next(5) == 0) amount += Main.rand.Next(1, 21);

					if (Main.rand.Next(5) == 0) amount += Main.rand.Next(1, 21);

					if (Main.rand.Next(5) == 0) amount += Main.rand.Next(1, 20);
				}
				else if (Main.rand.Next(30) == 0)
				{
					type = 72;
					if (Main.rand.Next(3) == 0) amount += Main.rand.Next(5, 26);

					if (Main.rand.Next(3) == 0) amount += Main.rand.Next(5, 26);

					if (Main.rand.Next(3) == 0) amount += Main.rand.Next(5, 26);

					if (Main.rand.Next(3) == 0) amount += Main.rand.Next(5, 25);
				}
				else
				{
					type = 71;
					if (Main.rand.Next(2) == 0) amount += Main.rand.Next(10, 26);

					if (Main.rand.Next(2) == 0) amount += Main.rand.Next(10, 26);

					if (Main.rand.Next(2) == 0) amount += Main.rand.Next(10, 26);

					if (Main.rand.Next(2) == 0) amount += Main.rand.Next(10, 25);
				}
			}
			else
			{
				switch (Main.rand.Next(8))
				{
					case 0:
						type = 12;
						break;
					case 1:
						type = 11;
						break;
					case 2:
						type = 14;
						break;
					case 3:
						type = 13;
						break;
					case 4:
						type = 699;
						break;
					case 5:
						type = 700;
						break;
					case 6:
						type = 701;
						break;
					default:
						type = 702;
						break;
				}

				if (Main.rand.Next(20) == 0) amount += Main.rand.Next(0, 2);

				if (Main.rand.Next(30) == 0) amount += Main.rand.Next(0, 3);

				if (Main.rand.Next(40) == 0) amount += Main.rand.Next(0, 4);

				if (Main.rand.Next(50) == 0) amount += Main.rand.Next(0, 5);

				if (Main.rand.Next(60) == 0) amount += Main.rand.Next(0, 6);
			}

			ItemLoader.ExtractinatorUse(ref type, ref amount, extractType);
			return type > 0 ? (type, amount) : default;
		}
	}
}