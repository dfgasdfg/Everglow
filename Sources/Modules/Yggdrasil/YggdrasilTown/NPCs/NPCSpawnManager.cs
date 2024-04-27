using SubworldLibrary;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs;

public class NPCSpawnManager : GlobalNPC
{
	private static HashSet<int> yggdrasilNPC = new HashSet<int>();
	public static void RegisterNPC(int type) => yggdrasilNPC.Add(type);
	public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
	{
		if (!SubworldSystem.IsActive<YggdrasilWorld>())
		{
			return;
		}

		var dict = new Dictionary<int, float>(yggdrasilNPC.Count);
		foreach(var pair in pool)
		{
			if(yggdrasilNPC.Contains(pair.Key))
			{
				dict.Add(pair.Key, pair.Value);
			}
		}
		pool.Clear();
		foreach(var pair in dict)
		{
			pool.Add(pair.Key, pair.Value);
		}
	}
	public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
	{
		if (SubworldSystem.IsActive<YggdrasilWorld>())
		{
			float fix;
			float x = player.Center.X - 600 * 16;
			float y = player.Center.Y - 11630 * 16;
			var v = FindTheIntersectionOfEllipseAndLine(1 / x, -1 / y, 300 * 16, 150 * 16);
			float l = v.Length();
			fix = new Vector2(x, y).Length() / l;
			if (fix < 1)
			{
				fix *= fix;
				spawnRate = (int)(1 / fix * spawnRate);
				maxSpawns = (int)(fix * maxSpawns);
			}
		}
	}
	public static Vector2 FindTheIntersectionOfEllipseAndLine(float a, float b, float c, float d)
	{
		float a2 = (float)Math.Pow(a, 2);
		float b2 = (float)Math.Pow(b, 2);
		float c2 = (float)Math.Pow(c, 2);
		float d2 = (float)Math.Pow(d, 2);
		float x = (float)Math.Pow(b2 * c2 * d2 / (b2 * d2 + a2 * c2), 0.5);
		return new(x, -a / b * x);
	}
}