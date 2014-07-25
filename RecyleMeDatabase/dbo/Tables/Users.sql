CREATE TABLE [dbo].[Users] (
    [UserId]           NVARCHAR (128) NOT NULL,
    [ExternalId]       NVARCHAR (MAX) NULL,
    [ExternalUserName] NVARCHAR (MAX) NULL,
    [FirstName]        NVARCHAR (MAX) NULL,
    [LastName]         NVARCHAR (MAX) NULL,
    [Email]            NVARCHAR (MAX) NULL,
    [Address]          NVARCHAR (MAX) NULL,
    [BirthDate]        DATETIME       NULL,
    [Mobile]           NVARCHAR (MAX) NULL,
    [Avatar]           NVARCHAR (MAX) NULL,
    [BgPic]            NVARCHAR (MAX) NULL,
    [ProfileStatus]    NVARCHAR (MAX) NULL,
    [IsActive]         BIT            NOT NULL,
    [LastActivity]     DATETIME       NULL,
    [ModifiedDate]     DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

