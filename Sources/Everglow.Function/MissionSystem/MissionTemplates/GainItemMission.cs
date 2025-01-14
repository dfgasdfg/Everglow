using Everglow.Commons.MissionSystem.MissionAbstracts;
using Everglow.Commons.MissionSystem.MissionIcons;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.MissionTemplates;

public abstract class GainItemMission : MissionBase, IGainItemMission, IRewardItemMission
{
	private float progress = 0f;

	public override float Progress => progress;

	public override MissionIconGroup Icon => new MissionIconGroup(
		[
			ItemMissionIcon.Create(DemandItems.Count > 0
				? DemandItems.First().Items.First()
				: 1)
		]);

	public virtual bool Consume => false;

	public abstract List<GainItemRequirement> DemandItems { get; }

	public abstract List<Item> RewardItems { get; }

	public override void PostComplete()
	{
		(this as IGainItemMission).ConsumeItem(Main.LocalPlayer.inventory);
		(this as IRewardItemMission).GiveReward();
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);

		LoadVanillaItemTextures(
			DemandItems.SelectMany(x => x.Items)
			.Concat(RewardItems.Select(x => x.type)));
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);
	}

	public override void Update()
	{
		base.Update();

		UpdateProgress();
	}

	public override void UpdateProgress(params object[] objs)
	{
		if (PoolType != MissionManager.PoolType.Accepted)
		{
			return;
		}

		progress = (this as IGainItemMission).CalculateProgress(Main.LocalPlayer.inventory);
	}
}