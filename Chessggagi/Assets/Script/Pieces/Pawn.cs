using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    // Start is called before the first frame update
    void Start()
    {
        _dragGauge = GetComponent<DragGauge>();
        objectRenderer = GetComponent<Renderer>(); // Renderer 컴포넌트 가져오기
        originalColor = objectRenderer.material.color; // 원래의 모델 색상 저장
        weight = 1f;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
}
