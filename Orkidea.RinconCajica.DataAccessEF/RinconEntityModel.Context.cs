﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Orkidea.RinconCajica.DataAccessEF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using Orkidea.RinconCajica.Entities;
    
    public partial class RinconEntities : DbContext
    {
        public RinconEntities()
            : base("name=RinconEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AccountSummary> AccountSummary { get; set; }
        public virtual DbSet<ClubPartner> ClubPartner { get; set; }
        public virtual DbSet<DocumentType> DocumentType { get; set; }
        public virtual DbSet<FileType> FileType { get; set; }
        public virtual DbSet<FileUpload> FileUpload { get; set; }
        public virtual DbSet<FrontUser> FrontUser { get; set; }
        public virtual DbSet<HomeSlider> HomeSlider { get; set; }
        public virtual DbSet<InstitutionalEvent> InstitutionalEvent { get; set; }
        public virtual DbSet<JoinContest> JoinContest { get; set; }
        public virtual DbSet<JoinEvent> JoinEvent { get; set; }
        public virtual DbSet<MessageActor> MessageActor { get; set; }
        public virtual DbSet<MessageBitacore> MessageBitacore { get; set; }
        public virtual DbSet<mimetype> mimetype { get; set; }
        public virtual DbSet<NewsPaper> NewsPaper { get; set; }
        public virtual DbSet<Page> Page { get; set; }
        public virtual DbSet<PartnerConsumption> PartnerConsumption { get; set; }
        public virtual DbSet<Process> Process { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectDocument> ProjectDocument { get; set; }
        public virtual DbSet<SideBar> SideBar { get; set; }
        public virtual DbSet<Sport> Sport { get; set; }
        public virtual DbSet<SportBranch> SportBranch { get; set; }
        public virtual DbSet<SportModality> SportModality { get; set; }
        public virtual DbSet<SportSchedule> SportSchedule { get; set; }
        public virtual DbSet<ProcessDocument> ProcessDocument { get; set; }
    }
}
