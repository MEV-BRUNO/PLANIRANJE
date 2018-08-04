DROP DATABASE IF EXISTS planiranje;
CREATE DATABASE planiranje;
USE PLANIRANJE;

CREATE TABLE aktivnost (
  id_aktivnost int(11) NOT NULL AUTO_INCREMENT,
  naziv varchar(50) NOT NULL,
  PRIMARY KEY (id_aktivnost)
);

CREATE TABLE ciljevi (
  id_cilj int(11) NOT NULL AUTO_INCREMENT,
  naziv varchar(50),
  PRIMARY KEY (id_cilj)
);

CREATE TABLE oblici (
  id_oblici int(11) NOT NULL AUTO_INCREMENT,
  naziv varchar(50),
  PRIMARY KEY (id_oblici)
);

CREATE TABLE podrucje_rada (
  id_podrucje int(11) NOT NULL AUTO_INCREMENT,
  naziv varchar(50) NOT NULL,
  PRIMARY KEY (id_podrucje)
);

CREATE TABLE skola (
  id_skola int(11) NOT NULL AUTO_INCREMENT,
  naziv varchar(50) NOT NULL,
  adresa varchar(50) DEFAULT NULL,
  grad varchar(20) DEFAULT NULL,
  tel varchar(20) DEFAULT NULL,
  url varchar(50) DEFAULT NULL,
  kontakt varchar(20) DEFAULT NULL,
  PRIMARY KEY (id_skola)
);

CREATE TABLE subjekti (
  id_subjekt int(11) NOT NULL AUTO_INCREMENT,
  naziv varchar(50) ,
  PRIMARY KEY (id_subjekt)
);

CREATE TABLE zadaci (
  id_zadatak int(11) NOT NULL AUTO_INCREMENT,
  naziv varchar(50) DEFAULT NULL,
  PRIMARY KEY (id_zadatak)
);

CREATE TABLE pedagog (
  id_pedagog int(11) NOT NULL AUTO_INCREMENT,
  ime varchar(50) NOT NULL,
  prezime varchar(50) NOT NULL,
  email varchar(50) NOT NULL,
  lozinka varchar(40) NOT NULL,
  licenca datetime NOT NULL,
  id_skola int(11) NOT NULL,
  aktivan char(1) NOT NULL,
  titula varchar(50) NOT NULL,
  PRIMARY KEY (id_pedagog),
  UNIQUE KEY email (email),
  KEY id_skola (id_skola),
  CONSTRAINT pedagog_ibfk_1 FOREIGN KEY (id_skola) REFERENCES skola (id_skola)
);

CREATE TABLE os_plan_1 (
  id_plan int(11) NOT NULL AUTO_INCREMENT,
  id_pedagog int(11) NOT NULL,
  ak_godina varchar(25) NOT NULL,
  naziv varchar(50) NOT NULL,
  opis text,
  PRIMARY KEY (id_plan),
  KEY id_pedagog (id_pedagog),
  CONSTRAINT os_plan_1_ibfk_1 FOREIGN KEY (id_pedagog) REFERENCES pedagog (id_pedagog)
);

CREATE TABLE os_plan_2 (
  id_plan int(11) NOT NULL AUTO_INCREMENT,
  id_pedagog int(11) NOT NULL,
  ak_godina varchar(25) NOT NULL,
  naziv varchar(50) NOT NULL,
  opis text,
  PRIMARY KEY (id_plan),
  KEY id_pedagog (id_pedagog),
  CONSTRAINT os_plan_2_ibfk_1 FOREIGN KEY (id_pedagog) REFERENCES pedagog (id_pedagog)
);

CREATE TABLE ss_plan (
  id_plan int(11) NOT NULL AUTO_INCREMENT,
  id_pedagog int(11) NOT NULL,
  ak_godina varchar(25) NOT NULL,
  naziv varchar(50) NOT NULL,
  opis text,
  PRIMARY KEY (id_plan),
  KEY id_pedagog (id_pedagog),
  CONSTRAINT ss_plan_ibfk_1 FOREIGN KEY (id_pedagog) REFERENCES pedagog (id_pedagog)
);

CREATE TABLE mjesecni_plan (
  id_plan int(11) NOT NULL AUTO_INCREMENT,
  id_pedagog int(11) NOT NULL,
  id_godina int(11) NOT NULL,
  naziv varchar(50),
  opis text,
  PRIMARY KEY (id_plan),
  KEY id_pedagog (id_pedagog),
  CONSTRAINT mjesecni_plan_ibfk_1 FOREIGN KEY (id_pedagog) REFERENCES pedagog (id_pedagog)
);

CREATE TABLE godisnji_plan (
  id_god int(11) NOT NULL AUTO_INCREMENT,
  id_pedagog int(11) NOT NULL,
  ak_godina varchar(25) NOT NULL,
  br_radnih_dana int(11) DEFAULT NULL,
  br_dana_godina_odmor int(11) DEFAULT NULL,
  ukupni_rad_dana int(11) DEFAULT NULL,
  god_fond_sati int(11) DEFAULT NULL,
  PRIMARY KEY (id_god),
  KEY id_pedagog (id_pedagog),
  CONSTRAINT godisnji_plan_ibfk_1 FOREIGN KEY (id_pedagog) REFERENCES pedagog (id_pedagog)
);

CREATE TABLE godisnji_detalji (
  id int(11) NOT NULL AUTO_INCREMENT,
  id_god int(11) NOT NULL,
  mjesec int(11) NOT NULL,
  naziv_mjeseca varchar(25) NOT NULL,
  ukupno_dana int(11) NOT NULL,
  radnih_dana int(11) NOT NULL,
  subota_dana int(11) NOT NULL,
  nedjelja_dana int(11) NOT NULL,
  blagdana_dana int(11) NOT NULL,
  nastavnih_dana int(11) NOT NULL,
  praznika_dana int(11) NOT NULL,
  br_sati int(11) DEFAULT NULL,
  odmor_dana int(11) DEFAULT NULL,
  odmor_sati int(11) DEFAULT NULL,
  mj_fond_sati int(11) DEFAULT NULL,
  PRIMARY KEY (id),
  CONSTRAINT g_plan_det FOREIGN KEY (id_god) REFERENCES godisnji_plan(id_god) ON DELETE CASCADE
);

CREATE TABLE aktivnost_akcija (
  id_akcija int(11) NOT NULL AUTO_INCREMENT,
  naziv varchar(50) NOT NULL,
  id_aktivnost int(11) DEFAULT NULL,
  PRIMARY KEY (id_akcija),
  KEY id_aktivnost (id_aktivnost),
  CONSTRAINT aktivnost_akcija_ibfk_1 FOREIGN KEY (id_aktivnost) REFERENCES aktivnost (id_aktivnost)
);

CREATE TABLE dnevnik_rada (
  id_dnevnik int(11) NOT NULL AUTO_INCREMENT,
  id_pedagog int(11) NOT NULL,
  ak_godina varchar(25),
  naziv varchar(50) NOT NULL,
  opis text,
  datum datetime NOT NULL,
  PRIMARY KEY (id_dnevnik),
  KEY id_pedagog (id_pedagog),
  CONSTRAINT dnevnik_rada_ibfk_1 FOREIGN KEY (id_pedagog) REFERENCES pedagog (id_pedagog)
);

CREATE TABLE dnevnik_detalji (
  id_dnevnik int(11) NOT NULL AUTO_INCREMENT,
  subjekt int(11) NOT NULL,
  vrijeme_od datetime NOT NULL,
  vrijeme_do datetime NOT NULL,
  aktivnost int(11) NOT NULL,
  suradnja int(11) DEFAULT NULL,
  zakljucak text,
  PRIMARY KEY (id_dnevnik),
  KEY aktivnost (aktivnost),
  KEY subjekt (subjekt),
  KEY suradnja (suradnja),
  CONSTRAINT dnevnik_detalji_ibfk_1 FOREIGN KEY (aktivnost) REFERENCES aktivnost (id_aktivnost),
  CONSTRAINT dnevnik_detalji_ibfk_2 FOREIGN KEY (subjekt) REFERENCES subjekti (id_subjekt),
  CONSTRAINT dnevnik_detalji_ibfk_3 FOREIGN KEY (suradnja) REFERENCES pedagog (id_pedagog)
);

CREATE TABLE mjesecni_detalji (
  id int(11) NOT NULL AUTO_INCREMENT,
  id_plan int(11) NOT NULL,
  podrucje text,
  aktivnost text,
  suradnici text,
  vrijeme datetime NOT NULL,
  br_sati int(11) NOT NULL,
  biljeska text,
  PRIMARY KEY (id)
);

CREATE TABLE os_plan_2_akcija (
  id_plan int(11) NOT NULL AUTO_INCREMENT,
  red_br_podrucje int(11) NOT NULL,
  red_br_aktivnost int(11) NOT NULL,
  red_br_akcija int(11) NOT NULL,
  opis_akcija text,
  sati int(11) NOT NULL,
  PRIMARY KEY (id_plan),
  KEY red_br_podrucje (red_br_podrucje),
  KEY red_br_aktivnost (red_br_aktivnost),
  KEY red_br_akcija (red_br_akcija),
  CONSTRAINT os_plan_2_akcija_ibfk_1 FOREIGN KEY (red_br_podrucje) REFERENCES podrucje_rada (id_podrucje),
  CONSTRAINT os_plan_2_akcija_ibfk_2 FOREIGN KEY (red_br_aktivnost) REFERENCES aktivnost (id_aktivnost),
  CONSTRAINT os_plan_2_akcija_ibfk_3 FOREIGN KEY (red_br_akcija) REFERENCES aktivnost_akcija (id_akcija)
);

CREATE TABLE os_plan_1_akcija (
  id_plan int(11) NOT NULL AUTO_INCREMENT,
  red_br_podrucje int(11) NOT NULL,
  red_br_aktivnost int(11) NOT NULL,
  red_br_akcija int(11) NOT NULL,
  opis_akcija text,
  red_br int(11) NOT NULL,
  potrebno_sati int(11) NOT NULL,
  br_sati int(11) NOT NULL,
  mj_1 int(11) DEFAULT NULL,
  mj_2 int(11) DEFAULT NULL,
  mj_3 int(11) DEFAULT NULL,
  mj_4 int(11) DEFAULT NULL,
  mj_5 int(11) DEFAULT NULL,
  mj_6 int(11) DEFAULT NULL,
  mj_7 int(11) DEFAULT NULL,
  mj_8 int(11) DEFAULT NULL,
  mj_9 int(11) DEFAULT NULL,
  mj_10 int(11) DEFAULT NULL,
  mj_11 int(11) DEFAULT NULL,
  mj_12 int(11) DEFAULT NULL,
  PRIMARY KEY (id_plan),
  KEY red_broj_podrucje (red_br_podrucje),
  KEY red_broj_aktivnost (red_br_aktivnost),
  KEY red_broj_akcija (red_br_akcija),
  CONSTRAINT os_plan_1_akcija_ibfk_1 FOREIGN KEY (red_br_podrucje) REFERENCES podrucje_rada (id_podrucje),
  CONSTRAINT os_plan_1_akcija_ibfk_2 FOREIGN KEY (red_br_aktivnost) REFERENCES aktivnost (id_aktivnost),
  CONSTRAINT os_plan_1_akcija_ibfk_3 FOREIGN KEY (red_br_akcija) REFERENCES aktivnost_akcija (id_akcija)
);

CREATE TABLE os_plan_1_aktivnost (
  id_plan int(11) NOT NULL AUTO_INCREMENT,
  id_podrucje int(11) NOT NULL,
  red_broj_aktivnost int(11) NOT NULL,
  opis_aktivnost int(11) NOT NULL,  
  potrebno_sati text,
  br_sati int(11) DEFAULT NULL,
  mj_1 int(11) DEFAULT NULL,
  mj_2 int(11) DEFAULT NULL,
  mj_3 int(11) DEFAULT NULL,
  mj_4 int(11) DEFAULT NULL,
  mj_5 int(11) DEFAULT NULL,
  mj_6 int(11) DEFAULT NULL,
  mj_7 int(11) DEFAULT NULL,
  mj_8 int(11) DEFAULT NULL,
  mj_9 int(11) DEFAULT NULL,
  mj_10 int(11) DEFAULT NULL,
  mj_11 int(11) DEFAULT NULL,
  mj_12 int(11) DEFAULT NULL,
  PRIMARY KEY (id_plan),
  KEY opis_aktivnost (opis_aktivnost),
  KEY red_broj_aktivnost (red_broj_aktivnost),
  CONSTRAINT os_plan_1_aktivnost_ibfk_1 FOREIGN KEY (opis_aktivnost) REFERENCES aktivnost (id_aktivnost)  
);

CREATE TABLE os_plan_1_podrucje (
  id_plan int(11) NOT NULL AUTO_INCREMENT,
  id_glavni_plan int(11) NOT NULL,
  red_br_podrucje int(11) NOT NULL,
  opis_podrucje int(11) NOT NULL,
  potrebno_sati text,
  cilj int(11) NOT NULL,
  br_sati int(11) DEFAULT NULL,
  mj_1 int(11) DEFAULT NULL,
  mj_2 int(11) DEFAULT NULL,
  mj_3 int(11) DEFAULT NULL,
  mj_4 int(11) DEFAULT NULL,
  mj_5 int(11) DEFAULT NULL,
  mj_6 int(11) DEFAULT NULL,
  mj_7 int(11) DEFAULT NULL,
  mj_8 int(11) DEFAULT NULL,
  mj_9 int(11) DEFAULT NULL,
  mj_10 int(11) DEFAULT NULL,
  mj_11 int(11) DEFAULT NULL,
  mj_12 int(11) DEFAULT NULL,
  PRIMARY KEY (id_plan),
  KEY opis_podrucje (opis_podrucje),
  KEY cilj (cilj),
  CONSTRAINT os_plan_1_podrucje_ibfk_1 FOREIGN KEY (opis_podrucje) REFERENCES podrucje_rada (id_podrucje),
  CONSTRAINT os_plan_1_podrucje_ibfk_2 FOREIGN KEY (cilj) REFERENCES ciljevi (id_cilj)
);

CREATE TABLE os_plan_2_aktivnost (
  id_plan int(11) NOT NULL AUTO_INCREMENT,
  red_br_podrucje int(11) NOT NULL,
  red_br_aktivnost int(11) NOT NULL,
  opis_aktivnost text,
  cilj int(11) NOT NULL,
  zadaci int(11) DEFAULT NULL,
  subjekti int(11) NOT NULL,
  oblici int(11) NOT NULL,
  vrijeme datetime NOT NULL,
  sati int(11) NOT NULL,
  PRIMARY KEY (id_plan),
  KEY red_br_podrucje (red_br_podrucje),
  KEY red_br_aktivnost (red_br_aktivnost),
  KEY cilj (cilj),
  KEY subjekti (subjekti),
  KEY zadaci (zadaci),
  CONSTRAINT os_plan_2_aktivnost_ibfk_1 FOREIGN KEY (red_br_podrucje) REFERENCES podrucje_rada (id_podrucje),
  CONSTRAINT os_plan_2_aktivnost_ibfk_2 FOREIGN KEY (red_br_aktivnost) REFERENCES aktivnost (id_aktivnost),
  CONSTRAINT os_plan_2_aktivnost_ibfk_3 FOREIGN KEY (cilj) REFERENCES ciljevi (id_cilj),
  CONSTRAINT os_plan_2_aktivnost_ibfk_4 FOREIGN KEY (subjekti) REFERENCES subjekti (id_subjekt),
  CONSTRAINT os_plan_2_aktivnost_ibfk_5 FOREIGN KEY (zadaci) REFERENCES zadaci (id_zadatak)
);

CREATE TABLE os_plan_2_podrucje (
  id_plan int(11) NOT NULL AUTO_INCREMENT,
  red_br_podrucje int(11) NOT NULL,
  opis_podrucje text,
  cilj int(11) NOT NULL,
  zadaci int(11) DEFAULT NULL,
  subjekti int(11) NOT NULL,
  oblici int(11) NOT NULL,
  vrijeme datetime NOT NULL,
  sati int(11) NOT NULL,
  PRIMARY KEY (id_plan),
  KEY red_br_podrucje (red_br_podrucje),
  KEY cilj (cilj),
  KEY subjekti (subjekti),
  KEY oblici (oblici),
  KEY zadaci (zadaci),
  CONSTRAINT os_plan_2_podrucje_ibfk_1 FOREIGN KEY (red_br_podrucje) REFERENCES podrucje_rada (id_podrucje),
  CONSTRAINT os_plan_2_podrucje_ibfk_2 FOREIGN KEY (cilj) REFERENCES ciljevi (id_cilj),
  CONSTRAINT os_plan_2_podrucje_ibfk_3 FOREIGN KEY (subjekti) REFERENCES subjekti (id_subjekt),
  CONSTRAINT os_plan_2_podrucje_ibfk_4 FOREIGN KEY (oblici) REFERENCES oblici (id_oblici),
  CONSTRAINT os_plan_2_podrucje_ibfk_5 FOREIGN KEY (zadaci) REFERENCES zadaci (id_zadatak)
);

CREATE TABLE ss_plan_podrucje (
  id_plan int(11) NOT NULL AUTO_INCREMENT,
  red_br_podrucje int(11) NOT NULL,
  opis_podrucje text,
  svrha text,
  zadaca text,
  sadrzaj text,
  oblici int(11) NOT NULL,
  suradnici text,
  mjesto varchar(30),
  vrijeme datetime NOT NULL,
  ishodi text,
  sati int(11) NOT NULL,
  PRIMARY KEY (id_plan),
  KEY red_br_podrucje (red_br_podrucje),
  KEY oblici (oblici),
  CONSTRAINT ss_plan_podrucje_ibfk_1 FOREIGN KEY (red_br_podrucje) REFERENCES podrucje_rada (id_podrucje),
  CONSTRAINT ss_plan_podrucje_ibfk_2 FOREIGN KEY (oblici) REFERENCES oblici (id_oblici)
);
