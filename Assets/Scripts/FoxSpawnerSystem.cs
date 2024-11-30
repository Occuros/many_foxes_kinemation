using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Latios.Transforms;


namespace DefaultNamespace
{
    public partial struct FoxSpawnerSystem : ISystem
    {
        private const float RING_SPACING = 2.0f;
        private const float FOX_SPACING = 2.0f;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FoxSpawner>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var foxQuery = SystemAPI.QueryBuilder().WithAll<Fox>().Build();
            if (foxQuery.CalculateEntityCount() > 0) {return;}

            var foxSpawner = SystemAPI.GetSingleton<FoxSpawner>();
            var foxedRemaining = foxSpawner.amountToSpawn;
            var radius = RING_SPACING;
            var ringIndex = 0;
           
            while (foxedRemaining > 0)
            {
                var baseRotation = quaternion.identity;

                var circumference = math.TAU * radius;
                var foxesInRing = (uint)(circumference / FOX_SPACING );
                foxesInRing = math.min(foxesInRing, foxedRemaining);
                var foxesSpacingAngle = circumference / (foxesInRing * radius);

                for (int foxI = 0; foxI < foxesInRing; foxI++)
                {
                    var foxAngle = foxI * foxesSpacingAngle;
                    var s = math.sin(foxAngle);
                    var c = math.cos(foxAngle);
                    var x = radius * c;
                    var z = radius * s;
                    var foxEntity = state.EntityManager.Instantiate(foxSpawner.foxPrefab);

                    var foxTransform = SystemAPI.GetAspect<TransformAspect>(foxEntity);
                    foxTransform.worldPosition = new float3(x, 0.0f, z);
                    foxTransform.worldRotation = math.mul(baseRotation, quaternion.RotateY(-foxAngle));
                    foxTransform.worldScale = 1.0f;
                }

                foxedRemaining -= foxesInRing;
                radius += RING_SPACING;
                ringIndex += 1;

            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}