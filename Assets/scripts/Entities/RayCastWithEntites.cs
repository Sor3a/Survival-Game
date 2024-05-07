using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class RayCastWithEntites : MonoBehaviour
{
    NativeList<Unity.Physics.RaycastHit> raycastHits;
    NativeList<Unity.Physics.RaycastHit> raycastCollisionHits;
    BuildPhysicsWorld physicsWorld;
    [SerializeField] float Distance,collisionDistance;
   // DataManager DataManager;
    EntityManager manager;
    StepPhysicsWorld stepPhysicsWorld;
    //playerMVT playerMvt;

    struct raycastHitJob : IJob
    {
        public PhysicsWorld physicsWorld;
        public NativeList<Unity.Physics.RaycastHit> raycastHits;
        public RaycastInput raycastInput;
        public void Execute()
        {
            physicsWorld.CastRay(raycastInput,ref raycastHits);
            
        }
    }

    private void Awake()
    {
        //playerMvt = FindObjectOfType<playerMVT>();
       // DataManager = FindObjectOfType<DataManager>();
    }

    private void Start()
    {
        physicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<StepPhysicsWorld>();
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        raycastHits = new NativeList<Unity.Physics.RaycastHit>(Allocator.Persistent);
        raycastCollisionHits = new NativeList<Unity.Physics.RaycastHit>(Allocator.Persistent);
    }

    private void LateUpdate()
    {
        stepPhysicsWorld.FinalSimulationJobHandle.Complete();

        raycastHits.Clear();
        raycastCollisionHits.Clear();


        DetectAttack();
        //DetectCollision();
        //if (raycastHits.IsCreated)
        //{ 
        //    if (!raycastHits.IsEmpty)
        //    {
        //        foreach (var hit in raycastHits.ToArray())
        //        {

        //            DataManager.DropItem(hit.Entity);
        //            manager.DestroyEntity(hit.Entity);
        //        }
        //    }

        //}
    }
    public bool DetectCollision(Vector3 CollisionDirection)
    {
        Vector3 direction = CollisionDirection * collisionDistance + transform.position;
        var raycastInputCollision = new RaycastInput
        {
            Start = transform.position,
            End = direction,
            Filter = CollisionFilter.Default
        };
        var raycastJob = new raycastHitJob
        {
            physicsWorld = physicsWorld.PhysicsWorld,
            raycastHits = raycastCollisionHits,
            raycastInput = raycastInputCollision,
        }.Schedule();
        raycastJob.Complete();
        bool isClear = true;
        if (raycastCollisionHits.IsCreated)
        {
            if (raycastCollisionHits.Length > 0)
            {
                foreach (var hit in raycastCollisionHits.ToArray())
                {
                    if (manager.HasComponent<TreeData>(hit.Entity))
                    {
                        //playerMvt.CanMove = false;
                        isClear = false;
                        Debug.Log("hitting");
                        break;
                    }
                }
            }  
        }
        return isClear;
    }
    void DetectAttack()
    {
        var raycastInput = new RaycastInput
        {
            Start = transform.position,
            End = transform.forward * Distance + transform.position,
            Filter = CollisionFilter.Default
        };
        var raycastJob = new raycastHitJob
        {
            physicsWorld = physicsWorld.PhysicsWorld,
            raycastHits = raycastHits,
            raycastInput = raycastInput,
        }.Schedule();
        raycastJob.Complete();
    }
    public Unity.Physics.RaycastHit[] getRayCastHits()
    {
        if (raycastHits.IsCreated)
            if (!raycastHits.IsEmpty) return raycastHits.ToArray();
        
        return null;
    }

    private void OnDestroy()
    {
        if (raycastHits.IsCreated)
            raycastHits.Dispose();

        if (raycastCollisionHits.IsCreated)
            raycastCollisionHits.Dispose();
    }


}
