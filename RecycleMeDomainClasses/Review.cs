using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecycleMeDomainClasses
{
    [Table("Review")]
    public class Review : ILogInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ReviewId { get; set; }
        public string ReviewtText { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual User User { get; set; }
    }
}
