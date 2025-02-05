using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourBookingManagment.Database;
using TourBookingManagment.Model;

namespace TourBookingManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CouponsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Coupons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coupons>>> GetCoupons()
        {
            return await _context.Coupons.ToListAsync();
        }

        // POST: api/Coupons
        [HttpPost]
        public async Task<ActionResult<Coupons>> CreateCoupon(Coupons coupon)
        {
            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCoupon), new { id = coupon.Id }, coupon);
        }

        // GET: api/Coupons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Coupons>> GetCoupon(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);

            if (coupon == null)
            {
                return NotFound();
            }

            return coupon;
        }

        // PUT: api/Coupons/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoupon(int id, Coupons coupon)
        {
            if (id != coupon.Id)
            {
                return BadRequest();
            }

            _context.Entry(coupon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CouponExists(id))
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

        // DELETE: api/Coupons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }

            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Coupons/validate
        [HttpPost("validate")]
        public async Task<ActionResult<Coupons>> ValidateCoupon(CouponValidationRequest request)
        {
            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(c => c.Code == request.Code && c.IsActive);

            if (coupon == null)
            {
                return NotFound("Invalid coupon code.");
            }

            if (coupon.ExpiryDate < DateTime.UtcNow)
            {
                return BadRequest("Coupon has expired.");
            }

            if (coupon.UsageLimit > 0 && coupon.UsageCount >= coupon.UsageLimit)
            {
                return BadRequest("Coupon usage limit exceeded.");
            }

            if (request.Amount < coupon.MinimumAmount)
            {
                return BadRequest($"Minimum amount of ${coupon.MinimumAmount} required.");
            }

            return coupon;
        }

        private bool CouponExists(int id)
        {
            return _context.Coupons.Any(e => e.Id == id);
        }
    }
    public class CouponValidationRequest
    {
        public string Code { get; set; }
        public decimal Amount { get; set; }
    }
}