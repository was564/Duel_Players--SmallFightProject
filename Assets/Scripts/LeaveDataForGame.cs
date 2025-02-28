using System;
using GameRound;
using UnityEngine;

namespace DefaultNamespace
{
    public class LeaveDataForGame : MonoBehaviour
    {
        public GameRoundManager.GameState SelectedGameState { get; set; } = GameRoundManager.GameState.SingleNormalPlay;

        public static LeaveDataForGame Instance = null;

        public float Volume { get; set; } = 0.5f;

        public void Start()
        {
            if (Instance != null)
            {
                Destroy(gameObject, 0.1f);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}