using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasElements : MonoBehaviour
{
    public Sprite SoundOn, SoundOff;
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Instagram()
    {
        Application.OpenURL("https://instagram.com/havvvanaclub?utm_medium=copy_link");
    }
    public void GameStart()
    {
        GameControll.Status();
    }

    public void Sound()
    {
        if (PlayerPrefs.GetString("sound") == "No")
        {
            PlayerPrefs.SetString("sound", "Yes");
            GetComponent<Image>().sprite = SoundOn;
        }
        else
        {
            PlayerPrefs.SetString("sound", "No");
            GetComponent<Image>().sprite = SoundOff;
        }
    }
}
