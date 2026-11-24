using ViVaKR.Korean.Hangul;

namespace ViVaKR.Korean.Pronunciation;

/// <summary>텍스트의 "읽는 소리"를 기준으로 끝소리의 받침을 판정한다.</summary>
public static class PronunciationReader
{
    public static BatchimKind GetEndingBatchim(ReadOnlySpan<char> text)
    {
        int end = text.Length;
        while (end > 0 && IsSkippable(text[end - 1]))
            end--;

        if (end == 0)
            return BatchimKind.Unknown;

        char last = text[end - 1];
        var body = text[..end];

        if (HangulChar.IsSyllable(last))
            return FromJongseong(HangulChar.GetJongseong(last));

        if (char.IsAsciiDigit(last))
            return FromDigits(body);

        if (char.IsAsciiLetter(last))
            return FromLetters(body);

        return last switch
        {
            '#' => BatchimKind.Other, // 샵
            '+' => BatchimKind.None,  // 플러스
            '%' => BatchimKind.None,  // 퍼센트
            _ => BatchimKind.Unknown,
        };
    }

    private static bool IsSkippable(char c)
        => char.IsWhiteSpace(c)
           || c is '.' or ',' or '!' or '?' or ';' or ':' or '…' or '~'
              or '"' or '\'' or '`' or '(' or ')' or '[' or ']' or '{' or '}'
              or '“' or '”' or '‘' or '’' or '「' or '」' or '『' or '』';

    private static BatchimKind FromJongseong(int jongseong) => jongseong switch
    {
        0 => BatchimKind.None,
        8 => BatchimKind.Rieul,
        _ => BatchimKind.Other,
    };

    // ── 숫자 ────────────────────────────────────────────────

    private static BatchimKind FromDigits(ReadOnlySpan<char> text)
    {
        // 끝의 숫자 덩어리 (숫자, 쉼표, 소수점)
        int start = text.Length;
        while (start > 0 && (char.IsAsciiDigit(text[start - 1]) || text[start - 1] is ',' or '.'))
            start--;
        var run = text[start..];

        char lastDigit = run[^1];

        // 소수점 이하는 한 자리씩 읽고, 0 이 아닌 끝자리는 그 숫자의 소리로 끝난다
        if (run.Contains('.') || lastDigit != '0')
            return DigitKind(lastDigit);

        // 정수이고 0 으로 끝남: 끝의 0 개수로 십·백·천·만·억·조 …
        int zeros = 0;
        bool allZero = true;
        for (int i = run.Length - 1; i >= 0; i--)
        {
            if (run[i] == ',') continue;
            if (run[i] == '0') { zeros++; continue; }
            allZero = false;
            break;
        }

        if (allZero)
            return BatchimKind.Other; // 영

        return (zeros, zeros / 4) switch
        {
            (1 or 2 or 3, _) => BatchimKind.Other, // 십 백 천
            (_, 1) => BatchimKind.Other,           // 만
            (_, 2) => BatchimKind.Other,           // 억
            (_, 3) => BatchimKind.None,            // 조
            (_, 4) => BatchimKind.Other,           // 경
            (_, 5) => BatchimKind.None,            // 해
            _ => BatchimKind.Unknown,
        };
    }

    private static BatchimKind DigitKind(char digit) => digit switch
    {
        '0' => BatchimKind.Other, // 영
        '1' => BatchimKind.Rieul, // 일
        '2' => BatchimKind.None,  // 이
        '3' => BatchimKind.Other, // 삼
        '4' => BatchimKind.None,  // 사
        '5' => BatchimKind.None,  // 오
        '6' => BatchimKind.Other, // 육
        '7' => BatchimKind.Rieul, // 칠
        '8' => BatchimKind.Rieul, // 팔
        '9' => BatchimKind.None,  // 구
        _ => BatchimKind.Unknown,
    };

    // ── 영문 ────────────────────────────────────────────────

    private static BatchimKind FromLetters(ReadOnlySpan<char> text)
    {
        int start = text.Length;
        while (start > 0 && char.IsAsciiLetter(text[start - 1]))
            start--;
        var word = text[start..];

        // 한 글자이거나 전부 대문자(약어)면 알파벳 이름으로 읽는다.
        // 소문자가 섞인 단어(Docker, iPhone)는 규칙으로 알 수 없다.
        bool spelledOut = word.Length == 1 || IsAllUpper(word);
        if (!spelledOut)
            return BatchimKind.Unknown;

        return char.ToUpperInvariant(word[^1]) switch
        {
            'L' or 'R' => BatchimKind.Rieul, // 엘, 알
            'M' or 'N' => BatchimKind.Other, // 엠, 엔
            _ => BatchimKind.None,           // 에이 비 씨 디 이 에프 지 에이치 아이 제이 케이 오 피 큐 에스 티 유 브이 더블유 엑스 와이 제트
        };
    }

    private static bool IsAllUpper(ReadOnlySpan<char> word)
    {
        foreach (char c in word)
            if (!char.IsAsciiLetterUpper(c))
                return false;
        return true;
    }
}
