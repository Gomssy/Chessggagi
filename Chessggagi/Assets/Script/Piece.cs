using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Piece : MonoBehaviour
{
    public float weight;
    public float dragPower;
    public float speed = 0f;
    private float initSpeed = 5f;

    private PieceState _state;

    public Color highlight = Color.yellow;
    public Renderer objectRenderer;
    public Color originalColor;

    private static List<Piece> selectedObjects = new List<Piece>(); // 선택된 DragNShoot 오브젝트를 저장하는 리스트
    private bool isSelected; // 오브젝트의 선택 상태를 나타내는 변수    

    public DragGauge _dragGauge;

    // Start is called before the first frame update
    void Start()
    {
        _dragGauge = GetComponent<DragGauge>();
        objectRenderer = GetComponent<Renderer>(); // Renderer 컴포넌트 가져오기
        originalColor = objectRenderer.material.color; // 원래의 모델 색상 저장
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public PieceState State
    {
        get => _state;
        set => ChangeState(value);
    }

    public void ChangeState(PieceState nextState)
    {
        _state = nextState;
    }

    private void OnMouseUp()
    {
        if (selectedObjects.Contains(this))
        {
            // 이미 선택된 오브젝트를 클릭한 경우 선택 해제
            //Deselect();
        }
        else
        {
            // 선택되지 않은 오브젝트를 클릭한 경우 선택
            Select();
        }
    }

    private void Select()
    {
        if (selectedObjects.Count > 0)
        {
            selectedObjects[0].Deselect();
        }

        selectedObjects.Add(this);
        State = PieceState.Selected;
        objectRenderer.material.color = highlight;
        _dragGauge.selectedPiece = this;

        // 선택된 상태에 대한 동작을 수행합니다.
        Debug.Log("Object selected: " + gameObject.name + State);
        Debug.Log(selectedObjects.Count);
    }

    private void Deselect()
    {
        selectedObjects.Remove(this);
        State = PieceState.Idle;
        objectRenderer.material.color = originalColor;

        // 선택 해제에 대한 동작을 수행합니다.
        Debug.Log("Object deselected: " + gameObject.name + State);
        Debug.Log(selectedObjects.Count);

    }

    public void Shoot(Vector3 dir, float dragPow)
    {
        // 발사 로직을 구현하세요.
        Vector3 shootDirection = -dir; // 정확히 반대 방향으로 발사되도록 설정
        dragPower = dragPow;

        speed = (dragPower * initSpeed)/(weight * 2);

        //float shootPower = 10f; // 발사의 세기 설정

        // 발사하는 로직을 구현하세요.
        // 예를 들어 Rigidbody 컴포넌트를 사용하여 힘을 가하거나, 이동 방향과 속도를 설정할 수 있습니다.
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(shootDirection * speed, ForceMode.Impulse);
        Debug.Log("Speed: " + speed);
    }



    public enum PieceState
    {
        Idle,
        Selected,
        Dragging,
    }

}
    