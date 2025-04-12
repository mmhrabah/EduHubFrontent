using UnityEngine.Events;

public class WriteOperation
{
    private UnityEvent onComplete;

    public WriteOperation(UnityEvent onComplete)
    {
        this.onComplete = onComplete;
    }

    public WriteOperation onWriteComplete(UnityAction action)
    {
        onComplete.AddListener(action);
        return this;
    }

    public void RemoveListener(UnityAction action)
    {
        onComplete.RemoveListener(action);
    }
}