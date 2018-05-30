USE [DB_A3AB1C_chat]
GO

INSERT INTO [dbo].[ConversationMemberStatuses]
           ([Name])
     VALUES
           (  'None='),
       (  'Admin'),
       (  'Active'),
       (  'Blocked'),
       (  'ReadOnly'),
       (  'LeftConversation')
GO


