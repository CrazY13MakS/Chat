﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DbMain.EFDbContext
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ChatEntities : DbContext
    {
        public ChatEntities()
            : base("name=ChatEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AccessToken> AccessTokens { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<ConversationMember> ConversationMembers { get; set; }
        public virtual DbSet<ConversationMemberStatus> ConversationMemberStatuses { get; set; }
        public virtual DbSet<ConversationReply> ConversationReplies { get; set; }
        public virtual DbSet<ConversationReplyStatus> ConversationReplyStatuses { get; set; }
        public virtual DbSet<Conversation> Conversations { get; set; }
        public virtual DbSet<ConversationType> ConversationTypes { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<NetworkStatus> NetworkStatuses { get; set; }
        public virtual DbSet<RelationshipType> RelationshipTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
