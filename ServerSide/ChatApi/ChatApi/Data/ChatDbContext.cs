using ChatApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Data
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options): base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) { 
        
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Chat>().HasOne(c => c.Sender).WithMany(u => u.SenderMessage).HasForeignKey(c => c.SenderId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chat>().HasOne(c => c.Receiver).WithMany(u => u.ReceiverMessage).HasForeignKey(c => c.ReceiverId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
