using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleMeDomainClasses
{
    public class Message
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(Order = 1)]
        public string SenderId { get; set; }

        [Column(Order = 2)]
        public string ReceiverId { get; set; }

        public virtual User Sender { get; set; }

        public virtual User Receiver { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime? DateSent { get; set; }

        public DateTime? DateReceived { get; set; }

        public bool IsDeleted { get; set; }

    }
}
