﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleMeDomainClasses
{
    public class ItemFollowers : ILogInfo
    {

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(Order = 1)]
        public string FollowerId { get; set; }

        [Column(Order = 2)]
        public long FollowedItemId { get; set; }

        public virtual User Follower { get; set; }

        public virtual Item FollowedItem { get; set; }

        public DateTime ModifiedDate { get; set; }

    }
}
