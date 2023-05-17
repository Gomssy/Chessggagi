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

    private static List<Piece> selectedObjects = new List<Piece>(); // ���õ� DragNShoot ������Ʈ�� �����ϴ� ����Ʈ
    private bool isSelected; // ������Ʈ�� ���� ���¸� ��Ÿ���� ����    

    public DragGauge _dragGauge;

    // Start is called before the first frame update
    void Start()
    {
        _dragGauge = GetComponent<DragGauge>();
        objectRenderer = GetComponent<Renderer>(); // Renderer ������Ʈ ��������
        originalColor = objectRenderer.material.color; // ������ �� ���� ����
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
            // �̹� ���õ� ������Ʈ�� Ŭ���� ��� ���� ����
            //Deselect();
        }
        else
        {
            // ���õ��� ���� ������Ʈ�� Ŭ���� ��� ����
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

        // ���õ� ���¿� ���� ������ �����մϴ�.
        Debug.Log("Object selected: " + gameObject.name + State);
        Debug.Log(selectedObjects.Count);
    }

    private void Deselect()
    {
        selectedObjects.Remove(this);
        State = PieceState.Idle;
        objectRenderer.material.color = originalColor;

        // ���� ������ ���� ������ �����մϴ�.
        Debug.Log("Object deselected: " + gameObject.name + State);
        Debug.Log(selectedObjects.Count);

    }

    public void Shoot(Vector3 dir, float dragPow)
    {
        // �߻� ������ �����ϼ���.
        Vector3 shootDirection = -dir; // ��Ȯ�� �ݴ� �������� �߻�ǵ��� ����
        dragPower = dragPow;

        speed = (dragPower * initSpeed)/(weight * 2);

        //float shootPower = 10f; // �߻��� ���� ����

        // �߻��ϴ� ������ �����ϼ���.
        // ���� ��� Rigidbody ������Ʈ�� ����Ͽ� ���� ���ϰų�, �̵� ����� �ӵ��� ������ �� �ֽ��ϴ�.
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
    