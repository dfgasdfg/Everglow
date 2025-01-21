using System.Text;
using Everglow.Commons.MissionSystem.Abstracts;
using Everglow.Commons.MissionSystem.Core;
using Everglow.Commons.MissionSystem.Shared;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.Templates.Abstracts;

public interface IUseItemMission : IMissionObjective
{
	public abstract List<CountItemRequirement> DemandUseItems { get; init; }

	/// <summary>
	/// Count kill for each demand group
	/// </summary>
	/// <param name="type">The type of NPC</param>
	/// <param name="count">The count of kill. Default to 1.</param>
	public void CountUse(Item item)
	{
		foreach (var di in DemandUseItems.Where(x => x.Items.Contains(item.type)))
		{
			di.Count();
		}
	}

	/// <summary>
	/// Calculate mission progress
	/// </summary>
	/// <returns></returns>
	public float CalculateProgress()
	{

		if (DemandUseItems.Count == 0 || DemandUseItems.Select(x => x.Requirement).Sum() == 0)
		{
			return 1f;
		}

		return DemandUseItems.Select(x => x.Progress()).Average();
	}

	public void Load(TagCompound tag)
	{
		tag.TryGet<List<CountItemRequirement>>(nameof(DemandUseItems), out var demandNPCs);
		if (demandNPCs != null && demandNPCs.Count != 0)
		{
			foreach (var demand in DemandUseItems)
			{
				demand.Count(
					demandNPCs
						.Where(d => d.Items.Intersect(demand.Items).Any())
						.Sum(x => x.Counter));
			}
		}

		MissionBase.LoadVanillaItemTextures(DemandUseItems.SelectMany(x => x.Items));
	}

	public void Save(TagCompound tag)
	{
		tag.Add(nameof(DemandUseItems), DemandUseItems);
	}

	public string GetObjectivesString()
	{
		var objectives = new StringBuilder();

		foreach (var demand in DemandUseItems)
		{
			if (demand.Items.Count > 1)
			{
				var itemString = string.Join(' ', demand.Items.ConvertAll(i => $"[ItemDrawer,Type='{i}']"));
				objectives.Append($"使用{itemString}合计{demand.Requirement}次\n");
			}
			else
			{
				objectives.Append($"使用[ItemDrawer,Type='{demand.Items.First()}']{demand.Requirement}次\n");
			}
		}

		return objectives.ToString();
	}
}