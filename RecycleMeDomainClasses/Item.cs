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
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        [Column(Order = 1)]
        public DateTime ModifiedDate { get; set; }
        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

    }
}
