CREATE TABLE `moviefinder`.`movieitems` (
  `Id` INT NOT NULL,
  `Adult` BIT(1) NULL,
  `Backdrop_path` VARCHAR(100) NULL,
  `Original_language` VARCHAR(5) NULL,
  `Original_title` VARCHAR(250) NULL,
  `Overview` VARCHAR(1000) NULL,
  `Popularity` DECIMAL(6,2) NULL,
  `Poster_path` VARCHAR(100) NULL,
  `Release_date` DATETIME NULL,
  `Title` VARCHAR(250) NULL,
  `Vote_average` DECIMAL(6,2) NULL,
  `Vote_count` INT NULL,
  PRIMARY KEY (`Id`));