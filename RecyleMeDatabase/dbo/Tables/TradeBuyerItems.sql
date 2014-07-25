CREATE TABLE [dbo].[TradeBuyerItems] (
    [Id]           BIGINT   IDENTITY (1, 1) NOT NULL,
    [ItemId]       BIGINT   NULL,
    [TradeId]      BIGINT   NULL,
    [ModifiedDate] DATETIME NOT NULL,
    CONSTRAINT [PK_dbo.TradeBuyerItems] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.TradeBuyerItems_dbo.Items_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Items] ([Id]),
    CONSTRAINT [FK_dbo.TradeBuyerItems_dbo.Trades_TradeId] FOREIGN KEY ([TradeId]) REFERENCES [dbo].[Trades] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ItemId]
    ON [dbo].[TradeBuyerItems]([ItemId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TradeId]
    ON [dbo].[TradeBuyerItems]([TradeId] ASC);

