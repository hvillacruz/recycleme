CREATE TABLE [dbo].[TradeComments] (
    [Id]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [Comment]          NVARCHAR (MAX) NULL,
    [TradeId]          BIGINT         NULL,
    [TradeCommenterId] NVARCHAR (128) NULL,
    [ModifiedDate]     DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.TradeComments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.TradeComments_dbo.Trades_TradeId] FOREIGN KEY ([TradeId]) REFERENCES [dbo].[Trades] ([Id]),
    CONSTRAINT [FK_dbo.TradeComments_dbo.Users_TradeCommenterId] FOREIGN KEY ([TradeCommenterId]) REFERENCES [dbo].[Users] ([UserId])
);




GO
CREATE NONCLUSTERED INDEX [IX_TradeId]
    ON [dbo].[TradeComments]([TradeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TradeCommenterId]
    ON [dbo].[TradeComments]([TradeCommenterId] ASC);


GO
CREATE TRIGGER [DBO].[INSERTTRADECOMMENTSSTRIGGER] ON [DBO].[TRADECOMMENTS]
FOR INSERT
AS

DECLARE @OWNERID NVARCHAR(128)
SET @OWNERID = (SELECT SELLERID FROM TRADES WHERE ID = (SELECT TRADEID FROM INSERTED) )

INSERT INTO NOTIFICATIONS
       (OWNERID,TYPE,TITLE,URLID,ISDELETED,ISREAD,MODIFIEDDATE)
    SELECT
        @OWNERID,5,COMMENT,TRADEID,0,0,GETDATE()
        FROM INSERTED