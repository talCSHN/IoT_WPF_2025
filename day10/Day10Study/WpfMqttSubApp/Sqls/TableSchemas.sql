﻿CREATE TABLE smarthome.fakedatas (
  sensing_dt DATETIME NOT NULL,
  pub_id VARCHAR(10) NOT NULL,
  count DECIMAL NOT NULL,
  temp DECIMAL(5,1) NOT NULL,
  humid DECIMAL(5,1) NOT NULL,
  light CHAR(1) NOT NULL,
  human CHAR(1) NOT NULL,
  PRIMARY KEY (sensing_dt, pub_id));

  CREATE TABLE smarthome.sensingdatas (
  idx BIGINT NOT NULL,
  sensing_dt DATETIME NOT NULL,
  light INT NOT NULL,
  rain INT NOT NULL,
  temp FLOAT NOT NULL,
  humid FLOAT NOT NULL,
  fan VARCHAR(3) NOT NULL,
  vul VARCHAR(3) NOT NULL,
  real_light VARCHAR(3) NOT NULL,
  chaim_bell VARCHAR(3) NOT NULL,
  PRIMARY KEY (idx));