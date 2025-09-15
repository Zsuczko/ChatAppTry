namespace ChatApi.Models
{
    public class Chat
    {

        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime TimeStamp { get; set; }


        public int SenderId { get; set; }
        public User Sender { get; set; }

        public int ReceiverId { get; set; }
        public User Receiver { get; set; }
    }
}
