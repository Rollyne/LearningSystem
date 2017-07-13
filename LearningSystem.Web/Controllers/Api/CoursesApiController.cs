using System.Web;
using System.Web.Http;
using LearningSystem.Data;
using LearningSystem.Models.ViewModels.Course;
using LearningSystem.Models.ViewModels.Filtering;
using LearningSystem.Services;
using LearningSystem.Web.Controllers.Api.Generic;
using Microsoft.AspNet.Identity;

namespace LearningSystem.Web.Controllers.Api
{
    [RoutePrefix("api/courses")]
    public class CoursesApiController : ServiceApiController<CoursesService<UnitOfWork>>
    {
        [Route("all/{page}")]
        [HttpGet]
        public IHttpActionResult GetAll(int page = 1, int itemsPerPage = 0)
        {
            var filter = new CourseFilterViewModel()
            {
                Page = page,
                ItemsPerPage = itemsPerPage < 1 ? ApplicationConstants.DefaultItemsPerPage : itemsPerPage
            };
            var execution = Service.GetAllFiltered(filter);
            if (execution.Succeded)
            {
                return Ok(execution.Result);
            }

            return BadRequest(execution.Message);
        }

        [HttpGet]
        public IHttpActionResult Get(string search = null, int page = 1, bool searchInDescription = false, bool searchInName = true, int itemsPerPage = 0)
        {
            var filter = new CourseFilterViewModel()
            {
                Page = page,
                Search = search,
                SearchInDescription = searchInDescription,
                SearchInName = searchInName,
                ItemsPerPage = itemsPerPage < 1 ? ApplicationConstants.DefaultItemsPerPage : itemsPerPage
            };
            var execution = Service.GetAllFiltered(filter);
            if (execution.Succeded)
            {
                return Ok(execution.Result);
            }

            return BadRequest(execution.Message);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var execution = Service.GetDetails(id, HttpContext.Current.User.Identity.GetUserId());

            if (execution.Succeded)
            {
                return Ok(execution.Result);
            }

            return BadRequest(execution.Message);
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] CourseModifyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var execution = Service.Create(model);

            if (execution.Succeded)
            {
                return Ok(execution.Message);
            }

            return BadRequest(execution.Message);
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(int id, [FromBody] CourseModifyViewModel model)
        {
            model.Id = id;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var execution = Service.Update(model);

            if (execution.Succeded)
            {
                return Ok(execution.Message);
            }

            return BadRequest(execution.Message);
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var execution = Service.Delete(id);

            if (execution.Succeded)
            {
                return Ok(execution.Message);
            }

            return BadRequest(execution.Message);
        }

        [HttpGet]
        [Route("{id}/students")]
        public IHttpActionResult GetStudents(int id)
        {
            var execution = Service.GetStudents(id);

            if (execution.Succeded)
            {
                return Ok(execution.Result);
            }
            
            return BadRequest(execution.Message);
        }

        [HttpPost]
        [Route("{id}/signUp")]
        public IHttpActionResult SignUp(int id)
        {
            var execution = Service.SignUpToCourse(id, HttpContext.Current.User.Identity.GetUserId());

            if (execution.Succeded)
            {
                return Ok(execution.Message);
            }

            return BadRequest(execution.Message);
        }

        [HttpPost]
        [Route("{id}/signOut")]
        public IHttpActionResult SignOut(int id)
        {
            var execution = Service.SignOutOfCourse(id, HttpContext.Current.User.Identity.GetUserId());

            if (execution.Succeded)
            {
                return Ok(execution.Message);
            }

            return BadRequest(execution.Message);
        }
    }
}
