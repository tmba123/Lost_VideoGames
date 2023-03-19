-- MySQL dump 10.13  Distrib 8.0.28, for Win64 (x86_64)
--
-- Host: localhost    Database: gamestoreinventory
-- ------------------------------------------------------
-- Server version	8.0.28

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `game`
--

DROP TABLE IF EXISTS `game`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `game` (
  `id_game` int NOT NULL AUTO_INCREMENT,
  `id_publisher` int NOT NULL,
  `img_url` varchar(256) NOT NULL DEFAULT '/images/default.png',
  `name` varchar(45) NOT NULL,
  `genre` varchar(20) NOT NULL,
  `platform` varchar(20) NOT NULL,
  `release_year` int NOT NULL,
  `state` enum('enabled','disabled') NOT NULL DEFAULT 'enabled',
  PRIMARY KEY (`id_game`),
  KEY `fk1_idx` (`id_publisher`),
  CONSTRAINT `fk1` FOREIGN KEY (`id_publisher`) REFERENCES `publisher` (`id_publisher`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `game`
--

LOCK TABLES `game` WRITE;
/*!40000 ALTER TABLE `game` DISABLE KEYS */;
INSERT INTO `game` VALUES (1,1,'/images/Games/Sonic.jpg','Sonic','Platform','Genesis',1991,'enabled'),(2,2,'/images/Games/Super_Mario.png','Super Mario','Platform','NES',1985,'enabled'),(3,3,'/images/Games/Dead_Space.jpg','Dead Space Remake','Horror','PC',2023,'enabled'),(4,4,'/images/Games/Tomb_Raider.png','Tomb Raider','Platform','PC',1996,'enabled'),(5,4,'/images/Games/Duke_Nukem_3D.png','Duke Nukem 3D','First Person Shooter','PC',1996,'enabled'),(6,1,'/images/Games/Streets_of_Rage.jpg','Streets of Rage','Beat\'em up','Genesis',1991,'enabled');
/*!40000 ALTER TABLE `game` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inventory`
--

DROP TABLE IF EXISTS `inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inventory` (
  `id_game` int NOT NULL,
  `id_warehouse` int NOT NULL,
  `quantity` int NOT NULL,
  PRIMARY KEY (`id_game`,`id_warehouse`),
  KEY `fk2_idx` (`id_game`),
  KEY `fk3_idx` (`id_warehouse`),
  CONSTRAINT `fk2` FOREIGN KEY (`id_game`) REFERENCES `game` (`id_game`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk3` FOREIGN KEY (`id_warehouse`) REFERENCES `warehouse` (`id_warehouse`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inventory`
--

LOCK TABLES `inventory` WRITE;
/*!40000 ALTER TABLE `inventory` DISABLE KEYS */;
INSERT INTO `inventory` VALUES (1,2,9),(2,3,7),(2,6,2),(3,1,5),(3,3,10),(3,7,5),(4,5,10),(5,1,1),(5,2,1),(6,7,5);
/*!40000 ALTER TABLE `inventory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `movement`
--

DROP TABLE IF EXISTS `movement`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `movement` (
  `id_movement` int NOT NULL AUTO_INCREMENT,
  `id_game` int NOT NULL,
  `id_warehouse` int NOT NULL,
  `movement_type` enum('in_transit','add_stock','remove_stock','stock_reconciliation') NOT NULL,
  `quantity` int NOT NULL,
  `movement_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_movement`),
  KEY `fk4_idx` (`id_game`),
  KEY `fk5_idx` (`id_warehouse`),
  CONSTRAINT `fk4` FOREIGN KEY (`id_game`) REFERENCES `game` (`id_game`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk5` FOREIGN KEY (`id_warehouse`) REFERENCES `warehouse` (`id_warehouse`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `movement`
--

LOCK TABLES `movement` WRITE;
/*!40000 ALTER TABLE `movement` DISABLE KEYS */;
INSERT INTO `movement` VALUES (1,1,2,'add_stock',10,'2023-03-19 23:22:58'),(2,2,3,'add_stock',10,'2023-03-19 23:23:10'),(3,4,5,'add_stock',15,'2023-03-19 23:23:19'),(4,6,7,'add_stock',5,'2023-03-19 23:23:29'),(5,3,3,'add_stock',20,'2023-03-19 23:23:49'),(6,2,6,'add_stock',2,'2023-03-19 23:24:03'),(7,5,2,'add_stock',7,'2023-03-19 23:33:18'),(8,1,2,'remove_stock',-1,'2023-03-19 23:42:23'),(9,2,3,'stock_reconciliation',-3,'2023-03-19 23:42:43'),(10,4,5,'remove_stock',-5,'2023-03-19 23:42:56'),(11,5,2,'in_transit',-2,'2023-03-19 23:43:11'),(12,5,1,'in_transit',2,'2023-03-19 23:43:11'),(13,3,3,'in_transit',-10,'2023-03-19 23:43:27'),(14,3,1,'in_transit',10,'2023-03-19 23:43:27'),(15,5,1,'in_transit',-1,'2023-03-19 23:43:45'),(16,5,2,'in_transit',1,'2023-03-19 23:43:45'),(17,3,1,'in_transit',-5,'2023-03-19 23:43:54'),(18,3,7,'in_transit',5,'2023-03-19 23:43:54');
/*!40000 ALTER TABLE `movement` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `publisher`
--

DROP TABLE IF EXISTS `publisher`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `publisher` (
  `id_publisher` int NOT NULL AUTO_INCREMENT,
  `img_url` varchar(100) NOT NULL DEFAULT '/images/default.png',
  `name` varchar(45) NOT NULL,
  `country` varchar(30) NOT NULL,
  `year` int NOT NULL,
  `state` enum('enabled','disabled') NOT NULL DEFAULT 'enabled',
  PRIMARY KEY (`id_publisher`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `publisher`
--

LOCK TABLES `publisher` WRITE;
/*!40000 ALTER TABLE `publisher` DISABLE KEYS */;
INSERT INTO `publisher` VALUES (1,'/images/Publisher/sega.png','Sega','Japan',1960,'enabled'),(2,'/images/Publisher/nintendo.png','Nintendo','Japan',1889,'enabled'),(3,'/images//Publisher/Electronic_Arts.png','Electronic Arts','United States',1982,'enabled'),(4,'/images/Publisher/Eidos.png','Eidos Interactive','United Kingdom',1990,'enabled'),(5,'/images/Publisher/Square_Enix.png','Square Enix','Japan',2003,'enabled'),(6,'/images/Publisher/Crystal_Dynamics.png','Crystal Dynamics','United States',1992,'enabled');
/*!40000 ALTER TABLE `publisher` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `warehouse`
--

DROP TABLE IF EXISTS `warehouse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `warehouse` (
  `id_warehouse` int NOT NULL AUTO_INCREMENT,
  `location` varchar(45) NOT NULL,
  `state` enum('enabled','disabled') NOT NULL DEFAULT 'enabled',
  PRIMARY KEY (`id_warehouse`),
  UNIQUE KEY `location_UNIQUE` (`location`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `warehouse`
--

LOCK TABLES `warehouse` WRITE;
/*!40000 ALTER TABLE `warehouse` DISABLE KEYS */;
INSERT INTO `warehouse` VALUES (1,'Transit','enabled'),(2,'Lisboa','enabled'),(3,'Porto','enabled'),(4,'Coimbra','enabled'),(5,'Madrid','enabled'),(6,'Barcelona','enabled'),(7,'Vigo','enabled');
/*!40000 ALTER TABLE `warehouse` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'gamestoreinventory'
--

--
-- Dumping routines for database 'gamestoreinventory'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-03-19 23:51:39
