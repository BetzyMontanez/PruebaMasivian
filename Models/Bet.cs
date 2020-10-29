using RouletteApi.Exceptions;
using RouletteApi.Helpers;
using System;

namespace RouletteApi.Models
{
    public class Bet
    {
        public string BetPlace { get; set; }
        public double Amount { get; set; }
        public string UserId { get; set; }
        public double EarnedAmount { get; set; }
        public Bet(string betPlace, double amount, string userId)
        {
            this.BetPlace = betPlace;
            this.Amount = amount;
            this.UserId = userId;
        }
        public bool IsValid()
        {
            var valid = true;
            if (this.Amount < 0 || this.Amount > BetRules.MaxAmountBet)
            {
                throw new MessageException("Bet placing error", "Amount bet is invalid");
            }
            try
            {
                var betNumber = int.Parse(this.BetPlace);
                if (betNumber < 0 || betNumber > 36) throw new MessageException("Bet placing error", "Bet number is out of range");
            }
            catch (FormatException exception)
            {
                if (!new BetRules().ColoursBet.Contains(this.BetPlace))
                {
                    throw new MessageException("Bet placing error", "Bet coulor is not valid");
                }
            }
            catch (MessageException exception)
            {
                throw exception;
            }
            return valid;
        }
    }
}
