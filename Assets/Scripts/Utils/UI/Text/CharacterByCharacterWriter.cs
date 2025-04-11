using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Events;

public class CharacterByCharacterWriter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text tmpText;

    [SerializeField]
    private float duration = 1f;

    private UnityEvent onWriteComplete = new UnityEvent();

    public WriteOperation Write(string text, bool removeListener = true)
    {
        StartCoroutine(WriteTextCoroutine(text, removeListener));
        return new WriteOperation(onWriteComplete);
    }

    public WriteOperation Write(string text, float duration, bool removeListener = true)
    {
        this.duration = duration;
        StartCoroutine(WriteTextCoroutine(text, removeListener));
        return new WriteOperation(onWriteComplete);
    }

    private IEnumerator WriteTextCoroutine(string text, bool removeListener)
    {
        tmpText.text = "";
        foreach (char c in text)
        {
            tmpText.text += c;
            yield return new WaitForSeconds(duration / text.Length);
        }
        onWriteComplete?.Invoke();
        if (removeListener)
        {
            onWriteComplete.RemoveAllListeners();
        }
    }

    public void SetTextColor(Color color)
    {
        tmpText.color = color;
    }
}
