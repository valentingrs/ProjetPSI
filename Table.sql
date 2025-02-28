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
   IDTiers INT,
   PRIMARY KEY(IDTiers),
   FOREIGN KEY(IDTiers) REFERENCES Tiers(IDTiers)
);

CREATE TABLE Plat(
   IDPlat INT,
   TypePlat VARCHAR(50),
   DateFabrication DATE,
   DatePeremption DATE,
   Nationalité VARCHAR(50),
   Régime VARCHAR(50),
   Ingrédients VARCHAR(50),
   PrixPlat DECIMAL(15,2),
   NombrePersonnes INT,
   PRIMARY KEY(IDPlat)
);

CREATE TABLE Commande(
   IDCommande INT,
   DateCommande DATE,
   HeureCommande TIME,
   IDTiers INT NOT NULL,
   PRIMARY KEY(IDCommande),
   FOREIGN KEY(IDTiers) REFERENCES Client(IDTiers)
);

CREATE TABLE Entreprise(
   NomEntreprise VARCHAR(50),
   IDTiers INT NOT NULL,
   PRIMARY KEY(NomEntreprise),
   FOREIGN KEY(IDTiers) REFERENCES Client(IDTiers)
);

CREATE TABLE Cusinier(
   IDTiers INT,
   IDPlat INT NOT NULL,
   PRIMARY KEY(IDTiers),
   FOREIGN KEY(IDTiers) REFERENCES Tiers(IDTiers),
   FOREIGN KEY(IDPlat) REFERENCES Plat(IDPlat)
);

CREATE TABLE Particulier(
   IDTiers INT,
   IDTiers_1 INT,
   PRIMARY KEY(IDTiers, IDTiers_1),
   FOREIGN KEY(IDTiers) REFERENCES Client(IDTiers),
   FOREIGN KEY(IDTiers_1) REFERENCES Cusinier(IDTiers)
);

CREATE TABLE cuisine(
   IDTiers INT,
   IDPlat INT,
   PRIMARY KEY(IDTiers, IDPlat),
   FOREIGN KEY(IDTiers) REFERENCES Cusinier(IDTiers),
   FOREIGN KEY(IDPlat) REFERENCES Plat(IDPlat)
);

CREATE TABLE sert(
   IDTiers INT,
   IDTiers_1 INT,
   Note INT,
   PRIMARY KEY(IDTiers, IDTiers_1),
   FOREIGN KEY(IDTiers) REFERENCES Client(IDTiers),
   FOREIGN KEY(IDTiers_1) REFERENCES Cusinier(IDTiers)
);

CREATE TABLE composé(
   IDPlat INT,
   IDCommande INT,
   PRIMARY KEY(IDPlat, IDCommande),
   FOREIGN KEY(IDPlat) REFERENCES Plat(IDPlat),
   FOREIGN KEY(IDCommande) REFERENCES Commande(IDCommande)
);
