using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RockPapeScissor
{
    public class MainMenu : MonoBehaviour
    {
        public static MainMenu instance;

        [Header("UI")]
        [SerializeField] private Canvas canvasMainMenu;
        [SerializeField] private Button buttonStart;

        #region Monobehaviour
        private void Awake()
        {
            if (instance != null)
                Destroy(gameObject);
            else
                instance = this;
        }

        private void Start() {
            AddButtonCallback();
            ShowCanvas();
        }
        #endregion

        #region Start Game
        public void StartGame()
        {
            HideCanvas();
            GameplayHandler.instance.StartNewGameplay();
        }
        #endregion

        #region Canvas Control
        private void ShowCanvas(){
            canvasMainMenu.enabled = true;
        }

        private void HideCanvas(){
            canvasMainMenu.enabled = false;
        }
        #endregion

        #region Button Callback
        private void AddButtonCallback()
        {
            buttonStart.onClick.AddListener(() => StartGame());
        }
        #endregion
    }
}
