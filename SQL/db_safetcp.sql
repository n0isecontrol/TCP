/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50527
Source Host           : localhost:3306
Source Database       : db_safetcp

Target Server Type    : MYSQL
Target Server Version : 50527
File Encoding         : 65001

Date: 2015-05-10 02:27:20
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `muser`
-- ----------------------------
DROP TABLE IF EXISTS `muser`;
CREATE TABLE `muser` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(10) DEFAULT NULL,
  `Password` varchar(50) DEFAULT NULL,
  `IpAddress` varchar(20) DEFAULT NULL,
  `Mail` varchar(50) DEFAULT NULL,
  `Phone` varchar(50) DEFAULT NULL,
  `Value1` varchar(50) DEFAULT NULL,
  `Value2` varchar(50) DEFAULT NULL,
  `Value3` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of muser
-- ----------------------------
INSERT INTO `MUser` VALUES ('1', 'test', 'test', '127.0.0.1', 'test@test.tset', '12345678', null, null, null);
