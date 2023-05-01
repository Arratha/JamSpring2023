using UnityEngine;
using UnityEngine.SceneManagement;

using Drop.Liquids;
using Drop.Movement;
using Drop.Visuals;


namespace Player.Controller
{
    public class PlayerController : MonoBehaviour
    {
        private MainController _main = new MainController();

        [SerializeField] private DropController _drop;

        private void Start()
        {
            _drop.Start();
        }

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
            [SerializeField] private Drop_LiquidsController _dropLiquids;
            [SerializeField] private Drop_MoveController _dropMove;
            [SerializeField] private Drop_VisualController _dropVisual;

            private float _jumpPreparation;
            private const float MaxJumpPreparation = 1f;

            public void Start()
            {
                _dropLiquids.ChangeLiquid(0);
            }

            public void Update()
            {
                Jump();
                ChangeLiquid();
            }

            public void FixedUpdate()
            {
                Move();
                Stop();
            }

            private void Jump()
            {
                if (Input.GetMouseButton(1))
                    _jumpPreparation = Mathf.Min(MaxJumpPreparation, _jumpPreparation + Time.deltaTime);
                else
                    _jumpPreparation = Mathf.Max(0, _jumpPreparation - 3 * Time.deltaTime);


                if (Input.GetMouseButtonUp(1))
                {
                    _dropMove.Jump(Input.mousePosition, _jumpPreparation);
                    _jumpPreparation = 0;
                }

                _dropVisual.Shrink(_jumpPreparation / MaxJumpPreparation);
            }

            private void Move()
            {
                if (Input.GetMouseButton(0))
                    _dropMove.Move(Input.mousePosition);
            }

            private void Stop()
            {
                if (!Input.GetMouseButton(0))
                    _dropMove.Stop();
            }

            private void ChangeLiquid()
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                    _dropLiquids.ChangeLiquid(0);

                if (Input.GetKeyDown(KeyCode.Alpha2))
                    _dropLiquids.ChangeLiquid(1);

                if (Input.GetKeyDown(KeyCode.Alpha3))
                    _dropLiquids.ChangeLiquid(2);
            }
        }
    }
}