using JOIEnergy.Domain;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JOIEnergy.Controllers
{
    [Route("billing")]
    public class BillingController : Controller
    {
        private readonly IBillingService _billingService;

        public BillingController(IBillingService billingService)
        {
            _billingService = billingService;
        }

        [HttpGet("generatebill/{smartMeterId}")]
        public ObjectResult GetBill(string smartMeterId,[FromBody]BillingRequest request )
        {
            try
            {
                return new OkObjectResult(_billingService.GenerateBill(smartMeterId
                                                        , request.StartDate
                                                        , request.EndDate));
            }catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
