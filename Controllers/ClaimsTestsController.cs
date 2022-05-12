using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spartan_claim_service.Models;

namespace spartan_claim_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsTestsController : ControllerBase
    {
        private readonly Claims_dbContext _context;

        public ClaimsTestsController(Claims_dbContext context)
        {
            _context = context;
        }

        // GET: api/ClaimsTests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClaimsTest>>> GetClaimsTests()
        {
            return await _context.ClaimsTests.OrderByDescending(u => u.Id).Take(10).ToListAsync();
        }

        // GET: api/ClaimsTests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClaimsTest>> GetClaimsTest(int id)
        {
            var claimsTest = await _context.ClaimsTests.FindAsync(id);

            if (claimsTest == null)
            {
                return NotFound();
            }

            return claimsTest;
        }

        // PUT: api/ClaimsTests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClaimsTest(int id, ClaimsTest claimsTest)
        {
            if (id != claimsTest.Id)
            {
                return BadRequest();
            }

            _context.Entry(claimsTest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClaimsTestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ClaimsTests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClaimsTest>> PostClaimsTest(ClaimsTest claimsTest)
        {
            _context.ClaimsTests.Add(claimsTest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClaimsTest", new { id = claimsTest.Id }, claimsTest);
        }

        // DELETE: api/ClaimsTests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClaimsTest(int id)
        {
            var claimsTest = await _context.ClaimsTests.FindAsync(id);
            if (claimsTest == null)
            {
                return NotFound();
            }

            _context.ClaimsTests.Remove(claimsTest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClaimsTestExists(int id)
        {
            return _context.ClaimsTests.Any(e => e.Id == id);
        }
    }
}
