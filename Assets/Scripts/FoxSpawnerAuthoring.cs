using Unity.Entities;
using UnityEngine;


public struct FoxSpawner : IComponentData
{
    public Entity foxPrefab;
    public uint amountToSpawn;
    public uint amountSpawned;
}

public class FoxSpawnerAuthoring : MonoBehaviour
{
    public GameObject FoxPrefab;
    public uint AmountToSpawn;

    public class FoxSpawnerBaker : Baker<FoxSpawnerAuthoring>
    {
        public override void Bake(FoxSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity,
                new FoxSpawner
                {
                    foxPrefab = GetEntity(authoring.FoxPrefab, TransformUsageFlags.Dynamic),
                    amountToSpawn = authoring.AmountToSpawn
                });
        }
    }
}
