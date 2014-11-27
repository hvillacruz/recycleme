CREATE TABLE [dbo].[Items] (
    [Id]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [OwnerId]        NVARCHAR (128) NULL,
    [Name]           NVARCHAR (MAX) NULL,
    [ImagePath]      NVARCHAR (MAX) NULL,
    [Description]    NVARCHAR (MAX) NULL,
    [TradeTag]       NVARCHAR (MAX) NULL,
    [ExchangeTag]    NVARCHAR (MAX) NULL,
    [IsDeleted]      BIT            NOT NULL,
    [ModifiedDate]   DATETIME       NOT NULL,
    [ItemCategoryId] INT            NOT NULL,
    [Status]         INT            NULL,
    CONSTRAINT [PK_dbo.Items] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Items_dbo.ItemCategories_ItemCategoryId] FOREIGN KEY ([ItemCategoryId]) REFERENCES [dbo].[ItemCategories] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Items_dbo.Users_OwnerId] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[Users] ([UserId])
);








GO
CREATE NONCLUSTERED INDEX [IX_OwnerId]
    ON [dbo].[Items]([OwnerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ItemCategoryId]
    ON [dbo].[Items]([ItemCategoryId] ASC);


GO



CREATE TRIGGER [dbo].[InsertItemTrigger] ON [dbo].[Items]
FOR INSERT
AS

INSERT INTO Notifications
       (OwnerId,SenderId,Type,IsDeleted,IsRead,ModifiedDate)
    SELECT
        OwnerId,OwnerId,1,0,0,GETDATE()
        FROM inserted