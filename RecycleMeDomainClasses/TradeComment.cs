using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleMeDomainClasses
{
    public class TradeComment: ILogInfo
    {

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(Order = 1)]
        public string Comment { get; set; }

        [Column(Order = 2)]
        public Nullable<long> TradeId { get; set; }

        [Column(Order = 3)]
        public string TradeCommenterId { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual User TradeCommenter { get; set; }

        public virtual Trade TradeItemCommented { get; set; }

    }
}
