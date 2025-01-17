using Everglow.Commons.MissionSystem.MissionAbstracts;
using Everglow.Commons.MissionSystem.MissionIcons;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.MissionTemplates;

public abstract class GainItemMission : MissionBase, IGainItemMission, IRewardItemMission
{
	private float progress = 0f;

	public override float Progress => progress;

	public override MissionIconGroup Icon => new MissionIconGroup(
		DemandGainItems.SelectMany(s => s.Items).Select(i => ItemMissionIcon.Create(i)));

	public virtual bool SubmitItemsOnComplete => false;

	public abstract List<GainItemRequirement> DemandGainItems { get; init; }

	public abstract List<Item> RewardItems { get; }

	public override void PostComplete()
	{
		(this as IGainItemMission).ConsumeItem(Main.LocalPlayer.inventory);
		(this as IRewardItemMission).GiveReward();
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);

		(this as IGainItemMission).Load(tag);
		(this as IRewardItemMission).Load(tag);
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);

		(this as IGainItemMission).Save(tag);
	}

	public override void UpdateProgress(params object[] objs)
	{
		progress = (this as IGainItemMission).CalculateProgress(Main.LocalPlayer.inventory);
	}
}