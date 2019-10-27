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
    public class StockController : ControllerBase
    {
        private StockService stockService = new StockService();
        private TokenService tokenService = new TokenService();
        private EventLogService eventLogService = new EventLogService();

        [HttpGet]
        [Route("findByIdUser/{idUser}")]
        [ProducesResponseType(200, Type = typeof(List<Stock>))]
        [ProducesResponseType(204, Type = typeof(List<Stock>))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult findByIdUser([FromHeader(Name = "Authorization")]string token, int idUser, string searcher = null)
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
                        List<Stock> stocks = stockService.findByIdUser(idUser, searcher);

                        if (stocks.Count > 0)
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
                                stocks = stocks,
                                countRows = stocks.Count,
                                statusCode = HttpStatusCode.OK
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                stocks = stocks,
                                countRows = stocks.Count,
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
        [Route("findByIdUserAndAvailable/{idUser}")]
        [ProducesResponseType(200, Type = typeof(List<Stock>))]
        [ProducesResponseType(204, Type = typeof(List<Stock>))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult findByIdUserAndAvailable([FromHeader(Name = "Authorization")]string token, int idUser, string searcher = null)
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
                        List<Stock> stocks = stockService.findByIdUserAndAvailable(idUser, searcher);

                        if (stocks.Count > 0)
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
                                stocks = stocks,
                                countRows = stocks.Count,
                                statusCode = HttpStatusCode.OK
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                stocks = stocks,
                                countRows = stocks.Count,
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
        [Route("findAllAvailable")]
        [ProducesResponseType(200, Type = typeof(List<Stock>))]
        [ProducesResponseType(204, Type = typeof(List<Stock>))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult findAllAvailable([FromHeader(Name = "Authorization")]string token, string searcher = null)
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
                        List<Stock> stocks = stockService.findAllAvailable(searcher);

                        if (stocks.Count > 0)
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
                                stocks = stocks,
                                countRows = stocks.Count,
                                statusCode = HttpStatusCode.OK
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                stocks = stocks,
                                countRows = stocks.Count,
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
        [Route("findByIdUserAndFilterDynamic/{idUser}/{property}/{condition}/{value}")]
        [ProducesResponseType(200, Type = typeof(List<Stock>))]
        [ProducesResponseType(204, Type = typeof(List<Stock>))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult findByIdUserAndFilterDynamic([FromHeader(Name = "Authorization")]string token, int idUser, string property, string condition, string value, string searcher = null)
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
                        List<Stock> stocks = stockService.findByIdUserAndFilterDynamic(idUser, searcher, property, condition, value);

                        if (stocks.Count > 0)
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
                                stocks = stocks,
                                countRows = stocks.Count,
                                statusCode = HttpStatusCode.OK
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                stocks = stocks,
                                countRows = stocks.Count,
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
        [Route("findById/{id}")]
        [ProducesResponseType(200, Type = typeof(Stock))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult findById([FromHeader(Name = "Authorization")]string token, int id)
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
                        Stock stock = stockService.findById(id);

                        if (stock != null)
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
                                stock = stock,
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

        [HttpGet]
        [Route("findByIdAndAvailable/{id}")]
        [ProducesResponseType(200, Type = typeof(Stock))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult findByIdAndAvailable([FromHeader(Name = "Authorization")]string token, int id)
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
                        Stock stock = stockService.findByIdAndAvailable(id);

                        if (stock != null)
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
                                stock = stock,
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
        [ProducesResponseType(409, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult create([FromHeader(Name = "Authorization")]string token, [FromBody]Stock stock)
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
                        if (string.IsNullOrEmpty(stock.IdUser.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id del usuario es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(stock.IdProduct.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id del producto es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(stock.Quantity.ToString()))
                        {
                            return Ok(new
                            {
                                message = "La cantidad es requerida.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else
                        {
                            Stock stockExisting = stockService.findByIdUserAndIdProduct(stock.IdUser, stock.IdProduct);

                            if (stockExisting == null)
                            {
                                stock = stockService.create(stock);

                                if (stock.Id != 0)
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
                                        message = "Stock creado.",
                                        statusCode = HttpStatusCode.Created
                                    });
                                }
                                else
                                {
                                    return Ok(new
                                    {
                                        message = "El stock no se pudo crear, intentalo nuevamente.",
                                        statusCode = HttpStatusCode.NotFound
                                    });
                                }
                            }
                            else
                            {
                                return Ok(new
                                {
                                    message = "El Stock ya existe.",
                                    statusCode = HttpStatusCode.Conflict
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

        [HttpPut]
        [Route("updateMassive")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult updateMassive([FromHeader(Name = "Authorization")]string token, [FromBody]List<Stock> stocks)
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
                        if (string.IsNullOrEmpty(stocks[0].Id.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(stocks[0].IdUser.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id del usuario es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(stocks[0].IdProduct.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id del producto es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(stocks[0].Quantity.ToString()))
                        {
                            return Ok(new
                            {
                                message = "La cantidad es requerida.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else
                        {
                            foreach (Stock stock in stocks)
                            {
                                if (stockService.findById(stock.Id) != null)
                                {
                                    stockService.updateById(stock);
                                }
                            }

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
                                message = "Stocks actualizados.",
                                statusCode = HttpStatusCode.OK
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

        [HttpPut]
        [Route("updateById")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(409, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult updateById([FromHeader(Name = "Authorization")]string token, [FromBody]Stock stock)
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
                        if (string.IsNullOrEmpty(stock.Id.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(stock.IdUser.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id del usuario es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(stock.IdProduct.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id del producto es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(stock.Quantity.ToString()))
                        {
                            return Ok(new
                            {
                                message = "La cantidad es requerida.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else
                        {
                            Stock stockExisting = stockService.findById(stock.Id);

                            if (stockExisting != null)
                            {
                                if (stockService.findByIdUserAndIdProduct(stock.IdUser, stock.IdProduct) == null || stockService.findByIdUserAndIdProduct(stock.IdUser, stock.IdProduct).Id == stock.Id)
                                {
                                    Stock stockUpdated = stockService.updateById(stock);

                                    if (stockUpdated != null)
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
                                            message = "Stock actualizado.",
                                            statusCode = HttpStatusCode.OK
                                        });
                                    }
                                    else
                                    {
                                        return Ok(new
                                        {
                                            message = "El stock no se pudo actualizar, intentalo nuevamente.",
                                            statusCode = HttpStatusCode.NotFound
                                        });
                                    }
                                }
                                else
                                {
                                    return Ok(new
                                    {
                                        message = "El stock ya existe.",
                                        statusCode = HttpStatusCode.Conflict
                                    });
                                }
                            }
                            else
                            {
                                return Ok(new
                                {
                                    message = "El stock no existe.",
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


        [HttpPut]
        [Route("updateStatusById")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult updateStatusById([FromHeader(Name = "Authorization")]string token, [FromBody]Stock stock)
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
                        if (string.IsNullOrEmpty(stock.Id.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(stock.Status.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El status es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else
                        {
                            Stock stockExisting = stockService.findById(stock.Id);

                            if (stockExisting != null)
                            {
                                Stock stockUpdated = stockService.updateStatusById(stock);

                                if (stockUpdated != null)
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
                                        message = "Stock actualizado.",
                                        statusCode = HttpStatusCode.OK
                                    });
                                }
                                else
                                {
                                    return Ok(new
                                    {
                                        message = "El stock no se pudo actualizar, intentalo nuevamente.",
                                        statusCode = HttpStatusCode.NotFound
                                    });
                                }
                            }
                            else
                            {
                                return Ok(new
                                {
                                    message = "El stock no existe.",
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
    }
}