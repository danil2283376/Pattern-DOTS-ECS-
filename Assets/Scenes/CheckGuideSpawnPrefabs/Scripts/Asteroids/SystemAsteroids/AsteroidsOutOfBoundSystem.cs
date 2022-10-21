using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

// �� ��������� ��� ������� � ������ FixedStepSimulationGroup
//[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
//[UpdateBefore(typeof(EndFixedStepSimulationEntityCommandBufferSystem))]
public partial class AsteroidsOutOfBoundSystem : SystemBase
{
    //�� ���������� ������������ EndFixedStepSimECB
    // ��� ������, ��� ����� �� ���������� Unity Physics, ���� ������ ����� �������� � FixedStepSimulationSystem
    // �� �������� ��������� ���� ������� � ������������ ������ ������
    // � FixedStepSimGroup ���� ���� ����������� EntityCommandBufferSystem, ������� �� ����� ������������ ��� �������� ����������� ���������
    //���������� �����������

    private EndFixedStepSimulationEntityCommandBufferSystem m_EndFixedStepSimECB;

    protected override void OnCreate()
    {
        m_EndFixedStepSimECB = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
        //�� ����� ���� �������, ��� �� ����� �����������, ���� �� ������� ��� GameSettingsComponent
        // ������ ��� ��� ����� ������ �� ����� ����������, ����� �����, ��� ��������� �������� ������ ����
        RequireSingletonForUpdate<GameSettingsComponent>();
    }

    protected override void OnUpdate()
    {
        //LocalToWorld player = EntityManager.GetComponentData<LocalToWorld>();
        //float deltaTime = Time.DeltaTime;
        //Entities.WithAll<AsteroidTag>().ForEach((Entity entity, ref Translation position,
        //    ref LocalToWorld localToWorld, ref Rotation rotation) =>
        //{
        //    Vector3 target = new Vector3(0, 100, -2);
        //    Vector3 newDIr = Vector3.RotateTowards(localToWorld.Forward, target, 360, 0.0f);
        //    rotation.Value = math.mul(rotation.Value, quaternion.EulerXYZ(newDIr.x, newDIr.y, newDIr.z));
        //    position.Value += 1f * deltaTime;
        //}).ScheduleParallel();



        //// �� ����� ��������� ��� ��� ������������ �������, ������� ��� ����� �������� �AsParallelWriter� ��� ��������
        //// ��� ��������� �����
        //var commandBuffer = m_EndFixedStepSimECB.CreateCommandBuffer().AsParallelWriter();

        //var settings = GetSingleton<GameSettingsComponent>();

        ////�� ���� ��� �� ����������� �������� � ������������, ��������� ��� "WithAll"
        ////��� �����������, ��� �� ����������� ������� ������ � ����������� AsteroidTag, ������� �� �� ������ �� ������ �������
        ////�������, ��������, ������ ��������
        //Entities.WithAll<AsteroidTag>().ForEach((Entity entity, int entityInQueryIndex, in Translation position) =>
        //{
        //    //���������, �� ������� �� ������� �������� �������� �� ������� ����������� ���������
        //    if (Mathf.Abs(position.Value.x) > settings.levelWidth / 2 ||
        //       Mathf.Abs(position.Value.y) > settings.levelHeight / 2 ||
        //       Mathf.Abs(position.Value.z) > settings.levelDepth / 2)
        //    {
        //        //���� �� ������� �� �������, �� ��������� ��������� DestroyTag � ������� � ����������
        //        //commandBuffer.AddComponent(entityInQueryIndex, entity, new DestroyTag());
        //        commandBuffer.AddComponent(entityInQueryIndex, entity, new DestroyTag());
        //        //Debug.Log("���������� ������");
        //        return;
        //    }
        //}).ScheduleParallel();

        //m_EndFixedStepSimECB.AddJobHandleForProducer(Dependency);
    }
}
