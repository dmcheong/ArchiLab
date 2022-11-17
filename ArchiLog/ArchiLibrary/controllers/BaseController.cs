using ArchiLibrary.Data;
using ArchiLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using ArchiLibrary.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Xml.Linq;
using Microsoft.VisualBasic;

namespace ArchiLibrary.controllers
{

    [ApiController]
    public abstract class BaseController<TContext, TModel> : ControllerBase where TContext : BaseDbContext where TModel : BaseModel
    {
        protected readonly TContext _context;

        public BaseController(TContext context)
        {
            _context = context;
        }



        [HttpGet]
        public async Task<IEnumerable<TModel>> GetAll([FromQuery] Params param)
        {
            if (param.Range != null) {
                return await _context.Set<TModel>().Where(x => x.Active).PicByRange(param).ToListAsync();
            }

            else if (param.CreatedAt != null)

            {

                var date = DateTime.Parse(param.CreatedAt);

                return await _context.Set<TModel>().Where(x => x.CreatedAt == date).ToListAsync();
            }
            //date entre
            else if(param.dateBetween != null)
            {
                string champ = param.dateBetween;
                string[] dates = champ.Split(',');

                var beginDate = DateTime.Parse(dates[0]);
                var endDate = DateTime.Parse(dates[1]);

                return await _context.Set<TModel>().Where(x => x.CreatedAt < beginDate & x.CreatedAt < endDate).ToListAsync();

            }

            else if (param.Type != null)
            {
                string champ = param.Type;

                string[] type = champ.Split(',');

                //var res = await _context.Set<TModel>().Where(x => x.Active).ToListAsync();
                //List.Add(1);
                foreach (var element in type)
                {
                    Console.WriteLine(element);
                    return await _context.Set<TModel>().Where(x => x.CarType == element).ToListAsync();
                }

                return await _context.Set<TModel>().Where(x => x.Active).ToListAsync();
            }

            //amount sold
            else if (param.Sold != null)
            {
                string champ = param.Sold;

                int amount = int.Parse(champ);

                if (!String.IsNullOrEmpty(amount.ToString())) {

                    return await _context.Set<TModel>().Where(x => x.AmountSold == amount).ToListAsync();
                }

            }
            //sold between
            else if(param.SoldBetween != null)
            {
                string champ = param.SoldBetween;
                string[] amount = champ.Split(',');

                int min = int.Parse(amount[0]);
                int max = int.Parse(amount[1]);

                if (!String.IsNullOrEmpty(min.ToString()) & !String.IsNullOrEmpty(max.ToString()))
                {
                    return await _context.Set<TModel>().Where(x => x.AmountSold >= min & x.AmountSold <= max).ToListAsync();
                }

            }

            else if (param.Asc != null)
            {
                return await _context.Set<TModel>().Where(x => x.Active).SortAsc(param).ToListAsync();

            }
            else if (param.Desc != null){
                return await _context.Set<TModel>().Where(x => x.Active).SortDsc(param).ToListAsync();

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
