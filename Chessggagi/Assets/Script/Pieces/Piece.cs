using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Chessggagi
{
    public class Piece : MonoBehaviour
    {
        public float weight;
        public float speed = 0f;
        private float initSpeed = 3f;

        private PieceState _state;

        public Color highlight = Color.yellow;
        private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();

        public static List<Piece> selectedObjects = new List<Piece>();

        public DragGauge _dragGauge;
        private bool isOut = false;


        // Start is called before the first frame update
        void Start()
        {
            _dragGauge = GetComponent<DragGauge>();
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

        private King FindKing()
        {
            GameObject kingsParent = gameObject.tag == "White" ? GameObject.Find("White/KingWhite") : GameObject.Find("Black/KingBlack");
            if (kingsParent == null) // kingsParent�� null�� ��츦 üũ
    {
        return null; // null�� ��ȯ
    }
            King[] kings = kingsParent.GetComponentsInChildren<King>();
            return kings.Length > 0 ? kings[0] : null;
        }

        private void OnMouseUp()
        {
            King king = FindKing();
            if (king != null && king.tryCastling)
            {
                if (selectedObjects.Contains(this))
                    Deselect();
                return;
            }

            if (!GameManager.Inst.IsPlayerTurn(gameObject) || GameManager.Inst.isGameEnd)
            {
                return;
            }
            if (selectedObjects.Contains(this))
            {
                
            }
            else
            {
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
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                originalColors[renderer] = renderer.material.color;
                renderer.material.color = highlight;
            }
            _dragGauge.selectedPiece = this;

            Debug.Log("Object selected: " + gameObject.name + State);
            Debug.Log(selectedObjects.Count);
        }

        public void Deselect()
        {
            selectedObjects.Remove(this);
            State = PieceState.Idle;
             foreach (Transform child in transform)
        {
            var renderer = child.GetComponent<Renderer>();
            if (renderer != null && originalColors.ContainsKey(renderer))
            {
                // �ڽ� ������Ʈ�� ���� �������� ����
                renderer.material.color = originalColors[renderer];
            }
        }

            //Debug.Log("Object deselected: " + gameObject.name + State);
            //Debug.Log(selectedObjects.Count);

        }

        public virtual void Shoot(Vector3 dir, float dragPow)
        {

            GameManager.Inst.CurrentPiece = this;
            Vector3 shootDirection = -dir; // ��Ȯ�� �ݴ� �������� �߻�ǵ��� ����

            speed = (dragPow * initSpeed) * (4f / 5f);

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(shootDirection * speed, ForceMode.VelocityChange);

            GameManager.Inst.CheckGameEnd();
            if(!GameManager.Inst.isGameEnd)
                StartCoroutine(ForceStopAfterDelay(2f));
            //Debug.Log("Speed: " + speed);

        }

        private IEnumerator ForceStopAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            // ������ �⹰�� ������Ŵ
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            Deselect();
            GameManager.Inst.CurrentPiece = null;
            // ���� �ѱ�
            GameManager.Inst.ChangeTurn();
        }



        public virtual void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Wall") && !isOut)
            {
                isOut = true;
                Deselect();
                GameManager.Inst.PieceRemoved(tag);
                gameObject.SetActive(false);
                GameManager.Inst.CheckGameEnd();
                
                if (GameManager.Inst.CurrentPiece == this)
                {
                    if (!GameManager.Inst.isGameEnd)
                        GameManager.Inst.ChangeTurn();
                }


            }


        }

        public void ChangeColor(Color newColor)
        {
            foreach (Transform child in transform)
            {
                var renderer = child.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // �ڽ� ������Ʈ�� ������ ����
                    renderer.material.color = newColor;
                }
            }
        }


        public enum PieceState
        {
            Idle,
            Selected,
            Dragging,
            Shot,
        }

    }
}
    