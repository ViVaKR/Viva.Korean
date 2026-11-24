using ViVaKR.Korean.Josa;

namespace Viva.Korean.Tests;

public class JosaTests
{
    [Theory]
    // 은/는 이/가 을/를 과/와
    [InlineData("사과", "은/는", "사과는")]
    [InlineData("책", "은/는", "책은")]
    [InlineData("철수", "이/가", "철수가")]
    [InlineData("민준", "이/가", "민준이")]
    [InlineData("사과", "을/를", "사과를")]
    [InlineData("책", "을/를", "책을")]
    [InlineData("사과", "과/와", "사과와")]
    [InlineData("책", "과/와", "책과")]
    // 이랑/랑 이나/나 이라/라
    [InlineData("사과", "이랑/랑", "사과랑")]
    [InlineData("책", "이랑/랑", "책이랑")]
    [InlineData("사과", "이나/나", "사과나")]
    [InlineData("책", "이나/나", "책이나")]
    [InlineData("사과", "이라/라", "사과라")]
    [InlineData("책", "이라/라", "책이라")]
    // 으로/로: ㄹ 받침은 "로", 겹받침 ㄺ 은 "으로"
    [InlineData("사과", "으로/로", "사과로")]
    [InlineData("책", "으로/로", "책으로")]
    [InlineData("서울", "으로/로", "서울로")]
    [InlineData("닭", "으로/로", "닭으로")]
    // 호격·서술
    [InlineData("철수", "아/야", "철수야")]
    [InlineData("민준", "아/야", "민준아")]
    [InlineData("사과", "이에요/예요", "사과예요")]
    [InlineData("책", "이에요/예요", "책이에요")]
    // 순서를 거꾸로 써도 같은 결과
    [InlineData("사과", "는/은", "사과는")]
    [InlineData("책", "를/을", "책을")]
    // 숫자
    [InlineData("2", "이/가", "2가")]
    [InlineData("3", "은/는", "3은")]
    [InlineData("1", "으로/로", "1로")]
    [InlineData("10", "을/를", "10을")]
    [InlineData("1,000", "이/가", "1,000이")]
    // 영문 약어·숫자 섞인 토큰·기호
    [InlineData("SQL", "은/는", "SQL은")]
    [InlineData("SQL", "으로/로", "SQL로")]
    [InlineData("AI", "이/가", "AI가")]
    [InlineData("Win11", "을/를", "Win11을")]
    [InlineData("C#", "은/는", "C#은")]
    // 끝의 따옴표·괄호는 뒤에 두고 조사만 붙임
    [InlineData("\"사과\"", "은/는", "\"사과\"는")]
    [InlineData("(책)", "을/를", "(책)을")]
    // 판단할 수 없으면 병기
    [InlineData("Docker", "을/를", "Docker을(를)")]
    [InlineData("iPhone", "이/가", "iPhone이(가)")]
    [InlineData("Docker", "으로/로", "Docker으로(로)")]
    public void Josa_단어에_조사를_붙인다(string word, string pair, string expected)
        => Assert.Equal(expected, word.Josa(pair));

    [Theory]
    [InlineData("사과", "을/를", "를")]
    [InlineData("책", "을/를", "을")]
    [InlineData("서울", "으로/로", "로")]
    [InlineData("Docker", "을/를", "을(를)")]
    public void Select_조사만_돌려준다(string word, string pair, string expected)
        => Assert.Equal(expected, JosaResolver.Select(word, pair));

    [Theory]
    [InlineData("")]
    [InlineData("은")]
    [InlineData("을를")]
    [InlineData("가/나")]
    [InlineData("은/는/이")]
    public void 지원하지_않는_쌍은_ArgumentException(string pair)
        => Assert.Throws<ArgumentException>(() => JosaResolver.Select("사과", pair));
}
