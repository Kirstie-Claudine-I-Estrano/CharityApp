using CharityApp.Data;  // Make sure you're using the correct namespace for ApplicationDbContext
using CharityApp.Models;  // Include the namespace for your Donation model
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CharityApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonationsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Inject ApplicationDbContext to access the database
        public DonationsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/donationsapi
        [HttpGet]
        public ActionResult<IEnumerable<Donation>> Get()
        {
            // Fetch all donations from the database
            var donations = _context.Donations.ToList();
            return Ok(donations);  // Return the list of donations as a response
        }

        // POST: api/donationsapi
        [HttpPost]
        public ActionResult<Donation> Post([FromBody] Donation donation)
        {
            // Add the new donation to the database
            _context.Donations.Add(donation);
            _context.SaveChanges();

            // Return a response with the created donation
            return CreatedAtAction(nameof(Get), new { id = donation.Id }, donation);
        }

        // PUT: api/donationsapi/{id}
        [HttpPut("{id}")]
        public ActionResult<Donation> Put(int id, [FromBody] Donation donation)
        {
            // Find the existing donation by ID
            var existingDonation = _context.Donations.FirstOrDefault(d => d.Id == id);
            if (existingDonation == null)
            {
                return NotFound();  // Return 404 if donation not found
            }

            // Update properties of the existing donation
            existingDonation.DonorName = donation.DonorName;
            existingDonation.DonorEmail = donation.DonorEmail;
            existingDonation.Amount = donation.Amount;
            existingDonation.GcashNumber = donation.GcashNumber;
            existingDonation.DonationDate = donation.DonationDate;
            existingDonation.Message = donation.Message;
            existingDonation.IsAnonymous = donation.IsAnonymous;

            // Save changes to the database
            _context.SaveChanges();

            return NoContent();  // Return 204 No Content (successful update)
        }

        // DELETE: api/donationsapi/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            // Find the donation by ID
            var donation = _context.Donations.FirstOrDefault(d => d.Id == id);
            if (donation == null)
            {
                return NotFound();  // Return 404 if donation not found
            }

            // Remove the donation from the database
            _context.Donations.Remove(donation);
            _context.SaveChanges();

            return NoContent();  // Return 204 No Content (successful deletion)
        }
    }
}
