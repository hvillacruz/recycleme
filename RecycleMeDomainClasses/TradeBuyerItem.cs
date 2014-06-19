using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleMeDomainClasses
{
    public class TradeBuyerItem :ILogInfo
    {

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(Order = 1)]
        public Nullable<long> ItemId { get; set; }

        [Column(Order = 2)]
        public long TradeId { get; set; }

        public virtual Item Item { get; set; }

        public virtual Trade Trade { get; set; }
    }
}
