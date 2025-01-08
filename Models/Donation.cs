using System;
using System.ComponentModel.DataAnnotations;

namespace CharityApp.Models
{
    public class Donation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string DonorName { get; set; }

        [Required]
        [EmailAddress]
        public string DonorEmail { get; set; }

        [Required]
        public decimal Amount { get; set; }
        public string? GcashNumber { get; set; }

        [Required]
        public DateTime DonationDate { get; set; }

        [StringLength(500)]
        public string Message { get; set; }

        public bool IsAnonymous { get; set; }
    }
}
