using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chessggagi
{
    public class Queen : Piece
    {
        private bool isDiagonal = false;
        private bool isCross = false;
        // Start is called before the first frame update
        void Start()
        {
            _dragGauge = GetComponent<DragGauge>();
            weight = 9f;

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


            isDiagonal = ((slope >= -8f / 5f && slope <= -5f / 8f) || (slope >= 5f / 8f && slope <= 8f / 5f));
            isCross = (slope <= -8f / 5f || slope >= 8f / 5f || (slope >= -5f / 8f && slope <= 5f / 8f));



            if (isDiagonal || isCross)
            {
                GetComponent<Rigidbody>().mass *= 1.8f;
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
                isCross = false;

                //Debug.Log(collision.gameObject.name);
            }
        }

    }
}