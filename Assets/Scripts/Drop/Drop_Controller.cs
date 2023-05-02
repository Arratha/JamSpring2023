using UnityEngine;

using Drop.Movement;
using Drop.Visuals;
using Drop.Shooting;
using Pickables;


namespace Drop
{
    public class Drop_Controller : MonoBehaviour
    {
        public Drop_ConditionController DropCondition;
        public Drop_MoveController DropMove;
        public Drop_VisualController DropVisual;
        public Drop_ShootingController DropShooting;

        private float _jumpPreparation;
        private const float MaxJumpPreparation = 1f;

        public void Update()
        {
            Jump();
            Shoot();

            MoveParts();
        }

        public void FixedUpdate()
        {
            Move();
            Stop();
        }

        private void MoveParts()
        {
            DropShooting.transform.position = DropMove.transform.position;
        }

        private void Jump()
        {
            if (Input.GetMouseButton(0))
                _jumpPreparation = Mathf.Min(MaxJumpPreparation, _jumpPreparation + Time.deltaTime);
            else
                _jumpPreparation = Mathf.Max(0, _jumpPreparation - 3 * Time.deltaTime);

            if (Input.GetMouseButtonUp(0))
            {
                DropMove.Jump(Input.mousePosition, _jumpPreparation);
                _jumpPreparation = 0;
            }

            DropVisual.GroupUp(_jumpPreparation / MaxJumpPreparation);
        }

        private void Move()
        {
            DropMove.Move(Input.GetAxis("Horizontal"));
        }

        private void Stop()
        {
            if (Input.GetAxis("Horizontal") == 0)
                DropMove.Stop();
        }

        private void Shoot()
        {
            if (Input.GetMouseButtonDown(1))
                DropShooting.Shoot(Input.mousePosition);
        }
    }
}