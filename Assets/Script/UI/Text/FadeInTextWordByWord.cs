using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeInTextWordByWord : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI textToUse;
    [SerializeField] private bool useThisText = false;
    [TextAreaAttribute(4, 15)]
    [SerializeField] private string textToShow;
    [SerializeField] private bool useTextText = false;

    [SerializeField] private float fadeSpeedMultiplier = 0.25f;
    [SerializeField] private bool fade;

    private float colorFloat = 0.1f;
    private int colorInt;
    private int wordCounter = 0;
    private string shownText;
    private string[] words;

    public void Init()
    {
        if (useThisText)
        {
            textToUse = GetComponent<TextMeshProUGUI>();
        }

        if (useTextText)
        {
            textToShow = textToUse.text;
        }

        words = textToShow.Split('\n');
        for (int i = 0; i < words.Length; i++)
        {
            Debug.Log(words[i]);
        }
    }

    private IEnumerator FadeInText()
    {
        while (wordCounter < words.Length)
        {
            if (colorFloat < 1.0f)
            {
                colorFloat += Time.deltaTime * fadeSpeedMultiplier;
                colorInt = (int)(Mathf.Lerp(0.0f, 1.0f, colorFloat) * 255.0f);

                textToUse.text = shownText + "<color=#FFFFFF" + string.Format("{0:X}", colorInt) + ">" + words[wordCounter] + "</color>";
            }
            else
            {
                colorFloat = 0.1f;
                shownText += words[wordCounter] + '\n';
                wordCounter++;
            }

            yield return null;
        }
    }

    public void Fade()
    {
        StartCoroutine(FadeInText());
    }
}