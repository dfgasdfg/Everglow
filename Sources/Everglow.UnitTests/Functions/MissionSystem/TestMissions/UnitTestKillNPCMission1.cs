﻿using Everglow.Commons.MissionSystem.MissionAbstracts;
using Everglow.Commons.MissionSystem.MissionTemplates;
using Terraria;

namespace Everglow.UnitTests.Functions.MissionSystem.TestMissions;

public class UnitTestKillNPCMission1 : KillNPCMission
{
	public UnitTestKillNPCMission1(List<KillNPCRequirement> requires)
	{
		DemandNPCs = requires;
	}

	public override List<KillNPCRequirement> DemandNPCs { get; init; }

	public override List<Item> RewardItems => [];

	public override string DisplayName => nameof(UnitTestGainItemMission1);

	public override string Description => nameof(UnitTestGainItemMission1);
}