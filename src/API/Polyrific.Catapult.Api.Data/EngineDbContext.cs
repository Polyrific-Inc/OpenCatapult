// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Data.CatapultEngineIdentity;
using Polyrific.Catapult.Api.Data.EntityConfigs;
using Polyrific.Catapult.Api.Data.Identity;

namespace Polyrific.Catapult.Api.Data
{
    public class EngineDbContext : IdentityDbContext<ApplicationCatapultEngine, ApplicationRole, int>
    {
        public EngineDbContext(DbContextOptions<EngineDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectDataModel> ProjectDataModels { get; set; }
        public virtual DbSet<ProjectDataModelProperty> ProjectDataModelProperties { get; set; }
        public virtual DbSet<JobDefinition> JobDefinitions { get; set; }
        public virtual DbSet<JobTaskDefinition> JobTaskDefinitions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProjectConfig());
            modelBuilder.ApplyConfiguration(new ProjectDataModelConfig());
            modelBuilder.ApplyConfiguration(new ProjectDataModelPropertyConfig());
            modelBuilder.ApplyConfiguration(new ProjectMemberConfig());
            modelBuilder.ApplyConfiguration(new ProjectMemberRoleConfig());
            modelBuilder.ApplyConfiguration(new JobDefinitionConfig());
            modelBuilder.ApplyConfiguration(new JobTaskDefinitionConfig());
            modelBuilder.ApplyConfiguration(new ApplicationCatapultEngineConfig());
            modelBuilder.ApplyConfiguration(new ApplicationCatapultEngineLoginConfig());
            modelBuilder.ApplyConfiguration(new ApplicationCatapultEngineTokenConfig());
        }
    }
}