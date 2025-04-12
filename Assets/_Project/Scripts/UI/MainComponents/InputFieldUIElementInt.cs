using Rabah.UI.MainComponents;

public class InputFieldUIElementInt : InputFieldUIElement
{

    public override T GetElementDataStructType<T>()
    {
        if (typeof(T) == typeof(int))
        {
            return (T)(object)int.Parse(InputField.text);
        }
        return default;
    }
}