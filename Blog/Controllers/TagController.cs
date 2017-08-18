using Blog.Business.Modules;
using Blog.Domain.ViewModel;
using System.Web.Http;

namespace Blog.Controllers
{
    public class TagController : ApiController
    {
        private readonly TagBusiness _tagBusiness = new TagBusiness();

        [HttpGet]
        [Route("Tags/Basic")]
        public IHttpActionResult BasicList()
        {
            var result = _tagBusiness.BasicList();
            return Ok(result);
        }
        [HttpGet]
        [Route("Tags")]
        public IHttpActionResult List()
        {
            var result = _tagBusiness.List();
            return Ok(result);
        }
        [HttpPost]
        [Route("Tags")]
        public IHttpActionResult Create(TagVm vm)
        {
            var result = _tagBusiness.Create(vm);
            return Ok(result);
        }
        [HttpPut]
        [Route("Tags/{id}")]
        public IHttpActionResult Update(int id, TagVm vm)
        {
            var result = _tagBusiness.Update(id, vm);
            return Ok(result);
        }
        [HttpPut]
        [Route("Tags/State/{id}")]
        public IHttpActionResult ChangeState(int id)
        {
            var result = _tagBusiness.ChangeState(id);
            return Ok(result);
        }
    }
}