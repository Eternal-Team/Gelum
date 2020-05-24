using BaseLibrary;
using Gelum.TileEntities;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour.HookGen;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Gelum
{
	// todo: mining laser
	// todo: auto-extractinator
	// todo: crusher
	// todo: glacial capacitor - determines speed of machines

	public class Gelum : Mod
	{
		internal static Texture2D OrbTexture;
		internal static Texture2D OrbBackground;
		internal static Texture2D PhotonTexture;

		internal static readonly Photon[] Photons = new Photon[1000];

		[EditorBrowsable(EditorBrowsableState.Never)]
		public delegate void orig_LoadTileEntities(IList<TagCompound> list);

		[EditorBrowsable(EditorBrowsableState.Never)]
		public delegate void hook_LoadTileEntities(orig_LoadTileEntities orig, IList<TagCompound> list);

		public static event hook_LoadTileEntities LoadTileEntities
		{
			add => HookEndpointManager.Add<hook_LoadTileEntities>(typeof(ModLoader).Assembly.GetType("Terraria.ModLoader.IO.TileIO").GetMethod("LoadTileEntities", Utility.defaultFlags), value);
			remove => HookEndpointManager.Remove<hook_LoadTileEntities>(typeof(ModLoader).Assembly.GetType("Terraria.ModLoader.IO.TileIO").GetMethod("LoadTileEntities", Utility.defaultFlags), value);
		}

		public override void Load()
		{
			On.Terraria.Main.DrawDust += MainOnDrawDust;
			On.Terraria.Dust.UpdateDust += DustOnUpdateDust;
			LoadTileEntities += (orig, list) =>
			{
				orig(list);

				List<BaseGelumTE> tes = TileEntity.ByPosition.Select(pair => pair.Value).OfType<BaseGelumTE>().ToList();

				foreach (BaseGelumTE te in tes)
				{
					te.Network = new GelumNetwork();
					te.Network.Tiles.Add(te);
				}

				foreach (BaseGelumTE te in tes)
				{
					List<GelumNetwork> networks = te.GetNeighbors().Select(duct => duct.Network).Distinct().ToList();

					foreach (GelumNetwork other in networks.Where(other => other != te.Network))
					{
						foreach (BaseGelumTE otherTile in other.Tiles) otherTile.Network = te.Network;
						te.Network.Tiles.AddRange(other.Tiles);
						GelumNetwork.Networks.Remove(other);
					}
				}
			};

			if (!Main.dedServ)
			{
				OrbTexture = ModContent.GetTexture("Gelum/Textures/Effects/Orb").Premultiply();
				OrbBackground = ModContent.GetTexture("Gelum/Textures/Effects/OrbBackground").Premultiply();
				PhotonTexture = ModContent.GetTexture("Gelum/Textures/Effects/Photon").Premultiply();
			}
		}

		private void DustOnUpdateDust(On.Terraria.Dust.orig_UpdateDust orig)
		{
			orig();

			for (int i = 0; i < Photons.Length; i++)
			{
				if (Photons[i].active) Photons[i].Update();
			}
		}

		private void MainOnDrawDust(On.Terraria.Main.orig_DrawDust orig, Main self)
		{
			orig(self);

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			for (int i = 0; i < Photons.Length; i++)
			{
				if (Photons[i].active) Main.spriteBatch.Draw(PhotonTexture, Photons[i].position - Main.screenPosition, null, Photons[i].color, 0f, PhotonTexture.Size() * 0.5f, 0.15f, SpriteEffects.None, 0f);
			}

			Main.spriteBatch.End();
		}

		public override void Unload()
		{
			this.UnloadNullableTypes();
		}
	}
}