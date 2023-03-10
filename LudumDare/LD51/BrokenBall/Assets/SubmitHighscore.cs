using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SubmitHighscore : MonoBehaviour
{
    public TMP_InputField NameField;
    public UnityEvent BeforeSubmit;
    public UnityEvent AfterSubmit;
    public TextMeshProUGUI ScoreText;

    private void Update()
    {
        ScoreText.text = FindObjectOfType<Highscore>().CurrentScore.ToString();
    }

    public void Submit()
    {
        BeforeSubmit.Invoke();
        FindObjectOfType<Highscore>().LogIn(NameField.text.PadLeft(3, ' '), () =>
        {
            FindObjectOfType<Highscore>().UploadHighscore(NameField.text, () =>
            {
                AfterSubmit.Invoke();
            });
        });
    }
}
