using System.Web.Mvc;

namespace LearningSystem.Web.Controllers.Generic
{
    public class ServiceController<TService> : Controller
        where TService : new()
    {
        protected readonly TService Service;
        public ServiceController(TService service)
        {
            this.Service = service;
        }

        public ServiceController()
        {
            this.Service = new TService();
        }
    }
}