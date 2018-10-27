DROP DATABASE IF EXISTS planiranje;
CREATE DATABASE planiranje;
USE PLANIRANJE;

CREATE TABLE aktivnost (
  id_aktivnost int(20) NOT NULL AUTO_INCREMENT,
  naziv text,
  vrsta int NOT NULL,
  PRIMARY KEY (id_aktivnost)
);

CREATE TABLE ciljevi (
  id_cilj int(20) NOT NULL AUTO_INCREMENT,
  naziv text,
  vrsta int NOT NULL,
  PRIMARY KEY (id_cilj)
);

CREATE TABLE oblici (
  id_oblici int(20) NOT NULL AUTO_INCREMENT,
  naziv text,
  vrsta int NOT NULL,
  PRIMARY KEY (id_oblici)
);

CREATE TABLE podrucje_rada (
  id_podrucje int(20) NOT NULL AUTO_INCREMENT,
  naziv text,
  vrsta int NOT NULL,
  PRIMARY KEY (id_podrucje)
);

CREATE TABLE skola (
  id_skola int(20) NOT NULL AUTO_INCREMENT,
  naziv varchar(50) NOT NULL,
  adresa varchar(50) DEFAULT NULL,
  grad varchar(20) DEFAULT NULL,
  tel varchar(20) DEFAULT NULL,
  url varchar(50) DEFAULT NULL,
  kontakt varchar(20) DEFAULT NULL,
  PRIMARY KEY (id_skola)
);

CREATE TABLE subjekti (
  id_subjekt int(20) NOT NULL AUTO_INCREMENT,
  naziv text,
  vrsta int NOT NULL,
  PRIMARY KEY (id_subjekt)
);

CREATE TABLE zadaci (
  id_zadatak int(20) NOT NULL AUTO_INCREMENT,
  naziv text,
  vrsta int NOT NULL,
  PRIMARY KEY (id_zadatak)
);

CREATE TABLE pedagog (
  id_pedagog int(20) NOT NULL AUTO_INCREMENT,
  ime varchar(50) NOT NULL,
  prezime varchar(50) NOT NULL,
  email varchar(50) NOT NULL,
  lozinka varchar(40) NOT NULL,
  licenca datetime NOT NULL,
  id_skola int(20) NOT NULL,
  aktivan char(1) NOT NULL,
  titula varchar(50) NOT NULL,
  PRIMARY KEY (id_pedagog),
  UNIQUE KEY email (email),
  KEY id_skola (id_skola),
  CONSTRAINT pedagog_ibfk_1 FOREIGN KEY (id_skola) REFERENCES skola (id_skola)
);

CREATE TABLE os_plan_1 (
  id_plan int(20) NOT NULL AUTO_INCREMENT,
  id_pedagog int(20) NOT NULL,
  ak_godina int(20) NOT NULL,
  naziv varchar(50) NOT NULL,
  opis text,
  PRIMARY KEY (id_plan),
  KEY id_pedagog (id_pedagog),
  CONSTRAINT os_plan_1_ibfk_1 FOREIGN KEY (id_pedagog) REFERENCES pedagog (id_pedagog)
);

CREATE TABLE os_plan_2 (
  id_plan int(20) NOT NULL AUTO_INCREMENT,
  id_pedagog int(20) NOT NULL,
  ak_godina int(20) NOT NULL,
  naziv text,
  opis text,
  PRIMARY KEY (id_plan),
  KEY id_pedagog (id_pedagog),
  CONSTRAINT os_plan_2_ibfk_1 FOREIGN KEY (id_pedagog) REFERENCES pedagog (id_pedagog)
);

CREATE TABLE ss_plan (
  id_plan int(20) NOT NULL AUTO_INCREMENT,
  id_pedagog int(20) NOT NULL,
  id_godina int(20) NOT NULL,
  naziv varchar(50) NOT NULL,
  opis text,
  PRIMARY KEY (id_plan),
  KEY id_pedagog (id_pedagog),
  CONSTRAINT ss_plan_ibfk_1 FOREIGN KEY (id_pedagog) REFERENCES pedagog (id_pedagog)
);

CREATE TABLE mjesecni_plan (
  id_plan int(20) NOT NULL AUTO_INCREMENT,
  id_pedagog int(20) NOT NULL,
  id_godina int(20) NOT NULL,
  naziv varchar(50),
  opis text,
  PRIMARY KEY (id_plan),
  KEY id_pedagog (id_pedagog),
  CONSTRAINT mjesecni_plan_ibfk_1 FOREIGN KEY (id_pedagog) REFERENCES pedagog (id_pedagog)
);

CREATE TABLE godisnji_plan (
  id_god int(20) NOT NULL AUTO_INCREMENT,
  id_pedagog int(20) NOT NULL,
  ak_godina int(20) NOT NULL,
  br_radnih_dana int(11) DEFAULT NULL,
  br_dana_godina_odmor int(11) DEFAULT NULL,
  ukupni_rad_dana int(11) DEFAULT NULL,
  god_fond_sati int(11) DEFAULT NULL,
  PRIMARY KEY (id_god),
  KEY id_pedagog (id_pedagog),
  CONSTRAINT godisnji_plan_ibfk_1 FOREIGN KEY (id_pedagog) REFERENCES pedagog (id_pedagog)
);

CREATE TABLE godisnji_detalji (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_god int(20) NOT NULL,
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
  id_akcija int(20) NOT NULL AUTO_INCREMENT,
  naziv varchar(50) NOT NULL,
  id_aktivnost int(20) DEFAULT NULL,
  PRIMARY KEY (id_akcija),
  KEY id_aktivnost (id_aktivnost),
  CONSTRAINT aktivnost_akcija_ibfk_1 FOREIGN KEY (id_aktivnost) REFERENCES aktivnost (id_aktivnost) ON DELETE CASCADE
);

CREATE TABLE dnevnik_rada (
  id_dnevnik int(20) NOT NULL AUTO_INCREMENT,
  id_pedagog int(20) NOT NULL,
  ak_godina int(20) NOT NULL,
  naziv varchar(50) NOT NULL,
  opis text,
  datum datetime NOT NULL,
  PRIMARY KEY (id_dnevnik),
  KEY id_pedagog (id_pedagog),
  CONSTRAINT dnevnik_rada_ibfk_1 FOREIGN KEY (id_pedagog) REFERENCES pedagog (id_pedagog)
);

CREATE TABLE dnevnik_detalji (
  id_dnevnik int(20) NOT NULL AUTO_INCREMENT,
  subjekt int(20) NOT NULL,
  vrijeme_od datetime NOT NULL,
  vrijeme_do datetime NOT NULL,
  aktivnost int(20) NOT NULL,
  suradnja int(20) DEFAULT NULL,
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
  id int(20) NOT NULL AUTO_INCREMENT,
  id_plan int(20) NOT NULL,
  podrucje text,
  aktivnost text,
  suradnici text,
  vrijeme datetime NOT NULL,
  br_sati int(11) NOT NULL,
  biljeska text,
  PRIMARY KEY (id)
);

CREATE TABLE os_plan_2_akcija (
  id_plan int(20) NOT NULL AUTO_INCREMENT,  
  id_aktivnost int(20) NOT NULL,
  red_br_akcija int(11) NOT NULL,
  opis_akcija text,
  sati int(11) NOT NULL,
  PRIMARY KEY (id_plan)   
);

CREATE TABLE os_plan_1_akcija (
  id_plan int(20) NOT NULL AUTO_INCREMENT,
  red_br_podrucje int(20) NOT NULL,
  red_br_aktivnost int(20) NOT NULL,
  red_br_akcija int(20) NOT NULL,
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
  id_plan int(20) NOT NULL AUTO_INCREMENT,
  id_podrucje int(20) NOT NULL,
  red_broj_aktivnost int(11) NOT NULL,
  opis_aktivnost int(20) NOT NULL,  
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
  id_plan int(20) NOT NULL AUTO_INCREMENT,
  id_glavni_plan int(20) NOT NULL,
  red_br_podrucje int(11) NOT NULL,
  opis_podrucje int(20) NOT NULL,
  potrebno_sati text,
  cilj int(20) NOT NULL,
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
  id_plan int(20) NOT NULL AUTO_INCREMENT,
  id_podrucje int(20) NOT NULL,
  red_br_aktivnost int(11) NOT NULL,
  opis_aktivnost text, 
  sati int(11) NOT NULL, 
  PRIMARY KEY (id_plan)  
);

CREATE TABLE os_plan_2_podrucje (
  id_plan int(20) NOT NULL AUTO_INCREMENT,
  id_glavni_plan int(20) NOT NULL,
  red_br_podrucje int(11) NOT NULL,
  opis_podrucje text,
  cilj int(20) NOT NULL,
  zadaci int(20) DEFAULT NULL,
  subjekti int(20) NOT NULL,
  oblici int(20) NOT NULL,
  vrijeme text,
  sati int(11) NOT NULL,   
  PRIMARY KEY (id_plan),   
  CONSTRAINT os_plan_2_podrucje_ibfk_2 FOREIGN KEY (cilj) REFERENCES ciljevi (id_cilj),
  CONSTRAINT os_plan_2_podrucje_ibfk_3 FOREIGN KEY (subjekti) REFERENCES subjekti (id_subjekt),
  CONSTRAINT os_plan_2_podrucje_ibfk_4 FOREIGN KEY (oblici) REFERENCES oblici (id_oblici),
  CONSTRAINT os_plan_2_podrucje_ibfk_5 FOREIGN KEY (zadaci) REFERENCES zadaci (id_zadatak)
);

CREATE TABLE ss_plan_podrucje (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_plan int(20) NOT NULL,
  opis_podrucje text,
  svrha text,
  zadaca text,
  sadrzaj text,
  oblici text,
  suradnici text,
  mjesto text,
  vrijeme datetime NOT NULL,
  ishodi text,
  sati int(11) NOT NULL,
  PRIMARY KEY (id)
);

INSERT INTO skola VALUES (
  1, "Međimursko veleučiliste", "Bana Josipa Jelačića", "Čakovec", "", "", ""
);

INSERT INTO skola VALUES (
  2, "Sveučiliste Sjever", "", "Varaždin", "", "", ""
);

INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Utvrđivanje obrazovnih potreba učenika, škole i okruženja");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Analiza odgojno-obrazovnih postignuća");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "SWOT analiza rada škole");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Kratkoročni i dugoročni razvojni plan rada škole i stručnog suradnika pedagoga");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Izvedbeno planiranje i programiranje");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Ostvarivanje uvjeta za realizaciju programa");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Upis učenika i formiranje razrednih odjela");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Uvođenje novih programa i inovacija");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Praćenje i izvođenje odgojno-obrazovnog rada");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Rad s učenicima sa posebnim potrebama: uočavanje, poticanje, praćenje darovitih učenika");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Savjetodavni rad i suradnja");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Profesionalno usmjeravanje i informiranje učenika");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Zdravstvena i socijalna zaštita učenika");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Sudjelovanje u realizaciji Programa kulturne i javne djelatnosti škole");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Vrednovanje u odnosu na utvrđene ciljeve");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Istraživanja u funkciji osuvremenjivanja");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Stručno usavršavanje pedagoga");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Stručno usavršavanje učitelja");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Stručno usavršavanje nastavnika");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Bibliotečno-informacijska djelatnost");
INSERT INTO aktivnost (vrsta, naziv) VALUES (0, "Dokumentacijska djelatnost");

INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Ispitivanje i utvrđivanje odgojno-obrazovnih potreba učenika, škole i okruženja, izvršiti pripremu za bolje i kvalitetnije planiranje odgojno-obrazovnog rada");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Osmišljanje i kreiranje kratkoročnoga i dugoročnoga razvoja škole");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Praćenje razvoja i odgojno-obrazovnih postignuća učenika");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Povezivanje škole s lokalnom i širom zajednicom");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Uvođenje i praćenje inovacija u svim sastavnicama odgojno-obrazovnog procesa");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Praćenje novih spoznaja iz područja odgojnih znanosti i njihovu primjenu u nastavnom i školskom radu");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Unaprjeđivanje kvalitete procesa upisa djece u školu");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Utvrđivanje pripremljenosti i zrelosti djece za školu");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Postizanje ujednačenih grupa učenika unutar svih razrednih odjela 1. razreda");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Stvaranje uvjeta za uspješan početak školovanja");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Osuvremenjivanje nastavnog procesa");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Doprinos rada stručnih tijela škole");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Razvoj stručnih kompetencija");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Preventivno djelovanje");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Osiguranje primjerenog odgojno-obrazovnog tretmana, uvođenje u novo školsko okruženje, podrška u prevladavanju odgojno-obrazovnih teškoća");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Podizanje kvalitete nastavnog procesa");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Koordinacija rada");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Savjetovanje, pružanje pomoći i podrške");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Koordinacija aktivnosti");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Informiranje učenika");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Pružanje pomoći u donošenju odluke o profesionalnoj budućnosti");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Analiza odgojno-obrazovnih rezultata");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Podizanje stručne kompetencije");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Kontinuirano stručno usavršavanje, cjeloživotno učenje");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Obogaćivanje i prenošenje znanja");
INSERT INTO ciljevi (vrsta, naziv) VALUES (0, "Sudjelovanje u ostvarivanju optimalnih uvjeta za individualno stručno usavršavanje, inoviranje novih izvora znanja");

INSERT INTO oblici (vrsta, naziv) VALUES (0, "Individualni");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Grupni");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Timski");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Rasprava");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Rad na testu");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Pisanje");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Proučavanje pedagoške dokumentacije");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Analitičko promatranje");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Savjetovanje");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Analiza dječjeg crteža");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Obrada podataka i rada na testu");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Savjetovanje djece, roditelja i učitelja");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Parlaonica");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Predavanje");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Savjetodavni rad");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Pedagoško praćenje učenika");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Metoda razgovora i obrada podataka");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Obrada anketa");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Pismeni i likovni radovi");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Metode istraživačkog rada");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Metoda otvorenog iskustvenog učenja");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Demonstracije");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Anketa");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Radionica");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Diskusije");
INSERT INTO oblici (vrsta, naziv) VALUES (0, "Kritičko mišljenje");

INSERT INTO subjekti (vrsta, naziv) VALUES (0, "Psihologinja");
INSERT INTO subjekti (vrsta, naziv) VALUES (0, "Ravnatelj");
INSERT INTO subjekti (vrsta, naziv) VALUES (0, "Nastavnici");
INSERT INTO subjekti (vrsta, naziv) VALUES (0, "Učenici");
INSERT INTO subjekti (vrsta, naziv) VALUES (0, "Razrednici");
INSERT INTO subjekti (vrsta, naziv) VALUES (0, "Voditelji izvannastavnih aktivnosti");
INSERT INTO subjekti (vrsta, naziv) VALUES (0, "Voditelji turnusa");
INSERT INTO subjekti (vrsta, naziv) VALUES (0, "Vanjski suradnici");
INSERT INTO subjekti (vrsta, naziv) VALUES (0, "Drugi stručni suradnici");
INSERT INTO subjekti (vrsta, naziv) VALUES (0, "Povjerenstvo za kvalitetu");
INSERT INTO subjekti (vrsta, naziv) VALUES (0, "Voditelji stručnih vijeća");
INSERT INTO subjekti (vrsta, naziv) VALUES (0, "Svi suradnici u odgojno-obrazovnom procesu");
INSERT INTO subjekti (vrsta, naziv) VALUES (0, "Vanjski suradnici mentori");
INSERT INTO subjekti (vrsta, naziv) VALUES (0, "Knjižničarka");
INSERT INTO subjekti (vrsta, naziv) VALUES (0, "ŽVS pedagoga");
INSERT INTO subjekti (vrsta, naziv) VALUES (0, "Druge institucije");
INSERT INTO subjekti (vrsta, naziv) VALUES (0, "Svi zainteresirani");

INSERT INTO podrucje_rada (vrsta, naziv) VALUES (0, "Poslovi pripreme za ostvarenje školskog programa");
INSERT INTO podrucje_rada (vrsta, naziv) VALUES (0, "Poslovi neposrednog sudjelovanja u odgojno-obrazovnom procesu");
INSERT INTO podrucje_rada (vrsta, naziv) VALUES (0, "Vrednovanje ostvarenih rezultata, studijske analize");
INSERT INTO podrucje_rada (vrsta, naziv) VALUES (0, "Stručno usavršavanje odgojno-obrazovnih djelatnika");
INSERT INTO podrucje_rada (vrsta, naziv) VALUES (0, "Bibliotečno-informacijska i dokumentacijska djelatnost");
INSERT INTO podrucje_rada (vrsta, naziv) VALUES (0, "Ostali poslovi");

INSERT INTO zadaci (vrsta, naziv) VALUES (0, "Analizirati realizaciju prijašnjih planova i programa rada škole");
INSERT INTO zadaci (vrsta, naziv) VALUES (0, "Utvrditi odgojno-obrazovne potrebe okruženja u kojem škola djeluje");
INSERT INTO zadaci (vrsta, naziv) VALUES (0, "Planirati i programirati godišnji plan rada škole, plan rada pedagoga, plan rada učitelja nastavnih predmeta i prilagođene programe");
INSERT INTO zadaci (vrsta, naziv) VALUES (0, "Ostvariti uvjete za realizaciju programa");
INSERT INTO zadaci (vrsta, naziv) VALUES (0, "Aktualizirati plan dugoročnog razvoja škole");
INSERT INTO zadaci (vrsta, naziv) VALUES (0, "Analizirati realizaciju prijašnjih planova");
INSERT INTO zadaci (vrsta, naziv) VALUES (0, "Rad na osmišljavanju suvremenog didaktičko-metodičnog ostvarivanja odgojno-obrazovnog procesa");
INSERT INTO zadaci (vrsta, naziv) VALUES (0, "Primijeniti nove spoznaje u radu sa svim subjektima odgojno-obrazovnog procesa");
INSERT INTO zadaci (vrsta, naziv) VALUES (0, "Utvrditi stilove života i navike učenja učenika");
INSERT INTO zadaci (vrsta, naziv) VALUES (0, "Upoznati učenike s osnovnim pojmovima i zakonitostima učenja, pamćenja i zaboravljanja");
INSERT INTO zadaci (vrsta, naziv) VALUES (0, "Unaprijediti učinkovitost procesa i rezultata odgojno-obrazovnog rada");