using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RouletteApi.Helpers
{
    public class BetRules
    {
        public const double MaxAmountBet = 10000;
        public const int MaxNumberBet = 36;
        public const int MinNumberBet = 0;
        public ReadOnlyCollection<string> ColoursBet => new List<string> { "Red", "Black" }.AsReadOnly();
    }
}
