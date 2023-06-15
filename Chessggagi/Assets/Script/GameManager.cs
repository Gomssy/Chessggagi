using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace Chessggagi
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField]
        public GameObject board;

        [SerializeField]
        public GameObject[] wall;

        public ClickUI[] Buttons;

        [SerializeField]
        private Sprite[] Winner;

        [SerializeField]
        private Image Result;

        [SerializeField]
        private Canvas Canvas;

        public enum Player { White, Black }

        public Player currentPlayer;

        public bool IsTurnChanging { get; private set; }

        public Piece CurrentPiece { get; set; }

        private int whitePieces;
        private int blackPieces;
        public Text resultText;  // assign this in the editor
        public bool isGameEnd = false;


        // Start is called before the first frame update
        void Start()
        {
            currentPlayer = Player.White;
            whitePieces = GameObject.FindGameObjectsWithTag("White").Length;
            blackPieces = GameObject.FindGameObjectsWithTag("Black").Length;
            Debug.Log("RemainingWhite: " + whitePieces);
            Debug.Log("RemainingBlack: " + blackPieces);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ChangeTurn()
        {
            if(!isGameEnd)
            {
                currentPlayer = currentPlayer == Player.White ? Player.Black : Player.White;
               TurnCamera();
            }


            Debug.Log("RemainingWhite: " + whitePieces);
            Debug.Log("RemainingBlack: " + blackPieces);
        }

        public bool IsPlayerTurn(GameObject piece)
        {
            return piece.tag == currentPlayer.ToString();
        }

        public void TurnCamera()
        {
            IsTurnChanging = true;

            board.gameObject.transform.rotation = Quaternion.Euler(board.transform.eulerAngles.x,
                                              board.transform.eulerAngles.y + 180f,
                                              board.transform.eulerAngles.z);

            IsTurnChanging = false;
        }

        public void PieceRemoved(string tag)
        {
            if (tag == "White")
            {
                whitePieces--;
            }
            else if (tag == "Black")
            {
                blackPieces--;
            }
        }

        public void CheckGameEnd()
        {
            if (whitePieces == 0 || blackPieces == 0)
            {
                isGameEnd = true;
                EndGame();
            }
        }

        private void EndGame()
        {
            int winner = whitePieces > blackPieces ? 1 : 0;
            /*if(currentPlayer is Player.Black)
            {
                Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.eulerAngles.x,
                                                          Camera.main.transform.eulerAngles.y + 180f,
                                                          Camera.main.transform.eulerAngles.z);
            }*/
            Canvas.gameObject.SetActive(true);

            Result.sprite = Winner[winner];

            Buttons[0].AddListenerOnly(() =>
            {
                SceneManager.LoadScene("StartScene");
            });

            Debug.Log("GameEnd" + " White Piece:" + whitePieces + " Black Pieces: " + blackPieces);
            Debug.Log("Winner: " + winner);
        }
    }
}

