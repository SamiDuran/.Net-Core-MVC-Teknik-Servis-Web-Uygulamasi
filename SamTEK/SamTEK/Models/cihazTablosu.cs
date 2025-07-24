using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SamTEK.Models
{
    public class cihazTablosu : DbContext
    {
        [Key]
        public int cihazID { get; set; }

        public int kullaniciID { get; set; }
        public string cihaz { get; set; }
        public string sorun { get; set; }
        public bool servis { get; set; }
    }
}
