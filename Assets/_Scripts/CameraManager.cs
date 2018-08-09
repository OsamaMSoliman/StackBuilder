using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts {
    public class CameraManager : MonoBehaviour {
        private AudioSource[] _audioSources;
        private Text          _scoreText;
        private Button        _restartBtn;

        [SerializeField] private Text _highScoreTxt;

        private void Awake() {
            //1st one is the Background music the other is for the SFX
            _audioSources = GetComponents<AudioSource>();
            _scoreText    = GetComponentInChildren<Text>();
            _restartBtn   = GetComponentInChildren<Button>();
            _restartBtn.gameObject.SetActive(false);

            if (_highScoreTxt != null && PlayerPrefs.HasKey("HighScore")) {
//                PlayerPrefs.SetInt("HighScore",0);
                _highScoreTxt.text = PlayerPrefs.GetInt("HighScore").ToString();
            }
        }

        public void UpdateCameraManager() {
            _scoreText.text = (GetCurrentScore() + 1).ToString();
            _audioSources[1].Play();
            transform.Translate(Vector3.up * 0.1f); //TODO: use update and make it a smooth motion
            if (int.Parse(_highScoreTxt.text) < GetCurrentScore()) {
                _highScoreTxt.text = _scoreText.text;
                _highScoreTxt.color = Random.ColorHSV();
            }
        }

//        private void Update() { transform.position = Vector3.Lerp(to, from, ratio); }

        //#Inspector
        public void RestartTheLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
        public void ShowRestartBtn()  { _restartBtn.gameObject.SetActive(true); }

        public int GetCurrentScore() { return int.Parse(_scoreText.text); }
    }
}