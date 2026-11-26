using UnityEngine;

namespace SrdMiniDemo
{
    [DisallowMultipleComponent]
    public class MotionPresetCycler : MonoBehaviour
    {
        [SerializeField] private CharacterMotionController controller;
        [SerializeField] private bool autoCycle = true;
        [SerializeField] private float cycleSeconds = 4f;

        private float _nextSwitchAt;

        private void Awake()
        {
            controller ??= GetComponent<CharacterMotionController>();
        }

        private void OnEnable()
        {
            _nextSwitchAt = Time.time + cycleSeconds;
        }

        private void Update()
        {
            if (controller == null)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)) controller.Preset = CharacterMotionController.MotionPreset.IdleBob;
            if (Input.GetKeyDown(KeyCode.Alpha2)) controller.Preset = CharacterMotionController.MotionPreset.Figure8;
            if (Input.GetKeyDown(KeyCode.Alpha3)) controller.Preset = CharacterMotionController.MotionPreset.Orbit;
            if (Input.GetKeyDown(KeyCode.Alpha4)) controller.Preset = CharacterMotionController.MotionPreset.Hop;

            if (!autoCycle || Time.time < _nextSwitchAt)
            {
                return;
            }

            var next = ((int)controller.Preset + 1) % 4;
            controller.Preset = (CharacterMotionController.MotionPreset)next;
            _nextSwitchAt = Time.time + cycleSeconds;
        }
    }
}
