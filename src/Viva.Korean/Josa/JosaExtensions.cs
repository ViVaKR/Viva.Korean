namespace ViVaKR.Korean.Josa;

public static class JosaExtensions
{
    public static string Josa(this string word, string pair)
    {
        ArgumentNullException.ThrowIfNull(word);

        return string.Concat(word, JosaResolver.Select(word, pair));
    }
}
