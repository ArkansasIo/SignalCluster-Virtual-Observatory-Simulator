namespace SignalCluster.Core;
public sealed record LogicalDrive(int Slot, string Name, string Role, int CapacityTB, bool IsPhysical=false);
public static class StorageInventory
{
    public static IReadOnlyList<LogicalDrive> CreateDefault(int capacityTB=32)
    {
        if(capacityTB<1 || capacityTB>1000) throw new ArgumentOutOfRangeException(nameof(capacityTB));
        return Enumerable.Range(1,24).Select(i=>new LogicalDrive(i,$"Drive {i:D2}",i==1?"Client / management":"Server / signal data",capacityTB,false)).ToArray();
    }
    public static long RawCapacityTB(IEnumerable<LogicalDrive> drives)
    {
        ArgumentNullException.ThrowIfNull(drives);
        return drives.Aggregate(0L,(total,d)=>checked(total+d.CapacityTB));
    }
}