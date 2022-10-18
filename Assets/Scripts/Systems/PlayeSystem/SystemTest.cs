using Unity.Entities;
using UnityEngine;
#pragma warning disable DC0058 // System Error
partial class SystemTest : SystemBase
{
    protected override void OnUpdate()
    {
        Debug.Log("Ты");
    }
}
#pragma warning restore DC0058 // System Error
