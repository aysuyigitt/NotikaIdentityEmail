namespace NotikaIdentityEmail.Entities
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string CommentDetail { get; set; }
        public DateTime CommentDate { get; set; }
        public string CommentStatus { get; set; }
        public string AppUserID { get; set; }
        public AppUser AppUser {  get; set; }
    }
}
