create trigger Conversations_UpdatedTrigger
on Conversations
for update
as
begin
    update t set LastChange = SYSDATETIMEOFFSET()
    from Conversations t
        join inserted i on t.Id = i.Id
end