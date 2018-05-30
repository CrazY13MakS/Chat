create trigger Users_UpdatedTrigger
on Users
for update
as
begin
    update t set LastEdit = SYSDATETIMEOFFSET()
    from Users t
        join inserted i on t.Id = i.Id
end