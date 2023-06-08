using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chessggagi
{
    public class Knight : Piece
    {
        // Start is called before the first frame update
        void Start()
        {
            _dragGauge = GetComponent<DragGauge>();
            weight = 3f;

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Shoot(Vector3 dir, float dragPow)
        {
            base.Shoot(dir, dragPow);
            StartCoroutine(ChangeDirection());
        }

        IEnumerator ChangeDirection()
        {
            float delay = Random.Range(0.3f, 0.6f);
            yield return new WaitForSeconds(delay);

            Rigidbody rb = GetComponent<Rigidbody>();
            Vector3 currentVelocity = rb.velocity;

            int randomValue = Random.Range(0, 2); // get a random value of either 0 or 1

            if (randomValue == 0) // turn left
            {
                rb.velocity = Quaternion.Euler(0, -45, 0) * currentVelocity;
            }
            else // turn right
            {
                rb.velocity = Quaternion.Euler(0, 45, 0) * currentVelocity;
            }

        }
    }
}