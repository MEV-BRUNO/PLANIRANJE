-- MySQL dump 10.13  Distrib 8.0.11, for Win64 (x86_64)
--
-- Host: localhost    Database: planiranje
-- ------------------------------------------------------
-- Server version	8.0.11

#!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
#!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
#!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
#!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
#!40103 SET TIME_ZONE='+00:00' */;
#!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
#!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
#!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
#!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `aktivnost`
--

DROP TABLE IF EXISTS `aktivnost`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `aktivnost` (
  `id_aktivnost` int(11) NOT NULL AUTO_INCREMENT,
  `naziv` varchar(25) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id_aktivnost`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aktivnost`
--

LOCK TABLES `aktivnost` WRITE;
#!40000 ALTER TABLE `aktivnost` DISABLE KEYS */;
#!40000 ALTER TABLE `aktivnost` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aktivnost_akcija`
--

DROP TABLE IF EXISTS `aktivnost_akcija`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `aktivnost_akcija` (
  `id_akcija` int(11) NOT NULL AUTO_INCREMENT,
  `naziv` varchar(25) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `id_aktivnost` int(11) DEFAULT NULL,
  PRIMARY KEY (`id_akcija`),
  KEY `id_aktivnost` (`id_aktivnost`),
  CONSTRAINT `aktivnost_akcija_ibfk_1` FOREIGN KEY (`id_aktivnost`) REFERENCES `aktivnost` (`id_aktivnost`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aktivnost_akcija`
--

LOCK TABLES `aktivnost_akcija` WRITE;
#!40000 ALTER TABLE `aktivnost_akcija` DISABLE KEYS */;
#!40000 ALTER TABLE `aktivnost_akcija` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ciljevi`
--

DROP TABLE IF EXISTS `ciljevi`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `ciljevi` (
  `id_cilj` int(11) NOT NULL AUTO_INCREMENT,
  `naziv` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`id_cilj`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ciljevi`
--

LOCK TABLES `ciljevi` WRITE;
#!40000 ALTER TABLE `ciljevi` DISABLE KEYS */;
#!40000 ALTER TABLE `ciljevi` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `dnevnik_detalji`
--

DROP TABLE IF EXISTS `dnevnik_detalji`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `dnevnik_detalji` (
  `id_dnevnik` int(11) NOT NULL AUTO_INCREMENT,
  `subjekt` int(11) NOT NULL,
  `vrijeme_od` datetime NOT NULL,
  `vrijeme_do` datetime NOT NULL,
  `aktivnost` int(11) NOT NULL,
  `suradnja` int(11) DEFAULT NULL,
  `zakljucak` text CHARACTER SET utf8 COLLATE utf8_general_ci,
  PRIMARY KEY (`id_dnevnik`),
  KEY `aktivnost` (`aktivnost`),
  KEY `subjekt` (`subjekt`),
  KEY `suradnja` (`suradnja`),
  CONSTRAINT `dnevnik_detalji_ibfk_1` FOREIGN KEY (`aktivnost`) REFERENCES `aktivnost` (`id_aktivnost`),
  CONSTRAINT `dnevnik_detalji_ibfk_2` FOREIGN KEY (`subjekt`) REFERENCES `subjekti` (`id_subjekt`),
  CONSTRAINT `dnevnik_detalji_ibfk_3` FOREIGN KEY (`suradnja`) REFERENCES `pedagog` (`id_pedagog`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dnevnik_detalji`
--

LOCK TABLES `dnevnik_detalji` WRITE;
#!40000 ALTER TABLE `dnevnik_detalji` DISABLE KEYS */;
#!40000 ALTER TABLE `dnevnik_detalji` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `dnevnik_rada`
--

DROP TABLE IF EXISTS `dnevnik_rada`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `dnevnik_rada` (
  `id_dnevnik` int(11) NOT NULL AUTO_INCREMENT,
  `id_pedagog` int(11) NOT NULL,
  `ak_godina` int(11) NOT NULL,
  `naziv` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `opis` text CHARACTER SET utf8 COLLATE utf8_general_ci,
  `datum` datetime NOT NULL,
  PRIMARY KEY (`id_dnevnik`),
  KEY `id_pedagog` (`id_pedagog`),
  CONSTRAINT `dnevnik_rada_ibfk_1` FOREIGN KEY (`id_pedagog`) REFERENCES `pedagog` (`id_pedagog`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dnevnik_rada`
--

LOCK TABLES `dnevnik_rada` WRITE;
#!40000 ALTER TABLE `dnevnik_rada` DISABLE KEYS */;
#!40000 ALTER TABLE `dnevnik_rada` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `godisnji_detalji`
--

DROP TABLE IF EXISTS `godisnji_detalji`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `godisnji_detalji` (
  `id_god` int(11) NOT NULL AUTO_INCREMENT,
  `mjesec` tinyint(4) NOT NULL,
  `naziv_mjeseca` varchar(15) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ukupno_dana` tinyint(4) NOT NULL,
  `radnih_dana` tinyint(4) NOT NULL,
  `subota_dana` tinyint(4) NOT NULL,
  `blagdana_dana` tinyint(4) NOT NULL,
  `nastavnih_dana` tinyint(4) NOT NULL,
  `praznika_dana` tinyint(4) NOT NULL,
  `br_sati` smallint(6) DEFAULT NULL,
  `odmor_dana` tinyint(4) DEFAULT NULL,
  `odmor_sati` smallint(6) DEFAULT NULL,
  `mj_fond_sati` smallint(6) DEFAULT NULL,
  `br_rad_dana_sk_god` smallint(6) DEFAULT NULL,
  `br_dana_god_odmor` smallint(6) NOT NULL,
  `br_rad_dana` smallint(6) NOT NULL,
  PRIMARY KEY (`id_god`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `godisnji_detalji`
--

LOCK TABLES `godisnji_detalji` WRITE;
#!40000 ALTER TABLE `godisnji_detalji` DISABLE KEYS */;
#!40000 ALTER TABLE `godisnji_detalji` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `godisnji_plan`
--

DROP TABLE IF EXISTS `godisnji_plan`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `godisnji_plan` (
  `id_god` int(11) NOT NULL AUTO_INCREMENT,
  `ak_godina` tinyint(4) NOT NULL,
  `id_pedagog` int(11) NOT NULL,
  `br_radnih_dana` int(11) DEFAULT NULL,
  `broj_dana_godina_odmor` int(11) DEFAULT NULL,
  `ukupni_rad_dana` int(11) DEFAULT NULL,
  `god_fond_sati` int(11) DEFAULT NULL,
  PRIMARY KEY (`id_god`),
  KEY `id_pedagog` (`id_pedagog`),
  CONSTRAINT `godisnji_plan_ibfk_1` FOREIGN KEY (`id_pedagog`) REFERENCES `pedagog` (`id_pedagog`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `godisnji_plan`
--

LOCK TABLES `godisnji_plan` WRITE;
#!40000 ALTER TABLE `godisnji_plan` DISABLE KEYS */;
#!40000 ALTER TABLE `godisnji_plan` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mjesecni_detalji`
--

DROP TABLE IF EXISTS `mjesecni_detalji`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mjesecni_detalji` (
  `id_plan` int(11) NOT NULL AUTO_INCREMENT,
  `red_br` int(11) NOT NULL,
  `podrucje` int(11) NOT NULL,
  `aktivnost` int(11) NOT NULL,
  `suradnici` int(11) DEFAULT NULL,
  `vrijeme` datetime NOT NULL,
  `br_sati` int(11) NOT NULL,
  `biljeska` text CHARACTER SET utf8 COLLATE utf8_general_ci,
  PRIMARY KEY (`id_plan`),
  KEY `podrucje` (`podrucje`),
  KEY `aktivnost` (`aktivnost`),
  KEY `suradnici` (`suradnici`),
  CONSTRAINT `mjesecni_detalji_ibfk_1` FOREIGN KEY (`podrucje`) REFERENCES `podrucje_rada` (`id_podrucje`),
  CONSTRAINT `mjesecni_detalji_ibfk_2` FOREIGN KEY (`aktivnost`) REFERENCES `aktivnost` (`id_aktivnost`),
  CONSTRAINT `mjesecni_detalji_ibfk_3` FOREIGN KEY (`suradnici`) REFERENCES `pedagog` (`id_pedagog`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mjesecni_detalji`
--

LOCK TABLES `mjesecni_detalji` WRITE;
#!40000 ALTER TABLE `mjesecni_detalji` DISABLE KEYS */;
#!40000 ALTER TABLE `mjesecni_detalji` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mjesecni_plan`
--

DROP TABLE IF EXISTS `mjesecni_plan`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mjesecni_plan` (
  `id_plan` int(11) NOT NULL AUTO_INCREMENT,
  `id_pedagog` int(11) NOT NULL,
  `ak_godina` int(11) NOT NULL,
  `naziv` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `opis` text CHARACTER SET utf8 COLLATE utf8_general_ci,
  PRIMARY KEY (`id_plan`),
  KEY `id_pedagog` (`id_pedagog`),
  CONSTRAINT `mjesecni_plan_ibfk_1` FOREIGN KEY (`id_pedagog`) REFERENCES `pedagog` (`id_pedagog`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mjesecni_plan`
--

LOCK TABLES `mjesecni_plan` WRITE;
#!40000 ALTER TABLE `mjesecni_plan` DISABLE KEYS */;
#!40000 ALTER TABLE `mjesecni_plan` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `oblici`
--

DROP TABLE IF EXISTS `oblici`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `oblici` (
  `id_oblici` int(11) NOT NULL AUTO_INCREMENT,
  `naziv` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`id_oblici`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `oblici`
--

LOCK TABLES `oblici` WRITE;
#!40000 ALTER TABLE `oblici` DISABLE KEYS */;
#!40000 ALTER TABLE `oblici` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `os_plan_1`
--

DROP TABLE IF EXISTS `os_plan_1`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `os_plan_1` (
  `id_plan` int(11) NOT NULL AUTO_INCREMENT,
  `id_pedagog` int(11) NOT NULL,
  `ak_godina` tinyint(4) NOT NULL,
  `naziv` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `opis` text CHARACTER SET utf8 COLLATE utf8_general_ci,
  PRIMARY KEY (`id_plan`),
  KEY `id_pedagog` (`id_pedagog`),
  CONSTRAINT `os_plan_1_ibfk_1` FOREIGN KEY (`id_pedagog`) REFERENCES `pedagog` (`id_pedagog`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `os_plan_1`
--

LOCK TABLES `os_plan_1` WRITE;
#!40000 ALTER TABLE `os_plan_1` DISABLE KEYS */;
#!40000 ALTER TABLE `os_plan_1` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `os_plan_1_akcija`
--

DROP TABLE IF EXISTS `os_plan_1_akcija`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `os_plan_1_akcija` (
  `id_plan` int(11) NOT NULL AUTO_INCREMENT,
  `red_broj_podrucje` int(11) NOT NULL,
  `red_broj_aktivnost` int(11) NOT NULL,
  `red_broj_akcija` int(11) NOT NULL,
  `opis_akcija` text CHARACTER SET utf8 COLLATE utf8_general_ci,
  `red_br` int(11) NOT NULL,
  `potrebno_sati` int(11) NOT NULL,
  `br_sati` int(11) NOT NULL,
  `mj_1` int(11) DEFAULT NULL,
  `mj_2` int(11) DEFAULT NULL,
  `mj_3` int(11) DEFAULT NULL,
  `mj_4` int(11) DEFAULT NULL,
  `mj_5` int(11) DEFAULT NULL,
  `mj_6` int(11) DEFAULT NULL,
  `mj_7` int(11) DEFAULT NULL,
  `mj_8` int(11) DEFAULT NULL,
  `mj_9` int(11) DEFAULT NULL,
  `mj_10` int(11) DEFAULT NULL,
  `mj_11` int(11) DEFAULT NULL,
  `mj_12` int(11) DEFAULT NULL,
  PRIMARY KEY (`id_plan`),
  KEY `red_broj_podrucje` (`red_broj_podrucje`),
  KEY `red_broj_aktivnost` (`red_broj_aktivnost`),
  KEY `red_broj_akcija` (`red_broj_akcija`),
  CONSTRAINT `os_plan_1_akcija_ibfk_1` FOREIGN KEY (`red_broj_podrucje`) REFERENCES `podrucje_rada` (`id_podrucje`),
  CONSTRAINT `os_plan_1_akcija_ibfk_2` FOREIGN KEY (`red_broj_aktivnost`) REFERENCES `aktivnost` (`id_aktivnost`),
  CONSTRAINT `os_plan_1_akcija_ibfk_3` FOREIGN KEY (`red_broj_akcija`) REFERENCES `aktivnost_akcija` (`id_akcija`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `os_plan_1_akcija`
--

LOCK TABLES `os_plan_1_akcija` WRITE;
#!40000 ALTER TABLE `os_plan_1_akcija` DISABLE KEYS */;
#!40000 ALTER TABLE `os_plan_1_akcija` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `os_plan_1_aktivnost`
--

DROP TABLE IF EXISTS `os_plan_1_aktivnost`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `os_plan_1_aktivnost` (
  `id_plan` int(11) NOT NULL AUTO_INCREMENT,
  `red_broj_podrucje` int(11) NOT NULL,
  `red_broj_aktivnost` int(11) NOT NULL,
  `opis_aktivnost` text CHARACTER SET utf8 COLLATE utf8_general_ci,
  `red_br` int(11) NOT NULL,
  `potrebno_sati` int(11) NOT NULL,
  `br_sati` int(11) NOT NULL,
  `mj_1` int(11) DEFAULT NULL,
  `mj_2` int(11) DEFAULT NULL,
  `mj_3` int(11) DEFAULT NULL,
  `mj_4` int(11) DEFAULT NULL,
  `mj_5` int(11) DEFAULT NULL,
  `mj_6` int(11) DEFAULT NULL,
  `mj_7` int(11) DEFAULT NULL,
  `mj_8` int(11) DEFAULT NULL,
  `mj_9` int(11) DEFAULT NULL,
  `mj_10` int(11) DEFAULT NULL,
  `mj_11` int(11) DEFAULT NULL,
  `mj_12` int(11) DEFAULT NULL,
  PRIMARY KEY (`id_plan`),
  KEY `red_broj_podrucje` (`red_broj_podrucje`),
  KEY `red_broj_aktivnost` (`red_broj_aktivnost`),
  CONSTRAINT `os_plan_1_aktivnost_ibfk_1` FOREIGN KEY (`red_broj_podrucje`) REFERENCES `podrucje_rada` (`id_podrucje`),
  CONSTRAINT `os_plan_1_aktivnost_ibfk_2` FOREIGN KEY (`red_broj_aktivnost`) REFERENCES `aktivnost` (`id_aktivnost`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `os_plan_1_aktivnost`
--

LOCK TABLES `os_plan_1_aktivnost` WRITE;
#!40000 ALTER TABLE `os_plan_1_aktivnost` DISABLE KEYS */;
#!40000 ALTER TABLE `os_plan_1_aktivnost` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `os_plan_1_podrucje`
--

DROP TABLE IF EXISTS `os_plan_1_podrucje`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `os_plan_1_podrucje` (
  `id_plan` int(11) NOT NULL AUTO_INCREMENT,
  `red_broj_podrucje` int(11) NOT NULL,
  `opis_podrucje` text CHARACTER SET utf8 COLLATE utf8_general_ci,
  `potrebno_sati` int(11) NOT NULL,
  `cilj` int(11) NOT NULL,
  `br_sati` int(11) NOT NULL,
  `mj_1` int(11) DEFAULT NULL,
  `mj_2` int(11) DEFAULT NULL,
  `mj_3` int(11) DEFAULT NULL,
  `mj_4` int(11) DEFAULT NULL,
  `mj_5` int(11) DEFAULT NULL,
  `mj_6` int(11) DEFAULT NULL,
  `mj_7` int(11) DEFAULT NULL,
  `mj_8` int(11) DEFAULT NULL,
  `mj_9` int(11) DEFAULT NULL,
  `mj_10` int(11) DEFAULT NULL,
  `mj_11` int(11) DEFAULT NULL,
  `mj_12` int(11) DEFAULT NULL,
  PRIMARY KEY (`id_plan`),
  KEY `red_broj_podrucje` (`red_broj_podrucje`),
  KEY `cilj` (`cilj`),
  CONSTRAINT `os_plan_1_podrucje_ibfk_1` FOREIGN KEY (`red_broj_podrucje`) REFERENCES `podrucje_rada` (`id_podrucje`),
  CONSTRAINT `os_plan_1_podrucje_ibfk_2` FOREIGN KEY (`cilj`) REFERENCES `ciljevi` (`id_cilj`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `os_plan_1_podrucje`
--

LOCK TABLES `os_plan_1_podrucje` WRITE;
#!40000 ALTER TABLE `os_plan_1_podrucje` DISABLE KEYS */;
#!40000 ALTER TABLE `os_plan_1_podrucje` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `os_plan_2`
--

DROP TABLE IF EXISTS `os_plan_2`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `os_plan_2` (
  `id_plan` int(11) NOT NULL AUTO_INCREMENT,
  `id_pedagog` int(11) NOT NULL,
  `ak_godina` tinyint(4) NOT NULL,
  `naziv` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `opis` text CHARACTER SET utf8 COLLATE utf8_general_ci,
  PRIMARY KEY (`id_plan`),
  KEY `id_pedagog` (`id_pedagog`),
  CONSTRAINT `os_plan_2_ibfk_1` FOREIGN KEY (`id_pedagog`) REFERENCES `pedagog` (`id_pedagog`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `os_plan_2`
--

LOCK TABLES `os_plan_2` WRITE;
#!40000 ALTER TABLE `os_plan_2` DISABLE KEYS */;
#!40000 ALTER TABLE `os_plan_2` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `os_plan_2_akcija`
--

DROP TABLE IF EXISTS `os_plan_2_akcija`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `os_plan_2_akcija` (
  `id_plan` int(11) NOT NULL AUTO_INCREMENT,
  `red_br_podrucje` int(11) NOT NULL,
  `red_br_aktivnost` int(11) NOT NULL,
  `red_br_akcija` int(11) NOT NULL,
  `opis_akcija` text CHARACTER SET utf8 COLLATE utf8_general_ci,
  `sati` int(11) NOT NULL,
  PRIMARY KEY (`id_plan`),
  KEY `red_br_podrucje` (`red_br_podrucje`),
  KEY `red_br_aktivnost` (`red_br_aktivnost`),
  KEY `red_br_akcija` (`red_br_akcija`),
  CONSTRAINT `os_plan_2_akcija_ibfk_1` FOREIGN KEY (`red_br_podrucje`) REFERENCES `podrucje_rada` (`id_podrucje`),
  CONSTRAINT `os_plan_2_akcija_ibfk_2` FOREIGN KEY (`red_br_aktivnost`) REFERENCES `aktivnost` (`id_aktivnost`),
  CONSTRAINT `os_plan_2_akcija_ibfk_3` FOREIGN KEY (`red_br_akcija`) REFERENCES `aktivnost_akcija` (`id_akcija`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `os_plan_2_akcija`
--

LOCK TABLES `os_plan_2_akcija` WRITE;
#!40000 ALTER TABLE `os_plan_2_akcija` DISABLE KEYS */;
#!40000 ALTER TABLE `os_plan_2_akcija` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `os_plan_2_aktivnost`
--

DROP TABLE IF EXISTS `os_plan_2_aktivnost`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `os_plan_2_aktivnost` (
  `id_plan` int(11) NOT NULL AUTO_INCREMENT,
  `red_br_podrucje` int(11) NOT NULL,
  `red_br_aktivnost` int(11) NOT NULL,
  `opis_aktivnost` text CHARACTER SET utf8 COLLATE utf8_general_ci,
  `cilj` int(11) NOT NULL,
  `zadaci` int(11) DEFAULT NULL,
  `subjekti` int(11) NOT NULL,
  `oblici` int(11) NOT NULL,
  `vrijeme` datetime NOT NULL,
  `sati` int(11) NOT NULL,
  PRIMARY KEY (`id_plan`),
  KEY `red_br_podrucje` (`red_br_podrucje`),
  KEY `red_br_aktivnost` (`red_br_aktivnost`),
  KEY `cilj` (`cilj`),
  KEY `subjekti` (`subjekti`),
  KEY `zadaci` (`zadaci`),
  CONSTRAINT `os_plan_2_aktivnost_ibfk_1` FOREIGN KEY (`red_br_podrucje`) REFERENCES `podrucje_rada` (`id_podrucje`),
  CONSTRAINT `os_plan_2_aktivnost_ibfk_2` FOREIGN KEY (`red_br_aktivnost`) REFERENCES `aktivnost` (`id_aktivnost`),
  CONSTRAINT `os_plan_2_aktivnost_ibfk_3` FOREIGN KEY (`cilj`) REFERENCES `ciljevi` (`id_cilj`),
  CONSTRAINT `os_plan_2_aktivnost_ibfk_4` FOREIGN KEY (`subjekti`) REFERENCES `subjekti` (`id_subjekt`),
  CONSTRAINT `os_plan_2_aktivnost_ibfk_5` FOREIGN KEY (`zadaci`) REFERENCES `zadaci` (`id_zadatak`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `os_plan_2_aktivnost`
--

LOCK TABLES `os_plan_2_aktivnost` WRITE;
#!40000 ALTER TABLE `os_plan_2_aktivnost` DISABLE KEYS */;
#!40000 ALTER TABLE `os_plan_2_aktivnost` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `os_plan_2_podrucje`
--

DROP TABLE IF EXISTS `os_plan_2_podrucje`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `os_plan_2_podrucje` (
  `id_plan` int(11) NOT NULL AUTO_INCREMENT,
  `red_br_podrucje` int(11) NOT NULL,
  `cilj` int(11) NOT NULL,
  `zadaci` int(11) DEFAULT NULL,
  `subjekti` int(11) NOT NULL,
  `oblici` int(11) NOT NULL,
  `vrijeme` datetime NOT NULL,
  `sati` int(11) NOT NULL,
  PRIMARY KEY (`id_plan`),
  KEY `red_br_podrucje` (`red_br_podrucje`),
  KEY `cilj` (`cilj`),
  KEY `subjekti` (`subjekti`),
  KEY `oblici` (`oblici`),
  KEY `zadaci` (`zadaci`),
  CONSTRAINT `os_plan_2_podrucje_ibfk_1` FOREIGN KEY (`red_br_podrucje`) REFERENCES `podrucje_rada` (`id_podrucje`),
  CONSTRAINT `os_plan_2_podrucje_ibfk_2` FOREIGN KEY (`cilj`) REFERENCES `ciljevi` (`id_cilj`),
  CONSTRAINT `os_plan_2_podrucje_ibfk_3` FOREIGN KEY (`subjekti`) REFERENCES `subjekti` (`id_subjekt`),
  CONSTRAINT `os_plan_2_podrucje_ibfk_4` FOREIGN KEY (`oblici`) REFERENCES `oblici` (`id_oblici`),
  CONSTRAINT `os_plan_2_podrucje_ibfk_5` FOREIGN KEY (`zadaci`) REFERENCES `zadaci` (`id_zadatak`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `os_plan_2_podrucje`
--

LOCK TABLES `os_plan_2_podrucje` WRITE;
#!40000 ALTER TABLE `os_plan_2_podrucje` DISABLE KEYS */;
#!40000 ALTER TABLE `os_plan_2_podrucje` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pedagog`
--

DROP TABLE IF EXISTS `pedagog`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `pedagog` (
  `id_pedagog` int(11) NOT NULL AUTO_INCREMENT,
  `ime` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `prezime` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `email` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `lozinka` varchar(40) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `licenca` datetime NOT NULL,
  `id_skola` int(11) NOT NULL,
  `aktivan` tinyint(1) NOT NULL,
  `titula` varchar(40) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id_pedagog`),
  UNIQUE KEY `email` (`email`),
  KEY `id_skola` (`id_skola`),
  CONSTRAINT `pedagog_ibfk_1` FOREIGN KEY (`id_skola`) REFERENCES `skola` (`id_skola`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pedagog`
--

LOCK TABLES `pedagog` WRITE;
#!40000 ALTER TABLE `pedagog` DISABLE KEYS */;
#!40000 ALTER TABLE `pedagog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `podrucje_rada`
--

DROP TABLE IF EXISTS `podrucje_rada`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `podrucje_rada` (
  `id_podrucje` int(11) NOT NULL AUTO_INCREMENT,
  `naziv` varchar(25) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id_podrucje`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `podrucje_rada`
--

LOCK TABLES `podrucje_rada` WRITE;
#!40000 ALTER TABLE `podrucje_rada` DISABLE KEYS */;
#!40000 ALTER TABLE `podrucje_rada` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `skola`
--

DROP TABLE IF EXISTS `skola`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `skola` (
  `id_skola` int(11) NOT NULL AUTO_INCREMENT,
  `naziv` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `adresa` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `grad` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `tel` varchar(15) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `url` varchar(25) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `kontakt` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`id_skola`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `skola`
--

LOCK TABLES `skola` WRITE;
#!40000 ALTER TABLE `skola` DISABLE KEYS */;
#!40000 ALTER TABLE `skola` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ss_plan`
--

DROP TABLE IF EXISTS `ss_plan`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `ss_plan` (
  `id_plan` int(11) NOT NULL AUTO_INCREMENT,
  `id_pedagog` int(11) NOT NULL,
  `ak_godina` tinyint(4) NOT NULL,
  `naziv` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `opis` text CHARACTER SET utf8 COLLATE utf8_general_ci,
  PRIMARY KEY (`id_plan`),
  KEY `id_pedagog` (`id_pedagog`),
  CONSTRAINT `ss_plan_ibfk_1` FOREIGN KEY (`id_pedagog`) REFERENCES `pedagog` (`id_pedagog`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ss_plan`
--

LOCK TABLES `ss_plan` WRITE;
#!40000 ALTER TABLE `ss_plan` DISABLE KEYS */;
#!40000 ALTER TABLE `ss_plan` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ss_plan_podrucje`
--

DROP TABLE IF EXISTS `ss_plan_podrucje`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `ss_plan_podrucje` (
  `id_plan` int(11) NOT NULL AUTO_INCREMENT,
  `red_br_podrucje` int(11) NOT NULL,
  `opis_podrucje` text CHARACTER SET utf8 COLLATE utf8_general_ci,
  `svrha` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `zadaca` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `sadrzaj` text CHARACTER SET utf8 COLLATE utf8_general_ci,
  `oblici` int(11) NOT NULL,
  `suradnici` int(11) NOT NULL,
  `mjesto` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `vrijeme` datetime NOT NULL,
  `ishodi` text CHARACTER SET utf8 COLLATE utf8_general_ci,
  `sati` int(11) NOT NULL,
  PRIMARY KEY (`id_plan`),
  KEY `red_br_podrucje` (`red_br_podrucje`),
  KEY `oblici` (`oblici`),
  KEY `suradnici` (`suradnici`),
  CONSTRAINT `ss_plan_podrucje_ibfk_1` FOREIGN KEY (`red_br_podrucje`) REFERENCES `podrucje_rada` (`id_podrucje`),
  CONSTRAINT `ss_plan_podrucje_ibfk_2` FOREIGN KEY (`oblici`) REFERENCES `oblici` (`id_oblici`),
  CONSTRAINT `ss_plan_podrucje_ibfk_3` FOREIGN KEY (`suradnici`) REFERENCES `pedagog` (`id_pedagog`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ss_plan_podrucje`
--

LOCK TABLES `ss_plan_podrucje` WRITE;
#!40000 ALTER TABLE `ss_plan_podrucje` DISABLE KEYS */;
#!40000 ALTER TABLE `ss_plan_podrucje` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `subjekti`
--

DROP TABLE IF EXISTS `subjekti`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `subjekti` (
  `id_subjekt` int(11) NOT NULL AUTO_INCREMENT,
  `naziv` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`id_subjekt`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `subjekti`
--

LOCK TABLES `subjekti` WRITE;
#!40000 ALTER TABLE `subjekti` DISABLE KEYS */;
#!40000 ALTER TABLE `subjekti` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `zadaci`
--

DROP TABLE IF EXISTS `zadaci`;
#!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `zadaci` (
  `id_zadatak` int(11) NOT NULL AUTO_INCREMENT,
  `naziv` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`id_zadatak`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_croatian_ci;
#!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `zadaci`
--

LOCK TABLES `zadaci` WRITE;
#!40000 ALTER TABLE `zadaci` DISABLE KEYS */;
#!40000 ALTER TABLE `zadaci` ENABLE KEYS */;
UNLOCK TABLES;
#!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

#!40101 SET SQL_MODE=@OLD_SQL_MODE */;
#!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
#!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
#!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
#!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
#!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
#!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-05-22 17:46:21
