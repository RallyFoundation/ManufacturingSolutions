
use odataplatform;

-- -----------------------------------------------------
-- Table `SCHEMA_NAME`.`my_aspnet_applications`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `odataplatform`.`my_aspnet_applications` (
  `id` INT(11) NOT NULL AUTO_INCREMENT ,
  `name` VARCHAR(256) NULL DEFAULT NULL ,
  `description` VARCHAR(256) NULL DEFAULT NULL ,
  PRIMARY KEY (`id`) )
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_swedish_ci;
-- -----------------------------------------------------
-- Table `SCHEMA_NAME`.`my_aspnet_membership`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `odataplatform`.`my_aspnet_membership` (
  `userId` INT(11) NOT NULL DEFAULT '0' ,
  `Email` VARCHAR(128) NULL DEFAULT NULL ,
  `Comment` VARCHAR(255) NULL DEFAULT NULL ,
  `Password` VARCHAR(128) NOT NULL ,
  `PasswordKey` CHAR(32) NULL DEFAULT NULL ,
  `PasswordFormat` TINYINT(4) NULL DEFAULT NULL ,
  `PasswordQuestion` VARCHAR(255) NULL DEFAULT NULL ,
  `PasswordAnswer` VARCHAR(255) NULL DEFAULT NULL ,
  `IsApproved` TINYINT(1) NULL DEFAULT NULL ,
  `LastActivityDate` DATETIME NULL DEFAULT NULL ,
  `LastLoginDate` DATETIME NULL DEFAULT NULL ,
  `LastPasswordChangedDate` DATETIME NULL DEFAULT NULL ,
  `CreationDate` DATETIME NULL DEFAULT NULL ,
  `IsLockedOut` TINYINT(1) NULL DEFAULT NULL ,
  `LastLockedOutDate` DATETIME NULL DEFAULT NULL ,
  `FailedPasswordAttemptCount` INT(10) UNSIGNED NULL DEFAULT NULL ,
  `FailedPasswordAttemptWindowStart` DATETIME NULL DEFAULT NULL ,
  `FailedPasswordAnswerAttemptCount` INT(10) UNSIGNED NULL DEFAULT NULL ,
  `FailedPasswordAnswerAttemptWindowStart` DATETIME NULL DEFAULT NULL ,
  PRIMARY KEY (`userId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_swedish_ci
COMMENT = '2';
-- -----------------------------------------------------
-- Table `SCHEMA_NAME`.`my_aspnet_profiles`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `odataplatform`.`my_aspnet_profiles` (
  `userId` INT(11) NOT NULL ,
  `valueindex` LONGTEXT NULL DEFAULT NULL ,
  `stringdata` LONGTEXT NULL DEFAULT NULL ,
  `binarydata` LONGBLOB NULL DEFAULT NULL ,
  `lastUpdatedDate` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`userId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_swedish_ci;
-- -----------------------------------------------------
-- Table `SCHEMA_NAME`.`my_aspnet_roles`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `odataplatform`.`my_aspnet_roles` (
  `id` INT(11) NOT NULL AUTO_INCREMENT ,
  `applicationId` INT(11) NOT NULL ,
  `name` VARCHAR(255) NOT NULL ,
  PRIMARY KEY (`id`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_swedish_ci
ROW_FORMAT = DYNAMIC;
-- -----------------------------------------------------
-- Table `SCHEMA_NAME`.`my_aspnet_schemaversion`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `odataplatform`.`my_aspnet_schemaversion` (
  `version` INT(11) NULL DEFAULT NULL )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_swedish_ci;
-- -----------------------------------------------------
-- Table `SCHEMA_NAME`.`my_aspnet_sessioncleanup`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `odataplatform`.`my_aspnet_sessioncleanup` (
  `LastRun` DATETIME NOT NULL ,
  `IntervalMinutes` INT(11) NOT NULL )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_swedish_ci;
-- -----------------------------------------------------
-- Table `SCHEMA_NAME`.`my_aspnet_sessions`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `odataplatform`.`my_aspnet_sessions` (
  `SessionId` VARCHAR(255) NOT NULL ,
  `ApplicationId` INT(11) NOT NULL ,
  `Created` DATETIME NOT NULL ,
  `Expires` DATETIME NOT NULL ,
  `LockDate` DATETIME NOT NULL ,
  `LockId` INT(11) NOT NULL ,
  `Timeout` INT(11) NOT NULL ,
  `Locked` TINYINT(1) NOT NULL ,
  `SessionItems` LONGBLOB NULL DEFAULT NULL ,
  `Flags` INT(11) NOT NULL ,
  PRIMARY KEY (`SessionId`, `ApplicationId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_swedish_ci;
-- -----------------------------------------------------
-- Table `SCHEMA_NAME`.`my_aspnet_users`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `odataplatform`.`my_aspnet_users` (
  `id` INT(11) NOT NULL AUTO_INCREMENT ,
  `applicationId` INT(11) NOT NULL ,
  `name` VARCHAR(256) NOT NULL ,
  `isAnonymous` TINYINT(1) NOT NULL DEFAULT '1' ,
  `lastActivityDate` DATETIME NULL DEFAULT NULL ,
  PRIMARY KEY (`id`) )
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_swedish_ci;
-- -----------------------------------------------------
-- Table `SCHEMA_NAME`.`my_aspnet_usersinroles`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `odataplatform`.`my_aspnet_usersinroles` (
  `userId` INT(11) NOT NULL DEFAULT '0' ,
  `roleId` INT(11) NOT NULL DEFAULT '0' ,
  PRIMARY KEY (`userId`, `roleId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_swedish_ci
ROW_FORMAT = DYNAMIC;