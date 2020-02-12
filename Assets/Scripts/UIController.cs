using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UIElements.Button;

public class UIController : MonoBehaviour {
    private Text money;
    private Image coin;
    
    private Text error;
    private float errorCountdown = .1f;
    
    private Text score;
    private float points = 0;
    
    private RectTransform gameOver;
    private Text gameOverText;
    private GameObject tryAgain;

    public static UIController instance { get; private set; }

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }
    void Start() {
        foreach (Transform childTransform in transform) {
            switch (childTransform.name) {
                case "Money":
                    money = childTransform.GetComponent<Text>();
                    break;
                case "Error":
                    error = childTransform.GetComponent<Text>();
                    break;
                case "Score":
                    score = childTransform.GetComponent<Text>();
                    break;
                case "GameOver":
                    gameOver = childTransform.GetComponent<RectTransform>();
                    gameOver.anchoredPosition = Vector2.up * GetComponent<RectTransform>().rect.height;
                    gameOverText = gameOver.Find("GameOverText").GetComponent<Text>();
                    tryAgain = gameOver.Find("Try Again").gameObject;
                    break;
                case "Coin":
                    coin = childTransform.GetComponent<Image>();
                    break;
            }
        }
    }
    void Update() {
        if (money != null) money.text = BuildController.instance.money.ToString();
        if (score != null) score.text = points.ToString();
        if (errorCountdown > 0f && error != null) {
            errorCountdown = Mathf.Max(errorCountdown - Time.deltaTime, 0f);
            Color color = error.color;
            color.a = Mathf.Min(errorCountdown, 1f);
            error.color = color;
        }
    }

    public void Error(string text) {
        error.text = text;
        errorCountdown = 2f;
    }

    public void AddPoints(int points) {
        this.points += points;
    }
    
    public void GameOver() {
        money.gameObject.SetActive(false);
        coin.gameObject.SetActive(false);
        error.gameObject.SetActive(false);
        score.gameObject.SetActive(false);
        gameOverText.text = "You died \n Score:" + points;
        gameOver.DOAnchorPos(Vector2.zero, .3f).SetEase(Ease.OutCubic);
        EventSystem.current.SetSelectedGameObject(tryAgain);
    }

    public void MainMenu() {
        SceneManager.LoadScene("Main Menu");
    }
    
    public void Game() {
        SceneManager.LoadScene("Game");
    }

    public void Exit() {
        Application.Quit();
    }

    public void Help() {
        
    }
}
