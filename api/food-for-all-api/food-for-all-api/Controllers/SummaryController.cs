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
    public class SummaryController : ControllerBase
    {
        private UserService userService = new UserService();
        private ChartService chartService = new ChartService();
        private StockCommentService stockCommentService = new StockCommentService();
        private ProductService productService = new ProductService();
        private TokenService tokenService = new TokenService();
        private EventLogService eventLogService = new EventLogService();

        [HttpGet]
        [Route("findByIdUser/{idUser}")]
        [ProducesResponseType(200, Type = typeof(object))]
        [ProducesResponseType(204, Type = typeof(string))]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
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
                        User user = userService.findById(idUser);

                        if (user != null)
                        {
                            List<Chart> chartsDoughnut = chartService.findByIdUserAndChartDoughnut(user.Id);
                            List<Chart> chartsDoughnutDonor = chartService.findByIdUserAndChartDoughnutDonor(user.Id);
                            List<Chart> chartsBar = chartService.findByIdUserAndChartBar(user.Id);

                            User userDonorMaxStockRetirated = userService.findByIdUserBeneficiaryAndMaxStockRetirated(user.Id);

                            StockComment stockCommentLastBeneficiary = stockCommentService.findByIdUserBeneficiaryAndLastComment(user.Id);
                            StockComment stockCommentLastDonor = stockCommentService.findByIdUserDonorAndLastComment(user.Id);

                            Product productLessReceived = productService.findByIdUserBeneficiaryAndLessReceived(user.Id);
                            Product productMoreReceived = productService.findByIdUserBeneficiaryAndMoreReceived(user.Id);
                            Product productLastReceived = productService.findByIdUserBeneficiaryAndLastReceived(user.Id);
                            Product productLastStock = productService.findByIdUserDonorAndLastStock(user.Id);
                            Product productLessQuantityStock = productService.findByIdUserDonorAndLessQuantityStock(user.Id);

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
                                userDonorMaxStockRetirated = userDonorMaxStockRetirated,
                                chartsDoughnut = chartsDoughnut,
                                chartsDoughnutDonor = chartsDoughnutDonor,
                                chartsBar = chartsBar,
                                stockCommentLastBeneficiary = stockCommentLastBeneficiary,
                                stockCommentLastDonor = stockCommentLastDonor,
                                productLessReceived = productLessReceived,
                                productMoreReceived = productMoreReceived,
                                productLastReceived = productLastReceived,
                                productLastStock = productLastStock,
                                productLessQuantityStock = productLessQuantityStock,
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
        [Route("findAllChartDoughnut")]
        [ProducesResponseType(200, Type = typeof(List<Chart>))]
        [ProducesResponseType(204, Type = typeof(List<Chart>))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult findAllChartDoughnut()
        {
            try
            {
                List<Chart> charts = chartService.findAllChartDoughnut();

                if (charts.Count > 0)
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
                        charts = charts,
                        countRows = charts.Count,
                        statusCode = HttpStatusCode.OK
                    });
                }
                else
                {
                    return Ok(new
                    {
                        charts = charts,
                        countRows = charts.Count,
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