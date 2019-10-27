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
    public class StockReceivedController : ControllerBase
    {
        private StockReceivedService stockReceivedService = new StockReceivedService();
        private TokenService tokenService = new TokenService();
        private EventLogService eventLogService = new EventLogService();
        private IHubContext<StockReceivedHub> stockReceivedContext;

        public StockReceivedController(IHubContext<StockReceivedHub> hubContext)
        {
            stockReceivedContext = hubContext;
        }

        [HttpGet]
        [Route("findByIdUserBeneficiary/{idUserBeneficiary}")]
        [ProducesResponseType(200, Type = typeof(List<StockReceived>))]
        [ProducesResponseType(204, Type = typeof(List<StockReceived>))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult findByIdUserBeneficiary([FromHeader(Name = "Authorization")]string token, int idUserBeneficiary, string searcher = null)
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
                        List<StockReceived> stockReceiveds = stockReceivedService.findByIdUserBeneficiary(idUserBeneficiary, searcher);

                        if (stockReceiveds.Count > 0)
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
                                stockReceiveds = stockReceiveds,
                                countRows = stockReceiveds.Count,
                                statusCode = HttpStatusCode.OK
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                stockReceiveds = stockReceiveds,
                                countRows = stockReceiveds.Count,
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

        [HttpGet]
        [Route("findByIdStock/{idStock}")]
        [ProducesResponseType(200, Type = typeof(List<StockReceived>))]
        [ProducesResponseType(204, Type = typeof(List<StockReceived>))]
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
                        List<StockReceived> stockReceiveds = stockReceivedService.findByIdStock(idStock);

                        if (stockReceiveds.Count > 0)
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
                                stockReceiveds = stockReceiveds,
                                countRows = stockReceiveds.Count,
                                statusCode = HttpStatusCode.OK
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                stockReceiveds = stockReceiveds,
                                countRows = stockReceiveds.Count,
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
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(201, Type = typeof(string))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult create([FromHeader(Name = "Authorization")]string token, [FromBody]StockReceived stockReceived)
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
                        if (string.IsNullOrEmpty(stockReceived.IdStock.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id del Stock es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(stockReceived.IdUserBeneficiary.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id del Usuario Beneficiado es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(stockReceived.Quantity.ToString()))
                        {
                            return Ok(new
                            {
                                message = "La Cantidad es requerida.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else
                        {
                            stockReceived.Date = DateTime.Now;

                            StockReceived stockReceivedExisting = stockReceivedService.findByIdUserBeneficiaryAndIdStockAndDate(stockReceived.IdUserBeneficiary, stockReceived.IdStock, stockReceived.Date);

                            if (stockReceivedExisting != null)
                            {
                                stockReceived.Id = stockReceivedExisting.Id;

                                StockReceived stockReceivedUpdated = stockReceivedService.updateQuantityById(stockReceived);

                                if (stockReceivedUpdated != null)
                                {
                                    EventLog eventLog = new EventLog();

                                    eventLog.IdUser = tokenExisting.IdUser;
                                    eventLog.IdEventLogType = 1;
                                    eventLog.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                                    eventLog.HttpMethod = ControllerContext.HttpContext.Request.Method;
                                    eventLog.Controller = ControllerContext.ActionDescriptor.ControllerName;
                                    eventLog.Method = ControllerContext.ActionDescriptor.ActionName;

                                    eventLogService.create(eventLog);

                                    stockReceivedUpdated = stockReceivedService.findById(stockReceivedUpdated.Id);

                                    stockReceivedContext.Clients.All.SendAsync("create", stockReceivedUpdated);

                                    return Ok(new
                                    {
                                        message = "Stock Recibido Actualizado.",
                                        statusCode = HttpStatusCode.OK
                                    });
                                }
                                else
                                {
                                    return Ok(new
                                    {
                                        message = "El Stock Recibido no se pudo actualizar, intentalo nuevamente.",
                                        statusCode = HttpStatusCode.NotFound
                                    });
                                }
                            }
                            else
                            {
                                stockReceived = stockReceivedService.create(stockReceived);

                                if (stockReceived.Id != 0)
                                {
                                    EventLog eventLog = new EventLog();

                                    eventLog.IdUser = tokenExisting.IdUser;
                                    eventLog.IdEventLogType = 1;
                                    eventLog.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                                    eventLog.HttpMethod = ControllerContext.HttpContext.Request.Method;
                                    eventLog.Controller = ControllerContext.ActionDescriptor.ControllerName;
                                    eventLog.Method = ControllerContext.ActionDescriptor.ActionName;

                                    eventLogService.create(eventLog);

                                    stockReceived = stockReceivedService.findById(stockReceived.Id);

                                    stockReceivedContext.Clients.All.SendAsync("create", stockReceived);

                                    return Ok(new
                                    {
                                        message = "Stock Recibido Agregado.",
                                        statusCode = HttpStatusCode.Created
                                    });
                                }
                                else
                                {
                                    return Ok(new
                                    {
                                        message = "El Stock Recibido no se pudo agregar, intentalo nuevamente.",
                                        statusCode = HttpStatusCode.NotFound
                                    });
                                }
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

    }
}