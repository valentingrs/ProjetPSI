DROP DATABASE IF EXISTS LivinParis;
CREATE DATABASE LivinParis;
USE LivinParis;

CREATE TABLE Tiers(
   IDTiers INT,
   MotDePasse VARCHAR(50),
   CodePostal VARCHAR(5),
   Ville VARCHAR(50),
   Email VARCHAR(50),
   Tel VARCHAR(10),
   Nom VARCHAR(50),
   Adresse VARCHAR(70),
   Prenom VARCHAR(50),
   PRIMARY KEY(IDTiers, Email)
);

CREATE TABLE Client(
   IDClient INT,
   PRIMARY KEY(IDClient),
   FOREIGN KEY(IDClient) REFERENCES Tiers(IDTiers) ON DELETE CASCADE
);

CREATE TABLE Cuisinier(
   IDCuisinier INT,
   PRIMARY KEY(IDCuisinier),
   FOREIGN KEY(IDCuisinier) REFERENCES Tiers(IDTiers) ON DELETE CASCADE
);

CREATE TABLE Plat (
	IDPlat INT,
	NomPlat VARCHAR(50),
    TypePlat VARCHAR(50),
    Ingredients VARCHAR(50),
    Nationalite VARCHAR(50),
	Regime VARCHAR(50),
    PRIMARY KEY (IDPlat) 
);

CREATE TABLE PlatCuisinier(
   IDPlatCuisinier INT,
   IDPlat INT,
   DateFabrication DATE,
   DatePeremption DATE,
   IngredientsSupp VARCHAR(50),
   PrixPlat DECIMAL(15,2),
   NombrePersonnes INT,
   IDCuisinier INT,
   PRIMARY KEY(IDPlatCuisinier),
   FOREIGN KEY(IDPlat) REFERENCES Plat(IDPlat) ON DELETE CASCADE,
   FOREIGN KEY(IDCuisinier) REFERENCES Cuisinier(IDCuisinier) ON DELETE CASCADE
);

CREATE TABLE Commande(
   IDCommande INT,
   DateCommande DATE,
   HeureCommande TIME,
   IDClient INT NOT NULL,
   IDCuisinier INT NOT NULL,
   PRIMARY KEY(IDCommande),
   FOREIGN KEY(IDClient) REFERENCES Client(IDClient) ON DELETE CASCADE,
   FOREIGN KEY(IDCuisinier) REFERENCES Cuisinier(IDCuisinier) ON DELETE CASCADE
);

CREATE TABLE PlatCommande(
	IDCommande INT, 
    IDPlatCuisinier INT,
    PRIMARY KEY (IDCommande, IDPlatCuisinier), /* un plat ne peut être affecté qu'à au plus une commande */
    FOREIGN KEY (IDCommande) REFERENCES Commande(IDCommande) ON DELETE CASCADE, 
    FOREIGN KEY (IDPlatCuisinier) REFERENCES Plat(IDPlat) ON DELETE CASCADE
);

CREATE TABLE Entreprise(
   NomEntreprise VARCHAR(50),
   IDTiers INT NOT NULL,
   PRIMARY KEY(NomEntreprise),
   FOREIGN KEY(IDTiers) REFERENCES Client(IDClient) ON DELETE CASCADE
);

CREATE TABLE cuisine(
   IDTiers INT,
   IDPlat INT,
   PRIMARY KEY(IDTiers, IDPlat),
   FOREIGN KEY(IDTiers) REFERENCES Cuisinier(IDCuisinier) ON DELETE CASCADE,
   FOREIGN KEY(IDPlat) REFERENCES Plat(IDPlat) ON DELETE CASCADE
);

CREATE TABLE sert(
   IDTiers INT,
   IDTiers_1 INT,
   Note INT,
   PRIMARY KEY(IDTiers, IDTiers_1),
   FOREIGN KEY(IDTiers) REFERENCES Client(IDClient) ON DELETE CASCADE,
   FOREIGN KEY(IDTiers_1) REFERENCES Cuisinier(IDCuisinier) ON DELETE CASCADE
);
