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

namespace alcolikLib.Controllers.V1
{
    public abstract class BaseController<TContext, TModel> : ControllerBase where TContext : BaseDbContext where TModel : BaseModel
    {
        protected readonly TContext _context;

        public BaseController(TContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<TModel>> GetAll()
        {
            return await _context.Set<TModel>().Where(x => x.Active).ToListAsync();
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
