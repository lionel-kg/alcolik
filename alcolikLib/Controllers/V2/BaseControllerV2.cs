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

namespace alcolikLib.Controllers.V2
{
    public abstract class BaseControllerV2<TContext, TModel> : ControllerBase where TContext : BaseDbContext where TModel : BaseModel
    {
        protected readonly TContext _context;

        public BaseControllerV2(TContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public async Task<IEnumerable<TModel>> GetBySearch([FromQuery] string name)
        {
            var result = _context.Set<TModel>().Where(e => e.Name.Contains(name));
            return await result.ToListAsync();
        }

        [HttpGet]
        public async Task<IEnumerable<TModel>> GetByfilter([FromQuery] Params param)
        {
            var url = this.Request.Query;
            var properties = typeof(TModel).GetProperties();
            var newArrayParam = new Dictionary<string, string>();
            var properParam = param.GetType();
            foreach (var item in url)
            {
                if (properParam.GetProperty(item.Key) == null && typeof(TModel).GetProperty(item.Key) != null)
                {
                    newArrayParam[item.Key] = item.Value;
                }
            }
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

            return await _context.Set<TModel>().filter(param, newArrayParam).Sort(param).ToListAsync();
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
