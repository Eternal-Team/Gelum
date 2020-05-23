using BaseLibrary.UI;
using ContainerLibrary;
using Gelum.TileEntities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;

namespace Gelum.UI
{
	public class EnergyExtractorPanel : BaseUIPanel<EnergyExtractor>, IItemHandlerUI
	{
		public string GetTexture(Item item) => "Gelum/Textures/Items/EnergyExtractor";
		public ItemHandler Handler { get; }

		private UIText textEnergy;

		public EnergyExtractorPanel(EnergyExtractor container) : base(container)
		{
			Width.Pixels = 16 + 200;
			Height.Pixels = 44 + 200;

			UIText textLabel = new UIText(Language.GetText("Mods.Gelum.MapObject.EnergyExtractor"))
			{
				X = { Percent = 50 },
				HorizontalAlignment = HorizontalAlignment.Center
			};
			Add(textLabel);

			textEnergy = new UIText("")
			{
				X = { Percent = 50 },
				Y = { Pixels = 28 },
				HorizontalAlignment = HorizontalAlignment.Center
			};
			Add(textEnergy);
		}

		protected override void Update(GameTime gameTime)
		{
			textEnergy.Text = $"Energy: {Container.EnergyHandler.Energy}/{Container.EnergyHandler.Capacity} DE";

			base.Update(gameTime);
		}
	}
}