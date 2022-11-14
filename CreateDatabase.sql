CREATE DATABASE `secure_password`;

use secure_password;

CREATE TABLE `users` (
  `iduser` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(45) NOT NULL,
  `passhash` varchar(130) NOT NULL,
  `login_status` int(11) NOT NULL,
  `login_attempts` int(11) DEFAULT NULL,
  PRIMARY KEY (`iduser`)
);

CREATE TABLE `peppers` (
  `idpepper` int(11) NOT NULL AUTO_INCREMENT,
  `pepper` varchar(70) NOT NULL,
  `userid` int(11) DEFAULT NULL,
  PRIMARY KEY (`idpepper`),
  KEY `userid_fk_idx` (`userid`),
  CONSTRAINT `userid_fk` FOREIGN KEY (`userid`) REFERENCES `users` (`iduser`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

