using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DigitusDemo.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsValid { get; set; }
        public string ValidationCode { get; set; }
        public DateTime? RegisteredDate { get; set; }
        public bool IsOnline { get; set; }
        public bool RegCodeSent { get; set; }
        public DateTime? RegCodeSentTime { get; set; }

    }
}