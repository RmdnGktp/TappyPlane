using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameOverScript : MonoBehaviour
{

    [SerializeField] GameObject newText;
    [SerializeField] Sprite[] Medals;
    [SerializeField] Image Medal;
    [SerializeField] TextMeshProUGUI bestScoreText;
    [SerializeField] TextMeshProUGUI scoreText;
    // float maxDistance;
    [SerializeField] GameObject board;
    [SerializeField] GameObject buttons;

    
    public void GameOver(float score)
    {
        StartCoroutine (GameOverAnimation(score));
    }

    IEnumerator GameOverAnimation(float score)
    {
        yield return new WaitForSeconds(0.5f);
        board.GetComponent<UIAnimationScript>().GameOver();
        yield return new WaitForSeconds(0.5f);
        buttons.GetComponent<UIAnimationScript>().GameOver();
        yield return new WaitForSeconds(0.5f);

        float maxDistance = PlayerPrefs.GetFloat("maxDistance", 0);
        bool isNewRecord = score > maxDistance;
        maxDistance = Mathf.Max (maxDistance, score);
        PlayerPrefs.SetFloat("maxDistance", maxDistance);
        PlayerPrefs.Save();

        // Set Score
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 1f;
            // set score
            float x = Mathf.SmoothStep(0, score, t);
            scoreText.text = Mathf.RoundToInt(x) + "m";
            // set best score
            float y = Mathf.SmoothStep(0, maxDistance, t);
            bestScoreText.text = Mathf.RoundToInt(y) + "m";

            yield return null;
        }

        newText.SetActive(isNewRecord);

        yield return new WaitForSeconds(0.5f);
        addMedals(score);
   
    }

    void addMedals (float value)
    {
        if (value > 2500f)
        {
            Medal.GetComponent<Image>().sprite = Medals [2];
            Medal.GetComponent<Image>().color = new Color (255f/255f, 255f/255f, 255f/255f);
        }
        else if (value > 1500f)
        {
            Medal.GetComponent<Image>().sprite = Medals [1];
            Medal.GetComponent<Image>().color = new Color (255f/255f, 255f/255f, 255f/255f);
        }
        else if (value > 500f)
        {
            Medal.GetComponent<Image>().sprite = Medals [0];
            Medal.GetComponent<Image>().color = new Color (255f/255f, 255f/255f, 255f/255f);
        }
    }


    [ContextMenu ("deleteAllSaves")]
    void deleteAllSaves ()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
