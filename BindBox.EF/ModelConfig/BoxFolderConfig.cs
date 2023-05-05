using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlindBox.Models;
using Microsoft.EntityFrameworkCore;
namespace BlindBox.EF.ModelConfig
{
    public class BoxFolderConfig : IEntityTypeConfiguration<BoxFolder>
    {
        public void Configure(EntityTypeBuilder<BoxFolder> builder)
        {
            builder.ToTable("box_folder", schema: "ro");
            builder.HasKey(x => x.BoxFolderId);
            builder.Property(x => x.BoxFolderName).HasMaxLength(20);
            builder.HasQueryFilter(x => x.IsDelete == false);
        }
    }
}
