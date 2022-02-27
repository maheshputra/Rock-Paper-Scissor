using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockPapeScissor
{
    public class PlayerStatus : MonoBehaviour
    {
        public static PlayerStatus instance;

        [Header("Current Status")]
        [SerializeField, ReadOnly] private RockPaperScissorMove rockPaperScissorMove;
        public RockPaperScissorMove RockPaperScissorMove { get => rockPaperScissorMove; }
        [SerializeField, ReadOnly] private int winCount;
        public int WinCount { get => winCount; }

        private void Awake()
        {
            if (instance != null)
                Destroy(gameObject);
            else
                instance = this;
        }

        private void Start()
        {
            rockPaperScissorMove = RockPaperScissorMove.None;
        }

        public void SetRockPaperScissorMove(RockPaperScissorMove move)
        {
            rockPaperScissorMove = move;
        }

        public void AddWinCount()
        {
            winCount++;
        }

        public void ResetScore(){
            winCount=0;
        }
    }
}
