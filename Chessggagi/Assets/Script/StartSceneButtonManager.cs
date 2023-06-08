using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chessggagi
{
    public class StartSceneButtonManager : MonoBehaviour
    {

        public ClickUI[] Buttons;
        [SerializeField]
        private GameObject Instruction;
        // Start is called before the first frame update
        void Start()
        {
            Buttons[0].AddListenerOnly(() =>
            {
                SceneManager.LoadScene("InGame");
            });

            Buttons[1].AddListenerOnly(() =>
            {
                Instruction.SetActive(true);
            });

            Buttons[2].AddListenerOnly(() =>
            {
                Instruction.SetActive(false);
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

