using System.ComponentModel;
using Dragons.Authoring;
using Latios.Kinemation;
using Latios.Transforms;
using Latios.Transforms.Systems;
using Unity.Burst;
using Unity.Entities;
using Unity.Collections;


using static Unity.Entities.SystemAPI;

namespace DefaultNamespace
{
    [UpdateBefore(typeof(TransformSuperSystem))]
    // [DisableAutoCreation]
    public partial struct SingleClipPlayerSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // new ExposedJob { clipLookup = GetComponentLookup<SingleClip>(true), et = (float)Time.ElapsedTime }.ScheduleParallel();
            new OptimizedJob() { et = (float)Time.ElapsedTime }.ScheduleParallel();

        }

        [BurstCompile]
        partial struct ExposedJob : IJobEntity
        {
            [Unity.Collections.ReadOnly] public ComponentLookup<SingleClip> clipLookup;
            public float et;

            public void Execute(TransformAspect transform, in BoneIndex boneIndex, in BoneOwningSkeletonReference skeletonRef)
            {
                if (boneIndex.index <= 0 || !clipLookup.HasComponent(skeletonRef.skeletonRoot))
                    return;

                ref var clip = ref clipLookup[skeletonRef.skeletonRoot].blob.Value.clips[0];
                var clipTime = clip.LoopToClipTime(et);

                transform.localTransformQvvs = clip.SampleBone(boneIndex.index, clipTime);
            }
        }
    }
    
    [BurstCompile]
    partial struct OptimizedJob : IJobEntity
    {
        public float et;

        public void Execute(OptimizedSkeletonAspect skeleton, in SingleClip singleClip)
        {
            ref var clip     = ref singleClip.blob.Value.clips[0];
            var     clipTime = clip.LoopToClipTime(et);

            clip.SamplePose(ref skeleton, clipTime, 1f);
            skeleton.EndSamplingAndSync();
        }
    }
    
    
    // [UpdateBefore(typeof(TransformSuperSystem))]
    // public partial struct SingleClipPlayerSystem : ISystem
    // {
    //
    //     [BurstCompile]
    //     public void OnUpdate(ref SystemState state)
    //     {
    //         var t = (float)SystemAPI.Time.ElapsedTime;
    //
    //         foreach (var (bones, singleClip) in SystemAPI.Query<DynamicBuffer<BoneReference>, RefRO<SingleClip>>())
    //         {
    //             ref var clip = ref singleClip.ValueRO.blob.Value.clips[0];
    //             var clipTime = clip.LoopToClipTime(t);
    //             for (int i = 1; i < bones.Length; i++)
    //             {
    //                 var boneSampledLocalTransform          = clip.SampleBone(i, clipTime);
    //                 var boneTransformAspect                = SystemAPI.GetAspect<TransformAspect>(bones[i].bone);
    //                 boneTransformAspect.localTransformQvvs = boneSampledLocalTransform;
    //             }
    //         }
    //     }
    //     
    // }
}