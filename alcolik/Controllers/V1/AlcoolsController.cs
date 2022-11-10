using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using alcolik.Data;
using alcolik.Model;
using alcolikLib.Controllers.V1;

namespace alcolik.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AlcoolsController : BaseController<AlcolikDbContext, Alcool>
    {
        public AlcoolsController(AlcolikDbContext context) : base(context)
        {

        }
    }
}
