using BaseLibrary;
using BaseLibrary.Items;
using EnergyLibrary;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Gelum.Items
{
	public abstract class BaseContainmentUnit : BaseItem, IEnergyHandler
	{
		public override bool CloneNewInstances => false;

		public EnergyHandler EnergyHandler { get; protected set; }
	
		public override ModItem Clone()
		{
			BaseContainmentUnit clone = (BaseContainmentUnit)base.Clone();
			clone.EnergyHandler = EnergyHandler.Clone();
			return clone;
		}

		public override ModItem Clone(Item item)
		{
			var clone = Clone();
			clone.SetValue("item", item);
			return clone;
		}

		public override ModItem NewInstance(Item itemClone)
		{
			ModItem copy = (ModItem)Activator.CreateInstance(GetType());
			copy.SetValue("item", itemClone);
			copy.SetValue("mod", mod);
			copy.SetValue("Name", Name);
			copy.SetValue("DisplayName", DisplayName);
			copy.SetValue("Tooltip", Tooltip);
			return copy;
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 1;
			item.rare = ItemRarityID.Pink;
			item.value = Item.sellPrice(gold: 8);
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			// todo: make this a visual bar in tooltip
			
			tooltips.Add(new TooltipLine(mod, "Energy", $"Stored energy: {EnergyHandler.Energy}/{EnergyHandler.Capacity} DE"));
		}

		public override TagCompound Save() => new TagCompound
		{
			["Energy"] = EnergyHandler.Save()
		};

		public override void Load(TagCompound tag)
		{
			EnergyHandler.Load(tag.GetCompound("Energy"));
		}
	}

	public class BasicContainmentUnit : BaseContainmentUnit
	{
		public BasicContainmentUnit()
		{
			EnergyHandler = new EnergyHandler(10000, 1000);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.GoldBar, 2);
			recipe.AddIngredient(ItemID.Sapphire);
			recipe.SetResult(this);
		}
	}
	
	public class AdvancedContainmentUnit : BaseContainmentUnit
	{
		public AdvancedContainmentUnit()
		{
			EnergyHandler = new EnergyHandler(100000, 10000);
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HellstoneBar, 2);
			recipe.AddIngredient(ItemID.Sapphire);
			recipe.SetResult(this);
		}
	}
	
	public class EliteContainmentUnit : BaseContainmentUnit
	{
		public EliteContainmentUnit()
		{
			EnergyHandler = new EnergyHandler(1000000, 100000);
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedBar, 2);
			recipe.AddIngredient(ItemID.Sapphire);
			recipe.SetResult(this);
		}
	}
}