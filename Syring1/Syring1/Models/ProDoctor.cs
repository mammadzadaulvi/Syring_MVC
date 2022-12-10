namespace Syring1.Models
{
    public class ProDoctor
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Icon { get; set; }
        public string PhotoPath { get; set; }
        public DoctorStatus Status { get; set; }

        public enum DoctorStatus
        {
            Dentist,
            Gynecologist,
            Surgeon
        }
    }
}
