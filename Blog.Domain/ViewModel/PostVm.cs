using Blog.Domain.Result;
using System;
using System.Collections.Generic;

namespace Blog.Domain.ViewModel
{
    public class PostVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public long ShowCounter { get; set; }
        public bool IsActive { get; set; }

        public List<IdNameStateResult> Tags { get; set; }
        public List<CommentVm> Comments { get; set; }
    }
}