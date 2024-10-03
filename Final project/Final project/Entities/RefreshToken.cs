namespace Final_project.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }  // Foreign key to ApplicationUser
        public DateTime Expiration { get; set; }

        public string UserAgent { get; set; }

        // Navigation property
        public virtual UserEntity UserEntity { get; set; }
    }
}
