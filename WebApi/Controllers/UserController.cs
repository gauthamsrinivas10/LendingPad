﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessEntities;
using Core.Services.Users;
using WebApi.Models.Users;

namespace WebApi.Controllers
{
    [RoutePrefix("users")]
    public class UserController : BaseApiController
    {
        private readonly ICreateUserService _createUserService;
        private readonly IDeleteUserService _deleteUserService;
        private readonly IGetUserService _getUserService;
        private readonly IUpdateUserService _updateUserService;

        public UserController(ICreateUserService createUserService, IDeleteUserService deleteUserService, IGetUserService getUserService, IUpdateUserService updateUserService)
        {
            _createUserService = createUserService;
            _deleteUserService = deleteUserService;
            _getUserService = getUserService;
            _updateUserService = updateUserService;
        }

        [Route("{userId:guid}/create")]
        [HttpPost]
        public HttpResponseMessage CreateUser(Guid userId, [FromBody] UserModel model)
        {
            try
            {
                var user = _createUserService.Create(userId, model.Name, model.Email, model.Type, model.AnnualSalary, model.Tags);
                return Found(new UserData(user));
            }
            catch (Exception)
            {                
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while creating the user. Review the input data");
            }
        }

        [Route("{userId:guid}/update")]
        [HttpPut]
        public HttpResponseMessage UpdateUser(Guid userId, [FromBody] UserModel model)
        {
            var user = _getUserService.GetUser(userId);
            if (user == null)
            {
                return DoesNotExist();
            } 
            try
            {
                
                _updateUserService.Update(user, model.Name, model.Email, model.Type, model.AnnualSalary, model.Tags);
                return Found(new UserData(user));
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while updating the user. Review the input data");
            }
        }

        [Route("{userId:guid}/delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteUser(Guid userId)
        {
            var user = _getUserService.GetUser(userId);
            if (user == null)
            {
                return DoesNotExist();
            }
            _deleteUserService.Delete(user);
            return Found();
        }

        [Route("{userId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetUser(Guid userId)
        {
            var user = _getUserService.GetUser(userId);
            return Found(new UserData(user));
        }

        [Route("list")]
        [HttpGet]
        public HttpResponseMessage GetUsers(int skip, int take, UserTypes? type = null, string name = null, string email = null)
        {
            var users = _getUserService.GetUsers(type, name, email)
                                       .Skip(skip).Take(take)
                                       .Select(q => new UserData(q))
                                       .ToList();
            return Found(users);
        }

        [Route("clear")]
        [HttpDelete]
        public HttpResponseMessage DeleteAllUsers()
        {
            _deleteUserService.DeleteAll();
            return Found();
        }

        [Route("list/tag")]
        [HttpGet]
        public HttpResponseMessage GetUsersByTag(string tag)
        {
            try
            {
                var users = string.IsNullOrEmpty(tag) ?
                    _getUserService.GetUsersByTag(string.Empty) :
                    _getUserService.GetUsersByTag(tag);

                            
                var userDataList = users.Select(q => new UserData(q)).ToList(); 
                return Found(userDataList); 
            }
            catch (Exception)
            {                
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while fetching users by tag.");
            }
        }
    }
}