using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    // Start is called before the first frame update
    List<string> PhraseList;
    bool PhraseStarted;
    public Text ChapterObj;
    public Text TextObj;
    public GameObject Confirm;
    [Header("Auto Atributes")]
    public Text dialogueText;
    public Text chapterText;
    public Canvas canvas;
    public GameObject ConfirmOb;
    public delegate void Accept();
    private void Awake()
    {
        PhraseList = new List<string>();
        canvas = gameObject.GetComponent<Canvas>();
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        if (PhraseList.Count != 0 && !PhraseStarted)
        {
            var coroutine = DialogueText(PhraseList[0]);
            PhraseStarted = true;
            StartCoroutine(coroutine);
        }
    }
    IEnumerator ChapterName(string a)
    {
        chapterText = Instantiate(ChapterObj);
        chapterText.transform.SetParent(canvas.transform);
        var rt = chapterText.GetComponent<RectTransform>();
        rt.localPosition = ChapterObj.GetComponent<RectTransform>().position;
        rt.localScale = ChapterObj.GetComponent<RectTransform>().localScale;
        chapterText.text = "";
        Color color;
        for (int i = 0; i < a.Length; i++)
        {
            chapterText.text += a[i];
            yield return new WaitForSeconds(0.03f);
        }
        for (int i = 0; i < 30; i++)
        {
            color = chapterText.color;
            color.a -= 0.02f;
            chapterText.color = color;
            yield return new WaitForSeconds(0.04f);
        }
        Destroy(chapterText);
        yield break;
    }

    IEnumerator DialogueText(string a)
    {
        dialogueText = Instantiate(TextObj);
        dialogueText.transform.SetParent(canvas.transform);
        var rt = dialogueText.GetComponent<RectTransform>();
        rt.localPosition = TextObj.GetComponent<RectTransform>().position;
        rt.localScale = TextObj.GetComponent<RectTransform>().localScale;
        dialogueText.text = "";
        Color color;
        for (int i = 0; i < a.Length; i++)
        {
            dialogueText.text += a[i];
            yield return new WaitForSeconds(0.005f);
        }
        for (int i = 0; i < 25; i++)
        {
            color = dialogueText.color;
            color.a -= 0.02f;
            dialogueText.color = color;
            yield return new WaitForSeconds(0.02f);
        }
        Destroy(dialogueText);
        PhraseList.RemoveAt(0);
        PhraseStarted = false;
        yield break;
    }

    public void StartDialogue(string[] a)
    {
        for (int i = 0; i < a.Length; i++)
        {
            PhraseList.Add(a[i]);
        }
    }
    public void StartDialogue(string a)
    {
        PhraseList.Add(a);
    }
    public void StartChapterName(string a) 
    {
       var corutine= ChapterName(a);
        StartCoroutine(corutine);
    }
    public Button ConfirmUse(string question) 
    {
        ConfirmOb = InstantiateConfirm();

        ConfirmOb.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(Close);
        ConfirmOb.transform.GetChild(0).GetComponent<Text>().text = question;
        ConfirmOb.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Close);
        return ConfirmOb.transform.GetChild(1).GetComponent<Button>();
    }
    public GameObject InstantiateConfirm() 
    {
        var temp = Instantiate(Confirm,gameObject.transform);
        var rt = temp.GetComponent<RectTransform>();
        rt.localPosition = Confirm.GetComponent<RectTransform>().position;
        rt.localScale = new Vector3(1,1,1);
        return temp;
    }
    public void Close() 
    {
        if (ConfirmOb != null) 
        {
            Destroy(ConfirmOb);
        }
    }
}
