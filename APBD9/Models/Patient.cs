﻿using System.ComponentModel.DataAnnotations;

namespace APBD9.Models
{
    public class Patient
    {
        public int IdPatient { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        
        [Required]
        public DateTime Birthdate { get; set; }
        
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}