namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class AuburnBell : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
		Main.buffNoTimeDisplay[Type] = true;
	}
}