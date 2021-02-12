using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public GameObject title;
    public GameObject subtitile;
    TMP_Text titleHolder;
    TMP_Text sentenceHolder;
    Queue<string> sentences;
    Queue<AudioClip> audios;
    bool titleBool;
    bool subtitleBool;


    public bool dialogueRunning=false;
    bool dialogueStarting=true;

    public bool end;

    public float typingSpeed=0.25f;
    void Start()
    {
        sentences = new Queue<string>();
        audios = new Queue<AudioClip>();

        titleHolder = title.GetComponentInChildren<TMP_Text>();
        sentenceHolder = subtitile.GetComponentInChildren<TMP_Text>();
    }

     
    //Starts the Dialogue
    public void StartDialogue(Dialogue dialogue, AudioSource audioSource)
    {
        Debug.Log("Starting Convo with : " + dialogue.title);

        dialogueRunning = true;
        dialogueStarting = true;
        end = false;

        StartCoroutine(Showtitle(dialogue));

        sentences.Clear();
        foreach (string sentence in dialogue.sentence)
        {
            sentences.Enqueue(sentence);
        }

        foreach (AudioClip audio in dialogue.audio)
        {
            audios.Enqueue(audio);
        }

        nextDialogue();
        nextAudio(audioSource);
    }


    IEnumerator Showtitle(Dialogue dialogue)
    {
        if (titleBool)
        {
            title.SetActive(true);
            titleHolder.text = dialogue.title;
            yield return new WaitForSeconds(2f);
            title.SetActive(false);

            Debug.Log("Showtitle runnung");
        }
        
    }

    //With the AudioSource

    public void nextAudio(AudioSource audioSource)
    {
        if (audios.Count == 0)
        {
            EndofDialogue();
            return;
        }
        
        AudioClip audioclip = audios.Dequeue();
        
        //sentenceHolder.text = sentence;

       
        StartCoroutine(StartDialogueAudio(audioclip, audioSource));

    }
    //Overloaded For Audio
    IEnumerator StartDialogueAudio(AudioClip clip, AudioSource source)
    {
        //sentenceHolder.text = "";

        source.clip = clip;
        source.Play();

        yield return new WaitForSeconds(source.clip.length+1F);

        nextAudio(source);
        nextDialogue();
    }

    #region Dialogue


    public void nextDialogue()
    {
        if (sentences.Count == 0)
        {
            EndofDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        Debug.Log(sentence);
        //sentenceHolder.text = sentence;

        
        StartCoroutine(StartDialogueSentence(sentence));

    }

    public void CanvasBools(bool t, bool s)
    {
        titleBool = t;
        subtitleBool = s;
    }

    IEnumerator StartDialogueSentence(string sentence)
    {
        if (subtitleBool)
        {
            subtitile.SetActive(true);
            sentenceHolder.text = "";
            yield return new WaitForEndOfFrame();
            sentenceHolder.text = sentence;
        }
        

        /*
        foreach (char letter in sentence.ToCharArray())
        {
            sentenceHolder.text += letter;
            yield return new WaitForSeconds(1 / typingSpeed);
        }*/
    }
    #endregion



    public void EndofDialogue()
    {
        Debug.Log("End of Sentence");
        dialogueRunning=false;
        dialogueStarting = false;
        subtitile.SetActive(false);
        StopAllCoroutines();
    }

     public bool End()
    {
        if ((sentences.Count == 0 && audios.Count == 0)&&dialogueStarting==false)
        {
            end = true;
            return true;
        }

        return false;
    }
}
