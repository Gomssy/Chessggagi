using UnityEngine;

public class DragGauge : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Piece selectedPiece;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool isDragging;
    private float maxLineLength = 2.5f;


    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>(); // LineRenderer ������Ʈ ��������

        // lineRenderer�� null�� ���, LineRenderer ������Ʈ�� �߰��Ͽ� �Ҵ��� ���� �ֽ��ϴ�.
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // LineRenderer ����
        lineRenderer.startWidth = 0.1f; // ���� �ʺ� ����
        lineRenderer.endWidth = 0.1f; // �� �ʺ� ����
        lineRenderer.material.color = Color.yellow; // ���� ���� ����

        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (isDragging)
        {
            startPosition = new Vector3(selectedPiece.transform.position.x, 5f, selectedPiece.transform.position.z);
            endPosition = new Vector3(GetMouseWorldPosition().x, 5f, GetMouseWorldPosition().z);

            float lineLength = Vector3.Distance(startPosition, endPosition);
            if (lineLength > maxLineLength)
            {
                Vector3 direction = (endPosition - startPosition).normalized;
                endPosition = startPosition + direction * maxLineLength;
            }

            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, endPosition);
        }
    }

    private void OnMouseDown()
    {
        if (selectedPiece != null && selectedPiece.State == Piece.PieceState.Selected)
        {
            selectedPiece.State = Piece.PieceState.Dragging;
            startPosition = GetMouseWorldPosition();
            lineRenderer.enabled = true;
            isDragging = true;
        }
    }

    private void OnMouseUp()
    {
        if (selectedPiece != null && selectedPiece.State == Piece.PieceState.Dragging)
        {
            lineRenderer.enabled = false;
            isDragging = false;
            selectedPiece.State = Piece.PieceState.Selected;

            Vector3 dir = endPosition - startPosition;
            float lineLength = Vector3.Distance(startPosition, endPosition);

            selectedPiece.Shoot(dir, lineLength);
            Debug.Log("Line Length: " + lineLength);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}