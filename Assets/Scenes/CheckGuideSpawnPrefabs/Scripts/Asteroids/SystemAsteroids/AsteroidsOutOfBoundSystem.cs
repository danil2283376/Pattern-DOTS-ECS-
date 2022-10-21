using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

// Мы добавляем эту систему в группу FixedStepSimulationGroup
//[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
//[UpdateBefore(typeof(EndFixedStepSimulationEntityCommandBufferSystem))]
public partial class AsteroidsOutOfBoundSystem : SystemBase
{
    //Мы собираемся использовать EndFixedStepSimECB
    // Это потому, что когда мы используем Unity Physics, наша физика будет работать в FixedStepSimulationSystem
    // Мы пытаемся поместить наши системы в определенные группы систем
    // У FixedStepSimGroup есть своя собственная EntityCommandBufferSystem, которую мы будем использовать для внесения структурных изменений
    //добавление уничтожения

    private EndFixedStepSimulationEntityCommandBufferSystem m_EndFixedStepSimECB;

    protected override void OnCreate()
    {
        m_EndFixedStepSimECB = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
        //Мы хотим быть уверены, что не будем обновляться, пока не получим наш GameSettingsComponent
        // потому что нам нужны данные из этого компонента, чтобы знать, где находится периметр нашего куба
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



        //// Мы хотим запускать это как параллельные задания, поэтому нам нужно добавить «AsParallelWriter» при создании
        //// наш командный буфер
        //var commandBuffer = m_EndFixedStepSimECB.CreateCommandBuffer().AsParallelWriter();

        //var settings = GetSingleton<GameSettingsComponent>();

        ////На этот раз мы запрашиваем сущности с компонентами, используя тег "WithAll"
        ////Это гарантирует, что мы захватываем объекты только с компонентом AsteroidTag, поэтому мы не влияем на другие объекты
        ////который, возможно, прошел периметр
        //Entities.WithAll<AsteroidTag>().ForEach((Entity entity, int entityInQueryIndex, in Translation position) =>
        //{
        //    //Проверяем, не выходит ли текущее значение перевода за пределы допустимого диапазона
        //    if (Mathf.Abs(position.Value.x) > settings.levelWidth / 2 ||
        //       Mathf.Abs(position.Value.y) > settings.levelHeight / 2 ||
        //       Mathf.Abs(position.Value.z) > settings.levelDepth / 2)
        //    {
        //        //Если он выходит за пределы, мы добавляем компонент DestroyTag к объекту и возвращаем
        //        //commandBuffer.AddComponent(entityInQueryIndex, entity, new DestroyTag());
        //        commandBuffer.AddComponent(entityInQueryIndex, entity, new DestroyTag());
        //        //Debug.Log("Уничтожить объект");
        //        return;
        //    }
        //}).ScheduleParallel();

        //m_EndFixedStepSimECB.AddJobHandleForProducer(Dependency);
    }
}
