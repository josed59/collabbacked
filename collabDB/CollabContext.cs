using collabDB.Fluent;
using collabDB.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace collabDB
{
    public class CollabContext : DbContext
    {
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<User>Users { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<TeamsMembers> TeamsMembers { get; set; }
        public DbSet<UsersTask> UsersTasks { get; set; }
        public DbSet<TaskState> TaskStates { get; set; }
        public DbSet<TaskSize> TaskSizes { get; set; }
        public DbSet<TaskChangeLogs> TaskChangeLogs { get; set; }





        public CollabContext(DbContextOptions<CollabContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserType>(userType =>
            {
                userType.ToTable("Usertype");
                userType.HasKey(p => p.UserTypeId);
                userType.Property(p => p.Description).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<User>(user =>
            {
                user.ToTable("User");
                user.HasKey(p => p.UserId);
                user.HasOne(p => p.UserType).WithMany(p => p.Users).HasForeignKey(p => p.UserTypeId);
                user.Property(p => p.Name).IsRequired().HasMaxLength(150);
                user.Property(p => p.Email).IsRequired().HasMaxLength(250);
                user.Property(p => p.Password).IsRequired().HasMaxLength(500);
                user.Property(p => p.Capacity);
            });

            modelBuilder.Entity<Teams>(teams => {
                teams.ToTable("Teams");
                teams.HasKey(p => p.TeamId);
                teams.Property(t => t.Name).IsRequired();
                teams.HasOne(t => t.User).WithMany(u => u.Teams).HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Restrict);
                teams.HasMany(t => t.TeamsMembers).WithOne(tm => tm.Teams).HasForeignKey(tm => tm.TeamId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TeamsMembers>(tm => {
                tm.ToTable("TeamsMembers");
                tm.HasKey(p => p.TeamsMembersId);
                tm.HasOne(tm => tm.Teams).WithMany(t => t.TeamsMembers).HasForeignKey(tm => tm.TeamId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.ApplyConfiguration(new UsersTaskConfiguration());

            //modelBuilder.Entity<UsersTask>(entity =>
            //{
            //    entity.ToTable("UsersTask");
            //    entity.HasKey(e => e.TaskId);
            //    entity.Property(e => e.Description).IsRequired();
            //    entity.Property(e => e.Title).IsRequired();
            //    entity.Property(e => e.CreationDate).IsRequired().HasDefaultValueSql("GetUtcDate()"); 
            //    entity.Property(e => e.From).IsRequired();
            //    entity.Property(e => e.To).IsRequired();
            //    entity.Property(e => e.UserTest).IsRequired().HasDefaultValue(true);
            //    entity.HasOne(e => e.TaskSize)
            //          .WithMany(ts => ts.UsersTask)
            //          .HasForeignKey(e => e.TaskSizeId)
            //          .OnDelete(DeleteBehavior.Restrict);

            //    entity.HasOne(e => e.User)
            //          .WithMany(u => u.UsersTasks)
            //          .HasForeignKey(e => e.OwnerId)
            //          .OnDelete(DeleteBehavior.Restrict);

            //    entity.HasOne(e => e.User)
            //          .WithMany(u => u.UsersTasks)
            //          .HasForeignKey(e => e.UserId)
            //          .OnDelete(DeleteBehavior.Restrict);
            //    entity.Property(e => e.UserId).IsRequired(false);

            //    entity.HasOne(e => e.TaskState)
            //          .WithMany(ts => ts.UsersTask)
            //          .HasForeignKey(e => e.TaskStateId)
            //          .OnDelete(DeleteBehavior.Restrict);
            //});

            //Data
            modelBuilder.Entity<TaskState>().HasData(
              new TaskState { TaskStateId = 1, Description = "Pending" },
              new TaskState { TaskStateId = 2, Description = "InProgress" },
              new TaskState { TaskStateId = 3, Description = "Completed" },
              new TaskState { TaskStateId = 4, Description = "Delayed" },
              new TaskState { TaskStateId = 5, Description = "StandBy" },
              new TaskState { TaskStateId = 6, Description = "Deleted" }



          );

            modelBuilder.Entity<TaskSize>().HasData(
                new TaskSize { TaskSizeId = 1, TaskDescription = "XS", Weigth = 5 },
                new TaskSize { TaskSizeId = 2, TaskDescription = "S", Weigth =  12},
                new TaskSize { TaskSizeId = 3, TaskDescription = "M", Weigth =  25 },
                new TaskSize { TaskSizeId = 4, TaskDescription = "L", Weigth =  50},
                new TaskSize { TaskSizeId = 5, TaskDescription = "XL", Weigth =  100 }

            );


            modelBuilder.Entity<TaskState>(entity =>
            {
                entity.ToTable("TaskState");
                entity.HasKey(e => e.TaskStateId);
                entity.Property(e => e.Description).IsRequired();
                entity.HasMany(e => e.UsersTask)
                      .WithOne(ut => ut.TaskState)
                      .HasForeignKey(ut => ut.TaskStateId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TaskSize>(entity =>
            {
                entity.ToTable("TaskSize");
                entity.HasKey(e => e.TaskSizeId);
                entity.Property(e => e.TaskDescription).IsRequired();
                entity.Property(e => e.Weigth).IsRequired();
                entity.HasMany(e => e.UsersTask)
                      .WithOne(ut => ut.TaskSize)
                      .HasForeignKey(ut => ut.TaskSizeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.ApplyConfiguration(new TaskChangeLogsConfiguration());


        }
    }
}