using BaseLibrary.UI;
using Gelum.TileEntities;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Gelum.UI
{
	public class TimerPanel : BaseUIPanel<Timer>
	{
		public TimerPanel(Timer container) : base(container)
		{
			Width.Pixels = 272;
			Height.Pixels = 348;
			BackgroundColor = new Color(38, 49, 90);

			UIText textLabel = new UIText(Language.GetText("Mods.Gelum.MapObject.Timer"))
			{
				X = { Percent = 50 },
				HorizontalAlignment = HorizontalAlignment.Center
			};
			Add(textLabel);

			UIText textActive = new UIText(container.active ? Language.GetText("Mods.Gelum.UI.Active") : Language.GetText("Mods.Gelum.UI.Inactive"))
			{
				X = { Percent = 50 },
				Y = { Pixels = 28 },
				Width = { Percent = 100 },
				Height = { Pixels = 20 },
				HorizontalAlignment = HorizontalAlignment.Center
			};
			textActive.OnClick += _ =>
			{
				container.active = !container.active;
				textActive.Text = container.active ? Language.GetText("Mods.Gelum.UI.Active") : Language.GetText("Mods.Gelum.UI.Inactive");
			};
			Add(textActive);
		}
	}
}