using BaseLibrary.UI;
using Gelum.TileEntities;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Gelum.UI
{
	public class EnergyRelayPanel : BaseUIPanel<EnergyRelay>
	{
		public EnergyRelayPanel(EnergyRelay container) : base(container)
		{
			Width.Pixels = 272;
			Height.Pixels = 300;
			BackgroundColor = new Color(38, 49, 90);

			UIText textLabel = new UIText(Language.GetText("Mods.Gelum.MapObject.EnergyRelay"))
			{
				X = { Percent = 50 },
				HorizontalAlignment = HorizontalAlignment.Center
			};
			Add(textLabel);

			UIEnergyStore energy = new UIEnergyStore(container)
			{
				Width = { Pixels = 256 },
				Height = { Pixels = 256 },
				X = { Percent = 50 },
				Y = { Pixels = 28 }
			};
			Add(energy);
		}
	}
}