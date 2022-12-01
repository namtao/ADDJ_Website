/*
 Navicat Premium Data Transfer

 Source Server         : 2022.Chi cục thuế huyện Lý Sơn
 Source Server Type    : SQLite
 Source Server Version : 3030001
 Source Schema         : main

 Target Server Type    : SQLite
 Target Server Version : 3030001
 File Encoding         : 65001

 Date: 10/02/2022 16:51:32
*/

PRAGMA foreign_keys = false;

-- ----------------------------
-- Table structure for THONGTINCAUHINH
-- ----------------------------
DROP TABLE IF EXISTS "THONGTINCAUHINH";
CREATE TABLE "THONGTINCAUHINH" (
  "TenCoQuan" TEXT,
  "DiaChiCoQuan" TEXT,
  "DienThoaiCoQuan" TEXT,
  "FaxCoQuan" TEXT,
  "EmailCoQuan" TEXT,
  "WebCoQuan" TEXT,
  "LogoCoQuan" TEXT,
  "NamChinhLy" TEXT,
  "TenMucLucHoSo" TEXT,
  "TenLanChinhLy" TEXT,
  "MaDVHinhThanhPhong" TEXT,
  "MaPhong" TEXT,
  "NamChinhLyPhong" TEXT,
  "HienThiLogoADDJ" TEXT,
  "cot1" TEXT,
  "cot2" TEXT,
  "cot3" TEXT,
  "cot4" TEXT,
  "cot5" TEXT,
  "cot6" TEXT,
  "cot7" TEXT,
  "cot8" TEXT,
  "cot9" TEXT,
  "cot10" TEXT,
  "cot11" TEXT,
  "cot12" TEXT,
  "cot13" TEXT,
  "cot14" TEXT,
  "cot15" TEXT,
  "cot16" TEXT,
  "cot17" TEXT,
  "cot18" TEXT,
  "cot19" TEXT,
  "cot20" TEXT,
  "cot21" TEXT,
  "cot22" TEXT,
  "cot23" TEXT,
  "image_logo" TEXT,
  "cot24" TEXT,
  "cot25" TEXT,
  "cot26" TEXT,
  "cot27" TEXT,
  "cot28" TEXT,
  "cot29" TEXT,
  "cot30" TEXT
);

-- ----------------------------
-- Records of THONGTINCAUHINH
-- ----------------------------
INSERT INTO "THONGTINCAUHINH" VALUES ('Chi cục thuế huyện Lý Sơn', '', '', '', NULL, '', '2022.Chi cục thuế huyện Lý Sơn', '2022', 'MLHS Có thời hạn', '2022.Chi cục thuế huyện Lý Sơn', '', '', '', '0', '1_Hộp số', '1_Hồ sơ số', '1_Tiêu đề hồ sơ', '1_Số tờ', '1_Ngày tháng', '1_Năm', '1_THBQ', '1_Ghi chú', '0_', '0_', '0_', '1_Bộ phận', '1_CL2022.Vĩnh viễn', '0_', '0_', '0_', '0_', '0_', '0_', '0_', '0_', '0_', '0_', NULL, '0_', '0_', '0_', '0_', '0_', '0_', '0_');

PRAGMA foreign_keys = true;
