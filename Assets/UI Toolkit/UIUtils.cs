using UnityEngine.UIElements;

public static class UIUtils
{
    /// <summary>
    /// Appends all the classNames of a VisualElement
    /// </summary>
    /// <param name="element"></param>
    /// <returns>A string containing all the classNames</returns>
    public static string ClassListAppended(VisualElement element)
    {
        string classListAppended = "";

        foreach (var className in element.GetClasses())
        {
            classListAppended += $"{className}, ";
        }

        return  $"{element.name} classNames: {(classListAppended.Length > 0 ? classListAppended : "none")}";
    }
}