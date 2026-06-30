using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScoringSettings _scoreSettings;

    [SerializeField, ReadOnly] private float _playerScore = 0;
    [SerializeField, ReadOnly] private int _wrongTrashSorted = 0;
    [SerializeField, ReadOnly] private int _trashSorted = 0;
    [SerializeField, ReadOnly] private int _vomitCleaned = 0;

    [SerializeField] private float _maxCleanliness = 100;
    [SerializeField] private float _cleanliness = 0;

    [SerializeField] private float _roundDuration = 300;

    [SerializeField] private Image _timerUI;
    [SerializeField] private Slider _rankingSlider;
    [SerializeField] private TextMeshProUGUI _scoreText;

    [SerializeField] private TextMeshProUGUI _finalScoreText;
    [SerializeField] private Image _rankUI;

    [SerializeField] private GameObject _scorePanel;
    [SerializeField] private GameObject _rankPanel;

    [SerializeField] private Transform _playerRig;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _finalPosition;

    [System.Serializable]
    public struct Ranking
    {
        public int ScoreMin;
        public Sprite Sprite;
    }
    [SerializeField] private List<Ranking> _rankings;

    private Timer _timer;

    public void StartGame()
    {
        _timer = new Timer(_roundDuration, Timer.TimerReset.Manual);

        _timer.OnTimerDone += StartFinal;

        _cleanliness = _maxCleanliness;
        _rankingSlider.maxValue = _maxCleanliness;
        _rankingSlider.minValue = 0;
        _rankingSlider.value = _cleanliness;

        SetTrashCans();

        FindAnyObjectByType<ClientSpawner>().StartSpawning();

        _playerRig.position = _startPosition.position;
    }

    public void SceneReload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void SetTrashCans()
    {
        Trashcan[] trashcans = FindObjectsByType<Trashcan>(0);

        foreach (Trashcan t in trashcans)
        {
            t.OnCorrectTrash.AddListener(() => _playerScore += _scoreSettings.CleanTrashPoints);
            t.OnWrongTrash.AddListener(() => _wrongTrashSorted++);
        }
    }

    private void Update()
    {
        _timer?.CountTimer();
        if (_timer != null) _timerUI.fillAmount = _timer.CurrentTime / _roundDuration;
        CheckCleanliness();
        _rankingSlider.value = _cleanliness;
        _scoreText.text = _playerScore.ToString();
    }

    private void CheckCleanliness()
    {
        _cleanliness = _maxCleanliness - (Trash.TotalTrash * _scoreSettings.TrashPenalization) - 
                                        (Vomit.TotalVomit * _scoreSettings.VomitPenalization);
    }

    private float CalculateMultiplier()
    {
        float result;

        int floored = Mathf.FloorToInt(_cleanliness / 10);

        if (floored <= 5)
        {
            result = floored * 0.1f;
        }
        else
        {
            result = floored - 5f;
        }

        Debug.Log($"MULT RESULT: {result}");

        return result;
    }

    private int CalculateFinalScore()
    {
        return (int)((_playerScore - (_wrongTrashSorted * _scoreSettings.WrongTrashPenalization)) * CalculateMultiplier());
    }

    private Sprite GetRankSprite(int score)
    {
        for (int i = _rankings.Count - 1; i >= 0; i--)
        {
            Ranking r = _rankings[i];
            
            if (score >= r.ScoreMin) return r.Sprite;
        }

        return null;
    }
    
    [Button]
    private void StartFinal()
    {
        _playerRig.position = _finalPosition.position;
        StartCoroutine(ShowFinalScoreCR());
    }

    private IEnumerator ShowFinalScoreCR()
    {
        _rankPanel.SetActive(false);
        _scorePanel.SetActive(true);

        _finalScoreText.text = "";

        yield return new WaitForSeconds(1f);

        _finalScoreText.text = $"Score {_playerScore}\n";

        yield return new WaitForSeconds(1f);

        _finalScoreText.text += $"<color=red>Penalizations -{_wrongTrashSorted * _scoreSettings.WrongTrashPenalization}</color>\n";

        yield return new WaitForSeconds(1f);

        if (CalculateMultiplier() >= 1) _finalScoreText.text += $"<color=green>Cleanliness x{CalculateMultiplier():f1}</color>\n";
        else _finalScoreText.text += $"<color=red>Cleanliness x{CalculateMultiplier():f1}</color>\n";

        yield return new WaitForSeconds(1f);

        _finalScoreText.text += $"\n";

        yield return new WaitForSeconds(1f);

        _finalScoreText.text += $"TOTAL {CalculateFinalScore()}";

        yield return new WaitForSeconds(1f);

        _rankUI.sprite = GetRankSprite(CalculateFinalScore());
        _rankUI.gameObject.SetActive(false);

        _rankPanel.SetActive(true);
        _scorePanel.SetActive(false);

        yield return new WaitForSeconds(2.5f);

        _rankUI.gameObject.SetActive(true);
    }
}
