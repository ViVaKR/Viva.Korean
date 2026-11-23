using ViVaKR.Korean.Hangul;

namespace Viva.Korean.Tests;

public class HangulCharTests
{
    [Theory]
    [InlineData('가', true)]       // 첫 음절 U+AC00
    [InlineData('힣', true)]       // 마지막 음절 U+D7A3
    [InlineData('책', true)]
    [InlineData('A', false)]
    [InlineData('2', false)]
    [InlineData('ㄱ', false)]      // 자모 단독은 음절이 아님
    [InlineData('ㅏ', false)]
    [InlineData('\uABFF', false)]  // 범위 바로 앞
    [InlineData('\uD7A4', false)]  // 범위 바로 뒤
    public void IsSyllable(char c, bool expected)
        => Assert.Equal(expected, HangulChar.IsSyllable(c));

    // 종성 번호: 0 = 없음, 1 = ㄱ, 4 = ㄴ, 8 = ㄹ, 9 = ㄺ, 17 = ㅂ, 18 = ㅄ, 21 = ㅇ, 27 = ㅎ
    [Theory]
    [InlineData('가', 0)]
    [InlineData('사', 0)]
    [InlineData('책', 1)]
    [InlineData('안', 4)]
    [InlineData('울', 8)]
    [InlineData('닭', 9)]
    [InlineData('밥', 17)]
    [InlineData('값', 18)]
    [InlineData('강', 21)]
    [InlineData('힣', 27)]
    [InlineData('A', -1)]
    [InlineData('2', -1)]
    [InlineData('ㄱ', -1)]
    public void GetJongseong(char c, int expected)
        => Assert.Equal(expected, HangulChar.GetJongseong(c));

    [Theory]
    [InlineData('책')]
    [InlineData('울')]
    [InlineData('밥')]
    [InlineData('강')]
    [InlineData('닭')]
    [InlineData('값')]
    [InlineData('안')]
    [InlineData('힣')]
    public void HasBatchim_받침이_있으면_true(char c)
        => Assert.True(HangulChar.HasBatchim(c));

    [Theory]
    [InlineData('가')]
    [InlineData('사')]
    [InlineData('과')]
    [InlineData('이')]
    [InlineData('서')]
    [InlineData('A')]   // 한글이 아니면 false (읽는 소리 판단은 Pronunciation 층의 일)
    [InlineData('2')]
    [InlineData('!')]
    [InlineData(' ')]
    [InlineData('ㄱ')]
    public void HasBatchim_받침이_없거나_한글이_아니면_false(char c)
        => Assert.False(HangulChar.HasBatchim(c));

    [Theory]
    [InlineData('울')]
    [InlineData('달')]
    [InlineData('말')]
    [InlineData('칼')]
    [InlineData('돌')]
    public void IsRieulBatchim_ㄹ_받침이면_true(char c)
        => Assert.True(HangulChar.IsRieulBatchim(c));

    [Theory]
    [InlineData('책')]
    [InlineData('밥')]
    [InlineData('닭')]   // 겹받침 ㄺ 은 ㄹ 이 아님 (닭으로)
    [InlineData('가')]
    [InlineData('L')]
    public void IsRieulBatchim_그_외는_false(char c)
        => Assert.False(HangulChar.IsRieulBatchim(c));
}
