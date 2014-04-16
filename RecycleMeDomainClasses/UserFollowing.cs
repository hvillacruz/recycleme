using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleMeDomainClasses
{
    public class UserFollowing : ILogInfo
    {

       

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(Order = 1)]
        public string FollowingId { get; set; }

        [Column(Order = 2)]
        public string FollowingUserId { get; set; }

        public virtual User Following { get; set; }

        public virtual User FollowingUser { get; set; }

        public DateTime ModifiedDate { get; set; }

    }
}
