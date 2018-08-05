using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace _Scripts {
    public class CameraManager : MonoBehaviour {
        private AudioSource[] _audioSources;
        private Text          _scoreText;

        private void Awake() {
            //1st one is the Background music the other is for the SFX
            _audioSources = GetComponents<AudioSource>();
            _scoreText    = GetComponentInChildren<Text>();
        }

        public void UpdateCameraManager() {
            _scoreText.text = (int.Parse(_scoreText.text) + 1).ToString();
            _audioSources[1].Play();
            transform.Translate(Vector3.up * 0.1f);//TODO: use update and make it a smooth motion
        }

//        private void Update() { transform.position = Vector3.Lerp(to, from, ratio); }
    }
}