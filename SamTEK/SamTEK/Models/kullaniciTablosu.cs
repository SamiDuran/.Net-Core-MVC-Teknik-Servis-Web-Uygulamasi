using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SamTEK.Models
{
    public class kullaniciTablosu : DbContext
    {
        [Key]
        public int kullaniciID { get; set; }

        public bool rank { get; set; }
        public string adi { get; set; }
        public string eposta { get; set; }
        public string sifre { get; set; }
    }
}
