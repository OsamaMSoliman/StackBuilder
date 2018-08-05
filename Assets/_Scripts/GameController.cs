using UnityEngine;

namespace _Scripts {
    public class GameController : MonoBehaviour {
        public  BuildingBlock  BuildingBlockPrefab;
        private BlockSpawner[] _spawners;
        private int            _spawnerTurn;

        private void Awake() {
            if (BuildingBlockPrefab == null) Debug.LogError("BuildingBlockPrefab is not set", gameObject);
            _spawners = GetComponentsInChildren<BlockSpawner>();
        }

        // Update is called once per frame
        void Update() {
#if UNITY_EDITOR
            bool clicked = Input.GetKeyDown(KeyCode.Mouse0);
#elif UNITY_ANDROID
            bool clicked = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
#endif
            if (!clicked) return;

            _spawners[_spawnerTurn++].SpawnBuildingBlock();
            _spawnerTurn %= _spawners.Length;
            if (BuildingBlock.Previous != BuildingBlock.Current)
                BuildingBlock.Current.Stack();
        }
    }
}