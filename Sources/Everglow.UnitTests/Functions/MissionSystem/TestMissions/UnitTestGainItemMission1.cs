using Everglow.Commons.MissionSystem.Shared;
using Everglow.Commons.MissionSystem.Templates;
using Terraria;

namespace Everglow.UnitTests.Functions.MissionSystem.TestMissions;

public class UnitTestGainItemMission1 : GainItemMission
{
	public UnitTestGainItemMission1()
	{
		DemandGainItems = [];
	}

	public UnitTestGainItemMission1(List<GainItemRequirement> requires)
	{
		DemandGainItems = requires;
	}

	public override List<GainItemRequirement> DemandGainItems { get; init; }

	public override List<Item> RewardItems => [];

	public override string DisplayName => nameof(UnitTestGainItemMission1);

	public override string Description => nameof(UnitTestGainItemMission1);
}
