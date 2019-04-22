// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Data.EntityConfigs;
using Polyrific.Catapult.Api.Data.Identity;

namespace Polyrific.Catapult.Api.Data
{
    public class CatapultDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        public CatapultDbContext(DbContextOptions<CatapultDbContext> options)
            : base(options)
        {
            
        }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectDataModel> ProjectDataModels { get; set; }
        public virtual DbSet<ProjectDataModelProperty> ProjectDataModelProperties { get; set; }
        public virtual DbSet<ProjectMember> ProjectMembers { get; set; }
        public virtual DbSet<ProjectMemberRole> ProjectMemberRoles { get; set; }
        public virtual DbSet<JobDefinition> JobDefinitions { get; set; }
        public virtual DbSet<JobTaskDefinition> JobTaskDefinitions { get; set; }
        public virtual DbSet<JobQueue> JobQueues { get; set; }
        public virtual DbSet<JobCounter> JobCounters { get; set; }
        public virtual DbSet<ExternalService> ExternalServices { get; set; }
        public virtual DbSet<ExternalServiceType> ExternalServiceTypes { get; set; }
        public virtual DbSet<ExternalServiceProperty> ExternalServiceProperties { get; set; }
        public virtual DbSet<TaskProvider> TaskProviders { get; set; }
        public virtual DbSet<TaskProviderAdditionalConfig> TaskProviderAdditionalConfigs { get; set; }
        public virtual DbSet<ManagedFile> ManagedFiles { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<TaskProviderTag> TaskProviderTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProjectConfig());
            modelBuilder.ApplyConfiguration(new ProjectDataModelConfig());
            modelBuilder.ApplyConfiguration(new ProjectDataModelPropertyConfig());
            modelBuilder.ApplyConfiguration(new ProjectMemberConfig());
            modelBuilder.ApplyConfiguration(new ProjectMemberRoleConfig());
            modelBuilder.ApplyConfiguration(new ApplicationUserConfig());
            modelBuilder.ApplyConfiguration(new ApplicationRoleConfig());
            modelBuilder.ApplyConfiguration(new ApplicationUserRoleConfig());
            modelBuilder.ApplyConfiguration(new ApplicationUserClaimConfig());
            modelBuilder.ApplyConfiguration(new ApplicationUserLoginConfig());
            modelBuilder.ApplyConfiguration(new ApplicationRoleClaimConfig());
            modelBuilder.ApplyConfiguration(new ApplicationUserTokenConfig());
            modelBuilder.ApplyConfiguration(new UserProfileConfig());
            modelBuilder.ApplyConfiguration(new CatapultEngineProfileConfig());
            modelBuilder.ApplyConfiguration(new JobDefinitionConfig());
            modelBuilder.ApplyConfiguration(new JobTaskDefinitionConfig());
            modelBuilder.ApplyConfiguration(new JobQueueConfig());
            modelBuilder.ApplyConfiguration(new JobCounterConfig());
            modelBuilder.ApplyConfiguration(new ExternalServiceConfig());
            modelBuilder.ApplyConfiguration(new ExternalServiceTypeConfig());
            modelBuilder.ApplyConfiguration(new ExternalServicePropertyConfig());
            modelBuilder.ApplyConfiguration(new TaskProviderConfig());
            modelBuilder.ApplyConfiguration(new TaskProviderAdditionalConfigConfig());
            modelBuilder.ApplyConfiguration(new ManagedFileConfig());
            modelBuilder.ApplyConfiguration(new TagConfig());
            modelBuilder.ApplyConfiguration(new TaskProviderTagConfig());
        }
    }
}
