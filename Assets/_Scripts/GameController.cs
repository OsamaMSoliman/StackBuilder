using UnityEngine;

namespace _Scripts {
    public class GameController : MonoBehaviour {
        [SerializeField] private CameraManager _cameraManager;

        public  BuildingBlock  BuildingBlockPrefab;
        private BlockSpawner[] _spawners;
        private int            _spawnerTurn;

        public static bool IsGameOver { private get; set; }

        private void Awake() {
            if (BuildingBlockPrefab == null) Debug.LogError("BuildingBlockPrefab is not set", gameObject);
            _spawners = GetComponentsInChildren<BlockSpawner>();
        }

        // Update is called once per frame
        public void Update() {
#if UNITY_EDITOR
            bool clicked = Input.GetKeyDown(KeyCode.Mouse0);
#elif UNITY_ANDROID
            bool clicked = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
#endif
            if (!clicked) return;

            if (BuildingBlock.Previous != BuildingBlock.Current)
                BuildingBlock.Current.Stack();

            if (IsGameOver) {
                BuildingBlock.DestroyTheStack();
                enabled = false;
                return;
            }
            
            _cameraManager.UpdateCameraManager();

            _spawners[_spawnerTurn++].SpawnBuildingBlock(_spawnerTurn % 2 == 0 ? MovingAxis.XForward : MovingAxis.ZForward);
            _spawnerTurn %= _spawners.Length;
        }
    }
}