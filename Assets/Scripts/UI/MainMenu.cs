using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject creditsUI;
    [SerializeField] private GameObject settingsUI;

    public void StartGame()
    {
        // Carga la siguiente escena en el Build Index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Credits()
    {
        transitionToUI(creditsUI);
    }

    public void Settings()
    {
        transitionToUI(settingsUI);
    }

    public void BackToMainMenu()
    {
        transitionToUI(mainMenuUI);
    }

    private void transitionToUI(GameObject targetUI)
    {
        mainMenuUI.SetActive(false);
        creditsUI.SetActive(false);
        settingsUI.SetActive(false);

        targetUI.SetActive(true);
    }
}
