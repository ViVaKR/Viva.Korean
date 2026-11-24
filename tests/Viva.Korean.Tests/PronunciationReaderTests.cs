using ViVaKR.Korean.Pronunciation;

namespace Viva.Korean.Tests;

public class PronunciationReaderTests
{
    [Fact]
    public void 기본값은_Unknown()
        => Assert.Equal(BatchimKind.Unknown, default(BatchimKind));

    [Theory]
    // 한글: 마지막 음절 그대로
    [InlineData("사과", BatchimKind.None)]
    [InlineData("책", BatchimKind.Other)]
    [InlineData("서울", BatchimKind.Rieul)]
    [InlineData("닭", BatchimKind.Other)]
    // 끝의 기호·공백은 건너뜀
    [InlineData("\"사과\"", BatchimKind.None)]
    [InlineData("(책)", BatchimKind.Other)]
    [InlineData("서울.", BatchimKind.Rieul)]
    [InlineData("사과 ", BatchimKind.None)]
    // 숫자 한 자리: 영 일 이 삼 사 오 육 칠 팔 구
    [InlineData("0", BatchimKind.Other)]
    [InlineData("1", BatchimKind.Rieul)]
    [InlineData("2", BatchimKind.None)]
    [InlineData("3", BatchimKind.Other)]
    [InlineData("4", BatchimKind.None)]
    [InlineData("5", BatchimKind.None)]
    [InlineData("6", BatchimKind.Other)]
    [InlineData("7", BatchimKind.Rieul)]
    [InlineData("8", BatchimKind.Rieul)]
    [InlineData("9", BatchimKind.None)]
    // 끝자리 0: 십(ㅂ) 백(ㄱ) 천(ㄴ) 만(ㄴ) 억(ㄱ) 조(받침 없음)
    [InlineData("10", BatchimKind.Other)]
    [InlineData("20", BatchimKind.Other)]
    [InlineData("100", BatchimKind.Other)]
    [InlineData("1000", BatchimKind.Other)]
    [InlineData("10000", BatchimKind.Other)]
    [InlineData("100000", BatchimKind.Other)]          // 십만 → 만
    [InlineData("100000000", BatchimKind.Other)]       // 억
    [InlineData("1000000000000", BatchimKind.None)]    // 조
    // 쉼표·소수점
    [InlineData("1,000", BatchimKind.Other)]
    [InlineData("1,234", BatchimKind.None)]            // 사
    [InlineData("3.5", BatchimKind.None)]              // 오
    // 영문 약어: 알파벳 이름으로 (에이아이, 에스큐엘, ...)
    [InlineData("AI", BatchimKind.None)]
    [InlineData("API", BatchimKind.None)]
    [InlineData("HTTP", BatchimKind.None)]             // 피
    [InlineData("PDF", BatchimKind.None)]              // 에프
    [InlineData("CPU", BatchimKind.None)]              // 유
    [InlineData("SQL", BatchimKind.Rieul)]             // 엘
    [InlineData("VR", BatchimKind.Rieul)]              // 알
    [InlineData("JSON", BatchimKind.Other)]            // 엔
    [InlineData("RAM", BatchimKind.Other)]             // 엠
    // 영문 한 글자
    [InlineData("C", BatchimKind.None)]
    [InlineData("L", BatchimKind.Rieul)]
    [InlineData("M", BatchimKind.Other)]
    // 숫자로 끝나는 토큰
    [InlineData("GPT4", BatchimKind.None)]
    [InlineData("M4", BatchimKind.None)]
    [InlineData("Win11", BatchimKind.Rieul)]           // 십일
    [InlineData("v2", BatchimKind.None)]
    // 기호
    [InlineData("C#", BatchimKind.Other)]              // 씨샵
    [InlineData("C++", BatchimKind.None)]              // 씨플러스피? 아니라 플러스 → 스
    [InlineData("50%", BatchimKind.None)]              // 퍼센트
    // 알 수 없음: 소문자가 섞인 영문 단어, 빈 값, 기호뿐
    [InlineData("Docker", BatchimKind.Unknown)]
    [InlineData("Viva.Chat", BatchimKind.Unknown)]
    [InlineData("iPhone", BatchimKind.Unknown)]
    [InlineData("server", BatchimKind.Unknown)]
    [InlineData("", BatchimKind.Unknown)]
    [InlineData("   ", BatchimKind.Unknown)]
    [InlineData("!!!", BatchimKind.Unknown)]
    public void GetEndingBatchim(string text, BatchimKind expected)
        => Assert.Equal(expected, PronunciationReader.GetEndingBatchim(text));
}
