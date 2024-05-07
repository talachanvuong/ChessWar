public static class StringFormat
{
    public static string Bold(this string text)
    {
        return $"<b>{text}</b>";
    }

    public static string Italic(this string text)
    {
        return $"<i>{text}</i>";
    }

    public static string Color(this string text, string color)
    {
        return $"<color={color}>{text}</color>";
    }

    public static string Size(this string text, int size)
    {
        return $"<size={size}>{text}</size>";
    }

    public static string BoldItalic(this string text)
    {
        return text.Bold().Italic();
    }

    public static string BoldColor(this string text, string color)
    {
        return text.Bold().Color(color);
    }

    public static string ItalicColor(this string text, string color)
    {
        return text.Italic().Color(color);
    }

    public static string Hightlight(this string text, string color)
    {
        return text.Bold().Italic().Color(color);
    }

    public static string Rainbow(this string text)
    {
        string[] colors = { "red", "orange", "yellow", "lime", "cyan", "blue", "magenta" };
        string[] coloredChar = new string[text.Length];
        char[] chars = text.ToCharArray();
        string finalText = string.Empty;

        for (int i = 0, j = 0; i < chars.Length; i++, j++)
        {
            if (j > 6)
            {
                j = 0;
            }
            coloredChar[i] = chars[i].ToString().Color(colors[j]);
        }
        foreach (string item in coloredChar)
        {
            finalText = finalText + item;
        }
        return finalText.BoldItalic();
    }

    public static string Reverse(this string text)
    {
        string result = string.Empty;
        for (int i = text.Length - 1; i > -1; i--)
        {
            result += text[i];
        }
        return result;
    }
}