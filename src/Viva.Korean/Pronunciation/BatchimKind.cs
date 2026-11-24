namespace ViVaKR.Korean.Pronunciation;

public enum BatchimKind
{
    /// <summary>판단할 수 없음 (기본값).</summary>
    Unknown = 0,
    /// <summary>받침 없음.</summary>
    None,
    /// <summary>ㄹ 받침.</summary>
    Rieul,
    /// <summary>ㄹ 이 아닌 받침.</summary>
    Other,
}
