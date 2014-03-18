using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleMeDomainClasses
{
    public class UserFollower
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string FollowerId { get; set; }

        [ForeignKey("FollowerId")]
        public virtual User User { get; set; }

        [ForeignKey("FollowerId")]
        public virtual User Follower { get; set; }
    }
}
