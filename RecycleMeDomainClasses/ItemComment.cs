using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleMeDomainClasses
{
    public class ItemComment : ILogInfo
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(Order = 1)]
        public string CommenterId { get; set; }

        [Column(Order = 2)]
        public long CommentedItemId { get; set; }

        public virtual User Commenter { get; set; }

        public virtual Item CommentedItem { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Comment { get; set; }

        public bool IsDeleted { get; set; }




    }
}
