using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleMeDomainClasses
{
    public class Notification : ILogInfo
    {

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(Order = 1)]
        public string OwnerId { get; set; }

        public string Title { get; set; }

        public int Type { get; set; }

        public string UrlId { get; set; }

        public bool IsRead { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual User Owner { get; set; }
    }
}
