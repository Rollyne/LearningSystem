using System.Web.Http;

namespace LearningSystem.Web.Controllers.Api.Generic
{
    public class ServiceApiController<TService> : ApiController
        where TService : new()
    {
        protected readonly TService Service;
        public ServiceApiController(TService service)
        {
            this.Service = service;
        }

        public ServiceApiController()
        {
            this.Service = new TService();
        }
    }
}
