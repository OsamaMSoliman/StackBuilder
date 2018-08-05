using UnityEngine;

namespace _Scripts {
    public class BlockSpawner : MonoBehaviour {
        private GameController _gameController;

        private void Awake() { _gameController = GetComponentInParent<GameController>(); }

        public void SpawnBuildingBlock(MovingAxis movingAxis) {
            var bb = Instantiate(_gameController.BuildingBlockPrefab.gameObject, transform).transform;
            bb.GetComponent<BuildingBlock>().MovingAxis = movingAxis;

            if (BuildingBlock.Previous.IsStartingBlock) return;

            var prevPos = BuildingBlock.Previous.transform.position;
            bb.position = new Vector3(movingAxis== MovingAxis.ZForward? prevPos.x:transform.position.x,
                                      BuildingBlock.Previous.transform.position.y + bb.localScale.y,
                                      movingAxis == MovingAxis.XForward? prevPos.z:transform.position.z);
        }


        private void OnDrawGizmosSelected() {
            if (BuildingBlock.Current == null) return;
            Gizmos.DrawCube(transform.position, _gameController.BuildingBlockPrefab.transform.localScale);
            
        }
    }
}