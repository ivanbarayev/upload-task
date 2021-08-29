using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.IO;
using System.Net.Http.Headers;

namespace upload_task.Models
{
    public class UploadModel
    {
        private Common cm = new Common();
        private string current_path = Directory.GetCurrentDirectory();
        public readonly IMongoDatabase db;
        private static string collection_name = "uploads";
        private int END = S.SUCCESS;
        private readonly IMongoCollection<FileInfo> _upload;
        private string img_base64 = null;
        private string file_path_dest = null;
        private string generated_name = null;
        public UploadModel()
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString("mongodb+srv://ivanbarayev:dobjdw7BMzdcKpJG@docdb.xmg8e.mongodb.net/upload_db?retryWrites=true&w=majority");
                var client = new MongoClient(settings);
                db = client.GetDatabase("upload_db");
                _upload = db.GetCollection<FileInfo>(collection_name);
            }
            catch (Exception ex)
            {
                cm.LogWriter(S.SUCCESS, "UploadModel.Constructor", ex.Message);
            }
        }

        public int UploadInsertFS(HttpRequest req)
        {
            try
            {
                var file = req.Form.Files[0];
                cm.ChkDir2(SettingsENV.destination_path);
                file_path_dest = (SettingsENV.allow_rename == true ? Guid.NewGuid().ToString() + file.FileName.Split(".")[1] : null);
                var file_name = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fs_path = Path.Combine(SettingsENV.destination_path, (SettingsENV.allow_rename == true ? cm.FileRenamer(file_name) : file_name));
                generated_name = (SettingsENV.allow_rename == true ? cm.FileRenamer(file_name) : null);

                using (var stream = new FileStream(fs_path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                if (SettingsENV.store_type == "DB")
                {
                    generated_name = null;
                    byte[] b = File.ReadAllBytes(fs_path);
                    img_base64 = "data:image/png;base64," + Convert.ToBase64String(b);
                    File.Delete(fs_path);
                }

                FileInfo structer = new FileInfo
                {
                    store_type = SettingsENV.store_type,
                    file_name_org = file.FileName,
                    file_name_gen = generated_name,
                    file_mime = file.ContentType,
                    file_size = file.Length,
                    file_path_dest = (SettingsENV.store_type == "FS" ? SettingsENV.destination_path : null),
                    upload_date = DateTime.Now,
                    image = img_base64
                };

                UploadInsertDB(structer);
            }
            catch (Exception ex)
            {
                cm.LogWriter(S.ERROR, "SettingsModel.UploadInsertFS", ex.Message);
                END = S.ERROR;
            }
            return END;
        }

        public int UploadInsertDB(FileInfo data)
        {
            try
            {
                var collection = db.GetCollection<FileInfo>(collection_name);
                collection.InsertOne(data);
                END = S.SUCCESS;
            }
            catch (Exception ex)
            {
                cm.LogWriter(2, "UploadModel.UploadInsertDB", ex.Message);
                END = S.ERROR;
            }

            return END;
        }

        public FileInfo UploadList(ObjectId id)
        {
            return _upload.Find<FileInfo>(dat => dat._id == id).FirstOrDefault();
        }

        public FileInfo UploadView(string id)
        {
            return _upload.Find<FileInfo>(dat => dat._id == ObjectId.Parse(id)).FirstOrDefault();
        }

        public int UploadDelete(string id)
        {
            try
            {
                var resp = _upload.DeleteOne(a => a._id == ObjectId.Parse(id));
                END = int.Parse(resp.DeletedCount.ToString());
            }
            catch (Exception ex)
            {
                cm.LogWriter(S.ERROR, "UploadModel.UploadDelete", ex.Message);
                END = S.ERROR;
            }
            return END;
        }
    }
}
