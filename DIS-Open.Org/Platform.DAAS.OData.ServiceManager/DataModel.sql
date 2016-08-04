CREATE DATABASE `odataplatform` /*!40100 DEFAULT CHARACTER SET utf8 */;

CREATE TABLE `applications` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `description` varchar(245) DEFAULT NULL,
  `owner` varchar(45) NOT NULL,
  `status` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `services` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `dbtype` int(11) NOT NULL,
  `dbconnectionstring` varchar(180) NOT NULL,
  `resourcename` varchar(80) NOT NULL,
  `modelmeta`  longtext,
  `servicemeta`  longtext,
  `binary` blob,
  `description`  longtext,
  `url` varchar(500),
  `version` varchar(45) NOT NULL,
  `status` int(11) NOT NULL,
  `applicationid` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`),
  KEY `fk_app_id_idx` (`applicationid`),
  CONSTRAINT `fk_app_id` FOREIGN KEY (`applicationid`) REFERENCES `applications` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `servicesubscriptions` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `serviceid` int(11) DEFAULT NULL,
  `subscriber` varchar(45) NOT NULL,
  `status` int(11) NOT NULL,
  `creationtime` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_srv_id_idx` (`service_id`),
  CONSTRAINT `fk_srv_id` FOREIGN KEY (`serviceid`) REFERENCES `services` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `serviceconsumptions` (
  `id` varchar(60) NOT NULL,
  `serviceid` int(11) NOT NULL,
  `consumer` varchar(45) NOT NULL,
  `urlreferrer` varchar(500) DEFAULT NULL,
  `result` varchar(3600) DEFAULT NULL,
  `creationtime` datetime DEFAULT NULL,
  `status` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_srv_id_idx` (`serviceid`),
  CONSTRAINT `fk_srvc_srv_id` FOREIGN KEY (`serviceid`) REFERENCES `services` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
