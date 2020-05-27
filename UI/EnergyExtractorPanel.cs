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
		public ItemHandler Handler => Container.Handler;

		public EnergyExtractorPanel(EnergyExtractor container) : base(container)
		{
			Width.Pixels = 272;
			Height.Pixels = 316 + 54;
			BackgroundColor = new Color(38, 49, 90);

			UIText textLabel = new UIText(Language.GetText("Mods.Gelum.MapObject.EnergyExtractor"))
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

			UIContainerSlot slot = new UIContainerSlot(() => Handler)
			{
				X = { Percent = 50 },
				Y = { Percent = 100 },
				Width = { Pixels = 54 },
				Height = { Pixels = 54 }
			};
			Add(slot);
		}
	}
}