﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlindBox.Models;
using Microsoft.EntityFrameworkCore;
namespace BlindBox.EF.ModelConfig
{
    public class DrawConfig : IEntityTypeConfiguration<Draw>
    {
        public void Configure(EntityTypeBuilder<Draw> builder)
        {
            builder.ToTable("draw", schema: "ro");
            builder.HasKey(x=>x.DrawId);
            builder.HasOne<CommdityDetail>(x=>x.CommdityDetail).WithMany(x=>x.Draws);
            builder.HasOne<UserInfo>(x => x.UserInfo).WithMany(x => x.Draws);
        }
    }
}
