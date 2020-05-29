using BaseLibrary.Tiles.TileEntites;
using BaseLibrary.UI;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader.IO;

namespace Gelum.TileEntities
{
	public class Timer : BaseTE, IHasUI
	{
		public override Type TileType => typeof(Tiles.Timer);

		public Guid UUID { get; set; }
		public BaseUIPanel UI { get; set; }
		public LegacySoundStyle CloseSound { get; }
		public LegacySoundStyle OpenSound { get; }

		private BaseLibrary.Timer timer;
		public bool active=true;

		public Timer()
		{
			timer = new BaseLibrary.Timer(60, Callback);
		}

		private void Callback()
		{
			Wiring.TripWire(Position.X, Position.Y, 1, 1);
		}

		public override void Update()
		{
			if (active) timer.Update();
		}

		public override TagCompound Save() => new TagCompound
		{
			["UUID"] = UUID,
			["Active"] = active,
			["Interval"] = timer.Interval
		};

		public override void Load(TagCompound tag)
		{
			UUID = tag.Get<Guid>("UUID");
			active = tag.GetBool("Active");
			timer.Interval = tag.GetInt("Interval");
		}
	}
}