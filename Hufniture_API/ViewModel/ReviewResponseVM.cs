namespace Hufniture_API.ViewModel
{
    public class ReviewResponseVM
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserFullName { get; set; }
        public string UserId { get; set; }
    }
}
