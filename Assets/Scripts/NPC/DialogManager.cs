using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogBox;
    public Text dialogText;
    public Text npcNameText;
    public int lettersPerSecond;
    public event Action OnShowDialog;
    public event Action OnHideDialog;

    public static DialogManager Instance {  get; private set; }

    Dialog dialog;
    int currentLine = 0;
    bool isTyping;
    string npcName;

    private void Awake()
    {
        Instance = this;
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isTyping)
        {
            ++currentLine;

            if (currentLine < dialog.lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {

                currentLine = 0;
                dialogBox.SetActive(false);
                OnHideDialog?.Invoke();
            }
        }
    }

    public IEnumerator ShowDialog(Dialog dialog, string npcName)
    {
        this.npcName = npcName;

        yield return new WaitForEndOfFrame();

        OnShowDialog?.Invoke();
        this.dialog = dialog;
        npcNameText.text = npcName;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }

    public IEnumerator TypeDialog(string line)
    {
        isTyping = true;
        dialogText.text = "";

        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }
}
