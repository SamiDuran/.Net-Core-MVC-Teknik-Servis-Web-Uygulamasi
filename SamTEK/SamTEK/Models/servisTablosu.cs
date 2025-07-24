using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SamTEK.Models
{
    public class servisTablosu : DbContext
    {
        [Key]
        public int servisID { get; set; }
        
        public int cihazID { get; set; }
        public string surec { get; set; }
        public bool teslim { get; set; }
    }
}
