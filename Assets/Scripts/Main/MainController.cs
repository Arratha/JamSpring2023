using UnityEngine;
using UnityEngine.SceneManagement;


namespace Main.Controller
{
    public class MainController : MonoBehaviour
    {
        private LevelController _level = new LevelController();

        private void Update()
        {
            _level.Update();
        }

        private class LevelController
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
    }
}