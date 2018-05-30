create trigger ConversationMembers_UpdatedTrigger
on ConversationMembers
for update
as
begin
    update t set LastStatusChanged = SYSDATETIMEOFFSET()
    from ConversationMembers t
        join inserted i on t.Id = i.Id
end