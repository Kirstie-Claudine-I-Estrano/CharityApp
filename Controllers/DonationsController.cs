using System;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CharityApp.Data;
using CharityApp.Models;

namespace CharityApp.Controllers
{
    public class DonationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Donations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Donations.ToListAsync());
        }

        // GET: Donations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // GET: Donations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Donations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DonorName,DonorEmail,Amount,GcashNumber,DonationDate,Message,IsAnonymous")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donation);
                await _context.SaveChangesAsync();

                // Send email confirmation
                await SendConfirmationEmail(donation);

                // Redirect to ThankYou page after successful donation
                return RedirectToAction(nameof(ThankYou));
            }
            return View(donation);
        }

        // Email Sending Method
        private async Task SendConfirmationEmail(Donation donation)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("HeartFund", "heartfund3@gmail.com")); // Replace with your email
                email.To.Add(new MailboxAddress(donation.DonorName, donation.DonorEmail));
                email.Subject = "Donation Confirmation";

                email.Body = new TextPart("plain")
                {
                    Text = $@"Dear {donation.DonorName},

Thank you for your generous donation of {donation.Amount:C} on {donation.DonationDate:d}.
Your contribution will greatly help our cause.

Message: {donation.Message}

We appreciate your support!

Best regards,
The CharityApp Team"
                };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls); // Replace with your SMTP server and port
                await smtp.AuthenticateAsync("heartfund3@gmail.com", "xoeg gten rqrj yrnp"); // Replace with your credentials
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Log the error (you can integrate a logging framework like Serilog or NLog)
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        // GET: Donations/ThankYou
        public IActionResult ThankYou()
        {
            return View();
        }

        // GET: Donations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound();
            }
            return View(donation);
        }

        // POST: Donations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DonorName,DonorEmail,Amount,GcashNumber,DonationDate,Message,IsAnonymous")] Donation donation)
        {
            if (id != donation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonationExists(donation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(donation);
        }

        // GET: Donations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // POST: Donations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation != null)
            {
                _context.Donations.Remove(donation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonationExists(int id)
        {
            return _context.Donations.Any(e => e.Id == id);
        }
    }
}
