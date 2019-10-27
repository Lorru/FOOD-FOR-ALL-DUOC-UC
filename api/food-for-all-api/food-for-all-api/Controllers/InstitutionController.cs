using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using food_for_all_api.Models;
using food_for_all_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace food_for_all_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class InstitutionController : ControllerBase
    {
        private InstitutionService institutionService = new InstitutionService();
        private EventLogService eventLogService = new EventLogService();

        [HttpGet]
        [Route("findAll")]
        [ProducesResponseType(200, Type = typeof(List<Institution>))]
        [ProducesResponseType(204, Type = typeof(List<Institution>))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult findAll(string searcher = null)
        {
            try
            {
                List<Institution> institutions = institutionService.findAll(searcher);

                if (institutions.Count > 0)
                {

                    EventLog eventLog = new EventLog();

                    eventLog.IdEventLogType = 1;
                    eventLog.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    eventLog.HttpMethod = ControllerContext.HttpContext.Request.Method;
                    eventLog.Controller = ControllerContext.ActionDescriptor.ControllerName;
                    eventLog.Method = ControllerContext.ActionDescriptor.ActionName;

                    eventLogService.create(eventLog);

                    return Ok(new
                    {
                        institutions = institutions,
                        countRows = institutions.Count,
                        statusCode = HttpStatusCode.OK
                    });
                }
                else
                {
                    return Ok(new
                    {
                        institutions = institutions,
                        countRows = institutions.Count,
                        statusCode = HttpStatusCode.NoContent
                    });
                }
            }
            catch (Exception exception)
            {
                EventLog eventLog = new EventLog();

                eventLog.IdEventLogType = 2;
                eventLog.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                eventLog.HttpMethod = ControllerContext.HttpContext.Request.Method;
                eventLog.Controller = ControllerContext.ActionDescriptor.ControllerName;
                eventLog.Method = ControllerContext.ActionDescriptor.ActionName;
                eventLog.Message = exception.InnerException != null ? exception.InnerException.Message : exception.Message;

                eventLogService.create(eventLog);

                return Ok(new
                {
                    message = "Upps!!, tenemos un problema, intentalo nuevamente.",
                    statusCode = HttpStatusCode.InternalServerError
                });
            }
        }
    }
}