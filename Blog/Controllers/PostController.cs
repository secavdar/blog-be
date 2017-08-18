using Blog.Business.Modules;
using Blog.Domain.ViewModel;
using System.Web.Http;

namespace Blog.Controllers
{
    public class PostController : ApiController
    {
        private readonly PostBusiness _postBusiness = new PostBusiness();

        [HttpGet]
        [Route("Posts/{id}")]
        public IHttpActionResult Get(int id)
        {
            var result = _postBusiness.Get(id);
            return Ok(result);
        }
        [HttpGet]
        [Route("Posts/Active")]
        public IHttpActionResult ActiveList()
        {
            var result = _postBusiness.ActiveList();
            return Ok(result);
        }
        [HttpGet]
        [Route("Posts")]
        public IHttpActionResult List()
        {
            var result = _postBusiness.List();
            return Ok(result);
        }
        [HttpPost]
        [Route("Posts")]
        public IHttpActionResult Create(PostVm vm)
        {
            var result = _postBusiness.Create(vm);
            return Ok(result);
        }
        [HttpPut]
        [Route("Posts/{id}")]
        public IHttpActionResult Update(int id, PostVm vm)
        {
            var result = _postBusiness.Update(id, vm);
            return Ok(result);
        }
        [HttpPut]
        [Route("Posts/State/{id}")]
        public IHttpActionResult ChangeState(int id)
        {
            var result = _postBusiness.ChangeState(id);
            return Ok(result);
        }
    }
}