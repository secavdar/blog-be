using Blog.Domain.Type;

namespace Blog.Domain.Result
{
    public class IdNameStateResult : IdNameResult
    {
        public ItemChangeState State { get; set; }
    }
}