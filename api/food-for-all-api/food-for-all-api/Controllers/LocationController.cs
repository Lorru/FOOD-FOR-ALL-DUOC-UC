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
    public class LocationController : ControllerBase
    {
        private LocationService locationService = new LocationService();
        private TokenService tokenService = new TokenService();
        private EventLogService eventLogService = new EventLogService();

        [HttpGet]
        [Route("findByIdUser/{idUser}")]
        [ProducesResponseType(200, Type = typeof(List<Location>))]
        [ProducesResponseType(204, Type = typeof(List<Location>))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult findByIdUser([FromHeader(Name = "Authorization")]string token, int idUser)
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
                        List<Location> locations = locationService.findByIdUser(idUser);

                        if (locations.Count > 0)
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
                                locations = locations,
                                countRows = locations.Count,
                                statusCode = HttpStatusCode.OK
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                locations = locations,
                                countRows = locations.Count,
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
        [Route("findAllStockAvailable")]
        [ProducesResponseType(200, Type = typeof(List<Location>))]
        [ProducesResponseType(204, Type = typeof(List<Location>))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult findAllStockAvailable([FromHeader(Name = "Authorization")]string token, string searcher = null, bool? isSearchLocation = null)
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
                        List<Location> locations = locationService.findAllStockAvailable(searcher, isSearchLocation);

                        if (locations.Count > 0)
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
                                locations = locations,
                                countRows = locations.Count,
                                statusCode = HttpStatusCode.OK
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                locations = locations,
                                countRows = locations.Count,
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
        [ProducesResponseType(200, Type = typeof(Location))]
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
                        Location location = locationService.findById(id);

                        if (location != null)
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
                                location = location,
                                statusCode = HttpStatusCode.OK
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                message = "La ubicación no existe.",
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
        [Route("findByIdUserAndMain/{idUser}")]
        [ProducesResponseType(200, Type = typeof(Location))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult findByIdUserAndMain([FromHeader(Name = "Authorization")]string token, int idUser)
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
                        Location location = locationService.findByIdUserAndMain(idUser);

                        if (location != null)
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
                                location = location,
                                statusCode = HttpStatusCode.OK
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                message = "La ubicación no existe.",
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
        public IActionResult create([FromHeader(Name = "Authorization")]string token, [FromBody]Location location)
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
                        if (string.IsNullOrEmpty(location.IdUser.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id del usuario es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(location.Longitude.ToString()))
                        {
                            return Ok(new
                            {
                                message = "La longitud es requerida.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(location.Latitude.ToString()))
                        {
                            return Ok(new
                            {
                                message = "La latitud es requerida.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(location.IsMain.ToString()))
                        {
                            return Ok(new
                            {
                                message = "Si es principal es requerida.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(location.Address))
                        {
                            return Ok(new
                            {
                                message = "La dirección es requerida.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(location.Country))
                        {
                            return Ok(new
                            {
                                message = "El país es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else
                        {
                            Location locationExisting = locationService.findByIdUserAndAddress(location.IdUser, location.Address);

                            if (locationExisting == null)
                            {
                                location = locationService.create(location);

                                if (location.Id != 0)
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
                                        message = "Ubicación creada.",
                                        statusCode = HttpStatusCode.Created
                                    });
                                }
                                else
                                {
                                    return Ok(new
                                    {
                                        message = "La ubicación no se pudo crear, intentalo nuevamente.",
                                        statusCode = HttpStatusCode.NotFound
                                    });
                                }
                            }
                            else
                            {
                                return Ok(new
                                {
                                    message = "La ubicación ya existe.",
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
        [Route("updateIsMainById")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult updateIsMainById([FromHeader(Name = "Authorization")]string token, [FromBody]Location location)
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
                        if (string.IsNullOrEmpty(location.Id.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(location.IsMain.ToString()))
                        {
                            return Ok(new
                            {
                                message = "Si es principal es requerida.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else
                        {
                            Location locationExisting = locationService.findById(location.Id);

                            if (locationExisting != null)
                            {
                                Location locationUpdated = locationService.updateIsMainById(location);

                                if (locationUpdated != null)
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
                                        message = "Ubicación actualizada.",
                                        statusCode = HttpStatusCode.OK
                                    });
                                }
                                else
                                {
                                    return Ok(new
                                    {
                                        message = "La ubicación no se pudo actualizar, intentalo nuevamente.",
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

        [HttpDelete]
        [Route("deleteById/{id}")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult deleteById([FromHeader(Name = "Authorization")]string token, int id)
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
                        Location locationExisting = locationService.findById(id);

                        if (locationExisting != null)
                        {
                            Location locationDeleted = locationService.deleteById(id);

                            if (!locationDeleted.Status)
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
                                    message = "Ubicación eliminada.",
                                    statusCode = HttpStatusCode.OK
                                });
                            }
                            else
                            {
                                return Ok(new
                                {
                                    message = "La ubicación no se pudo eliminar, intentalo nuevamente.",
                                    statusCode = HttpStatusCode.NotFound
                                });
                            }
                        }
                        else
                        {
                            return Ok(new
                            {
                                message = "La ubicación no existe.",
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