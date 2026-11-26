using System;
using System.Linq;
using UnityEngine;

namespace SrdMiniDemo
{
    /// <summary>
    /// Attempts to attach the SRD SDK camera component to Main Camera.
    /// Works even when SDK assemblies are not present by using reflection.
    /// </summary>
    [DisallowMultipleComponent]
    public class SrdMiniDisplayBootstrap : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;
        [SerializeField] private bool force60Fps = true;
        [SerializeField] private bool logDiagnostics = true;

        private static readonly string[] CandidateTypeNames =
        {
            "SRD.Core.SRDCamera, SRD.Core.Runtime",
            "SRD.Core.SRDCamera, SRD.Core",
            "Sony.SRD.SRDCamera, Sony.SRD"
        };

        private void Awake()
        {
            if (force60Fps)
            {
                Application.targetFrameRate = 60;
                QualitySettings.vSyncCount = 0;
            }

            targetCamera ??= Camera.main;
            if (targetCamera == null)
            {
                Debug.LogError("[SRD Mini Demo] Main Camera not found. Create a camera first.");
                return;
            }

            var srdCameraType = ResolveSrdCameraType();
            if (srdCameraType == null)
            {
                Debug.LogWarning(
                    "[SRD Mini Demo] SRD SDK camera type not found. Import the official Spatial Reality Display Mini SDK package and reopen the scene.");
                return;
            }

            if (targetCamera.GetComponent(srdCameraType) == null)
            {
                targetCamera.gameObject.AddComponent(srdCameraType);
                if (logDiagnostics)
                {
                    Debug.Log($"[SRD Mini Demo] Added {srdCameraType.FullName} to {targetCamera.name}.");
                }
            }

            if (logDiagnostics)
            {
                var loaded = AppDomain.CurrentDomain.GetAssemblies()
                    .Select(a => a.GetName().Name)
                    .Where(n => n != null && (n.Contains("SRD", StringComparison.OrdinalIgnoreCase) ||
                                               n.Contains("Sony", StringComparison.OrdinalIgnoreCase)));
                Debug.Log($"[SRD Mini Demo] Relevant loaded assemblies: {string.Join(", ", loaded)}");
            }
        }

        private static Type ResolveSrdCameraType()
        {
            foreach (var typeName in CandidateTypeNames)
            {
                var type = Type.GetType(typeName, throwOnError: false);
                if (type != null)
                {
                    return type;
                }
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    types = e.Types.Where(t => t != null).Cast<Type>().ToArray();
                }

                foreach (var type in types)
                {
                    if (typeof(MonoBehaviour).IsAssignableFrom(type) &&
                        type.Name.Equals("SRDCamera", StringComparison.OrdinalIgnoreCase))
                    {
                        return type;
                    }
                }
            }

            return null;
        }
    }
}
