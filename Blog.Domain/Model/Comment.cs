namespace Blog.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(300)]
        public string Content { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsApproved { get; set; }

        public bool IsActive { get; set; }

        public virtual Post Post { get; set; }
    }
}
