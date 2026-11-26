using UnityEngine;

namespace SrdMiniDemo
{
    [DisallowMultipleComponent]
    public class CharacterMotionController : MonoBehaviour
    {
        public enum MotionPreset
        {
            IdleBob = 0,
            Figure8 = 1,
            Orbit = 2,
            Hop = 3
        }

        [Header("Motion")]
        [SerializeField] private MotionPreset preset = MotionPreset.IdleBob;
        [SerializeField] private float speed = 1.2f;
        [SerializeField] private float xAmplitude = 0.08f;
        [SerializeField] private float yAmplitude = 0.08f;
        [SerializeField] private float zAmplitude = 0.08f;
        [SerializeField] private float yawAmplitude = 14f;

        private Vector3 _basePosition;
        private Quaternion _baseRotation;

        public MotionPreset Preset
        {
            get => preset;
            set => preset = value;
        }

        private void Start()
        {
            _basePosition = transform.localPosition;
            _baseRotation = transform.localRotation;
        }

        private void Update()
        {
            var t = Time.time * speed;
            var pos = _basePosition;
            var rot = _baseRotation;

            switch (preset)
            {
                case MotionPreset.IdleBob:
                    pos.y += Mathf.Sin(t) * yAmplitude;
                    rot *= Quaternion.Euler(0f, Mathf.Sin(t * 0.5f) * yawAmplitude, 0f);
                    break;
                case MotionPreset.Figure8:
                    pos.x += Mathf.Sin(t) * xAmplitude;
                    pos.z += Mathf.Sin(t * 2f) * zAmplitude;
                    rot *= Quaternion.Euler(0f, Mathf.Sin(t) * yawAmplitude, 0f);
                    break;
                case MotionPreset.Orbit:
                    pos.x += Mathf.Cos(t) * xAmplitude;
                    pos.z += Mathf.Sin(t) * zAmplitude;
                    rot *= Quaternion.Euler(0f, t * 40f, 0f);
                    break;
                case MotionPreset.Hop:
                    pos.y += Mathf.Abs(Mathf.Sin(t * 1.6f)) * yAmplitude;
                    rot *= Quaternion.Euler(Mathf.Sin(t * 0.8f) * 4f, Mathf.Sin(t * 0.5f) * yawAmplitude, 0f);
                    break;
            }

            transform.localPosition = pos;
            transform.localRotation = rot;
        }
    }
}
