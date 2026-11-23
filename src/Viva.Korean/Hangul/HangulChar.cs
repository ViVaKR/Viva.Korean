namespace ViVaKR.Korean.Hangul;

/// <summary>한글 완성형 음절 한 글자에 대한 낮은 수준의 판별·계산.</summary>
public static class HangulChar
{
    private const char SyllableFirst = '\uAC00'; // 가
    private const char SyllableLast = '\uD7A3';  // 힣
    private const int JongseongCount = 28;
    private const int RieulJongseong = 8;

    /// <summary>완성형 한글 음절(가~힣)인지.</summary>
    public static bool IsSyllable(char c) => c is >= SyllableFirst and <= SyllableLast;

    /// <summary>종성 번호(0 = 받침 없음, 8 = ㄹ). 음절이 아니면 -1.</summary>
    public static int GetJongseong(char c)
        => IsSyllable(c) ? (c - SyllableFirst) % JongseongCount : -1;

    /// <summary>받침이 있는 음절인지. 한글이 아니면 false.</summary>
    public static bool HasBatchim(char c) => GetJongseong(c) > 0;

    /// <summary>받침이 정확히 ㄹ 인지 (겹받침 ㄺ 등은 제외). 으로/로 판정에 쓴다.</summary>
    public static bool IsRieulBatchim(char c) => GetJongseong(c) == RieulJongseong;
}
