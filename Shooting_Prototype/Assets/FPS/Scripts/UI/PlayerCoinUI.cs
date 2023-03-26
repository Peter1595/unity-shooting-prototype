using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;

namespace Unity.FPS.UI
{

    public class PlayerCoinUI : MonoBehaviour
    {
        public Text CoinText;

        private PlayerCharacterController playerCharacterController;

        private CoinsStat CoinStat;
        // Start is called before the first frame update
        void Start()
        {
            playerCharacterController = FindFirstObjectByType<PlayerCharacterController>();
            CoinStat = playerCharacterController.GetComponent<CoinsStat>();
        }


        // Update is called once per frame
        void Update()
        {
            if (!(CoinText && CoinStat))
                return;
            CoinText.text = NumberConverter.FixNumber(CoinStat.get());
        }
    }
}
