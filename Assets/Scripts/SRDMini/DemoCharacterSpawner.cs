using UnityEngine;

namespace SrdMiniDemo
{
    [DisallowMultipleComponent]
    public class DemoCharacterSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject characterPrefab;
        [SerializeField] private Vector3 spawnPosition = new Vector3(0f, -0.1f, 0.6f);
        [SerializeField] private Vector3 spawnEuler = new Vector3(0f, 180f, 0f);
        [SerializeField] private bool addFallbackPrimitiveWhenNoPrefab = true;

        private void Start()
        {
            var root = characterPrefab != null
                ? Instantiate(characterPrefab, spawnPosition, Quaternion.Euler(spawnEuler), transform)
                : null;

            if (root == null && addFallbackPrimitiveWhenNoPrefab)
            {
                root = CreateFallbackCharacter();
            }

            if (root == null)
            {
                Debug.LogError("[SRD Mini Demo] No character prefab provided and fallback creation disabled.");
                return;
            }

            if (root.GetComponent<CharacterMotionController>() == null)
            {
                root.AddComponent<CharacterMotionController>();
            }

            if (root.GetComponent<MotionPresetCycler>() == null)
            {
                root.AddComponent<MotionPresetCycler>();
            }
        }

        private GameObject CreateFallbackCharacter()
        {
            var root = new GameObject("FallbackCharacter");
            root.transform.SetParent(transform);
            root.transform.localPosition = spawnPosition;
            root.transform.localRotation = Quaternion.Euler(spawnEuler);

            var body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            body.name = "Body";
            body.transform.SetParent(root.transform, false);
            body.transform.localScale = new Vector3(0.25f, 0.35f, 0.25f);

            var head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            head.name = "Head";
            head.transform.SetParent(root.transform, false);
            head.transform.localPosition = new Vector3(0f, 0.45f, 0f);
            head.transform.localScale = new Vector3(0.18f, 0.18f, 0.18f);

            var mat = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"))
            {
                color = new Color(0.2f, 0.8f, 1f)
            };
            body.GetComponent<MeshRenderer>().sharedMaterial = mat;
            head.GetComponent<MeshRenderer>().sharedMaterial = mat;
            return root;
        }
    }
}
