namespace Rabah.UI.MainComponents
{
    public class InputFieldUIElementFloat : InputFieldUIElement
    {
        public override T GetElementDataStructType<T>()
        {
            if (typeof(T) == typeof(float))
            {
                return (T)(object)float.Parse(InputField.text);
            }
            return default;
        }
    }
}