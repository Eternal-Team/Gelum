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
	public class EnergyExtractor : BaseTE, IItemHandler, IHasUI, IEnergyHandler
	{
		public override Type TileType => typeof(Tiles.EnergyExtractor);

		public ItemHandler Handler { get; }
		public EnergyHandler EnergyHandler { get; }

		public Guid UUID { get; set; }
		public BaseUIPanel UI { get; set; }
		public LegacySoundStyle CloseSound { get; }
		public LegacySoundStyle OpenSound { get; }

		private Timer timer;

		public EnergyExtractor()
		{
			timer = new Timer(60, Callback);

			Handler = new ItemHandler();
			Handler.IsItemValid += (slot, item) => item.modItem is BaseContainmentUnit;

			EnergyHandler = new EnergyHandler(1000000, 1000);
		}

		private void Callback()
		{
			for (int i = Position.X - 10; i < Position.X + 10; i++)
			{
				for (int j = Position.Y - 10; j < Position.Y + 10; j++)
				{
					// todo: check if tile in within world bounds

					if (!WorldGen.TileEmpty(i, j) && Main.tile[i, j].type == TileID.IceBlock)
					{
						WorldGen.KillTile(i, j, noItem: true);
						
						WorldGen.PlaceTile(i, j, TileID.SnowBlock);
						
						EnergyHandler.InsertEnergy(1000);

						for (int k = 0; k < 5; k++)
						{
							Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 113, 1f, 1f);
						}

						return;
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