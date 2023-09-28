using collabDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace collabDB.Fluent
{
    internal class UsersTaskConfiguration : IEntityTypeConfiguration<UsersTask>
    {
        public void Configure(EntityTypeBuilder<UsersTask> builder)
        {
            builder.ToTable("UsersTask");
            builder.HasKey(e => e.TaskId);
            builder.Property(e => e.Description).IsRequired();
            builder.Property(e => e.Title).IsRequired();
            builder.Property(e => e.CreationDate).IsRequired().HasDefaultValueSql("GetUtcDate()");
            builder.Property(e => e.From).IsRequired();
            builder.Property(e => e.To).IsRequired();
            builder.Property(e => e.UserTest).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.QaDateFinished).IsRequired(false);
            builder.Property(e => e.CloseDate).IsRequired(false);
            builder.HasOne(e => e.TaskSize)
                  .WithMany(ts => ts.UsersTask)
                  .HasForeignKey(e => e.TaskSizeId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.User)
                  .WithMany(u => u.UsersTasks)
                  .HasForeignKey(e => e.OwnerId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.User)
                  .WithMany(u => u.UsersTasks)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
            builder.Property(e => e.UserId).IsRequired(false);

            builder.HasOne(e => e.TaskState)
                  .WithMany(ts => ts.UsersTask)
                  .HasForeignKey(e => e.TaskStateId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
