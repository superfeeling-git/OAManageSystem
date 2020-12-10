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

            //创建实体关系、播种数据
            builder.Seed();
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