namespace Syring1.Models
{
    public class AboutPhoto
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public int Order { get; set; }  
        public int AboutId { get; set; }    
        public About About { get; set; }
    }
}
