create trigger Contacts_UpdatedTrigger
on Contacts
for update
as
begin
    update t set LastChangeDate = SYSDATETIMEOFFSET()
    from Contacts t
        join inserted i on t.Id = i.Id
end