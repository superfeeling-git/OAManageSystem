using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OA.Model.Entity;

namespace OA.Model
{
    public class OADbContext : IdentityDbContext
        <OmsUser,OmsRoles,long,OmsUserClaims,OmsUserRoles,OmsUserLogins,OmsRoleClaims,OmsUserTokens>
    {

        public OADbContext(DbContextOptions<OADbContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<OmsRoleClaims>().ToTable("OmsRoleClaims");
            builder.Entity<OmsRoles>().ToTable("OmsRoles");
            builder.Entity<OmsUser>().ToTable("OmsUser");
            builder.Entity<OmsUserClaims>().ToTable("OmsUserClaims");
            builder.Entity<OmsUserLogins>().ToTable("OmsUserLogins");
            builder.Entity<OmsUserRoles>().ToTable("OmsUserRoles");
            builder.Entity<OmsUserTokens>().ToTable("OmsUserTokens");

            builder.Entity<OmsUser>(Entity => {
                Entity.Property("RefreshToken")
                    .HasMaxLength(100);
            });

            builder.Entity<OmsBlog>(entity => {
                //映射表名、配置外键及外键约束名
                entity.ToTable("OmsBlogs")
                    .HasOne("OmsBlogCategory")
                    .WithMany()
                    .HasForeignKey("CategoryID")    //外键
                    .HasConstraintName("FK_CategoryID_BlogID");     //外键约束名

                //配置主键及主键约束名
                entity.HasKey(m => m.BlogID)
                    .HasName("PK_OmsBlogID");

                //配置字段
                entity.Property(m => m.BlogTitle)
                    .HasMaxLength(50)       //配置字段长度
                    .IsFixedLength(false)   //配置定长/变长字段
                    .IsRequired()           //配置字段是否必填
                    .IsUnicode(true);       //配置是否Unicode字段

                entity.Property(m => m.AddTime)
                    .HasColumnName("CreateTime")        //配置（修改）目标字段名称
                    .HasColumnType("smalldatetime")     //配置目标字段类型
                    .HasComment("博客创建时间")         //配置字段注释
                    .IsRequired()
                    .HasDefaultValueSql("getdate()");   //配置字段默认值
            });

            builder.Entity<OmsSysMenu>(entity => {
                entity.HasKey("MenuID");
            });

           
            builder.Entity<OmsSysMenuRole>(Entity => {
                Entity.HasKey("ID");
                Entity.HasOne<OmsSysMenu>(m => m.OmsSysMenu).WithMany(o => o.OmsSysMenuRole).HasForeignKey(key => key.MenuID);
                Entity.HasOne<OmsRoles>(m => m.OmsRoles).WithMany(o => o.OmsSysMenuRole).HasForeignKey(key => key.RoleID);
            });

            builder.Entity<OmsBlogCategory>(entity => {
                entity.ToTable("OmsBlogCategory");

                entity.HasKey(e => e.CategoryID)
                    .HasName("PK_CategoryID");
            });            
        }

        //可以用于EF表查询
        public DbSet<OmsBlogCategory> OmsBlogCategory { get; set; }
        public DbSet<OmsBlog> OmsBlog { get; set; }
        public DbSet<OmsRoleClaims> OmsRoleClaims { get; set; }
        public DbSet<OmsRoles> OmsRoles { get; set; }
        public DbSet<OmsUser> OmsUser { get; set; }
        public DbSet<OmsUserClaims> OmsUserClaims { get; set; }
        public DbSet<OmsUserLogins> OmsUserLogins { get; set; }
        public DbSet<OmsUserRoles> OmsUserRoles { get; set; }
        public DbSet<OmsUserTokens> OmsUserTokens { get; set; }
        public DbSet<OmsSysMenu> OmsSysMenu { get; set; }
        public DbSet<OmsSysMenuRole> OmsSysMenuRole { get; set; }
    }
}