USE [Bingo]
GO
/****** Object:  Table [dbo].[HistorialBolillero]    Script Date: 12/1/2023 00:50:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HistorialBolillero](
	[IdBolillero] [int] IDENTITY(1,1) NOT NULL,
	[FechaHora] [datetime] NULL,
	[Bolillas] [varchar](300) NULL,
 CONSTRAINT [PK_HistorialBolillero] PRIMARY KEY CLUSTERED 
(
	[IdBolillero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HistorialCartones]    Script Date: 12/1/2023 00:50:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HistorialCartones](
	[IdCartones] [int] IDENTITY(1,1) NOT NULL,
	[IdBolillero] [int] NOT NULL,
	[FechaHora] [datetime] NULL,
	[Carton1] [varchar](150) NULL,
	[Carton2] [varchar](150) NULL,
	[Carton3] [varchar](150) NULL,
	[Carton4] [varchar](150) NULL,
 CONSTRAINT [PK_HistorialCartones_1] PRIMARY KEY CLUSTERED 
(
	[IdBolillero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[HistorialCartones]  WITH CHECK ADD  CONSTRAINT [FK_HistorialCartones_HistorialBolillero] FOREIGN KEY([IdBolillero])
REFERENCES [dbo].[HistorialBolillero] ([IdBolillero])
GO
ALTER TABLE [dbo].[HistorialCartones] CHECK CONSTRAINT [FK_HistorialCartones_HistorialBolillero]
GO
