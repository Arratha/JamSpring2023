using UnityEngine;
using UnityEngine.SceneManagement;

using Drop.Movement;
using Drop.Visuals;
using Drop.Shooting;

namespace Player.Controller
{
    public class PlayerController : MonoBehaviour
    {
        private MainController _main = new MainController();

        [SerializeField] private DropController _drop;

        private void Update()
        {
            _main.Update();
            _drop.Update();
        }

        private void FixedUpdate()
        {
            _drop.FixedUpdate();
        }

        private class MainController
        {
            public void Update()
            {
                ResetScene();
                Exit();
            }

            private void ResetScene()
            {
                if (Input.GetKeyDown(KeyCode.R))
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            private void Exit()
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    Application.Quit();
            }
        }

        [System.Serializable]
        private class DropController
        {
            [SerializeField] private Drop_MoveController _dropMove;
            [SerializeField] private Drop_VisualController _dropVisual;
            [SerializeField] private Drop_ShootingController _dropShooting;

            public void Update()
            {
                Jump();
                Shoot();
            }

            public void FixedUpdate()
            {
                Move();
                Stop();
            }

            private void Jump()
            {
                if (Input.GetKeyDown(KeyCode.W))
                    _dropMove.Jump();
            }

            private void Move()
            {
                _dropMove.Move(Input.GetAxis("Horizontal"));
            }

            private void Stop()
            {
                if (Input.GetAxis("Horizontal") == 0)
                    _dropMove.Stop();
            }

            private void Shoot()
            {
                if (Input.GetMouseButtonDown(0))
                    _dropShooting.Shoot(Input.mousePosition);
            }
        }
    }
}