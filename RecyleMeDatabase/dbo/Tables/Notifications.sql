CREATE TABLE [dbo].[Notifications] (
    [Id]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [OwnerId]      NVARCHAR (128) NULL,
    [Title]        NVARCHAR (MAX) NULL,
    [Type]         INT            NOT NULL,
    [UrlId]        NVARCHAR (MAX) NULL,
    [IsRead]       BIT            NOT NULL,
    [IsDeleted]    BIT            NOT NULL,
    [ModifiedDate] DATETIME       NOT NULL,
    [SenderId]     NVARCHAR (128) NULL,
    CONSTRAINT [PK_dbo.Notifications] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Notifications_dbo.Users_OwnerId] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_dbo.Notifications_dbo.Users_SenderId] FOREIGN KEY ([SenderId]) REFERENCES [dbo].[Users] ([UserId])
);




GO
CREATE NONCLUSTERED INDEX [IX_OwnerId]
    ON [dbo].[Notifications]([OwnerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SenderId]
    ON [dbo].[Notifications]([SenderId] ASC);

