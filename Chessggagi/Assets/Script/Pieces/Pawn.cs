using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chessggagi
{
    public class Pawn : Piece
    {
        private bool isDiagonal = false;
        bool isFront = false;
        // Start is called before the first frame update
        void Start()
        {
            _dragGauge = GetComponent<DragGauge>();
            weight = 1f;

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Shoot(Vector3 dir, float dragPow)
        {
            Vector3 shootDirection = -dir; // 정확히 반대 방향으로 발사되도록 설정
            float slope = shootDirection.z / shootDirection.x; // Calculate the slope of the shooting direction

            //Debug.Log("ShootDir" + shootDirection);
            //Debug.Log("slope" + slope);

            // Check if the shooting direction is nearly diagonal


            isDiagonal = ((slope >= -8f / 5f && slope <= -5f / 8f) || (slope >= 5f / 8f && slope <= 8f / 5f));
            if (tag == "White")
                isFront = shootDirection.z > 0.0f ? true : false;
            else
                isFront = shootDirection.z < 0.0f ? true : false;

            if (isDiagonal && isFront)
            {
                GetComponent<Rigidbody>().mass *= 2;
            }


            base.Shoot(dir, dragPow);


        }

        public override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);

            if (isDiagonal && collision.gameObject.name != "Board")
            {
                GetComponent<Rigidbody>().mass /= 2;

                isDiagonal = false;

                Debug.Log(collision.gameObject.name);
            }
        }

    }
}