using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chessggagi
{
    public class Rook : Piece
    {
        private bool isCross = false;
        
        // Start is called before the first frame update
        void Start()
        {
            _dragGauge = GetComponent<DragGauge>();
            weight = 5f;

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                // Raycast to check if we're clicking on this rook
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        // Perform the castling if possible
                        Castling();
                    }
                }
            }
        }

        private void Castling()
        {
            King king = FindKing();

            if (king != null && king.tryCastling && king.canCastle)
            {
                // Swap positions with king
                Vector3 tempPosition = transform.position;
                transform.position = king.transform.position;
                king.transform.position = tempPosition;

                // Cancel castling mode
                king.tryCastling = false;
                king.canCastle = false;

                // Reset rook colors
                king.HighlightRooks(king.originalRookColor);
            }
        }

        private King FindKing()
        {
            GameObject kingsParent = gameObject.tag == "White" ? GameObject.Find("White/KingWhite") : GameObject.Find("Black/KingBlack");
            King[] kings = kingsParent.GetComponentsInChildren<King>();
            return kings.Length > 0 ? kings[0] : null;
        }

        public override void Shoot(Vector3 dir, float dragPow)
        {
            Vector3 shootDirection = -dir; // 정확히 반대 방향으로 발사되도록 설정
            float slope = shootDirection.z / shootDirection.x; // Calculate the slope of the shooting direction

            //Debug.Log("ShootDir" + shootDirection);
            //Debug.Log("slope" + slope);

            // Check if the shooting direction is nearly diagonal


            isCross = (slope <= -8f / 5f || slope >= 8f / 5f || (slope >= -5f / 8f && slope <= 5f / 8f));


            Debug.Log("isCross:" + isCross);

            if (isCross)
            {
                GetComponent<Rigidbody>().mass *= 2;
            }


            base.Shoot(dir, dragPow);


        }

        public override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);

            if (isCross && collision.gameObject.name != "Board")
            {
                GetComponent<Rigidbody>().mass /= 2;

                isCross = false;

                //Debug.Log(collision.gameObject.name);
            }
        }



    }
}