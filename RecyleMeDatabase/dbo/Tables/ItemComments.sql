CREATE TABLE [dbo].[ItemComments] (
    [Id]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [CommenterId]     NVARCHAR (128) NULL,
    [CommentedItemId] BIGINT         NOT NULL,
    [ModifiedDate]    DATETIME       NOT NULL,
    [Comment]         NVARCHAR (MAX) NULL,
    [IsDeleted]       BIT            NOT NULL,
    CONSTRAINT [PK_dbo.ItemComments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ItemComments_dbo.Items_CommentedItemId] FOREIGN KEY ([CommentedItemId]) REFERENCES [dbo].[Items] ([Id]),
    CONSTRAINT [FK_dbo.ItemComments_dbo.Users_CommenterId] FOREIGN KEY ([CommenterId]) REFERENCES [dbo].[Users] ([UserId])
);






GO
CREATE NONCLUSTERED INDEX [IX_CommenterId]
    ON [dbo].[ItemComments]([CommenterId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CommentedItemId]
    ON [dbo].[ItemComments]([CommentedItemId] ASC);


GO
CREATE TRIGGER [dbo].[INSERTITEMCOMMENTSSTRIGGER] ON [dbo].[ItemComments]
FOR INSERT
AS

DECLARE @OWNERID NVARCHAR(128)
SET @OWNERID = (SELECT OWNERID FROM ITEMS WHERE ID = (SELECT COMMENTEDITEMID FROM INSERTED) )

INSERT INTO NOTIFICATIONS
      (OWNERID,SENDERID,TYPE,TITLE,URLID,ISDELETED,ISREAD,MODIFIEDDATE)
    SELECT
        @OWNERID,COMMENTERID,3,COMMENT,COMMENTERID,0,0,GETDATE()
        FROM INSERTED