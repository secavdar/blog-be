using Blog.Business.Base;
using Blog.Domain.Model;
using Blog.Domain.Result;
using Blog.Domain.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Business.Modules
{
    public class TagBusiness
    {
        private readonly UnitOfWork _uow = new UnitOfWork();

        public List<IdNameResult> BasicList()
        {
            return _uow.Call<Tag>()
                       .Query(x => x.IsActive)
                       .Select(x => new IdNameResult
                       {
                           Id = x.Id,
                           Name = x.Name
                       })
                       .OrderBy(x => x.Name)
                       .ToList();
        }
        public List<TagVm> List()
        {
            return _uow.Call<Tag>()
                       .All()
                       .Select(x => new TagVm
                       {
                           Id = x.Id,
                           Name = x.Name,
                           IsActive = x.IsActive
                       })
                       .ToList();
        }
        public bool Create(TagVm vm)
        {
            _uow.Call<Tag>()
                .Add(new Tag
                {
                    Name = vm.Name,
                    IsActive = true
                });
            return _uow.Commit();
        }
        public bool Update(int id, TagVm vm)
        {
            var entity = _uow.Call<Tag>().Find(id);

            entity.Name = vm.Name;

            _uow.Call<Tag>().Update(entity);
            return _uow.Commit();
        }
        public bool ChangeState(int id)
        {
            var entity = _uow.Call<Tag>().Find(id);
            entity.IsActive = !entity.IsActive;

            _uow.Call<Tag>().Update(entity);
            return _uow.Commit();
        }
    }
}