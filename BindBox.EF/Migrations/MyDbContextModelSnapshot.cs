﻿// <auto-generated />
using System;
using BlindBox.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlindBox.EF.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BlindBox.Models.AddressInfo", b =>
                {
                    b.Property<int>("AddressInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AddressInfoId"), 1L, 1);

                    b.Property<string>("AddressDetails")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Area")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("Phone2")
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("UserInfoId")
                        .HasColumnType("int");

                    b.HasKey("AddressInfoId");

                    b.HasIndex("UserInfoId");

                    b.ToTable("addressinfo", "ao");
                });

            modelBuilder.Entity("BlindBox.Models.BoxCommodity", b =>
                {
                    b.Property<int>("BoxCommodityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BoxCommodityId"), 1L, 1);

                    b.Property<int?>("BoxFolderId")
                        .HasColumnType("int");

                    b.Property<string>("CommodityName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("CoverPhoto")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Desc")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.HasKey("BoxCommodityId");

                    b.HasIndex("BoxFolderId");

                    b.ToTable("box_commodity", "ro");
                });

            modelBuilder.Entity("BlindBox.Models.BoxFolder", b =>
                {
                    b.Property<int>("BoxFolderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BoxFolderId"), 1L, 1);

                    b.Property<string>("BoxFolderName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.HasKey("BoxFolderId");

                    b.ToTable("box_folder", "ro");
                });

            modelBuilder.Entity("BlindBox.Models.CommdityDetail", b =>
                {
                    b.Property<int>("CommdityDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommdityDetailId"), 1L, 1);

                    b.Property<string>("ComminfoImg")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ComminfoName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal>("ComminfoPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ComminfoSpec")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int?>("GradeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<decimal?>("OfficiaPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("CommdityDetailId");

                    b.HasIndex("GradeId");

                    b.ToTable("box_commodity_detail", "ro");
                });

            modelBuilder.Entity("BlindBox.Models.DescribeType", b =>
                {
                    b.Property<int>("DescribeTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DescribeTypeId"), 1L, 1);

                    b.Property<int?>("CommdityDetailsCommdityDetailId")
                        .HasColumnType("int");

                    b.Property<string>("DescContent")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("DescTitle")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.HasKey("DescribeTypeId");

                    b.HasIndex("CommdityDetailsCommdityDetailId");

                    b.ToTable("describetype", "ro");
                });

            modelBuilder.Entity("BlindBox.Models.Draw", b =>
                {
                    b.Property<string>("DrawId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("CommdityDetailId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<int?>("UserInfoId")
                        .HasColumnType("int");

                    b.HasKey("DrawId");

                    b.HasIndex("CommdityDetailId");

                    b.HasIndex("UserInfoId");

                    b.ToTable("draw", "ro");
                });

            modelBuilder.Entity("BlindBox.Models.Grade", b =>
                {
                    b.Property<int>("GradeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GradeId"), 1L, 1);

                    b.Property<string>("GradeName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<double>("Probability")
                        .HasColumnType("float");

                    b.HasKey("GradeId");

                    b.ToTable("grade", "ro");
                });

            modelBuilder.Entity("BlindBox.Models.Login", b =>
                {
                    b.Property<int>("LoginId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LoginId"), 1L, 1);

                    b.Property<string>("LoginName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int?>("StaffId")
                        .HasColumnType("int");

                    b.HasKey("LoginId");

                    b.HasIndex("StaffId");

                    b.ToTable("login", "ro");
                });

            modelBuilder.Entity("BlindBox.Models.OrderInfo", b =>
                {
                    b.Property<int>("OrderInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderInfoId"), 1L, 1);

                    b.Property<decimal>("ActualPrice")
                        .HasColumnType("money");

                    b.Property<int?>("AddressInfoId")
                        .HasColumnType("int");

                    b.Property<string>("Channel")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Count")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("DeliverTime")
                        .HasColumnType("datetime");

                    b.Property<string>("DrawId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Freight")
                        .HasColumnType("money");

                    b.Property<string>("Order")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("OrderState")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime?>("PaymentTime")
                        .HasColumnType("datetime");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("money");

                    b.HasKey("OrderInfoId");

                    b.HasIndex("AddressInfoId");

                    b.HasIndex("DrawId");

                    b.HasIndex("Order")
                        .IsUnique();

                    b.ToTable("orderinfo", "ao");
                });

            modelBuilder.Entity("BlindBox.Models.Staff", b =>
                {
                    b.Property<int>("StaffId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StaffId"), 1L, 1);

                    b.Property<string>("Area")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("City")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Details")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Image")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Province")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("StaffCode")
                        .IsRequired()
                        .HasMaxLength(18)
                        .HasColumnType("nvarchar(18)");

                    b.Property<DateTime>("StaffEntryTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("StaffGender")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("char(2)");

                    b.Property<string>("StaffName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("StaffPhone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("StaffPosition")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("StaffState")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal>("StaffWages")
                        .HasColumnType("money");

                    b.HasKey("StaffId");

                    b.ToTable("staff", "ro");
                });

            modelBuilder.Entity("BlindBox.Models.UserInfo", b =>
                {
                    b.Property<int>("UserInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserInfoId"), 1L, 1);

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("HeadPortrait")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("UserGender")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("char(2)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("UserNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("UserPhone")
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("UserPwd")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("UserInfoId");

                    b.ToTable("userinfo", "ao");
                });

            modelBuilder.Entity("BoxCommodityCommdityDetail", b =>
                {
                    b.Property<int>("BoxCommoditiesBoxCommodityId")
                        .HasColumnType("int");

                    b.Property<int>("CommdityDetailsCommdityDetailId")
                        .HasColumnType("int");

                    b.HasKey("BoxCommoditiesBoxCommodityId", "CommdityDetailsCommdityDetailId");

                    b.HasIndex("CommdityDetailsCommdityDetailId");

                    b.ToTable("box_connect", "ro");
                });

            modelBuilder.Entity("BlindBox.Models.AddressInfo", b =>
                {
                    b.HasOne("BlindBox.Models.UserInfo", "UserInfo")
                        .WithMany("AddressInfos")
                        .HasForeignKey("UserInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserInfo");
                });

            modelBuilder.Entity("BlindBox.Models.BoxCommodity", b =>
                {
                    b.HasOne("BlindBox.Models.BoxFolder", "BoxFolder")
                        .WithMany("BoxCommodities")
                        .HasForeignKey("BoxFolderId");

                    b.Navigation("BoxFolder");
                });

            modelBuilder.Entity("BlindBox.Models.CommdityDetail", b =>
                {
                    b.HasOne("BlindBox.Models.Grade", "Grade")
                        .WithMany("CommdityDetails")
                        .HasForeignKey("GradeId");

                    b.Navigation("Grade");
                });

            modelBuilder.Entity("BlindBox.Models.DescribeType", b =>
                {
                    b.HasOne("BlindBox.Models.CommdityDetail", "CommdityDetails")
                        .WithMany("DescribeTypes")
                        .HasForeignKey("CommdityDetailsCommdityDetailId");

                    b.Navigation("CommdityDetails");
                });

            modelBuilder.Entity("BlindBox.Models.Draw", b =>
                {
                    b.HasOne("BlindBox.Models.CommdityDetail", "CommdityDetail")
                        .WithMany("Draws")
                        .HasForeignKey("CommdityDetailId");

                    b.HasOne("BlindBox.Models.UserInfo", "UserInfo")
                        .WithMany("Draws")
                        .HasForeignKey("UserInfoId");

                    b.Navigation("CommdityDetail");

                    b.Navigation("UserInfo");
                });

            modelBuilder.Entity("BlindBox.Models.Login", b =>
                {
                    b.HasOne("BlindBox.Models.Staff", "Staff")
                        .WithMany("Logins")
                        .HasForeignKey("StaffId");

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("BlindBox.Models.OrderInfo", b =>
                {
                    b.HasOne("BlindBox.Models.AddressInfo", "AddressInfo")
                        .WithMany()
                        .HasForeignKey("AddressInfoId");

                    b.HasOne("BlindBox.Models.Draw", "Draw")
                        .WithMany()
                        .HasForeignKey("DrawId");

                    b.Navigation("AddressInfo");

                    b.Navigation("Draw");
                });

            modelBuilder.Entity("BoxCommodityCommdityDetail", b =>
                {
                    b.HasOne("BlindBox.Models.BoxCommodity", null)
                        .WithMany()
                        .HasForeignKey("BoxCommoditiesBoxCommodityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlindBox.Models.CommdityDetail", null)
                        .WithMany()
                        .HasForeignKey("CommdityDetailsCommdityDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BlindBox.Models.BoxFolder", b =>
                {
                    b.Navigation("BoxCommodities");
                });

            modelBuilder.Entity("BlindBox.Models.CommdityDetail", b =>
                {
                    b.Navigation("DescribeTypes");

                    b.Navigation("Draws");
                });

            modelBuilder.Entity("BlindBox.Models.Grade", b =>
                {
                    b.Navigation("CommdityDetails");
                });

            modelBuilder.Entity("BlindBox.Models.Staff", b =>
                {
                    b.Navigation("Logins");
                });

            modelBuilder.Entity("BlindBox.Models.UserInfo", b =>
                {
                    b.Navigation("AddressInfos");

                    b.Navigation("Draws");
                });
#pragma warning restore 612, 618
        }
    }
}