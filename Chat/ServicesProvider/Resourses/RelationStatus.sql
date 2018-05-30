USE [DB_A3AB1C_chat]
GO

INSERT INTO [dbo].[RelationshipTypes]
           ([Name])
     VALUES
           ( 'None'),
        ('Friendship'),
        ( 'FriendshipRequestSent'),
       ( 'FrienshipRequestRecive'),
       ( 'BlockedByMe'),
       ( 'BlockedByPartner'),
       ( 'BlockedBoth')
GO


