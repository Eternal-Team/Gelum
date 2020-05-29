using BaseLibrary.UI;
using ContainerLibrary;
using EnergyLibrary;
using Gelum.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace Gelum.TileEntities
{
	public class MiningLaser : BaseGelumTE, IItemHandler, IHasUI, IEnergyReceiver
	{
		public override Vector2 InsertionPoint => new Vector2(40, 16);

		public override Type TileType => typeof(Tiles.MiningLaser);

		public ItemHandler Handler { get; }
		public EnergyHandler EnergyHandler { get; }

		public Guid UUID { get; set; }
		public BaseUIPanel UI { get; set; }
		public LegacySoundStyle CloseSound { get; }
		public LegacySoundStyle OpenSound { get; }

		private BaseLibrary.Timer timer;
		public Point16 CurrentTile = Point16.NegativeOne;

		public MiningLaser()
		{
			timer = new BaseLibrary.Timer(15, Callback);

			Handler = new ItemHandler();

			Handler.IsItemValid += (slot, item) =>
			{
				switch (slot)
				{
					case 0:
						return item.modItem is BaseContainmentUnit;
					default:
						return false;
				}
			};

			EnergyHandler = new EnergyHandler(1000000, 1000);
		}

		private const int EnergyPerTile = 100;

		public int radius = 2;
		public int height = 5;

		private void Callback()
		{
			if (EnergyHandler.Energy < EnergyPerTile) return;

			if (CurrentTile == Point16.NegativeOne) CurrentTile = new Point16(Position.X + 2 - radius, Position.Y + 5);

			WorldGen.KillTile(CurrentTile.X, CurrentTile.Y);

			if (CurrentTile.X < Position.X + 2 + radius) CurrentTile = new Point16(CurrentTile.X + 1, CurrentTile.Y);
			else
			{
				if (CurrentTile.Y < Position.Y + 4 + height) CurrentTile = new Point16(Position.X + 2 - radius, CurrentTile.Y + 1);
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
			["Energy"] = EnergyHandler.Save(),
			["Radius"] = radius,
			["Depth"] = height
		};

		public override void Load(TagCompound tag)
		{
			UUID = tag.Get<Guid>("UUID");
			Handler.Load(tag.GetCompound("Items"));
			EnergyHandler.Load(tag.GetCompound("Energy"));
			radius = tag.GetInt("Radius");
			height = tag.GetInt("Depth");
		}
	}
}