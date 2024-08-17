// Create a new C# script called TagEnumGenerator.cs and place it in the Editor folder.
using UnityEngine;
using UnityEditor;
using System.IO;

public class TagEnumGenerator : MonoBehaviour
{
    private const string enumName = "Tags";
    private const string filePath = "Assets/Scripts/Tools"; // Change this path as needed.

    [MenuItem("Tools/Generate Tag Enum")]
    public static void GenerateTagEnum()
    {
        string[] tags = UnityEditorInternal.InternalEditorUtility.tags;
        string enumCode = GenerateEnumCode(tags);

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        string fullPath = Path.Combine(filePath, $"{enumName}.cs");
        File.WriteAllText(fullPath, enumCode);
        AssetDatabase.Refresh();

        Debug.Log("Tag enum generated successfully!");
    }

    private static string GenerateEnumCode(string[] tags)
    {
        string enumHeader = "public enum " + enumName + "\n{\n";
        string enumBody = string.Empty;

        for (int i = 0; i < tags.Length; i++)
        {
            enumBody += "    " + tags[i].Replace(" ", "_");
            if (i < tags.Length - 1)
                enumBody += ",";
            
            enumBody += "\n";
        }

        string enumFooter = "}";

        return enumHeader + enumBody + enumFooter;
    }
}