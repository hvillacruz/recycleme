CREATE TABLE [dbo].[ItemImages] (
    [Id]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [ItemId]    BIGINT         NULL,
    [Name]      NVARCHAR (MAX) NULL,
    [Path]      NVARCHAR (MAX) NULL,
    [IsDeleted] BIT            NOT NULL,
    CONSTRAINT [PK_dbo.ItemImages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ItemImages_dbo.Items_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Items] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ItemId]
    ON [dbo].[ItemImages]([ItemId] ASC);

