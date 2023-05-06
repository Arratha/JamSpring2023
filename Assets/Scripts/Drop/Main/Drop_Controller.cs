using UnityEngine;

using Drop.Movement;
using Drop.Visuals;
using Drop.Shooting;


namespace Drop
{
    public class Drop_Controller : MonoBehaviour
    {
        public Drop_ConditionController DropCondition;
        public Drop_MoveController DropMove;
        public Drop_VisualController DropVisual;
        public Drop_ShootingController DropShooting;
        public Drop_MessageController DropMessage;

        private float _jumpPreparation;
        private const float MaxJumpPreparation = 1f;

        private bool _hasControl = true;

        [SerializeField] private AudioSource _audio;

        private void Update()
        {
            MoveParts();

            if (!_hasControl)
                return;

            Jump();
            Shoot();
        }

        private void FixedUpdate()
        {
            if (!_hasControl)
                return;

            Move();
            Stop();
        }

        public void SetControl(bool isActive)
        {
            _hasControl = isActive;
        }

        public void RemoveControl()
        {
            _hasControl = false;

            DropMove.RemovePhysics();
        }

        private void MoveParts()
        {
            DropShooting.transform.position = DropMove.transform.position;
            DropMessage.transform.position = DropMove.transform.position;
        }

        private void Jump()
        {
            if (Input.GetMouseButton(1))
                _jumpPreparation = Mathf.Min(MaxJumpPreparation, _jumpPreparation + Time.deltaTime);
            else
                _jumpPreparation = Mathf.Max(0, _jumpPreparation - 3 * Time.deltaTime);

            if (Input.GetMouseButtonUp(1))
            {
                DropMove.Jump(Input.mousePosition, _jumpPreparation);
                _jumpPreparation = 0;
                _audio.Play();
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
            if (Input.GetMouseButtonDown(0))
                DropShooting.Shoot(Input.mousePosition);
        }
    }
}