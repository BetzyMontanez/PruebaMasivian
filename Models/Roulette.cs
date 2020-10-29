using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace RouletteApi.Models
{
    public class Roulette
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdRoulette { get; set; }
        public bool Status { get; set; }
        public List<Bet> Bets { get; set; }
        public int WinnerNumber { get; set; }
    }
}
