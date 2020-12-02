

CREATE TABLE [dbo].[WexinLoger](
	[LogId] [bigint] IDENTITY(1,1) NOT NULL,
	[Level] [varchar](50) NOT NULL,
	[Logger] [varchar](50) NOT NULL,
	[Message] [ntext] NOT NULL,
	[Exception] [text] NOT NULL,
	[LogDate] [datetime] NOT NULL,
 CONSTRAINT [PK_T_Rpg_Summer_WexinLoger_Log4Net] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[WexinLoger] ADD  CONSTRAINT [DF_T_Rpg_Summer_WexinLoger_Log4Net_Level]  DEFAULT ('') FOR [Level]
GO

ALTER TABLE [dbo].[WexinLoger] ADD  CONSTRAINT [DF_T_Rpg_Summer_WexinLoger_Log4Net_Logger]  DEFAULT ('') FOR [Logger]
GO

ALTER TABLE [dbo].[WexinLoger] ADD  CONSTRAINT [DF_T_Rpg_Summer_WexinLoger_Log4Net_Message]  DEFAULT ('') FOR [Message]
GO

ALTER TABLE [dbo].[WexinLoger] ADD  CONSTRAINT [DF_T_Rpg_Summer_WexinLoger_Log4Net_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

