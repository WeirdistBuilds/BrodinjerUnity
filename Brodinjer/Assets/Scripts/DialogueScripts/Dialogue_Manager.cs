using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Dialogue_Manager : MonoBehaviour
{
    public TextMeshProUGUI Dialouge_Text, Character_Text;
    public GameObject Dialouge_Object;
    public string interact_key;
    public NPCObject NPC;
    public UnityEvent OnInteract, OnFinish;
    public BoolData ConvStart;
    public List<UnityEvent> dialogueActions;

    private bool inRange;
    private string _text_to_display;
    public float textScrollSpeed;
    private string _actionCharacter = "^";
    private int _actionIndex;

    private bool continueText;

    public PauseMenu menuScript;

    public GameObject TutorialInteract;
    
    
    private void Start()
    {
        inRange = false;
        ConvStart.value = false;
        Dialouge_Text.text = "";
        Character_Text.text = "";
        Dialouge_Object.SetActive(false);
        continueText = false;
        TutorialInteract.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(TutorialInteract!= null)
                TutorialInteract.SetActive(true);
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(TutorialInteract != null)
                TutorialInteract.SetActive(false);
            inRange = false;
        }
    }

    private void FixedUpdate()
    {
        if (inRange && !(ConvStart.value) && Input.GetButtonDown(interact_key))
        {
            if(TutorialInteract != null)
                TutorialInteract.SetActive(false);
            OnInteract.Invoke();
        }
    }

    public void StartConvInteract()
    {
        if (!ConvStart.value){
            ConvStart.value = true;
            Dialouge_Object.SetActive(true);
            StartCoroutine(ScrollText());
            menuScript.enabled = false;
            TutorialInteract.SetActive(false);
        }
    }

    public void StartConvCutscene()
    {
        if (!ConvStart.value){
            ConvStart.value = true;
            Dialouge_Object.SetActive(true);
            StartCoroutine(ScrollTextCutscene());
            menuScript.enabled = false;
            TutorialInteract.SetActive(false);
        }
    }

    public void NextLine()
    {
        if (!ConvStart.value)
        {
            StartConvCutscene();
        }
        else
        {
            continueText = true;
        }
    }

    private IEnumerator ScrollTextCutscene()
    {
        Character_Text.text = NPC.dialogue.characterName;
        for (int i = 0; i < NPC.dialogue.lines.Count; i++)
        {
            continueText = false;
            _text_to_display = "";
            if (NPC.dialogue.lines[i].Contains(_actionCharacter))
            {

                _actionIndex = int.Parse(NPC.dialogue.lines[i].Split('^')[1]);
                dialogueActions[_actionIndex].Invoke();
            }
            else
            {
                for (int j = 0; j < NPC.dialogue.lines[i].Length; j++)
                {
                    _text_to_display += NPC.dialogue.lines[i][j];
                    Dialouge_Text.text = _text_to_display;
                    yield return new WaitForSeconds(textScrollSpeed);
                }

                yield return new WaitForSeconds(.01f);

                yield return new WaitUntil(() => continueText);

            }
        }
        Dialouge_Object.SetActive(false);
        OnFinish.Invoke();
        menuScript.enabled = true;
        ConvStart.value = false;
    }
    
    private IEnumerator ScrollText()
    {
        Character_Text.text = NPC.dialogue.characterName;
        for (int i = 0; i < NPC.dialogue.lines.Count; i++)
        {
            _text_to_display = "";
            if (NPC.dialogue.lines[i].Contains(_actionCharacter))
            {
                
                _actionIndex = int.Parse(NPC.dialogue.lines[i].Split('^')[1]);
                dialogueActions[_actionIndex].Invoke();
            }
            else
            {
                textScrollSpeed = .001f;
                for (int j = 0; j < NPC.dialogue.lines[i].Length; j++)
                {
                    _text_to_display += NPC.dialogue.lines[i][j];
                    Dialouge_Text.text = _text_to_display;
                    yield return new WaitForSeconds(textScrollSpeed);
                    if (Input.GetButtonDown(interact_key))
                    {
                        Dialouge_Text.text = NPC.dialogue.lines[i];
                        break;
                    }
                }

                yield return new WaitForSeconds(.01f);
                if (TutorialInteract != null)
                    TutorialInteract.SetActive(true);
                yield return new WaitUntil(() => Input.GetButtonDown(interact_key));
            }
        }
        
        Dialouge_Object.SetActive(false);
        OnFinish.Invoke();
        menuScript.enabled = true;
        ConvStart.value = false;
        if (TutorialInteract != null)
            TutorialInteract.SetActive(false);
    }



    public void CloseDialogue()
    {
        if(TutorialInteract != null)
            TutorialInteract.SetActive(false);
        Dialouge_Object.SetActive(false);
        ConvStart.value = false;
        menuScript.enabled = true;

    }


}
