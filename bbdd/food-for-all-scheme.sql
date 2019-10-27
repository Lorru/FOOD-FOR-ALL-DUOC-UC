-- IP IIS = 3.91.208.66
-- IIS PASSWORD = &ElOdGyWa5F&Gms9H6=lOfxP=%!t83cu
-- SQL PASSWORD = VEwPWp6gb99SNna4
-- USER SQL = dev

DROP TABLE EventLog;
DROP TABLE EventLogType;
DROP TABLE GlobalSetting;
DROP TABLE Location;
DROP TABLE StockComment;
DROP TABLE [dbo].[Message];
DROP TABLE StockReceived;
DROP TABLE StockAvailable;
DROP TABLE CalificationStock;
DROP TABLE StockImage;
DROP TABLE TypeMessage;
DROP TABLE Stock;
DROP TABLE Product;
DROP TABLE ProductType;
DROP TABLE Token;
DROP TABLE CalificationUser;
DROP TABLE Denounced;
DROP TABLE ListBlack;
DROP TABLE [dbo].[User];
DROP TABLE UserType;
DROP TABLE Institution;

CREATE TABLE UserType(
Id INTEGER IDENTITY(1,1) NOT NULL,
Name VARCHAR(50) NOT NULL,
Description VARCHAR(100) NULL,
Status BIT NOT NULL,
PRIMARY KEY(Id)
);

CREATE TABLE Institution(
Id INTEGER IDENTITY(1,1) NOT NULL,
Name VARCHAR(150) NOT NULL,
Rut VARCHAR(20) NOT NULL,
Activity VARCHAR(150) NULL,
Address VARCHAR(100) NULL,
Commune VARCHAR(100) NULL,
Phone INTEGER NULL,
Email VARCHAR(50) NULL,
Status BIT NOT NULL,
PRIMARY KEY(Id)
);

CREATE TABLE [dbo].[User](
Id INTEGER IDENTITY(1,1) NOT NULL,
IdUserType INTEGER NOT NULL,
IdInstitution INTEGER NULL,
UserName VARCHAR(50) NOT NULL,
Password VARCHAR(MAX) NULL,
Email VARCHAR(50) NOT NULL,
Phone INTEGER NULL,
Photo VARCHAR(MAX) NULL,
OneSignalPlayerId VARCHAR(MAX) NULL,
Star INTEGER NULL,
DateOfAdmission DATETIME NOT NULL,
IsWithFacebook BIT NOT NULL,
OnLine BIT NULL,
Status BIT NOT NULL,
PRIMARY KEY(Id),
FOREIGN KEY(IdUserType) REFERENCES UserType(Id),
FOREIGN KEY(IdInstitution) REFERENCES Institution(Id)
);

CREATE TABLE CalificationUser(
Id INTEGER IDENTITY(1,1) NOT NULL,
IdUser INTEGER NOT NULL,
IdUserCalification INTEGER NOT NULL,
Calification INTEGER NOT NULL,
PRIMARY KEY(Id),
FOREIGN KEY(IdUser) REFERENCES [dbo].[User](Id),
FOREIGN KEY(IdUserCalification) REFERENCES [dbo].[User](Id)    
);

CREATE TABLE Denounced(
Id INTEGER IDENTITY(1,1) NOT NULL,
IdUser INTEGER NOT NULL,
IdUserAccuser INTEGER NOT NULL,
Reason VARCHAR(MAX) NULL,
PRIMARY KEY(Id),
FOREIGN KEY(IdUser) REFERENCES [dbo].[User](Id),
FOREIGN KEY(IdUserAccuser) REFERENCES [dbo].[User](Id)
);

CREATE TABLE ListBlack(
Id INTEGER IDENTITY(1,1) NOT NULL,
IdUser INTEGER NOT NULL,
Phone INTEGER NULL,
Email VARCHAR(50) NOT NULL,
OneSignalPlayerId VARCHAR(MAX) NULL,
PRIMARY KEY(Id),
FOREIGN KEY(IdUser) REFERENCES [dbo].[User](Id)
);

CREATE TABLE Location(
Id INTEGER IDENTITY(1,1) NOT NULL,
IdUser INTEGER NOT NULL,
Longitude FLOAT NOT NULL,
Latitude FLOAT NOT NULL,
Address VARCHAR(200) NOT NULL,
Country VARCHAR(50) NOT NULL,
IsMain BIT NOT NULL,
Status BIT NOT NULL,
PRIMARY KEY(Id),
FOREIGN KEY(IdUser) REFERENCES [dbo].[User](Id)
);

CREATE TABLE ProductType(
Id INTEGER IDENTITY(1,1) NOT NULL,
Name VARCHAR(50) NOT NULL,
Description VARCHAR(100) NULL,
ReferenceImage VARCHAR(MAX) NULL,
Status BIT NOT NULL,
PRIMARY KEY(Id)
);

CREATE TABLE Product(
Id INTEGER IDENTITY(1,1) NOT NULL,
IdProductType INTEGER NOT NULL,
Name VARCHAR(50) NOT NULL,
Description VARCHAR(100) NULL,
ReferenceImage VARCHAR(MAX) NULL,
Status BIT NOT NULL,
PRIMARY KEY(Id),
FOREIGN KEY(IdProductType) REFERENCES ProductType(Id)
);

CREATE TABLE Stock(
Id INTEGER IDENTITY(1,1) NOT NULL,
IdUser INTEGER NOT NULL,
IdProduct INTEGER NOT NULL,
Quantity INTEGER NOT NULL,
IsAvailable BIT NOT NULL,
DateOfAdmission DATETIME NOT NULL,
UpdateDate DATETIME NULL,
ExpirationDate DATETIME NULL,
Observation VARCHAR(MAX) NULL,
Star INTEGER NULL,
Status BIT NOT NULL,
PRIMARY KEY(Id),
FOREIGN KEY(IdUser) REFERENCES [dbo].[User](Id),
FOREIGN KEY(IdProduct) REFERENCES Product(Id)
);

CREATE TABLE CalificationStock(
Id INTEGER IDENTITY(1,1) NOT NULL,
IdStock INTEGER NOT NULL,
IdUserCalification INTEGER NOT NULL,
Calification INTEGER NOT NULL,
PRIMARY KEY(Id),
FOREIGN KEY(IdStock) REFERENCES Stock(Id),
FOREIGN KEY(IdUserCalification) REFERENCES [dbo].[User](Id),
);

CREATE TABLE StockAvailable(
Id INTEGER IDENTITY(1,1) NOT NULL,
IdStock INTEGER NOT NULL,
DateOfAdmission DATETIME NOT NULL,
PRIMARY KEY(Id),
FOREIGN KEY(IdStock) REFERENCES Stock(Id),
);

CREATE TABLE StockImage(
Id INTEGER IDENTITY(1,1) NOT NULL,
IdStock INTEGER NOT NULL,
ReferenceImage VARCHAR(MAX) NOT NULL,
PRIMARY KEY(Id),
FOREIGN KEY(IdStock) REFERENCES Stock(Id),
);

CREATE TABLE StockReceived(
Id INTEGER IDENTITY(1,1) NOT NULL,
IdStock INTEGER NOT NULL,
IdUserBeneficiary INTEGER NOT NULL,
Quantity INTEGER NOT NULL,
Date DATE NOT NULL,
PRIMARY KEY(Id),
FOREIGN KEY(IdStock) REFERENCES Stock(Id),
FOREIGN KEY(IdUserBeneficiary) REFERENCES [dbo].[User](Id)
);

CREATE TABLE TypeMessage(
Id INTEGER IDENTITY(1,1) NOT NULL,
Name VARCHAR(50) NOT NULL,
Description VARCHAR(100) NULL,
Status BIT NOT NULL,
PRIMARY KEY(Id)
);

CREATE TABLE [dbo].[Message](
Id INTEGER IDENTITY(1,1) NOT NULL,
IdTypeMessage INTEGER NOT NULL,
IdUserSend INTEGER NOT NULL,
IdUserReceived INTEGER NOT NULL,
Message VARCHAR(MAX) NOT NULL,
Date DATETIME NOT NULL,
PRIMARY KEY(Id),
FOREIGN KEY(IdTypeMessage) REFERENCES TypeMessage(Id),
FOREIGN KEY(IdUserSend) REFERENCES [dbo].[User](Id),
FOREIGN KEY(IdUserReceived) REFERENCES [dbo].[User](Id)
);

CREATE TABLE StockComment(
Id INTEGER IDENTITY(1,1) NOT NULL,
IdTypeMessage INTEGER NOT NULL,
IdStock INTEGER NOT NULL,
IdUser INTEGER NOT NULL,
Comment VARCHAR(MAX) NULL,
Date DATETIME NOT NULL,
PRIMARY KEY(Id),
FOREIGN KEY(IdStock) REFERENCES Stock(Id),
FOREIGN KEY(IdTypeMessage) REFERENCES TypeMessage(Id),
FOREIGN KEY(IdUser) REFERENCES [dbo].[User](Id)
);

CREATE TABLE GlobalSetting(
Id INTEGER IDENTITY(1,1) NOT NULL,
Property VARCHAR(50) NOT NULL,
Value VARCHAR(100) NULL,
Status BIT NOT NULL,
PRIMARY KEY(Id)
);

CREATE TABLE EventLogType(
Id INTEGER IDENTITY(1,1) NOT NULL,
Name VARCHAR(50) NOT NULL,
Description VARCHAR(100) NULL,
Status BIT NOT NULL,
PRIMARY KEY(Id)
);

CREATE TABLE EventLog(
Id INTEGER IDENTITY(1,1) NOT NULL,
IdUser INTEGER NULL,
IdEventLogType INTEGER NOT NULL,
Controller VARCHAR(50) NOT NULL,
Method VARCHAR(100) NOT NULL,
HttpMethod VARCHAR(50) NOT NULL,
Host VARCHAR(50) NOT NULL,
Message VARCHAR(MAX) NULL,
Date DATETIME NOT NULL,
PRIMARY KEY(Id),
FOREIGN KEY(IdUser) REFERENCES [dbo].[User](Id),
FOREIGN KEY(IdEventLogType) REFERENCES EventLogType(Id)
);

CREATE TABLE Token(
Id INTEGER IDENTITY(1,1) NOT NULL,
IdUser INTEGER NOT NULL,
Token VARCHAR(MAX) NOT NULL,
Host VARCHAR(50) NOT NULL,
ExpirationDate DATETIME NOT NULL,
PRIMARY KEY(Id),
FOREIGN KEY(IdUser) REFERENCES [dbo].[User](Id)
);

INSERT INTO UserType (Name, Status) VALUES ('ADMINISTRADOR', 1);
INSERT INTO UserType (Name, Status) VALUES ('DONADOR', 1);
INSERT INTO UserType (Name, Status) VALUES ('RECEPTOR', 1);

INSERT INTO TypeMessage (Name, Status) VALUES ('TEXT', 1);
INSERT INTO TypeMessage (Name, Status) VALUES ('IMAGE', 1);

INSERT INTO EventLogType (Name, Status) VALUES ('EVENT', 1);
INSERT INTO EventLogType (Name, Status) VALUES ('EXCEPTION', 1);

INSERT INTO ProductType (Name, Status) VALUES ('FRUTAS Y VERDURAS', 1);
INSERT INTO ProductType (Name, Status) VALUES ('FRUTOS SECOS Y SEMILLAS', 1);
INSERT INTO ProductType (Name, Status) VALUES ('CEREALES Y LEGUMBRES', 1);
INSERT INTO ProductType (Name, Status) VALUES ('CARNES, HUEVOS Y PESCADOS', 1);
INSERT INTO ProductType (Name, Status) VALUES ('LACTEOS Y DERIVADOS', 1);
INSERT INTO ProductType (Name, Status) VALUES ('ACEITES Y AZUCARES', 1);

INSERT INTO GlobalSetting (Property, Value, Status) VALUES ('DURATION_TOKEN', '50', 1);
INSERT INTO GlobalSetting (Property, Value, Status) VALUES ('CHARACTER_TOKEN', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789', 1);
INSERT INTO GlobalSetting (Property, Value, Status) VALUES ('LENGTH_TOKEN', '50', 1);
INSERT INTO GlobalSetting (Property, Value, Status) VALUES ('EMAIL_ADDRESS', 'appfoodforall@gmail.com', 1);
INSERT INTO GlobalSetting (Property, Value, Status) VALUES ('EMAIL_PASSWORD', 'UwQ9HgyhpPyR7Kdt', 1);
INSERT INTO GlobalSetting (Property, Value, Status) VALUES ('EMAIL_NAME', 'Food For All', 1);
INSERT INTO GlobalSetting (Property, Value, Status) VALUES ('EMAIL_PORT', '587', 1);
INSERT INTO GlobalSetting (Property, Value, Status) VALUES ('EMAIL_SMTP', 'smtp.gmail.com', 1);
INSERT INTO GlobalSetting (Property, Value, Status) VALUES ('EMAIL_PATH_TEMPLATE', 'C:\inetpub\wwwroot\food-for-all-api\Template\template-recovery-password.html', 1);

INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'AJO', 'ajo.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'ACELGA', 'acelga.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'AGUACATE', 'aguacate.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'ESPINACA', 'espinaca.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'BERROS', 'berros.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'CALABAZA', 'calabaza.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'CALABACIN', 'calabacin.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'CEBOLLA', 'cebolla.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'COLIFLOR', 'coliflor.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'BROCOLI', 'brocoli.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'COL', 'col.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'GUISANTES', 'guisantes.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'JUDIAS VERDES', 'judias_verde.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'LECHUGA', 'lechuga.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'ESCAROLA', 'escarola.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'ENDIBIAS', 'endibias.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'PATATAS', 'patatas.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'PIMIENTOS', 'pimientos.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'TOMATES', 'tomates.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'ZANAHORIAS', 'zanahorias.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'ARANDANOS', 'arandanos.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'CAQUI', 'caqui.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'CIRUELAS', 'ciruelas.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'FRAMBUESAS', 'frambuesas.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'FRESAS', 'fresas.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'GRANADA', 'granada.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'HIGOS', 'higos.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'KIWI', 'kiwi.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'LIMON', 'limon.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'MANDARINA', 'mandarina.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'MANGO', 'mango.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'MANZANA', 'manzana.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'MELON', 'melon.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'NARANJA', 'naranja.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'PERA', 'pera.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'PIÑA', 'pina.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'PLATANO', 'platano.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'SANDIA', 'sandia.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'PAPAYA', 'papaya.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (1, 'UVA', 'uva.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (2, 'NUECES', 'nueces.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (2, 'ALMENDRAS', 'almendras.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (2, 'CACAHUETES', 'cacahuetes.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (2, 'PISTACHOS', 'pistachos.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (2, 'SESAMO', 'sesamo.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (2, 'LINO', 'lino.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (3, 'ARROZ BLANCO', 'arroz_blanco.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (3, 'ARROZ INTEGRAL', 'arroz_integral.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (3, 'AVENA', 'avena.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (3, 'CENTENO', 'centeno.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (3, 'PAN', 'pan.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (3, 'PAN BLANCO', 'pan_blanco.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (3, 'PAN INTEGRAL', 'pan_integral.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (3, 'PASTA', 'pasta.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (3, 'GALLETAS', 'galletas.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (3, 'TRIGO', 'trigo.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (3, 'HARINA DE TRIGO', 'harina_de_trigo.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (3, 'GARBANZOS', 'garbanzos.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (3, 'LENTEJAS', 'lentejas.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (3, 'SOJA', 'soja.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (4, 'CERDO', 'cerdo.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (4, 'CORDERO', 'cordero.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (4, 'PAVO', 'pavo.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (4, 'POLLO', 'pollo.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (4, 'TERNERA', 'ternera.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (4, 'ALMEJAS', 'almejas.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (4, 'ANCHOAS', 'anchoas.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (4, 'ATUN', 'atun.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (4, 'BACALAO', 'bacalao.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (4, 'CALAMAR', 'calamar.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (4, 'DORADA', 'dorada.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (4, 'LANGOSTINO', 'langostino.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (4, 'LUBINA', 'lubina.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (4, 'MERLUZA', 'merluza.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (4, 'SALMON', 'salmon.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (4, 'TRUCHA', 'trucha.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (4, 'SARDINAS', 'sardinas.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (5, 'LECHE ENTERA', 'leche_entera.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (5, 'LECHE SEMIDESCREMADA', 'leche_semidescremada.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (5, 'YOGUR ENTERO', 'yogur_entero.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (5, 'YOGUR CON FRUTAS', 'yogur_con_frutas.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (5, 'QUESO FRESCO', 'queso_fresco.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (5, 'QUESO CURADO', 'queso_curado.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (6, 'ACEITE DE OLIVA', 'aceite_de_oliva.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (6, 'ACEITE DE GIRASOL', 'aceite_de_girasol.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (6, 'ACEITE DE SESAMO', 'aceite_de_sesamo.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (6, 'MANTEQUILLA', 'mantequilla.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (6, 'MARGARINA', 'margarina.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (6, 'AZUCAR', 'azucar.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (6, 'MIEL', 'miel.png', 1);
INSERT INTO Product (IdProductType, Name, ReferenceImage, Status) VALUES (6, 'MERMELADA', 'mermelada.png', 1);

INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Redes de Santa Clara', '73329300-4', 'Servicios Sociales con Alojamiento', 'Nueva Rengifo Nº 251', 'Recoleta', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación de Beneficencia Hogar de Cristo', '81496800-6', 'Venta al por menor de Otros productos varios y otras asociaciones', 'Hogar de Cristo Nº 3812', 'Estación Central', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación las Rosas de Ayuda Fraterna', '70543600-2', 'Institución de Asistencia Social', 'Rivera Nº 2005', 'Independencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación de Organizaciones Solidarias', '65018488-2', 'Fomenta, fortalecer y motivar la colaboración entre las organizaciones solidarias', 'Sánchez Fontecilla Nº 1246', 'Las Condes', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Ciudad del Niño Ricardo Espinosa', '70017730-0', 'Servicios Sociales con Alojamiento; Fabricación de Muebles; C/Vta. y Alquiler Inmuebles', 'Barros Arana Nº 514, Piso 2', 'Concepción', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación un Techo para Chile', '65533130-1', 'Servicio Social sin Alojamiento', 'Avda. Departamental Nº 440', 'Pedro Aguirre Cerda', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Hermanitas del Cordero', '65816320-5', 'Organizaciones Religiosas', 'Matías Cousiño Nº 82 Of. 1101', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Villa de Ancianos Padre Alberto Hurtado', '65878800-0', 'Servicios De Institutos De Estudios - Fundaciones, Corporaciones de Desarrollo (Educación, Salud)', 'Boroa N° 5.575', 'Pedro Aguirre Cerda', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Rodrigo Zaldivar Larraín', '53226150-3', 'Servicios Sociales con Alojamiento', 'Samuel Escobar N° 390', 'Recoleta', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Congregación de hermanas del Buen Samaritano', '71545600-1', 'Actividades De Organizaciones Religiosas', 'Igualdad 1482', 'Molina', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Hogar Luterano de Valdivia', '65728560-9', 'Actividades De Otras Asociaciones N.C.P.', 'Lastarrias N° 200- El Laurel', 'Valdivia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación Red de Alimentos', '65020518-9', 'Servicios De Institutos De Estudios - Fundaciones, Corporaciones de Desarrollo (Educación, Salud)', 'Av.Bicentenario Nº 3883 Of. 502', 'Vitacura', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Congregación Hijas de San José Protectoras de la infancia', '82690200-0', 'Establecimientos de Enseñanza preescolar y primaria, Organizaciones Religiosas, Fotocopias y otros', 'Agustinas Nº 2874', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Congregación Iglesia Apostólica de la Familia Cristiana', '65661470-6', 'Congregación y Actividades de otras Asociaciones', 'Rodrigo de Quiroga Nº 6968 - Valle Dorado', 'Pudahuel', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación de Ayuda al Niño Limitado (COANIL)', '70267000-4', 'Ensenanza Preescolar, Primaria y Servicios Sociales Con Alojamiento', 'Julio Prado Nº 1761', 'Ñuñoa', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Consejo Nacional de Protección a la Ancianidad', '70113500-8', 'Actividades De Otras Asociaciones N.C.P.', 'Tomás Moro Nº 200', 'Las Condes', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación Nuestra Casa', '65171800-7', 'Actividades De Otras Asociaciones N.C.P.Otras Actividades De Servicios Personales', 'Huérfanos Nº 2832', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Nuestros Hijos', '72038400-0', 'Servicios Sociales con Alojamiento', 'Barros Luco Nº 3103', 'San Miguel', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Club de Leones de Osorno', '70603200-2', 'Otras Actividades De Servicios Personales', 'Lord Cochrane Nº 816', 'Osorno', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación para la Nutrición Infantil (CONIN)', '70362000-0', 'Vta. Por Mayor Otros Productos;Educación Extraescolar; Servicios Sociales con Alojamiento.', 'Avda. Pedro de Valdivia Nº 1880', 'Providencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'ONG Casa de Acogida la Esperanza', '73188700-4', 'Fabricación de otros productos de madera;Servicios sociales con Alojamiento', 'Av.Departamental Nº 323', 'San Joaquín', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación de Formación para el Trabajo Romanos XII', '65692700-3', 'Servicios de Institutos de Estudios', 'Av. Santa Rosa Nº 7766', 'La Granja', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Demos una Oportunidad al Menor', '71936500-0', 'Servicios Sociales con Alojamiento', 'Lautaro Nº 2755', 'La Pintana', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Ministerio Social Cristiano Arca de Noe', '65024078-2', 'Actividades De Organizaciones Religiosas', 'Juan Fernandez Nº 1351', 'Cerro Navia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación de Amigos del Hospital Roberto del Río', '72210600-8', 'Actividades de Otras Asociaciones N.C.P.', 'Belisario Prat Nº 1597', 'Independencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Ludovico Rutten', '70000440-6', 'Establecimientos de Enseñanza Primaria, Otras Actividades de Servicios Personales', 'Porto Seguro Nº 4629', 'Quinta Normal', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Aldeas Infantiles SOS Chile', '73597200-6', 'Otros Productos en Pequenos Almacenes, Servidios Sociales con Alojamiento y Actividad de Otra Asociaciones N.C.P.', 'Los Leones Nº 382 Of. 501', 'Providencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación de Oportunidad y Acción Solidaria', '71715000-7', 'Servicios Sociales sin Alojamiento', 'Carlos Justiniano Nº 1123', 'Providencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Educ. de Promoción de Viviendas y de Beneficencia Rodelillo', '71581400-5', 'Actividades de Otras Asociaciones N.C.P.', 'Concha Y Toro Nº 9 y 11', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación Nuestra Ayuda Inspirada en María Curicó', '65368670-6', 'Servicios de Institutos de Estudios', 'Licanten S/N Aguas Negras', 'Curicó', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación de Ayuda al Paciente Mental', '70962100-9', 'Servicios Sociales con alojamiento, Actividades de Otras Asociaciones N.C.P.', 'Santa María Nº 2870', 'Providencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación Carpe Diem', '65616040-3', 'Otras Actividades Empresariales Relacionadas', 'Lo Espejo Nº 0280', 'El Bosque', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Sociedad de Asistencia y Capacitación', '70012450-9', 'Otras Actividades EmpresarialesN.C.P., Servicios Sociales con lAlojamiento', 'Concha y Toro Nº 1898', 'Puente Alto', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Cristo Vive', '71735400-1', 'Fundación de beneficencia en Educación y Asisitencia social', 'Av. Recoleta Nº 5441', 'Huechuraba', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación María Jesús Vergara Arthur', '73073500-6', 'Actividades De Otras Asociaciones N.C.P.', 'Marin Nº 0560', 'Providencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación de Beneficencia de los Sagrados Corazones', '71152400-2', 'Actividades De Otras Asociaciones N.C.P.', 'Calle 6 Nº 4584', 'Ñuñoa', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Hogar Esperanza', '71436400-6', 'Servicios Sociales Con Alojamiento', 'Trinidad Oriente Nº 3400', 'La Florida', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Hogar de Niñas las Creches', '70021750-7', 'Servicios Sociales Con Alojamiento', 'Larrain N°11401 Parque Res La Reina', 'La Reina', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Congregación de las Hermanitas de los Ancianos Desamparados', '70283103-2', 'Actividades De Otras Asociaciones N.C.P.; Hogares privados Individuales con Servicios', 'Avda Islon S/N Villa El Parque', 'La Serena', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Arzobispado de Santiago Caritas Arquidiocesana Santiago', '70287214-6', 'Servicios Sociales Con Alojamiento', 'Catedral Nº 1063 Piso 6', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Arzobispado de Santiago Vicaría de la Pastoral Social', '72160000-9', 'Actividades De Organizaciones Religiosas', 'Catedral Nº 1063', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corp.Hogar de Niños San Roque Casa San Lorenzo', '72416700-4', 'Actividades De Otras Asociaciones N.C.P.', 'Caleuche Nº 1446', 'Cerro Navia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación La Granja', '71720000-4', 'Arriendo De Inmuebles Amoblados O Con Eq; Otras Actividades De Servicios Personale', 'Gaston Ossa N° 0450 Canal Chacao', 'Viña del Mar', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Maria Ayuda Corporación de Beneficencia', '71209100-2', 'Establecimientos De Ensenanza Primaria; Servicios Sociales Con Alojamiento', 'Colombia Nº 7742', 'La Florida', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Paula Jaraquemada Alquizar', '70678600-7', 'Actividades de Otras Asociaciones N.C.P.', 'Av.Providencia Nº 1443 Of. 62', 'Providencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Hermanitas de los Pobres', '71579600-7', 'Servicios Sociales con Alojamiento', 'San Pablo Nº 3776', 'Quinta Normal', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Consejo de Defensa del Niño', '70037600-1', 'Servicios Sociales con Alojamiento', 'Presidente Errázuriz Nº 2631 Piso 5°', 'Providencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Hermanas Franciscanas Misioneras de Jesus Hogar Redes', '65254490-8', 'Actividades de Otras Asociaciones N.C.P.', 'Parcela Nº 53 Peñuelas', 'La Serena', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación de Ayuda al Niño Oncológico Casa de la Sagrada Familia', '65037590-4', 'Actividades De Otras Asociaciones N.C.P.', 'Francisco De Villagra Nº 392', 'Ñuñoa', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Instituto para el Desarrollo Comunitario IDECO, Miguel de Pujadas Vergara', '71877800-k', 'Servicios Sociales Con Alojamiento', 'Av. Segunda Transversal Nº 1371', 'Maipú', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Asociación Benéfica Cristiana de Desarrollo Integral', '71626200-6', 'Impresion Principalmente Libros; Fabricación Productos Metalicos; Actividades Organizaciones Religiosas', 'Lib. Bernardo O Higgins Nº 1537', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Ejercito de Salvación', '70023000-7', 'Actividades De Organizaciones Religiosas', 'Av. España Nº 46', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Congregación Pequeñas Hnas. Misioneras de la Caridad', '70081311-8', 'Establecimientos De Ensenanza Primaria', 'Camino Padre Hurtado Nº 0399', 'Buin', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Padre Semeria', '71178900-6', 'Institutos Profesionales; Actividades De Otras Asociaciones N.C.P.', 'Av. Gabriela Nº 02980', 'La Pintana', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Moreau', '71814400-0', 'Actividades De Otras Asociaciones N.C.P.; Otras actividades de servicios Personales', 'Andes Nº 2529', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Obra Don Guanella', '70015910-8', 'Servicios Sociales Con Alojamiento.', 'Panam Norte Km-25 Batuco', 'Lampa', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación de Beneficencia Regazo', '70012280-8', 'Actividades De Otras Asociaciones N.C.P.', 'Diagonal Cervantes Nº 683', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación de Beneficencia Hogar Niños San José', '70024920-4', 'Transporte De Carga Por Carretera, Servicios Sociales Con Alojamiento y Otras Actividades de Servicios Personales', '21 De Mayo Nº 1081', 'Talagante', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Sanatorio Marítimo San Juan de Dios', '82369500-4', 'Servicios de Transporte Escolar, Establecimientos de Ensenanza Primaria y Hospitales y Clinicas', 'Av. Atlantico Nº 4050 Pobl. Gomez Carreno 05', 'Viña del Mar', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Organización No Gubernamental Santa María', '65041820-4', 'Servicios De Institutos De Estudios', 'Martin De Zamora Nº 4028', 'Las Condes', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación de Beneficencia Nuestra Señora de Guadalupe', '65176540-4', 'Servicios De Institutos De Estudios', 'Nueva Imperial Nº 4231', 'Quinta Normal', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Arzobispado de Santiago Parroquia Nuestra Señora de los Dolores', '81795198-8', 'Ventas Al Por Menor De Otros Productos', 'Carrascal Nº 4483', 'Quinta Normal', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Uniendo Mundos SS. CC.', '65672500-1', 'Servicios De Institutos De Estudios', 'Padre Damian de Veuster Nº 2215', 'Vitacura', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Mis Amigos', '71656900-4', 'Servicios Sociales con alojamiento, Actividades de Otras Asociaciones N.C.P.', 'Av V. Mackenna Nº 3153', 'Peñaflor', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Arzobispado Santiago Parroquia San Pedro y San Pablo', '70287241-3', 'Actividades De Organizaciones Religiosas, Otras Actividades de Servicios Personale', 'Av Central Nº 0498 Rol 5118-001', 'La Granja', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación Misión de María', '72598400-6', 'Servicios Sociales Con Alojamiento', 'Bremen Nº 1316', 'Ñuñoa', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación San José de Calasanz para la Juventud', '65032529-k', 'Actividades De Otras Asociaciones N.C.P.', 'Pedro Torres Nº 98', 'Ñuñoa', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Hogar Español', '70080300-7', 'Servicios De Institutos De Estudios, Otras Actividades De Servicios Personale', 'Alcantara Nº 1320', 'Las Condes', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Instituto Servidoras del Señor y de la Virgen de Matara', '65573350-7', 'Actividades De Organizaciones Religiosas', 'Los Nogales Nº 2730', 'La Pintana', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Hogar San Francisco de Regis', '70015631-1', 'Actividades De Otras Asociaciones N.C.P.', 'Romero Nº 2757', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Crescendo', '65006822-k', 'Actividades De Otras Asociaciones N.C.P.', 'Doctor Johow Nº 411', 'Ñuñoa', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Madre Josefa Fernández Concha una Madre para Chile', '65296840-6', 'Actividades De Otras Asociaciones N.C.P.', 'Esmeralda Nº 732', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación para la Infancia Ronald Mcdonald', '74325400-7', 'Actividades De Otras Asociaciones N.C.P.', 'Av. Kennedy N° 5454 P 16', 'Vitacura', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación San José para la Adopción Familiar Cristiana', '72778300-8', 'Servicios Sociales Con Alojamiento.', 'Antonio De Pastrana Nº 2888', 'Vitacura', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación Inglesa Alianza Cristiana y Misionera', '70017500-6', 'Establecimientos de Enseñanza Primaria y Otras Actividades de Servicios Personales.', 'Dinamarca Nº 711', 'Temuco', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Organización No Gubernamental del Desarrollo Comunidad Terapéutica Plenitud', '65081730-3', 'Actividades De Otras Asociaciones N.C.P.', 'Gral. Baquedano N° 80', 'Paine', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Koinomadelfia', '73238400-6', 'Servicios Sociales Con Alojamiento.', 'Av Pajaritos Pc 10 b Malloco', 'Peñaflor', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Organización no Gubernamental de Desarrollo Fraternidad las Viñas', '65020619-3', 'Otras Actividades Empresariales N.C.P.', 'Patricio Edwards Nº 1360', 'Pudahuel', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación Amor y Esperanza para el Niño Oncológico', '72455200-5', 'Servicios de Institutos de Estudios y Otras Actividades de Servicios Personales N.C.P.', 'Adriana Cousino N° 344 Rol 322-17', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Instituto de Educación Rural', '82067200-3', 'Establecimientos Ensenanza Primaria y Secundaria; Explotac.Animales y Otros Cultivos.', 'Vicuña Mackenna Nº 391', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Hospital Parroquial de San Bernardo', '82031800-5', 'Hospitales y Clínicas; Estacionamiento de Vehiculos y Parquimetros.', 'O Higgins Nº 04', 'San Bernardo', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Congregación Hermanas Hospital del Sagrado Corazón de Jesús', '65299620-5', 'Actividades de Organizaciones Religiosas; Otras Actividades de Servicios Personales.', 'Avda Brasil Nº 551', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Paternitas', '72026600-8', 'Institutos Profesionales', 'Recoleta Nº 900', 'Recoleta', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Scalabrini', '65337630-8', 'Actividades De Otras Asociaciones N.C.P.', 'Av Bustamante Nº 180', 'Providencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Patronato Nacional de la Infancia', '81725000-9', 'Otras Actividades De Servicios Personales', 'Los Militares Nº 5620 Of. 715', 'Las Condes', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Gente de la Calle', '65443510-3', 'Servicios Sociales Con Alojamiento; Actividades de Otras Asociaciones N.C.P.', 'Olivos Nº 704', 'Recoleta', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'ONG de Desarrollo para Personas en Vulnerabilidad Social', '65237280-5', 'Actividades De Otras Asociaciones N.C.P.', 'Arcadia Nº 1381', 'San Miguel', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Centro Esperanza Nuestra', '70021390-0', 'Servicios Sociales Con Alojamiento', 'Avda. Republica N° 1802', 'Maipú', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación Hogar de Menores Cardenal José María Caro', '71452300-7', 'Servicios Sociales Con Alojamiento', 'Miguel Angel Nº 03420', 'La Pintana', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Hogares de Menores La Promesa', '71842900-5', 'Servicios Sociales Con Alojamiento', 'Pedro Torres Nº 537', 'Ñuñoa', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación de Educación y Promiciló Social KAIROS', '72642200-1', 'Actividades De Otras Asociaciones N.C.P.', 'Paula Jara Quemada Nº 1560', 'Renca', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'ONG de Desarrollo Hogar Casa Santa Catalina', '65040138-7', 'Actividades De Otras Asociaciones N.C.P.', 'Juan Castellon Nº 4005', 'Quinta Normal', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación María de la Luz Zañartu', '75187300-K', 'Servicios Sociales Con Alojamiento', 'Calle Uno Nº 3011', 'Quilicura', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación de Ayuda al Niño Desnutrido de Paine CANDES', '71793200-5', 'Actividades De Otras Asociaciones N.C.P.', 'Buin Nº 444', 'Paine', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'ONG de Desarrollo Raíces', '74494300-0', 'Servicios Sociales Con Alojamiento, Servicios de Institutos de Estudios.', 'Moneda N° 812 1014 Rol 383', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Las Asambleas de Dios', '70019800-6', 'Actividades De Organizaciones Religiosas', 'Esperanza Nº 390', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Imagina, Pequeños Negocios, Grandes Emprendedores', '65943320-6', 'Empresas De Publicidad, Servicios De Institutos De Estudios', 'Nueva Costanera Nº 3698 404', 'Vitacura', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Caritas San Bernardo', '65043318-1', 'Actividades De Organizaciones Religiosas', 'Freire Nº 516', 'San Bernardo', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación para la Ayuda y Rehabilitación de Discapacitados', '75564800-0', 'Actividades De Otras Asociaciones N.C.P.', 'Avda. del Villar Nº 957', 'Paine', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Sociedad Pro Ayuda del Niño Lisiado', '81897500-7', 'Fabricación de Equipo Médico y Quirúrgico, Venta al Por Menor de Articulos Ortopédicos, Hospitales y Clínicas y Servicios Sociales Con Alojamiento.', 'Lib. Bdo. O Higgins Nº 4620', 'Estación Central', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Orden Hospitalaria Hnos. de San Juan de Dios', '80696700-9', 'Hospitales y Clínicas y Servicios de Médicos en forma Independiente', 'Avda. Quilin Nº 3679', 'Macul', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación NoTe Rindas', '65593350-6', 'Servicios De Institutos De Estudios', 'Las Margaritas Nº 2295', 'Renca', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Sagrada Familia', '65139100-8', 'Actividades De Otras Asociaciones N.C.P.', 'Pje. Los Himalayas Nº 906', 'Maipu', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Congregación Pequeña Obra de la Divina Providencia', '82156700-9', 'Establecimientos De Ensenanza Primaria, Establecimientos De Ensenanza Secundaria, Actividades de Organizaciones Religiosas.', 'Av Pedro Aguirre Cerda N°7335', 'Cerrillos', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Paicaví', '71690400-8', 'Otros Servicios Desarrollados por Profesionales, Actividades De Otras Asociaciones N.C.P. y Otras Actividades De Servicios Personales.', 'Molina Nº 623 Maipo', 'Buin', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'ONG de Desarrollo Prog. Atención a Drogadictos Caleta Sur', '75996000-9', 'Servicios Sociales Sin Alojamiento', 'Buenaventura N° 03906', 'Lo Espejo', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación Asociación Pro Derechos de los Niños y Jovenes', '73101300-4', 'Actividades De Otras Asociaciones N.C.P.', 'Brown Norte N°379', 'Ñuñoa', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación de Voluntarias de Oncología Infantil y Obras Sociales, Damas de Café', '71367100-2', 'Servicios Sociales Con Alojamiento', 'A Varas Nº 360', 'Providencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación de Ayuda a la Comunidad Escuela de Servicio de Manquehue', '75995940-K', 'Servicios Sociales Con Alojamiento.', 'Juarez Corta Nº 579', 'Recoleta', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'ONG Compañía de las Hijas de la Caridad de San Vicente de Paul', '70200200-1', 'Establecimientos Ensenanza Primaria; Servicios Sociales con Alojamiento; Otros Profesionales de la Salud y Actividades de Organizaciones Religiosas.', 'Venecia Nº 1640', 'Independencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación Crece Chile', '65006081-4', 'Servicios De Institutos De Estudios', 'Garibaldi Nº 1653', 'Ñuñoa', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'ONG de Desarrollo Casa Azúl', '65758090-2', 'Clubes Sociales y Servicios de Institutos de Estudios.', 'Yungay Nº 0641', 'La Granja', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación Educacional y de Beneficencia Cristo Joven', '73099800-7', 'Establecimientos Ensenanza Preescolar; Servicios Sociales sin Alojamiento y Actividades de Otras Asociaciones N.C.P.', 'General Baquedano Nº 2285', 'Peñalolén', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Asociación de Padres y Amigos de los Autistas', '71086700-3', 'Establecimiento de Enseñanza Primaria y Servicios Sociales sin Alojamiento', 'Gran Avenida Nº 2820', 'San Miguel', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Sociedad de San Vicente de Paul', '70001640-4', 'Servicios Sociales con Alojamiento', 'Duble Almeyda Nº 4751', 'Ñuñoa', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación Educacional y Asistencial Hellen Keller', '71162000-1', 'Servicios Sociales Con Alojamiento', '18 de Septiembre N° 0436', 'El Bosque', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Liga Chilena Contra el Mal de Parkinson', '71613900-K', 'Establecimientos Medicos De Atencion Amb. Y Servicios Sociales con Alojamiento', 'A Prat Nº 1341', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'O.N.G. de Desarrollo Acción Comunitaria Evangélica', '74288500-3', 'Actividades de Otras Asociaciones N.C.P.', 'Santa Isabel Nº 80', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación de la Familia', '71689100-3', 'Actividades De Otras Asociaciones N.C.P.', 'Ahumada Nº 341 7 Ps', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Vicaria para la Educación San Egidio', '81795119-8', 'Otras Actividades de Servicios Personales', 'Erasmo Escala Nº 2819', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación San Francisco de Borja', '65450420-2', 'Actividades de Otras Asociaciones N.C.P. Y Otras Actividades De Servicios Personales', 'José Miguel Carrera Nº 1499', 'La Florida', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Misión Batuco', '65878090-5', 'Servicios de Institutos de Estudios', 'Uno Sur N° 65 Batuco', 'Lampa', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación ONG - Visión por chile', '65033166-4', 'Servicios De Institutos De Estudios', 'Miguel Angel Nº 2431 V/Las Rosas', 'La Pintana', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación Programa Poblacional Servicio La Caleta', '72441600-4', 'Servicios Sociales sin Alojamiento y Otras Actividades De Servicios Personales', 'Barnechea N° 322', 'Independencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Vida Compartida', '65382330-4', 'Clinicas Psiquiatricas, Centros de Rehab. Y Actividades De Otras Asociaciones N.C.P.', 'Av. Lib.Bernardo Ohiggins Nº 2361', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Creando Redes para Chile y el Mundo', '65041632-5', 'Actividades De Otras Asociaciones N.C.P.', 'Alcalde Jorge Indo N° 604', 'Quilicura', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'O.N.G. de Desarrollo Corporación de Beneficencia Jesús Niño', '73064000-5', 'Institutos Profesionales, Servicios Sociales con Alojamiento y Otras Actividades Personales', 'La Frontera N° 03709', 'La Pintana', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación de Apoyo a la Niñez y Juventud en Riesgo Social', '71992600-2', 'Servicios De Institutos De Estudios y Actividades de Otras Asociaciones N.C.P.', 'Maipon N° 428', 'Chillán', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación de Investigación y Desarrollo de la Sociedad y las Migraciones Colectivo sin Fronteras', '65015346-4', 'Servicios de Institutos de Estudios', 'Barnechea N° 320', 'Independencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación La Casa del Padre Demetrio', '72244800-6', 'Servicios Sociales Con Alojamiento', 'Av. Padre Demetrio Bravo N° 0110', 'Melipilla', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Marcelo Astoreca', '71635600-0', 'Establecimientos De Enseñanza Primaria', 'Villarrica N° 1653 - Villa Sarmiento', 'Renca', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Hogar Crea Chile', '74295700-4', 'Actividades De Otras Asociaciones N.C.P.', 'Guillermo Acuna N° 2510 F: 209-2503', 'Providencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Renuevo Centro Integal', '65827920-3', 'Actividades De Otras Asociaciones N.C.P.', 'Copiapo N° 288 A', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Sueños en Red', '65028778-9', 'Otros Servicios Desarrollados Por Profes y Servicios de Institutos De Estudios', '23 de Febrero N° 8860 Casa D', 'La Reina', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Gesta Fundación Marista por la Solidaridad', '74009700-8', 'Investigaciones Y Desarrollo Experimental y Otras Actividades Empresariales N.C.P.', 'Grajales N° 2176', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Jardín Centro Infantil Nuestra Señora de la Victoria', '65442660-0', 'Actividades De Otras Asociaciones N.C.P.', 'Galo Gonzalez N° 4585 - La Victoria', 'Pedro Aguirre Cerda', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'World Visión Internacional', '70689300-8', 'Otras Actividades De Servicios Personale', 'Emilio Vaisse N° 338', 'Providencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Organización no Gubernamental de Desarrollo Pather Nostrum', '65879820-0', 'Servicios de Institutos de Estudios', 'Av. Ambrosio O Higgins N° 1397', 'Curacavi', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'O.N.G. Cordillera', '74615700-2', 'Otras Actividades Empresariales N.C.P., Servicios de Institutos de Estudios', 'Juan de Pineda N° 7580', 'La Florida', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'O.N.G. SURCOS', '75347400-5', 'Servicios de Institutos de Estudios y Actividades de otras Asociaciones N.C.P.', 'San Antonio N° 378 - OF 1210', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación de Acción Social Educacional Coretti', '70826500-4', 'Establecimientos de Ensenanza Primaria y Otras Actividades de Servicios Personales', 'Antártica N° 4061 - Los Nogales', 'Estación Central', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'O.N.G. Corporación Ascorp Liwen', '65309500-7', 'Otras Actividades Empresariales N.C.P. y Actividades de Otras Asociaciones N.C.P.', 'Avda. Grecia N° 6871', 'Peñalolén', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Estudio para un Hermano', '74016500-3', 'Venta al por Mayor de Papel y Carton, Venta al por Mayor de Desechos Metalicos, Actividades de Otras Asociaciones N.C.P.', 'Eyzaguirre N° 310', 'Puente Alto', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'O.N.G. de Desarrollo y Futuro Chile', '65766470-7', 'Actividades De Otras Asociaciones N.C.P.', 'Manuel de Amat N° 3119', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Solidaria Trabajo para un Hermano', '71440800-3', 'Venta Al Por Menor de Otros Productos y Servicios de Institutos de Estudios', 'Av. Ejercito N° 390', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Congregación de las Hermanitas de los Ancianos Desamparados', '70283104-0', 'Otras Actividades De Servicios Personales', 'Holandesa X S 0902', 'Temuco', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'O.N.G. Corporación Evangélica para el Desarrollo', '65731820-5', 'Actividades De Organizaciones Religiosas', 'Obispo Manuel Umana N° 139', 'Estación Central', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Iglesia de Cristo Iberoamericana', '72417700-k', 'Establecimientos De Enseñanza Preescolar; Establecimientos De Enseñanza Primaria y Actividades De Otras Asociaciones N.C.P.', 'Libertad N° 672', 'Maipú', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Madres Adolescentes Raíz, Flor y Fruto', '65047196-2', 'Actividades De Otras Asociaciones N.C.P.', 'Juan Vicuña N° 1277', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Tacal', '72250700-2', 'Otras Actividades Empresariales N.C.P. y Otras Actividades De Servicios Personales', 'Adolfo Ibáñez 469', 'Independencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación de Desarrollo Juvenil Proyecto Joven', '65058379-5', 'Actividades De Otras Asociaciones N.C.P.', 'Pasaje Goliat N° 1481-Gral. Baquedano', 'Maipú', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Paréntesis', '72421000-7', 'Clínicas Psiquiátricas, Centros de Rehab. Y Actividades De Otras Asociaciones N.C.P.; Establecimientos Médicos De Atención Amb.; Otras Actividades Empresariales Relacionadas.', 'Lafayette N° 1610', 'Independencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Instituto Verbo Encarnado', '65529900-9', 'Actividades De Otras Asociaciones N.C.P.', 'Los Nogales N° 0290', 'La Pintana', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Convento Santo Domingo', '81681300-k', 'Actividades De Organizaciones Religiosas', 'Vega De Saldias N° 160', 'Chillán', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Hogares Evanglélicos de Chile', '70039300-3', 'Otras Actividades De Servicios Personale', 'J Rodriguez N° 2748', 'Macul', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundacin Sagrado Corazón', '65728940-k', 'Actividades De Otras Asociaciones N.C.P.', 'Llico N° 931', 'San Miguel', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Cristiana La Puerta', '65058204-7', 'Servicios De Institutos De Estudios y Actividades De Otras Asociaciones N.C.P.', 'Av Lib Bdo Ohiggins N° 341', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'O.N.G. de Desarrollo La Matríz', '65060696-5', 'Actividades De Otras Asociaciones N.C.P.', 'Santo Domingo N° 36', 'Valparaíso', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación Educacional El Despertar', '72786100-9', 'Servicios Sociales Sin Alojamiento', 'Av La Marina N° 2710', 'Pedro Aguirre Cerda', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación de Desarrollo para Niños en Riesgo Social', '65936280-5', 'Servicios De Institutos De Estudios', 'Av. Lo Barnechea 1250', 'Lo Barnechea', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación San Pedro', '71657300-1', 'Institutos Profesionales', 'Padre Hurtado N° 14600', 'San Bernardo', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación misses por una causa', '65623830-5', 'Servicios de Institutos de Estudios, Servicios de Producciones de Recitales y Otros Servicios Profesionales', 'San Ignacio de Loyola N° 1709', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Puerta Abierta', '65033693-3', 'Servicios de Institutos de Estudios', 'Echaurren N° 581', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Tiempo de Reir', '65053335-6', 'Actividades de Otras Asociaciones N.C.P', 'Av. Balmaceda N° 0612 Santa Marta', 'San Bernardo', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Ministerio Evangélico la Piedra del Angulo', '65928530-4', 'Actividades de Organizaciones Religiosas', 'Arturo Prat N° 1880', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación para el Desarrollo Económico y Social', '65060273-0', 'Servicios de Institutos de Estudios, Actividades de Otras Asociaciones N.C.P', 'José Joaquín Pérez N° 385-B', 'Quirihue', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Tierra de Esperanza', '73868900-3', 'Actividades De Asesoramiento Empresarial y Otras Actividades Empresariales N.C.P.', 'Exeter N° 540 D', 'Concepción', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación de Desarrollo y Acción Social Cristiana Alborada', '65066253-9', 'Actividades De Otras Asociaciones N.C.P.', 'Las Espuelas 11 Haras', 'Machalí', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Congregación Hermanitas de los Ancianos Desamparados', '70283100-8', 'Otras Actividades De Servicios Personales', 'Las Condes N° 13200', 'Las Condes', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Sustento y Refugio', '65069508-9', 'Servicios de Institutos de Estudios', 'Av. Manuel Antonio Matta N° 119', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación para la Dignidad del Hombre', '73755700-6', 'Servicios Sociales con Alojamiento', 'Diego Portales N° 1569', 'La Florida', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Abrazarte Artistas por una Obra', '65003688-3', 'Venta al por Menor de Otros Productos; Actividades de Otras 			Asociaciones N.C.P.', 'Moneda N° 1845', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación de Beneficencia de la Mujer Levántate', '65019819-0', 'Actividades De Organizaciones Religiosas', 'Echaurren N° 04', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Educacional Belén', '74805100-7', 'Establecimientos De Enseñanza Primaria', 'Moneda N° 1958', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Organización No Gubernamental de Desarrollo Dianova Chile', '65058700-6', 'Otras Actividades De Servicios Personale', 'Málaga N° 89 Of. 22', 'Las Condes', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Diabetes Juvenil de Chile', '71494700-1', 'Servicios Sociales Con Alojamiento', 'Lota N° 2344', 'Providencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Organización No Gubernamental de Desarrollo Comunidad Terapeuta 			Raices', '65535800-5', 'Otras Actividades Empresariales N.C.P.; Clínicas Psiquiátricas, 			Centros de Rehabilitación', 'Guacolda N° 11550', 'La Florida', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación de Desarrollo y Acción Social, cultural y 			Educacional Arrebol Chile', '65073235-9', 'Actividades De Otras Asociaciones N.C.P.', 'San Sebastián N° 1291- San Antonio', 'Temuco', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Comunidad Terapéutica Horizonte', '65053320-8', 'Actividades De Otras Asociaciones N.C.P', 'Camino Rinconada N° 1224', 'Maipú', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación Comunidad Terapéutica Cristo', '65071555-1', 'Otras Actividades Empresariales Relacionadas Con La Salud 			Humana', 'Escritora Marcela Paz N° 550', 'Maipú', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Cristo Especial', '65063655-4', 'Servicios Sociales sin Alojamiento, Otras Actividades De 			Servicios Personales', 'Antártica N° 3433 La Legua', 'San Joaquín', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Humaniza', '65062466-1', 'Servicios De Institutos De Estudios, Fundaciones, Corporaciones 			De Desarrollo (Educación, Salud)', 'Fueguinos N° 9174', 'Las Condes', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Organización No Gubernamental de Desarrollo Corporación de 			Apoyo al Desarrollo Auto gestionado Grada', '73102600-9', 'Actividades De Otras Asociaciones N.C.P.', 'Carmen Covarrubias N° 213', 'Ñuñoa', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación de Emprendimiento Social y Acogida Mañana', '65056976-8', 'Actividades De Otras Asociaciones N.C.P.', 'Brown Sur N° 150', 'Ñuñoa', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación de Beneficencia Aldea de Niños Cardenal Raúl Silva 			Henríquez', '71404100-2', 'Actividades de Organizaciones Empresariales y de Empleadores; 			Actividades de Organizaciones Religiosas', 'Del Pastor S/N Punta De Tralca', 'El Quisco', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación de Ayuda al Cáncer Infantil Amor y Vida', '65207610-6', 'Actividades De Otras Asociaciones N.C.P.', 'Von Schroeders N° 218 Of.24', 'Viña del Mar', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Pérez & Castillo', '65072269-8', 'Servicios De Institutos De Estudios, Fundaciones, Corporaciones 			De Desarrollo (Educación, Salud)', 'Melinka N° 2514 Pobl. Dav', 'Pedro Aguirre Cerda', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Cerro Comunitario Laura Vicuña', '65546160-4', 'Establecimientos de Enseñanza Primaria, Otras Actividades de 			Servicios Personales N.C.P', 'Manuel Plaza N° 440 - Villa Olimpica', 'Puerto Montt', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación de Desarrollo Integral de las Personas Amar Chile', '65071692-2', 'Actividades de Otras Asociaciones N.C.P.', 'Huérfanos N° 1044 - OF 905', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Corazones Abiertos', '65079358-7', 'Actividades de Otras Asociaciones N.C.P.', 'San Juan De La Luz N° 4186 - OF 516', 'Providencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Educacional Choshenco', '65049791-0', 'Servicios de Institutos de Estudios, Fundaciones, Corporaciones 			de Desarrollo (Educacion, Salud); Actividades de Otras Asociaciones 			N.C.P.', 'Av. El Bosque Norte N° 500 P.24', 'Las Condes', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Ministerio Evangelístico Salvados por Cristo', '6512770-1', 'Actividades De Organizaciones Religiosas', 'Av Jose Manuel Balmaceda N° 1622', 'La Pintana', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Centro Protestante de Chile', '71983900-2', 'Otras Actividades De Servicios Personales N.C.P.', 'Avda Matta N° 119', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Agencia Adventista de Desarrollo y Recursos Asistencial', '70051600-8', 'Compra, Venta Y Alquiler de Inmuebles Propios o Arrendados, 			Actividades de Asesoramiento Empresarial y en Materia de Gestion, 			Servicios Sociales sin Alojamiento, Actividades de Organizaciones 			Religiosas', 'Los Cerezos N° 6251', 'Peñalolen', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Iglesia Cuerpo de Cristo', '65636380-0', 'Actividades De Organizaciones Religiosas', 'General Gana N° 678', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Tupahue', '65073765-2', 'Servicios Sociales son Alojamiento, 			Servicios Sociales sin Alojamiento y, Actividades de Otras 			Asociaciones N.C.P.', 'Av Mexico N° 2045 Of. 51', 'La Florida', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Org. No Gubernamental de Desarrollo Social, Cultural y 			Productivo ""CIDETS""', '65227510-9', 'Actividades De Otras Asociaciones N.C.P.', 'El Guerrillero N° 9687', 'La Florida', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Servicio Jesuita a Migrantes', '65030892-1', 'Servicios De Institutos De Estudios, Fundaciones, Corporaciones 			De Desarrollo (Educacion, Salud)', 'Padre Alonso de Ovalle N° 1362', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Organización de padres y Amigos de Niños con Síndrome de Down', '65567530-2', 'Actividades de Otras Asociaciones N.C.P', 'Mario Delgado Leyton N° 0335', 'Victoria', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Las Parcelas', '65068557-1', 'Servicios De Institutos De Estudios, Fundaciones, Corporaciones De Desarrollo (Educacion, Salud)', 'Av. Santa Rosas N° 13240', 'La Pintana', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'ONG de Desarrollo Programa Terapéutico-Educativo Proyecto Sur', '73779800-3', 'Servicios Sociales con Alojamiento', '19 de Septiembre N° 6161', 'Cerrillos', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Centro Cristiano Integral Manos que Salvan', '65067729-3', 'Actividades de Organizaciones Religiosas', 'Jalisco N° 8502', 'Pudahuel', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Amalegría', '65080205-5', 'Servicios De Institutos De Estudios, Fundaciones, Corporaciones De Desarrollo (Educación, Salud)', 'Roma N° 01307', 'Villa Alemana', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación Acción y Gestión', '65054177-4', 'Actividades De Otras Asociaciones N.C.P.', 'Almirante Riveros N° 043', 'Providencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Org. No Gubernamental de Desarrollo Centro de Estudios Desarrollo y Futuro', '65080819-3', 'Servicios De Institutos De Estudios, Fundaciones, Corporaciones De Desarrollo (Educacion, Salud)', 'Av.La Castrina N° 335', 'San Joaquín', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Internacional María Luisa de Moreno', '65067527-4', 'Servicios De Institutos De Estudios, Fundaciones, Corporaciones De Desarrollo (Educacion, Salud)', 'Catedral N° 1289 Depto. 907', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Centro Abierto Hermanos de Cristo', '71778300-k', 'Actividades De Otras Asociaciones N.C.P.', 'Montriou N° 1435', 'Concepción', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Org. No Gubernamental de Desarrollo Padre Luís Amigo', '65076983-k', 'Actividades De Otras Asociaciones N.C.P.', 'Serrano N° 310', 'Concepción', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'ONG de Desarrollo al Menor en Riesgo Social un Rincón de Alegría', '65554850-5', 'Servicios Sociales Con Alojamiento', 'Caliche N° 812', 'Recoleta', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Nacional de Orquestas Juveniles e Infantiles de Chile', '75991930-0', 'Arriendo De Inmuebles Amoblados O Con Equipos Y Maquinarias, Actividades de Otras Asociaciones N.C.P.', 'Balmaceda (Interior) N° 1301 Pza. de La Cultura Santiago', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Amigos de Jesús', '65044194-k', 'Actividades De Otras Asociaciones N.C.P.', 'Huelen N° 2001', 'Cerro Navia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Cristo de la Noche', '65083378-3', 'Servicios De Institutos De Estudios, Fundaciones, Corporaciones De Desarrollo (Educacion, Salud)', 'Pje. Mejillones N° 5373 Sarmiento', 'Renca', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación por una Carrera', '65999628-6', 'Otras Actividades Empresariales N.C.P.; Actividades De Otras Asociaciones N.C.P.', 'Aurelio Gonzalez N° 3838 02', 'Vitacura', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Comité de Adelanto y Desarrollo Comunitario Ebenecer', '65018711-3', 'Actividades De Organizaciones Religiosas', 'Juan De Dios Marticorena Sn Sitio 10', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Religiosas de María Inmaculada', '70023890-3', 'Compra, Venta Y Alquiler (Excepto Amoblados) De Inmuebles Propios O Arrendados y Actividades De Organizaciones Religiosas', 'Olivares N° 1370', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Iglesia comunidad Jireh', '65072133-0', 'Actividades De Organizaciones Religiosas', 'Avda. El Trebol N° 706', 'Padre Hurtado', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Consejo de Curso', '65087434-k', 'Servicios De Institutos De Estudios, Fundaciones, Corporaciones De Desarrollo (Educacion, Salud)', 'Encomendadores N° 253 Of.12', 'Las Condes', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Caritas Diocesana de Linares', '75463400-6', 'Servicios Sociales Con Alojamiento; Actividades De Organizaciones Religiosas', 'Freire N° 452 3 Piso', 'Linares', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Ayuda Mundial', '65091801-0', 'Servicios De Institutos De Estudios, Fundaciones, Corporaciones De Desarrollo (Educacion, Salud)', 'Eyzaguirre N° 494 El Esfuerzo', 'Puente Alto', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Congregación de Religiosos Terciarios Capuchinos', '81795172-4', 'Servicios Sociales sin Alojamiento y Otras Actividades De Servicios Personales', 'Avenida Victoria N° 050', 'Villa Alemana', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Intercultural La Concepción', '65098271-1', 'Educacion Extraescolar (Escuela De Conduccion, Musica, Modelaje, Etc.)', 'Calle Dos N° 176 Fte de Piedra/Villuco', 'Chiguayante', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Moviliza', '65860350-7', 'Servicios De Institutos De Estudios, Fundaciones, Corporaciones De Desarrollo (Educacion, Salud) y Otras Actividades De Servicios Personales N.C.P.', 'Victor Hendrych N° 357', 'Providencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Bernarda Morín', '65877120-5', 'Servicios Sociales Con Alojamiento y Servicios Sociales Sin Alojamiento', 'Teranova N° 140', 'Providencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación para la Atención Integral del Maltrato al Menor en la Región del Bío Bío', '72607900-5', 'Servicios De Institutos De Estudios, Fundaciones, Corporaciones De Desarrollo (Educacion, Salud)', 'Ohiggins N° 445 Depto 501', 'Concepción', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'O.N.G La Barra', '65063845-k', 'Servicios De Institutos De Estudios, Fundaciones, Corporaciones De Desarrollo (Educación, Salud) y Actividades de Otras Asociaciones N.C.P.', 'San Antonio N° 385 Of. 301', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Asesorías Educacionales Panal', '65079404-4', 'Actividades de Asesoramiento Empresarial, de Institutos de Estudios, Fundaciones, Corporaciones de Desarrollo (Educación, Salud)', 'Italia N° 850 B', 'Providencia', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Agrupación de Padres de Niños Discapacitados', '71618200-2', 'Establecimientos De Enseñanza Primaria y Otras Actividades de Servicios Personales N.C.P.', 'Loreto Cousiño N° 68 Lota Alto', 'Lota', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Monasterio de la Adoración Perpetua', '70090200-5', 'Actividades de Organizaciones Religiosas', 'Santo Domingo N° 2055', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'O.N.G Langar Chile', '65021162-6', 'Educación Extraescolar; Servicios Sociales con Alojamiento; Servicios Sociales sin Alojamiento', 'La Nina N° 3025', 'Las Condes', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Movimiento Anónimo por la Vida', '71414100-7', 'Actividades de Otras Asociaciones N.C.P.', 'Lorenzo Arenas N° 2245 Pje', 'Concepción', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación Ancianos Bernabé', '65084832-2', 'Actividades de Otras Asociaciones N.C.P.', 'José Joaquín Prieto N° 3391', 'Pedro Aguirre Cerda', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corp. de Ayuda al Paciente Esquizofrénico y Familiares', '72424400-9', 'Servicios de Institutos de Estudios, Fundaciones, Corporaciones de Desarrollo (Educación, Salud)', 'Río Loa N° 137- Pobl. Ríos de Chile', 'Concepción', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Alfonso Pacheco Berríos', '65105035-9', 'Servicios Sociales con Alojamiento, Servicios de Institutos de Estudios, Fundaciones, Corporaciones de Desarrollo', 'Bernardino Corral N° 196 Collao', 'Concepción', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Golheth Tem Wuok', '65020591-k', 'Actividades de Otras Asociaciones N.C.P.', 'Armando Sanhueza N° 110', 'Punta Arenas', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Ciencia Joven', '65050503-4', 'Edición de Períodicos, Revistas y Publicaciones Períodicas, Servicios de Institutos de Estudios, Fundaciones, Corporaciones de Desarrollo', 'Dinamarca N° 399 12', 'Valparaíso', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundaciones del Mundo Nuevo', '71943800-8', 'Actividades de Otras Asociaciones N.C.P.', 'Avda. El Salto Norte N° 5625', 'Huechuraba', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Agrupación de Padres y Amigos por la Integración', '74686600-3', 'Actividades de Otras Asociaciones N.C.P.', 'Janequeo N° 360 2do Piso', 'Concepción', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Asociación de Padres y Amigos de los Autistas Octava', '65290120-4', 'Servicios Personales de Educación', 'Manquimavida N° 317, Manquimavida, Chiguayante', 'Concepción', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Centro de Espiritualidad y Desarrollo Cultural', '65064024-1', 'Servicios de Institutos de Estudios, Fundaciones, Corporaciones de Desarrollo (Educación, Salud)', 'Camino de Estrellas Pc-20 Lt-7, Barrio Ecológico', 'Peñalolén', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación La Familia de María', '65099266-0', 'Servicios de Institutos de Estudios, Fundaciones, Corporaciones de Desarrollo (Educación, Salud)', '2 Norte N° 1670', 'Talca', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Deportistas por un Sueño', '65020274-0', 'Servicios Sociales sin Alojamiento', 'Alejandro Vial N° 8706', 'La Cisterna', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Portas', '72399200-1', 'Actividades De Otras Asociaciones N.C.P.', 'Pedro de Valdivia N° 2921', 'Ñuñoa', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Mer Niños por la Educación y Restauración de los Niños', '65112666-5', 'Servicios de Institutos de Estudios, Fundaciones, Corporaciones De Desarrollo (Educacion, Salud) y Actividades de Otras Asociaciones N.C.P.', 'Avenida La Palmilla N° 4328', 'Conchalí', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Servicio Social sin Fronteras', '65100972-3', 'Servicios De Institutos De Estudios, Fundaciones, Corporaciones De Desarrollo (Educacion, Salud) y Actividades De Otras Asociaciones N.C.P', 'Diego Barros Arana N° 876 Valle Vespucio Norte', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Organización No Gubernamental de Desarrollo Solidaridad Social', '65098477-3', 'Servicios De Institutos De Estudios, Fundaciones, Corporaciones De Desarrollo (Educacion, Salud)', 'Batuco N° 150 Vina Del Mar', 'Viña del Mar', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Refugio', '65114553-8', 'Servicios De Institutos De Estudios, Fundaciones, Corporaciones De Desarrollo (Educacion, Salud)', 'Tres Rios N° 1486 Galilea', 'Rancagua', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Organización No Gubernamental de Desarrollo Mpa8', '65112353-4', 'Actividades De Otras Asociaciones N.C.P.; Otras Actividades De Entretenimiento N.C.P.; Promocion Y Organizacion De Espectaculos Deportivos; Otros Servicios De Diversion Y Esparcimientos N.C.P.', 'Santa Rosa N° 325 Of. 113', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'ONG de Desarrollo Asociación Voluntariado Internacional Mujer Educación Desarrollo', '65670080-7', 'Actividades De Organizaciones Religiosas', 'Av Matta N° 762 Santiago', 'Santiago', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación S.O.S. Familias', '65064818-8', 'Servicios De Institutos De Estudios, Fundaciones, Corporaciones De Desarrollo (Educacion, Salud)', 'Condell N° 1246 Valparaíso', 'Valparaíso', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Dar Dignidad Amor y Restauración', '65113279-7', 'Actividades De Otras Asociaciones N.C.P.', 'Ara N° 2533 F 312', 'San Miguel', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación educacional Koulutus', '65098661-k', 'Servicios De Institutos De Estudios, Fundaciones, Corporaciones De Desarrollo (Educacion, Salud)', 'Pasaje Fraternidad N° 1599 Bernardo Leighton', 'La Florida', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corp. De Educación y Desarrollo Popular el Trampolín', '72571200-6', 'Comercio Al Por Menor De Articulos Tipicos (Artesanias) y Actividades De Otras Asociaciones N.C.P.', 'Candelaria N°1998 Villa San Luis', 'Maipú', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación de Hogares Estudiantiles Divina Providencia', '65067728-5', 'Actividades De Otras Asociaciones N.C.P.', 'Patria Vieja N° 310 Lan C', 'Talcahuano', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Manos a la Obra', '65077895-2', 'Actividades De Otras Asociaciones N.C.P.', 'Santa Teresa N° 1071', 'Estación Central', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Parroquia de Santa Cruz', '70021530-k', 'Actividades De Organizaciones Religiosas', 'Pinguinos N° 4255', 'Estación Central', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Arzobispado de Santiago Parroquia Jesús Obrero', '81795195-3', 'Actividades De Organizaciones Religiosas', 'Avda.Padre Alberto Hurtado N° 1090', 'Estación Central', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Refugio de Cristo', '70015560-9', 'Ventas Al Por Menor De Otros Productos En Almacenes Especializados N.C.P. y Servicios Sociales Con Alojamiento', 'Victoria N° 2370', 'Valparaíso', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación Hogar Belén', '72758100-6', 'Actividades De Otras Asociaciones N.C.P.', '10sur N° 31y32 Ote N° 1345 7 Carlos Trupp', 'Talca', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Misión Evangélica San Pablo de Chile', '71318900-6', 'Otras Actividades Empresariales N.C.P. y Actividades de Organizaciones Religiosas', 'Ricardo S/N Lota', 'Lota', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación Centro de Apoyo Familiar', '65098836-1', 'Actividades De Otras Asociaciones N.C.P.', 'Ordonez N° 498', 'Maipú', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Asoc. Hogar de Niños Arturo Prat', '70013440-7', 'Servicios Sociales con Alojamiento', 'Blanco N° 1623 - OF. 403', 'Valparaíso', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Centro de Rehabilitación Esperanza Juvenil', '65769920-9', 'Otras Actividades Empresariales Relacionadas Con La Salud Humana', 'Las Heras N° 359', 'Los Andes', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Corporación para la Prevención Rehabilitación e Integración Social de Personas con Problemas de Adicción Cristo Obrero', '65100532-9', 'Otras Actividades Empresariales Relacionadas Con La Salud Humana y Actividades De Otras Asociaciones N.C.P.', 'Las Heras N° 359', 'Los Andes', 1);
INSERT INTO Institution (Name, Rut, Activity, Address, Commune, Status) VALUES ( 'Fundación para la Fibromialgia Abacitos de Algodón', '65116917-8', 'Otras Actividades Empresariales Relacionadas Con La Salud Humana y Actividades de Otras Asociaciones N.C.P.', 'Las Heras N° 359', 'Los Andes', 1);
