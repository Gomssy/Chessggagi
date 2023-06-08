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
                StartCoroutine(TurnCamera());
            }


            Debug.Log("RemainingWhite: " + whitePieces);
            Debug.Log("RemainingBlack: " + blackPieces);
        }

        public bool IsPlayerTurn(GameObject piece)
        {
            return piece.tag == currentPlayer.ToString();
        }

        IEnumerator TurnCamera()
        {
            IsTurnChanging = true;

            Quaternion startRotation = Camera.main.transform.rotation;
            Quaternion endRotation = Quaternion.Euler(Camera.main.transform.eulerAngles.x,
                                                      Camera.main.transform.eulerAngles.y + 180f,
                                                      Camera.main.transform.eulerAngles.z);
            float elapsedTime = 0;
            float rotateTime = 1f; // Set your rotation time (1 sec in this case)

            while (elapsedTime < rotateTime)
            {
                Camera.main.transform.rotation = Quaternion.Slerp(startRotation, endRotation, (elapsedTime / rotateTime));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

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
            /*if (whitePieces <= 0 && blackPieces > 0)
            {
                resultText.text = "Black Win";
                // Do any other end of game processing here
            }
            else if (blackPieces <= 0 && whitePieces > 0)
            {
                resultText.text = "White Win";
                // Do any other end of game processing here
            }
            else if (blackPieces <= 0 && whitePieces <= 0)
            {
                resultText.text = "Draw";
                // Do any other end of game processing here
            }*/
            string winner = whitePieces > blackPieces ? "White" : "Black";
            if (whitePieces == blackPieces)
                winner = "Draw";

            Buttons[0].gameObject.SetActive(true);
            if(currentPlayer is Player.Black)
            {
                Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.eulerAngles.x,
                                                          Camera.main.transform.eulerAngles.y + 180f,
                                                          Camera.main.transform.eulerAngles.z);
            }

            Buttons[0].AddListenerOnly(() =>
            {
                SceneManager.LoadScene("StartScene");
                Buttons[0].gameObject.SetActive(false);
            });

            Debug.Log("GameEnd" + " White Piece:" + whitePieces + " Black Pieces: " + blackPieces);
            Debug.Log("Winner: " + winner);
        }
    }
}

