using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleMeDomainClasses
{
    public class Trade : ILogInfo
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        [Column(Order = 1)]
        public Nullable<long> ItemId { get; set; }
        
        
        [Column(Order = 2)]
        public string BuyerId { get; set; }
        
        [Column(Order = 3)]
        public string SellerId { get; set; }

        public virtual Item Item { get; set; }
        
        public virtual User Buyer { get; set; }

        public virtual User Seller { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Status { get; set; }

        public virtual ICollection<TradeBuyerItem> Trades { get; set; }

        public virtual ICollection<TradeComment> TradeItem { get; set; }

    }
}
