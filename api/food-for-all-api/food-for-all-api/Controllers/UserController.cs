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
    public class UserController : ControllerBase
    {
        private UserService userService = new UserService();
        private ListBlackService listBlackService = new ListBlackService();
        private TokenService tokenService = new TokenService();
        private EventLogService eventLogService = new EventLogService();
        private EmailService emailService = new EmailService();
        private IHubContext<UserHub> userHubContext;

        public UserController(IHubContext<UserHub> hubContext)
        {
            userHubContext = hubContext;
        }


        [HttpGet]
        [Route("findByIdUserAndFilterDynamic/{idUser}/{property}/{condition}/{value}")]
        [ProducesResponseType(200, Type = typeof(List<User>))]
        [ProducesResponseType(204, Type = typeof(List<User>))]
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
                        List<User> users = userService.findByIdUserAndFilterDynamic(idUser, searcher, property, condition, value);

                        if (users.Count > 0)
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
                                users = users,
                                countRows = users.Count,
                                statusCode = HttpStatusCode.OK
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                users = users,
                                countRows = users.Count,
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
        [ProducesResponseType(200, Type = typeof(User))]
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
                        User user = userService.findById(id);

                        if (user != null)
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
                                user = user,
                                statusCode = HttpStatusCode.OK
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                message = "El Usuario no existe.",
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
        [Route("findByToken/{token}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult findByToken(string token)
        {
            try
            {
                User user = userService.findByToken(token);

                if (user != null)
                {
                    EventLog eventLog = new EventLog();

                    eventLog.IdUser = user.Id;
                    eventLog.IdEventLogType = 1;
                    eventLog.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    eventLog.HttpMethod = ControllerContext.HttpContext.Request.Method;
                    eventLog.Controller = ControllerContext.ActionDescriptor.ControllerName;
                    eventLog.Method = ControllerContext.ActionDescriptor.ActionName;

                    eventLogService.create(eventLog);

                    return Ok(new
                    {
                        user = user,
                        statusCode = HttpStatusCode.OK
                    });
                }
                else
                {
                    return Ok(new
                    {
                        message = "El Usuario no existe.",
                        statusCode = HttpStatusCode.NotFound
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

        [HttpPost]
        [Route("findByUserNameAndPassword")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult findByUserNameAndPassword([FromBody] User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.UserName))
                {
                    return Ok(new
                    {
                        message = "El nombre de usuario es requerido.",
                        statusCode = HttpStatusCode.NoContent
                    });
                }
                else if (string.IsNullOrEmpty(user.Password))
                {
                    return Ok(new
                    {
                        message = "La clave es requerida.",
                        statusCode = HttpStatusCode.NoContent
                    });
                }
                else
                {
                    User userExisting = userService.findByUserNameAndPassword(user);

                    if (userExisting == null)
                    {
                        return Ok(new
                        {
                            message = "Credenciales incorrectas.",
                            statusCode = HttpStatusCode.Forbidden
                        });
                    }
                    else
                    {
                        Token token = new Token();

                        token.IdUser = userExisting.Id;
                        token.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                        token = tokenService.create(token);

                        EventLog eventLog = new EventLog();

                        eventLog.IdUser = userExisting.Id;
                        eventLog.IdEventLogType = 1;
                        eventLog.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                        eventLog.HttpMethod = ControllerContext.HttpContext.Request.Method;
                        eventLog.Controller = ControllerContext.ActionDescriptor.ControllerName;
                        eventLog.Method = ControllerContext.ActionDescriptor.ActionName;

                        eventLogService.create(eventLog);

                        userHubContext.Clients.All.SendAsync("findByUserNameAndPassword", userExisting);

                        return Ok(new
                        {
                            userExisting = userExisting,
                            token = token.Token1,
                            statusCode = HttpStatusCode.OK
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
        [Route("findByEmailWithFacebook")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult findByEmailWithFacebook([FromBody] User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Email))
                {
                    return Ok(new
                    {
                        message = "El Email es requerido.",
                        statusCode = HttpStatusCode.NoContent
                    });
                }
                else
                {
                    User userExisting = userService.findByEmailWithFacebook(user.Email);

                    if (userExisting == null)
                    {
                        return Ok(new
                        {
                            message = "Credenciales incorrectas.",
                            statusCode = HttpStatusCode.Forbidden
                        });
                    }
                    else
                    {
                        Token token = new Token();

                        token.IdUser = userExisting.Id;
                        token.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                        token = tokenService.create(token);

                        EventLog eventLog = new EventLog();

                        eventLog.IdUser = userExisting.Id;
                        eventLog.IdEventLogType = 1;
                        eventLog.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                        eventLog.HttpMethod = ControllerContext.HttpContext.Request.Method;
                        eventLog.Controller = ControllerContext.ActionDescriptor.ControllerName;
                        eventLog.Method = ControllerContext.ActionDescriptor.ActionName;

                        eventLogService.create(eventLog);

                        userHubContext.Clients.All.SendAsync("findByEmailWithFacebook", userExisting);

                        return Ok(new
                        {
                            userExisting = userExisting,
                            token = token.Token1,
                            statusCode = HttpStatusCode.OK
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
        [ProducesResponseType(201, Type = typeof(User))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(409, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult create([FromBody] User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.UserName))
                {
                    return Ok(new
                    {
                        message = "El nombre de usuario es requerido.",
                        statusCode = HttpStatusCode.NoContent
                    });
                }
                else if (string.IsNullOrEmpty(user.Email))
                {
                    return Ok(new
                    {
                        message = "El email es requerido.",
                        statusCode = HttpStatusCode.NoContent
                    });
                }
                else if (string.IsNullOrEmpty(user.IdUserType.ToString()))
                {
                    return Ok(new
                    {
                        message = "El tipo de usuario es requerido.",
                        statusCode = HttpStatusCode.NoContent
                    });
                }
                else if (string.IsNullOrEmpty(user.IsWithFacebook.ToString()))
                {
                    return Ok(new
                    {
                        message = "Si es con facebook es requerido.",
                        statusCode = HttpStatusCode.NoContent
                    });
                }
                else
                {
                    if (userService.findByEmail(user.Email) != null)
                    {
                        return Ok(new
                        {
                            message = "El usuario ya existe con este correo, intenta con otro correo.",
                            statusCode = HttpStatusCode.Conflict
                        });
                    }
                    else if (userService.findByUserName(user.UserName) != null)
                    {
                        return Ok(new
                        {
                            message = "El usuario ya existe con este nombre de usuario, intenta con otro nombre de usuario.",
                            statusCode = HttpStatusCode.Conflict
                        });
                    }
                    else
                    {
                        if (user.IdUserType == 3)
                        {
                            if (string.IsNullOrEmpty(user.IdInstitution.ToString()))
                            {
                                return Ok(new
                                {
                                    message = "El Id de la Institución es requerido.",
                                    statusCode = HttpStatusCode.NoContent
                                });
                            }
                            else if (string.IsNullOrEmpty(user.Password))
                            {
                                return Ok(new
                                {
                                    message = "La clave es requerida.",
                                    statusCode = HttpStatusCode.NoContent
                                });
                            }
                            else
                            {
                                ListBlack listBlack = listBlackService.findByUser(user);

                                if (listBlack == null)
                                {
                                    user = userService.create(user);

                                    if (user != null)
                                    {
                                        Token token = new Token();

                                        token.IdUser = user.Id;
                                        token.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                                        token = tokenService.create(token);

                                        EventLog eventLog = new EventLog();

                                        eventLog.IdUser = user.Id;
                                        eventLog.IdEventLogType = 1;
                                        eventLog.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                                        eventLog.HttpMethod = ControllerContext.HttpContext.Request.Method;
                                        eventLog.Controller = ControllerContext.ActionDescriptor.ControllerName;
                                        eventLog.Method = ControllerContext.ActionDescriptor.ActionName;

                                        eventLogService.create(eventLog);

                                        return Ok(new
                                        {
                                            userCreated = user,
                                            token = token.Token1,
                                            statusCode = HttpStatusCode.Created
                                        });
                                    }
                                    else
                                    {
                                        return Ok(new
                                        {
                                            message = "El usuario no se pudo crear, intentalo nuevamente.",
                                            statusCode = HttpStatusCode.NotFound
                                        });
                                    }
                                }
                                else
                                {
                                    return Ok(new
                                    {
                                        message = "Es usuario no se puede registrar por denuncias.",
                                        statusCode = HttpStatusCode.Forbidden
                                    });
                                }
                            }
                        }
                        else
                        {
                            if (user.IsWithFacebook)
                            {
                                ListBlack listBlack = listBlackService.findByUser(user);

                                if (listBlack == null)
                                {
                                    user = userService.create(user);

                                    if (user != null)
                                    {
                                        Token token = new Token();

                                        token.IdUser = user.Id;
                                        token.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                                        token = tokenService.create(token);

                                        EventLog eventLog = new EventLog();

                                        eventLog.IdUser = user.Id;
                                        eventLog.IdEventLogType = 1;
                                        eventLog.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                                        eventLog.HttpMethod = ControllerContext.HttpContext.Request.Method;
                                        eventLog.Controller = ControllerContext.ActionDescriptor.ControllerName;
                                        eventLog.Method = ControllerContext.ActionDescriptor.ActionName;

                                        eventLogService.create(eventLog);

                                        return Ok(new
                                        {
                                            userCreated = user,
                                            token = token.Token1,
                                            statusCode = HttpStatusCode.Created
                                        });
                                    }
                                    else
                                    {
                                        return Ok(new
                                        {
                                            message = "El usuario no se pudo crear, intentalo nuevamente.",
                                            statusCode = HttpStatusCode.NotFound
                                        });
                                    }
                                }
                                else
                                {
                                    return Ok(new
                                    {
                                        message = "Es usuario no se puede registrar por denuncias.",
                                        statusCode = HttpStatusCode.Forbidden
                                    });
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(user.Password))
                                {
                                    return Ok(new
                                    {
                                        message = "La clave es requerida.",
                                        statusCode = HttpStatusCode.NoContent
                                    });
                                }
                                else
                                {
                                    ListBlack listBlack = listBlackService.findByUser(user);

                                    if (listBlack == null)
                                    {
                                        user = userService.create(user);

                                        if (user != null)
                                        {
                                            Token token = new Token();

                                            token.IdUser = user.Id;
                                            token.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                                            token = tokenService.create(token);

                                            EventLog eventLog = new EventLog();

                                            eventLog.IdUser = user.Id;
                                            eventLog.IdEventLogType = 1;
                                            eventLog.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                                            eventLog.HttpMethod = ControllerContext.HttpContext.Request.Method;
                                            eventLog.Controller = ControllerContext.ActionDescriptor.ControllerName;
                                            eventLog.Method = ControllerContext.ActionDescriptor.ActionName;

                                            eventLogService.create(eventLog);

                                            return Ok(new
                                            {
                                                userCreated = user,
                                                token = token.Token1,
                                                statusCode = HttpStatusCode.Created
                                            });
                                        }
                                        else
                                        {
                                            return Ok(new
                                            {
                                                message = "El usuario no se pudo crear, intentalo nuevamente.",
                                                statusCode = HttpStatusCode.NotFound
                                            });
                                        }
                                    }
                                    else
                                    {
                                        return Ok(new
                                        {
                                            message = "Es usuario no se puede registrar por denuncias.",
                                            statusCode = HttpStatusCode.Forbidden
                                        });
                                    }
                                }
                            }
                        }
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
        [Route("sendRecoveryPassword")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult sendRecoveryPassword([FromBody]User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Email))
                {
                    return Ok(new
                    {
                        message = "El email es requerido.",
                        statusCode = HttpStatusCode.NoContent
                    });
                }
                else
                {
                    User userExisting = userService.findByEmail(user.Email);

                    if (userExisting != null)
                    {
                        Token token = new Token();

                        token.IdUser = userExisting.Id;
                        token.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                        token = tokenService.create(token);

                        bool resultSendEmail = emailService.sendRecoveryPassword(userExisting, token);

                        if (resultSendEmail)
                        {
                            EventLog eventLog = new EventLog();

                            eventLog.IdUser = userExisting.Id;
                            eventLog.IdEventLogType = 1;
                            eventLog.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                            eventLog.HttpMethod = ControllerContext.HttpContext.Request.Method;
                            eventLog.Controller = ControllerContext.ActionDescriptor.ControllerName;
                            eventLog.Method = ControllerContext.ActionDescriptor.ActionName;

                            eventLogService.create(eventLog);

                            return Ok(new
                            {
                                message = "Correo enviado.",
                                token = token.Token1,
                                userExisting = userExisting,
                                statusCode = HttpStatusCode.OK
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                message = "El correo no se pudo enviar, intentalo nuevamente.",
                                statusCode = HttpStatusCode.NotFound
                            });
                        }
                    }
                    else
                    {
                        return Ok(new
                        {
                            message = "No existe ningun usuario asignado a este correo.",
                            statusCode = HttpStatusCode.NotFound
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
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult updateById([FromHeader(Name = "Authorization")]string token, [FromBody]User user)
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
                        if (string.IsNullOrEmpty(user.Id.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(user.UserName))
                        {
                            return Ok(new
                            {
                                message = "El nombre de usuario es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(user.Email))
                        {
                            return Ok(new
                            {
                                message = "El email es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(user.IdUserType.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El tipo de usuario es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else
                        {
                            if (user.IdUserType == 3)
                            {
                                if (string.IsNullOrEmpty(user.IdInstitution.ToString()))
                                {
                                    return Ok(new
                                    {
                                        message = "El Id de la Institución es requerido.",
                                        statusCode = HttpStatusCode.NoContent
                                    });
                                }
                                else
                                {
                                    User userExisting = userService.findById(user.Id);

                                    if (userExisting != null)
                                    {
                                        User userUpdated = userService.updateById(user);

                                        if (userUpdated != null)
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
                                                message = "Datos actualizado.",
                                                statusCode = HttpStatusCode.OK
                                            });
                                        }
                                        else
                                        {
                                            return Ok(new
                                            {
                                                message = "Los datos no se pudieron actualizar, intentalo nuevamente.",
                                                statusCode = HttpStatusCode.NotFound
                                            });
                                        }
                                    }
                                    else
                                    {
                                        return Ok(new
                                        {
                                            message = "El Usuario no existe.",
                                            statusCode = HttpStatusCode.NotFound
                                        });
                                    }
                                }
                            }
                            else
                            {
                                User userExisting = userService.findById(user.Id);

                                if (userExisting != null)
                                {
                                    User userUpdated = userService.updateById(user);

                                    if (userUpdated != null)
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
                                            message = "Datos actualizado.",
                                            statusCode = HttpStatusCode.OK
                                        });
                                    }
                                    else
                                    {
                                        return Ok(new
                                        {
                                            message = "Los datos no se pudieron actualizar, intentalo nuevamente.",
                                            statusCode = HttpStatusCode.NotFound
                                        });
                                    }
                                }
                                else
                                {
                                    return Ok(new
                                    {
                                        message = "El Usuario no existe.",
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

        [HttpPut]
        [Route("updatePasswordById")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult updatePasswordById([FromHeader(Name = "Authorization")]string token, [FromBody]User user)
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
                        if (string.IsNullOrEmpty(user.Id.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(user.Password))
                        {
                            return Ok(new
                            {
                                message = "La Clave es requerida.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else
                        {
                            User userExisting = userService.findById(user.Id);

                            if (userExisting != null)
                            {
                                User userUpdated = userService.updatePasswordById(user);

                                if (userUpdated != null)
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
                                        message = "Contraseña actualizado.",
                                        statusCode = HttpStatusCode.OK
                                    });
                                }
                                else
                                {
                                    return Ok(new
                                    {
                                        message = "La contraseña no se pudo actualizar, intentalo nuevamente.",
                                        statusCode = HttpStatusCode.NotFound
                                    });
                                }
                            }
                            else
                            {
                                return Ok(new
                                {
                                    message = "El Usuario no existe.",
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
        [Route("updateOnLineById")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult updateOnLineById([FromHeader(Name = "Authorization")]string token, [FromBody]User user)
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
                        if (string.IsNullOrEmpty(user.Id.ToString()))
                        {
                            return Ok(new
                            {
                                message = "El Id es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else if (string.IsNullOrEmpty(user.OnLine.ToString()))
                        {
                            return Ok(new
                            {
                                message = "OnLine es requerido.",
                                statusCode = HttpStatusCode.NoContent
                            });
                        }
                        else
                        {
                            User userExisting = userService.findById(user.Id);

                            if (userExisting != null)
                            {
                                User userUpdated = userService.updateOnLineById(user);

                                if (userUpdated != null)
                                {
                                    EventLog eventLog = new EventLog();

                                    eventLog.IdUser = tokenExisting.IdUser;
                                    eventLog.IdEventLogType = 1;
                                    eventLog.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                                    eventLog.HttpMethod = ControllerContext.HttpContext.Request.Method;
                                    eventLog.Controller = ControllerContext.ActionDescriptor.ControllerName;
                                    eventLog.Method = ControllerContext.ActionDescriptor.ActionName;

                                    eventLogService.create(eventLog);

                                    userHubContext.Clients.All.SendAsync("updateOnLineById", userUpdated);

                                    return Ok(new
                                    {
                                        message = "OnLine actualizado.",
                                        statusCode = HttpStatusCode.OK
                                    });
                                }
                                else
                                {
                                    return Ok(new
                                    {
                                        message = "OnLine no se pudo actualizar, intentalo nuevamente.",
                                        statusCode = HttpStatusCode.NotFound
                                    });
                                }
                            }
                            else
                            {
                                return Ok(new
                                {
                                    message = "El Usuario no existe.",
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
        [Route("updatePasswordByIdAndRecovery")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult updatePasswordByIdAndRecovery([FromBody]User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Id.ToString()))
                {
                    return Ok(new
                    {
                        message = "El Id es requerido.",
                        statusCode = HttpStatusCode.NoContent
                    });
                }
                else if (string.IsNullOrEmpty(user.Password))
                {
                    return Ok(new
                    {
                        message = "La Clave es requerida.",
                        statusCode = HttpStatusCode.NoContent
                    });
                }
                else
                {
                    User userExisting = userService.findById(user.Id);

                    if (userExisting != null)
                    {
                        User userUpdated = userService.updatePasswordById(user);

                        if (userUpdated != null)
                        {
                            Token token = new Token();

                            token.IdUser = userUpdated.Id;
                            token.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                            token = tokenService.create(token);

                            EventLog eventLog = new EventLog();

                            eventLog.IdUser = userUpdated.Id;
                            eventLog.IdEventLogType = 1;
                            eventLog.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                            eventLog.HttpMethod = ControllerContext.HttpContext.Request.Method;
                            eventLog.Controller = ControllerContext.ActionDescriptor.ControllerName;
                            eventLog.Method = ControllerContext.ActionDescriptor.ActionName;

                            eventLogService.create(eventLog);

                            return Ok(new
                            {
                                message = "Contraseña actualizado.",
                                userExisting = userExisting,
                                token = token.Token1,
                                statusCode = HttpStatusCode.OK
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                message = "La contraseña no se pudo actualizar, intentalo nuevamente.",
                                statusCode = HttpStatusCode.NotFound
                            });
                        }
                    }
                    else
                    {
                        return Ok(new
                        {
                            message = "El Usuario no existe.",
                            statusCode = HttpStatusCode.NotFound
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
                        User userExisting = userService.findById(id);

                        if (userExisting != null)
                        {
                            User userDeleted = userService.deleteById(id);

                            if (!userDeleted.Status)
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
                                    message = "Usuario eliminado.",
                                    statusCode = HttpStatusCode.OK
                                });
                            }
                            else
                            {
                                return Ok(new
                                {
                                    message = "El Usuario no se pudo eliminar, intentalo nuevamente.",
                                    statusCode = HttpStatusCode.NotFound
                                });
                            }
                        }
                        else
                        {
                            return Ok(new
                            {
                                message = "El Usuario no existe.",
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