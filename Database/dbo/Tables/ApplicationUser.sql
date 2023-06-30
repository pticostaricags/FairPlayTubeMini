CREATE TABLE [dbo].[ApplicationUser]
(
	[ApplicationUserId] BIGINT NOT NULL CONSTRAINT PK_ApplicationUser PRIMARY KEY IDENTITY, 
    [Username] NVARCHAR(50) NOT NULL, 
    [ProviderUserId] NVARCHAR(50) NOT NULL
)
