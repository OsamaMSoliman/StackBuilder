using System.Collections.Generic;
using UnityEngine;

namespace _Scripts {
    public class BuildingBlock : MonoBehaviour {
        public static BuildingBlock Current { get; private set; }

        public static BuildingBlock Previous { get; private set; }

        public bool IsStartingBlock { get; private set; }

        public MovingAxis MovingAxis { private get; set; }

        private float _movingSpeed = 1;

        private static readonly List<BuildingBlock> BuildingBlocks = new List<BuildingBlock>();

        private ParticleSystem _deathParticleSystem;

        private Color _blockColor;

        // Use this for initialization
        private void Start() {
            _deathParticleSystem = GetComponentInChildren<ParticleSystem>();
            // if there is no previous then i'm the startingBlock and i don't have to move
            if (Previous == null) {
                Previous        = this;
                _movingSpeed    = 0;
                IsStartingBlock = true;
                BuildingBlocks.Clear();
            }

            Current = this;
            BuildingBlocks.Add(Current);

            transform.localScale =
                new Vector3(Previous.transform.localScale.x, transform.localScale.y, Previous.transform.localScale.z);

            _blockColor                             = new Color(Random.value, Random.value, Random.value);
            GetComponent<Renderer>().material.color = _blockColor;
        }

        public void Stack() {
            //stop
            _movingSpeed = 0;

            //which axis r u moving on so i can stack right
            var extraPart = MovingAxis == MovingAxis.ZForward ? StackOnZ() : StackOnX();
            if (extraPart == null) return;

            // drop the excess part, and destroy it after 1 sec
            extraPart.gameObject.AddComponent<Rigidbody>();
            Destroy(extraPart.gameObject, 1);

            // now i'm the previous
            Previous = this;
        }


        private Transform StackOnZ() {
            var extraLength = Previous.transform.position.z - transform.position.z;
            if (Mathf.Abs(extraLength) >= Previous.transform.localScale.z) {
                GameController.IsGameOver = true;
                return null;
            }

            var wantedLength = transform.localScale.z - Mathf.Abs(extraLength);

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, wantedLength);
            transform.Translate(transform.forward * extraLength / 2);

            // my pos +- (0.5 wanted length + 0.5 Abs(extraLength)) .. +-: + if extraLength < 0 else -  
            var extraCubePos = transform.position.z + (wantedLength / 2 + Mathf.Abs(extraLength) / 2) * (extraLength > 0 ? -1 : 1);
//            var extraCubePos = transform.position.z + wantedLength * (extraLength > 0 ? -0.5f : 0.5f) - extraLength / 2;

            var extraPart = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            extraPart.localScale = new Vector3(transform.localScale.x, transform.localScale.y, extraLength);
            extraPart.position   = new Vector3(transform.position.x,   transform.position.y,   extraCubePos);
            return extraPart;
        }

        private Transform StackOnX() {
            var extraLength = Previous.transform.position.x - transform.position.x;
            if (Mathf.Abs(extraLength) >= Previous.transform.localScale.x) {
                GameController.IsGameOver = true;
                return null;
            }

            var wantedLength = transform.localScale.x - Mathf.Abs(extraLength);

            transform.localScale = new Vector3(wantedLength, transform.localScale.y, transform.localScale.z);
            transform.Translate(transform.right * extraLength / 2);

            // my pos +- (0.5 wanted length + 0.5 Abs(extraLength)) .. +-: + if extraLength < 0 else -  
//            var extraCubePos = transform.position.x + (wantedLength / 2 + Mathf.Abs(extraLength) / 2) * (extraLength > 0 ? -1 : 1);
            var extraCubePos = transform.position.x + wantedLength * (extraLength > 0 ? -0.5f : 0.5f) - extraLength / 2;

            var extraPart = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            extraPart.localScale = new Vector3(extraLength,  transform.localScale.y, transform.localScale.z);
            extraPart.position   = new Vector3(extraCubePos, transform.position.y,   transform.position.z);
            return extraPart;
        }


        private void Update() {
            // just translate on the right axis
            transform.Translate((MovingAxis == MovingAxis.ZForward ? transform.forward : transform.right)
                                * Time.deltaTime
                                * _movingSpeed);
        }

        public static void DestroyTheStack() {
            for (int i = BuildingBlocks.Count - 1; i >= 0; i--) {
                Debug.Log(i.ToString(), BuildingBlocks[i].gameObject);
                BuildingBlocks[i].PlayDeathEffect();
                Debug.Log(i.ToString(), BuildingBlocks[i].gameObject);
            }
        }

        private void PlayDeathEffect() {
//            _deathParticleSystem.startColor = _blockColor;
            _deathParticleSystem.Play();
        }
    }
}