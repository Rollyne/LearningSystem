using System.Collections.Generic;
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

        public void AddMessage(string message)
        {
            if (!TempData.ContainsKey("Messages"))
            {
                TempData.Add("Messages", new List<string>() { message });
            }
            else
            {
                var messages = TempData["Messages"] as List<string>;
                messages?.Add(message);
            }
        }

        public void AddError(string error)
        {
            if (!TempData.ContainsKey("Errors"))
            {
                TempData.Add("Errors", new List<string>() {error});
            }
            else
            {
                var errors = TempData["Errors"] as List<string>;
                errors?.Add(error);
            }
            
        }
    }
}