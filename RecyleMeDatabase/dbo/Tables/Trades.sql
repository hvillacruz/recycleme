CREATE TABLE [dbo].[Trades] (
    [Id]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [ItemId]       BIGINT         NULL,
    [BuyerId]      NVARCHAR (128) NULL,
    [SellerId]     NVARCHAR (128) NULL,
    [ModifiedDate] DATETIME       NOT NULL,
    [Status]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Trades] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Trades_dbo.Items_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Items] ([Id]),
    CONSTRAINT [FK_dbo.Trades_dbo.Users_BuyerId] FOREIGN KEY ([BuyerId]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_dbo.Trades_dbo.Users_SellerId] FOREIGN KEY ([SellerId]) REFERENCES [dbo].[Users] ([UserId])
);










GO
CREATE NONCLUSTERED INDEX [IX_BuyerId]
    ON [dbo].[Trades]([BuyerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SellerId]
    ON [dbo].[Trades]([SellerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ItemId]
    ON [dbo].[Trades]([ItemId] ASC);


GO
CREATE TRIGGER [dbo].[INSERTTRADESSTRIGGER] ON [dbo].[Trades]
FOR INSERT
AS

INSERT INTO NOTIFICATIONS
       (OWNERID,SENDERID,TYPE,TITLE,URLID,ISDELETED,ISREAD,MODIFIEDDATE)
    SELECT
        SELLERID,BUYERID,4,'Trade',ID,0,0,GETDATE()
        FROM INSERTED
GO
CREATE TRIGGER [dbo].[UPDATETRADESSTRIGGER] ON [dbo].[Trades]
FOR UPDATE
AS

INSERT INTO NOTIFICATIONS
       (OWNERID,SENDERID,TYPE,TITLE,URLID,ISDELETED,ISREAD,MODIFIEDDATE)
    SELECT
        BUYERID,SELLERID,8,[Status] ,ID,0,0,GETDATE()
        FROM INSERTED