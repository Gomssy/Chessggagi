using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chessggagi
{
    public class Pawn : Piece
    {
        // Start is called before the first frame update
        void Start()
        {
            _dragGauge = GetComponent<DragGauge>();
            objectRenderer = GetComponent<Renderer>();
            originalColor = objectRenderer.material.color;
            weight = 1f;

        }

        // Update is called once per frame
        void Update()
        {

        }



    }
}