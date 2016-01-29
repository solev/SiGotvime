using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Repository
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly FoodDatabase db;

        public SettingsRepository(FoodDatabase _db)
        {
            db = _db;
        }

        public string GetSettingWithKey(string key)
        {
            string result = db.Settings.Where(x => x.SettingKey == key).Select(x => x.SettingValue).FirstOrDefault();
            return result;
        }
        
        public List<Settings> GetSettingsInKeys(List<string> keys)
        {
            var result = db.Settings.Where(x => keys.Contains(x.SettingKey)).Select(x => new { Key = x.SettingKey, Value = x.SettingValue }).ToList();

            return result.Select(x => new Settings { SettingKey = x.Key, SettingValue = x.Value }).ToList();
        }
    }
}
