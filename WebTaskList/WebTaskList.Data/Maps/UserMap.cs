using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WebTaskList.Domain.Models;

namespace WebTaskList.Data.Maps
{
    internal class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasMany(x => x.Tasks).WithRequired(x => x.User).HasForeignKey(x => x.UserId);
        }
    }
}
