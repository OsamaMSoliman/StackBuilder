using UnityEngine;

namespace _Scripts {
    public class BuildingBlock : MonoBehaviour {
        public static BuildingBlock Current { get; set; }

        public static BuildingBlock Previous { get; set; }

        private float _movingSpeed   = 1;
        private bool  _startingBlock = false;
        
        

        // Use this for initialization
        private void Start() {
            if (Previous == null) {
                Previous       = this;
                _movingSpeed   = 0;
                _startingBlock = true;
            }

            Current = this;

            transform.localScale =
                new Vector3(Previous.transform.localScale.x, transform.localScale.y, Previous.transform.localScale.z);

            GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
        }

        public void Stack() {
            _movingSpeed = 0;

            var extraLength  = Previous.transform.position.z - transform.position.z;
            var wantedLength = transform.localScale.z        - Mathf.Abs(extraLength);

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, wantedLength);
            transform.Translate(transform.forward * extraLength / 2);

            // my pos +- (0.5 wanted length + 0.5 Abs(extraLength)) .. +-: + if extraLength < 0 else -  
            var extraCubePos = transform.position.z + (wantedLength / 2 + Mathf.Abs(extraLength) / 2) * (extraLength > 0 ? -1 : 1);
//            var extraCubePos = transform.position.z + wantedLength * (extraLength > 0 ? -0.5f : 0.5f) - extraLength / 2;

            var extraPart = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            extraPart.localScale = new Vector3(transform.localScale.x, transform.localScale.y, extraLength);
            extraPart.position   = new Vector3(transform.position.x,   transform.position.y,   extraCubePos);

            extraPart.gameObject.AddComponent<Rigidbody>();
            Destroy(extraPart.gameObject, 1);

            Previous = this;
        }

        // Update is called once per frame
        private void Update() { transform.Translate(transform.forward * Time.deltaTime * _movingSpeed); }
    }
}