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
        public float dragPower;
        public float speed = 0f;
        private float initSpeed = 3f;

        private PieceState _state;

        public Color highlight = Color.yellow;
        public Renderer objectRenderer;
        public Color originalColor;

        private static List<Piece> selectedObjects = new List<Piece>();

        public DragGauge _dragGauge;

        // Start is called before the first frame update
        void Start()
        {
            _dragGauge = GetComponent<DragGauge>();
            objectRenderer = GetComponent<Renderer>();
            originalColor = objectRenderer.material.color;
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
            objectRenderer.material.color = highlight;
            _dragGauge.selectedPiece = this;

            Debug.Log("Object selected: " + gameObject.name + State);
            Debug.Log(selectedObjects.Count);
        }

        private void Deselect()
        {
            selectedObjects.Remove(this);
            State = PieceState.Idle;
            objectRenderer.material.color = originalColor;

            Debug.Log("Object deselected: " + gameObject.name + State);
            Debug.Log(selectedObjects.Count);

        }

        public void Shoot(Vector3 dir, float dragPow)
        {
            Vector3 shootDirection = -dir; // 정확히 반대 방향으로 발사되도록 설정
            dragPower = dragPow;

            speed = (dragPower * initSpeed) * (2f / 3f) + (1f - (weight / 10f) * (dragPower * initSpeed) * (1f / 3f));

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = shootDirection * speed;
            Debug.Log("Speed: " + speed);
        }



        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {

                Deselect();
                gameObject.SetActive(false);
            }
        }


        public enum PieceState
        {
            Idle,
            Selected,
            Dragging,
        }

    }
}
    