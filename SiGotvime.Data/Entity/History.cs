using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Entity
{
    public class History
    {
        public int ID { get; set; }
        public int EntityID { get; set; }
        public DateTime DateCreated { get; set; }
        public HistoryType Type { get; set; }
    }

    public enum HistoryType
    {
        Recipe,
        User
    }
}
