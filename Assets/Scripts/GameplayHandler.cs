using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

namespace RockPapeScissor
{
    public class GameplayHandler : MonoBehaviour
    {
        #region Fields
        public static GameplayHandler instance;

        [Header("Settings")]
        [SerializeField] private float playerTurnTime;
        [SerializeField] private int winCount;

        [Header("Player UI")]
        [SerializeField] private Canvas canvasGameplay;
        [SerializeField] private Button buttonRock;
        [SerializeField] private Button buttonPaper;
        [SerializeField] private Button buttonScissor;
        [SerializeField] private Image imagePlayerTimeFill;

        [Header("AI UI")]
        [SerializeField] private Image imageAIRock;
        [SerializeField] private Image imageAIPaper;
        [SerializeField] private Image imageAIScissor;
        [SerializeField] private Color disabledColor;
        private Sequence seq0;

        [Header("Result UI")]
        [SerializeField] private RectTransform panelWinLose;
        [SerializeField] private TextMeshProUGUI textPlayerResult;
        [SerializeField] private TextMeshProUGUI textPlayerScore;
        [SerializeField] private TextMeshProUGUI textAIResult;
        [SerializeField] private TextMeshProUGUI textAIScore;
        [SerializeField] private Color winColor;
        [SerializeField] private Color loseColor;

        [Header("Final Result")]
        [SerializeField] private GameObject panelFinalResult;
        [SerializeField] private TextMeshProUGUI textFinalResult;
        [SerializeField] private Button buttonPlayAgain;

        private bool lastDraw;
        private bool lastPlayerWin;
        #endregion


        #region Monobehaviour
        private void Awake()
        {
            if (instance != null)
                Destroy(gameObject);
            else
                instance = this;
        }

        private void Start()
        {
            AddButtonCallback();
            HideGameplayCanvas();
            panelWinLose.anchoredPosition = new Vector2(0, Screen.height);
            panelFinalResult.SetActive(false);
        }
        #endregion

        #region Gameplay Control
        public void StartNewGameplay()
        {
            StartGameplay();
            ResetResult();
        }

        public void StartGameplay()
        {
            RefreshRound();
            WaitPlayerMove();
            ShowGameplayCanvas();
        }

        private void MoveSelected(RockPaperScissorMove move)
        {
            PlayerStatus.instance.SetRockPaperScissorMove(move);
        }

        private void FinalizeMovement()
        {
            bool playerLose = false;
            switch (AIStatus.instance.RockPaperScissorMove)
            {
                case RockPaperScissorMove.Rock:
                    imageAIPaper.color = disabledColor;
                    imageAIScissor.color = disabledColor;

                    imageAIRock.transform.DOScale(1.25f, .25f);
                    break;

                case RockPaperScissorMove.Paper:
                    imageAIRock.color = disabledColor;
                    imageAIScissor.color = disabledColor;

                    imageAIPaper.transform.DOScale(1.25f, .25f);
                    break;

                case RockPaperScissorMove.Scissor:
                    imageAIRock.color = disabledColor;
                    imageAIPaper.color = disabledColor;

                    imageAIScissor.transform.DOScale(1.25f, .25f);
                    break;
            }

            switch (PlayerStatus.instance.RockPaperScissorMove)
            {
                case RockPaperScissorMove.None:
                    buttonRock.interactable = false;
                    buttonPaper.interactable = false;
                    buttonScissor.interactable = false;
                    playerLose = true;
                    break;

                case RockPaperScissorMove.Rock:
                    buttonPaper.interactable = false;
                    buttonScissor.interactable = false;
                    break;

                case RockPaperScissorMove.Paper:
                    buttonRock.interactable = false;
                    buttonScissor.interactable = false;
                    break;

                case RockPaperScissorMove.Scissor:
                    buttonRock.interactable = false;
                    buttonPaper.interactable = false;
                    break;
            }

            CheckRound(playerLose);
        }

        private void CheckRound(bool playerLose)
        {
            if (playerLose)
            {
                AIStatus.instance.AddWinCount();
                lastPlayerWin = false;
                CalculateRound();
                return;
            }

            var playerMove = PlayerStatus.instance.RockPaperScissorMove;
            var aiMove = AIStatus.instance.RockPaperScissorMove;

            if (playerMove == aiMove)
            {
                lastDraw = true;
                CalculateRound();
                return;
            }

            lastDraw = false;

            if (playerMove == RockPaperScissorMove.Rock && aiMove == RockPaperScissorMove.Scissor)
            {
                PlayerStatus.instance.AddWinCount();
                lastPlayerWin = true;
            }
            else if (playerMove == RockPaperScissorMove.Paper && aiMove == RockPaperScissorMove.Rock)
            {
                PlayerStatus.instance.AddWinCount();
                lastPlayerWin = true;
            }
            else if (playerMove == RockPaperScissorMove.Scissor && aiMove == RockPaperScissorMove.Paper)
            {
                PlayerStatus.instance.AddWinCount();
                lastPlayerWin = true;
            }
            else
            {
                AIStatus.instance.AddWinCount();
            }

            CalculateRound();
        }

        // Must be yield
        private void CalculateRound()
        {
            RefreshScore();
            if (PlayerStatus.instance.WinCount < winCount && AIStatus.instance.WinCount < winCount)
            {
                ContinueRound();
            }
            else
            {
                EndGame();
            }
        }

        private void ContinueRound()
        {
            StartCoroutine(ContinueRoundCoroutine());
        }

        private IEnumerator ContinueRoundCoroutine()
        {
            ShowResult();
            imagePlayerTimeFill.DOFillAmount(1, 1f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(3f);
            StartGameplay();
        }

        private void RefreshRound()
        {
            lastPlayerWin = false;
            imagePlayerTimeFill.fillAmount = 1;

            PlayerStatus.instance.SetRockPaperScissorMove(RockPaperScissorMove.None);

            buttonRock.interactable = true;
            buttonPaper.interactable = true;
            buttonScissor.interactable = true;

            buttonRock.transform.DOScale(1, .25f);
            buttonPaper.transform.DOScale(1, .25f);
            buttonScissor.transform.DOScale(1, .25f);

            imageAIScissor.transform.localScale = Vector3.one * .75f;
            imageAIRock.transform.localScale = Vector3.one * .75f;
            imageAIPaper.transform.localScale = Vector3.one * .75f;
            imageAIRock.color = Color.white;
            imageAIPaper.color = Color.white;
            imageAIScissor.color = Color.white;
        }

        private void EndGame()
        {
            if (PlayerStatus.instance.WinCount >= winCount)
            {
                textFinalResult.color = winColor;
                textFinalResult.text = "YOU WIN";
            }
            else if (AIStatus.instance.WinCount >= winCount)
            {
                textFinalResult.color = loseColor;
                textFinalResult.text = "YOU LOSE";
            }
            panelFinalResult.SetActive(true);
        }

        private void ResetResult()
        {
            PlayerStatus.instance.ResetScore();
            AIStatus.instance.ResetScore();
        }
        #endregion

        #region Score UI
        private void RefreshScore()
        {
            textPlayerScore.text = PlayerStatus.instance.WinCount.ToString();
            textAIScore.text = AIStatus.instance.WinCount.ToString();
        }

        private void ShowResult()
        {
            if (lastDraw)
            {
                textPlayerResult.color = Color.white;
                textAIResult.color = Color.white;

                textPlayerResult.text = "DRAW";
                textAIResult.text = "DRAW";
            }
            else if (lastPlayerWin)
            {
                textPlayerResult.color = winColor;
                textAIResult.color = loseColor;

                textPlayerResult.text = "WIN";
                textAIResult.text = "LOSE";
            }
            else if (!lastPlayerWin)
            {
                textPlayerResult.color = loseColor;
                textAIResult.color = winColor;

                textPlayerResult.text = "LOSE";
                textAIResult.text = "WIN";
            }

            Sequence seq = DOTween.Sequence();
            seq.Append(panelWinLose.DOAnchorPosY(0, .5f));
            seq.Append(panelWinLose.DOAnchorPosY(0, 1f));
            seq.Append(panelWinLose.DOAnchorPosY(Screen.height, .5f));

        }

        private void ShowFinalResult()
        {

        }
        #endregion

        #region Player Controller
        private void WaitPlayerMove()
        {
            StartCoroutine(WaitPlayerMoveCoroutine());
        }

        private IEnumerator WaitPlayerMoveCoroutine()
        {
            imagePlayerTimeFill.DOFillAmount(0, playerTurnTime).SetEase(Ease.Linear);
            AIStatus.instance.GetRockPaperScissorMove();
            AnimateAIMove();

            while (imagePlayerTimeFill.fillAmount != 0)
            {
                yield return null;
            }

            StopAnimateAI();
            FinalizeMovement();
        }
        #endregion

        #region AI
        private void AnimateAIMove()
        {
            seq0 = DOTween.Sequence();

            seq0.Append(imageAIScissor.transform.DOScale(1.25f, .1f));
            seq0.Append(imageAIScissor.transform.DOScale(.75f, .1f));
            seq0.Append(imageAIRock.transform.DOScale(1.25f, .1f));
            seq0.Append(imageAIRock.transform.DOScale(.75f, .1f));
            seq0.Append(imageAIPaper.transform.DOScale(1.25f, .1f));
            seq0.Append(imageAIPaper.transform.DOScale(.75f, .1f));
            seq0.SetLoops(-1).SetEase(Ease.Linear);
        }

        private void StopAnimateAI()
        {
            seq0.Kill();
        }
        #endregion

        #region Canvas Control
        private void ShowGameplayCanvas()
        {
            canvasGameplay.enabled = true;
        }

        private void HideGameplayCanvas()
        {
            canvasGameplay.enabled = false;
        }
        #endregion

        #region Button Callback
        private void SelectRock()
        {
            MoveSelected(RockPaperScissorMove.Rock);
            buttonRock.transform.DOScale(1.25f, .25f);
            buttonPaper.transform.DOScale(.75f, .25f);
            buttonScissor.transform.DOScale(.75f, .25f);
        }

        private void SelectPaper()
        {
            MoveSelected(RockPaperScissorMove.Paper);
            buttonPaper.transform.DOScale(1.25f, .25f);
            buttonScissor.transform.DOScale(.75f, .25f);
            buttonRock.transform.DOScale(.75f, .25f);
        }

        private void SelectScissor()
        {
            MoveSelected(RockPaperScissorMove.Scissor);
            buttonScissor.transform.DOScale(1.25f, .25f);
            buttonRock.transform.DOScale(.75f, .25f);
            buttonPaper.transform.DOScale(.75f, .25f);
        }

        private void RestartGame()
        {
            SceneManager.LoadScene($"{SceneManager.GetActiveScene().path}");
        }

        private void AddButtonCallback()
        {
            buttonRock.onClick.AddListener(() => SelectRock());
            buttonPaper.onClick.AddListener(() => SelectPaper());
            buttonScissor.onClick.AddListener(() => SelectScissor());
            buttonPlayAgain.onClick.AddListener(() => RestartGame());
        }
        #endregion
    }
}