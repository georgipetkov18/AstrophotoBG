namespace AstrophotoBG.Data.Models
{
    public class UserPicture
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int PictureId { get; set; }
        public Picture Picture { get; set; }
    }
}
