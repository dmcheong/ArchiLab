using ArchiLibrary.Data;
using ArchiLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ArchiLibrary.controllers
{
    [ApiController]
    public abstract class BaseController<TContext, TModel> : ControllerBase where TContext : BaseDbContext where TModel : BaseModel 
    {
        protected readonly TContext _context;
        private readonly ILogger _logger;


        public BaseController(TContext context, Microsoft.Extensions.Logging.ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<TModel>> GetAll([FromQuery] Params param)
        {
            //return await _context.Set<TModel>().Where(x => x.Active).SortDsc(param).PicByRange(param).ToListAsync();
            _logger.LogInformation("Api executing...");
            if (param.CreatedAt != null)
            {
                var date = DateTime.Parse(param.CreatedAt);

                return await _context.Set<TModel>().Where(x => x.CarType == param.Type || x.CreatedAt == date).ToListAsync();
            }
            return await _context.Set<TModel>().Where(x => x.Active).ToListAsync();

        }

        [HttpGet("{id}")]// /api/{item}/3
        public async Task<ActionResult<TModel>> GetById([FromRoute] int id)
        {
            var item = await _context.Set<TModel>().SingleOrDefaultAsync(x => x.ID == id);
            if (item == null || !item.Active)
                return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<IActionResult> PostItem([FromBody] TModel item)
        {
            item.Active = true;
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new { id = item.ID }, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TModel>> PutItem([FromRoute] int id, [FromBody] TModel item)
        {
            if (id != item.ID)
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
            //_context.Entry(item).State = EntityState.Deleted;
            _context.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        private bool ItemExists(int id)
        {
            return _context.Set<TModel>().Any(x => x.ID == id);
        }

    }
}
