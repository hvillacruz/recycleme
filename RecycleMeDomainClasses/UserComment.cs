using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleMeDomainClasses
{
    public class UserComment : ILogInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Comment { get; set; }

        [Required]
        public string UserCommenterId { get; set; }

        [ForeignKey("UserCommenterId")]
        public virtual User User { get; set; }

        [ForeignKey("UserCommenterId")]
        public virtual User Commenter { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifiedDate { get; set; }


    }
}
