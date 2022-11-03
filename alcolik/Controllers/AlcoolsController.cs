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

namespace alcolik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlcoolsController : BaseController<AlcolikDbContext,Alcool>
    {
        public AlcoolsController(AlcolikDbContext context) : base(context)
        {

        }
    }
}
