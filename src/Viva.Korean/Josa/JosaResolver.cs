using System.Collections.Frozen;
using ViVaKR.Korean.Pronunciation;

namespace ViVaKR.Korean.Josa;

public static class JosaResolver
{
    // WithBatchim: 받침 있을 때, WithoutBatchim: 받침 없을 때.
    // RieulAsNone: ㄹ 받침을 "받침 없음"처럼 취급 (으로/로)
    private readonly record struct JosaPair(string WithBatchim, string WithoutBatchim, bool RieulAsNone = false);

    private static readonly FrozenDictionary<string, JosaPair> Pairs = BuildPairs();

    private static FrozenDictionary<string, JosaPair> BuildPairs()
    {
        JosaPair[] pairs =
        [
            new("은", "는"),
            new("이", "가"),
            new("을", "를"),
            new("과", "와"),
            new("이랑", "랑"),
            new("이나", "나"),
            new("이라", "라"),
            new("으로", "로", RieulAsNone: true),
            new("아", "야"),
            new("이에요", "예요"),
        ];

        var map = new Dictionary<string, JosaPair>(pairs.Length * 2);
        foreach (var p in pairs)
        {
            map[$"{p.WithBatchim}/{p.WithoutBatchim}"] = p; // 은/는
            map[$"{p.WithoutBatchim}/{p.WithBatchim}"] = p; // 는/은 (거꾸로도 허용)
        }

        return map.ToFrozenDictionary();
    }

    /// <summary>
    /// <paramref name="word"/> 뒤에 붙일 조사만 돌려준다.
    /// 끝소리를 판단할 수 없으면 "은(는)" 처럼 병기한다.
    /// </summary>
    /// <exception cref="ArgumentException">지원하지 않는 조사 쌍.</exception>
    public static string Select(ReadOnlySpan<char> word, string pair)
    {
        ArgumentNullException.ThrowIfNull(pair);

        if (!Pairs.TryGetValue(pair, out var p))
        {
            throw new ArgumentException(
                $"지원하지 않는 조사 쌍입니다: '{pair}'. " +
                "사용 가능: 은/는, 이/가, 을/를, 과/와, 이랑/랑, 이나/나, 이라/라, 으로/로, 아/야, 이에요/예요",
                nameof(pair));
        }

        return PronunciationReader.GetEndingBatchim(word) switch
        {
            BatchimKind.None => p.WithoutBatchim,
            BatchimKind.Rieul => p.RieulAsNone ? p.WithoutBatchim : p.WithBatchim,
            BatchimKind.Other => p.WithBatchim,
            _ => $"{p.WithBatchim}({p.WithoutBatchim})",
        };
    }
}
