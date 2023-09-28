using collabDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TaskChangeLogsConfiguration : IEntityTypeConfiguration<TaskChangeLogs>
{
    public void Configure(EntityTypeBuilder<TaskChangeLogs> builder)
    {
        builder.ToTable("TaskChangeLogs"); // Nombre de la tabla

        builder.HasKey(tc => tc.Id); // Clave primaria

        builder.Property(tc => tc.Id)
            .ValueGeneratedOnAdd(); // Generación automática del valor en la inserción

        builder.Property(tc => tc.Comment)
            .IsRequired();

        builder.Property(tc => tc.TaskId)
            .IsRequired();


        builder.Property(tc => tc.Date)
            .IsRequired().HasDefaultValueSql("GetUtcDate()");

        builder.Property(tc => tc.IsCompleted)
            .IsRequired().HasDefaultValue(false); 

        builder.Property(tc => tc.isUatCompleted)
            .IsRequired().HasDefaultValue(false);

        builder.Property(tc => tc.userid)
            .IsRequired();

        // Relación con la entidad User
        builder.HasOne(tc => tc.user)
            .WithMany(u => u.TaskChangeLogs)
            .HasForeignKey(tc => tc.userid)
            .OnDelete(DeleteBehavior.Restrict);

        //Relacion con usersTaks
        builder.HasOne(tc => tc.UsersTask)
            .WithMany(u => u.TaskChangeLogs)
            .HasForeignKey(tc => tc.TaskId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
