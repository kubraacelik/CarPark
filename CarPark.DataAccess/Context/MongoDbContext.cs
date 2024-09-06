using CarPark.Core.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPark.DataAccess.Context
{
    // MongoDB veritabanıyla çalışmak için kullanılan bir context sınıfı
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoSettings> settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings), "MongoSettings cannot be null");
            }

            var mongoSettings = settings.Value;

            if (string.IsNullOrEmpty(mongoSettings.ConnectionString))
            {
                throw new ArgumentNullException(nameof(mongoSettings.ConnectionString), "ConnectionString cannot be null or empty");
            }

            if (string.IsNullOrEmpty(mongoSettings.Database))
            {
                throw new ArgumentNullException(nameof(mongoSettings.Database), "Database cannot be null or empty");
            }

            var client = new MongoClient(mongoSettings.ConnectionString);
            _database = client.GetDatabase(mongoSettings.Database);
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>()
        {
            return _database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public IMongoDatabase GetDatabase()
        {
            return _database;
        }
    }

}
