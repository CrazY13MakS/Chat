//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class ConversationMember
    {
        public long Id { get; set; }
        public long ConversationId { get; set; }
        public long MemberId { get; set; }
        public Nullable<long> AddedId { get; set; }
        public System.DateTimeOffset Joined { get; set; }
        public System.DateTimeOffset LastStatusChanged { get; set; }
        public int MemberStatusId { get; set; }
    
        public virtual Conversation Conversation { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual ConversationMemberStatus ConversationMemberStatus { get; set; }
    }
}
