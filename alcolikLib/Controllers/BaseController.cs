using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;
using alcolikLib.Data;
using alcolikLib.Model;
using alcolikLib.Extensions;
using System.Diagnostics;
using System.Xml.Linq;

namespace alcolikLib.Controllers
{
    public abstract class BaseController<TContext, TModel> : ControllerBase where TContext : BaseDbContext where TModel : BaseModel
    {
        protected readonly TContext _context;

        public BaseController(TContext context)
        {
            _context = context;
        }


        /*[HttpGet]
       public async Task<IEnumerable<TModel>> GetAll([FromQuery] string? asc, [FromQuery] Params param)
       {
            return await _context.Set<TModel>().Where(x => x.Active).Sort(param).ToListAsync();
       }*/

        [HttpGet]
        public async Task<IEnumerable<TModel>> GetByfilter([FromQuery] Params? param)
       {
            var url = this.Request.Query;
            var properties = typeof(TModel).GetProperties();
            var newArrayParam = new Dictionary<string,string>();
            var properParam = param.GetType();
            foreach (var item in url)
            {
                if (properParam.GetProperty(item.Key) == null && typeof(TModel).GetProperty(item.Key) != null)
                {
                    newArrayParam[item.Key] = item.Value;
                }
            }*/
           /* foreach (var property in properties)
            {
                if(param.GetType().GetProperty(property.Name) != null)
                {
                    var name = property.Name;
                    newArrayParam[name] = ;
                }
                //if(x.GetProperty(p.Name))
            }*/
            
           /* foreach (KeyValuePair<string,string> kvp in properties)
            {
                if (kvp.Key == )
                
            }*/
           
            return await _context.Set<TModel>().filter(param,newArrayParam).Sort(param).ToListAsync();
        }

        //pagination 

        [HttpGet("/pagination")]
        public async Task<ActionResult<IEnumerable<TModel>>> GetAll([FromQuery] Params p)
        {
            const int Accept = 50;
            var url = this.Request.Query;
            var route = url + Request.Path.Value + Request.QueryString.Value;
            route = route.Remove(route.IndexOf("Range=") + 6, 3);

            var query = _context.Set<TModel>().Where(x => x.Active);
            query = query.Sort(p);
            if (!string.IsNullOrWhiteSpace(p.Range))
            {
                string[] values = p.Range.Split('-');
                var start = int.Parse(values[0]);
                var end = int.Parse(values[1]);

                var nb = end - start;
                int nbitems1 = end - 1;
                int nbitems2 = 0;
                int totalItems = _context.Set<TModel>().Where(x => x.Active).Count();

                if (start > end || nb > (Accept - 1) || end > (totalItems - 1))
                    return BadRequest();

                string first = ("0-" + nb);
                string prev = "";
                string next = "";
                string last = "";

                first = route.Replace("Range=", "Range=" + first);
                first = first + "; rel=\"first\", ";
                last = route.Replace("Range=", "Range=" + (totalItems - nb) + "-" + totalItems + "; rel=\"last\"");

                nbitems2 = start - 1;
                nbitems1 = nbitems2 - nb;

                if (nbitems1 >= 0)
                {
                    prev = route.Replace("Range=", "Range=" + nbitems1 + "-" + nbitems2 + "; rel=\"prev\", ");
                }
                else
                {
                    prev = first.Replace("first", "prev");
                }

                nbitems1 = (start + nb) + 1;
                nbitems2 = nbitems1 + nb;

                if (nbitems2 <= (totalItems - 1))
                {
                    next = route.Replace("Range=", "Range=" + nbitems1 + "-" + nbitems2 + "; rel=\"last\", ");
                }
                else
                {
                    next = last.Replace("last", "next");
                }

                //var first = route
                /*if (nb < 0 && Accept < nb)
                {
                    return (IEnumerable<TModel>)BadRequest();
                }*/
                this.Response.Headers.Add("Content-Range", p.Range);
                this.Response.Headers.Add("Accept-Range", Accept.ToString());
                this.Response.Headers.Add("Link", url + first + prev + next + last);
                //return await QueryExtensions.Sort(_context.Set<TModel>().Where(x => x.Active), param).ToListAsync();
                query = query.Pagination(start, end);

            }
            return await query.ToListAsync();

            //return await _context.Set<TModel>().Where(x => x.Active).OrderBy(x => x.CreatedAt).ThenBy(x => x.ID).ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> PostItem([FromBody] TModel item)
        {
            item.Active = true;
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new { id = item.Id }, item);
        }
        [HttpGet("{id}")] // /api/brands/3
        public async Task<ActionResult<TModel>> GetById([FromRoute] int id)
        {
            var item = await _context.Set<TModel>().SingleOrDefaultAsync(x => x.Id == id);
            if (item == null || !item.Active)
                return NotFound();
            return item;
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<TModel>> PutItem([FromRoute] int id, [FromBody] TModel item)
        {
            if (id != item.Id)
                return BadRequest();
            if (!ItemExists(id))
                return NotFound();

            //_context.Entry(item).State = EntityState.Modified;
            _context.Update(item);
            await _context.SaveChangesAsync();

            return item;
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<TModel>> DeleteItem([FromRoute] int id)
        {
            var item = await _context.Set<TModel>().FindAsync(id);
            if (item == null)
                return BadRequest();
            _context.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }
        private bool ItemExists(int id)
        {
            return _context.Set<TModel>().Any(x => x.Id == id);
        }
    }
}
