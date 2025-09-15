namespace ChatApi.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Userame { get; set; }

        public string Password { get; set; }

        public ICollection<Chat> SenderMessage { get; set; }
        public ICollection<Chat> ReceiverMessage { get; set; }
    }
}
