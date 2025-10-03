using UnityEngine;
using UnityEngine.UI;

namespace JuiceBits
{
    public class GameManager2D : MonoBehaviour
    {
        public Text ScoreText;
        public Text StartText;
        private int _pointsPerSecond = 10;
        private int _currentScore = 0;
        private float _timer;

        private bool isStarting;

        private void Start()
        {
            Time.timeScale = 0f;
        }

        void Update()
        {
            IncreaseScoreOverTime();
            StartGame();
        }

        // Updates the score when collecting coins
        public void CollectCoins(int points)
        {
            _currentScore += points;
            UpdateScore();
        }

        // Score increase over time
        private void IncreaseScoreOverTime()
        {
            _timer += Time.deltaTime;

            if (_timer >= 1f)
            {
                _currentScore += _pointsPerSecond;
                ScoreText.text = "Score: " + _currentScore.ToString();
                _timer -= 1f;
            }
        }

        // Score update
        private void UpdateScore()
        {
            ScoreText.text = "Score: " + _currentScore.ToString();
        }

        // Starts the game when pressing space
        private void StartGame()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isStarting)
            {
                Time.timeScale = 1f;
                isStarting = true;
                StartText.enabled = false;
            }
        }
    }
}