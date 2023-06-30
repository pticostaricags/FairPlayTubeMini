CREATE TABLE [dbo].[VideoInfo]
(
	[VideoInfoId] BIGINT NOT NULL CONSTRAINT PK_VideoInfo PRIMARY KEY IDENTITY, 
    [OwnerApplicationUserId] BIGINT NOT NULL, 
    [AccountId] UNIQUEIDENTIFIER NOT NULL, 
    [VideoId] NVARCHAR(50) NOT NULL, 
    [Location] NVARCHAR(50) NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(500) NOT NULL, 
    CONSTRAINT [FK_VideoInfo_ApplicationUser] FOREIGN KEY ([OwnerApplicationUserId]) REFERENCES [ApplicationUser]([ApplicationUserId])
)
