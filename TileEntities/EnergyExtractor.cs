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
	public class EnergyExtractor : BaseGelumTE, IItemHandler, IHasUI, IEnergySource
	{
		public override Vector2 InsertionPoint=>new Vector2(24, 24);
		
		public override Type TileType => typeof(Tiles.EnergyExtractor);

		public ItemHandler Handler { get; }
		public EnergyHandler EnergyHandler { get; }

		public Guid UUID { get; set; }
		public BaseUIPanel UI { get; set; }
		public LegacySoundStyle CloseSound { get; }
		public LegacySoundStyle OpenSound { get; }

		private BaseLibrary.Timer timer;

		public EnergyExtractor()
		{
			timer = new BaseLibrary.Timer(15, Callback);

			Handler = new ItemHandler();
			Handler.IsItemValid += (slot, item) => item.modItem is BaseContainmentUnit;

			EnergyHandler = new EnergyHandler(10000, 1000);
		}

		private void Callback()
		{
			for (int radius = 2; radius < 16; radius++)
			{
				foreach (Point point in Utility.GetCircle(Position.X + 1, Position.Y + 1, radius))
				{
					if (Utility.InWorldBounds(point.X, point.Y) && !WorldGen.TileEmpty(point.X, point.Y) && Main.tile[point.X, point.Y].type == TileID.IceBlock)
					{
						WorldGen.KillTile(point.X, point.Y, noItem: true);

						for (int i = 0; i < 10; i++)
						{
							Vector2 start = point.ToWorldCoordinates(Main.rand.NextFloat() * 16f, Main.rand.NextFloat() * 16f);
							Vector2 end = Position.ToWorldCoordinates(24f, 24f);
							Vector2 dir = Vector2.Normalize(end - start);
							int timeLeft = (int)(Vector2.Distance(start, end) / dir.Length());

							Photon.Spawn(start, dir, new Color(0, 237, 217), timeLeft, () => EnergyHandler.InsertEnergy(100));
						}

						return;
					}
				}
			}
		}

		public override void Update()
		{
			timer.Update();

			Item item = Handler.GetItemInSlot(0);
			if (!item.IsAir && item.modItem is BaseContainmentUnit unit && unit.EnergyHandler.Energy < unit.EnergyHandler.Capacity) EnergyHandler.TransferEnergy(unit.EnergyHandler);
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