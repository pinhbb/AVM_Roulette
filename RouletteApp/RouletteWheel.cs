using System;
using System.Threading.Tasks;

namespace RouletteApp;

public class RouletteWheel
{
    public float _bankroll { get; set; }
    public float _betSize { get; set; }
    public int _betType { get; set; }
    public int _betSelection { get; set; }
    private int _lastBetType;
    private Random _randomGenerator;


    // default constructor
    public RouletteWheel()
    {
        _bankroll = 0f;
        _betSize = 0f;
        _betType = 0;
        _betSelection = 0;
        _randomGenerator = new Random();
    }

    // main game loop - call this to play the game 
    public void Play()
    {
        PromptForBankroll();
        while (IsPlaying())
        {
            PromptForBet();
            if (_betType == 0) { break; }
            CheckBet(SpinWheel());
        }
        Thread.Sleep(500);
        Console.WriteLine("You currently have $" + Math.Round(_bankroll, 2) + ".");
        Thread.Sleep(500);
        Console.WriteLine("Come back soon!");
        Thread.Sleep(1000);

    }

    // spin the roulette wheel returning a number [-1, 36] where -1 reprsents 00
    public int SpinWheel()
    {
        int wheelNumber = _randomGenerator.Next(-1, 37);
        int delay_ms = 500;
        Console.WriteLine("Bets Closed!");
        Thread.Sleep(delay_ms);
        Console.WriteLine("Spinning!");
        Thread.Sleep(delay_ms);
        Console.WriteLine(".");
        Thread.Sleep(delay_ms * 2);
        Console.WriteLine("..");
        Thread.Sleep(delay_ms * 3);
        Console.WriteLine("...");
        Thread.Sleep(delay_ms * 4);
        string color = (IsRed(wheelNumber) ? "Red " : "Black ");
        if (wheelNumber < 1) { color = "Green"; }
        Console.WriteLine(color + wheelNumber);
        return wheelNumber;
    }

    // get starting bankroll amount from the player
    private void PromptForBankroll() 
    {
        Console.Clear();
        Console.WriteLine("Welcome to the Roulette table!\nPlease enter your starting bankroll:");
        string? input = Console.ReadLine();
        int value = -1;
        while (!int.TryParse(input, out value) || value < 0)
        {
            Console.WriteLine("Invalid bankroll amount. Please enter a number > 0 for your starting bankroll:");
            input = Console.ReadLine();
        }
        _bankroll = value;
    }

    // get bet information from the player
    private void PromptForBet() 
    {
        PromptForBetType();
        if (_betType == 0) { return; }
        PromptForBetSelection();
        PromptForBetSize();
    }
    private void PromptForBetType()
    {
        Console.Clear();

        // get bet type
        Thread.Sleep(500);
        Console.WriteLine("You currently have $" + Math.Round(_bankroll, 2) + ".");
        Thread.Sleep(1000);
        Console.WriteLine("Please select the type of bet you want to make:");
        Console.WriteLine("\t1 -> Red or Black   (Pays 1:1)\n\t2 -> Even or Odd    (Pays 1:1)");
        Console.WriteLine("\t3 -> Single Number  (Pays 35:1)\n\t4 -> Thirds         (Pays 2:1)");
        Console.WriteLine("\t5 -> High or Low    (Pays 1:1)");
        if (_betType != 0) { Console.WriteLine("\t6 -> Repeat Last Bet\n\t7 -> Double Last Bet"); }
        Console.WriteLine("\n\t0 -> Leave Table");
        _lastBetType = _betType;
        _betType = (_betType != 0) ? GetValidInt(0, 7) : GetValidInt(0, 5);
    }
    private void PromptForBetSelection()
    {
        if (_betType == 0) { return; }
        if (_betType != 6 && _betType != 7) { Console.WriteLine("Please make your bet:"); }
        switch (_betType)
        {
            case 1:
                Console.WriteLine("\t0 -> Red\n\t1 -> Black");
                _betSelection = GetValidInt(0, 1);
                break;
            case 2:
                Console.WriteLine("\t0 -> Even\n\t1 -> Odd");
                _betSelection = GetValidInt(0, 1);
                break;
            case 3:
                Console.WriteLine("\tEnter a number 0-36 (-1 for 00):");
                _betSelection = GetValidInt(-1, 36);
                break;
            case 4:
                Console.WriteLine("\t0 -> 1st Third (1 - 12)\n\t1 -> 2nd Third (13 - 24)\n\t2 -> 3rd Third (25 - 36)");
                _betSelection = GetValidInt(0, 2);
                break;
            case 5:
                Console.WriteLine("\t0 -> Low (1 - 18)\n\t1 -> High (19 - 36)");
                _betSelection = GetValidInt(0, 1);
                break;
            case 6:
                Console.WriteLine("Repeating Last Bet...");
                break;
            case 7:
                Console.WriteLine("Doubling Last Bet...");
                _betSize = MathF.Min(_betSize * 2, _bankroll);
                Console.WriteLine("New bet size is $" + MathF.Round(_betSize, 2));
                break;
            default:
                break;
        }
    }
    private void PromptForBetSize() 
    {
        if (_betType == 6 || _betType == 7) { return; }
        Console.WriteLine("How much would you like to bet?");
        string? input = Console.ReadLine();
        int value = -1;
        while (!int.TryParse(input, out value) || _betSize < 0 || _betSize > _bankroll)
        {
            Console.WriteLine("Invalid bet size. You currently have $" + Math.Round(_bankroll, 2) + ".\nHow much would you like to bet?");
            input = Console.ReadLine();
        }
        _betSize = value;
    }

    // get an integer from the player within a range
    private int GetValidInt(int min, int max)
    {
        string? input = Console.ReadLine();
        int value;
        while (!int.TryParse(input, out value) || value < min || value > max)
        {
            Console.WriteLine("Invalid selection, please try again:");
            input = Console.ReadLine();
        }
        return value;
    }

    // check if the player's bet was a success
    public bool CheckBet(int wheelNumber)
    {

        // if 0 or 00 (-1) bet fails (if not single number bet)
        if (wheelNumber < 1 && _betType != 0) 
        {
            _bankroll -= _betSize;
            Console.WriteLine("You lost $" + Math.Round(_betSize, 2) + ".");
            Thread.Sleep(3000);
            return false;
        }
        bool betOutcome = false;
        int betType = (_betType > 5) ? _lastBetType : _betType;
        // red/black or even/odd  
        if (betType == 1 || betType == 2) 
        {
            // bet selection of 0 -> red/even, 1 -> black/odd
            betOutcome = (_betSelection == 0) ? IsRed(wheelNumber) : !IsRed(wheelNumber);
        }
        // single number
        else if (betType == 3)
        {
            // bet selection corresponds to a single number (1-indexed)
            betOutcome = (_betSelection == wheelNumber);
        }
        // thirds
        else if (betType == 4)
        {
            // bet selection of 0 -> (1-12), 1 -> (13-24), 2 -> (25-36)
            betOutcome = (_betSelection == (wheelNumber - 1) / 12);
        }
        // high/low
        else if (betType == 5)
        {
            // bet selection of 0 -> (1-18), 1 -> (19-36)
            betOutcome = (_betSelection == (wheelNumber - 1) / 18);
        }
        // apply bet size and outcome to bankroll
        if (betOutcome)
        {
            _bankroll += _betSize * PayoutOdds();
            Console.WriteLine("You won $" + Math.Round(_betSize * PayoutOdds(), 2) + "!");
            Thread.Sleep(3000);
        }
        else
        {
            _bankroll -= _betSize;
            Console.WriteLine("You lost $" + Math.Round(_betSize, 2) + ".");
            Thread.Sleep(3000);
        }
        return betOutcome;
    }

    // get the payout odds from the bet type
    public int PayoutOdds()
    {
        int odds = 0;
        if (_betType == 1 || _betType == 2 || _betType == 5)
        {
            odds = 1;
        }
        else if (_betType == 3) 
        {
            odds = 35;
        }
        else if (_betType == 4)
        {
            odds = 2;
        }
        return odds;
    }

    // returns true if current number is red, false if black
    private bool IsRed(int wheelNumber) 
    {
        return (wheelNumber % 2 == 0);
    }

    // check if player has money left 
    private bool IsPlaying()
    {
        return (_bankroll > 0);
    }

}