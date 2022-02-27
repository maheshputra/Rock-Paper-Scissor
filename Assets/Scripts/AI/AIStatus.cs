using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockPapeScissor
{
    public class AIStatus : MonoBehaviour
    {
        public static AIStatus instance;

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

        public void GetRockPaperScissorMove()
        {
            int random = Random.Range(0, 3);
            if (random == 0)
            {
                rockPaperScissorMove = RockPaperScissorMove.Rock;
            }
            else if (random == 1)
            {
                rockPaperScissorMove = RockPaperScissorMove.Paper;
            }
            else if (random == 2)
            {
                rockPaperScissorMove = RockPaperScissorMove.Scissor;
            }
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
