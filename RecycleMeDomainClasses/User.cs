using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleMeDomainClasses
{
    public class User : ILogInfo
    {

        [Key]
        public string UserId { get; set; }
        public string ExternalId { get; set; }
        public string ExternalUserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Mobile { get; set; }
        public string Avatar { get; set; }
        public string ProfileStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastActivity { get; set; }
        public DateTime ModifiedDate { get; set; }


        public virtual ICollection<UserComment> UserCommenter { get; set; }
        public virtual ICollection<UserComment> UserCommented { get; set; }
        public virtual ICollection<UserFollower> UserFollowers { get; set; }
        public virtual ICollection<UserFollower> UserFollowerUsers { get; set; }
        public virtual ICollection<UserFollowing> UserFollowing { get; set; }
        public virtual ICollection<UserFollowing> UserFollowingUsers { get; set; }


    }


    public class UserViewModel
    {
        public string ExternalId { get; set; }
        public string ExternalUserName { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Mobile { get; set; }
        public string Avatar { get; set; }
        public string ProfileStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastActivity { get; set; }


        public ICollection<UserComment> UserComments { get; set; }
        public ICollection<UserFollower> Followers { get; set; }
        public ICollection<UserFollowing> UserFollowing { get; set; }

    }
}
