using SQLite;
using System;

namespace MVVM.Models
{
    public class Poetry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; } = String.Empty;
    }
}
