using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleMeDomainClasses
{
    public class UserFollower : ILogInfo
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column(Order = 1)]
        public string FollowerId { get; set; }

        [Column(Order = 2)]
        public string FollowedUserId { get; set; }

        public virtual User Follower { get; set; }

        public virtual User FollowedUser { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
