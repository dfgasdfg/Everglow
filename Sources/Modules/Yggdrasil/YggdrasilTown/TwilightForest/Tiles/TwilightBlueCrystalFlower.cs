using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Tiles;

public class TwilightBlueCrystalFlower : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileLavaDeath[Type] = false;

		Main.tileWaterDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 3;
		TileObjectData.newTile.Width = 4;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			18
		};
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.addTile(Type);
		AddMapEntry(new Color(40, 80, 148));
	}
	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		r = 0.05f;
		g = 0.25f;
		b = 0.45f;
		base.ModifyLight(i, j, ref r, ref g, ref b);
	}
	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Color lightColor = Lighting.GetColor(i, j);
		Tile tile = Main.tile[i, j];
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

		if (Main.drawToScreen)
			zero = Vector2.Zero;
		spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, new Vector2(i, j) * 16 - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), lightColor * 3.1f, 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);
	}
}