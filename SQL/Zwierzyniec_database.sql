DROP TABLE drzewo;
DROP TABLE uprawnienia;
  DROP TABLE uzytkownicy;
  DROP TABLE sklad_wizyty;
   DROP TABLE wizyta;
  DROP TABLE weterynarz;
    DROP TABLE pracownik;
  DROP TABLE lecznica;
  DROP TABLE usluga;
  DROP TABLE zwierze;
DROP TABLE klient;


CREATE TABLE `zwierzyniec`.`klient` (
    `pesel` BIGINT NOT NULL,
    `imie` VARCHAR(45),
    `nazwisko` VARCHAR(45),
    `adres` VARCHAR(45),
    `telefon_kontaktowy` VARCHAR(9),
    PRIMARY KEY (`pesel`)
);

CREATE TABLE `zwierze` (
    `id_zwierzecia` INT(11) NOT NULL auto_increment,
    `imie` VARCHAR(45),
    `gatunek` VARCHAR(45),
    `rasa` VARCHAR(45),
    `data_urodzenia` DATE,
    `ubarwienie` VARCHAR(45),
    `szczepienia` VARCHAR(45),
    `fk_klienta` BIGINT(11) NOT NULL,
    PRIMARY KEY (`id_zwierzecia`),
    FOREIGN KEY (`fk_klienta`)
        REFERENCES klient (pesel)
);


CREATE TABLE `zwierzyniec`.`usluga` (
    `id_uslugi` INT(11) NOT NULL auto_increment,
    `nazwa` VARCHAR(45),
    `koszta` INT(11),
    `dlugosc` INT(11),
    PRIMARY KEY (`id_uslugi`)
);

CREATE TABLE `zwierzyniec`.`lecznica` (
    `id_lecznicy` INT(11) NOT NULL auto_increment,
    `nazwa` VARCHAR(45),
    `adres` VARCHAR(45),
    `ilosc_pokoi_zabiegowych` INT(11),
    PRIMARY KEY (`id_lecznicy`)
);
  
CREATE TABLE `zwierzyniec`.`pracownik` (
    `pesel` BIGINT NOT NULL,
    `imie` VARCHAR(45),
    `nazwisko` VARCHAR(45),
    `wynagrodzenie` INT(11),
    `stanowisko` VARCHAR(45),
    `adres_zamieszkania` VARCHAR(45),
    `telefon_kontaktowy` INT(11),
    `fk_lecznicy` INT(11) NOT NULL,
    PRIMARY KEY (`pesel`),
    FOREIGN KEY (`fk_lecznicy`)
        REFERENCES lecznica (id_lecznicy)
);



CREATE TABLE `zwierzyniec`.`weterynarz` (
    `pesel` BIGINT NOT NULL,
    `tytul` VARCHAR(45),
    `specjalizacja` VARCHAR(45),
    PRIMARY KEY (`pesel`),
    FOREIGN KEY (`pesel`)
        REFERENCES pracownik (`pesel`)
);



  
CREATE TABLE `zwierzyniec`.`wizyta` (
    `id_wizyty` INT(11) NOT NULL auto_increment,
    `koszt` INT(11),
    `data` DATE,
    `godzina` TIME(0),
    `fk_zwierzecia` INT(11) NOT NULL,
    `fk_lecznicy` INT(11) NOT NULL,
    `fk_pesel` BIGINT NOT NULL,
    PRIMARY KEY (`id_wizyty`),
    FOREIGN KEY (`fk_zwierzecia`)
    REFERENCES zwierze (`id_zwierzecia`),
   FOREIGN KEY (`fk_lecznicy`)
        REFERENCES lecznica (`id_lecznicy`),
    FOREIGN KEY (`fk_pesel`)
      REFERENCES weterynarz (`pesel`)
);

CREATE TABLE `zwierzyniec`.`sklad_wizyty` (
    `fk_wizyty` INT(11) NOT NULL,
    `fk_uslugi` INT(11) NOT NULL,
    PRIMARY KEY (fk_wizyty , fk_uslugi),
    FOREIGN KEY (`fk_wizyty`)
        REFERENCES wizyta (`id_wizyty`),
    FOREIGN KEY (`fk_uslugi`)
        REFERENCES usluga (`id_uslugi`)
);
  
  
CREATE TABLE `zwierzyniec`.`uzytkownicy` (
    `id_uzytkownika` INT(11) NOT NULL auto_increment,
    `login` VARCHAR (20) ,
    `pass_hash` CHAR(128),
    `adres_e-mail` VARCHAR(45),
    `imie` VARCHAR(45),
    `nazwisko` VARCHAR(45),
    `data_utworzenia` DATE,
    `przejmij` bool,
    PRIMARY KEY (`id_uzytkownika`)
);

CREATE TABLE `zwierzyniec`.`uprawnienia` (
    `fk_uzytkownika` INT(11) NOT NULL,
    `zwierze` VARCHAR(8),
    `klient` VARCHAR(8),
    `usluga` VARCHAR(8),
    `wizyta` VARCHAR(8),
    `lecznica` VARCHAR(8),
    `pracownik` VARCHAR(8),
    `weterynarz` VARCHAR(8),
    PRIMARY KEY (`fk_uzytkownika`),
     FOREIGN KEY (`fk_uzytkownika`)
        REFERENCES uzytkownicy (`id_uzytkownika`)
);

CREATE TABLE `zwierzyniec`.`drzewo` (
	`fk_uzytkownika` INT(11) NOT NULL,
    `tablica` VARCHAR(20) NOT NULL,
    `fk_read` INT(11),
    `fk_create` INT(11),
    `fk_update` INT(11),
    `fk_delete` INT(11),
	`fk_read_przekaz` INT(11),
    `fk_create_przekaz` INT(11),
    `fk_update_przekaz` INT(11),
    `fk_delete_przekaz` INT(11),
   
		PRIMARY KEY (`fk_uzytkownika`,`tablica`),
						FOREIGN KEY (`fk_create`)
			references drzewo (`fk_uzytkownika`),
                        FOREIGN KEY (`fk_create_przekaz`)
			references drzewo (`fk_uzytkownika`),
                         FOREIGN KEY (`fk_read`)
			references drzewo (`fk_uzytkownika`),
                         FOREIGN KEY (`fk_read_przekaz`)
			references drzewo (`fk_uzytkownika`),
                         FOREIGN KEY (`fk_update`)
			references drzewo (`fk_uzytkownika`),
                         FOREIGN KEY (`fk_update_przekaz`)
			references drzewo (`fk_uzytkownika`),
						FOREIGN KEY (`fk_delete`)
			references drzewo (`fk_uzytkownika`),
                         FOREIGN KEY (`fk_delete_przekaz`)
			references drzewo (`fk_uzytkownika`),
			FOREIGN KEY (`fk_uzytkownika`)
			references uzytkownicy (`id_uzytkownika`)

);

INSERT INTO `klient` (`pesel`, `imie`, `nazwisko`, `adres`, `telefon_kontaktowy`) VALUES ('95101006752', 'Jan', 'Basalaj', 'kolobrzeska 10g2', '123321123');
INSERT INTO `klient` (`pesel`, `imie`, `nazwisko`, `adres`, `telefon_kontaktowy`) VALUES ('90101006752', 'Ewa', 'Barciak', 'opolska 10u4', '432234432');
INSERT INTO `klient` (`pesel`, `imie`, `nazwisko`, `adres`, `telefon_kontaktowy`) VALUES ('95051006752', 'Marcin', 'Chodkowski', 'chlopska 5a3', '465234432');
INSERT INTO `klient` (`pesel`, `imie`, `nazwisko`, `adres`, `telefon_kontaktowy`) VALUES ('90101206752', 'Artur', 'Suchocki', 'rzeczpospolitej 4r10', '765234432');
INSERT INTO `klient` (`pesel`, `imie`, `nazwisko`, `adres`, `telefon_kontaktowy`) VALUES ('90010106752', 'Blazej', 'Szewczyk', 'dabrowsczakow', '439234432');

INSERT INTO `zwierze` ( `imie`, `gatunek`, `rasa`, `data_urodzenia`, `ubarwienie`, `szczepienia`, `fk_klienta`) VALUES ( 'Shiva', 'kot domowy', 'Ragdoll', '2010-1-1', 'szare', 'brak', '95101006752');
INSERT INTO `zwierze` ( `imie`, `gatunek`, `rasa`, `data_urodzenia`, `ubarwienie`, `szczepienia`, `fk_klienta`) VALUES ( 'Obama', 'kot domowy', 'Europejski', '2011-1-1', 'czarne', 'brak', '95101006752');
INSERT INTO `zwierze` ( `imie`, `gatunek`, `rasa`, `data_urodzenia`, `ubarwienie`, `szczepienia`, `fk_klienta`) VALUES ( 'Emilka', 'kot domowy', 'Nebelung', '2012-2-3', 'szare', 'wscieklizna', '90101006752');
INSERT INTO `zwierze` ( `imie`, `gatunek`, `rasa`, `data_urodzenia`, `ubarwienie`, `szczepienia`, `fk_klienta`) VALUES ( 'Zofia', 'kot domowy', 'Europejski', '2008-2-3', 'biale', 'odra', '90101006752');
INSERT INTO `zwierze` ( `imie`, `gatunek`, `rasa`, `data_urodzenia`, `ubarwienie`, `szczepienia`, `fk_klienta`) VALUES ( 'Churchill', 'pies', 'Owczarek Niemiecki', '2006-2-3', 'czarne', 'odra', '90101006752');


INSERT INTO `lecznica` ( `nazwa`, `adres`, `ilosc_pokoi_zabiegowych`) VALUES ( 'Petmed', 'Obroncow wybrzeza 10', '5');
INSERT INTO `lecznica` ( `nazwa`, `adres`, `ilosc_pokoi_zabiegowych`) VALUES ( 'Aniheal', 'Olsztynska 7a', '2');
INSERT INTO `lecznica` ( `nazwa`, `adres`, `ilosc_pokoi_zabiegowych`) VALUES ( 'Futrolek', 'Grunwaldzka 321', '20');
INSERT INTO `lecznica` ( `nazwa`, `adres`, `ilosc_pokoi_zabiegowych`) VALUES ( 'VetHeaven', 'Traugutta', '1');
INSERT INTO `lecznica` ( `nazwa`, `adres`, `ilosc_pokoi_zabiegowych`) VALUES ( 'Klakopol', 'Olsztynska 10', '6');

INSERT INTO `pracownik` (`pesel`, `imie`, `nazwisko`, `wynagrodzenie`, `stanowisko`, `adres_zamieszkania`, `telefon_kontaktowy`, `fk_lecznicy`) VALUES ('60201004523', 'Wojciech', 'Brzydal', '2000', 'asystent', 'startowa 90a4', '123432234', '1');
INSERT INTO `pracownik` (`pesel`, `imie`, `nazwisko`, `wynagrodzenie`, `stanowisko`, `adres_zamieszkania`, `telefon_kontaktowy`, `fk_lecznicy`) VALUES ('78920101523', 'Adam', 'Kura', '3000', 'weterynarz', 'pilotow 7d4', '212123543', '2');
INSERT INTO `pracownik` (`pesel`, `imie`, `nazwisko`, `wynagrodzenie`, `stanowisko`, `adres_zamieszkania`, `telefon_kontaktowy`, `fk_lecznicy`) VALUES ('23545128912', 'Bodgan', 'Szpadel', '4000', 'recjepcjonista', 'bajana 2a3', '562123543', '2');
INSERT INTO `pracownik` (`pesel`, `imie`, `nazwisko`, `wynagrodzenie`, `stanowisko`, `adres_zamieszkania`, `telefon_kontaktowy`, `fk_lecznicy`) VALUES ('90652391911', 'Waclaw', 'Koniu', '3100', 'weterynarz', 'pilotow 8a2', '222123543', '1');
INSERT INTO `pracownik` (`pesel`, `imie`, `nazwisko`, `wynagrodzenie`, `stanowisko`, `adres_zamieszkania`, `telefon_kontaktowy`, `fk_lecznicy`) VALUES ('87123214112', 'Alicja', 'Pingwin', '5000', 'weterynarz', 'kolobrzeska 10f4', '792123543', '2');

INSERT INTO `weterynarz` (`pesel`, `tytul`, `specjalizacja`) VALUES ('90652391911', 'starszy weterynarz', 'konie');
INSERT INTO `weterynarz` (`pesel`, `tytul`, `specjalizacja`) VALUES ('78920101523', 'mlodszy weterynarz', 'koty domowe');
INSERT INTO `weterynarz` (`pesel`, `tytul`, `specjalizacja`) VALUES ('87123214112', 'starszy weterynarz', 'uchatki');

INSERT INTO `usluga` ( `nazwa`, `koszta`, `dlugosc`) VALUES ( 'leczenie zeba', '50', '20');
INSERT INTO `usluga` ( `nazwa`, `koszta`, `dlugosc`) VALUES ( 'sterylizacja kota', '500', '180');
INSERT INTO `usluga` ( `nazwa`, `koszta`, `dlugosc`) VALUES ( 'sterylizacja psa', '500', '180');
INSERT INTO `usluga` ( `nazwa`, `koszta`, `dlugosc`) VALUES ( 'usuniecie kleszcza', '50', '20');
INSERT INTO `usluga` ( `nazwa`, `koszta`, `dlugosc`) VALUES ( 'szczepienie na wscieklizne', '80', '15');

INSERT INTO `wizyta` ( `koszt`, `data`, `godzina`, `fk_zwierzecia`, `fk_lecznicy`, `fk_pesel`) VALUES ('60', '2010-2-3', '8:00:00', '1', '1', '90652391911');
INSERT INTO `wizyta` ( `koszt`, `data`, `godzina`, `fk_zwierzecia`, `fk_lecznicy`, `fk_pesel`) VALUES ( '800', '2011-5-4', '10:00:00', '2', '2', '90652391911');
INSERT INTO `wizyta` ( `koszt`, `data`, `godzina`, `fk_zwierzecia`, `fk_lecznicy`, `fk_pesel`) VALUES ( '200', '2011-6-4', '11:00:00', '1', '1', '90652391911');
INSERT INTO `wizyta` ( `koszt`, `data`, `godzina`, `fk_zwierzecia`, `fk_lecznicy`, `fk_pesel`) VALUES ( '40', '2011-2-4', '10:30:00', '2', '2', '78920101523');
INSERT INTO `wizyta` ( `koszt`, `data`, `godzina`, `fk_zwierzecia`, `fk_lecznicy`, `fk_pesel`) VALUES ( '50', '2011-10-4', '12:00:00', '2', '1', '78920101523');


INSERT INTO `sklad_wizyty` (`fk_wizyty`, `fk_uslugi`) VALUES ('1', '1');
INSERT INTO `sklad_wizyty` (`fk_wizyty`, `fk_uslugi`) VALUES ('2', '2');
INSERT INTO `sklad_wizyty` (`fk_wizyty`, `fk_uslugi`) VALUES ('2', '1');

INSERT INTO `uzytkownicy` (`login`, `pass_hash`,`adres_e-mail`,`imie` ,`nazwisko` ,`data_utworzenia` , `przejmij` ) VALUES ('Krzysztof','7D2A4394A858D3771CA600006131156BDB3B7120C3CC7147183F15DC7CAD0B1204D9081A835CD80D1078BA4D061A55E28AE4CA581FF4AD668833690F1E99AFE3','kr@gmail.com', 'Krzysztof','Dlugokinski','2014-2-4',1); -- 1
INSERT INTO `uzytkownicy` (`login`, `pass_hash`,`adres_e-mail`,`imie` ,`nazwisko` ,`data_utworzenia` , `przejmij` ) VALUES ('Zuzanna','46DF83E52559FCA8FD145E423F7919F1D0FCE81C679C4B3DC2CFFA6122226E7C9DAEFAE5AEF6966D104FF2BD737429FA1A663AFA1DF8A3461D63B6BCE434CA23','zuz@gmail.com', 'Zuzanna','Pusiewicz','2015-6-7',0); -- 2
INSERT INTO `uzytkownicy` (`login`, `pass_hash`,`adres_e-mail`,`imie` ,`nazwisko` ,`data_utworzenia` , `przejmij` ) VALUES ('Lech','DBCD53ED474EF928A2DB23EFAFD0291DB7305BC31B17BB4F467F351EDC4562E020DB9A5601B14049AD2DE2E72CC4D9969329FDF8ADC591FEFBABCEE26927E9B5','le@gmail.com', 'Lech','Abacki','2014-2-3',1); -- 3
INSERT INTO `uzytkownicy` (`login`, `pass_hash`,`adres_e-mail`,`imie` ,`nazwisko` ,`data_utworzenia` , `przejmij` ) VALUES ('Pawel','08673FC8C3E0AF6BD0F6E7E594E74FE9963A7A52BEA070596DE5EF2F039DD49F291895787EB64D324186EECB69FD97A41C005C42152921F390AED5172E84C0F0','paw@gmail.com', 'Pawel','Babacki','2014-5-4',0); -- 4
INSERT INTO `uzytkownicy` (`login`, `pass_hash`,`adres_e-mail`,`imie` ,`nazwisko` ,`data_utworzenia` , `przejmij` ) VALUES ('Weronika','D22EC59F0BAFBD264FCC7A664364A2E060546E67893B7BAAE00CFBEF9D51435CAFC7E3C8D1C4282F56DD75E35541A40E399A62B48BA54B17400AEEAC20499AE4','wer@gmail.com', 'Weronika','Cabacka','2014-2-6',1); -- 5
INSERT INTO `uzytkownicy` (`login`, `pass_hash`,`adres_e-mail`,`imie` ,`nazwisko` ,`data_utworzenia` , `przejmij` ) VALUES ('Urszula','8B24738F5366105924792A7A2A334FE36EA11C0A4623DAACFFD6484F32ED81CF709FD9FE02472F1267CBA6C7AA0CA3FBAB64A9CB5F414A4F7F7E28EE99C2F4E8','ur@gmail.com', 'Urszula','Dabacka','2015-1-4',1); -- 6
INSERT INTO `uzytkownicy` (`login`, `pass_hash`,`adres_e-mail`,`imie` ,`nazwisko` ,`data_utworzenia` , `przejmij` ) VALUES ('Wieslaw','7EDB91B69221875A7CCC497CC4B2CD6BD003D0279531D5B70A5A91EF9150DEF014B6580E38E263BD6F175C6520C684F1568D91504A414C4CC096E161015C4C69','wie@gmail.com', 'Wieslaw','Ebacki','2014-2-10',0); -- 7
INSERT INTO `uzytkownicy` (`login`, `pass_hash`,`adres_e-mail`,`imie` ,`nazwisko` ,`data_utworzenia` , `przejmij` ) VALUES ('Abraham','A219F6DFA15690A4594579AA9A2B6EAED7903DC4D91B1BCA2586902CACC5E1544C9C35EB9EA08F81BA0C52BFF9768B8BAFA0D2E79CB298FFE05FDA69BCCC1CFB','ab@gmail.com', 'Abraham','Abacki','2014-2-4',0); -- 8
INSERT INTO `uzytkownicy` (`login`, `pass_hash`,`adres_e-mail`,`imie` ,`nazwisko` ,`data_utworzenia` , `przejmij` ) VALUES ('Blazej','4F9C04EF7E1860AA71166DE357F24772B2715C5099A0843CC8CE93F1C622C4592FEA3F86BFA2DEE6880418BAF1F7AD295AC3550028DA501C73A444B0D68B250A','bl@gmail.com', 'Blazej','Babacki','2014-2-4',0); -- 9
INSERT INTO `uzytkownicy` (`login`, `pass_hash`,`adres_e-mail`,`imie` ,`nazwisko` ,`data_utworzenia` , `przejmij` ) VALUES ('Celina','E28E4FA69D84FBEC2A489E59890B2D021227E6D9F18035C5320273F382AC2B12D171E0D4573955EC80107FDFC99B8E67415948ECC737867838DAEA85D3949084','cel@gmail.com', 'Celina','Cabacki','2014-2-4',0); -- 10
INSERT INTO `uzytkownicy` (`login`, `pass_hash`,`adres_e-mail`,`imie` ,`nazwisko` ,`data_utworzenia` , `przejmij` ) VALUES ('Dominika','FE00C2ADB5881706DCEFDB479D23C1726233D41E777B5A9956DB1C1FE13C4A4205E6158DD1DDBF9F326D95728BDE107FD443DF3A8DABD06F77CAA864384859CE','dom@gmail.com', 'Dominika','Dabacki','2014-2-4',0); -- 11
INSERT INTO `uzytkownicy` (`login`, `pass_hash`,`adres_e-mail`,`imie` ,`nazwisko` ,`data_utworzenia` , `przejmij` ) VALUES ('admin','C7AD44CBAD762A5DA0A452F9E854FDC1E0E7A52A38015F23F3EAB1D80B931DD472634DFAC71CD34EBC35D16AB7FB8A90C81F975113D6C7538DC69DD8DE9077EC','admin@gmail.com', '','','2014-2-4',0); -- 12


INSERT INTO `uprawnienia` (`fk_uzytkownika`, `zwierze` ,`klient` ,`usluga` ,`wizyta` ,`lecznica` ,`pracownik` ,`weterynarz` ) VALUES ('1','10000000','11111111','10000000','10000000','10000000','10000000','10000000'); -- 1
INSERT INTO `uprawnienia` (`fk_uzytkownika`, `zwierze` ,`klient` ,`usluga` ,`wizyta` ,`lecznica` ,`pracownik` ,`weterynarz` ) VALUES ('2','11111111','10000000','10000000','10000000','10000000','10000000','10000000'); -- 2
INSERT INTO `uprawnienia` (`fk_uzytkownika`, `zwierze` ,`klient` ,`usluga` ,`wizyta` ,`lecznica` ,`pracownik` ,`weterynarz` ) VALUES ('3','10000000','10000000','10000000','10000000','11111111','10000000','10000000'); -- 3
INSERT INTO `uprawnienia` (`fk_uzytkownika`, `zwierze` ,`klient` ,`usluga` ,`wizyta` ,`lecznica` ,`pracownik` ,`weterynarz` ) VALUES ('4','10000000','10000000','10000000','10000000','10000000','11111111','10000000'); -- 4
INSERT INTO `uprawnienia` (`fk_uzytkownika`, `zwierze` ,`klient` ,`usluga` ,`wizyta` ,`lecznica` ,`pracownik` ,`weterynarz` ) VALUES ('5','10000000','10000000','10000000','10000000','10000000','10000000','11111111'); -- 5
INSERT INTO `uprawnienia` (`fk_uzytkownika`, `zwierze` ,`klient` ,`usluga` ,`wizyta` ,`lecznica` ,`pracownik` ,`weterynarz` ) VALUES ('6','10000000','10000000','11111111','10000000','10000000','10000000','10000000'); -- 6
INSERT INTO `uprawnienia` (`fk_uzytkownika`, `zwierze` ,`klient` ,`usluga` ,`wizyta` ,`lecznica` ,`pracownik` ,`weterynarz` ) VALUES ('7','10000000','10000000','10000000','11111111','10000000','10000000','10000000'); -- 7
INSERT INTO `uprawnienia` (`fk_uzytkownika`, `zwierze` ,`klient` ,`usluga` ,`wizyta` ,`lecznica` ,`pracownik` ,`weterynarz` ) VALUES ('8','11110000','11110000','11110000','11110000','11110000','11110000','11110000'); -- 8
INSERT INTO `uprawnienia` (`fk_uzytkownika`, `zwierze` ,`klient` ,`usluga` ,`wizyta` ,`lecznica` ,`pracownik` ,`weterynarz` ) VALUES ('9','00000000','00000000','00000000','00000000','00000000','00000000','00000000'); -- 9
INSERT INTO `uprawnienia` (`fk_uzytkownika`, `zwierze` ,`klient` ,`usluga` ,`wizyta` ,`lecznica` ,`pracownik` ,`weterynarz` ) VALUES ('10','11110000','11000000','10000000','11100000','00000000','00000000','00000000'); -- 10
INSERT INTO `uprawnienia` (`fk_uzytkownika`, `zwierze` ,`klient` ,`usluga` ,`wizyta` ,`lecznica` ,`pracownik` ,`weterynarz` ) VALUES ('11','00000000','00000000','00000000','00000000','11110000','11100000','11110000'); -- 11
INSERT INTO `uprawnienia` (`fk_uzytkownika`, `zwierze` ,`klient` ,`usluga` ,`wizyta` ,`lecznica` ,`pracownik` ,`weterynarz` ) VALUES ('12','11111111','11111111','11111111','11111111','11111111','11111111','11111111'); -- 12


INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (12,'zwierze',null,null,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (2,'zwierze',12,12,12,12,12,12,12,12);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (8,'zwierze',2,2,2,2,null,null,null,null);
  INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (10,'zwierze',2,2,2,2,null,null,null,null);
    INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (1,'zwierze',null,2,null,null,null,null,null,null);
     INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (3,'zwierze',null,2,null,null,null,null,null,null);
     INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (4,'zwierze',null,2,null,null,null,null,null,null);
   INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (5,'zwierze',null,2,null,null,null,null,null,null);
      INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (6,'zwierze',null,2,null,null,null,null,null,null);
     INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (7,'zwierze',null,2,null,null,null,null,null,null);
      INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (9,'zwierze',null,null,null,null,null,null,null,null);
      INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (11,'zwierze',null,null,null,null,null,null,null,null);
 
 
INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (12,'klient',null,null,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (1,'klient',12,12,12,12,12,12,12,12);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (2,'klient',null,1,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (3,'klient',null,1,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (4,'klient',null,1,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (5,'klient',null,1,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (6,'klient',null,1,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (7,'klient',null,1,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (8,'klient',1,1,1,1,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (9,'klient',null,null,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (10,'klient',1,1,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (11,'klient',null,null,null,null,null,null,null,null);
 
 
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (12,'usluga',null,null,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (1,'usluga',null,6,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (2,'usluga',null,6,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (3,'usluga',null,6,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (4,'usluga',null,6,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (5,'usluga',null,6,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (6,'usluga',12,12,12,12,12,12,12,12);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (7,'usluga',null,6,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (8,'usluga',6,6,6,6,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (9,'usluga',null,null,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (10,'usluga',null,6,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (11,'usluga',null,null,null,null,null,null,null,null);
 
 
 
  INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (12,'wizyta',null,null,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (1,'wizyta',null,7,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (2,'wizyta',null,7,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (3,'wizyta',null,7,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (4,'wizyta',null,7,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (5,'wizyta',null,7,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (6,'wizyta',null,7,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (7,'wizyta',12,12,12,12,12,12,12,12);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (8,'wizyta',7,7,7,7,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (9,'wizyta',null,null,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (10,'wizyta',7,7,7,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (11,'wizyta',null,null,null,null,null,null,null,null);


  INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (12,'lecznica',null,null,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (1,'lecznica',null,3,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (2,'lecznica',null,3,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (3,'lecznica',12,12,12,12,12,12,12,12);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (4,'lecznica',null,3,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (5,'lecznica',null,3,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (6,'lecznica',null,3,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (7,'lecznica',null,3,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (8,'lecznica',3,3,3,3,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (9,'lecznica',null,null,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (10,'lecznica',null,null,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (11,'lecznica',3,3,3,3,null,null,null,null);
 
 
   INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (12,'pracownik',null,null,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (1,'pracownik',null,4,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (2,'pracownik',null,4,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (3,'pracownik',null,4,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (4,'pracownik',12,12,12,12,12,12,12,12);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (5,'pracownik',null,4,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (6,'pracownik',null,4,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (7,'pracownik',null,4,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (8,'pracownik',4,4,4,4,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (9,'pracownik',null,null,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (10,'pracownik',null,null,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (11,'pracownik',4,4,4,null,null,null,null,null);
 
    INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (12,'weterynarz',null,null,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (1,'weterynarz',null,5,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (2,'weterynarz',null,5,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (3,'weterynarz',null,5,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (4,'weterynarz',null,5,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (5,'weterynarz',12,12,12,12,12,12,12,12);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (6,'weterynarz',null,5,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (7,'weterynarz',null,5,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (8,'weterynarz',5,5,5,5,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (9,'weterynarz',null,null,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (10,'weterynarz',null,null,null,null,null,null,null,null);
 INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_read` ,`fk_update` ,`fk_delete` ,`fk_create_przekaz`,`fk_read_przekaz`,`fk_update_przekaz`, `fk_delete_przekaz` )
 VALUES (11,'weterynarz',5,5,5,5,null,null,null,null);



-- INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`) VALUES ('12', 'klient');
