using Blog.Business.Base;
using Blog.Domain.Model;
using Blog.Domain.Result;
using Blog.Domain.Type;
using Blog.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Blog.Business.Modules
{
    public class PostBusiness
    {
        private readonly UnitOfWork _uow = new UnitOfWork();

        public PostVm Get(int id)
        {
            var entity = _uow.Call<Post>()
                             .Query(x => x.Id == id)
                             .Include(x => x.Tags)
                             .Include(x => x.Comments)
                             .SingleOrDefault();

            entity.ShowCounter++;

            _uow.Call<Post>().Update(entity);
            _uow.Commit();

            return new PostVm
            {
                Id = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
                CreateDate = entity.CreateDate,
                ShowCounter = entity.ShowCounter,
                Tags = entity.Tags
                             .Select(x => new IdNameStateResult
                             {
                                 Id = x.Id,
                                 Name = x.Name,
                                 State = ItemChangeState.Unchanged
                             })
                             .ToList(),
                Comments = entity.Comments
                                 .Where(x => x.IsActive && x.IsApproved)
                                 .Select(x => new CommentVm
                                 {
                                     Id = x.Id,
                                     PostId = x.PostId,
                                     UserName = x.UserName,
                                     Email = x.Email,
                                     Content = x.Content,
                                     CreateDate = x.CreateDate
                                 })
                                 .ToList()
            };
        }
        public List<PostVm> ActiveList()
        {
            return _uow.Call<Post>()
                       .Query(x => x.IsActive)
                       .Include(x => x.Tags)
                       .Include(x => x.Comments)
                       .OrderBy(x => x.CreateDate)
                       .Select(x => new PostVm
                       {
                           Id = x.Id,
                           Title = x.Title,
                           Content = x.Content,
                           CreateDate = x.CreateDate,
                           ShowCounter = x.ShowCounter,
                           Tags = x.Tags
                                   .Select(y => new IdNameStateResult
                                   {
                                       Id = y.Id,
                                       Name = y.Name,
                                       State = ItemChangeState.Unchanged
                                   })
                                   .ToList(),
                           Comments = x.Comments
                                       .Where(y => y.IsActive && y.IsApproved)
                                       .Select(y => new CommentVm
                                       {
                                           Id = y.Id,
                                           PostId = y.PostId,
                                           UserName = y.UserName,
                                           Email = y.Email,
                                           Content = y.Content,
                                           CreateDate = y.CreateDate
                                       })
                                       .ToList()
                       })
                       .ToList();
        }
        public List<PostVm> List()
        {
            return _uow.Call<Post>()
                       .All()
                       .OrderBy(x => x.CreateDate)
                       .Select(x => new PostVm
                       {
                           Id = x.Id,
                           Title = x.Title,
                           Content = x.Content,
                           CreateDate = x.CreateDate,
                           ShowCounter = x.ShowCounter,
                           IsActive = x.IsActive,
                           Tags = x.Tags
                                   .Select(y => new IdNameStateResult
                                   {
                                       Id = y.Id,
                                       Name = y.Name,
                                       State = ItemChangeState.Unchanged
                                   })
                                   .ToList()
                       })
                       .ToList();
        }
        public bool Create(PostVm vm)
        {
            _uow.Begin();

            var entity = new Post
            {
                Title = vm.Title,
                Content = vm.Content,
                CreateDate = DateTime.Now,
                ShowCounter = 0,
                IsActive = true
            };

            foreach (var item in vm.Tags)
            {
                var tag = _uow.Call<Tag>().Find(item.Id);
                entity.Tags.Add(tag);
            }

            _uow.Call<Post>().Add(entity);
            return _uow.Commit();
        }
        public bool Update(int id, PostVm vm)
        {
            var entity = _uow.Call<Post>()
                             .Query(x => x.Id == id)
                             .Include(x => x.Tags)
                             .SingleOrDefault();

            entity.Title = vm.Title;
            entity.Content = vm.Content;

            foreach (var item in vm.Tags)
            {
                var tag = _uow.Call<Tag>().Find(item.Id);
                switch (item.State)
                {
                    case ItemChangeState.Added:
                        _uow.Call<Post>().AddRelationship(entity, x => x.Tags, tag);
                        break;
                    case ItemChangeState.Deleted:
                        _uow.Call<Post>().RemoveRelationship(entity, x => x.Tags, tag);
                        break;
                }
            }

            _uow.Call<Post>().Update(entity);
            return _uow.Commit();
        }
        public bool ChangeState(int id)
        {
            var entity = _uow.Call<Post>().Find(id);
            entity.IsActive = !entity.IsActive;

            _uow.Call<Post>().Update(entity);
            return _uow.Commit();
        }
    }
}