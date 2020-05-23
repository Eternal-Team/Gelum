using BaseLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Gelum
{
	// todo: mining laser
	// todo: auto-extractinator
	// todo: crusher
	// todo: glacial capacitor - determines speed of machines

	public struct CustomDust
	{
		public Vector2 position;
		public Vector2 velocity;
		public Color color;
		public int timeLeft;
		public bool active;

		public void Update()
		{
			position += velocity;

			timeLeft--;
			if (timeLeft == 0) active = false;
		}

		private static int index;

		public static void SpawnDust(Vector2 position, Vector2 velocity, Color color, int timeLeft = -1)
		{
			Gelum.dusts[index].position = position;
			Gelum.dusts[index].velocity = velocity;
			Gelum.dusts[index].color = color;
			Gelum.dusts[index].timeLeft = timeLeft;
			Gelum.dusts[index].active = true;

			index++;
			if (index >= Gelum.dusts.Length) index = 0;
		}
	}

	public class Gelum : Mod
	{
		internal static Texture2D OrbTexture;
		internal static Texture2D OrbBackground;
		internal static Texture2D PhotonTexture;

		internal static CustomDust[] dusts = new CustomDust[1000];

		public override void Load()
		{
			On.Terraria.Main.DrawDust += MainOnDrawDust;
			On.Terraria.Dust.UpdateDust += DustOnUpdateDust;

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

			for (int i = 0; i < dusts.Length; i++)
			{
				if (dusts[i].active) dusts[i].Update();
			}
		}

		private void MainOnDrawDust(On.Terraria.Main.orig_DrawDust orig, Main self)
		{
			orig(self);

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			for (int i = 0; i < dusts.Length; i++)
			{
				if (dusts[i].active) Main.spriteBatch.Draw(PhotonTexture, dusts[i].position - Main.screenPosition, null, dusts[i].color, 0f, PhotonTexture.Size() * 0.5f, 0.1f, SpriteEffects.None, 0f);
			}

			Main.spriteBatch.End();
		}

		public override void Unload()
		{
			this.UnloadNullableTypes();
		}
	}
}