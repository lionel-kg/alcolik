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
using alcolikLib.Controllers.V1;
using alcolikLib.Controllers.V2;

namespace alcolik.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : BaseControllerV2<AlcolikDbContext, Brand>
    {
        public BrandsController(AlcolikDbContext context) : base(context)
        {

        }
    }
}
