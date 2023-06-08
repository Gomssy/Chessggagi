using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chessggagi
{
    public class King : Piece
    {
        public Rook[] rooks;
        public bool canCastle = true;
        public bool tryCastling = false;

        [SerializeField]
        private Material rookMaterial;

        public Color originalRookColor;


        // Start is called before the first frame update
        void Start()
        {
            _dragGauge = GetComponent<DragGauge>();
            weight = 10f;

            rooks = FindRooks();
            originalRookColor = rookMaterial.color;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform.gameObject == gameObject && canCastle)
                    {
                        ChangeState(PieceState.Idle);
                        if(selectedObjects.Count > 0)
                            selectedObjects[0].Deselect();
                        tryCastling = !tryCastling;
                        if (tryCastling)
                        {
                            HighlightRooks(Color.green);
                        }

                        else
                        {
                            HighlightRooks(originalRookColor);
                        }

                    }
                }
            }
        }
        public void HighlightRooks(Color color)
        {
            foreach (Rook rook in rooks)
            {
                rook.ChangeColor(color);
            }
        }

        private Rook[] FindRooks()
        {
            GameObject rooksParent;

            rooksParent = gameObject.tag == "White" ? GameObject.Find("White/Rooks") : GameObject.Find("Black/Rooks");

            return rooksParent.GetComponentsInChildren<Rook>();
        }


    }
}