﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using RepositoryContracts.ServiceContracts;
using ServiceContracts;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IFinnhubRespository _finnhubRespository;

        public FinnhubService(IFinnhubRespository finnhubRespository)
        {
            _finnhubRespository = finnhubRespository;
        }

        public async Task<Dictionary<string, object?>?> GetStockPriceQuote(string? stockSymbol)
        {
            if (string.IsNullOrEmpty(stockSymbol))
            {
                return null;
            }

            Dictionary<string, object?>? priceQuote = await _finnhubRespository.GetStockPriceQuote(stockSymbol);
            if (priceQuote == null)
            {
                return null;
            }
            return priceQuote;

        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string? stockSymbol)
        {
            if (string.IsNullOrEmpty(stockSymbol))
            {
                return null;
            }


            Dictionary<string, object>? companyProfile = await _finnhubRespository.GetCompanyProfile(stockSymbol);
            if (companyProfile == null)
            {
                return null;
            }

            return companyProfile;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            var stocks = await _finnhubRespository.GetStocks();
            if (stocks != null) return stocks.ToList();
            return null;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return null;
            }

            Dictionary<string, object>? matchingStocks = await _finnhubRespository.SearchStocks(query);
            if (matchingStocks != null) return matchingStocks;
            return null;
        }
    }
}
