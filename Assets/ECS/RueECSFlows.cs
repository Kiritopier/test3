using System;
using System.Collections.Generic;
using UnityEngine;
using RueECS;
namespace RueECS
{ 
    public partial class RueECSFlows: MonoBehaviour
    {
        public static bool WasInitialized {  get { return _WasInitialized; } }
        static bool _WasInitialized = false;
        public static RueECSFlows _Flows { get; } = Application.isPlaying ? new GameObject("RueECSFlowsGO").AddComponent<RueECSFlows>() : null;
        public static void _SetupAll() { if (!_WasInitialized) { SetupAll(); _WasInitialized = true; }  }
        static partial void SetupAll();
        public static void _Step() { Step(); }
        static partial void Step();
    }
}
