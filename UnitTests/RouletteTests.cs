using RouletteApp;

namespace UnitTests;
public class RouletteTests
{
    RouletteWheel roulette;
    [SetUp]
    public void Setup()
    {
        roulette = new RouletteWheel();
    }

    [Test]
    public void SpinWheelTest()
    {
        int wheelNumber = roulette.SpinWheel();
        Assert.IsNotNull(wheelNumber);
        Assert.True(wheelNumber >= -1);
        Assert.True(wheelNumber < 37);
    }

    [Test]
    public void CheckBetTest_EvenOdd()
    {
        roulette._bankroll = 200;
        roulette._betSize = 100;
        roulette._betType = 1;
        roulette._betSelection = 0;
        int wheelNumber = 1;
        Assert.False(roulette.CheckBet(wheelNumber));
        Assert.True(roulette._bankroll == 100);
        wheelNumber = 2;
        Assert.True(roulette.CheckBet(wheelNumber));
        Assert.True(roulette._bankroll == 200);
    }

    [Test]
    public void CheckBetTest_RedBlack()
    {
        roulette._bankroll = 200;
        roulette._betSize = 100;
        roulette._betType = 2;
        roulette._betSelection = 0;
        int wheelNumber = 1;
        Assert.False(roulette.CheckBet(wheelNumber));
        Assert.True(roulette._bankroll == 100);
        wheelNumber = 2;
        Assert.True(roulette.CheckBet(wheelNumber));
        Assert.True(roulette._bankroll == 200);
        wheelNumber = -1;
        Assert.False(roulette.CheckBet(wheelNumber));
        Assert.True(roulette._bankroll == 100);
    }

    [Test]
    public void CheckBetTest_SingleNumber()
    {
        roulette._bankroll = 200;
        roulette._betSize = 100;
        roulette._betType = 3;
        roulette._betSelection = 36;
        int wheelNumber = 1;
        Assert.False(roulette.CheckBet(wheelNumber));
        Assert.True(roulette._bankroll == 100);
        wheelNumber = 36;
        Assert.True(roulette.CheckBet(wheelNumber));
        Assert.True(roulette._bankroll == 3600);
    }

    [Test]
    public void CheckBetTest_Thirds()
    {
        roulette._bankroll = 200;
        roulette._betSize = 100;
        roulette._betType = 4;
        roulette._betSelection = 0;
        int wheelNumber = 13;
        Assert.False(roulette.CheckBet(wheelNumber));
        Assert.True(roulette._bankroll == 100);
        wheelNumber = 12;
        Assert.True(roulette.CheckBet(wheelNumber));
        Assert.True(roulette._bankroll == 300);
    }

    [Test]
    public void CheckBetTest_HighLow()
    {
        roulette._bankroll = 200;
        roulette._betSize = 100;
        roulette._betType = 5;
        roulette._betSelection = 0;
        int wheelNumber = 19;
        Assert.False(roulette.CheckBet(wheelNumber));
        Assert.True(roulette._bankroll == 100);
        wheelNumber = 18;
        Assert.True(roulette.CheckBet(wheelNumber));
        Assert.True(roulette._bankroll == 200);
    }

}
