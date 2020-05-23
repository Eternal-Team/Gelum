using BaseLibrary;
using BaseLibrary.Tiles.TileEntites;
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
	public class GlacialConcentrator : BaseGelumTE, IItemHandler, IHasUI, IEnergyReceiver
	{
		public override Type TileType => typeof(Tiles.GlacialConcentrator);

		public ItemHandler Handler { get; }
		public EnergyHandler EnergyHandler { get; }

		public Guid UUID { get; set; }
		public BaseUIPanel UI { get; set; }
		public LegacySoundStyle CloseSound { get; }
		public LegacySoundStyle OpenSound { get; }

		private Timer timer;

		public GlacialConcentrator()
		{
			timer = new Timer(60, Callback);

			Handler = new ItemHandler();
			Handler.IsItemValid += (slot, item) => item.modItem is BaseContainmentUnit;

			EnergyHandler = new EnergyHandler(1000000, 1000);
		}

		private void Callback()
		{
			if (EnergyHandler.Energy < 100) return;

			float angle = Main.rand.NextFloat(MathHelper.TwoPi);

			int radius = Main.rand.Next(9);
			int dX = (int)(Math.Cos(angle) * radius);
			int dY = (int)(Math.Sin(angle) * radius);

			int i = Position.X + dX;
			int j = Position.Y + dY;

			if (Utility.InWorldBounds(i, j))
			{
				Tile tile = Main.tile[i, j];
				if (WorldGen.TileEmpty(i, j) && tile.liquidType() == Tile.Liquid_Water && tile.liquid == 255)
				{
					WorldGen.PlaceTile(i, j, TileID.IceBlock);
					tile.liquid = 0;
					EnergyHandler.ExtractEnergy(100);
				}
			}
		}

		public override void Update()
		{
			timer.Update();

			Item item = Handler.GetItemInSlot(0);
			if (!item.IsAir && item.modItem is BaseContainmentUnit unit) unit.EnergyHandler.TransferEnergy(EnergyHandler);
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