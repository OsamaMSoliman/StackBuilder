using UnityEngine;

namespace _Scripts {
    public class BlockSpawner : MonoBehaviour {
        private GameController _gameController;

        private void Awake() { _gameController = GetComponentInParent<GameController>(); }

        public void SpawnBuildingBlock() {
            var bb = Instantiate(_gameController.BuildingBlockPrefab.gameObject,transform).transform;
            bb.rotation = transform.rotation;
            if (BuildingBlock.Current != BuildingBlock.Previous)
                bb.position = new Vector3(transform.position.x,
                                          BuildingBlock.Previous.transform.position.y + bb.position.y,
                                          transform.position.z);
            
        }


        private void OnDrawGizmosSelected() {
            if (BuildingBlock.Current == null) return;
            Gizmos.DrawCube(transform.position, _gameController.BuildingBlockPrefab.transform.localScale);
        }
    }
}