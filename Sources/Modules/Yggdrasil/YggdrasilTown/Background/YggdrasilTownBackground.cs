using Everglow.Commons.DataStructures;
using Everglow.Yggdrasil.Common.BackgroundManager;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;
using SubworldLibrary;
using static Everglow.Yggdrasil.WorldGeneration.YggdrasilWorldGeneration;

namespace Everglow.Yggdrasil.YggdrasilTown.Background;

public class YggdrasilTownBackground : ModSystem
{
	/// <summary>
	/// 地形中心
	/// </summary>
	public static Vector2 BiomeCenter => new Vector2(Main.maxTilesX / 2f * 16, (Main.maxTilesY - 1000) * 16);

	/// <summary>
	/// 初始化
	/// </summary>
	public override void OnModLoad()
	{
		if (Main.netMode != NetmodeID.Server)
		{
			Ins.HookManager.AddHook(CodeLayer.PostDrawBG, DrawBackground);
		}
	}

	// Total background alpha.
	public float BackgroundAlpha = 0f;

	/// <summary>
	/// LampWoodAnchor Y
	/// </summary>
	public float LampWoodCenterY = 0;

	/// <summary>
	/// Background alpha of lampwood
	/// </summary>
	public float BackgroundAlphaLampWood = 0f;

	/// <summary>
	/// Background alpha of lampwood
	/// </summary>
	public float BackgroundAlphaTwilight = 0f;

	public override void PostUpdateEverything()// 开启地下背景
	{
		const float increase = 0.02f;
		if (BiomeActive() && Main.BackgroundEnabled)
		{
			if (BackgroundAlpha < 1)
			{
				BackgroundAlpha += increase;
			}
			else
			{
				BackgroundAlpha = 1;
				Ins.HookManager.Disable(TerrariaFunction.DrawBackground);
			}
		}
		else
		{
			if (BackgroundAlpha > 0)
			{
				BackgroundAlpha -= increase;
			}
			else
			{
				BackgroundAlpha = 0;
			}
			Ins.HookManager.Enable(TerrariaFunction.DrawBackground);
		}
	}

	/// <summary>
	/// 判定是否开启地形
	/// </summary>
	/// <returns></returns>
	public static bool BiomeActive()
	{
		if (Main.screenPosition.Y > (BiomeCenter.Y - 16000))
		{
			if (SubworldSystem.IsActive<YggdrasilWorld>())
			{
				YggdrasilEnvironmentLightManager.LightingScene = YggdrasilScene.YggdrasilTown;
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// 绘制天穹城镇背景
	/// </summary>
	/// <param name="baseColor"></param>
	private void DrawYggdrasilTownBackground(Color baseColor)
	{
		var texSky = ModAsset.YggdrasilTownBackgroundSky.Value;
		var texClose = ModAsset.Great_Outpost.Value;
		var texC1 = ModAsset.YggdrasilTownBackgroundC1.Value;
		var texC2 = ModAsset.YggdrasilTownBackgroundC2.Value;
		var texC3 = ModAsset.YggdrasilTownBackgroundC3.Value;
		var texBound = ModAsset.KelpCurtainBound.Value;

		Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
		if (SubworldSystem.Current != null)
		{
			YggdrasilWorld yWorld = SubworldSystem.Current as YggdrasilWorld;
			if (yWorld != null)
			{
				// 获取挑战者石牢的位置
				if (yWorld.StoneCageOfChallengesCenter == Vector2.zeroVector)
				{
					for (int x = 50; x < Main.maxTilesX - 50; x++)
					{
						for (int y = 50; y < Main.maxTilesY - 50; y++)
						{
							Tile tile = SafeGetTile(x, y);
							if (tile.TileType == ModContent.TileType<SquamousShellSeal>())
							{
								if (tile.TileFrameX == 180 && tile.TileFrameY == 162)
								{
									yWorld.StoneCageOfChallengesCenter = new Vector2(x, y - 40) * 16;
									Main.NewText((x, y));
									x = Main.maxTilesX - 50;
									break;
								}
							}
						}
					}
					if (yWorld.StoneCageOfChallengesCenter == Vector2.zeroVector)
					{
						yWorld.StoneCageOfChallengesCenter = new Vector2(Main.maxTilesX * 16 / 2, Main.maxTilesY - 9000);
					}
				}

				// 如果在挑战者石牢附近
				// if (Math.Abs(yWorld.StoneCageOfChallengesCenter.X - screenCenter.X) < 240 * 16)
				// {
				// if (Math.Abs(yWorld.StoneCageOfChallengesCenter.Y - screenCenter.Y) < 60 * 16)
				// {
				// var stoneClose2 = ModAsset.StoneCageOfChallengesClose2.Value;
				// var stoneClose = ModAsset.StoneCageOfChallengesClose.Value;
				// var stoneMiddle = ModAsset.StoneCageOfChallengesMiddle.Value;
				// var stoneFar = ModAsset.StoneCageOfChallengesFar.Value;
				// var stoneSky = ModAsset.StoneCageOfChallengesSky.Value;
				// Vector2 correction = yWorld.StoneCageOfChallengesCenter + new Vector2(0, -8000) - BiomeCenter;
				// float setSize = 1f;
				// BackgroundManager.QuickDrawBG(stoneSky, GetDrawRect(stoneSky.Size(), 0f, correction, setSize), baseColor, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), true, true);
				// BackgroundManager.QuickDrawBG(stoneFar, GetDrawRect(stoneFar.Size(), 0.05f, correction, setSize), baseColor, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), true, true);
				// BackgroundManager.QuickDrawBG(stoneMiddle, GetDrawRect(stoneMiddle.Size(), 0.10f, correction + new Vector2(0, 5000), setSize), baseColor, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), true, true);
				// BackgroundManager.QuickDrawBG(stoneClose, GetDrawRect(stoneFar.Size(), 0.15f, correction + new Vector2(0, 6000), setSize), baseColor, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), true, true);
				// BackgroundManager.QuickDrawBG(stoneClose2, GetDrawRect(stoneMiddle.Size(), 0.20f, correction + new Vector2(0, 7000), setSize), baseColor, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), true, true);
				// return;
				// }
				// }
				// if (ModContent.GetInstance<LampWoodForest>().IsBiomeActive(Main.LocalPlayer))
				// {

				// }
				// else
				// {
				// if (Math.Abs(LampWoodCenterY - Main.screenPosition.Y) > 200)
				// {
				// LampWoodCenterY = Main.screenPosition.Y;
				// }
				// if (BackgroundSwitchingAlpha > 0f)
				// {
				// BackgroundSwitchingAlpha -= 0.02f;
				// }
				// else
				// {
				// BackgroundSwitchingAlpha = 0f;
				// }
				// }
			}
		}

		// 旧背景
		BackgroundManager.QuickDrawBG(texSky, float.PositiveInfinity, BiomeCenter, baseColor, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 16000));

		DrawLampWood(baseColor);
		DrawTwilightForsetAndRelic(baseColor);
	}

	public void DrawLampWood(Color baseColor)
	{
		if (ModContent.GetInstance<LampWoodForest>().IsBiomeActive(Main.LocalPlayer))
		{
			if(Math.Abs(LampWoodCenterY - Main.screenPosition.Y) < Main.screenHeight)
			{
				if (BackgroundAlphaLampWood < 1f)
				{
					BackgroundAlphaLampWood += 0.02f;
				}
				else
				{
					BackgroundAlphaLampWood = 1f;
				}
			}
			else
			{
				if (BackgroundAlphaLampWood > 0f)
				{
					BackgroundAlphaLampWood -= 0.02f;
				}
				else
				{
					LampWoodCenterY = Main.screenPosition.Y;
					BackgroundAlphaLampWood = 0;
				}
			}
		}
		else
		{
			if (BackgroundAlphaLampWood > 0f)
			{
				BackgroundAlphaLampWood -= 0.02f;
			}
			else
			{
				BackgroundAlphaLampWood = 0;
			}
		}

		if(BackgroundAlphaLampWood > 0)
		{
			var lampClose = ModAsset.LampWoodClose.Value;
			var lampMiddle = ModAsset.LampWoodMiddle.Value;
			var lampFar = ModAsset.LampWoodFar.Value;
			var lampSky = ModAsset.LampWoodSky.Value;
			Vector2 correction = new Vector2(0, LampWoodCenterY - 4000);

			BackgroundManager.QuickDrawBG(lampSky, float.PositiveInfinity, correction, baseColor * BackgroundAlphaLampWood, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), false, true);
			BackgroundManager.QuickDrawBG(lampFar, 20f, correction, baseColor * BackgroundAlphaLampWood, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), false, true);
			BackgroundManager.QuickDrawBG(lampMiddle, 10f, correction, baseColor * BackgroundAlphaLampWood, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), false, true);
			BackgroundManager.QuickDrawBG(lampClose, 6f, correction + new Vector2(0, 6000), baseColor * BackgroundAlphaLampWood, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), false, true);
		}
	}

	public void DrawTwilightForsetAndRelic(Color baseColor)
	{
		if (ModContent.GetInstance<TwilightForsetAndRelic>().IsBiomeActive(Main.LocalPlayer))
		{
			if (BackgroundAlphaTwilight < 1f)
			{
				BackgroundAlphaTwilight += 0.02f;
			}
			else
			{
				BackgroundAlphaTwilight = 1f;
			}
		}
		else
		{
			if (BackgroundAlphaTwilight > 0f)
			{
				BackgroundAlphaTwilight -= 0.02f;
			}
			else
			{
				BackgroundAlphaTwilight = 0;
			}
		}

		if (BackgroundAlphaTwilight > 0)
		{
			var twilightClose = ModAsset.TwilightClose.Value;
			var twilightMiddleClose = ModAsset.TwilightMiddleClose.Value;
			var twilightMiddle = ModAsset.TwilightMiddle.Value;
			var twilightMiddleFar = ModAsset.TwilightMiddleFar.Value;
			var twilightFar = ModAsset.TwilightFar.Value;
			var twilightSky = ModAsset.TwilightSky.Value;
			BackgroundManager.QuickDrawBG(twilightSky, float.PositiveInfinity, BiomeCenter, baseColor * BackgroundAlphaTwilight, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), true, true);
			BackgroundManager.QuickDrawBG(twilightFar, 40, BiomeCenter, baseColor * BackgroundAlphaTwilight, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), true, true);
			BackgroundManager.QuickDrawBG(twilightMiddleFar, 30, BiomeCenter, baseColor * BackgroundAlphaTwilight, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), true, true);
			BackgroundManager.QuickDrawBG(twilightMiddle, 16, BiomeCenter, baseColor * BackgroundAlphaTwilight, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), true, true);
			BackgroundManager.QuickDrawBG(twilightMiddleClose, 10, BiomeCenter, baseColor * BackgroundAlphaTwilight, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), true, true);
			BackgroundManager.QuickDrawBG(twilightClose, 5, BiomeCenter, baseColor * BackgroundAlphaTwilight, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), true, true);
		}
	}

	/// <summary>
	/// 当然是绘制主体啦
	/// </summary>
	private void DrawBackground()
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		SpriteBatchState sBS2 = sBS;
		sBS2.BlendState = BlendState.NonPremultiplied;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS2);
		if (BackgroundAlpha <= 0)
		{
			return;
		}

		Color baseColor = Color.White * BackgroundAlpha;
		DrawYggdrasilTownBackground(baseColor);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}
}