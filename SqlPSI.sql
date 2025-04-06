DROP DATABASE IF EXISTS LivinParis;
CREATE DATABASE LivinParis;
USE LivinParis;

CREATE TABLE Tiers(
   IDTiers INT,
   CodePostal VARCHAR(5),
   Ville VARCHAR(50),
   Email VARCHAR(50),
   Tel VARCHAR(10),
   Nom VARCHAR(50),
   Adresse VARCHAR(70),
   Prenom VARCHAR(50),
   PRIMARY KEY(IDTiers)
);

CREATE TABLE Client(
   IDClient INT,
   PRIMARY KEY(IDClient),
   FOREIGN KEY(IDClient) REFERENCES Tiers(IDTiers)
);

CREATE TABLE Cuisinier(
   IDCuisinier INT,
   PRIMARY KEY(IDCuisinier),
   FOREIGN KEY(IDCuisinier) REFERENCES Tiers(IDTiers)
);

CREATE TABLE Plat(
   IDPlat INT,
   TypePlat VARCHAR(50),
   NomPlat VARCHAR(50),
   DateFabrication DATE,
   DatePeremption DATE,
   Nationalite VARCHAR(50),
   Regime VARCHAR(50),
   Ingredients VARCHAR(100),
   PrixPlat DECIMAL(15,2),
   NombrePersonnes INT,
   IDCuisinier INT,
   PRIMARY KEY(IDPlat),
   FOREIGN KEY(IDCuisinier) REFERENCES Cuisinier(IDCuisinier)
);

CREATE TABLE Commande(
   IDCommande INT,
   DateCommande DATE,
   HeureCommande TIME,
   IDClient INT NOT NULL,
   IDCuisinier INT NOT NULL,
   PRIMARY KEY(IDCommande),
   FOREIGN KEY(IDClient) REFERENCES Client(IDClient),
   FOREIGN KEY(IDCuisinier) REFERENCES Cuisinier(IDCuisinier)
);

CREATE TABLE PlatCommande(
	IDCommande INT, 
    IDPlat INT,
    PRIMARY KEY (IDCommande, IDPlat), /* un plat ne peut être affecté qu'à au plus une commande */
    FOREIGN KEY (IDCommande) REFERENCES Commande(IDCommande),
    FOREIGN KEY (IDPlat) REFERENCES Plat(IDPlat)
);

CREATE TABLE Entreprise(
   NomEntreprise VARCHAR(50),
   IDTiers INT NOT NULL,
   PRIMARY KEY(NomEntreprise),
   FOREIGN KEY(IDTiers) REFERENCES Client(IDClient)
);

CREATE TABLE cuisine(
   IDTiers INT,
   IDPlat INT,
   PRIMARY KEY(IDTiers, IDPlat),
   FOREIGN KEY(IDTiers) REFERENCES Cuisinier(IDCuisinier),
   FOREIGN KEY(IDPlat) REFERENCES Plat(IDPlat)
);

CREATE TABLE sert(
   IDTiers INT,
   IDTiers_1 INT,
   Note INT,
   PRIMARY KEY(IDTiers, IDTiers_1),
   FOREIGN KEY(IDTiers) REFERENCES Client(IDClient),
   FOREIGN KEY(IDTiers_1) REFERENCES Cuisinier(IDCuisinier)
);
