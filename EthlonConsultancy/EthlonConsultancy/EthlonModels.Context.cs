﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EthlonConsultancy
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EthlonEntities : DbContext
    {
        public EthlonEntities()
            : base("name=EthlonEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<AdminType> AdminTypes { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Organizationscv> Organizationscvs { get; set; }
        public virtual DbSet<Partner> Partners { get; set; }
        public virtual DbSet<Partnerscv> Partnerscvs { get; set; }
        public virtual DbSet<PostJob> PostJobs { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<OrganizationGpaView> OrganizationGpaViews { get; set; }
        public virtual DbSet<OrganizationQualView> OrganizationQualViews { get; set; }
        public virtual DbSet<PartnersGpaView> PartnersGpaViews { get; set; }
        public virtual DbSet<PartnersQualView> PartnersQualViews { get; set; }
        public virtual DbSet<Applicantscv> Applicantscvs { get; set; }
        public virtual DbSet<ApplicantBBAView> ApplicantBBAViews { get; set; }
        public virtual DbSet<ApplicantCAView> ApplicantCAViews { get; set; }
        public virtual DbSet<EmployeLogo> EmployeLogos { get; set; }
        public virtual DbSet<ApplicantpartnerACCAView> ApplicantpartnerACCAViews { get; set; }
        public virtual DbSet<ApplicantpartnerBBAView> ApplicantpartnerBBAViews { get; set; }
        public virtual DbSet<Applicantpartnerscv> Applicantpartnerscvs { get; set; }
        public virtual DbSet<Applicant> Applicants { get; set; }
    }
}
