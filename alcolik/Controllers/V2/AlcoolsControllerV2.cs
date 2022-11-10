using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using alcolik.Data;
using alcolik.Model;
using alcolikLib.Controllers;
using alcolikLib.Controllers.V2;

namespace alcolik.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AlcoolsController : BaseControllerV2<AlcolikDbContext, Alcool>
    {
        public AlcoolsController(AlcolikDbContext context) : base(context)
        {

        }
    }
}
