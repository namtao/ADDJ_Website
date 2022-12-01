/*
 Navicat Premium Data Transfer

 Source Server         : .
 Source Server Type    : SQL Server
 Source Server Version : 15002000
 Source Catalog        : ADDJ_DB
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 15002000
 File Encoding         : 65001

 Date: 10/02/2022 16:54:34
*/


-- ----------------------------
-- Table structure for THONGTINCAUHINH
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[THONGTINCAUHINH]') AND type IN ('U'))
	DROP TABLE [dbo].[THONGTINCAUHINH]
GO

CREATE TABLE [dbo].[THONGTINCAUHINH] (
  [TenCoQuan] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [DiaChiCoQuan] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [DienThoaiCoQuan] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [FaxCoQuan] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [EmailCoQuan] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [WebCoQuan] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [LogoCoQuan] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [NamChinhLy] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [TenMucLucHoSo] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [TenLanChinhLy] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [MaDVHinhThanhPhong] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [MaPhong] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [NamChinhLyPhong] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [HienThiLogoADDJ] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot1] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot2] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot3] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot4] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot5] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot6] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot7] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot8] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot9] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot10] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot11] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot12] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot13] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot14] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot15] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot16] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot17] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot18] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot19] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot20] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot21] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot22] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot23] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [image_logo] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot24] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot25] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot26] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot27] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot28] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot29] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cot30] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[THONGTINCAUHINH] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of THONGTINCAUHINH
-- ----------------------------
INSERT INTO [dbo].[THONGTINCAUHINH] ([TenCoQuan], [DiaChiCoQuan], [DienThoaiCoQuan], [FaxCoQuan], [EmailCoQuan], [WebCoQuan], [LogoCoQuan], [NamChinhLy], [TenMucLucHoSo], [TenLanChinhLy], [MaDVHinhThanhPhong], [MaPhong], [NamChinhLyPhong], [HienThiLogoADDJ], [cot1], [cot2], [cot3], [cot4], [cot5], [cot6], [cot7], [cot8], [cot9], [cot10], [cot11], [cot12], [cot13], [cot14], [cot15], [cot16], [cot17], [cot18], [cot19], [cot20], [cot21], [cot22], [cot23], [image_logo], [cot24], [cot25], [cot26], [cot27], [cot28], [cot29], [cot30]) VALUES (N'Chi cục thuế huyện Lý Sơn', N'', N'', N'', NULL, N'', N'2022.Chi cục thuế huyện Lý Sơn', N'2022', N'MLHS Có thời hạn', N'2022.Chi cục thuế huyện Lý Sơn', N'', N'', N'', N'0', N'1_Hộp số', N'1_Hồ sơ số', N'1_Tiêu đề hồ sơ', N'1_Số tờ', N'1_Ngày tháng', N'1_Năm', N'1_THBQ', N'1_Ghi chú', N'0_', N'0_', N'0_', N'1_Bộ phận', N'1_CL2022.Vĩnh viễn', N'0_', N'0_', N'0_', N'0_', N'0_', N'0_', N'0_', N'0_', N'0_', N'0_', NULL, N'0_', N'0_', N'0_', N'0_', N'0_', N'0_', N'0_')
GO

