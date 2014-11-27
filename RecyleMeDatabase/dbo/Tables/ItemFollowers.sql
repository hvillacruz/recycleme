CREATE TABLE [dbo].[ItemFollowers] (
    [Id]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [FollowerId]     NVARCHAR (128) NULL,
    [FollowedItemId] BIGINT         NOT NULL,
    [ModifiedDate]   DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.ItemFollowers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ItemFollowers_dbo.Items_FollowedItemId] FOREIGN KEY ([FollowedItemId]) REFERENCES [dbo].[Items] ([Id]),
    CONSTRAINT [FK_dbo.ItemFollowers_dbo.Users_FollowerId] FOREIGN KEY ([FollowerId]) REFERENCES [dbo].[Users] ([UserId])
);




GO
CREATE NONCLUSTERED INDEX [IX_FollowerId]
    ON [dbo].[ItemFollowers]([FollowerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FollowedItemId]
    ON [dbo].[ItemFollowers]([FollowedItemId] ASC);


GO
CREATE TRIGGER [dbo].[INSERTITEMFOLLOWERSTRIGGER] ON [dbo].[ItemFollowers]
FOR INSERT
AS

DECLARE @OWNERID NVARCHAR(128)
SET @OWNERID = (SELECT OWNERID FROM ITEMS WHERE ID = (SELECT FollowedItemId FROM INSERTED) )

INSERT INTO NOTIFICATIONS
      (OWNERID,SENDERID,TYPE,TITLE,URLID,ISDELETED,ISREAD,MODIFIEDDATE)
    SELECT
        @OWNERID,FollowerId,7,'Like',FollowedItemId,0,0,GETDATE()
        FROM INSERTED