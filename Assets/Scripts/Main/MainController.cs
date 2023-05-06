using UnityEngine;
using UnityEngine.SceneManagement;


namespace Main.Controller
{
    public class MainController : MonoBehaviour
    {
        private LevelController _level = new LevelController();

        private void Awake()
        {
            _level.Awake();
        }

        private void Update()
        {
            _level.Update();
        }

        private class LevelController
        {
            public void Awake()
            {
                InitializeLayers();
            }

            public void Update()
            {
                ResetScene();
                Exit();
            }

            private void InitializeLayers()
            {
                Physics2D.IgnoreLayerCollision(6, 6, true);
                Physics2D.IgnoreLayerCollision(6, 10, true);
                Physics2D.IgnoreLayerCollision(6, 11, true);
                Physics2D.IgnoreLayerCollision(7, 8, true);
                Physics2D.IgnoreLayerCollision(7, 10, true);
                Physics2D.IgnoreLayerCollision(8, 8, true);
                Physics2D.IgnoreLayerCollision(8, 10, true);
                Physics2D.IgnoreLayerCollision(8, 11, true);
                Physics2D.IgnoreLayerCollision(10, 10, true);
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
    }
}