using System;

namespace Blog.Domain.ViewModel
{
    public class CommentVm
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
    }
}