using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class StoneBridgeTile : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;
		Main.tileSolid[Type] = true;
		Main.tileWaterDeath[Type] = false;
		Main.tileBlendAll[Type] = false;
		Main.tileBlockLight[Type] = true;
		this.MinPick = int.MaxValue;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 1;
		TileObjectData.newTile.Width = 1;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16
		};
		TileObjectData.newTile.CoordinateWidth = 16;
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.addTile(Type);
		AddMapEntry(new Color(64, 64, 61));
	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}
	public override bool CanKillTile(int i, int j, ref bool blockDamaged)
	{
		return false;
	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Tile tile = Main.tile[i, j];
		Color c = Lighting.GetColor(i, j);
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
			zero = Vector2.Zero;
		spriteBatch.Draw(ModAsset.StoneBridge.Value, new Vector2(i, j - 4) * 16 + zero - Main.screenPosition, new Rectangle((int)(tile.TileFrameX / 18f * 16), 0, 16, 320), c, 0, new Vector2(0), 1f, SpriteEffects.None, 0);
		return false;
	}
	public override void NearbyEffects(int i, int j, bool closer)
	{

	}
}