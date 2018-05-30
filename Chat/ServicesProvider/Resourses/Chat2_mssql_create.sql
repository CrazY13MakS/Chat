--CREATE DATABASE Chat
--GO
--USE Chat
--GO

CREATE TABLE [RelationshipTypes] (
	Id int NOT NULL IDENTITY(0,1),
	Name nvarchar(50) NOT NULL UNIQUE,
  CONSTRAINT [PK_RELATIONSHIPTYPES] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Users] (
	Id bigint NOT NULL IDENTITY,
	Email nvarchar(250) NOT NULL UNIQUE,
	Name nvarchar(50) NOT NULL UNIQUE,
	Phone nvarchar(20),
	PasswordHash binary(32) NOT NULL,
	PasswordSalt binary(20) NOT NULL,
	Birthdate date,
	CityId bigint,
	FriendsCount int NOT NULL DEFAULT 0,
	Icon varbinary(max),
	GenderId int,
	Status nvarchar(100),
	LastEdit datetimeoffset NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	Login nvarchar(50) NOT NULL UNIQUE,
	RegistrationDate datetimeoffset NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	NetworkStatusId int NOT NULL,
  CONSTRAINT [PK_USERS] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [ConversationReplies] (
	Id bigint NOT NULL IDENTITY,
	AuthorId bigint NOT NULL,
	ConversationId bigint NOT NULL,
	Body nvarchar(max) NOT NULL,
	SendingTime datetimeoffset NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	IsDeleted bit NOT NULL DEFAULT 0,
	ConversationReplyStatusId int NOT NULL DEFAULT 1,
  CONSTRAINT [PK_CONVERSATIONREPLIES] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Conversations] (
	Id bigint NOT NULL IDENTITY,
	AuthorId bigint NOT NULL,
	PartnerId bigint,
	Name nvarchar(250) NOT NULL DEFAULT 'New Conversation',
	Description nvarchar(250),
	Created datetimeoffset NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	LastChange datetimeoffset NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	Icon varbinary(max),
	ConversationTypeId int NOT NULL DEFAULT 1,
  CONSTRAINT [PK_CONVERSATIONS] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [ConversationMembers] (
	Id bigint NOT NULL IDENTITY,
	ConversationId bigint NOT NULL,
	MemberId bigint NOT NULL,
	AddedId bigint,
	Joined datetimeoffset NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	LastStatusChanged datetimeoffset NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	MemberStatusId int NOT NULL DEFAULT 1,
  CONSTRAINT [PK_CONVERSATIONMEMBERS] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Contacts] (
	Id bigint NOT NULL IDENTITY,
	AdderId bigint NOT NULL,
	InvitedId bigint NOT NULL,
	SendingDate datetimeoffset NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	RelationTypeId int NOT NULL DEFAULT 1,
	LastChangeDate datetimeoffset DEFAULT SYSDATETIMEOFFSET(),
	ConversationId bigint NOT NULL,
  CONSTRAINT [PK_CONTACTS] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Genders] (
	Id int NOT NULL IDENTITY(0,1),
	Name nvarchar(50) NOT NULL UNIQUE,
  CONSTRAINT [PK_GENDERS] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Countries] (
	Id int NOT NULL IDENTITY,
	Name nvarchar(250) NOT NULL UNIQUE,
  CONSTRAINT [PK_COUNTRIES] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Cities] (
	Id bigint NOT NULL IDENTITY,
	Name nvarchar(250) NOT NULL,
	CountryId int NOT NULL,
  CONSTRAINT [PK_CITIES] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [ConversationMemberStatuses] (
	Id int NOT NULL IDENTITY(0,1),
	Name nvarchar(50) NOT NULL UNIQUE,
  CONSTRAINT [PK_CONVERSATIONMEMBERSTATUSES] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [ConversationReplyStatuses] (
	Id int NOT NULL IDENTITY(0,1),
	Name nvarchar(50) NOT NULL UNIQUE,
  CONSTRAINT [PK_CONVERSATIONREPLYSTATUSES] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [AccessTokens] (
	Id bigint NOT NULL IDENTITY,
	UserId bigint NOT NULL,
	Token nvarchar(100) NOT NULL UNIQUE,
	Created datetimeoffset NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	IsActive bit NOT NULL DEFAULT 1,
  CONSTRAINT [PK_ACCESSTOKENS] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [ConversationTypes] (
	Id int NOT NULL IDENTITY(0,1),
	Name nvarchar(50) NOT NULL UNIQUE,
  CONSTRAINT [PK_CONVERSATIONTYPES] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [NetworkStatuses] (
	Id int NOT NULL IDENTITY(0,1),
	Name nvarchar(50) NOT NULL UNIQUE,
  CONSTRAINT [PK_NETWORKSTATUSES] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [BlokedUsers] (
	Id bigint NOT NULL IDENTITY,
	AdderId bigint NOT NULL,
	BlockedUserId bigint NOT NULL,
	Created datetimeoffset NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	LastChange datetimeoffset NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	IsActive bit NOT NULL DEFAULT 1,
  CONSTRAINT [PK_BLOKEDUSERS] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO

ALTER TABLE [Users] WITH CHECK ADD CONSTRAINT [Users_fk0] FOREIGN KEY ([CityId]) REFERENCES [Cities]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [Users] CHECK CONSTRAINT [Users_fk0]
GO
ALTER TABLE [Users] WITH CHECK ADD CONSTRAINT [Users_fk1] FOREIGN KEY ([GenderId]) REFERENCES [Genders]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [Users] CHECK CONSTRAINT [Users_fk1]
GO
ALTER TABLE [Users] WITH CHECK ADD CONSTRAINT [Users_fk2] FOREIGN KEY ([NetworkStatusId]) REFERENCES [NetworkStatuses]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [Users] CHECK CONSTRAINT [Users_fk2]
GO

ALTER TABLE [ConversationReplies] WITH CHECK ADD CONSTRAINT [ConversationReplies_fk0] FOREIGN KEY ([AuthorId]) REFERENCES [Users]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [ConversationReplies] CHECK CONSTRAINT [ConversationReplies_fk0]
GO
ALTER TABLE [ConversationReplies] WITH CHECK ADD CONSTRAINT [ConversationReplies_fk1] FOREIGN KEY ([ConversationId]) REFERENCES [Conversations]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [ConversationReplies] CHECK CONSTRAINT [ConversationReplies_fk1]
GO
ALTER TABLE [ConversationReplies] WITH CHECK ADD CONSTRAINT [ConversationReplies_fk2] FOREIGN KEY ([ConversationReplyStatusId]) REFERENCES [ConversationReplyStatuses]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [ConversationReplies] CHECK CONSTRAINT [ConversationReplies_fk2]
GO

ALTER TABLE [Conversations] WITH CHECK ADD CONSTRAINT [Conversations_fk0] FOREIGN KEY ([AuthorId]) REFERENCES [Users]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [Conversations] CHECK CONSTRAINT [Conversations_fk0]
GO
ALTER TABLE [Conversations] WITH CHECK ADD CONSTRAINT [Conversations_fk1] FOREIGN KEY ([PartnerId]) REFERENCES [Users]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [Conversations] CHECK CONSTRAINT [Conversations_fk1]
GO
ALTER TABLE [Conversations] WITH CHECK ADD CONSTRAINT [Conversations_fk2] FOREIGN KEY ([ConversationTypeId]) REFERENCES [ConversationTypes]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [Conversations] CHECK CONSTRAINT [Conversations_fk2]
GO

ALTER TABLE [ConversationMembers] WITH CHECK ADD CONSTRAINT [ConversationMembers_fk0] FOREIGN KEY ([ConversationId]) REFERENCES [Conversations]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [ConversationMembers] CHECK CONSTRAINT [ConversationMembers_fk0]
GO
ALTER TABLE [ConversationMembers] WITH CHECK ADD CONSTRAINT [ConversationMembers_fk1] FOREIGN KEY ([MemberId]) REFERENCES [Users]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [ConversationMembers] CHECK CONSTRAINT [ConversationMembers_fk1]
GO
ALTER TABLE [ConversationMembers] WITH CHECK ADD CONSTRAINT [ConversationMembers_fk2] FOREIGN KEY ([AddedId]) REFERENCES [Users]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [ConversationMembers] CHECK CONSTRAINT [ConversationMembers_fk2]
GO
ALTER TABLE [ConversationMembers] WITH CHECK ADD CONSTRAINT [ConversationMembers_fk3] FOREIGN KEY ([MemberStatusId]) REFERENCES [ConversationMemberStatuses]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [ConversationMembers] CHECK CONSTRAINT [ConversationMembers_fk3]
GO

ALTER TABLE [Contacts] WITH CHECK ADD CONSTRAINT [Contacts_fk0] FOREIGN KEY ([AdderId]) REFERENCES [Users]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [Contacts] CHECK CONSTRAINT [Contacts_fk0]
GO
ALTER TABLE [Contacts] WITH CHECK ADD CONSTRAINT [Contacts_fk1] FOREIGN KEY ([InvitedId]) REFERENCES [Users]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [Contacts] CHECK CONSTRAINT [Contacts_fk1]
GO
ALTER TABLE [Contacts] WITH CHECK ADD CONSTRAINT [Contacts_fk2] FOREIGN KEY ([RelationTypeId]) REFERENCES [RelationshipTypes]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [Contacts] CHECK CONSTRAINT [Contacts_fk2]
GO
ALTER TABLE [Contacts] WITH CHECK ADD CONSTRAINT [Contacts_fk3] FOREIGN KEY ([ConversationId]) REFERENCES [Conversations]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [Contacts] CHECK CONSTRAINT [Contacts_fk3]
GO



ALTER TABLE [Cities] WITH CHECK ADD CONSTRAINT [Cities_fk0] FOREIGN KEY ([CountryId]) REFERENCES [Countries]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [Cities] CHECK CONSTRAINT [Cities_fk0]
GO



ALTER TABLE [AccessTokens] WITH CHECK ADD CONSTRAINT [AccessTokens_fk0] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [AccessTokens] CHECK CONSTRAINT [AccessTokens_fk0]
GO



ALTER TABLE [BlokedUsers] WITH CHECK ADD CONSTRAINT [BlokedUsers_fk0] FOREIGN KEY ([AdderId]) REFERENCES [Users]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [BlokedUsers] CHECK CONSTRAINT [BlokedUsers_fk0]
GO
ALTER TABLE [BlokedUsers] WITH CHECK ADD CONSTRAINT [BlokedUsers_fk1] FOREIGN KEY ([BlockedUserId]) REFERENCES [Users]([Id])
--ON UPDATE CASCADE
GO
ALTER TABLE [BlokedUsers] CHECK CONSTRAINT [BlokedUsers_fk1]
GO

