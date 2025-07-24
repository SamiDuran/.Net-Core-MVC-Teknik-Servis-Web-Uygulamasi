namespace SamTEK.Models
{
    public class kullaniciCihazModel
    {
        public kullaniciTablosu kullanici { get; set; }
        public List<cihazTablosu> cihaz { get; set; }
        public List<servisTablosu> servis { get; set; }
    }
}
