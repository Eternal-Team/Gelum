using BaseLibrary;
using BaseLibrary.UI;
using EnergyLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Gelum.UI
{
	public class UIEnergyStore : BaseElement
	{
		private IEnergyHandler energyHandler;
		public EnergyHandler Handler => energyHandler.EnergyHandler;

		public UIEnergyStore(IEnergyHandler energyHandler)
		{
			this.energyHandler = energyHandler;
		}

		protected override void Update(GameTime gameTime)
		{
			HoverText = Handler.Energy + "/" + Handler.Capacity + " DE";
		}

		private float angle;

		protected override void Draw(SpriteBatch spriteBatch)
		{
			angle -= 0.01f;
			float scale = (Handler.Energy / (float)Handler.Capacity).Remap(0f, 1f, 0.05f, 1f);

			spriteBatch.Draw(Gelum.OrbBackground, Utility.Center(Dimensions), null, Color.White, 0f, Gelum.OrbTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(Gelum.OrbTexture, Utility.Center(Dimensions), null, new Color(0, 237, 217), angle, Gelum.OrbTexture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
		}
	}
}