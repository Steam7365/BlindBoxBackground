using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlindBox.Models;
using Microsoft.EntityFrameworkCore;
namespace BlindBox.EF.ModelConfig
{
    public class GradeConfig : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.ToTable("grade", schema: "ro");
            builder.HasKey(x => x.GradeId);
            builder.Property(x => x.GradeName).HasMaxLength(20);
            builder.Property(x => x.Probability).HasColumnType("float");
            builder.HasQueryFilter(x => x.IsDelete == false);
        }
    }
}
