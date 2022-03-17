using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class Manager : MonoBehaviour
{
    public AudioSource audCoin;
    public AudioSource dial;

    public float coins = 0;

    public float currentLevel;

    public bool canDash = false;
    public bool haveKey = false;

    public int nextScene;

    public bool firstDeath;

    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    public float typingSpeed;

    public bool trigger;
    public Color tutorial = new Color(49, 249, 255, 255);
    public Color narration = new Color(255, 249, 49, 255);

    public Dialogue[] dialogues;

    public Dialogue fD;
    bool fDdone;

    public Dialogue key;
    public bool noKey;

    public bool far;
    public Dialogue farD;

    bool dashDone;
    public Dialogue dashD;

    public TextMeshProUGUI coinDisplay;
    public TextMeshProUGUI rightSide;

    void Update()
    {
        HandleUI();
        RestartScene();
        OtherCases();
        Dialogues();
    }

    public void CanDash()
    {
        canDash = true;
    }

    public void CannotDash()
    {
        canDash = false;
    }

    public void HaveKey()
    {
        haveKey = true;
    }

    public void DontHaveKey()
    {
        haveKey = false;
    }

    IEnumerator Type()
    {
        foreach (string sentence in sentences)
        {
            textDisplay.text = "";
            dial.Play();
            char[] rakho = sentence.ToCharArray();
            foreach (char letter in rakho)
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            yield return new WaitForSeconds(4);
        }
        textDisplay.text = "";   
    }

    void RestartScene()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    void Dialogues()
    {

        foreach(Dialogue dialogue in dialogues)
        {
            if (dialogue.trigger)
            {
                StopCoroutine("Type");
                sentences = dialogue.sentences;
                StartCoroutine("Type");
                dialogue.trigger = false;
            }
        }
    }

    void OtherCases()
    {
        if (far && !farD.trigger)
        {
            farD.trigger = true;
            far = false;
        }

        if(currentLevel == 1)
        {
            if (firstDeath && !fDdone)
            {
                fD.trigger = true;
                fDdone = true;
            }
        }

        if(currentLevel == 2)
        {
            if (noKey && !key.trigger)
            {
                key.trigger = true;
                noKey = false;
            }
            if(!dashDone && canDash && !dashD.trigger)
            {
                dashDone = true;
                dashD.trigger = true;
            }
        }
    }

    void HandleUI()
    {
        if (coins > 0) coinDisplay.text = "Coins: " + coins;

        if (canDash) rightSide.text = "Dash Available";
        else if (haveKey) rightSide.text = "Have Key";
        else rightSide.text = "";
    }

    public void PlayLastCoinAudio()
    {
        audCoin.Play();
    }

}
