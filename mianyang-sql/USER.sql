/*
Navicat Oracle Data Transfer
Oracle Client Version : 12.1.0.2.0

Source Server         : mianyang
Source Server Version : 110100
Source Host           : 192.168.1.114:1521
Source Schema         : MIANYANG

Target Server Type    : ORACLE
Target Server Version : 110100
File Encoding         : 65001

Date: 2016-06-11 19:36:36
*/


-- ----------------------------
-- Table structure for USER
-- ----------------------------
DROP TABLE "MIANYANG"."USER";
CREATE TABLE "MIANYANG"."USER" (
"USER_NAME" VARCHAR2(255 BYTE) NOT NULL ,
"USER_PASSWORD" VARCHAR2(255 BYTE) NOT NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;

-- ----------------------------
-- Indexes structure for table USER
-- ----------------------------

-- ----------------------------
-- Checks structure for table USER
-- ----------------------------
ALTER TABLE "MIANYANG"."USER" ADD CHECK ("USER_NAME" IS NOT NULL);
ALTER TABLE "MIANYANG"."USER" ADD CHECK ("USER_PASSWORD" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table USER
-- ----------------------------
ALTER TABLE "MIANYANG"."USER" ADD PRIMARY KEY ("USER_NAME");
