using BaseLibrary;
using BaseLibrary.UI;
using EnergyLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader.IO;

namespace Gelum.TileEntities
{
	public class EnergyRelay : BaseGelumTE, IHasUI, IEnergyHandler
	{
		public override Vector2 InsertionPoint => new Vector2(16, 2);

		public override Type TileType => typeof(Tiles.EnergyRelay);

		public EnergyHandler EnergyHandler { get; }

		public Guid UUID { get; set; }
		public BaseUIPanel UI { get; set; }
		public LegacySoundStyle CloseSound { get; }
		public LegacySoundStyle OpenSound { get; }

		public IEnumerable<BaseGelumTE> Connections => Network.Tiles.Where(te => !(te is EnergyRelay) && Vector2.DistanceSquared(te.Position.ToWorldCoordinates(te.InsertionPoint), Position.ToWorldCoordinates(InsertionPoint)) < 160 * 160);

		private BaseLibrary.Timer timer;

		public EnergyRelay()
		{
			EnergyHandler = new EnergyHandler(1000, 1000);

			timer = new BaseLibrary.Timer(5, Callback);
		}

		private void Callback()
		{
			if (EnergyHandler.Energy < EnergyHandler.Capacity)
			{
				foreach (BaseGelumTE tile in Connections)
				{
					if (tile is IEnergySource source && source.EnergyHandler.Energy > 0)
					{
						Vector2 start = tile.Position.ToWorldCoordinates(tile.InsertionPoint);
						Vector2 end = Position.ToWorldCoordinates(InsertionPoint);
						Vector2 dir = Vector2.Normalize(end - start);
						int timeLeft = (int)(Vector2.Distance(start, end) / dir.Length());

						long extracted = -source.EnergyHandler.ExtractEnergy(100);
						Photon.Spawn(start, dir, new Color(0, 237, 217), timeLeft, () => EnergyHandler.InsertEnergy(extracted));
					}
				}
			}

			foreach (BaseGelumTE tile in Connections)
			{
				if (tile is IEnergyReceiver receiver && receiver.EnergyHandler.Energy < receiver.EnergyHandler.Capacity)
				{
					foreach (EnergyRelay relay in Network.Tiles.OfType<EnergyRelay>())
					{
						if (relay.EnergyHandler.Energy <= 0) continue;

						Vector2 start = Position.ToWorldCoordinates(InsertionPoint);
						Vector2 end = tile.Position.ToWorldCoordinates(tile.InsertionPoint);
						Vector2 dir = Vector2.Normalize(end - start);
						int timeLeft = (int)(Vector2.Distance(start, end) / dir.Length());

						long extracted = -relay.EnergyHandler.ExtractEnergy(100);
						Photon.Spawn(start, dir, new Color(0, 237, 217), timeLeft, () => receiver.EnergyHandler.InsertEnergy(extracted));
					}
				}
			}
		}

		public override void Update() => timer.Update();

		public override TagCompound Save() => new TagCompound
		{
			["UUID"] = UUID,
			["Energy"] = EnergyHandler.Save()
		};

		public override void Load(TagCompound tag)
		{
			UUID = tag.Get<Guid>("UUID");
			EnergyHandler.Load(tag.GetCompound("Energy"));
		}
	}
}