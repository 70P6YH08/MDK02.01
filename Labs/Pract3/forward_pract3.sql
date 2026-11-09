-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema pract3trpo
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema pract3trpo
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `pract3trpo` DEFAULT CHARACTER SET utf8mb3 ;
SHOW WARNINGS;
USE `pract3trpo` ;

-- -----------------------------------------------------
-- Table `pract3trpo`.`roles`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `pract3trpo`.`roles` (
  `role_id` BINARY(16) NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`role_id`),
  UNIQUE INDEX `name_UNIQUE` (`name` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `pract3trpo`.`users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `pract3trpo`.`users` (
  `user_id` BINARY(16) NOT NULL,
  `role_id` BINARY(16) NOT NULL,
  `first_name` VARCHAR(20) NULL DEFAULT NULL,
  `last_name` VARCHAR(20) NULL DEFAULT NULL,
  `login` VARCHAR(20) NOT NULL,
  `email` VARCHAR(40) NOT NULL,
  `passwordHash` VARCHAR(200) NOT NULL,
  `status` ENUM('Active', 'Inactive') NOT NULL DEFAULT 'Inactive',
  PRIMARY KEY (`user_id`),
  UNIQUE INDEX `login_UNIQUE` (`login` ASC) VISIBLE,
  UNIQUE INDEX `email_UNIQUE` (`email` ASC) VISIBLE,
  INDEX `fk_users_roles_idx` (`role_id` ASC) VISIBLE,
  CONSTRAINT `fk_users_roles`
    FOREIGN KEY (`role_id`)
    REFERENCES `pract3trpo`.`roles` (`role_id`)
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;

SHOW WARNINGS;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
