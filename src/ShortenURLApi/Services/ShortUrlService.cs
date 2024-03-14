namespace ShortenURLApi.Services
{
    using StackExchange.Redis;
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class ShortUrlService
    {
        private readonly IDatabase _redis;

        public ShortUrlService(IDatabase redis)
        {
            _redis = redis;
        }

        public string CreateShortUrl(string longUrl)
        {
            var shortCode = GenerateShortCode();
            _redis.StringSet(shortCode, longUrl, TimeSpan.FromDays(30)); // Set expiration to 30 days
            return shortCode;
        }

        public string? GetLongUrl(string shortCode)
        {
            return _redis.StringGet(shortCode);
        }

        private static string GenerateShortCode()
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
            return Convert.ToBase64String(hash)[..6].Replace('/', '_').Replace('+', '-');
        }
    }

}
