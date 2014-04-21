using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleMeDomainClasses
{
    public class Item : ILogInfo
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Column(Order = 1)]
        public string OwnerId { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public string TradeTag { get; set; }
        public string ExchangeTag { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual User Owner { get; set; }
        
        [ForeignKey("Category")]
        public int ItemCategoryId { get; set; }
        public virtual ItemCategory Category { get; set; }


        public virtual ICollection<ItemImage> ItemImages { get; set; }
        public virtual ICollection<ItemComment> ItemCommented { get; set; }
        public virtual ICollection<ItemFollowers> ItemUserFollowers { get; set; }

    }
}
