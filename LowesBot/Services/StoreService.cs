using System.Collections.Generic;
using LowesBot.Models;
using Microsoft.Bot.Connector;
namespace LowesBot.Services
{
    public static class StoreService
    {
        public static IEnumerable<StoreData> FindStores(Place place)
        {
            yield return new StoreData { Name = "Lowes Store #123", Distance = "1 mile" };
            yield return new StoreData { Name = "Lowes Store #234", Distance = "5 miles" };
            yield return new StoreData { Name = "Lowes Store #345", Distance = "10 miles" };
        }
    }
}
