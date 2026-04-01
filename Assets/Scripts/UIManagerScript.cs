using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerScript : MonoBehaviour
{
    

    public void Restart ()
    {
        LoadScene(0);
    }

    public void LoadScene(int value)
    {
        SceneManager.LoadScene (value);
    }
}
