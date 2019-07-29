DROP DATABASE IF EXISTS planiranje;
CREATE DATABASE planiranje;
USE PLANIRANJE;

CREATE TABLE pedagog (
  id_pedagog int(20) NOT NULL AUTO_INCREMENT,
  ime varchar(50) NOT NULL,
  prezime varchar(50) NOT NULL,
  email varchar(50) NOT NULL,
  lozinka varchar(40) NOT NULL,
  licenca datetime NOT NULL,  
  aktivan bool NOT NULL,
  titula varchar(50) NOT NULL,
  PRIMARY KEY (id_pedagog),
  UNIQUE KEY email (email)  
);

CREATE TABLE sk_godina (
  sk_godina int(20) NOT NULL,
  PRIMARY KEY (sk_godina)
);

CREATE TABLE skola (
  id_skola int(20) NOT NULL AUTO_INCREMENT,
  naziv varchar(50) NOT NULL,
  adresa varchar(50) DEFAULT NULL,
  grad varchar(20) DEFAULT NULL,
  tel varchar(20) DEFAULT NULL,
  url varchar(50) DEFAULT NULL,
  kontakt varchar(20) DEFAULT NULL,
  vrsta tinyint NOT NULL,
  PRIMARY KEY (id_skola)
);

CREATE TABLE nastavnik (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_skola int(20) NOT NULL,
  id_pedagog int(20) NOT NULL,
  ime text,
  prezime text,
  titula text,
  kontakt text,
  PRIMARY KEY (id),
  CONSTRAINT nastavnik_to_skola FOREIGN KEY (id_skola) REFERENCES skola(id_skola),
  CONSTRAINT nastavnik_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog(id_pedagog)
);

CREATE TABLE RazredniOdjel (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_skola int(20) NOT NULL,
  sk_godina int(20) NOT NULL,
  naziv text,
  razred tinyint,
  id_razrednik int(20),
  id_pedagog int(20),
  PRIMARY KEY (id),
  CONSTRAINT razredniOdjel_to_skola FOREIGN KEY (id_skola) REFERENCES skola(id_skola),
  CONSTRAINT razredniOdjel_to_nastavnik FOREIGN KEY (id_razrednik) REFERENCES nastavnik(id),
  CONSTRAINT razredniOdjel_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog(id_pedagog)
);

CREATE TABLE ucenik (
  id_ucenik int(20) NOT NULL AUTO_INCREMENT,
  ime text,
  prezime text,
  spol tinyint NOT NULL,
  oib varchar(11),
  grad text,
  adresa text,
  biljeska text,
  datum datetime,  
  id_razred int(20),
  PRIMARY KEY (id_ucenik)
);

CREATE TABLE obitelj (
  id_obitelj int(20) NOT NULL AUTO_INCREMENT,
  ime text,
  prezime text,
  svojstvo tinyint NOT NULL,
  adresa text,
  zanimanje text,
  kontakt text,
  id_ucenik int(20),
  PRIMARY KEY (id_obitelj),
  KEY (id_ucenik),
  CONSTRAINT obitelj_to_ucenik FOREIGN KEY (id_ucenik) REFERENCES ucenik(id_ucenik)
);

CREATE TABLE ucenik_razred (
  id int NOT NULL AUTO_INCREMENT,
  id_razred int(20) NOT NULL,
  id_ucenik int(20) NOT NULL,
  PRIMARY KEY (id),
  CONSTRAINT ucenik_razred_to_ucenik FOREIGN KEY (id_ucenik) REFERENCES ucenik(id_ucenik) ON DELETE CASCADE,
  CONSTRAINT ucenik_razred_to_razredniOdjel FOREIGN KEY (id_razred) REFERENCES RazredniOdjel(id) ON DELETE CASCADE
);

CREATE TABLE pedagog_skola (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_pedagog int(20) NOT NULL,
  id_skola int(20) NOT NULL,
  PRIMARY KEY (id),
  CONSTRAINT pedagog_skola_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog(id_pedagog),
  CONSTRAINT pedagog_skola_to_skola FOREIGN KEY (id_skola) REFERENCES skola(id_skola)  
);

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

CREATE TABLE aktivnost_akcija (
  id_akcija int(20) NOT NULL AUTO_INCREMENT,
  naziv varchar(50) NOT NULL,
  id_aktivnost int(20) DEFAULT NULL,
  PRIMARY KEY (id_akcija),
  KEY id_aktivnost (id_aktivnost),
  CONSTRAINT aktivnost_akcija_ibfk_1 FOREIGN KEY (id_aktivnost) REFERENCES aktivnost (id_aktivnost) ON DELETE CASCADE
);

CREATE TABLE pracenje_ucenika (
  id int(20) NOT NULL AUTO_INCREMENT,  
  id_ucenik_razred int(20) NOT NULL,
  id_pedagog int(20) NOT NULL,
  pocetak_pracenja datetime,
  razlog text,
  inic_procjena_ucenik text,
  inic_procjena_roditelj text,
  inic_procjena_razrednik text,
  soc_uvjeti text,
  ucenje text,
  soc_vjestine text,
  zakljucak text,
  PRIMARY KEY (id),
  KEY (id_ucenik_razred),
  CONSTRAINT pracenje_to_ucenik_razred FOREIGN KEY (id_ucenik_razred) REFERENCES ucenik_razred(id),
  CONSTRAINT pracenje_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog(id_pedagog)
);

CREATE TABLE postignuce (
  id_postignuce int(20) NOT NULL AUTO_INCREMENT,
  id_ucenik_razred int(20) NOT NULL,
  id_pedagog int(20) NOT NULL,
  razred int(20) NOT NULL,
  godina int(20) NOT NULL,
  napomena text,
  PRIMARY KEY (id_postignuce),  
  KEY (id_pedagog),
  CONSTRAINT postignuce_to_ucenik_razred FOREIGN KEY (id_ucenik_razred) REFERENCES ucenik_razred(id),
  CONSTRAINT postignuce_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog(id_pedagog)  
); 

CREATE TABLE neposredni_rad (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_ucenik_razred int(20) NOT NULL,
  id_pedagog int(20) NOT NULL,
  datum datetime,
  napomena text,
  PRIMARY KEY (id),
  KEY (id_pedagog),
  CONSTRAINT rad_to_ucenik_razred FOREIGN KEY (id_ucenik_razred) REFERENCES ucenik_razred(id),
  CONSTRAINT rad_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog(id_pedagog)
); 

CREATE TABLE popis_ucenika (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_ucenik_razred int(20) NOT NULL,
  ponavlja_razred tinyint,
  putnik tinyint,
  zaduzenje text,
  PRIMARY KEY (id),
  KEY (id_ucenik_razred),
  CONSTRAINT popis_to_ucenik_razred FOREIGN KEY (id_ucenik_razred) REFERENCES ucenik_razred(id)
);

CREATE TABLE ucenik_biljeska (
  id_biljeska int(20) NOT NULL AUTO_INCREMENT,
  id_ucenik_razred int(20) NOT NULL,
  id_pedagog int(20) NOT NULL,
  inicijalni_podaci text,
  zapazanje text,
  PRIMARY KEY (id_biljeska),
  KEY (id_ucenik_razred),
  CONSTRAINT biljeska_to_ucenik_razred FOREIGN KEY (id_ucenik_razred) REFERENCES ucenik_razred(id),
  CONSTRAINT biljeska_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog(id_pedagog)
);

CREATE TABLE mjesecna_biljeska (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_ucenik_biljeska int(20) NOT NULL,
  mjesec tinyint NOT NULL,
  biljeska text,
  sk_godina int(20) NOT NULL,
  PRIMARY KEY (id),
  KEY (id_ucenik_biljeska),
  CONSTRAINT mj_biljeska_to_biljeska FOREIGN KEY (id_ucenik_biljeska) REFERENCES ucenik_biljeska (id_biljeska) ON DELETE CASCADE
);

CREATE TABLE promatranje_ucenika (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_ucenik_razred int(20) NOT NULL,  
  id_pedagog int(20) NOT NULL,
  nadnevak datetime,
  vrijeme datetime,
  socstatusucenika text,
  cilj text,
  spremnost text,
  prilagodljivost text,
  odnos text,
  doprinos text,
  opis text,
  zakljucak text,
  PRIMARY KEY (id),
  KEY (id_ucenik_razred),
  CONSTRAINT promatranje_to_ucenik_razred FOREIGN KEY (id_ucenik_razred) REFERENCES ucenik_razred(id),
  CONSTRAINT promatranje_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog(id_pedagog)  
);

CREATE TABLE ucenik_zapisnik (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_ucenik_razred int(20) NOT NULL,
  id_pedagog int(20) NOT NULL,
  razlog text,
  odgojni_utjecaj_majka tinyint,
  odgojni_utjecaj_otac tinyint,
  procjena_statusa_obitelji text,
  odnos_prema_ucenju_majka tinyint,
  odnos_prema_ucenju_otac tinyint,
  suradnja_roditelja_majka tinyint,
  suradnja_roditelja_otac tinyint,
  odnos_s_prijateljima text,
  kako_provodi_slobodno_vrijeme text,
  procjena_mogucih_losih_utjecaja text,
  zdravstvene_poteskoce_ucenika text,
  podaci_o_naglim_promjenama text,
  izrecene_pedagoske_mjere text,
  PRIMARY KEY(id),
  CONSTRAINT zapisnik_to_ucenik_razred FOREIGN KEY (id_ucenik_razred) REFERENCES ucenik_razred(id),
  CONSTRAINT zapisnik_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog(id_pedagog)
);

CREATE TABLE ucenik_zapisnik_biljeska (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_ucenik_zapisnik int(20) NOT NULL,
  datum datetime,
  sadrzaj text,
  dogovor text,
  PRIMARY KEY(id),
  CONSTRAINT uc_zap_bilj_to_uc_zapisnik FOREIGN KEY (id_ucenik_zapisnik) REFERENCES ucenik_zapisnik(id) ON DELETE CASCADE
);

CREATE TABLE roditelj_biljeska (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_ucenik_razred int(20) NOT NULL,
  id_pedagog int(20) NOT NULL,
  id_roditelj int(20) NOT NULL,
  naslov varchar(100) NOT NULL,
  rujan text,
  listopad text,
  studeni text,
  prosinac text,
  sijecanj text,
  veljaca text,
  ozujak text,
  travanj text,
  svibanj text,
  lipanj text,
  zakljucak1 text,
  zakljucak2 text,
  zapazanje text,
  PRIMARY KEY (id),
  CONSTRAINT roditelj_biljeska_to_ucenik_razred FOREIGN KEY(id_ucenik_razred) REFERENCES ucenik_razred(id),
  CONSTRAINT roditelj_biljeska_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog(id_pedagog),
  CONSTRAINT roditelj_biljeska_to_obitelj FOREIGN KEY (id_roditelj) REFERENCES obitelj(id_obitelj)
);  

CREATE TABLE roditelj_procjena (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_ucenik_razred int(20) NOT NULL,
  id_pedagog int(20) NOT NULL,
  id_roditelj int(20) NOT NULL,
  naziv text,
  opis text,
  interes text,
  predmet text,
  gradivo text,
  boravak text,
  odnos text,
  aktivnosti text,
  hobiji text,
  ocekivanja text,
  dodatni_podaci text,
  PRIMARY KEY(id),
  CONSTRAINT roditelj_procjena_to_ucenik_razred FOREIGN KEY(id_ucenik_razred) REFERENCES ucenik_razred(id),
  CONSTRAINT roditelj_procjena_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog(id_pedagog),
  CONSTRAINT roditelj_procjena_to_obitelj FOREIGN KEY (id_roditelj) REFERENCES obitelj(id_obitelj)
);

CREATE TABLE roditelj_razgovor (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_ucenik_razred int(20) NOT NULL,
  id_pedagog int(20) NOT NULL,
  id_roditelj int(20) NOT NULL,
  datum datetime,
  vrijeme datetime,
  trazi text,
  razlog text,
  dolazak text,
  biljeska text,
  prijedlog_roditelja text,
  prijedlog_skole text,
  dogovor text,
  izvjestiti text,
  datum_slijedeci datetime,
  PRIMARY KEY(id),
  CONSTRAINT roditelj_razgovor_to_ucenik_razred FOREIGN KEY(id_ucenik_razred) REFERENCES ucenik_razred(id),
  CONSTRAINT roditelj_razgovor_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog(id_pedagog),
  CONSTRAINT roditelj_razgovor_to_obitelj FOREIGN KEY (id_roditelj) REFERENCES obitelj(id_obitelj)
);

CREATE TABLE roditelj_ugovor (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_ucenik_razred int(20) NOT NULL,
  id_pedagog int(20) NOT NULL,
  id_roditelj int(20) NOT NULL,
  datum datetime,
  cilj1 text,
  cilj2 text,
  poduzeto text,
  predstavnik_skole text,
  slijedeci_susret datetime,
  zapazanje text,
  biljeska1 text,
  biljeska2 text,
  biljeska3 text,
  biljeska4 text,
  biljeska5 text,
  biljeska6 text,
  izvjesce text,
  ostala_zapazanja text,
  PRIMARY KEY(id),
  CONSTRAINT roditelj_ugovor_to_ucenik_razred FOREIGN KEY(id_ucenik_razred) REFERENCES ucenik_razred(id),
  CONSTRAINT roditelj_ugovor_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog(id_pedagog),
  CONSTRAINT roditelj_ugovor_to_obitelj FOREIGN KEY (id_roditelj) REFERENCES obitelj(id_obitelj)
);

CREATE TABLE nastavnik_analiza (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_pedagog int(20) NOT NULL,
  id_nastavnik int(20) NOT NULL,
  id_skola int(20) NOT NULL,
  sk_godina int(20) NOT NULL,
  id_odjel int(20) NOT NULL,
  datum datetime,
  nastavni_sat text,
  cilj_posjete text,
  predmet text,
  nastavna_jedinica text,
  vrsta_nastavnog_sata text,
  planiranje_priprema text,
  izvedba_nastavnog_sata text,
  vodjenje_nastavnog_sata text,
  razredni_ugodjaj text,
  disciplina text,
  ocjenjivanje_ucenika text,
  osvrt text,
  prijedlozi text,
  uvid text,
  PRIMARY KEY (id),
  CONSTRAINT nas_analiza_to_odjel FOREIGN KEY(id_odjel) REFERENCES razredniodjel(id),
  CONSTRAINT nas_analiza_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog(id_pedagog),
  CONSTRAINT nas_analiza_to_nastavnik FOREIGN KEY (id_nastavnik) REFERENCES nastavnik(id),
  CONSTRAINT nas_analiza_to_skola FOREIGN KEY (id_skola) REFERENCES skola(id_skola)  
);

CREATE TABLE nastavnik_protokol (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_nastavnik int(20) NOT NULL,
  id_pedagog int(20) NOT NULL,
  id_skola int(20) NOT NULL,
  sk_godina int(20) NOT NULL,
  id_odjel int(20) NOT NULL,
  nastavni_predmet text,
  datum datetime,
  vrijeme datetime,
  broj_nastavnog_sata text,
  mjesto_izvodjenja text,
  nastavna_jedinica text,
  nastavna_cjelina text,
  cilj_i_zadaci_za_nastavnu_jedinicu tinyint,
  struktura_sadrzaja tinyint,
  plan_i_shvatljiv_koncept tinyint,
  plan_ploce tinyint,
  tip_nastavnog_sata tinyint,
  verbalne tinyint,
  vizualno_dokumentacijske tinyint,
  demonstracijske tinyint,
  prakseoloske tinyint,
  kombinirane tinyint,
  socioloski_oblici_rada tinyint,
  koristenje_nastavnih_sredstava text,
  funkcionalna_pripremljenost tinyint,
  uvod_i_najava_cilja tinyint,
  uspostavljanje_komunikacije tinyint,
  humor_u_nastavi tinyint,
  odrzavanje_paznje tinyint,
  sto_nastavnik_radi text,
  nas_strukturne_komponente text,
  nas_tijek_aktivnosti text,
  sto_ucenici_rade text,
  uc_strukturne_komponente text,
  uc_tijek_aktivnosti text,
  razgovor_s_ucenicima tinyint,
  ohrabruje_ucenike_za_iznosenje_misljenja tinyint,
  obeshrabruje_ucenikovu_aktivnost tinyint,
  uvazava_ucenicke_primjedbe tinyint,
  kritizira_ili_se_poziva_na_svoj_autoritet tinyint,
  pokazuje_empatiju tinyint,
  pomaze_ucenicima_koji_imaju_teskoce tinyint,
  neverbalnim_porukama_pridonosi_pozitivnom_radnom_ozracju tinyint,
  ucenik_ima_mogucnost_i_inicijativu_slobodnog_iznosenja_stavova tinyint,
  nastavnik_redovito_provjerava_uratke tinyint,
  daje_povratnu_informaciju tinyint,
  koristi_se_domacom_zadacom_kao_podlogom tinyint,
  daje_ocjenu_za_ucenje_u_razredu tinyint,
  kratki_komentar_nastavnika text,
  prijedlozi_za_daljnje_unapredjenje_rada text,
  PRIMARY KEY(id),
  CONSTRAINT nas_protokol_to_odjel FOREIGN KEY(id_odjel) REFERENCES razredniodjel(id),
  CONSTRAINT nas_protokol_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog(id_pedagog),
  CONSTRAINT nas_protokol_to_nastavnik FOREIGN KEY (id_nastavnik) REFERENCES nastavnik(id),
  CONSTRAINT nas_protokol_to_skola FOREIGN KEY (id_skola) REFERENCES skola(id_skola)
);

CREATE TABLE nastavnik_uvid (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_nastavnik int(20) NOT NULL,
  id_pedagog int(20) NOT NULL,
  id_skola int(20) NOT NULL,
  sk_godina int(20) NOT NULL,
  id_odjel int(20) NOT NULL,
  datum datetime,
  vrijeme datetime,
  nastavni_predmet text, 
  nastavna_cjelina text, 
  nastavna_jedinica text,
  broj_sati_nazocnosti text,        
  nastava_se_izvodi text,
  prostor_i_oprema text,
  estetsko_higijensko_stanje_ucionice text,
  materijalno_tehnicka_priprema_za_nastavu text,       
  nastavnik_se_redovito_priprema_za_nastavu text,
  nastavnikova_priprava_je text,
  priprava_sadrzi text,
  pripremanje_nastavnika_je_bilo_u_skladu_s_postignucima text,
  plan_ploce_u_pisanoj_pripravi text,
  pismena_priprava_sadrzi text,        
  didakticni_model_nastavnog_sata text,
  socioloski_oblici_rada text,
  nastavne_metode text,
  metodicke_strategije_postupci_i_oblici text,
  nastavna_sredstva_i_pomagala text,
  odgojno_obrazovni_sadrzaji_broj_novih_pojmova text,
  nastavne_metode_metodicki_postupci text,
  ciljevi_postignuca_i_kompetencije_ucenika text,
  odnos_nastavnika_prema_ucenicima text,
  nastavnik_posvecuje_pozornost text,
  nastavnikov_nastup text,
  govor_nastavnika_u_skladu_je text,
  kakvim_se_stilom_poucavanja_nastavnik_koristi text,        
  je_li_na_satu_dosao_do_izrazaja_ucenikov_rad text,
  postignuca_ucenika_i_produktivnost_sata_nastavnika text,
  domaca_zadaca_zadana_je text,
  karakter_domace_zadace text,
  domaca_zadaca_je_provjerena text,
  zapis_na_skolskoj_ploci_bio_je text,
  procjena_uspjesnosti_nastavnog_sata text,
  evaluacija_nastavnog_sata text,
  ostala_zapazanja text,        
  nastavnik_ima_i_vodi_pedagosku_dokumentaciju text,
  u_dnevniku_rada_upisani_su text,
  pripreme_nastavnika_za_nastavu_su text,
  iz_imenika_je_vidljivo_da_nastavnik text,
  ocjene_u_imeniku_su text,
  poslovi_razrednika_izvijesca_i_analize text,
  procjena_vodjenja_nastavne_dokumentacije text,
  PRIMARY KEY(id),
  CONSTRAINT nas_uvid_to_odjel FOREIGN KEY(id_odjel) REFERENCES razredniodjel(id),
  CONSTRAINT nas_uvid_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog(id_pedagog),
  CONSTRAINT nas_uvid_to_nastavnik FOREIGN KEY (id_nastavnik) REFERENCES nastavnik(id),
  CONSTRAINT nas_uvid_to_skola FOREIGN KEY (id_skola) REFERENCES skola(id_skola)  
);

CREATE TABLE nastavnik_obrazac (
  Id int(20) NOT NULL AUTO_INCREMENT,
  Id_nastavnik int(20) NOT NULL,
  Id_pedagog int(20) NOT NULL,
  Id_odjel int(20) NOT NULL,
  Sk_godina int(20) NOT NULL,
  Id_skola int(20) NOT NULL,
  Predmet text,
  Supervizor text,
  Nastavnik_pocetnik int,
  Mjesovita_dobna_skupina int,
  Br_ucenika_razred int,
  Br_stanovnika_zajednica int,
  Br_ucenika_skola int,
  Dobna_skupina text,
  Postotak_ucenika_obitelj int,
  Postotak_ucenika_jezik int,  
  Koriste_li_se_cesto_udzbenici int,
  Pokrivaju_li_udzbenici_i_metode int,
  Jesu_li_udzbenici_i_metode_poucavanja int,
  Koliko_se_sati_na_tjedan_posvecuje int,
  Koliko_se_ucenika_koristi_postupcima int,
  Koliko_se_puta_godisnje_testiraju_postignuca int,
  Dijagnosticira_li_nastavnik_probleme_u_ucenju int,
  Ima_li_nastavnik_propisane_nastavne_planove int,
  Provodi_li_nastavnik_propisane_planove int,  
  Stvara_opustenu_atmosferu tinyint,
  Djeci_se_obraca_na_pozitivan_nacin tinyint,
  Reagira_s_humorom_i_potice_humor tinyint,
  Dopusta_djeci_da_cine_pogreske tinyint,
  Iskazuje_toplinu_i_empatiju tinyint,
  Ucenicima_iskazuje_postovanje_rijecima_i_ponasanjem tinyint,
  Dopusta_ucenicima_da_zapoceto_izgovore_do_kraja tinyint,
  Slusa_sto_ucenici_imaju_za_reci tinyint,
  Ne_daje_primjedbe_kojima_naglasava_svoju_dominantnu_ulogu tinyint,
  Promice_uzajamno_postovanje tinyint,
  Potice_djecu_da_slusaju_jedni_druge tinyint,
  Intervenira_kad_su_djeca_ismijavana tinyint,
  Uzima_u_obzir_razlike_i_posebnosti tinyint,
  Potice_solidarnost_medju_ucenicima tinyint,
  Omogucuje_djeci_da_dogadjaje_i_aktivnosti_dozivljavaju tinyint,
  Potice_samopouzdanje_ucenika tinyint,
  Pozitivno_reagira_na_pitanja_i_odgovore_ucenika tinyint,
  Pohvaljuje_rezultate_ucenika tinyint,
  Pokazuje_da_cijeni_doprinos_ucenika tinyint,
  Ohrabruje_ucenike_da_daju_sve_od_sebe tinyint,
  Hvali_ucenike_kada_daju_sve_od_sebe tinyint,
  Jasno_pokazuje_da_od_svih_ucenika_ocekuje_da_daju_sve_od_sebe tinyint,
  Ucenicima_iskazuje_pozitivna_ocekivanja tinyint,
  Na_pocetku_sata_pojasnjava_nastavne_ciljeve tinyint,
  Na_pocetku_sata_informira_ucenike_o_ciljevima_sata tinyint,
  Razjasnjava_cilj_zadatka tinyint,
  Na_kraju_sata_procjenjuje_jesu_li_postignuti_nastavni_ciljevi tinyint,
  Provjerava_postignuca_ucenika tinyint,
  Provjerava_i_procjenjuje_jesu_li_postignuti_ciljevi_sata tinyint,
  Daje_jasne_upute_i_objasnjenja tinyint,
  Aktivira_prethodno_znanje_ucenika tinyint,
  Postupno_objasnjava tinyint,
  Postavlja_pitanja_koja_ucenici tinyint,
  Povremeno_sazima_nastavno_gradivo tinyint,
  Daje_jasna_objasnjenja_materijala_za_ucenje tinyint,
  Vodi_racuna_o_tome_da_svaki_ucenik tinyint,
  Objasnjava_kaku_su_zadaci_uskladjeni tinyint,
  Jasno_navodi_koji_se_materijali_mogu_koristiti tinyint,
  Ukljucuje_sve_ucenike_na_satu tinyint,
  Daje_zadatke_koji_poticu_ucenike tinyint,
  Ukljucuje_i_one_ucenike_koji_sami_ne_sudjeluju tinyint,
  Potice_ucenike_da_pazljivo_slusaju tinyint,
  Nakon_postavljenog_pitanja_ceka_dovoljno_dugo tinyint,
  Daje_priliku_ucenicima_koji_nisu_podigli_ruku tinyint,
  Koristi_se_nastavnim_metodama tinyint,
  Koristi_se_razlicitim_oblicima_razgovora tinyint,
  Daje_sve_slozenije_zadatke tinyint,
  Dopusta_rad_u_manjim_grupama tinyint,
  Koristi_se_informacijskom tinyint,
  Koristi_se_raznim_strategijama tinyint,
  Zadaje_razlicite_zadatke tinyint,
  Koristi_se_razlicitim_nastavnim_materijalima tinyint,
  Koristi_se_materijalima_i_primjerima_iz_zivota tinyint,
  Postavlja_puno_pitanja tinyint,
  Postavlja_pitanja_koja_poticu_razmisljanje tinyint,
  Daje_svim_ucenicima_dovoljno_vremena_za_razmisljanje tinyint,
  Ohrabruje_ucenike_da_postavljaju_jedni_drugima_pitanja tinyint,
  Trazi_ucenike_da_objasne_jedni_drugima tinyint,
  Redovito_provjerava_jesu_li_ucenici_razumjeli tinyint,
  Postavlja_pitanja_koja_poticu_davanje_povratne_informacije tinyint,
  PRIMARY KEY (id),
  CONSTRAINT nas_obrazac_to_odjel FOREIGN KEY(id_odjel) REFERENCES razredniodjel(id),
  CONSTRAINT nas_obrazac_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog(id_pedagog),
  CONSTRAINT nas_obrazac_to_nastavnik FOREIGN KEY (id_nastavnik) REFERENCES nastavnik(id),
  CONSTRAINT nas_obrazac_to_skola FOREIGN KEY (id_skola) REFERENCES skola(id_skola)  
);

CREATE TABLE dokument (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_pedagog int(20) NOT NULL,
  id_skola int(20) NOT NULL,
  path text,
  opis text,
  PRIMARY KEY(id),  
  CONSTRAINT dokument_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog(id_pedagog),  
  CONSTRAINT dokument_to_skola FOREIGN KEY (id_skola) REFERENCES skola(id_skola)  
);

CREATE TABLE os_plan_1 (
  id_plan int(20) NOT NULL AUTO_INCREMENT,
  id_pedagog int(20) NOT NULL,
  ak_godina int(20) NOT NULL,
  naziv varchar(50) NOT NULL,
  opis text,
  PRIMARY KEY (id_plan),
  KEY id_pedagog (id_pedagog),
  CONSTRAINT osp1_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog (id_pedagog)
);

CREATE TABLE os_plan_1_podrucje (
  id_plan int(20) NOT NULL AUTO_INCREMENT,
  id_glavni_plan int(20) NOT NULL,
  red_br_podrucje int(11) NOT NULL,
  opis_podrucje int(20) NOT NULL,
  potrebno_sati text,
  cilj text,
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
  CONSTRAINT osp1_pdrucje_to_podrucje_rada FOREIGN KEY (opis_podrucje) REFERENCES podrucje_rada (id_podrucje),
  CONSTRAINT osp1_to_osp1 FOREIGN KEY (id_glavni_plan) REFERENCES os_plan_1 (id_plan) ON DELETE CASCADE  
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
  CONSTRAINT osp1_aktivnost_to_aktivnost FOREIGN KEY (opis_aktivnost) REFERENCES aktivnost (id_aktivnost),
  CONSTRAINT osp1_aktivnost_to_osp1_podrucje FOREIGN KEY (id_podrucje) REFERENCES os_plan_1_podrucje (id_plan) ON DELETE CASCADE   
);

CREATE TABLE os_plan_1_akcija (
  id int(20) NOT NULL AUTO_INCREMENT,  
  id_aktivnost int(20) NOT NULL,
  red_br_akcija int(20) NOT NULL,
  opis_akcija text,  
  potrebno_sati text,
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
  PRIMARY KEY (id),
  CONSTRAINT osp1_akcija_to_osp1_aktivnost FOREIGN KEY (id_aktivnost) REFERENCES os_plan_1_aktivnost (id_plan) ON DELETE CASCADE 
);

CREATE TABLE os_plan_2 (
  id_plan int(20) NOT NULL AUTO_INCREMENT,
  id_pedagog int(20) NOT NULL,
  ak_godina int(20) NOT NULL,
  naziv text,
  opis text,
  PRIMARY KEY (id_plan),
  KEY id_pedagog (id_pedagog),
  CONSTRAINT osp2_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog (id_pedagog)
);

CREATE TABLE os_plan_2_podrucje (
  id_plan int(20) NOT NULL AUTO_INCREMENT,
  id_glavni_plan int(20) NOT NULL,
  red_br_podrucje int(11) NOT NULL,
  opis_podrucje text,
  cilj text,
  zadaci text,
  subjekti text,
  oblici text,
  vrijeme text,
  sati int(11) NOT NULL,   
  PRIMARY KEY (id_plan),
  CONSTRAINT osp2_podrucje_to_osp2 FOREIGN KEY (id_glavni_plan) REFERENCES os_plan_2 (id_plan) ON DELETE CASCADE  
);

CREATE TABLE os_plan_2_aktivnost (
  id_plan int(20) NOT NULL AUTO_INCREMENT,
  id_podrucje int(20) NOT NULL,
  red_br_aktivnost int(11) NOT NULL,
  opis_aktivnost text, 
  sati int(11) NOT NULL, 
  PRIMARY KEY (id_plan),
  CONSTRAINT osp2_aktivnost_to_osp2_podrucje FOREIGN KEY (id_podrucje) REFERENCES os_plan_2_podrucje (id_plan) ON DELETE CASCADE 
);

CREATE TABLE os_plan_2_akcija (
  id_plan int(20) NOT NULL AUTO_INCREMENT,  
  id_aktivnost int(20) NOT NULL,
  red_br_akcija int(11) NOT NULL,
  opis_akcija text,
  sati int(11) NOT NULL,
  PRIMARY KEY (id_plan),
  CONSTRAINT osp2_akcija_to_osp2_aktivnost FOREIGN KEY (id_aktivnost) REFERENCES os_plan_2_aktivnost (id_plan) ON DELETE CASCADE  
);

CREATE TABLE ss_plan (
  id_plan int(20) NOT NULL AUTO_INCREMENT,
  id_pedagog int(20) NOT NULL,
  ak_godina int(20) NOT NULL,
  naziv text,
  opis text,
  PRIMARY KEY (id_plan),
  KEY id_pedagog (id_pedagog),
  CONSTRAINT ss_plan_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog (id_pedagog)
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
  vrijeme text,
  ishodi text,
  sati int(20) NOT NULL,
  red_br int(20) NOT NULL,
  PRIMARY KEY (id),
  CONSTRAINT ss_plan_podrucje_to_ss_plan FOREIGN KEY (id_plan) REFERENCES ss_plan (id_plan) ON DELETE CASCADE
);

CREATE TABLE mjesecni_plan (
  id_plan int(20) NOT NULL AUTO_INCREMENT,
  id_pedagog int(20) NOT NULL,
  ak_godina int(20) NOT NULL,
  naziv text,
  opis text,
  PRIMARY KEY (id_plan),
  KEY id_pedagog (id_pedagog),
  CONSTRAINT mj_plan_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog (id_pedagog)
);

CREATE TABLE mjesecni_detalji (
  id int(20) NOT NULL AUTO_INCREMENT,
  id_plan int(20) NOT NULL,
  subjekti text,
  podrucje text,
  aktivnost text,
  suradnici text,
  vrijeme datetime NOT NULL,
  br_sati int(11) NOT NULL,
  biljeska text,
  PRIMARY KEY (id),
  CONSTRAINT mj_detalji_to_mj_plan FOREIGN KEY (id_plan) REFERENCES mjesecni_plan (id_plan) ON DELETE CASCADE
);

CREATE TABLE godisnji_plan (
  id_god int(20) NOT NULL AUTO_INCREMENT,
  naziv text,
  id_pedagog int(20) NOT NULL,
  ak_godina int(20) NOT NULL,
  br_radnih_dana int(11) DEFAULT NULL,
  br_dana_godina_odmor int(11) DEFAULT NULL,
  ukupni_rad_dana int(11) DEFAULT NULL,
  god_fond_sati int(11) DEFAULT NULL,
  PRIMARY KEY (id_god),
  KEY id_pedagog (id_pedagog),
  CONSTRAINT god_plan_to_pedagog FOREIGN KEY (id_pedagog) REFERENCES pedagog (id_pedagog)
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
  CONSTRAINT god_detalji_to_god_plan FOREIGN KEY (id_god) REFERENCES godisnji_plan (id_god) ON DELETE CASCADE
);

INSERT INTO skola VALUES (
  0, "1. OŠ Čakovec", "", "Čakovec", "", "", "",0
);

INSERT INTO skola VALUES (
  0, "2. OŠ Čakovec", "", "Čakovec", "", "", "",0
);

INSERT INTO skola VALUES (
  0, "3. OŠ Čakovec", "", "Čakovec", "", "", "",0
);

INSERT INTO skola VALUES (
  0, "Gimnazija J.Slavenskog Čakovec", "", "Čakovec", "", "", "",1
);

INSERT INTO skola VALUES (
  0, "Gospodarska škola Čakovec", "", "Čakovec", "", "", "",1
);

INSERT INTO skola VALUES (
  0, "Ekonomska škola Čakovec", "", "Čakovec", "", "", "",1
);

INSERT INTO skola VALUES (
  0, "Srednja škola Čakovec", "", "Čakovec", "", "", "",1
);

INSERT INTO skola VALUES (
  0, "Tehnička škola Čakovec", "", "Čakovec", "", "", "",1
);

INSERT INTO skola VALUES (
  0, "Graditeljska škola Čakovec", "", "Čakovec", "", "", "",1
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

INSERT INTO sk_godina VALUES (2018);
INSERT INTO sk_godina VALUES (2019);
INSERT INTO sk_godina VALUES (2020);
INSERT INTO sk_godina VALUES (2021);
INSERT INTO sk_godina VALUES (2022);
INSERT INTO sk_godina VALUES (2023);
INSERT INTO sk_godina VALUES (2024);
INSERT INTO sk_godina VALUES (2025);
INSERT INTO sk_godina VALUES (2026);
INSERT INTO sk_godina VALUES (2027);
INSERT INTO sk_godina VALUES (2028);
INSERT INTO sk_godina VALUES (2029);
