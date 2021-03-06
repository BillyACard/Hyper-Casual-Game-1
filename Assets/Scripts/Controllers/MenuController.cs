using DG.Tweening;
using TMPro;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject inputPreventer;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject inGameMenu;
    [SerializeField] private GameObject skinListMenu;
    [SerializeField] private TextMeshProUGUI endGameScore;
    public static MenuController Instance;
    private CanvasGroup _mainMenuGroup;
    private CanvasGroup _gameOverMenuGroup;
    private CanvasGroup _inGameMenuGroup;
    private CanvasGroup _skinListMenuGroup;

    private float _fadeDuration = 1f;

    private void Awake()
    {
        _mainMenuGroup = mainMenu.GetComponent<CanvasGroup>();
        _gameOverMenuGroup = gameOverMenu.GetComponent<CanvasGroup>();
        _inGameMenuGroup = inGameMenu.GetComponent<CanvasGroup>();
        _skinListMenuGroup = skinListMenu.GetComponent<CanvasGroup>();
        if (Instance == null)
        {
            Instance = this;
            return;
        }

        Destroy(this);
    }

    public void PlayPressed()
    {
        inputPreventer.SetActive(true);
        inGameMenu.SetActive(true);
        ScoreManager.Instance.ResetScore();
        _inGameMenuGroup.DOFade(1f, _fadeDuration).SetEase(Ease.Linear);
        _mainMenuGroup.DOFade(0f, _fadeDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            mainMenu.SetActive(false);
            inputPreventer.SetActive(false);
            GameManager.Instance.StartGame();
        });
    }

    public void MainMenuPressed()
    {
        inputPreventer.SetActive(true);
        mainMenu.SetActive(true);
        _mainMenuGroup.DOFade(1f, _fadeDuration).SetEase(Ease.Linear);
        _gameOverMenuGroup.DOFade(0f, _fadeDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            gameOverMenu.SetActive(false);
            inputPreventer.SetActive(false);
        });
    }

    public void PlayAgainPressed()
    {
        inputPreventer.SetActive(true);
        inGameMenu.SetActive(true);
        ScoreManager.Instance.ResetScore();
        _inGameMenuGroup.DOFade(1f, _fadeDuration).SetEase(Ease.Linear);
        _gameOverMenuGroup.DOFade(0f, _fadeDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            gameOverMenu.SetActive(false);
            inputPreventer.SetActive(false);
            GameManager.Instance.StartGame();
        });
    }

    public void OnGameOver()
    {
        inputPreventer.SetActive(true);
        gameOverMenu.SetActive(true);
        endGameScore.text = ScoreManager.Instance.GetScore().ToString();
        _gameOverMenuGroup.DOFade(1f, _fadeDuration).SetEase(Ease.Linear);
        _inGameMenuGroup.DOFade(0f, _fadeDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            inGameMenu.SetActive(false);
            inputPreventer.SetActive(false);
        });
    }

    public void SkinButtonPressed()
    {
        inputPreventer.SetActive(true);
        skinListMenu.SetActive(true);
        _skinListMenuGroup.DOFade(1f, _fadeDuration).SetEase(Ease.Linear);
        _mainMenuGroup.DOFade(0f, _fadeDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            mainMenu.SetActive(false);
            inputPreventer.SetActive(false);
        });
    }
    
    public void BackButtonPressed()
    {
        inputPreventer.SetActive(true);
        mainMenu.SetActive(true);
        _mainMenuGroup.DOFade(1f, _fadeDuration).SetEase(Ease.Linear);
        _skinListMenuGroup.DOFade(0f, _fadeDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            skinListMenu.SetActive(false);
            inputPreventer.SetActive(false);
        });
    }
}