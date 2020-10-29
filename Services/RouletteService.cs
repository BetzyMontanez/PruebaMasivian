using MongoDB.Bson;
using MongoDB.Driver;
using RouletteApi.Exceptions;
using RouletteApi.Helpers;
using RouletteApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RouletteApi.Services
{
    public class RouletteService
    {
        private readonly IMongoCollection<Roulette> _Roulettes;

        public RouletteService(IRouletteDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _Roulettes = database.GetCollection<Roulette>(settings.RoulettesCollectionName);
        }

        public async Task<CustomResponse<string>> CreateRoulette()
        {
            CustomResponse<string> createResponse;
            Roulette roulette = new Roulette();
            try
            {
                roulette.Status = false;
                _Roulettes.InsertOne(roulette);
                createResponse = new CustomResponse<string>((int)HttpStatusCode.Created, "Roulette Created", "IdRoulette", roulette.IdRoulette);
            }
            catch (Exception e)
            {
                throw new MessageException("Roulette creation error", e.Message);
            }
            return createResponse;
        }

        public async Task<Roulette> GetRoulette(string id)
        {
            Roulette roulette = _Roulettes.Find<Roulette>(roulette => roulette.IdRoulette == id).FirstOrDefault();
            if (roulette == null) throw new MessageException("Roulette updating error", "Roulette doesn't exist");
            return roulette;
        }

        public async Task<CustomResponse<string>> UpdateStatus(string id, bool status)
        {
            CustomResponse<string> createResponse;
            try
            {
                Roulette roulette = await GetRoulette(id);
                if (roulette.Status == status)
                {
                    var state = status ? "Opened" : "Closed";
                    throw new MessageException("Roulette updating error", $"Roulette is already {state}");
                }
                var filter = Builders<Roulette>.Filter.Eq("IdRoulette", id);
                var update = Builders<Roulette>.Update.Set("Status", status);
                var resp = _Roulettes.UpdateOne(filter, update);
                createResponse = new CustomResponse<string>((int)HttpStatusCode.OK, "Roulette Updated", "Status", status ? "Open" : "Closed");

                return createResponse;
            }
            catch (Exception e)
            {
                throw new MessageException("Roulette updating error", e.Message);
            }
        }

        public async Task<bool> CleanBets(string id)
        {
            try
            {
                var filter = Builders<Roulette>.Filter.Eq("IdRoulette", id);
                var update = Builders<Roulette>.Update.Set("Bets", BsonNull.Value);
                var resp = _Roulettes.UpdateOne(filter, update);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<CustomResponse<Bet>> PlaceBet(string id, string betPlace, double amount, string userId)
        {
            CustomResponse<Bet> createResponse;
            try
            {
                Bet bet = new Bet(betPlace, amount, userId);
                List<Bet> bets = new List<Bet>();
                Roulette roulette = await GetRoulette(id);
                if (!roulette.Status) throw new MessageException("Bet placing error", "Roulette is closed");
                if (roulette.Bets != null) bets = roulette.Bets;
                if (!bet.IsValid()) throw new MessageException("Bet placing error", "Bet validation error");
                bets.Add(bet);
                var filter = Builders<Roulette>.Filter.Eq("IdRoulette", id);
                var update = Builders<Roulette>.Update.Set("Bets", bets);
                var resp = _Roulettes.UpdateOne(filter, update);
                createResponse = new CustomResponse<Bet>((int)HttpStatusCode.OK, "Roulette Updated", "New Bet", bet);
                return createResponse;
            }
            catch (MessageException mException)
            {
                throw mException;
            }
            catch (Exception e)
            {
                throw new MessageException("Roulette updating error", e.Message);
            }
        }

        public async Task<CustomResponse<List<Bet>>> CloseBets(string id)
        {
            CustomResponse<List<Bet>> createResponse;
            try
            {
                List<Bet> bets = new List<Bet>();
                Roulette roulette = await GetRoulette(id);
                if (!roulette.Status) throw new MessageException("Bets closing error", "Roulette is already closed");
                if (roulette.Bets != null) bets = roulette.Bets;
                Random randomWinner = new Random();
                int winnerNumber = randomWinner.Next(BetRules.MinNumberBet, BetRules.MaxNumberBet);
                string winnerColour = (winnerNumber % 2 == 0) ? "Red" : "Black";
                bets.ForEach(bet => bet.EarnedAmount = bet.EarnedAmount + ((bet.BetPlace == winnerNumber.ToString()) ? bet.Amount * 5 : ((bet.BetPlace == winnerColour) ? bet.Amount * 1.8 : 0)));
                var filter = Builders<Roulette>.Filter.Eq("IdRoulette", id);
                var update = Builders<Roulette>.Update.Set("Bets", bets).Set("Status", false).Set("WinnerNumber", winnerNumber);
                var resp = _Roulettes.UpdateOne(filter, update);
                createResponse = new CustomResponse<List<Bet>>((int)HttpStatusCode.OK, "Roulette Closed", "Bets Summary", bets);
                return createResponse;
            }
            catch (MessageException mException)
            {
                throw mException;
            }
            catch (Exception e)
            {
                throw new MessageException("Roulette closing error", e.Message);
            }
        }

        public List<Roulette> GetRoulettes() =>
            _Roulettes.Find(roulette => true).ToList();

    }
}
