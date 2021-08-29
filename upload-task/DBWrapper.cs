using System;
using MongoDB.Driver;
using upload_task.Models;

namespace upload_task
{
    public class DBWrapper
    {
        private readonly IMongoDatabase db;
        private Common cm = new Common();
        public DBWrapper()
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString("mongodb+srv://ivanbarayev:dobjdw7BMzdcKpJG@docdb.xmg8e.mongodb.net/upload_db?retryWrites=true&w=majority");
                var client = new MongoClient(settings);
                db = client.GetDatabase("upload_db");
                /*FileInfo leo = new FileInfo
                {
                    file_name = "leo",
                    file_mime = "jpg",
                    file_size = 5000,
                    file_path_src = "C:/",
                    file_path_dest = "D:/",
                    upload_date = DateTime.Now
                };*/
                SettingsModel settings_mod = new SettingsModel();

                var collection = db.GetCollection<Settings>("settings");
                cm.LogWriter(1, "SettingsModel.SettingsInsert", settings_mod.SettingsRead().ToString());
                //collection.InsertOne(settings_mod.SettingsRead());

            } catch(Exception ex)
            {
                cm.LogWriter(1, "DBWrapper", ex.Message);
            }
        }

        public void InsertFile(FileInfo data)
        {
            var collection = db.GetCollection<FileInfo>("settings");
            collection.InsertOne(data);
        }

    }
}