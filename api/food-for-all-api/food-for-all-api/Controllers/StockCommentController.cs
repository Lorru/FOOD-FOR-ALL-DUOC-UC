using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using food_for_all_api.Hubs;
using food_for_all_api.Models;
using food_for_all_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace food_for_all_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StockCommentController : ControllerBase
    {
        private StockCommentService stockCommentService = new StockCommentService();
        private TokenService tokenService = new TokenService();
        private EventLogService eventLogService = new EventLogService();
        private IHubContext<StockCommentHub> stockCommentHubContext;

        public StockCommentController(IHubContext<StockCommentHub> hubContext)
        {
            stockCommentHubContext = hubContext;
        }

        [HttpGet]
        [Route("findByIdStock/{idStock}")]
        [ProducesResponseType(200, Type = typeof(List<StockComment>))]
        [ProducesResponseType(204, Type = typeof(List<StockComment>))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult findByIdStock([FromHeader(Name = "Authorization")]string token, int idStock)
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
                        List<StockComment> stockComments = stockCommentService.findByIdStock(idStock);

                        if (stockComments.Count > 0)
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
                                stockComments = stockComments,
                                countRows = stockComments.Count,
                                statusCode = HttpStatusCode.OK
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                stockComments = stockComments,
                                countRows = stockComments.Count,
                                statusCode = HttpStatusCode.NoContent
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
        public IActionResult create([FromHeader(Name = "Authorization")]string token, [FromBody]StockComment stockComment)
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
                        if (string.IsNullOrEmpty(stockComment.IdUser.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id del Usuario es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(stockComment.IdStock.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id del Stock es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(stockComment.IdTypeMessage.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id del Tipo de Mensaje es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(stockComment.Comment))
                        {
                            return Ok(new
                            {
                                message = "El Comentario es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else
                        {
                            stockComment = stockCommentService.create(stockComment);

                            if (stockComment.Id != 0)
                            {
                                EventLog eventLog = new EventLog();

                                eventLog.IdUser = tokenExisting.IdUser;
                                eventLog.IdEventLogType = 1;
                                eventLog.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                                eventLog.HttpMethod = ControllerContext.HttpContext.Request.Method;
                                eventLog.Controller = ControllerContext.ActionDescriptor.ControllerName;
                                eventLog.Method = ControllerContext.ActionDescriptor.ActionName;

                                eventLogService.create(eventLog);

                                stockCommentHubContext.Clients.All.SendAsync("create", stockComment);

                                return Ok(new
                                {
                                    message = "Comentario Agregado.",
                                    statusCode = HttpStatusCode.Created
                                });
                            }
                            else
                            {
                                return Ok(new
                                {
                                    message = "El Comentario no se pudo agregar, intentalo nuevamente.",
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
                        StockComment stockCommentExisting = stockCommentService.findById(id);

                        if (stockCommentExisting != null)
                        {
                            StockComment stockCommentDeleted = stockCommentService.destroyById(id);

                            if (stockCommentDeleted == null)
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
                                    message = "Comentario eliminado.",
                                    statusCode = HttpStatusCode.OK
                                });
                            }
                            else
                            {
                                return Ok(new
                                {
                                    message = "El comentario no se pudo eliminar, intentalo nuevamente.",
                                    statusCode = HttpStatusCode.NotFound
                                });
                            }
                        }
                        else
                        {
                            return Ok(new
                            {
                                message = "El comentario no existe.",
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