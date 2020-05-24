using BaseLibrary;
using BaseLibrary.UI;
using ContainerLibrary;
using EnergyLibrary;
using Gelum.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace Gelum.TileEntities
{
	public class Crusher : BaseGelumTE, IItemHandler, IHasUI, IEnergyReceiver
	{
		public override Vector2 InsertionPoint => new Vector2(32, 66);

		public override Type TileType => typeof(Tiles.Crusher);

		public ItemHandler Handler { get; }
		public EnergyHandler EnergyHandler { get; }

		public Guid UUID { get; set; }
		public BaseUIPanel UI { get; set; }
		public LegacySoundStyle CloseSound { get; }
		public LegacySoundStyle OpenSound { get; }

		private Timer timer;

		public Crusher()
		{
			timer = new Timer(60, Callback);

			Handler = new ItemHandler(5)
			{
				Modes =
				{
					[1] = SlotMode.Input,
					[2] = SlotMode.Input,
					[3] = SlotMode.Output,
					[4] = SlotMode.Output
				}
			};

			Handler.IsItemValid += (slot, item) =>
			{
				switch (slot)
				{
					case 0:
						return item.modItem is BaseContainmentUnit;
					case 1:
					case 2:
						return item.type == ItemID.StoneBlock || item.type == ItemID.SandBlock;
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
				if (item.type == ItemID.StoneBlock)
				{
					Handler.Shrink(slot, 1);

					for (int i = 3; i <= 4; i++)
					{
						if (Handler.Items[i].type == ItemID.SandBlock && Handler.Items[i].stack < Handler.Items[i].maxStack)
						{
							Handler.Items[i].stack++;
							break;
						}

						if (Handler.Items[i].IsAir)
						{
							Handler.Items[i].SetDefaults(ItemID.SandBlock);
							break;
						}
					}
				}
			}
		}

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
	}
}