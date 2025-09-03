namespace WeddingApp.Extensions;

public static class StringExtensions
{
    public static int LevenshteinDistance(this string s1, string s2)
    {
        s1 = s1.ToLower();
        s2 = s2.ToLower();
        
        if (s1.Length == 0) return s2.Length;
        if (s2.Length == 0) return s1.Length;

        var d = new int[s1.Length + 1, s2.Length + 1];

        for (int i = 0; i <= s1.Length; i++) d[i, 0] = i;
        for (int j = 0; j <= s2.Length; j++) d[0, j] = j;

        for (int i = 1; i <= s1.Length; i++)
        {
            for (int j = 1; j <= s2.Length; j++)
            {
                int cost = s1[i - 1] == s2[j - 1] ? 0 : 1;
                d[i, j] = Math.Min(Math.Min(
                        d[i - 1, j] + 1,      // deletion
                        d[i, j - 1] + 1),     // insertion
                    d[i - 1, j - 1] + cost // substitution
                );
            }
        }

        return d[s1.Length, s2.Length];
    }
}