using Latios.Authoring;
using Latios.Kinemation;
using Latios.Kinemation.Authoring;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace DefaultNamespace
{
    public struct Fox : IComponentData
    {
        public BlobAssetReference<SkeletonClipSetBlob> blob;
    }

    [DisallowMultipleComponent]
    public class FoxAuthoring : MonoBehaviour
    {
        public class FoxBaker : Baker<FoxAuthoring>
        {
            public override void Bake(FoxAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<Fox>(entity);
            }
        }
    }
}