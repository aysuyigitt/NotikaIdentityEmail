namespace NotikaIdentityEmail.Models.MessageModels
{
    public class MesssageListWithUserInfoViewModel
    {
        public string FullName {  get; set; }
        public string ProfileImageUrl { get; set; }
        public string MessageDetail { get; set; }
        public DateTime SendDate { get; set; }
    }
}
