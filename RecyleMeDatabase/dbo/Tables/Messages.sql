CREATE TABLE [dbo].[Messages] (
    [Id]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [SenderId]     NVARCHAR (128) NULL,
    [ReceiverId]   NVARCHAR (128) NULL,
    [Subject]      NVARCHAR (MAX) NULL,
    [Body]         NVARCHAR (MAX) NULL,
    [DateSent]     DATETIME       NULL,
    [DateReceived] DATETIME       NULL,
    [IsDeleted]    BIT            NOT NULL,
    [ParentId]     BIGINT         NULL,
    CONSTRAINT [PK_dbo.Messages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Messages_dbo.Messages_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Messages] ([Id]),
    CONSTRAINT [FK_dbo.Messages_dbo.Users_ReceiverId] FOREIGN KEY ([ReceiverId]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_dbo.Messages_dbo.Users_SenderId] FOREIGN KEY ([SenderId]) REFERENCES [dbo].[Users] ([UserId])
);








GO
CREATE NONCLUSTERED INDEX [IX_SenderId]
    ON [dbo].[Messages]([SenderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ReceiverId]
    ON [dbo].[Messages]([ReceiverId] ASC);


GO


CREATE TRIGGER [DBO].[INSERTMESSAGESTRIGGER] ON [DBO].[MESSAGES]
FOR INSERT
AS

INSERT INTO NOTIFICATIONS
       (OWNERID,SenderId,TYPE,TITLE,URLID,ISDELETED,ISREAD,MODIFIEDDATE)
    SELECT
        ReceiverId,SENDERID,1,SUBJECT,ID,0,0,DateSent
        FROM INSERTED
GO
CREATE NONCLUSTERED INDEX [IX_ParentId]
    ON [dbo].[Messages]([ParentId] ASC);

