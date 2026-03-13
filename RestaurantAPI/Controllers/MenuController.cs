using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MenuController(AppDbContext context)
        {
            _context = context;
        }

        // GET ALL MENU ITEMS
        [HttpGet]
        public async Task<IActionResult> GetMenu()
        {
            var menu = await _context.MenuItems.ToListAsync();
            return Ok(menu);
        }

        // ADD MENU ITEM
        [HttpPost]
        public async Task<IActionResult> AddMenu(MenuItem item)
        {
            _context.MenuItems.Add(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        // DELETE MENU ITEM
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);

            if (item == null)
                return NotFound();

            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}