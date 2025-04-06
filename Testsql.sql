USE LivinParis;
SHOW TABLES;
SET SQL_SAFE_UPDATES=0;

-- 1. Création des Tiers
INSERT INTO Tiers (IDTiers, CodePostal, Ville, Email, Tel, Nom, Adresse, Prenom) VALUES
(1, '75001', 'Paris', 'client1@email.com', '0123456789', 'Dupont', '10 rue de Paris', 'Jean'),
(2, '75002', 'Paris', 'client2@email.com', '0123456790', 'Martin', '15 avenue des Champs', 'Marie'),
(3, '75003', 'Paris', 'cuisinier1@email.com', '0123456791', 'Lemoine', '20 rue de la République', 'Pierre'),
(4, '75004', 'Paris', 'cuisinier2@email.com', '0123456792', 'Durand', '25 boulevard Saint-Germain', 'Lucie'),
(5, '75005', 'Paris', 'client_cuisinier1@email.com', '0123456793', 'Benoit', '30 rue de l''Opéra', 'Antoine'),
(6, '75006', 'Paris', 'client_cuisinier2@email.com', '0123456794', 'Berger', '35 rue du Faubourg', 'Sophie');

-- 2. Création des Clients (Client est une référence à Tiers)
INSERT INTO Client (IDClient) VALUES
(1),  -- Client 1
(2),  -- Client 2
(5),  -- Client 3 qui est aussi Cuisinier
(6);  -- Client 4 qui est aussi Cuisinier

-- 3. Création des Cuisiniers (Cuisinier est une référence à Tiers)
INSERT INTO Cuisinier (IDCuisinier) VALUES
(3),  -- Cuisinier 1
(4),  -- Cuisinier 2
(5),  -- Cuisinier 3 qui est aussi Client
(6);  -- Cuisinier 4 qui est aussi Client

-- 4. Création des Plats
INSERT INTO Plat (IDPlat, TypePlat, NomPlat, DateFabrication, DatePeremption, Nationalite, Regime, Ingredients, PrixPlat, NombrePersonnes, IDCuisinier) VALUES
(1, 'Entrée', 'Salade César', '2025-04-01', '2025-04-10', 'Française', 'Végétarien', 'Laitue, Parmesan, Croutons, Poulet', 12.50, 4, 3),  -- Cuisinier 1
(2, 'Plat principal', 'Ratatouille', '2025-04-02', '2025-04-15', 'Française', 'Végétarien', 'Courgettes, Aubergines, Tomates, Poivrons', 18.00, 4, 3),  -- Cuisinier 2
(3, 'Dessert', 'Tarte Tatin', '2025-04-03', '2025-04-20', 'Française', 'Sucré', 'Pommes, Beurre, Pâte feuilletée', 9.00, 6, 5),  -- Cuisinier 3
(4, 'Plat principal', 'Boeuf Bourguignon', '2025-04-04', '2025-04-18', 'Française', 'Carnivore', 'Boeuf, Carottes, Vin rouge, Oignons', 22.00, 6, 6),  -- Cuisinier 4
(5, 'Entrée', 'Soupe à l\'oignon', '2025-04-05', '2025-04-12', 'Française', 'Végétarien', 'Oignons, Bouillon, Fromage râpé, Pain', 10.00, 4, 3);  -- Cuisinier 1

