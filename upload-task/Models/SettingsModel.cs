using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;

namespace upload_task.Models
{
    public class SettingsModel
    {
        private Common cm = new Common();
        public readonly IMongoDatabase db;
        private static string collection_name = "settings";
        private int END = 1;
        private readonly IMongoCollection<Settings> _sets;

        public SettingsModel()
        {
            try
            {
                ConventionRegistry.Register("Ignore null values", new ConventionPack { new IgnoreIfNullConvention(true) }, t => true);
                var settings = MongoClientSettings.FromConnectionString("mongodb+srv://ivanbarayev:dobjdw7BMzdcKpJG@docdb.xmg8e.mongodb.net/upload_db?retryWrites=true&w=majority");
                var client = new MongoClient(settings);
                db = client.GetDatabase("upload_db");
                _sets = db.GetCollection<Settings>(collection_name);
            }
            catch (Exception ex)
            {
                cm.LogWriter(S.ERROR, "SettingsModel.Constructor", ex.Message);
            }
        }

        public Settings SettingsRead()
        {
            return _sets.Find(dat => dat._id == 6453).FirstOrDefault();
        }

        public int SettingsInsert(Settings data)
        {
            try
            {
                var collection = db.GetCollection<Settings>(collection_name);
                collection.InsertOne(data);
            }
            catch (Exception ex)
            {
                cm.LogWriter(S.ERROR, "SettingsModel.SettingsInsert", ex.Message);
                END = 2;
            }

            return END;
        }

        public int SettingsUpdate(Settings data)
        {
            var collection = db.GetCollection<Settings>(collection_name);
            var filter = Builders<Settings>.Filter.Eq("_id", 6453);
            collection.ReplaceOne(filter, data);
            
            return END;
        }

    }
}
