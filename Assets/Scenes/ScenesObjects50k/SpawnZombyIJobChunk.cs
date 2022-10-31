using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public class SpawnZombyIJobChunk : IJobChunk
{
    public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
    {

    }
}
