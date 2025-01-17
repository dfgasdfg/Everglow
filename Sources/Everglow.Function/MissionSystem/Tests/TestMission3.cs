using Everglow.Commons.MissionSystem.MissionAbstracts;
using Everglow.Commons.MissionSystem.MissionTemplates;

namespace Everglow.Commons.MissionSystem.Tests;

public class TestMission3 : GainItemMission
{
	public override List<GainItemRequirement> DemandGainItems { get; init; } = [
		GainItemRequirement.Create([ItemID.IronOre], 1000)];

	public override List<Item> RewardItems => [
		new Item(ItemID.Zenith, 1000)];

	public override string DisplayName => "获取10个铁矿";

	public override string Description => "测试介绍3";

	public override MissionType MissionType => MissionType.MainStory;

	public static TestMission3 Create() => new TestMission3();
}