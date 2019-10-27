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
    public class CalificationUserController : ControllerBase
    {
        private CalificationUserService calificationUserService = new CalificationUserService();
        private TokenService tokenService = new TokenService();
        private EventLogService eventLogService = new EventLogService();

        [HttpGet]
        [Route("findByIdUserAndIdUserCalification/{idUser}/{idUserCalification}")]
        [ProducesResponseType(200, Type = typeof(Stock))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult findByIdUserAndIdUserCalification([FromHeader(Name = "Authorization")]string token, int idUser, int idUserCalification)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return Ok(new
                    {
                        message = "El Token es requerido.",
                        statusCode = HttpStatusCode.NoContent
                    });
                }
                else
                {
                    string host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    Token tokenExisting = tokenService.findByToken(token, host);

                    if (tokenExisting != null)
                    {
                        CalificationUser calificationUser = calificationUserService.findByIdUserAndIdUserCalification(idUser, idUserCalification);

                        if (calificationUser != null)
                        {
                            EventLog eventLog = new EventLog();

                            eventLog.IdUser = tokenExisting.IdUser;
                            eventLog.IdEventLogType = 1;
                            eventLog.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                            eventLog.HttpMethod = ControllerContext.HttpContext.Request.Method;
                            eventLog.Controller = ControllerContext.ActionDescriptor.ControllerName;
                            eventLog.Method = ControllerContext.ActionDescriptor.ActionName;

                            eventLogService.create(eventLog);

                            return Ok(new
                            {
                                calificationUser = calificationUser,
                                statusCode = HttpStatusCode.OK
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                message = "El Stock no existe.",
                                statusCode = HttpStatusCode.NotFound
                            });
                        }
                    }
                    else
                    {
                        return Ok(new
                        {
                            message = "Token no permitido.",
                            statusCode = HttpStatusCode.Forbidden
                        });
                    }
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

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(201, Type = typeof(string))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult create([FromHeader(Name = "Authorization")]string token, [FromBody]CalificationUser calificationUser)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return Ok(new
                    {
                        message = "El Token es requerido.",
                        statusCode = HttpStatusCode.NoContent
                    });
                }
                else
                {
                    string host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    Token tokenExisting = tokenService.findByToken(token, host);

                    if (tokenExisting != null)
                    {
                        if (string.IsNullOrEmpty(calificationUser.IdUser.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id del Usuario es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(calificationUser.IdUserCalification.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id del Usuario Clasificador es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(calificationUser.Calification.ToString()))
                        {
                            return Ok(new
                            {
                                message = "La Clasificación es requerida.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else
                        {
                            calificationUser = calificationUserService.create(calificationUser);

                            if (calificationUser.Id != 0)
                            {
                                EventLog eventLog = new EventLog();

                                eventLog.IdUser = tokenExisting.IdUser;
                                eventLog.IdEventLogType = 1;
                                eventLog.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                                eventLog.HttpMethod = ControllerContext.HttpContext.Request.Method;
                                eventLog.Controller = ControllerContext.ActionDescriptor.ControllerName;
                                eventLog.Method = ControllerContext.ActionDescriptor.ActionName;

                                eventLogService.create(eventLog);

                                return Ok(new
                                {
                                    message = "Calificación Agregado.",
                                    statusCode = HttpStatusCode.Created
                                });
                            }
                            else
                            {
                                return Ok(new
                                {
                                    message = "La Calificación no se pudo agregar, intentalo nuevamente.",
                                    statusCode = HttpStatusCode.NotFound
                                });
                            }
                        }
                    }
                    else
                    {
                        return Ok(new
                        {
                            message = "Token no permitido.",
                            statusCode = HttpStatusCode.Forbidden
                        });
                    }
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

        [HttpDelete]
        [Route("destroyById/{id}")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult destroyById([FromHeader(Name = "Authorization")]string token, int id)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return Ok(new
                    {
                        message = "El Token es requerido.",
                        statusCode = HttpStatusCode.NoContent
                    });
                }
                else
                {
                    string host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    Token tokenExisting = tokenService.findByToken(token, host);

                    if (tokenExisting != null)
                    {
                        CalificationUser calificationUserExisting = calificationUserService.findById(id);

                        if (calificationUserExisting != null)
                        {
                            CalificationUser calificationUserDestroyied = calificationUserService.destroyById(id);

                            if (calificationUserDestroyied == null)
                            {
                                EventLog eventLog = new EventLog();

                                eventLog.IdUser = tokenExisting.IdUser;
                                eventLog.IdEventLogType = 1;
                                eventLog.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                                eventLog.HttpMethod = ControllerContext.HttpContext.Request.Method;
                                eventLog.Controller = ControllerContext.ActionDescriptor.ControllerName;
                                eventLog.Method = ControllerContext.ActionDescriptor.ActionName;

                                eventLogService.create(eventLog);

                                return Ok(new
                                {
                                    message = "Calificación eliminado.",
                                    statusCode = HttpStatusCode.OK
                                });
                            }
                            else
                            {
                                return Ok(new
                                {
                                    message = "La Calificación no se pudo eliminar, intentalo nuevamente.",
                                    statusCode = HttpStatusCode.NotFound
                                });
                            }
                        }
                        else
                        {
                            return Ok(new
                            {
                                message = "La Calificación no existe.",
                                statusCode = HttpStatusCode.NotFound
                            });
                        }
                    }
                    else
                    {
                        return Ok(new
                        {
                            message = "Token no permitido.",
                            statusCode = HttpStatusCode.Forbidden
                        });
                    }
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