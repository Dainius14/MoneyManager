using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyManager.Models.Domain
{
    public class Category: IAudited
    {
        [Column]
        public int? ID { get; set; }

        [Column]
        public string Name { get; set; } = null!;

        [Column]
        public int? ParentID { get; set; }

        [Column]
        public DateTime CreatedAt { get; set; }

        [Column]
        public DateTime? UpdatedAt { get; set; }


        public Category? Parent { get; set; }

        public Category()
        {
        }

        public Category(int id, string name, int? parentId, DateTime createdAt)
        {
            ID = id;
            Name = name;
            ParentID = parentId;
            CreatedAt = createdAt;
    }
    }
}