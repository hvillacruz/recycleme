using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleMeDomainClasses
{
    public class UserFollowing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string FollowingId { get; set; }

        [ForeignKey("FollowingId")]
        public virtual User User { get; set; }

        [ForeignKey("FollowingId")]
        public virtual User Following { get; set; }
    }
}
